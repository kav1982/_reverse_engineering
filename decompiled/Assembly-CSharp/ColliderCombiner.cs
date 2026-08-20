using UnityEngine;

public class ColliderCombiner : MonoBehaviour
{
	public GameObject Combine(string newGOName, bool destroyCollider = true)
	{
		MeshCollider[] componentsInChildren = GetComponentsInChildren<MeshCollider>(includeInactive: false);
		CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
		GameObject gameObject = new GameObject(newGOName);
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.parent = base.transform.parent;
		if (componentsInChildren.Length == 0)
		{
			Debug.Log("这个物体下没有Mesh Collider:" + base.gameObject.name);
		}
		else
		{
			gameObject.tag = componentsInChildren[0].tag;
			gameObject.layer = componentsInChildren[0].gameObject.layer;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i].mesh = componentsInChildren[i].sharedMesh;
			array[i].transform = componentsInChildren[i].transform.localToWorldMatrix;
			if (destroyCollider)
			{
				Object.Destroy(componentsInChildren[i]);
			}
		}
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(array);
		meshCollider.sharedMesh = mesh;
		return gameObject;
	}
}
