using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    private float soundDuration;
    private float timeElapsed;

    // Start is called before the first frame update
    void Start() {
        timeElapsed = 0;
        soundDuration = GetComponent<AudioSource>().clip.length;
    }

    // Update is called once per frame
    void Update() {
        timeElapsed += Time.deltaTime;
        if(timeElapsed >= soundDuration) {
            Stop();
        }
    }

    public void Stop() {
        Destroy(gameObject);
    }
}
