using System.Collections.Generic;
using UnityEngine;

public class ChangeTexAnimaEvent : MonoBehaviour
{
	public MeshRenderer mr;

	public List<Sprite> sprites;

	public List<Sprite> sprites_Harmony;

	public void Start()
	{
		if (GameMgr.IsHarmony_Static && sprites_Harmony.Count > 0)
		{
			sprites = sprites_Harmony;
		}
	}

	public void ChangeTex(int index)
	{
		mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprites[index].texture);
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[index].texture);
	}
}
