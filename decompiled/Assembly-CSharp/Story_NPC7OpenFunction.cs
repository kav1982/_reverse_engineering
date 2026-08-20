using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;

public class Story_NPC7OpenFunction : MonoBehaviour
{
	private enum StoryState
	{
		BornIdle,
		WaitTrigger,
		Story1_1,
		Story1PlayerMove,
		Story1_2,
		PlayerWalkToNPC7,
		WaitPlayerClose,
		Story2
	}

	public float bornIdleTime;

	public float playerTriggerYPoint;

	public float playerTriggerDistance;

	public PlayableDirector pd_Story;

	public Transform tsf_NPC7;

	public SkeletonAnimation sAnima_NPC7;

	public StoryPlayerChangeMaterial spcm;

	public Vector3 storyEndPlayerPoint;

	[Header("Hole and Focus")]
	public float focusSize;

	public float focusTime;

	public Vector3 focusPoint;

	public Vector3 focusPlayerMovePoint;

	public float focuPlayerMoveSpeed;

	[Header("Player Close To")]
	public Vector3 playerCloseToPoint;

	public float playerCloseToSpeed;

	public float playerCloseToDistanceNPC7Talk;

	public float playerCloseToFadeTime;

	public AudioSource as_;

	private StoryState state;

	private float bornIdleWaitTimer;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		BattleMgr.Inst.npc7.Hide();
		pd_Story.Stop();
		SoundVolumeChange();
		DataMgr.selectedWorldData.storyHardFinishNPC7OpenFunction = true;
		DataMgr.selectedWorldData.npc6ImportantPlot.SetNewState(65);
		if (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot.gameObject.SetActive(value: false);
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_DruidRing != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_DruidRing.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		switch (state)
		{
		case StoryState.BornIdle:
			bornIdleWaitTimer += Time.deltaTime;
			if (bornIdleWaitTimer >= bornIdleTime)
			{
				bornIdleWaitTimer = 0f;
				state = StoryState.WaitTrigger;
			}
			break;
		case StoryState.WaitTrigger:
		{
			if (!(PlayerMgr.Inst.PlayerPoint.y > playerTriggerYPoint) || !(Vector3.SqrMagnitude(tsf_NPC7.position - PlayerMgr.Inst.PlayerPoint) > playerTriggerDistance * playerTriggerDistance))
			{
				break;
			}
			state = StoryState.Story1_1;
			CamController.Inst.MouseOffsetPause();
			BattleMgr.Inst.npc7.MonsterAIPause();
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			PlayerMgr.Inst.PlayerCtrller.StopFace(isFlip: true);
			pd_Story.Play();
			if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
			{
				PlayerMgr.Inst.ItemCtrller.relic_Huang.PlotFaceLeft();
			}
			List<HideSelf> specifyPool = ObjPoolMgr.Inst.GetSpecifyPool("Prefabs/Spell/10161");
			if (specifyPool != null)
			{
				for (int i = 0; i < specifyPool.Count; i++)
				{
					specifyPool[i].GetComponent<Spell1016Dash>()?.PoolRecycle();
				}
			}
			MusicMgr.Inst.ForcePlayMusic("");
			break;
		}
		case StoryState.Story1PlayerMove:
		{
			Vector3 playerPoint3 = Vector3.MoveTowards(PlayerMgr.Inst.PlayerT.position, focusPlayerMovePoint, focuPlayerMoveSpeed * Time.deltaTime);
			PlayerMgr.Inst.SetPlayerPoint(playerPoint3);
			if (PlayerMgr.Inst.PlayerT.position == focusPlayerMovePoint)
			{
				state = StoryState.Story1_2;
				PlayerMgr.Inst.PlayerCtrller.SetBodyAnima(PlayerBodyAnima.GroundIdleDown);
			}
			break;
		}
		case StoryState.PlayerWalkToNPC7:
		{
			Vector3 playerPoint2 = Vector3.MoveTowards(PlayerMgr.Inst.PlayerT.position, playerCloseToPoint, playerCloseToSpeed * Time.deltaTime);
			PlayerMgr.Inst.SetPlayerPoint(playerPoint2);
			if (Vector3.SqrMagnitude(PlayerMgr.Inst.PlayerT.position - tsf_NPC7.position) < playerCloseToDistanceNPC7Talk * playerCloseToDistanceNPC7Talk)
			{
				state = StoryState.WaitPlayerClose;
				GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900602, tsf_NPC7);
			}
			break;
		}
		case StoryState.WaitPlayerClose:
		{
			Vector3 playerPoint = Vector3.MoveTowards(PlayerMgr.Inst.PlayerT.position, playerCloseToPoint, playerCloseToSpeed * Time.deltaTime);
			PlayerMgr.Inst.SetPlayerPoint(playerPoint);
			if (PlayerMgr.Inst.PlayerT.position == playerCloseToPoint)
			{
				state = StoryState.Story2;
				PlayerMgr.Inst.HideAndDisableControl();
				pd_Story.Play();
				StartCoroutine(WaitPlayerCloseRelicHuang());
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case StoryState.Story1_1:
		case StoryState.Story1_2:
		case StoryState.Story2:
			break;
		}
	}

	private IEnumerator WaitPlayerCloseRelicHuang()
	{
		yield return null;
		yield return null;
		spcm.RelicHuangLie();
	}

	public void _PlayerQuest()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900601, PlayerMgr.Inst.PlayerT);
	}

	public void _CamFocus()
	{
		state = StoryState.Story1PlayerMove;
		CamController.Inst.FocusOn(focusSize, focusTime, focusPoint);
		UIMgr.Inst.uiFilmBlackEdge.Show(focusTime);
		UIPlayerDataMgr.Inst.Hide();
		PlayerMgr.Inst.PlayerCtrller.SetBodyAnima(PlayerBodyAnima.GroundWalkDown);
	}

	public void _NPC7Appear()
	{
		NativeList<Entity> results = new NativeList<Entity>(Allocator.Temp);
		UnitDotsSyncSystem.GetAttackableEntitiesInRange(tsf_NPC7.position, 1f, UnitType.Player, containsBrittleness: true, ref results);
		if (results.Length > 0)
		{
			TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
			damageInfo.damage = 10f;
			foreach (Entity item in results)
			{
				Entity targetEtt = item;
				UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, World.DefaultGameObjectInjectionWorld.EntityManager);
			}
		}
		results.Dispose();
		BattleMgr.Inst.npc7.ChangeHoleBig();
	}

	public void _Talk()
	{
		pd_Story.Pause();
		sAnima_NPC7.AnimationState.SetAnimation(0, "Idle", loop: true);
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(63, (Action)delegate
		{
			state = StoryState.PlayerWalkToNPC7;
			CamController.Inst.MouseOffsetPause();
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			PlayerMgr.Inst.PlayerCtrller.SetBodyAnima(PlayerBodyAnima.GroundWalkDown);
		});
	}

	public void _SaySorry()
	{
		GameUISingletonMono<UIDialogueMgr>.Inst.SDShow(900603, tsf_NPC7);
	}

	public void _UIFadeShow()
	{
		UIMgr.Inst.uiFade.Show(playerCloseToFadeTime);
	}

	public void _UIFadeHide()
	{
		UnityEngine.Object.Destroy(BattleMgr.Inst.go_Guide);
		UIMgr.Inst.uiFade.Hide(playerCloseToFadeTime);
	}

	public void _LieUp()
	{
		spcm.RelicHuangLieUp();
	}

	public void _Explain()
	{
		pd_Story.Pause();
		GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(64, (Action)delegate
		{
			UIMgr.Inst.uiFade.Show(playerCloseToFadeTime, delegate
			{
				BattleMgr.Inst.npc7.Show();
				PlayerMgr.Inst.ShowAndEnableControl();
				PlayerMgr.Inst.SetPlayerPoint(storyEndPlayerPoint);
				CamController.Inst.FocusRecover(playerCloseToFadeTime);
				UIMgr.Inst.uiFade.Hide(playerCloseToFadeTime);
				PlayerMgr.Inst.PlayerCtrller.StartMotion();
				PlayerMgr.Inst.PlayerCtrller.StartMotion();
				MusicMgr.Inst.UpdateThemeMusic();
				if (PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot != null)
				{
					PlayerMgr.Inst.ItemCtrller.relic_RemoteShoot.gameObject.SetActive(value: true);
				}
				if (PlayerMgr.Inst.ItemCtrller.relic_DruidRing != null)
				{
					PlayerMgr.Inst.ItemCtrller.relic_DruidRing.gameObject.SetActive(value: true);
				}
				UIMgr.Inst.uiFilmBlackEdge.Hide(playerCloseToFadeTime, delegate
				{
					UIPlayerDataMgr.Inst.Show();
					BattleMgr.Inst.npc7.MonsterAIRecovery();
				});
				if (GameMgr.IsMobile_Static)
				{
					GameUISingletonMono<UIUnlockSystem>.ShowInit(UIUnlockSystem.UIUnlockSystemType.SpellDisable);
				}
				UnityEngine.Object.Destroy(base.gameObject);
			});
		});
	}
}
