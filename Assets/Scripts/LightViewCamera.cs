using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightViewCamera : MonoBehaviour {
    public RenderTexture lastRender;

    public void Awake()
    {
        Camera cam = transform.parent.GetComponent<Camera>();
        lastRender = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 1);
        GetComponent<Camera>().targetTexture = lastRender;
    }
}
