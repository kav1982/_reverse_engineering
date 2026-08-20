using Unity.Entities;
using UnityEngine;

public class Brittleness2 : UnitBase, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Sprite[] sprites;

	public MeshRenderer mr;

	public float offset;

	public VariableFloat scale;

	public LayerMask canCollideLayer;

	[Header("和谐")]
	public bool needHarmonize;

	public Sprite[] sprites_H;

	public Entity thisEntity { get; set; }

	public override void EveryInitialCallback()
	{
		if (needHarmonize && GameMgr.IsHarmony_Static)
		{
			for (int i = 0; i < sprites.Length; i++)
			{
				sprites[i] = sprites_H[i];
			}
		}
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[Random.Range(0, sprites.Length)].texture);
		mr.material.SetFloat(GameConstManaged.shaderFlipXIndex, Random.Range(0, 2) * 2 - 1);
		base.transform.position += Tool2D.GetDir() * Random.Range(0f, offset);
		myPpt.tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
		myPpt.tsf_Layer.localScale = Vector3.one * scale.RandomResult();
		SyncDotsPosition();
		base.enabled = false;
		myPpt.enabled = false;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		DotsAnnouncedDeath();
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
