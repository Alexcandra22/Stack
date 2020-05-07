using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text record;
    public TMP_Text scoreText;
    public TMP_Text scoreTextRunTime;
    public int score;

    private static ScoreManager instance;
    public static ScoreManager Instance { get { return instance; } }

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

    public void SetScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void SaveRecord()
    {
        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("Score", score);
            record.text = "New record: " + score + " !";
        }
    }

    public void SetRecord()
    {
        record.text = "Record: " + PlayerPrefs.GetInt("Score", score);
    }

    public void ScoreUp()
    {
        score++;
        scoreTextRunTime.text = score.ToString();
    }

    public void ScoreTextEnable()
    {
        scoreText.gameObject.SetActive(true);
    }

    public void ScoreTextDisable()
    {
        scoreText.gameObject.SetActive(false);
    }

    public void ScoreTextRunTimeEnable()
    {
        scoreTextRunTime.gameObject.SetActive(true);
    }

    public void ScoreTextRunTimeDisable()
    {
        scoreTextRunTime.gameObject.SetActive(false);
    }

    public void RecordEnable()
    {
        record.gameObject.SetActive(true);
    }

    public void RecordDisable()
    {
        record.gameObject.SetActive(false);
    }

    public void GameOver(GameObject currentCube)
    {
        ScoreTextEnable();
        ScoreTextRunTimeDisable();
        SetScore();
        SaveRecord();
        RecordEnable();

        MainManager.Instance.tapToStartText.gameObject.SetActive(true);
        MainManager.Instance.gameOverText.gameObject.SetActive(true);
        return;
    }
}
