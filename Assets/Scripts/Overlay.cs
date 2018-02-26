using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class Overlay : UIBehaviour
{
    public GameObject[] reInitialize;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        foreach(GameObject obj in reInitialize)
        {
            obj.GetComponent<LightViewCamera>().Awake();
        }
    }
}
