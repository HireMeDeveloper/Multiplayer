using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Window window;
    public Transform insideGoal;

    public bool isWindowBroken()
    {
        return window.isBroken;
    }
}
