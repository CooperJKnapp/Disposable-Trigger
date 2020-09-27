using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType {
    Ammo,
    Durability,
    None
}

public class itemPickup : MonoBehaviour
{
    [Header("Object Components")]
    [SerializeField]
    private Sprite[] pickupSprites;
    private SpriteRenderer sr;

    [Header("Data Fields")]
    [SerializeField]
    private PickupType pickupType;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private int pickupStrength;

    [Header("SFX")]
    [SerializeField]
    private GameObject pickupSFX;

    // Start is called before the first frame update
    void Start() {
        sr = GetComponent<SpriteRenderer>();

        // Set sprite
        sr.sprite = pickupSprites[(int)pickupType];
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag.Equals("Player")) {
            switch(pickupType) {
                case PickupType.Ammo:
                    other.GetComponent<playerController>().Reload(pickupStrength);
                    break;

                case PickupType.Durability:
                    other.GetComponent<playerController>().Repair(pickupStrength);
                    break;
            }

            Instantiate(pickupSFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
