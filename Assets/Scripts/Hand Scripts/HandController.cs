using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandController : MonoBehaviour {

    private BuildController _buildController;
    public GameObject ballPrefab;

    void Start()
    {
        _buildController = GameObject.FindObjectOfType<BuildController>().GetComponent<BuildController>();
    }

    public void ThrowUnitsToWall(Queue<WallChunk> wallSegments,Vector3 midpoint)
    {
        StartCoroutine(ThrowToWall(wallSegments,midpoint));
    }

    IEnumerator ThrowToWall(Queue<WallChunk> segments,Vector3 midpoint)
    {
        int i = 0;
        int f = segments.Count;

        GameObject ball = (Instantiate(ballPrefab, _buildController.transform.position, Quaternion.identity) as GameObject);

        Vector3 target = midpoint;
        Vector3 midPoint = (((target - transform.position) * 0.5f) + transform.position) + Vector3.up * 5f;
        Vector3[] throwPath = new Vector3[] { transform.position, midPoint, target };

        Hashtable newWallUnitHash = iTween.Hash("path", throwPath, "time", 2f, "easetype", iTween.EaseType.easeInQuad, "oncomplete", "TurnUnitIntoWall", "onCompleteTarget", gameObject, "onCompleteParams", segments);
        StartCoroutine(ThrowUnitToWallCoroutine(ball, newWallUnitHash));
        yield return null;
    }

    IEnumerator ThrowUnitToWallCoroutine(GameObject ball, Hashtable unitHash)
    {
        iTween.MoveTo(ball, unitHash);
        yield return null;
    }

    void TurnUnitIntoWall(Queue<WallChunk> segments)
    {
        while (segments.Count > 0)
        {
            WallChunk chunk = segments.Dequeue();
            chunk.SwitchToState(BuildingState.BUILDING);
        }
    }
}
