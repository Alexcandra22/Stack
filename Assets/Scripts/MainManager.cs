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
    public TMP_Text gameOverText;
    bool started = true;

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

        CameraManager.Instance.SetCameraPosition(Spawner.Instance.currentCube);
        ScoreManager.Instance.SetRecord(); 
    }

    void Update()
    {
        if (gameOverText.gameObject.activeSelf)
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
        ScoreManager.Instance.ScoreTextRunTimeEnable();
        ScoreManager.Instance.RecordDisable();
        started = false;
    }

    private void MovingNewCube()
    {
        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);

        var positionUp = Spawner.Instance.lastCube.transform.position + Vector3.up * 10f;

        var position1 = positionUp + (Vector3.left) * 120;
        var position2 = positionUp + (Vector3.right) * 120;
        var position3 = positionUp + (Vector3.forward) * -120;
        var position4 = positionUp + (Vector3.back) * -120;

        switch (Spawner.Instance.side)
        {
            case "x":
                Spawner.Instance.currentCube.transform.position = Vector3.Lerp(position1, positionUp, time);
                break;
            case "z":
                Spawner.Instance.currentCube.transform.position = Vector3.Lerp(positionUp, position2, time);
                Spawner.Instance.currentCube.transform.position = Vector3.Lerp(positionUp, position2, time);
                break;
            case "-x":
                Spawner.Instance.currentCube.transform.position = Vector3.Lerp(positionUp, position3, time);
                break;
            case "-z":
                Spawner.Instance.currentCube.transform.position = Vector3.Lerp(position4, positionUp, time);
                break;
        }
    }

    public void GameOver(GameObject currentCube)
    {
        ScoreManager.Instance.ScoreTextEnable();
        ScoreManager.Instance.ScoreTextRunTimeDisable();
        ScoreManager.Instance.SetScore();
        ScoreManager.Instance.SaveRecord();
        ScoreManager.Instance.RecordEnable();

        tapToStartText.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        return;
    }

    private void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
