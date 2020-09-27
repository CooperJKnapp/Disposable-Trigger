using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    // Object components
    private Rigidbody2D rb2d;

    // Movement fields
    [Header("Movement Fields")]
    [SerializeField]
    private float initialSpeed;
    [SerializeField]
    private float finalSpeed;
    [SerializeField]
    private float timeToSlowdown;
    private float timePassed;
    private float speedPerSecond;
    protected bool finished;
    private float lifetime;
    private const float TOTAL_LIFETIME = 5f;

    // Start is called before the first frame update
    protected virtual void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        speedPerSecond = (finalSpeed - initialSpeed) / timeToSlowdown;
        timePassed = 0;
        finished = false;
        lifetime = 0;

        // Set bullet in motion
        rb2d.velocity = transform.up * initialSpeed;
    }

    // Update is called once per frame
    protected virtual void Update() {
        lifetime += Time.deltaTime;
        if(lifetime > TOTAL_LIFETIME) {
            Destroy(gameObject);
        }

        if(timePassed <= timeToSlowdown) {
            rb2d.velocity += rb2d.velocity.normalized * speedPerSecond * Time.deltaTime;
            timePassed += Time.deltaTime;

            // Clamp speed
            if(timePassed >= timeToSlowdown) {
                rb2d.velocity = rb2d.velocity.normalized * finalSpeed;
                finished = true;
            }
        }
    }
}
