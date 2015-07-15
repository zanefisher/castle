using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall : MonoBehaviour {

    public WallTower startTower;
    public WallTower endTower;

    public List<WallChunk> chunks = new List<WallChunk>();

    private WallHealth wallHealth;

    void Start()
    {
        wallHealth = this.gameObject.AddComponent<WallHealth>();
        wallHealth.SetParent(this);
        wallHealth.SetHealth(10);
    }

    public void SetTowers(WallTower st, WallTower et)
    {
        this.transform.position = (st.transform.position + et.transform.position) / 2f;

        this.startTower = st;
        st.SetParent(this);
        this.endTower = et;
        et.SetParent(this);

        //this.startTower.tag = "WallTower";
        //this.endTower.tag = "WallTower";
        
    }

    public void AddChunk(WallChunk chunk) {
        //chunk.tag = "WallChunk";
        chunks.Add(chunk);
        chunk.SetParent(this);
    }

    public void dealDamage(int amount, string type) { this.wallHealth.dealDamage(amount, type); }
    public void SetHealth(int h) { this.wallHealth.SetHealth(h); }

    public void DestroyWall()
    {
        foreach (WallChunk chunk in chunks)
        {
            chunk.SwitchToState(BuildingState.DESTROYING);
        }

        startTower.SwitchToState(BuildingState.DESTROYING);
        endTower.SwitchToState(BuildingState.DESTROYING);
        Destroy(this.gameObject);
    }
}
