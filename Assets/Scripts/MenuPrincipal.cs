using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour {

    private Text texto;

	// Use this for initialization
	void Start () {
        PlayerPrefs.GetInt("HIGHSCORE", 0);
        texto = GameObject.Find("Score").GetComponent<Text>();
        texto.text = "Máximo numero de días: " + PlayerPrefs.GetInt("HIGHSCORE", 0).ToString();
    }

    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
