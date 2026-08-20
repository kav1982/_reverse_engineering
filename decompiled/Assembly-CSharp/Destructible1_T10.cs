using UnityEngine;

public class Destructible1_T10 : UnitBase
{
	[Space(50f)]
	public MeshRenderer mr;

	public MeshRenderer mr_Shadow;

	public Sprite[] sprites;

	public Sprite[] sprites_Holloween;

	public Sprite[] sprites_Spring;

	public Sprite[] sprites_Summer;

	public float offset;

	public override void EveryInitialCallback()
	{
		Sprite[] array = sprites;
		if (GameMgr.CampSkinType == CampSkinType.Halloween)
		{
			array = sprites_Holloween;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Spring)
		{
			array = sprites_Spring;
		}
		else if (GameMgr.CampSkinType == CampSkinType.Summer)
		{
			array = sprites_Summer;
		}
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, array[Random.Range(0, array.Length)].texture);
		if (mr_Shadow != null)
		{
			mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, mr.material.mainTexture);
		}
		base.transform.position += Tool2D.GetDir() * offset;
		myPpt.CorrectLayerOnce();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		ItemInfo rewardD1_T = OutputMgr.GetRewardD1_T10();
		if (rewardD1_T.id != 0)
		{
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, rewardD1_T, base.transform.position);
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		base.Anima.SetTrigger("BeHit");
	}
}
