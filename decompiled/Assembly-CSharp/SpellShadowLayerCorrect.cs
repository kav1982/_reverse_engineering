using UnityEngine;

public class SpellShadowLayerCorrect : MonoBehaviour
{
	public Transform Layer;

	private void Update()
	{
		Vector3 position = Layer.position;
		position.z = 1.05f;
		Layer.position = position;
	}
}
