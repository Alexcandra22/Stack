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

    public void SetCameraPosition(GameObject current)
    {
        Camera.main.transform.position = new Vector3(0f, Spawner.Instance.currentCube.transform.position.y, 0f) + new Vector3(160f, 170f, 160f);
        Camera.main.transform.LookAt(current.transform.position);
    }

    public void LookAtStack()
    {
        Camera.main.transform.position = new Vector3(160f, 250f, 160f);
        Camera.main.transform.rotation = Quaternion.Euler(35f, -135f, 0f);
    }
}
