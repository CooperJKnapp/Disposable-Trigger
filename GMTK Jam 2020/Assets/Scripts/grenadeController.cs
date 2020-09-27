using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeController : projectileController
{
    [Header("Object References")]
    [SerializeField]
    private GameObject explosionPrefab;

    [Header("Damage Values")]
    [SerializeField]
    private int impactDamage;
    [SerializeField]
    private int explosionDamage;

    [Header("SFX")]
    [SerializeField]
    private GameObject explosionSFX;

    // Screenshake fields
    private const float EXPLOSION_SHAKE = 0.15f;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if(finished) {
            Explode();
        }
    }

    /// <summary>
    /// Spawn explosion
    /// </summary>
    private void Explode() {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.GetComponent<explosionController>().ExplosionDamage = explosionDamage;

        Camera.main.gameObject.GetComponent<CameraManager>().Shake(EXPLOSION_SHAKE);
        Instantiate(explosionSFX, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Enemy")) {
            other.GetComponent<enemyController>().Hit(impactDamage);
            Explode();
        }
    }
}
