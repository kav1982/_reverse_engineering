using UnityEngine;

public class Monster6_Corpse : MonoBehaviour
{
	public float radius;

	public SphereCollider thisCollider;

	public Transform tsf_EF;

	public void Initialize(float scale)
	{
		thisCollider.radius = radius * scale;
		tsf_EF.localScale = Vector3.one * scale;
	}
}
