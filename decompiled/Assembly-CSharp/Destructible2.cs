using UnityEngine;

public class Destructible2 : UnitBase
{
	[Space(50f)]
	public bool randomFlipX;

	public Sprite[] textures;

	public Sprite[] texturesH;

	public Sprite[] texturesH_14;

	public MeshRenderer mr;

	public MeshRenderer mr_Shadow;

	public override void EveryInitialCallback()
	{
		Sprite sprite = ((GameMgr.IsChAge16_Static && texturesH_14.Length != 0) ? texturesH_14[Random.Range(0, texturesH_14.Length)] : ((!GameMgr.IsHarmony_Static || texturesH.Length == 0) ? textures[Random.Range(0, textures.Length)] : texturesH[Random.Range(0, texturesH.Length)]));
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite.texture);
		mr.transform.localScale = new Vector3((float)sprite.texture.width / sprite.pixelsPerUnit, (float)sprite.texture.height / sprite.pixelsPerUnit, 1f);
		Vector2 vector = -new Vector2(sprite.pivot.x / (float)sprite.texture.width, sprite.pivot.y / (float)sprite.texture.height) + Vector2.one * 0.5f;
		vector.x *= mr.transform.localScale.x;
		vector.y *= mr.transform.localScale.y;
		mr.transform.localPosition = vector;
		if (mr_Shadow != null)
		{
			mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite.texture);
			mr_Shadow.transform.localScale = mr.transform.localScale;
			mr_Shadow.transform.localPosition = vector;
		}
		if (randomFlipX)
		{
			float value = GeneralTool.HalfChanceNPOne();
			mr.material.SetFloat(GameConstManaged.shaderFlipXIndex, value);
			if (mr_Shadow != null)
			{
				mr_Shadow.material.SetFloat(GameConstManaged.shaderFlipXIndex, value);
			}
		}
	}
}
