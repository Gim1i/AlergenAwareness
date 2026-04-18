using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject homeUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private TMP_Text test;

    private void Start()
    {
        if (PlayerPrefs.GetInt("existingGame") != 0) { //If theres a game saved
            homeUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(true); //Enable the continue button
        } else {
            homeUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(false); //Disable if no saved game
        }
    }

    public void Continue() { //Pressing the continue button
        //SceneManager.LoadScene("Gameplay");
    }

    public void New() { //Pressing the new button
        test.text = "NEW";
        //SceneManager.LoadScene("Gameplay");
    }

    public void Settings() { //Pressing the settings button
        
    }

    public void Exit() { //Pressing the exit button
        Application.Quit();
    }
}
