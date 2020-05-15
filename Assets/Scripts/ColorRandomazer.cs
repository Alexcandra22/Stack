using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomazer : MonoBehaviour
{
    [HideInInspector] public float stackColorRandom;
    [HideInInspector] public float backgroundColorRandomUp;
    [HideInInspector] public float backgroundColorRandomDown;

    public Material skyBoxMaterial;
    public Material bottomPlatformMaterial;

    private static ColorRandomazer instance;
    public static ColorRandomazer Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        stackColorRandom = Random.Range(0f, 255f);
        SetBackgroundColor();
        SetBottomplatformColor();
    }

    public void SetColor(GameObject GO)
    {
        GO.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((stackColorRandom / 100f) % 1f, 1f, 1f));
        stackColorRandom++;
    }

    public void SetBackgroundColor()
    {
        backgroundColorRandomUp = Random.Range(0f, 255f);
        skyBoxMaterial.SetColor("_Color1", Color.HSVToRGB((backgroundColorRandomUp / 100f) % 1f, 1f, 1f));

        backgroundColorRandomDown = Random.Range(0f, 255f);
        skyBoxMaterial.SetColor("_Color2", Color.HSVToRGB((backgroundColorRandomDown / 100f) % 1f, 1f, 1f));
    }

    public void SetBottomplatformColor()
    {
        bottomPlatformMaterial.SetColor("_ColorMid", Color.HSVToRGB((stackColorRandom / 100f) % 1f, 1f, 0.7f));
        bottomPlatformMaterial.SetColor("_ColorBot", Color.HSVToRGB((backgroundColorRandomUp / 100f) % 1f, 0.7f, 1f));
    }
}
