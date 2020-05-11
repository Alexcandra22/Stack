using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomazer : MonoBehaviour
{
    [HideInInspector] public float colorRandom;

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

        colorRandom = Random.Range(0f, 255f);
    }

    void Start()
    {
    }

    public void SetColor(GameObject GO)
    {
        GO.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((colorRandom / 100f) % 1f, 1f, 1f));
        colorRandom++;
    }
}
