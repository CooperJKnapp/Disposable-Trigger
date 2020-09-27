using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyData : MonoBehaviour
{
    // FIELDS
    private float survivalTime;

    public float SurvivalTime {
        get { return survivalTime; }
        set { survivalTime = value; }
    }

    // Start is called before the first frame update
    void Start() {
        Object.DontDestroyOnLoad(gameObject);
    }
}
