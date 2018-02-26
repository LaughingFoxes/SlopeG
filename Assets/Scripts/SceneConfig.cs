using Boo.Lang;
using UnityEngine;

[ExecuteInEditMode]
public class SceneConfig : MonoBehaviour {
    [Range(0, 24)]
    public float hourOfTheDay = 0f;
    public float timeElapseModifier = 0.1f;

    //public Material unlitShaderMaterial = null;
    public LightViewCamera lightCamera = null;
    public Material lightingShaderMaterial = null;

    Camera cam = null;

    //public RenderTexture lightSource = null;

    void Awake()
    {
        Config.hourOfTheDay = this.hourOfTheDay;
        cam = GetComponent<Camera>();

        //SetupLightSystem();
    }

    Texture2D RTImage(Camera cam) {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;
        return image;
    }

    void OnPreRender()
    {
        //Texture2D tmpTex = RTImage(lightCamera);
        //lightCamera.Render();
        //lightingShaderMaterial.EnableKeyword("_LightTex");
        //lightingShaderMaterial.SetTexture("_LightTex", lightCamera.targetTexture);
    }

    void Update()
    {
        hourOfTheDay = (hourOfTheDay + Time.deltaTime * timeElapseModifier) % 24f;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        /*if (this.hourOfTheDay)
        {
            Graphics.Blit(source, destination);
            return;
        }*/

        lightingShaderMaterial.SetTexture("_LightTex", lightCamera.lastRender);
        lightingShaderMaterial.SetFloat("_HourOfTheDay", hourOfTheDay);

        Graphics.Blit(source, destination, lightingShaderMaterial);
    }
}

public static class Config
{
    public static float hourOfTheDay = 0f;
}
