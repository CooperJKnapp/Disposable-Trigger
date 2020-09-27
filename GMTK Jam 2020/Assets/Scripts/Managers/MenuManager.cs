using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField]
    private GameObject buttonPressSFX;
    [SerializeField]
    private GameObject titleSFX;

    void Start() {
        Instantiate(titleSFX, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Begins the game
    /// </summary>
    public void BeginGame() {
        Instantiate(buttonPressSFX, transform.position, Quaternion.identity);
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Closes out the game
    /// </summary>
    public void QuitGame() {
        Instantiate(buttonPressSFX, transform.position, Quaternion.identity);
        Application.Quit();
    }
}
