using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocalEndlessRankingSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Text dateText;

	public Text levelText;

	private FinishEndlessGameBuild endlessBuild;

	public GameObject SelectFrame;

	public int buildindex;

	public void OnClick()
	{
		GameUISingletonMono<UIEndlessRankingList>.Inst.endlessFinishPanel.Show(delegate
		{
			if (!GameUISingletonMono<UIEndlessRankingList>.Inst.finishbuildshow.gameObject.activeSelf)
			{
				GameUISingletonMono<UIEndlessRankingList>.Inst.finishbuildshow.gameObject.SetActive(value: true);
				GameUISingletonMono<UIEndlessRankingList>.Inst.finishbuildshow.UpdateBuildInfoFinishBattle(endlessBuild.finishGameBuild, UIFinishBuildShow.RecordUIFrom.RankingListLocal, buildindex, "", localRankingList: true);
				GameUISingletonMono<UIEndlessRankingList>.Inst.finishbuildshow.Show();
			}
		}, endlessBuild, null, IsGameFinish: false, 4f);
	}

	public void initialize(FinishEndlessGameBuild newbuild, int index)
	{
		endlessBuild = newbuild;
		buildindex = index;
		dateText.text = GeneralTool.TimeStampToTime(endlessBuild.finishGameBuild.time.ToString(), DataMgr.settingData.language);
		levelText.text = endlessBuild.EndlessLevel.ToString();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SelectFrame.gameObject.SetActive(value: true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SelectFrame.gameObject.SetActive(value: false);
	}
}
