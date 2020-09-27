using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barUI : MonoBehaviour
{
    protected playerController player;
    private Transform spriteMask;
    private Vector3 deltaPosition;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player").GetComponent<playerController>();
        spriteMask = transform.GetChild(0);
        deltaPosition = spriteMask.position - transform.position;
    }

    protected void SetMaskPercent(float percent) {
        spriteMask.position = transform.position + deltaPosition * percent;
    }
}
