using System.Collections;
using System.IO;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SteamRankingSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Text text_Name;

	public Text score;

	public Text rank;

	public Coroutine coroutine;

	public GameObject SelectFrame;

	private DifficultyType Difficulty;

	public UGCHandle_t _ugc;

	public void OnPointerEnter(PointerEventData eventData)
	{
		SelectFrame.gameObject.SetActive(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SelectFrame.gameObject.SetActive(value: false);
	}

	public void InitializeFriend(RankData rankdata, UGCHandle_t ugc, DifficultyType difficulty = DifficultyType.Easy, int _rank = 0)
	{
		Difficulty = difficulty;
		_ugc = ugc;
		text_Name.text = rankdata.name;
		rank.text = _rank.ToString();
		int num = 0;
		switch (difficulty)
		{
		case DifficultyType.Easy:
			num = rankdata.score;
			break;
		case DifficultyType.Normal:
			num = rankdata.scorehard;
			break;
		case DifficultyType.Hard:
			num = rankdata.scorenightmare;
			break;
		case DifficultyType.Nightmare1:
			num = rankdata.scoreNewNightmare1;
			break;
		case DifficultyType.Nightmare2:
			num = rankdata.scoreNewNightmare2;
			break;
		case DifficultyType.Nightmare3:
			num = rankdata.scoreNewNightmare3;
			break;
		}
		int num2 = num / 3600;
		int num3 = num % 3600 / 60;
		int num4 = num % 60;
		score.text = $"{num2:D2}:{num3:D2}:{num4:D2}";
	}

	public void OnClick()
	{
		if (!GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.IsOpen)
		{
			coroutine = StartCoroutine(DownloadUGC(Difficulty));
		}
	}

	public IEnumerator DownloadUGC(DifficultyType difficulty)
	{
		string Filename = "LeaderboardBuild" + difficulty;
		if (coroutine == null)
		{
			Debug.Log("开始下载UGC");
			SteamLeadBoardManager.Inst.downloadingUGC = true;
			SteamLeadBoardManager.Inst.downloadUGC(_ugc, Filename);
			GameUISingletonMono<UI_RankingList>.Inst.gameobject_loading.SetActive(value: true);
			while (SteamLeadBoardManager.Inst.downloadingUGC)
			{
				yield return new WaitForEndOfFrame();
			}
			Debug.Log("结束下载UGC");
			GameUISingletonMono<UI_RankingList>.Inst.gameobject_loading.SetActive(value: false);
			if (SteamLeadBoardManager.Inst.downloadedUGCpath != "")
			{
				StreamReader streamReader = new StreamReader(Application.persistentDataPath + "\\" + Filename);
				string value = streamReader.ReadToEnd();
				streamReader.Close();
				FinishGameBuild build = JsonConvert.DeserializeObject<FinishGameBuild>(value);
				GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.gameObject.SetActive(value: true);
				GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.UpdateBuildInfoFinishBattle(build, UIFinishBuildShow.RecordUIFrom.RankingListSteam, -1, score.text);
				GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.Show();
				coroutine = null;
				SteamLeadBoardManager.Inst.downloadedUGCpath = "";
			}
			else
			{
				Debug.Log("下载UGC失败");
			}
		}
	}
}
