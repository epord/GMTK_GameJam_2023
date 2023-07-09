using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float Score { get; set; }
    private float _startTime;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame()
    {
        this.Score = 0.0f;
        this._startTime = Time.time;
    }

    public void EndGame()
    {
        this.Score = Time.time - this._startTime;
    }
}
