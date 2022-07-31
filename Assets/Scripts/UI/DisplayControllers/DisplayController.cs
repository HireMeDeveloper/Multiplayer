using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayController : MonoBehaviour
{
    [SerializeField] private GameObject display;
    public void Hide()
    {
        display.SetActive(false);
    }

    public void Show()
    {
        display.SetActive(true);
    }
}
