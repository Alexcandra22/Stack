using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector] public int score;

    public TMP_Text recordText;
    public TMP_Text scoreText;

    public GameObject recordGO;

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

    public void SetScore()
    {
        scoreText.text = score.ToString();
    }

    public void SaveRecord()
    {
        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("Score", score);
            recordText.text = "NEW RECORD";
            RecordDisable();
        }
        else
            RecordEnable();
    }

    public void SetRecord()
    {
        recordText.text = PlayerPrefs.GetInt("Score", score).ToString();
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void ScoreTextEnable()
    {
        scoreText.gameObject.SetActive(true);
    }

    public void ScoreTextDisable()
    {
        scoreText.gameObject.SetActive(false);
    }

    public void RecordEnable()
    {
        recordGO.SetActive(true);
    }

    public void RecordDisable()
    {
        recordGO.SetActive(false);
    }
}
