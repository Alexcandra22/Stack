using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomazer : MonoBehaviour
{
    public static float colorRandom;

    void Start()
    {
        colorRandom = Random.Range(0f, 255f);
    }

    public static void SetColor(GameObject GO)
    {
        GO.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((colorRandom / 100f) % 1f, 1f, 1f));
        colorRandom++;
    }
}
