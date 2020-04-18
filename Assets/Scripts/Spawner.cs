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
    private System.Random random = new System.Random();
    float colorRandom;

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
        colorRandom = UnityEngine.Random.Range(0f, 255f);

        sides.Add("x");
        sides.Add("-x");
        sides.Add("z");
        sides.Add("-z");

        SetColor();
    }

    public void SetCameraPosition(GameObject current)
    {
        Camera.main.transform.position = new Vector3(0f, currentCube.transform.position.y, 0f) + new Vector3(200f, 150f, 200f);
        Camera.main.transform.LookAt(current.transform.position);
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
                SetCameraPosition(MainManager.Instance.stack);
                Destroy(currentCube);
                return;
            }
        }

        lastCube = currentCube;
        currentCube = Instantiate(currentCube);
        side = sides[random.Next(sides.Count)];
        SetColor();
        MainManager.Instance.score++;
        MainManager.Instance.scoreTextRunTime.text = MainManager.Instance.score.ToString();
        SetCameraPosition(currentCube);
    }

    private void SetColor()
    {
        currentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((colorRandom / 100f) % 1f, 1f, 1f));
        colorRandom++;
    }

    private void SetCorrectPrefabName()
    {
        currentCube.name = currentCube.name.Replace("(Clone)", "");
    }
}
