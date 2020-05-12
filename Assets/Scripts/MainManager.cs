using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public TMP_Text tapToStartText;
    bool started = true;
    [HideInInspector] public bool movingX;
    [HideInInspector] public bool movingZ;

    public GameObject stack;

    public delegate void NewPlatformDelegate();
    public event NewPlatformDelegate NewPlatformEvent;

    private static MainManager instance;
    public static MainManager Instance { get { return instance; } }

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

    void Update()
    {
        if (ScoreManager.Instance.recordGO.activeSelf)
        {
            if (Input.GetButtonDown("Fire1"))
                RestartGame();
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            tapToStartText.gameObject.SetActive(false);

            if (!tapToStartText.gameObject.activeSelf)
            {
                NewPlatformEvent();
                if (started) //утечка ресурсов (возможно лучше создать новую сцену StartGame)
                    StartGame();
            }
        }

        if (!tapToStartText.gameObject.activeSelf)
        {
            MovingNewCube();
        }
    }

    private void StartGame()
    {
        tapToStartText.gameObject.SetActive(false);
        ScoreManager.Instance.ScoreTextEnable();
        ScoreManager.Instance.RecordDisable();
        started = false;
    }

    private void MovingNewCube()
    {
        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);
        var positionUp = Spawner.Instance.lastCube.transform.position + Vector3.up * Spawner.Instance.currentCube.transform.localScale.y;

        movingX = false;
        movingZ = false;

        switch (Spawner.Instance.side)
        {
            case "z":
                Spawner.Instance.currentCube.transform.position = Vector3.Lerp(new Vector3(positionUp.x, positionUp.y, 120f), positionUp + (Vector3.forward) * -120, time);
                movingZ = true;
                break;
            case "x":
                Spawner.Instance.currentCube.transform.position = Vector3.Lerp(new Vector3(-120f, positionUp.y, positionUp.z), positionUp + (Vector3.right) * 120, time);
                movingX = true;
                break;
        }
    }

    private void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
