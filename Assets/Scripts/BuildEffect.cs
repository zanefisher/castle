using UnityEngine;
using System.Collections;

public class BuildEffect : MonoBehaviour {

	// Use this for initialization

	public float buildSpeed=.1f;
	void Start () {
	
	}


	public void Build(float amount){
		//transform.Translate(Vector3.down*transform.localScale.y);
		StartCoroutine(BuildCoroutine (amount));
	}

	IEnumerator BuildCoroutine(float amount){
		float i=0;
			Vector3 startPos=transform.position;
		while (i<1f){

				transform.position=Vector3.Lerp(startPos,startPos+Vector3.down*amount,i);
			i+=buildSpeed;
			yield return 0;
		}

	}	
	// Update is called once per frame
	void Update () {
	
	}
}
