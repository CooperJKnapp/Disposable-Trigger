using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField]
    private float shakeDampening;
    private float shakeStrength;
    private Vector3 baseCameraPosition;
    private bool shaking;

    // Start is called before the first frame update
    void Start() {
        shakeStrength = 0;
        shaking = false;
        baseCameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update() {
        // Vector based screenshake
        if(shaking) {
            Vector3 shakeVector = new Vector3(shakeStrength, 0, 0);
            shakeVector = MathUtil.Rotate(shakeVector, Random.Range(0, 360));
            transform.position = baseCameraPosition + shakeVector;

            shakeStrength -= shakeDampening * Time.deltaTime;
            if (shakeStrength <= 0) {
                shakeStrength = 0;
                shaking = false;
                transform.position = baseCameraPosition;
            }
        }
    }

    // Camera.main.gameObject.GetComponent<CameraManager>().Shake();
    /// <summary>
    /// Shake the screen;
    /// </summary>
    /// <param name="strength"></param>
    public void Shake(float strength) {
        shakeStrength += strength;
        shaking = true;
    }
}
