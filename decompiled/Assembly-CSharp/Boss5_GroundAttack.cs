using System.Collections.Generic;
using UnityEngine;

public class Boss5_GroundAttack : MonoBehaviour
{
	public SpriteRenderer thisRenderer;

	public List<Sprite> randomSprite;

	private void OnEnable()
	{
		thisRenderer.sprite = randomSprite[Random.Range(0, randomSprite.Count)];
		thisRenderer.flipX = (double)Random.Range(0f, 1f) > 0.5;
	}
}
