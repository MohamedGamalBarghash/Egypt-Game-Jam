using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private void Start() 
    { 
        //easy way to disable mouse during entire game
        //I wanted to make the menu traversable using keyboard only
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayGame()
    {
        //could also use SceneManager.LoadScene("PlayScene"); to start playing
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        // preprocessor directives to check whether
        // the game is running in unity editor or as a seperate app and quit in both cases
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
