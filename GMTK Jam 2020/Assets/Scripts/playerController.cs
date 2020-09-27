using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.U2D;

public class playerController : MonoBehaviour
{
    // Object components
    [Header("Object Components")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject grenade;
    private Rigidbody2D rb2d;
    [SerializeField]
    private Sprite[] playerSprites;
    private SpriteRenderer sr;
    private GameManager gameManager;

    // Meta fields
    private bool dead;

    // Movement fields
    [Header("Movement Fields")]
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float deceleration;

    // Combat fields
    [Header("Combat Fields")]
    [SerializeField]
    private int maxHealth;
    private int currentHealth;
    [SerializeField]
    private int maxAmmo;
    private int currentAmmo;
    [SerializeField]
    private int maxDurability;
    private int currentDurability;
    [SerializeField]
    private float machinegunFireRate;
    private float machinegunFireDelay;
    private float timeSinceLastShot;
    private bool firing;
    [SerializeField]
    private int grenadeAmmoCost;
    [SerializeField]
    private float iframeDuration;
    private float iframeCounter;
    private const float IFRAME_FLASH_INTERVAL = 0.1f;

    // Screenshake fields
    private const float DEATH_SHAKE = 0.1f;
    private const float GUN_SHAKE = 0.05f;
    private const float GRENADE_SHAKE = 0.05f;

    [Header("SFX")]
    [SerializeField]
    private GameObject ammoClickSFX;
    [SerializeField]
    private GameObject deathSFX;
    [SerializeField]
    private GameObject grenadeSFX;
    [SerializeField]
    private GameObject gunSFX;
    [SerializeField]
    private GameObject hitSFX;

    // Misc fields
    [Header("Misc Fields")]
    [SerializeField]
    private float bulletSpawnOffset;

    // Properties
    public int Durability {
        get { return currentDurability; }
    }

    public int MaxDurability {
        get { return maxDurability; }
    }

    public int Ammo {
        get { return currentAmmo; }
    }

    public int MaxAmmo {
        get { return maxAmmo; }
    }

    public int Health {
        get { return currentHealth; }
    }

    public int MaxHealth {
        get { return maxHealth; }
    }


    // Start is called before the first frame update
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        firing = false;
        dead = false;
        iframeCounter = 0;
        machinegunFireDelay = 1 / machinegunFireRate;
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        currentDurability = maxDurability;
        sr.sprite = playerSprites[0];
    }

    // Update is called once per frame
    void Update() {
        if(dead) {
            return;
        }

        Animate();
        Move();
        Aim();
        Shoot();
    }

    /// <summary>
    /// Performs movement logic
    /// </summary>
    private void Move() {
        // Movement control
        Vector2 controlVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        if (controlVector != Vector2.zero) {
            rb2d.velocity += controlVector * acceleration;
        } else {
            rb2d.velocity -= rb2d.velocity.normalized * deceleration;
        }

        // Clamp speed
        if (rb2d.velocity.magnitude >= maxSpeed) {
            rb2d.velocity = rb2d.velocity.normalized * maxSpeed;
        }
    }

    /// <summary>
    /// Performs aiming logic
    /// </summary>
    private void Aim() {
        // Aim control
        Vector3 aimScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newUp = (aimScreenPosition - transform.position).normalized;
        transform.up = newUp;
    }

    /// <summary>
    /// Performs shooting logic
    /// </summary>
    private void Shoot() {
        // Fire control
        if(Input.GetButton("Fire1") && firing && currentAmmo > 0) {
            // Continue firing machinegun
            timeSinceLastShot += Time.deltaTime;
            if(timeSinceLastShot >= machinegunFireDelay) {
                timeSinceLastShot -= machinegunFireDelay;
                currentAmmo--;

                // Spawn bullet
                GameObject newBullet = Instantiate(bullet, transform.position + transform.up * bulletSpawnOffset, Quaternion.identity);
                newBullet.transform.up = transform.up;
                Camera.main.GetComponent<CameraManager>().Shake(GUN_SHAKE);
                Instantiate(gunSFX, transform.position, Quaternion.identity);
            }
        } else if(Input.GetButtonUp("Fire1") && firing || currentAmmo <= 0) {
            // Stop firing machinegun
            firing = false;
        }

        if(currentDurability > 0) {
            if(Input.GetButtonDown("Fire1") && currentAmmo > 0) {
                // Begin firing machinegun
                firing = true;
                timeSinceLastShot = 0;
                currentAmmo--;
                currentDurability--;

                // Spawn bullet
                GameObject newBullet = Instantiate(bullet, transform.position + transform.up * bulletSpawnOffset, Quaternion.identity);
                newBullet.transform.up = transform.up;
                Camera.main.GetComponent<CameraManager>().Shake(GUN_SHAKE);
                Instantiate(gunSFX, transform.position, Quaternion.identity);

            } else if(Input.GetButtonDown("Fire2") && currentAmmo >= grenadeAmmoCost) {
                // Fire grenade launcher
                currentAmmo -= grenadeAmmoCost;
                currentDurability--;
                GameObject newGrenade = Instantiate(grenade, transform.position + transform.up * bulletSpawnOffset, Quaternion.identity);
                newGrenade.transform.up = transform.up;
                Camera.main.GetComponent<CameraManager>().Shake(GRENADE_SHAKE);
                Instantiate(grenadeSFX, transform.position, Quaternion.identity);
            }
        }

        if((currentDurability <= 0
            && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")))
            || (currentAmmo < grenadeAmmoCost && Input.GetButtonDown("Fire2"))
            || (currentAmmo <= 0 && Input.GetButtonDown("Fire1"))) {
            Instantiate(ammoClickSFX, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Process iframe data
    /// </summary>
    private void Animate() {
        if(iframeCounter > 0) {
            iframeCounter -= Time.deltaTime;

            // Flash the sprite
            sr.sprite = playerSprites[(int)(iframeCounter / IFRAME_FLASH_INTERVAL) % 2];
        } else {
            sr.sprite = playerSprites[0];
        }
    }

    /// <summary>
    /// Health pickup
    /// </summary>
    public void Reload(int strength) {
        currentAmmo += strength;
        
        if(currentAmmo > maxAmmo) {
            currentAmmo = maxAmmo;
        }
    }

    /// <summary>
    /// Durability pickup
    /// </summary>
    public void Repair(int strength) {
        currentDurability += strength;

        if(currentDurability > maxDurability) {
            currentDurability = maxDurability;
        }
    }

    /// <summary>
    /// Attempt to make the player take the given amount of damage
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(int damage) {
        if(iframeCounter <= 0) {
            currentHealth -= damage;
            iframeCounter = iframeDuration;
            Camera.main.GetComponent<CameraManager>().Shake(DEATH_SHAKE);
            
            if(currentHealth <= 0) {
                Die();
            } else {
                Instantiate(hitSFX, transform.position, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Death logic
    /// </summary>
    private void Die() {
        dead = true;
        sr.sprite = null;
        rb2d.velocity = Vector3.zero;
        Instantiate(deathSFX, transform.position, Quaternion.identity);
        gameManager.EndGame();
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag.Equals("Enemy")) {
            Hit(1);
        }
    }
}
