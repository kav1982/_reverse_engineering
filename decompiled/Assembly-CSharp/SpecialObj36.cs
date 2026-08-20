using System;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj36 : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public MeshRenderer mr;

	public Sprite sprite_Open;

	public Sprite sprite_Close;

	public Sprite sprite_Disable;

	public UnityEngine.CapsuleCollider thisCollider;

	private bool isEndlessDisable;

	private bool beforeEndlessBattleOpen;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.EndlessStageStart = (Action)Delegate.Combine(EventMgr.EndlessStageStart, new Action(Hide));
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(Show));
	}

	private void OnDisable()
	{
		EventMgr.EndlessStageStart = (Action)Delegate.Remove(EventMgr.EndlessStageStart, new Action(Hide));
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(Show));
	}

	private void Show()
	{
		isEndlessDisable = false;
		DataMgr.selectedWorldData.isScarecrowOpen = beforeEndlessBattleOpen;
		mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, DataMgr.selectedWorldData.isScarecrowOpen ? sprite_Open.texture : sprite_Close.texture);
		EventMgr.ScarecrowChange?.Invoke();
	}

	private void Hide()
	{
		isEndlessDisable = true;
		mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_Disable.texture);
		beforeEndlessBattleOpen = DataMgr.selectedWorldData.isScarecrowOpen;
		DataMgr.selectedWorldData.isScarecrowOpen = false;
		EventMgr.ScarecrowChange?.Invoke();
	}

	private void Start()
	{
		mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, DataMgr.selectedWorldData.isScarecrowOpen ? sprite_Open.texture : sprite_Close.texture);
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!isEndlessDisable && !(other != PlayerMgr.Inst.PlayerEtt))
		{
			DataMgr.selectedWorldData.isScarecrowOpen = !DataMgr.selectedWorldData.isScarecrowOpen;
			mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, DataMgr.selectedWorldData.isScarecrowOpen ? sprite_Open.texture : sprite_Close.texture);
			EventMgr.ScarecrowChange?.Invoke();
			SEMgr.Inst.puzzleClick.PlaySE();
			if (!DataMgr.selectedWorldData.isScarecrowOpen)
			{
				PlayerMgr.Inst.SummonsLoseTarget();
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
