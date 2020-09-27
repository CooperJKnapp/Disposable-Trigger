using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSlideIn : MonoBehaviour
{
    [SerializeField]
    private float totalTime;
    private float currentTime;
    [SerializeField]
    private float startX;
    [SerializeField]
    private float startY;
    [SerializeField]
    private float endX;
    [SerializeField]
    private float endY;
    private float currentX;
    private float currentY;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        transform.position = new Vector2(startX, startY);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime >= totalTime) {
            transform.position = new Vector2(endX, endY);
            return;
        } else {
            currentX = (endX - startX) * Mathf.Sqrt(1 - (Mathf.Pow(currentTime - totalTime, 2) / Mathf.Pow(totalTime, 2))) + startX;
            currentY = (endY - startY) * Mathf.Sqrt(1 - (Mathf.Pow(currentTime - totalTime, 2) / Mathf.Pow(totalTime, 2))) + startY;
            transform.position = new Vector2(currentX, currentY);

            currentTime += Time.deltaTime;
        }
    }

    public void ReplayAnimation() {
        currentTime = 0;
    }
}
