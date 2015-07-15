using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandController : MonoBehaviour {

    private BuildController _buildController;
    public float grabRange = 5f;

    void Start()
    {
        _buildController = GameObject.FindObjectOfType<BuildController>().GetComponent<BuildController>();
    }

    public void ThrowUnitsToWall(Queue<WallChunk> wallSegments)
    {
        StartCoroutine(ThrowToWall(wallSegments));
    }

    IEnumerator ThrowToWall(Queue<WallChunk> segments)
    {
        int i = 0;
        int f = segments.Count;

        while (i < f) 
        {
            Unit newWallUnit = (Instantiate(_buildController.unitPrefab, _buildController.transform.position, Quaternion.identity) as GameObject).GetComponent<Unit>();
            WallChunk chunk = segments.Dequeue();

            Vector3 target = chunk.transform.position;
            Vector3 midPoint = (((target - transform.position) * 0.5f) + transform.position) + Vector3.up * 5f;
            Vector3[] throwPath = new Vector3[] { transform.position, midPoint, target };
            chunk.SetThrownMinion(newWallUnit.GetComponent<Minion>());

            Hashtable newWallUnitHash = iTween.Hash("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "TurnUnitIntoWall", "onCompleteTarget", gameObject, "onCompleteParams", chunk);
            StartCoroutine(ThrowUnitToWallCoroutine(newWallUnit, newWallUnitHash));

            i++;
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    IEnumerator ThrowUnitToWallCoroutine(Unit unitToThrow, Hashtable unitHash)
    {
        iTween.MoveTo(unitToThrow.gameObject, unitHash);
        yield return null;
    }

    void TurnUnitIntoWall(WallChunk chunk)
    {
        chunk.thrownMinion.SwitchToState(UnitState.DESTROYING);
        chunk.SwitchToState(BuildingState.BUILDING);
    }
}
