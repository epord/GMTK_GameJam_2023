using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        gameOverText.text = $"You have been captured in {Mathf.RoundToInt(scoreManager.Score)} seconds.\nThere is no escape now\n\n\n<Space> to retry";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
