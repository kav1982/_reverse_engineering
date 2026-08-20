using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpecialObj309EndlessSideTeleporter : InteractiveObj, IRoomObjExtraData
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public SpriteRenderer main;

	public ParticleSystem particle;

	public List<Sprite> showSprite;

	public Sprite waitSprite;

	public GameObject Model;

	public SpriteRenderer sr_Sign;

	private bool isMainRoom;

	private Entity interactiveEntity;

	private Vector3 signOriginPos;

	private bool newWandHint;

	private bool processHint;

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.EndlessStageStart = (Action)Delegate.Combine(EventMgr.EndlessStageStart, new Action(Hide));
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(Show));
		signOriginPos = sr_Sign.transform.localPosition;
	}

	private void OnDisable()
	{
		EventMgr.EndlessStageStart = (Action)Delegate.Remove(EventMgr.EndlessStageStart, new Action(Hide));
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(Show));
	}

	private void Show()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_EndlessStoreRefresh", base.transform.position, 3f);
		SEMgr.Inst.endlessStoreRefresh.PlaySE();
		Model.SetActive(value: true);
		sr_Sign.enabled = true;
		SetDotsObjLayer(interactiveEntity, isOpen: true);
		particle.Play();
	}

	private IEnumerator ShowDelay()
	{
		yield return null;
	}

	private void Hide()
	{
		Model.SetActive(value: false);
		sr_Sign.enabled = false;
		SetDotsObjLayer(interactiveEntity, isOpen: false);
		particle.Stop();
		particle.Clear();
	}

	private void Start()
	{
		outline.enabled = false;
		InteractiveObjType type = (isMainRoom ? InteractiveObjType.SpecialObj309EndlessSideTeleporter : InteractiveObjType.SpecialObj309EndlessSideTeleporterToBattle);
		interactiveEntity = RegisterDotsInteractiveObj(CC, type);
		Hide();
	}

	private void Update()
	{
		sr_Sign.transform.localPosition = signOriginPos + Vector3.up * 0.1f * Mathf.Sin(Time.time * MathF.PI);
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
		if (isMainRoom)
		{
			LevelMgr.Inst.PlayerEnterAccess(FourDir.Right);
			if (!newWandHint && BattleMgr.Inst.CurrentLevel % 5 == 1 && BattleMgr.Inst.CurrentLevel < 26 && BattleMgr.Inst.CurrentLevel > 1)
			{
				newWandHint = true;
				GameUISingletonMono<UIEndlessBattle>.Inst.NewLevelWandHint();
			}
			if (!processHint && SpecialObj301EndlessMonsterSpawner.Inst.HaveSpellProcessor)
			{
				processHint = true;
				GameUISingletonMono<UIEndlessBattle>.Inst.ProcessEnableHint();
			}
		}
		else
		{
			LevelMgr.Inst.PlayerEnterAccess(FourDir.Left);
		}
	}

	void IRoomObjExtraData.SetExtraData(float data1, float data2, float data3)
	{
		if (data1 == 0f)
		{
			isMainRoom = true;
			sr_Sign.sprite = showSprite[0];
		}
		else
		{
			isMainRoom = false;
			sr_Sign.sprite = showSprite[1];
		}
	}
}
