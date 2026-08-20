using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocalRankingSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Text date;

	public Text timeuse;

	public Text Difficulty;

	private FinishGameBuild build;

	public GameObject SelectFrame;

	public int buildindex;

	public void OnClick()
	{
		if (!GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.IsOpen)
		{
			GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.gameObject.SetActive(value: true);
			GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.UpdateBuildInfoFinishBattle(build, UIFinishBuildShow.RecordUIFrom.RankingListLocal, buildindex, "", localRankingList: true);
			GameUISingletonMono<UI_RankingList>.Inst.finishbuildshow.Show();
		}
	}

	public void initialize(FinishGameBuild newbuild, int rank)
	{
		buildindex = rank - 1;
		build = newbuild;
		date.text = GeneralTool.TimeStampToTime(build.time.ToString(), DataMgr.settingData.language);
		int num = (int)build.timeuse / 3600;
		int num2 = (int)(build.timeuse % 3600f) / 60;
		int num3 = (int)build.timeuse % 60;
		timeuse.text = $"{num:D2}:{num2:D2}:{num3:D2}";
		switch (newbuild.Difficulty)
		{
		case 0:
			Difficulty.text = 1002601.GetText();
			break;
		case 1:
			Difficulty.text = 1002602.GetText();
			break;
		case 2:
			Difficulty.text = 1002603.GetText();
			break;
		case 3:
			Difficulty.text = 1002605.GetText();
			break;
		case 4:
			Difficulty.text = 1002606.GetText();
			break;
		case 5:
			Difficulty.text = 1002607.GetText();
			break;
		}
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
