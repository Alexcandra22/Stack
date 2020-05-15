using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [NonSerialized] public GameObject lastCube;
    [NonSerialized] public GameObject cutCube;
    [NonSerialized] public Vector3 previousScale;
    [NonSerialized] public Vector3 previousPosition;
    [NonSerialized] public string side;
    [NonSerialized] public bool movingCube = false;
    [NonSerialized] private bool posA;
    [NonSerialized] private bool posB;

    public GameObject currentCube;
    public float speed = 100f;

    Vector3 target;
    private List<string> sides = new List<string>();

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
        MainManager.Instance.NewPlatformEvent += CreateNewCube;

        sides.Add("x");
        sides.Add("z");
    }

    private void FixedUpdate()
    {
        if (cutCube != null)
            cutCube.GetComponent<Rigidbody>().velocity = cutCube.GetComponent<Rigidbody>().velocity.normalized * 100f;
    }

    private void SetSideSpawn()
    {
        if (side == "x" || side == null)
            side = sides[1];
        else
            side = sides[0];
    }

    public void MovingNewCube()
    {
        var time = Mathf.Abs(Time.realtimeSinceStartup  % 2f - 1f);
        var positionUp = lastCube.transform.position + Vector3.up * currentCube.transform.localScale.y;

        MainManager.Instance.movingX = false;
        MainManager.Instance.movingZ = false;

        switch (side)
        {
            case "x":
                if (currentCube.transform.position.x == 120f)
                    SetPosAFalse();

                if (currentCube.transform.position.x == -120f)
                    SetPosATrue();

                if (posA)
                {
                    target = new Vector3(120f, currentCube.transform.position.y, currentCube.transform.position.z);
                    currentCube.transform.position = Vector3.MoveTowards(currentCube.transform.position, target, Time.deltaTime * speed);
                }
                else
                {
                    target = new Vector3(-120f, currentCube.transform.position.y, currentCube.transform.position.z);
                    currentCube.transform.position = Vector3.MoveTowards(currentCube.transform.position, target, Time.deltaTime * speed);
                }

                MainManager.Instance.movingX = true;
                break;

            case "z":
                if (currentCube.transform.position.z == 120f)
                    SetPosAFalse();

                if (currentCube.transform.position.z == -120f)
                    SetPosATrue();

                if (posA)
                {
                    target = new Vector3(currentCube.transform.position.x, currentCube.transform.position.y, 120f);
                    currentCube.transform.position = Vector3.MoveTowards(currentCube.transform.position, target, Time.deltaTime * speed);
                }
                else
                {
                    target = new Vector3(currentCube.transform.position.x, currentCube.transform.position.y, -120f);
                    currentCube.transform.position = Vector3.MoveTowards(currentCube.transform.position, target, Time.deltaTime * speed);
                }

                MainManager.Instance.movingZ = true;
                break;
        }

        //switch (side)
        //{
        //    case "z":
        //        currentCube.transform.position = Vector3.Lerp(new Vector3(positionUp.x, positionUp.y, 120f), positionUp + (Vector3.forward) * -120, Mathf.PingPong(Time.time * 1f, 0.5f));
        //        MainManager.Instance.movingZ = true;
        //        break;
        //    case "x":
        //        currentCube.transform.position = Vector3.Lerp(new Vector3(-120f, positionUp.y, positionUp.z), positionUp + (Vector3.right) * 120, time);
        //        MainManager.Instance.movingX = true;
        //        break;
        //}
    }

    private void SetPosATrue()
    {
        posB = false;
        posA = true;
    }

    private void SetPosAFalse()
    {
        posB = true;
        posA = false;
    }

    private void CreateNewCube()
    {
        lastCube = currentCube;
        currentCube = Instantiate(currentCube, MainManager.Instance.stack.transform);
        SetSideSpawn();
        ColorRandomazer.Instance.SetColor(currentCube);
        ScoreManager.Instance.ScoreUp();
        CameraManager.Instance.SetCameraPosition(currentCube);
        movingCube = true;

        switch (side)
        {
            case "x":
                currentCube.transform.position = new Vector3(currentCube.transform.position.x - 120f, currentCube.transform.position.y + currentCube.transform.localScale.y, currentCube.transform.position.z);
                posA = true;
                posB = false;
                break;
            case "z":
                currentCube.transform.position = new Vector3(currentCube.transform.position.x, currentCube.transform.position.y + currentCube.transform.localScale.y, currentCube.transform.position.z - 120f);
                posA = true;
                posB = false;
                break;
        }
    }

    public void CreateCutPlatform()
    {
        cutCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        CheckCutCubePosition();
        CheckCutCubeScale();
        cutCube.GetComponent<MeshRenderer>().material.color = currentCube.GetComponent<MeshRenderer>().material.color;
        cutCube.AddComponent<Rigidbody>();
        cutCube.GetComponent<BoxCollider>().material = MainManager.Instance.cuCubeMaterial;
        var cutCubeRigibody = cutCube.GetComponent<Rigidbody>();
        cutCubeRigibody.constraints = RigidbodyConstraints.FreezeRotationY;
        Destroy(cutCube.gameObject, 10f);
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
}
