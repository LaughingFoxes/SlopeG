Shader "Custom/SurfaceShader" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_LightMap("LightMap", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull Off
		CGPROGRAM

		#pragma surface surf Lambert

		struct Input {
			float2 uv_MainTex;
			float2 uv_LightMap;
			float3 worldPos;
		};

		sampler2D _MainTex;
		sampler2D _LightMap;

		void surf(Input IN, inout SurfaceOutput o) {
			//clip(frac((IN.worldPos.y + IN.worldPos.z*0.1) * 5) - 0.5);
			o.Albedo = tex2D(_LightMap, IN.uv_LightMap).rgb;
			o.Albedo *= 0.1;
			o.Albedo = floor(o.Albedo / 0);
			//o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}
	FallBack "Diffuse"
}
