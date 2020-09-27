using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScreenManager : MonoBehaviour
{
    // Object references
    [Header("Object References")]
    [SerializeField]
    private GameObject buttonPressSFX;
    [SerializeField]
    private Text scoreText;
    private DontDestroyData gameData;

    // Start is called before the first frame update
    void Start() {
        gameData = GameObject.Find("Data").GetComponent<DontDestroyData>();
        scoreText.text = Math.Round(gameData.SurvivalTime, 1) + " Seconds";
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    public void PlayAgain() {
        Instantiate(buttonPressSFX, transform.position, Quaternion.identity);
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Return to the main menu
    /// </summary>
    public void MainMenu() {
        Instantiate(buttonPressSFX, transform.position, Quaternion.identity);
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Quit() {
        Instantiate(buttonPressSFX, transform.position, Quaternion.identity);
        Application.Quit();
    }
}
