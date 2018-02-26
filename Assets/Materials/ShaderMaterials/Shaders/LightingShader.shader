
Shader "Sunpy/LightingShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightTex("Light Mask", 2D) = "red" {}
		_HourOfTheDay("Hour of the day", Range(0.0, 24.0)) = 0.0
		_StarSize("Star Size", Range(0.01, 2.0)) = 1.0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#define HALF_PI 1.5707963267948966
            #define TIME_MOD 0.2617993877991494

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex, _LightTex;
			float4 _MainTex_ST, _LightTex_ST;
			float _HourOfTheDay;
			float _StarSize;
			
			float Noise2d(in float2 x)
			{
				float xhash = cos(x.x * 37.0);
				float yhash = cos(x.y * 57.0);
				return frac(415.92653 * (xhash + yhash));
			}

			float NoisyStarField(in float2 vSamplePos, float fThreshhold)
			{
				float StarVal = Noise2d(vSamplePos);
				if (StarVal >= fThreshhold)
					StarVal = pow((StarVal - fThreshhold) / (1.0 - fThreshhold), 6.0);
				else
					StarVal = 0.0;
				return StarVal;
			}

			float StableStarField(in float2 vSamplePos, float fThreshhold)
			{
				float fractX = frac(vSamplePos.x);
				float fractY = frac(vSamplePos.y);
				float2 floorSample = floor(vSamplePos);
				float v1 = NoisyStarField(floorSample, fThreshhold);
				float v2 = NoisyStarField(floorSample + float2(0.0, 1.0), fThreshhold);
				float v3 = NoisyStarField(floorSample + float2(1.0, 0.0), fThreshhold);
				float v4 = NoisyStarField(floorSample + float2(1.0, 1.0), fThreshhold);

				float StarVal = v1 * (1.0 - fractX) * (1.0 - fractY)
					+ v2 * (1.0 - fractX) * fractY
					+ v3 * fractX * (1.0 - fractY)
					+ v4 * fractX * fractY;
				return StarVal;
			}

			float3 getSky(float2 uv, float2 pos)
			{
				float atmosphere = sqrt(1.0 - uv.y);
				float3 skyColor = float3(0.2, 0.4, 0.8);

				float scatter = pow(pos.y, 1.0 / 15.0);
				scatter = 1.0 - clamp(scatter, 0.8, 1.0);

				float3 scatterColor = lerp(float3(1.0, 1.0, 1.0), float3(1.0, 0.3, 0.0) * 1.5, scatter);
				return lerp(skyColor, float3(scatterColor), atmosphere / 1.3);

			}

			float3 getSun(float2 uv, float2 pos) {
				float sun = 1.0 - distance((uv * _ScreenParams) / _ScreenParams.yy, pos + float2(0.8, 0.0));
				sun = clamp(sun, 0.0, 1.0);

				float glow = sun;
				glow = clamp(glow, 0.0, 1.0);

				sun = pow(sun, 100.0);
				sun *= 100.0;
				sun = clamp(sun, 0.0, 1.0);

				glow = pow(glow, 6.0) * 1.0;
				glow = pow(glow, (uv.y));
				glow = clamp(glow, 0.0, 1.0);

				sun *= pow(dot(uv.y, uv.y), 1.0 / 1.65);

				glow *= pow(dot(uv.y, uv.y), 1.0 / 2.0);

				sun += glow;

				float3 sunColor = float3(1.0, 0.6, 0.05) * sun;

				return sunColor;
			}
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed2 uv				= i.uv;
				
				// Sample textures
				fixed4 col				= tex2D(_MainTex, uv);
				fixed4 light			= tex2D(_LightTex, uv);

				float dayScale			= _HourOfTheDay * TIME_MOD;
				float day				= (cos(dayScale) + 1.0) / 2.0;

				float2 sunCoord			= float2(cos(dayScale + HALF_PI), sin(dayScale + HALF_PI)) * -1.0;


				// Make background layers
				float3 dayLayer			= getSky(uv, sunCoord);
				float3 nightLayer		= float3(0.1, 0.2, 0.4) * uv.y;


				// Applies stars onto the night layer
				float starVal			= StableStarField(uv * _ScreenParams / _StarSize, 0.96);
				starVal				   *= sin(day * HALF_PI);
				nightLayer			   += lerp(fixed3(starVal, starVal, starVal), fixed3(0.0, 0.0, 0.0), col.a);


				// Shift betwin the 2 layers
				float3 backgroundLayer	= lerp(dayLayer, nightLayer, day) + getSun(uv, sunCoord);


				// Because I am lazy and got confused in the ordering of lerp I somehow inverted the day scale without knowing so this will fix it after I have used it
				day						= 1.0 - day;

				// Apply light layer
				col.rgb				   *= max(max(fixed3(day, day, day), fixed3(0.2, 0.2, 0.2)), light.rgb);
				// Apply background
				col.rgb					= backgroundLayer.rgb + col.rgb;

				return col;
			}
			ENDCG
		}
	}
}
