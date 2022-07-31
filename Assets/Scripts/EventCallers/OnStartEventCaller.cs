using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStartEventCaller : MonoBehaviour
{
    public UnityEvent onAwake;
    public UnityEvent onStart;

    private void Awake()
    {
        onAwake.Invoke();
    }

    private void Start()
    {
        onStart.Invoke();
    }
}
