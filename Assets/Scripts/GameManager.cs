using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsCapturing { get; set; }
    public GameObject _player;
    public GameObject _pokeball;
    public AudioClip mainTheme;

    private AudioSource _audioSource;

    public void Start()
    {
        FindObjectOfType<ScoreManager>().StartGame();
        _audioSource = GetComponent<AudioSource>();

        this.RestoreMainThemeVolume();
    }

    public GameObject GetPlayerGameObject()
    {
        return _player;
    }

    public void SetPlayerActive(bool value)
    {
        _player.SetActive(value);
    }

    public void StartCapture(int damage)
    {
        _pokeball.GetComponent<Pokeball>().StartCapture(damage);
    }

    public void ReduceMainThemeVolume()
    {
        _audioSource.volume = 0.1f;
    }

    public void RestoreMainThemeVolume()
    {
        _audioSource.volume = 0.3f;
    }

}