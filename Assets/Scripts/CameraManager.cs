using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetCameraPosition(GameObject current)
    {
        Camera.main.transform.position = new Vector3(0f, Spawner.Instance.currentCube.transform.position.y, 0f) + new Vector3(200f, 150f, 200f);
        Camera.main.transform.LookAt(current.transform.position);
    }
}
