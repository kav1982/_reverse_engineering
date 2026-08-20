using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleFinishDropMono : MonoBehaviour
{
	private enum SelfState
	{
		WaitInteract,
		DialogueFinishWait,
		Fade,
		Finish
	}

	public GameObject pfb_UIFade;

	public float focusSize;

	public float focusTime;

	public float waitTime;

	public float fadeTime;

	[Header("Difficulty")]
	public SpriteRenderer sr_Icon;

	public SpriteRenderer sr_Outline;

	public Sprite sprite_Easy;

	public Sprite sprite_EasyH;

	public Sprite sprite_Normal;

	public Sprite sprite_NormalH;

	public Sprite sprite_Hard;

	public Sprite sprite_HardH;

	private DifficultyType difficultyType;

	private SelfState state;

	private float waitTimer;

	private float fadeTimer;

	private void Update()
	{
		switch (state)
		{
		case SelfState.DialogueFinishWait:
			waitTimer += Time.deltaTime;
			if (waitTimer >= waitTime)
			{
				waitTimer = 0f;
				state = SelfState.Fade;
				UnityEngine.Object.Instantiate(pfb_UIFade, UIBattleMgr.Inst.rtsf_CanvasThings);
			}
			break;
		case SelfState.Fade:
			fadeTimer += Time.deltaTime;
			if (!(fadeTimer >= fadeTime) || UIBattleMgr.Inst.uiFinishBuildShow.IsOpen)
			{
				break;
			}
			UIBattleMgr.Inst.PopoutCurrentFinishBuild(delegate
			{
				GameMgr.Inst.DestroyAllTeammate();
				GameMgr.Inst.ClearAllPool();
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.PluginActivity.UploadItemSnapshot(2);
				}
				DataMgr.selectedWorldData.inBattle9 = false;
				switch (difficultyType)
				{
				case DifficultyType.Easy:
					DataMgr.selectedWorldData.storyKillChapter3BossPickup = true;
					SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishEasy);
					SceneManager.LoadScene("EasyFinishBackHome");
					break;
				case DifficultyType.Normal:
					DataMgr.selectedWorldData.storyHardBossDropPickup = true;
					SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishNormal);
					SceneManager.LoadScene("Camp");
					break;
				case DifficultyType.Hard:
					DataMgr.selectedWorldData.storyFinishHardDropPickup = true;
					SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.FinishHard);
					SceneManager.LoadScene("Camp");
					break;
				default:
					Debug.LogError(difficultyType);
					break;
				}
				DataMgr.selectedWorldData.BackCampCheckPlot();
				DataMgr.SaveSelectedWorldData();
				UIMgr.Inst.uiFade.Show(0f);
				Debug.Log(UIBattleMgr.Inst.uiFinishBuildShow.IsOpen);
				state = SelfState.Finish;
			});
			break;
		default:
			Debug.LogError(state);
			break;
		case SelfState.WaitInteract:
		case SelfState.Finish:
			break;
		}
	}

	public void Initialize(DifficultyType difficultyType)
	{
		this.difficultyType = difficultyType;
		switch (difficultyType)
		{
		case DifficultyType.Easy:
			sr_Icon.sprite = (GameMgr.IsChAge14_Static ? sprite_EasyH : sprite_Easy);
			break;
		case DifficultyType.Normal:
			sr_Icon.sprite = (GameMgr.IsChAge14_Static ? sprite_NormalH : sprite_Normal);
			break;
		case DifficultyType.Hard:
			sr_Icon.sprite = (GameMgr.IsChAge14_Static ? sprite_HardH : sprite_Hard);
			break;
		default:
			Debug.LogError(difficultyType);
			break;
		}
		sr_Outline.sprite = sr_Icon.sprite;
	}

	public void Select()
	{
		sr_Outline.gameObject.SetActive(value: true);
	}

	public void Unselect()
	{
		sr_Outline.gameObject.SetActive(value: false);
	}

	public void Interact()
	{
		int _hdID = 0;
		switch (difficultyType)
		{
		case DifficultyType.Easy:
			_hdID = 36;
			break;
		case DifficultyType.Normal:
			_hdID = 51;
			break;
		case DifficultyType.Hard:
			_hdID = 67;
			break;
		default:
			Debug.LogError(difficultyType);
			break;
		}
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.InvincibleRegister();
		UIPlayerDataMgr.Inst.Hide();
		CamController.Inst.FocusOn(focusSize, focusTime, base.transform.position);
		UIMgr.Inst.uiFilmBlackEdge.Show(focusTime, delegate
		{
			GameUISingletonMono<UIDialogueMgr>.Inst.HDShow(_hdID, (Action)delegate
			{
				PlayerMgr.Inst.PlayerCtrller.StartMotion();
				CamController.Inst.FocusRecover(focusTime);
				UIMgr.Inst.uiFilmBlackEdge.Hide();
				state = SelfState.DialogueFinishWait;
			});
		});
	}
}
