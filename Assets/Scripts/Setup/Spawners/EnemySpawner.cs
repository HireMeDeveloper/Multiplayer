using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPun
{
    [SerializeField] private int currentWave = 0;
    [SerializeField] private int remainingSpawns;
    [SerializeField] private int playerCount;
    [SerializeField] private int enemyCount;

    [SerializeField] private float currentSpeed;
    [SerializeField] private int currentCount = 2;
    [SerializeField] private int currentHealth;

    private const int BASE_HEALTH = 100;
    private const float BASE_SPEED = 1;
    private const int BASE_COUNT = 6;

    private const int HEALTH_INCREASE_FLAT = 100;
    private const float SPEED_INCREASE_FLAT = .4f;
    private const int COUNT_INCREASE_FLAT_PRE_TEN = 1;
    private const int COUNT_INCREASE_FLAT_POST_TEN = 10;

    private const float HEALTH_INCREASE_MULTIPLIER = 1.1f;

    [Header("Refrences")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private DropManager dropManager;
    [SerializeField] private SpawnPoint[] spawnPoints;

    [Header("Settings")]
    [SerializeField] private int maxHordeSize = 30;
    [SerializeField] private float spawnDelay = 0.2f;
    [SerializeField] private float waveDelay = 5f;

    public UnityEvent onWaveIncrease;

    private void Awake()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
    }

    private void Start()
    {
        currentHealth = BASE_HEALTH;
        currentSpeed = BASE_SPEED;
        currentCount = BASE_COUNT + playerCount;

        StartCoroutine("gameTimer");
    }

    private void startFirstWave()
    {
        currentWave = 1;
        StartCoroutine("waveTimer");
    }

    private void startNextWave()
    {
        currentWave++;
        onWaveIncrease.Invoke();

        if (currentWave < 10)
        {
            preTenUpgrade();
        }
        else
        {
            postTenUpgrade();
        }

        StartCoroutine("waveTimer");
    }

    private IEnumerator gameTimer()
    {
        startFirstWave();

        while (playerCount > 0)
        {
            yield return new WaitForSeconds(waveDelay);

            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (remainingSpawns <= 0 && enemyCount <= 0)
            {
                startNextWave();
            }
        }
    }

    private IEnumerator waveTimer()
    {

        remainingSpawns = currentCount;
        while (remainingSpawns > 0)
        {
            yield return new WaitForSeconds(spawnDelay);

            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCount < maxHordeSize) spawnNext();
        }
    }

    private void spawnNext()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            remainingSpawns--;
            return;
        }
        var spawn = getSpawn();
        var enemy = PhotonNetwork.InstantiateRoomObject(enemyPrefab.name, spawn.transform.position, Quaternion.identity);
        var status = enemy.GetComponent<EnemyStatus>();
        var ai = enemy.GetComponent<EnemyAi>();

        if (status == null || ai == null) return;

        status.setHealth(currentHealth);
        status.onDeath.AddListener(pos => dropManager.enemyKilled(pos));
        ai.setSpeed(currentSpeed);
        ai.setSpawnPoint(spawn);

        remainingSpawns--;
    }

    private SpawnPoint getSpawn()
    {
        var rand = Random.Range(0, spawnPoints.Length);
        return spawnPoints[rand];
    }

    private void preTenUpgrade()
    {
        currentHealth += HEALTH_INCREASE_FLAT;
        currentSpeed += SPEED_INCREASE_FLAT;
        currentCount += (COUNT_INCREASE_FLAT_PRE_TEN * currentWave) + playerCount;
    }

    private void postTenUpgrade()
    {
        currentHealth = Mathf.FloorToInt(currentHealth * HEALTH_INCREASE_MULTIPLIER);
        currentCount += COUNT_INCREASE_FLAT_POST_TEN;
    }
    
}
