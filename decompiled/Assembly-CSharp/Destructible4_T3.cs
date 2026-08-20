using UnityEngine;

public class Destructible4_T3 : UnitBase, IRoomCtrller
{
	[Space(50f)]
	public MeshRenderer mr;

	public MeshRenderer mr_Shadow;

	public Sprite[] sprites;

	public Sprite sprite_Fruit;

	public Sprite sprite_FruitDave;

	private RoomController belongCtrller;

	private ItemInfo rewardItemInfo;

	public override void EveryInitialCallback()
	{
		rewardItemInfo = OutputMgr.GetRewardD4_T3();
		if (rewardItemInfo.id == 0)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[Random.Range(0, sprites.Length)].texture);
		}
		else if (DataMgr.selectedWorldData.IsDave)
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_FruitDave.texture);
			myPpt.unitCfg = UnitConfig.map[10402];
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg = myPpt.unitCfg;
			SetComponentData(componentData);
		}
		else
		{
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Fruit.texture);
		}
		mr_Shadow.material.SetTexture(GameConstManaged.shaderTextureIndex, mr.material.GetTexture(GameConstManaged.shaderTextureIndex));
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (rewardItemInfo.id != 0)
		{
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, rewardItemInfo, base.transform.position);
		}
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongCtrller = roomCtrller;
	}
}
