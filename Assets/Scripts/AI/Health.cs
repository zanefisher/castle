using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    protected static List<string> _damageTypes = new List<string>() { "normal", "fire", "explode" };
    protected int health;

    public virtual void dealDamage(int amount, string type)
    {
        this.OnDamage(amount, type);
    }

    protected virtual void OnDamage(int amount, string type)
    {
        this.health -= amount;
        if (this.health <= 0) { this.OnDestroy(); }
    }

    protected virtual void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
