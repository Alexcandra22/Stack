using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject currentCube;
    [NonSerialized]
    public GameObject lastCube;
    [NonSerialized]
    public string side;
    private List<string> sides = new List<string>();

    private static Spawner instance;
    public static Spawner Instance { get { return instance; } }

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
    }

    private void Start()
    {
        MainManager.Instance.NewPlatformEvent += NewCube;

        sides.Add("x");
        sides.Add("-x");
        sides.Add("z");
        sides.Add("-z");

        ColorRandomazer.SetColor();
    }

    private void NewCube()
    {
        SetCorrectPrefabName();

        if (lastCube != null)
        {
            currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x), currentCube.transform.position.y, Mathf.Round(currentCube.transform.position.z));
            currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x - Mathf.Abs(currentCube.transform.position.x - lastCube.transform.position.x), lastCube.transform.localScale.y, lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z - lastCube.transform.position.z));
            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, lastCube.transform.position, 0.5f) + Vector3.up * 5f;
             
            if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f)
            {
                MainManager.Instance.GameOver(currentCube);
                CameraManager.Instance.SetCameraPosition(MainManager.Instance.stack);
                Destroy(currentCube);
                return;
            }
        }

        lastCube = currentCube;
        currentCube = Instantiate(currentCube);
        side = sides[ColorRandomazer.random.Next(sides.Count)];
        ColorRandomazer.SetColor();
        ScoreManager.Instance.ScoreUp();
        CameraManager.Instance.SetCameraPosition(currentCube);
    }

    private void SetCorrectPrefabName()
    {
        currentCube.name = currentCube.name.Replace("(Clone)", "");
    }
}
