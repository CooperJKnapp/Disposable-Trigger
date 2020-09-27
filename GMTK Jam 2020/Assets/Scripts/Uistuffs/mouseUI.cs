using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseUI : barUI
{
    // Update is called once per frame
    void Update() {
        SetMaskPercent((float)player.Durability/(float)player.MaxDurability);
    }
}
