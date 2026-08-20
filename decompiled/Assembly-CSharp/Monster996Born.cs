using Unity.Entities;
using UnityEngine;

public class Monster996Born : LayerCorrect
{
	[Space(50f)]
	public Animator animator;

	public AnimaEvent animaEvent;

	private bool isSingleInitialized;

	private Entity itemEntity;

	public Curse_Stealthy CurseStealthy { get; private set; }

	public RoomController BelongRoom { get; private set; }

	public void Initialize(Curse_Stealthy curse_Stealthy, RoomController belongRoom, Entity targetItem)
	{
		if (!isSingleInitialized)
		{
			isSingleInitialized = true;
			animaEvent.DoAction = AnimaAction;
		}
		CurseStealthy = curse_Stealthy;
		BelongRoom = belongRoom;
		SEMgr.Inst.monster996Born.PlaySE();
		itemEntity = targetItem;
	}

	public void Update()
	{
	}

	private void AnimaAction(string animaName)
	{
		if (animaName == "CreateObj")
		{
			Monster996 component = CurseStealthy.MiniPool.GetGO("Prefabs/Units/" + 199601, base.transform.position).GetComponent<Monster996>();
			component.Setting(this, itemEntity);
			CurseStealthy.MonsterRegister(component);
			CurseStealthy.BornUnregister(this);
		}
		else
		{
			Debug.LogError(animaName);
		}
	}
}
