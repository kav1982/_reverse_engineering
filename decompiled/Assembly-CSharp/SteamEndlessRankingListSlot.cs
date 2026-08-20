using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SteamEndlessRankingListSlot : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Text text_Rank;

	[SerializeField]
	private Text text_Name;

	[SerializeField]
	private Text text_Score;

	[SerializeField]
	private GameObject SelectFrame;

	public void OnInitialize(SteamLeadBoardManager.EndlessLeaderboardEntryData data)
	{
		text_Name.text = data.steamName;
		text_Rank.text = data.globalRank.ToString();
		text_Score.text = data.score.ToString();
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
