using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionController : MonoBehaviour
{
    [Header("Variable Fields")]
    [SerializeField]
    private float explosionDuration;
    private float explosionSpeed;
    [SerializeField]
    private float maxScale;
    private float currentScale;
    [SerializeField]
    private float rotationSpeed;
    
    private int explosionDamage;
    public int ExplosionDamage {
        get {
            return explosionDamage;
        }

        set {
            explosionDamage = value;
        }
    }

    // Start is called before the first frame update
    void Start() {
        currentScale = transform.localScale.x;
        explosionSpeed = (maxScale + transform.localScale.x) / explosionDuration;
    }

    // Update is called once per frame
    void Update() {
        // Rotate
        transform.Rotate(0,0, rotationSpeed * Time.deltaTime);

        // Setup scale value
        currentScale += explosionSpeed * Time.deltaTime;
        if (transform.localScale.x >= maxScale) {
            explosionSpeed *= -1;
            currentScale = maxScale + explosionSpeed * Time.deltaTime;
        } else if (transform.localScale.x <= 0) {
            Destroy(gameObject);
        }

        // Set scale to value
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Enemy")) {
            other.GetComponent<enemyController>().Hit(explosionDamage);
        }
    }
}
