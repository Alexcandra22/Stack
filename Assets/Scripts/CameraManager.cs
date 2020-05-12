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
        Camera.main.transform.position = new Vector3(gameObject.transform.position.x, Spawner.Instance.currentCube.transform.position.y + 180f, gameObject.transform.position.z);
    }

    public void LookAtStack()
    {
        Camera.main.transform.position = new Vector3(165f, 180f, 165f);
        Camera.main.transform.rotation = Quaternion.Euler(25f, -135f, 0f);
    }
}
