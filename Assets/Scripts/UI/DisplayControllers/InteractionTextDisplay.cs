using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionTextDisplay : MonoBehaviour
{
    private UiRefrenceManager refrenceManager;

    private void Start()
    {
        var uiManager = GameObject.FindGameObjectWithTag("UiManager");
        if (uiManager == null) return;

        refrenceManager = uiManager.GetComponent<UiRefrenceManager>();
    }

    public void DisplayText(string text)
    {
        refrenceManager.showInteractionText(text);
    }

    public void Hide()
    {
        refrenceManager.hideInteractionText();
    }

    public void DisplayDynamicText()
    {
        var dynamicTextCreator = gameObject.GetComponent<IDynamicTextCreator>();
        if (dynamicTextCreator == null) return;

        DisplayText(dynamicTextCreator.GetTextFromCreator());
    }
}
