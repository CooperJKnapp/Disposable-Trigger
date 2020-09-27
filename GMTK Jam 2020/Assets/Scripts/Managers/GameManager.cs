using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Important pointers
    [Header("Prefabs and References")]
    [SerializeField]
    private GameObject[] enemyPrefabs;
    private GameObject playerObject;
    private playerController playerController;
    [SerializeField]
    private Text timerText;
    private float gameTimePassed;
    private float gameOverTimePassed;
    private bool gameOver;
    private DontDestroyData gameData;

    [Header("Game Stats")]
    [SerializeField]
    private float baseSpawnRate;
    private float spawnRate;
    private float spawnInterval;
    private float spawnTimePassed;
    [SerializeField]
    private float spawnRateIncrease;
    private float spawnCount;
    [SerializeField]
    private float spawnCountIncrease;
    [SerializeField]
    private float baseItemDropChance;
    [SerializeField]
    private float guaranteedDropPercent;

    // Start is called before the first frame update
    void Start() {
        playerObject = GameObject.Find("Player");
        playerController = playerObject.GetComponent<playerController>();

        gameTimePassed = 0;
        gameOver = false;
        gameData = GameObject.Find("Data").GetComponent<DontDestroyData>();

        // Spawn rate logic
        spawnRate = baseSpawnRate;
        spawnInterval = 1 / spawnRate;
        spawnCount = 1;
        spawnTimePassed = spawnInterval - 1;
    }

    // Update is called once per frame
    void Update() {
        // Game over logic
        if(gameOver) {
            gameOverTimePassed += Time.deltaTime;
            if(gameOverTimePassed > 2) {
                SceneManager.LoadScene(2);
            } return;
        }

        // Spawning logic
        spawnTimePassed += Time.deltaTime;
        gameTimePassed += Time.deltaTime;
        if(spawnTimePassed >= spawnInterval) {
            SpawnEnemies();
        }

        timerText.text = "TIME: " + Math.Round(gameTimePassed, 1);

        UpdateDifficulty();
    }

    /// <summary>
    /// Returns a PickupType depending on the state of the game
    /// </summary>
    /// <returns></returns>
    public PickupType RollForDrop() {
        if((float)playerController.Ammo / (float)playerController.MaxAmmo
            <= guaranteedDropPercent) {
            // Check if there are any current ammo drops out
            GameObject[] currentAmmos = GameObject.FindGameObjectsWithTag("Ammo Pickup");
            if(currentAmmos.Length == 0) {
                return PickupType.Ammo;
            }

        } else if((float)playerController.Durability / (float)playerController.MaxDurability
            <= guaranteedDropPercent) {
            // Check if there are any current ammo drops out
            GameObject[] currentDurabilities = GameObject.FindGameObjectsWithTag("Durability Pickup");
            if (currentDurabilities.Length == 0) {
                return PickupType.Durability;
            }

        } else {
            if(UnityEngine.Random.value < baseItemDropChance) {
                if(UnityEngine.Random.value > 0.5) {
                    return PickupType.Ammo;
                } else {
                    return PickupType.Durability;
                }
            }
        }

        return PickupType.None;
    }

    /// <summary>
    /// Spawns enemeies onto the field
    /// </summary>
    private void SpawnEnemies() {
        // Spawn enemies
        for(int i = 0; i < (int)spawnCount; i++) {
            // Set spawning position
            Vector3 position = new Vector3(12, 0, 0);
            position = MathUtil.Rotate(position, UnityEngine.Random.Range(0,360));

            // Set enemy type
            int enemyType = Mathf.RoundToInt(UnityEngine.Random.Range(0, Mathf.Clamp(spawnCount - 1, 0, enemyPrefabs.Length - 1)));

            // Spawn enemy
            Instantiate(enemyPrefabs[enemyType], position, Quaternion.identity);
        }

        spawnCount += spawnCountIncrease;
        spawnTimePassed -= spawnInterval;
    }

    /// <summary>
    /// Increments difficulty settings
    /// </summary>
    private void UpdateDifficulty() {
        spawnRate += spawnRateIncrease * Time.deltaTime;
        spawnInterval = 1 / spawnRate;
    }

    /// <summary>
    /// End the game due to a playerdeath
    /// </summary>
    public void EndGame() {
        gameOver = true;
        gameData.SurvivalTime = gameTimePassed;
    }
}
