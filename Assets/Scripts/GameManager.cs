using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _player;

    public GameObject GetPlayerGameObject()
    {
        return _player;
    }

    public void SetPlayerActive(bool value)
    {
        _player.SetActive(value);
    }
}
