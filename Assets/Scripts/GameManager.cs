using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _player;
    public GameObject _pokeball;
    public bool IsCapturing { get; set; }

    public void Start()
    {
        FindObjectOfType<ScoreManager>().StartGame();
    }

    public GameObject GetPlayerGameObject()
    {
        return _player;
    }

    public void SetPlayerActive(bool value)
    {
        _player.SetActive(value);
    }

    public void StartCapture()
    {
        _pokeball.GetComponent<Pokeball>().StartCapture();
    }
}
