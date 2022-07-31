using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject interactable;
    public void spawnHere()
    {
        spawn(transform);
    }

    public virtual void spawn(Transform transform)
    {
        Instantiate(interactable, transform.position, Quaternion.identity);
    }
}
