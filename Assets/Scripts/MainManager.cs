using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [HideInInspector] public bool movingX;
    [HideInInspector] public bool movingZ;
    [HideInInspector] public bool gameOver = false;

    public TMP_Text tapToStartText;
    public TMP_Text stackText;

    public GameObject stack;
    public PhysicMaterial cuCubeMaterial;

    bool started = true;

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

    void Start()
    {
        ColorRandomazer.Instance.SetColor(Spawner.Instance.currentCube);
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
                PositioningCurrentCube(Spawner.Instance.currentCube, Spawner.Instance.lastCube);

                if (!gameOver)
                    NewPlatformEvent();

                SetCorrectPrefabName(Spawner.Instance.currentCube);

                if (started) //утечка ресурсов (возможно лучше создать новую сцену StartGame)
                    StartGame();
            }
        }

        if (Spawner.Instance.movingCube)
             Spawner.Instance.MovingNewCube();
    }

    private void StartGame()
    {
        tapToStartText.gameObject.SetActive(false);
        stackText.gameObject.SetActive(false);
        ScoreManager.Instance.ScoreTextEnable();
        ScoreManager.Instance.RecordDisable();
        AudioManager.Instance.StartGameOff();
        started = false;
    }

    private void SetCorrectPrefabName(GameObject currentCube)
    {
        currentCube.name = currentCube.name.Replace("(Clone)", "");
    }

    private void PositioningCurrentCube(GameObject currentCube, GameObject lastCube)
    {
        if (lastCube != null)
        {
            Spawner.Instance.previousScale = currentCube.transform.localScale;
            Spawner.Instance.previousPosition = currentCube.transform.position;

            currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x), currentCube.transform.position.y, Mathf.Round(currentCube.transform.position.z));
            currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x - Mathf.Abs(currentCube.transform.position.x - lastCube.transform.position.x),
                                                           lastCube.transform.localScale.y,
                                                           lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z - lastCube.transform.position.z));
            AudioManager.Instance.StopStackOn();
            Spawner.Instance.CreateCutPlatform();
            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, lastCube.transform.position, 0.5f) + Vector3.up * 5f;

            CheckCurrentCubeLocation(Spawner.Instance.currentCube);
        }
    }

    private void CheckCurrentCubeLocation(GameObject currentCube)
    {
        if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f)
        {
            GameOver(currentCube);
            CameraManager.Instance.LookAtStack();
            Destroy(currentCube);
            Destroy(Spawner.Instance.cutCube);
            gameOver = true;
        }
    }

    private void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GameOver(GameObject currentCube)
    {
        ScoreManager.Instance.ScoreTextEnable();
        ScoreManager.Instance.SetScore();
        ScoreManager.Instance.SetRecord();
        ScoreManager.Instance.SaveRecord();
        Spawner.Instance.movingCube = false;

        tapToStartText.gameObject.SetActive(true);
        return;
    }
}
