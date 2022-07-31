using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PersistantEffect
{
    public void enableEffect(PlayerInteraction playerInteraction);

    public void disableEffect(PlayerInteraction playerInteraction);
}
