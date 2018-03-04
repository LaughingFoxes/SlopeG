using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {

    // Loads the next scene when called
	public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Open the Options-menu (currently non-existent)
    public void openOptions()
    {
        Debug.Log("sonpi pls help me code here");
    }

    // Quits the game when called
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
