using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource stopStackSource;
    [SerializeField] private AudioSource startGameSource;

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

    public void StopStackOn()
    {
        stopStackSource.Play();
    }
}
