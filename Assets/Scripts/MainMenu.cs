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

    // NOT USED.
    public void openOptions()
    {
        Debug.Log("cleeeean and smooth code readable for real");
    }

    // Quits the game when called
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
