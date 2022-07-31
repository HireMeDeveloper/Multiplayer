using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DropManager : MonoBehaviour
{
    [SerializeField] private int minKillsPerDrop = 15;
    [SerializeField] private int maxKillsPerDrop = 45;
    private int drops = 0;
    private int remainingKills;
    [SerializeField] private List<Enterable> powerups = new List<Enterable>();
    private List<Enterable> powerupBag = new List<Enterable>();

    private void Start()
    {
        resetKills();
    }
    public void enemyKilled(Vector2 position)
    {
        if(!PhotonNetwork.IsMasterClient) return;

        remainingKills--;
        if (remainingKills > 0) return;

        dropNextDrop(position);
    }

    private void fillBag()
    {
        foreach (var powerup in powerups)
        {
            powerupBag.Add(powerup);
        }
    }

    private void resetKills()
    {
        remainingKills = Random.Range(minKillsPerDrop + drops, maxKillsPerDrop + drops);
    }

    private void dropNextDrop(Vector2 position)
    {
        if (powerupBag.Count == 0) fillBag();
        var rand = Random.Range(0, powerupBag.Count);

        PhotonNetwork.Instantiate(powerupBag[rand].name, new Vector3(position.x, position.y, 1f), Quaternion.identity);
        powerupBag.Remove(powerupBag[rand]);
        drops++;
        resetKills();
    }
}
