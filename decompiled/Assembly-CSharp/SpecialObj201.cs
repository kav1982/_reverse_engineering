using System.Collections;
using System.Collections.Generic;
using PlayerLogger;
using UnityEngine;

public class SpecialObj201 : MonoBehaviour, IRoomCtrller
{
	[Space(50f)]
	public GameObject pfb_Button;

	public float space;

	public float wrongTime;

	[Header("Spell")]
	public int spellCount;

	public VariableFloat spellInterval;

	public Vector2 spellOffset;

	public float spellHeight;

	public float spellDuration;

	public int spellDamage;

	[HideInInspector]
	public int currentOrder;

	private List<SpecialObj201Button> buttons = new List<SpecialObj201Button>();

	private RoomController belongCtrller;

	private bool isWrong;

	private float wrongTimer;

	private SpellInitialParameter sipFireBall = new SpellInitialParameter();

	private SpellSpawnParams ssp;

	private void Start()
	{
		List<int> list = new List<int> { 1, 2, 3, 4 };
		list.Upset();
		SpecialObj201Button component = Object.Instantiate(pfb_Button, base.transform.position + new Vector3((0f - space) / 2f, space / 2f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj201Button>();
		component.Initialize(this, list[0], list[0] - 1);
		buttons.Add(component);
		component = Object.Instantiate(pfb_Button, base.transform.position + new Vector3(space / 2f, space / 2f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj201Button>();
		component.Initialize(this, list[1], list[1] - 1);
		buttons.Add(component);
		component = Object.Instantiate(pfb_Button, base.transform.position + new Vector3((0f - space) / 2f, (0f - space) / 2f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj201Button>();
		component.Initialize(this, list[2], list[2] - 1);
		buttons.Add(component);
		component = Object.Instantiate(pfb_Button, base.transform.position + new Vector3(space / 2f, (0f - space) / 2f, 0f), Quaternion.identity, base.transform.parent).GetComponent<SpecialObj201Button>();
		component.Initialize(this, list[3], list[3] - 1);
		buttons.Add(component);
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10121);
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = 0f;
		sSPModifier.ColorType = SpellColorType.Player;
		sSPModifier.CurrentFallSpeed = 0f;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.CriticalChance = -999999f;
		sSPModifier.ApplyToSSP(ref ssp);
		ssp.ConfigComponentData.UndifferDamageRatio = 1f;
		ssp.ConfigComponentData.MaxUndifferDamageReceive = -1f;
	}

	public void Update()
	{
		if (!isWrong)
		{
			return;
		}
		wrongTimer += Time.deltaTime;
		if (wrongTimer >= wrongTime)
		{
			wrongTimer = 0f;
			isWrong = false;
			for (int i = 0; i < buttons.Count; i++)
			{
				buttons[i].Idle();
			}
			belongCtrller.AllAccessOpen();
		}
	}

	public void StepWrong(SpecialObj201Button button)
	{
		StartCoroutine(IEStepWrong(button));
	}

	private IEnumerator IEStepWrong(SpecialObj201Button button)
	{
		isWrong = true;
		currentOrder = 0;
		for (int j = 0; j < buttons.Count; j++)
		{
			if (buttons[j] != button)
			{
				buttons[j].Wrong();
			}
		}
		belongCtrller.AllAccessClose();
		for (int i = 0; i < spellCount; i++)
		{
			UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = base.transform.position + new Vector3(Random.Range(0f - spellOffset.x, spellOffset.x), Random.Range(0f - spellOffset.y, spellOffset.y), 0f - spellHeight);
			sSPModifier.Direction = Vector3.right;
			sSPModifier.ApplyToSSP(ref ssp);
			ssp.DisableShootSound = true;
			UnitDotsSyncSystem.ShootSpell(ssp);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_SO201_Warning", Tool2D.IgnoreZPoint(sSPModifier.SpawnPosition), spellDuration);
			yield return new WaitForSeconds(spellInterval.RandomResult());
		}
	}

	public void AllCorrect()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position, 2f);
		int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
		ItemInfo itemInfo = default(ItemInfo);
		itemInfo.type = ItemType.Spell;
		itemInfo.id = specialRoomSpell;
		ItemInfo info = itemInfo;
		QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
		LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
