using Unity.Physics;
using UnityEngine;

public class Destructible1_T0 : UnitBase, IRoomCtrller
{
	private static readonly int MainTex = Shader.PropertyToID("_MainTex");

	[Space(50f)]
	public Sprite spriteSelected;

	public MeshRenderer mr;

	public MeshRenderer mr_HighLight;

	public Sprite[] sprites;

	public Sprite sprite_Iron;

	private RoomController belongCtrller;

	private ItemInfo rewardItemInfo;

	[Header("铁皮箱子高亮提示")]
	public float highLightDuration;

	public AnimationCurve highLightCurve;

	private float highLightTimer;

	public unsafe override void EveryInitialCallback()
	{
		rewardItemInfo = OutputMgr.GetRewardD1_T0();
		if (rewardItemInfo.id == 0 || rewardItemInfo.id == 11)
		{
			base.tag = "Destructible";
			base.gameObject.layer = LayerMask.NameToLayer("Destructible");
			mr.material.SetTexture(MainTex, sprites[Random.Range(0, sprites.Length)].texture);
			mr_HighLight.enabled = false;
		}
		else
		{
			base.tag = "SolidObj";
			mr.material.SetTexture(MainTex, sprite_Iron.texture);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.isSolidObj = true;
			SetComponentData(componentData);
			PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
			CollisionFilter collisionFilter = componentData2.ColliderPtr->GetCollisionFilter();
			collisionFilter.BelongsTo |= 256u;
			componentData2.ColliderPtr->SetCollisionFilter(collisionFilter);
		}
		SetMRFlip(mr, Random.Range(0, 2) == 0);
		if (rewardItemInfo.id != 0)
		{
			SetMRFlip(mr, flipX: false);
		}
	}

	public override void Update()
	{
		base.Update();
		if (rewardItemInfo.id != 0)
		{
			highLightTimer += Time.deltaTime;
			if (highLightTimer > highLightDuration)
			{
				highLightTimer = 0f;
			}
			mr_HighLight.material.SetFloat("_Offset", highLightCurve.Evaluate(highLightTimer));
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
