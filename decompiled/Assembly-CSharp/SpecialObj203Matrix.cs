using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj203Matrix : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public GameObject go_Wrong;

	public int damage;

	public ParticleSystem[] ps_ChangeColors;

	public UnityEngine.BoxCollider thisCollider;

	private bool isWrong;

	private Vector3 backPoint;

	private bool isEntered;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2097664u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void SetWrong(bool isWrong, Vector3 backPoint)
	{
		this.isWrong = isWrong;
		this.backPoint = backPoint;
	}

	public void GameEnd()
	{
		base.gameObject.SetActive(value: false);
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!base.gameObject.activeSelf || !isWrong || !UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
		{
			return;
		}
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
		info.damage = damage;
		UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
		if (result.unitCfg.unitType == UnitType.Player)
		{
			PlayerMgr.Inst.SetPlayerPoint(backPoint);
			PlayerMgr.Inst.ItemCtrller.ItemPointerToPlayer();
		}
		else if (result.unitCfg.isHybirdUnit)
		{
			UnitDotsSyncSystem.GetComponentObject<UnitPptReference>(other).unitPpt.UnitBas.Theme6Reposition(backPoint - base.transform.position);
		}
		else
		{
			UnitBase_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitBase_Dots>(other);
			componentData.onChapter3Reposition = true;
			componentData.repositionValue = backPoint - base.transform.position;
			UnitDotsSyncSystem.SetComponentData(componentData, other);
		}
		go_Wrong.SetActive(value: false);
		go_Wrong.SetActive(value: true);
		if (!isEntered)
		{
			isEntered = true;
			for (int i = 0; i < ps_ChangeColors.Length; i++)
			{
				Color red = Color.red;
				ParticleSystem.MainModule main = ps_ChangeColors[i].main;
				red.a = main.startColor.color.a;
				main.startColor = red;
			}
		}
		SEMgr.Inst.curseInjuredRandomPoint.PlaySE();
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
