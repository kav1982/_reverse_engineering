using UnityEngine;

public class Destructible1_T3 : UnitBase, IRoomCtrller
{
	[Space(50f)]
	public MeshRenderer mr;

	public MeshRenderer mr_Shadow;

	public Sprite[] sprites;

	public Texture[] textures;

	private RoomController belongCtrller;

	private ItemInfo rewardItemInfo;

	[Header("和谐")]
	public bool needHarmonize;

	public Texture[] sprite_H;

	public Texture[] sprite_H_16;

	[ContextMenu("同步Texture")]
	private void SyncTextures()
	{
		if (sprites == null || sprites.Length == 0)
		{
			return;
		}
		if (textures == null || textures.Length != sprites.Length)
		{
			textures = new Texture[sprites.Length];
		}
		for (int i = 0; i < sprites.Length; i++)
		{
			if (sprites[i] != null)
			{
				textures[i] = sprites[i].texture;
			}
			else
			{
				textures[i] = null;
			}
		}
	}

	public override void EveryInitialCallback()
	{
		if (needHarmonize && GameMgr.IsChAge14_Static && sprite_H.Length != 0)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_H[Random.Range(0, sprite_H.Length)]);
		}
		else if (needHarmonize && GameMgr.IsHarmony_Static && sprite_H_16.Length != 0)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_H_16[Random.Range(0, sprite_H_16.Length)]);
		}
		else if (sprites.Length != 0)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[Random.Range(0, sprites.Length)].texture);
		}
		rewardItemInfo = OutputMgr.GetRewardD1_T3();
		if (mr_Shadow != null)
		{
			mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, mr.material.GetTexture(GameConstManaged.shaderTextureIndex));
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (rewardItemInfo.id != 0)
		{
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, rewardItemInfo, base.transform.position);
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.Anima.SetTrigger("BeHit");
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongCtrller = roomCtrller;
	}
}
