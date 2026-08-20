using System;
using System.Collections;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[GameUISingletonPrefab("UIPlayerDead")]
public class UIPlayerDead : GameUISingletonMono<UIPlayerDead>
{
	private enum UIState
	{
		Idle,
		Show,
		MoveToCenter,
		WaitFinish
	}

	public RectTransform rtsf_PlayerRoot;

	public Animator anima;

	public SkeletonGraphic sGraphic;

	public Vector2 centerLocalPoint;

	public float moveSpeed;

	public float timeScale;

	public AudioSource as_DeadMusic;

	public Animator pfb_UIRelicHuangInSet;

	public float uiRelicHuangInSetInitialSize;

	public Vector2 uiRelicHuangInSetOffset;

	[Header("Language")]
	public Text text_YouDie;

	public TrailRenderer trailRenderer;

	public Gradient trailGradientH;

	public Material trailMaterialH;

	private UIState state;

	private Animator anima_UIRelicHunagInSet;

	protected override void RegistarWhenInit()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void OnEnable()
	{
		LanguageChange();
		SoundVolumeChange();
	}

	private void LanguageChange()
	{
		text_YouDie.text = 1002301.GetText();
	}

	private void SoundVolumeChange()
	{
		as_DeadMusic.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		if (GameMgr.IsHarmony_Static)
		{
			trailRenderer.colorGradient = trailGradientH;
			trailRenderer.material = trailMaterialH;
		}
	}

	protected override void OnShow(object obj = null)
	{
		UIPlayerDataMgr.Inst.UISlotBagExitall();
		state = UIState.Show;
		anima.SetTrigger("Show");
		DataMgr.selectedWorldData.inBattle9 = false;
		DataMgr.selectedWorldData.isPlayerDeadBackCamp = true;
		DataMgr.selectedWorldData.BackCampCheckPlot();
		DataMgr.SaveSelectedWorldData();
		DataMgr.SaveWorldDataBackup();
		MusicMgr.Inst.ForcePlayMusic("");
		CamController.Inst.MouseOffsetPause();
		PlayerMgr.Inst.HideAndDisableControl();
		float num = Mathf.Pow(PlayerMgr.Inst.BaData.bodySize, 0.5f);
		sGraphic.transform.localScale *= num;
		rtsf_PlayerRoot.localPosition = GeneralTool.WorldToCanvasLocalPoint_FitDisplay(PlayerMgr.Inst.PlayerPoint);
		Time.timeScale = timeScale;
		string str = SEMgr.Inst.playerDead;
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_DaveHarpoons != null)
		{
			str = SEMgr.Inst.playerDead_Dave;
		}
		else
		{
			switch (DataMgr.selectedWorldData.playerLook)
			{
			case PlayerLook.PrettyGril:
			case PlayerLook.Nvliu:
			case PlayerLook.TapTap:
			case PlayerLook.HaoYou:
			case PlayerLook.MaoNiang:
			case PlayerLook.SummerGirl:
				str = SEMgr.Inst.playerDead_Girl;
				break;
			case PlayerLook.Frog:
				str = SEMgr.Inst.playerDead_Frog;
				break;
			case PlayerLook.Horse:
				str = SEMgr.Inst.playerDead_Horse;
				break;
			}
		}
		if (PlayerMgr.Inst.ItemCtrller.uiRelic_WarmSnow != null)
		{
			str = SEMgr.Inst.playerDead;
		}
		else if (PlayerMgr.Inst.ItemCtrller.relic_Reaper != null)
		{
			str = SEMgr.Inst.playerDead_Girl;
		}
		else if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			str = SEMgr.Inst.playerDead_Huang;
		}
		else if (PlayerMgr.Inst.ItemCtrller.uiRelic_DaveHarpoons != null)
		{
			str = SEMgr.Inst.playerDead_Dave;
		}
		str.PlaySE();
		PlayerSkinMgr.Inst.SetSkin(sGraphic.Skeleton, DataMgr.selectedWorldData.playerLook, PlayerMgr.Inst.BaData.relicCfgs, ignoreDisableRelicSkin: false, inBattle: true);
		sGraphic.AnimationState.SetAnimation(0, "Dead", loop: false);
		if (PlayerMgr.Inst.ItemCtrller.relic_MirrorOfSoul != null)
		{
			sGraphic.Skeleton.FindSlot("tui_l").A = 0f;
			sGraphic.Skeleton.FindSlot("tui_r").A = 0f;
			sGraphic.Skeleton.FindSlot("xiaotui_l").A = 0f;
			sGraphic.Skeleton.FindSlot("xiaotui_r").A = 0f;
			sGraphic.Skeleton.FindSlot("xie_l").A = 0f;
			sGraphic.Skeleton.FindSlot("xie_r").A = 0f;
			sGraphic.Skeleton.FindSlot("bilibili_xie_l").A = 0f;
			sGraphic.Skeleton.FindSlot("bilibili_xie_r").A = 0f;
			sGraphic.Skeleton.FindSlot("Hand_L").A = 0f;
			sGraphic.Skeleton.FindSlot("Hand_R").A = 0f;
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_Reaper != null)
		{
			sGraphic.Skeleton.FindSlot("Hand_R").A = 0f;
		}
		StartCoroutine(CheckRelicHuang(num));
		if (GameMgr.InEndlessMode)
		{
			SteamLeadBoardManager.Inst.StartUploadEndlessScore(BattleMgr.Inst.EndlessCurrentLevel, OnGetMyRank);
		}
	}

	private void OnGetMyRank(int rank)
	{
		DataMgr.finishEndlessGameBuilds.MyBestRank = rank;
	}

	protected override void OnHide()
	{
	}

	private IEnumerator CheckRelicHuang(float finalBodySize)
	{
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			sGraphic.transform.localScale = Vector3.zero;
			if (anima_UIRelicHunagInSet == null)
			{
				anima_UIRelicHunagInSet = UnityEngine.Object.Instantiate(pfb_UIRelicHuangInSet, rtsf_PlayerRoot);
				anima_UIRelicHunagInSet.GetComponent<RectTransform>().anchoredPosition = uiRelicHuangInSetOffset;
			}
			anima_UIRelicHunagInSet.gameObject.SetActive(value: true);
		}
		else if (anima_UIRelicHunagInSet != null)
		{
			UnityEngine.Object.Destroy(anima_UIRelicHunagInSet.gameObject);
		}
		yield return null;
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			anima_UIRelicHunagInSet.transform.localScale = Vector3.one * (uiRelicHuangInSetInitialSize * finalBodySize);
			anima_UIRelicHunagInSet.Play("Dead", 0, 0f);
		}
	}

	private void Update()
	{
		switch (state)
		{
		case UIState.MoveToCenter:
			rtsf_PlayerRoot.anchoredPosition = Vector3.MoveTowards((Vector3)rtsf_PlayerRoot.anchoredPosition, (Vector3)centerLocalPoint, moveSpeed * Time.deltaTime);
			if (rtsf_PlayerRoot.anchoredPosition == centerLocalPoint)
			{
				state = UIState.WaitFinish;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UIState.Idle:
		case UIState.Show:
		case UIState.WaitFinish:
			break;
		}
	}

	private void _ChangeTimeScale()
	{
		Time.timeScale = 1f;
		GameMgr.Inst.DestroyAllTeammate();
		GameMgr.Inst.RecycleAllPool();
	}

	private void _MoveToCenter()
	{
		state = UIState.MoveToCenter;
		as_DeadMusic.Play();
	}

	private void ReloadScene()
	{
		GameMgr.Inst.ClearAllPool();
		CamController.Inst.MouseOffsetContinue();
		SetIsOpen(isOpen: false);
		TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
		SceneManager.LoadScene(ScriptableObjMgr.Inst.testCtrller.isBW ? "Battle" : "Camp");
	}

	private void _Finish()
	{
		if (GameMgr.InEndlessMode && BattleMgr.Inst.EndlessCurrentLevel != 0)
		{
			UIBattleMgr.Inst.PopoutEndlessFinishBuild(ReloadScene);
		}
		else
		{
			UIMgr.Inst.uiFade.Show(ReloadScene);
		}
	}

	public void _PlayerDisappear()
	{
		if (anima_UIRelicHunagInSet != null)
		{
			anima_UIRelicHunagInSet.gameObject.SetActive(value: false);
		}
	}
}
