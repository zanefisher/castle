using UnityEngine;
using System.Collections;

public class UnitSpawner : MonoBehaviour {

    public GameObject unitPrefab;
    public float spawnFrequency = 5f;
    private float lastSpawn = -5f;

    void Start()
    {

    }

    void Update()
    {
        if (Time.time > lastSpawn + spawnFrequency)
        {
            Unit u = SpawnUnit(unitPrefab);
            //Vector2 v = Random.insideUnitCircle * 100f;
            //Vector3 n = new Vector3(v.x, 0f, v.y);
            //u.SetGoal(n);
            lastSpawn = Time.time;
        }
    }

    private Unit SpawnUnit(GameObject prefab)
    {
        return ((GameObject)GameObject.Instantiate(prefab)).GetComponent<Unit>();
    }
}
