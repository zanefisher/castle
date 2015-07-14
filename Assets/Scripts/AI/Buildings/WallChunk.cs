using UnityEngine;
using System.Collections;

public class WallChunk : Building {

    public Wall parent;

    public void SetParent(Wall p)
    {
        this.parent = p;
        this.transform.SetParent(p.transform);
    }

    protected override void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    public void dealDamage(int amount, string type)
    {
        this.parent.dealDamage(amount, type);
    }
}
