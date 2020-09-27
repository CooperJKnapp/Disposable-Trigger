using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthUI : barUI
{
    // Update is called once per frame
    void Update() {
        SetMaskPercent((float)player.Health / (float)player.MaxHealth);
    }
}
