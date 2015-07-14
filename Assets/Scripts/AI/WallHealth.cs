using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallHealth : Health {

    private Wall parent;

    public void SetParent(Wall p) { this.parent = p; }
    public void SetHealth(int h)  { this.health = h; }

    protected override void OnDamage(int amount, string type)
    {
        this.health -= amount;
        if (this.health <= 0)
        {
            parent.DestroyWall();
        }
    }
}
