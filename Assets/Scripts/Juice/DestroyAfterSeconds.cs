using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float delay = 5f;

    public UnityEvent onDestroy;

    private void Start()
    {
        Invoke("remove", delay);
    }

    private void remove()
    {
        onDestroy.Invoke();
        Destroy(this.gameObject);
    }
}
