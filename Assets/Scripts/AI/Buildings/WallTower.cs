using UnityEngine;
using System.Collections;

public class WallTower : Building {

    public Wall parent;

    public void SetParent(Wall p)
    {
        this.parent = p;
        this.transform.SetParent(p.transform);
    }

	protected override void SwitchToDestroying(){
		//Insert Destruction animation
		//Change wall tower model to "Destroyed" model
		this.gameObject.layer = 0;
		this._renderer.material.color = Color.green;
	}

    public void dealDamage(int amount, string type)
    {
        this.parent.dealDamage(amount, type);
    }

    protected override bool IsBuildable()
    {
        if (base.IsBuildable())
        {
            return true;
        }
        else if (this._collidedObjects.Count > 0 && this.GetCollidingTower())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	protected override void SwitchToBuilding()
	{
		this._renderer.material.color = this._originalColor;
		this.SwitchToState(BuildingState.IDLE); //SKIPS IDLE FOR NOW. ONBUILD USED FOR ANIMATIONS
	}
	
	protected override void SwitchToIdle()
    {
        this.gameObject.layer = 0;

        Barracks[] enemies = GameObject.FindObjectsOfType<Barracks>();

        Debug.Log(enemies.Length);

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].converted) continue;
            
            Building b = enemies[i].GetComponent<Building>();
            Debug.Log(b);

            if (b.checkSurroundedByWall(30, 50))
            {
                Debug.Log("Surrounded!");
                b.convertToFriendly();
            }
        }
    }

    public WallTower GetCollidingTower()
    {
        for (int i = 0; i < this._collidedObjects.Count; i++)
        {
            if (_collidedObjects[i].GetComponent<WallTower>())
            {
                return _collidedObjects[i].GetComponent<WallTower>();
            }
        }

        return null;
    }


}
