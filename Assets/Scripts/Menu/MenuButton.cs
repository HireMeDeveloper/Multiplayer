using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private SettingsData settingsData;
    public void connectToSoloGame()
    {
        settingsData.isOnline = false;
        SceneManager.LoadScene("ConnectToServer");
    }

    public void connectToOnlineGame()
    {
        settingsData.isOnline = true;
        SceneManager.LoadScene("ConnectToServer");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
