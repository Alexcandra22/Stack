using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomazer : MonoBehaviour
{
    [HideInInspector] public float stackColorRandom;
    [HideInInspector] public float backgroundColorRandom;

    public Material skyBoxMaterial;

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
    }

    public void SetColor(GameObject GO)
    {
        GO.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((stackColorRandom / 100f) % 1f, 1f, 1f));
        stackColorRandom++;
    }

    public void SetBackgroundColor()
    {
        backgroundColorRandom = Random.Range(0f, 255f);
        skyBoxMaterial.SetColor("_Color1", Color.HSVToRGB((backgroundColorRandom / 100f) % 1f, 1f, 1f));
        backgroundColorRandom = Random.Range(0f, 255f);
        skyBoxMaterial.SetColor("_Color2", Color.HSVToRGB((backgroundColorRandom / 100f) % 1f, 1f, 1f));
    }
}
