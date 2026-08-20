using System;
using UnityEngine;

[Serializable]
public struct SpriteArray
{
	public Sprite[] sprites;

	public Sprite RandomSprite()
	{
		return sprites[UnityEngine.Random.Range(0, sprites.Length)];
	}
}
