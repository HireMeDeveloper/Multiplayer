using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShowHideScoreboard : MonoBehaviour
{
    [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private GameObject survivedToRoundDisplay;

    [SerializeField] private KeyCode toggleButton = KeyCode.Tab;

    private bool canToggle = true;

    private void Update()
    {
        if (Input.GetKeyDown(toggleButton))
        {
            show();
        }

        if (Input.GetKeyUp(toggleButton))
        {
            hide();
        }
    }

    private void show()
    {
        if (!canToggle) return;

        scoreboard.SetActive(true);
        gameUi.SetActive(false);
    }

    private void hide()
    {
        if (!canToggle) return;

        scoreboard.SetActive(false);
        gameUi.SetActive(true);
    }

    public void showForEndScreen()
    {
        canToggle = false;
        show();
        survivedToRoundDisplay.SetActive(true);
    }
}
