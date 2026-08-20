using UnityEngine;

public class NavMeshGround : MonoBehaviour
{
	public Sprite[] randomSprites;

	public void Initialize()
	{
		base.transform.GetComponentInChildren<SpriteRenderer>().sprite = randomSprites[Random.Range(0, randomSprites.Length)];
	}
}
