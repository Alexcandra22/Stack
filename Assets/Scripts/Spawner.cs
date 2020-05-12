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
    Vector3 previousScale;
    Vector3 previousPosition;
    GameObject cutCube;
    float intScaleX;
    float intScaleZ;
    float intPositionX;
    float intPositionZ;
    float margin = 0.1f;

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
        sides.Add("z");

        ColorRandomazer.Instance.SetColor(currentCube);
    }

    private void NewCube()
    {
        SetCorrectPrefabName();

        if (lastCube != null)
        {
            previousScale = currentCube.transform.localScale;
            previousPosition = currentCube.transform.position;

            currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x), currentCube.transform.position.y, Mathf.Round(currentCube.transform.position.z));
            currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x - Mathf.Abs(currentCube.transform.position.x - lastCube.transform.position.x), 
                                                           lastCube.transform.localScale.y, 
                                                           lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z - lastCube.transform.position.z));

            CreateCutPlatform();

            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, lastCube.transform.position, 0.5f) + Vector3.up * 5f;

            if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f)
            {
                ScoreManager.Instance.GameOver(currentCube);
                CameraManager.Instance.LookAtStack();
                Destroy(currentCube);
                Destroy(cutCube);
                return;
            }
        }

        lastCube = currentCube;
        currentCube = Instantiate(currentCube, MainManager.Instance.stack.transform);
        SetSideSpawn();
        ColorRandomazer.Instance.SetColor(currentCube);
        ScoreManager.Instance.ScoreUp();
        CameraManager.Instance.SetCameraPosition(currentCube);
    }

    private void SetSideSpawn()
    {
        if (side == "x" || side == null)
            side = sides[1];
        else
            side = sides[0];
    }

    private void FixedUpdate()
    {
        if(cutCube != null)
        cutCube.GetComponent<Rigidbody>().velocity = cutCube.GetComponent<Rigidbody>().velocity.normalized * 100f;
    }

    private void CreateCutPlatform()
    {
        cutCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cutCube.GetComponent<MeshRenderer>().material.color = currentCube.GetComponent<MeshRenderer>().material.color;
        cutCube.AddComponent<Rigidbody>();
        CheckCutCubePosition();
        CheckCutCubeScale();
        Destroy(cutCube.gameObject, 5f);
    }

    private void CheckCutCubeScale()
    {
        if (!MainManager.Instance.movingX)
            intScaleX = currentCube.transform.localScale.x;
        else
            intScaleX = Mathf.Abs(previousScale.x - currentCube.transform.localScale.x);

        if (!MainManager.Instance.movingZ)
            intScaleZ = currentCube.transform.localScale.z;
        else
            intScaleZ = Mathf.Abs(previousScale.z - currentCube.transform.localScale.z);

        cutCube.transform.localScale = new Vector3(intScaleX, currentCube.transform.localScale.y, intScaleZ);
    }

    private void CheckCutCubePosition()
    {
        if (!MainManager.Instance.movingX)
        {
            float delta = lastCube.transform.position.z - currentCube.transform.position.z;

            if (Mathf.Abs(delta) > margin)
                cutCube.transform.position = new Vector3(currentCube.transform.position.x,
                                                         currentCube.transform.position.y,
                                                        (currentCube.transform.position.z > 0) 
                                                        ?currentCube.transform.position.z + (currentCube.transform.localScale.z / 2)
                                                        :currentCube.transform.position.z - (currentCube.transform.localScale.z / 2));
        }
        else
        {
            float delta = lastCube.transform.position.x - currentCube.transform.position.x;

            if (Mathf.Abs(delta) > margin)
                cutCube.transform.position = new Vector3((currentCube.transform.position.x > 0)
                                                         ?currentCube.transform.position.x + (currentCube.transform.localScale.x / 2)
                                                         :currentCube.transform.position.x - (currentCube.transform.localScale.x / 2),
                                                          currentCube.transform.position.y,
                                                          currentCube.transform.position.z);
        }
    }

    private void SetCorrectPrefabName()
    {
        currentCube.name = currentCube.name.Replace("(Clone)", "");
    }
}
