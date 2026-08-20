using System.Collections.Generic;
using UnityEngine;

public class Spell9043ImageRandomize : MonoBehaviour
{
	public List<Sprite> images = new List<Sprite>();

	public SpriteRenderer sr;

	private void OnEnable()
	{
		sr.sprite = images[Random.Range(0, images.Count)];
		sr.transform.localEulerAngles = Vector3.forward * Random.value * 360f;
	}
}
