// This script is placed in public domain. The author takes no responsibility for any possible harm.
import System.Collections.Generic;

var scale = 1.0;
var slopeScaler = 10.5;
var perlinLoc=5000;

private var baseVertices : Vector3[];
    
function Start(){

	var mesh : Mesh = GetComponent(MeshFilter).mesh;
	
	if (baseVertices == null)
		baseVertices = mesh.vertices;
		
	var vertices = new Vector3[baseVertices.Length];
	
	for (var i=0;i<vertices.Length;i++)
	{
		var vertex = baseVertices[i];
		
		vertex.y += Mathf.PerlinNoise((vertex.x+perlinLoc)/slopeScaler,(vertex.z+perlinLoc)/slopeScaler)*scale;

		vertices[i] = vertex;
	}
	
	
	mesh.vertices = vertices;
	//MeshCollider.
	
	GetComponent(MeshCollider).sharedMesh=mesh;
	
}


