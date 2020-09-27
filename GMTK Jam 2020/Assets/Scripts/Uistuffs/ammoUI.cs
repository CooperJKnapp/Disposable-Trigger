using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoUI : barUI
{
    // Update is called once per frame
    void Update() {
        SetMaskPercent((float)player.Ammo / (float)player.MaxAmmo);
    }
}
