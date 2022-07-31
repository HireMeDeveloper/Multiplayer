using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    public UnityEvent onUseItem; 

    public UnityEvent onDodgeRoll; 

    public UnityEvent onHit; 

    public UnityEvent onPurchase;

    private void Awake()
    {
        var cursorManager = CursorManager.instance;
        onDodgeRoll.AddListener(() => cursorManager.dodge());
    }
}
