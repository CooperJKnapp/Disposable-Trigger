using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiType {
    Follower,
    Dasher,
    Encirlcer
}

public class enemyController : MonoBehaviour
{
    [Header("Object Components")]
    [SerializeField]
    private Sprite[] enemySprites;
    private SpriteRenderer sr;
    private Rigidbody2D rb2d;
    private Transform playerTransform;
    [SerializeField]
    private GameObject[] itemPickups;
    private GameManager gameManager;

    [Header("Movement Fields")]
    [SerializeField]
    private AiType aiType;
    [SerializeField]
    private float[] movementSpeeds;
    [SerializeField]
    private float dasherBaseDelay;
    private float dasherCurrentDelay;
    private float dasherTimePassed;
    private Vector3 dasherVelocity;
    private float encirclerAngle;

    [Header("Combat Fields")]
    [SerializeField]
    private int maxHealth;
    private int currentHealth;

    [Header("SFX")]
    [SerializeField]
    private GameObject deathSFX;

    // Screenshake fields
    private const float DEATH_SHAKE = 0.1f;


    // Start is called before the first frame update
    void Start() {
        // Get components
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        encirclerAngle = UnityEngine.Random.Range(-80, 80);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // Field setup
        dasherTimePassed = 0;
        currentHealth = maxHealth;
        dasherCurrentDelay = dasherBaseDelay;

        // Sprite setup
        switch (aiType) {
            // Choose sprite based off the AI selected
            case AiType.Follower:
                sr.sprite = enemySprites[0];
                break;

            case AiType.Dasher:
                sr.sprite = enemySprites[1];
                break;

            case AiType.Encirlcer:
                sr.sprite = enemySprites[2];
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        switch(aiType) {
            // Move according to the correct AI
            case AiType.Follower:
                FollowerMove();
                break;

            case AiType.Dasher:
                DasherMove();
                break;

            case AiType.Encirlcer:
                EncirclerMove();
                break;
        }
    }

    private void FollowerMove() {
        // Directly follow the player
        transform.up = (playerTransform.position - transform.position).normalized;

        rb2d.velocity = transform.up * movementSpeeds[0];
    }

    private void DasherMove() {
        // Follow the player by intermittently dashing
        dasherTimePassed += Time.deltaTime;

        if(dasherTimePassed >= dasherCurrentDelay) {
            dasherTimePassed -= dasherCurrentDelay;
            dasherCurrentDelay = dasherBaseDelay * UnityEngine.Random.Range(0.7f, 1.3f);

            transform.up = (playerTransform.position - transform.position).normalized;
            rb2d.velocity = transform.up * movementSpeeds[1];
            dasherVelocity = rb2d.velocity;
        } else {
            rb2d.velocity = dasherVelocity;
        }
    }

    private void EncirclerMove() {
        // Attempt to circle around the player
        transform.up = (playerTransform.position - transform.position).normalized;

        rb2d.velocity = MathUtil.Rotate(transform.up, encirclerAngle) * movementSpeeds[2];
    }

    /// <summary>
    /// Deal indicated damage to the enemy
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(int damage) {
        currentHealth -= damage;
        if(currentHealth <= 0) {
            Die();
        }
    }

    /// <summary>
    /// On-death logic for the enemy
    /// </summary>
    private void Die() {
        PickupType onDeathPickup = gameManager.RollForDrop();
        if(onDeathPickup != PickupType.None) {
            Instantiate(itemPickups[(int)onDeathPickup], transform.position, Quaternion.identity);
        }
        Camera.main.GetComponent<CameraManager>().Shake(DEATH_SHAKE);
        Instantiate(deathSFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
