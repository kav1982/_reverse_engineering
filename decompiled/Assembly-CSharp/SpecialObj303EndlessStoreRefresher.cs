using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class SpecialObj303EndlessStoreRefresher : InteractiveObj
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public GameObject Model;

	public GameObject BaseObject;

	public Text text_Cost;

	public int costStage;

	public float initialCost;

	public float eachStageCost;

	private Entity interactiveEntity;

	public int finalCost => Mathf.FloorToInt(((float)costStage * eachStageCost + initialCost) * (1f + (float)Mathf.Min(BattleMgr.Inst.CurrentLevel - 1, 30) / 10f));

	public bool coinEnough => PlayerMgr.Inst.CoinCount >= finalCost;

	private void Show()
	{
		BaseObject.SetActive(value: true);
		Model.SetActive(value: true);
		SetDotsObjLayer(interactiveEntity, isOpen: true);
	}

	private void Hide()
	{
		BaseObject.SetActive(value: false);
		Model.SetActive(value: false);
		SetDotsObjLayer(interactiveEntity, isOpen: false);
		costStage -= 2;
		costStage = Mathf.Max(costStage, 0);
	}

	private void Start()
	{
		outline.enabled = false;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.SpecialObj18);
		Hide();
	}

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

	public void Update()
	{
		if (GameMgr.InEndlessMode)
		{
			text_Cost.text = finalCost.ToString();
			if (!coinEnough)
			{
				text_Cost.color = Color.red;
			}
			else
			{
				text_Cost.color = Color.green;
			}
		}
	}

	public override void Select()
	{
		outline.enabled = true;
	}

	public override void Unselect()
	{
		outline.enabled = false;
	}

	public override void Interact()
	{
		if (coinEnough)
		{
			SpecialObj301EndlessMonsterSpawner.Inst.RefreshStoreItems();
			PlayerMgr.Inst.ChangeCoin(-finalCost);
			costStage++;
			SEMgr.Inst.endlessStoreRefresh.PlaySE();
		}
	}
}
