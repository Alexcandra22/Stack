using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource stopStackSource;
    [SerializeField] private AudioSource startGameSource;
    [SerializeField] private AudioSource[] stackOnStackSource;

    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

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
    }

    public void StopStackOn()
    {
        stopStackSource.enabled = true;
        stopStackSource.Play();
    }

    public void StactOnStackOn(int i)
    {
        stackOnStackSource[i].enabled = true;
        stackOnStackSource[i].Play();
        MainManager.Instance.i++;
        if (MainManager.Instance.i >= 8)
            MainManager.Instance.i = 0;
    }

    public void StartGameOff()
    {
        startGameSource.Stop();
    }
}
