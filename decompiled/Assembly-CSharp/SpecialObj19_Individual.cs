using UnityEngine;

public class SpecialObj19_Individual : LayerCorrect
{
	[Space(50f)]
	public MeshRenderer mr;

	public MeshRenderer mr_Lava;

	public Sprite sprite_H;

	public Sprite sprite_Lava_H;

	private void Start()
	{
		if (GameMgr.IsChAge14_Static)
		{
			if (sprite_H != null)
			{
				mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_H.texture);
			}
			if (sprite_Lava_H != null)
			{
				mr_Lava.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Lava_H.texture);
			}
		}
		mr.material.SetInt("_FlipX", Random.Range(0, 2) * 2 - 1);
		mr_Lava.material.SetInt("_FlipX", mr.material.GetInt("_FlipX"));
		mr_Lava.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Lava1);
		Object.Destroy(this);
	}
}
