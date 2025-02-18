using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject buttons;
    public TMP_Text title;
    public GameObject cutScene;
    public GameObject menuCam;
    public void Play()
    {
        menuCam.SetActive(false);
        buttons.SetActive(false);
        title.gameObject.SetActive(false);
        cutScene.SetActive(true);
        Invoke(nameof(StartGame), 13f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
