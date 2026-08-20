using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialObj312EndlessExit : InteractiveObj
{
	public CapsuleCollider CC;

	public SpriteRenderer outline;

	public SpriteRenderer main;

	public ParticleSystem particle;

	public GameObject Model;

	public SpriteRenderer sr_Sign;

	private Entity interactiveEntity;

	private Vector3 signOriginPos;

	private bool interacted;

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
		if (!((float)BattleMgr.Inst.CurrentLevel < 18f))
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_EndlessStoreRefresh", base.transform.position, 3f);
			SEMgr.Inst.endlessStoreRefresh.PlaySE();
			Model.SetActive(value: true);
			SetDotsObjLayer(interactiveEntity, isOpen: true);
			sr_Sign.enabled = true;
			particle.Play();
		}
	}

	private void Hide()
	{
		Model.SetActive(value: false);
		SetDotsObjLayer(interactiveEntity, isOpen: false);
		sr_Sign.enabled = false;
		particle.Stop();
		particle.Clear();
	}

	private void Start()
	{
		outline.enabled = false;
		interactiveEntity = RegisterDotsInteractiveObj(CC, InteractiveObjType.BackCampPortal);
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

	private void BackCamp()
	{
		UIMgr.Inst.uiFade.Show(delegate
		{
			GameMgr.Inst.DestroyAllTeammate();
			GameMgr.Inst.ClearAllPool();
			GameMgr.Inst.AllFunctionReset();
			DataMgr.selectedWorldData.inBattle9 = false;
			if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare1)
			{
				DataMgr.selectedWorldData.storyFinishNightmare1 = true;
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare2)
			{
				DataMgr.selectedWorldData.storyFinishNightmare2 = true;
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare3)
			{
				DataMgr.selectedWorldData.storyFinishNightmare3 = true;
			}
			if (GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.PluginActivity.UploadItemSnapshot(2);
			}
			DataMgr.selectedWorldData.BackCampCheckPlot();
			DataMgr.SaveSelectedWorldData();
			TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
			SceneManager.LoadScene("Camp");
		});
	}

	public override void Interact()
	{
		if (!interacted)
		{
			interacted = true;
			UIBattleMgr.Inst.PopoutCurrentFinishBuild(BackCamp);
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
		}
	}
}
