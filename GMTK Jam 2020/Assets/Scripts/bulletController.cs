using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : projectileController
{
    [Header("Damage Values")]
    [SerializeField]
    private int impactDamage;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Enemy")) {
            other.GetComponent<enemyController>().Hit(impactDamage);
            Destroy(gameObject);
        }
    }
}
