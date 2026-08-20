using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIArchiveSlot : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	public Text text_Empty;

	public GameObject panel_Data;

	public GameObject go_Blood;

	public GameObject go_Core;

	public Text text_Crystal;

	public Text text_difficulty;

	public Text text_Blood;

	public Text text_Core;

	public Text text_Time;

	public Text text_Location_Desc;

	public Text text_difficulty_Desc;

	public Text text_Crystal_Desc;

	public Text text_Blood_Desc;

	public Text text_Core_Desc;

	public Text text_Time_Desc;

	public RectTransform imageCrystal;

	public RectTransform imageBlood;

	public RectTransform imageCore;

	public Button btn_Delete;

	public GameObject go_Select;

	public GameObject go_SelectDelete;

	public UIArchive uiArchive;

	public float imageLayoutWidth = 30f;

	public float textImageLayoutWitdh = -20f;

	public float layoutInterval = 10f;

	public float[] languageOffsetsX;

	public RectTransform Infos;

	public Vector2 InfosOffset;

	public int DataIndex { get; set; }

	public bool IsSelected => go_Select.activeSelf;

	public bool IsDeleteSelected => go_SelectDelete.activeSelf;

	public WorldData SelfData
	{
		get
		{
			if (DataIndex == 0)
			{
				return DataMgr.WorldData0;
			}
			if (DataIndex == 1)
			{
				return DataMgr.WorldData1;
			}
			if (DataIndex == 2)
			{
				return DataMgr.WorldData2;
			}
			return DataMgr.WorldData0;
		}
	}

	private void OnEnable()
	{
		EventMgr.LanguageChange = (Action)Delegate.Combine(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	private void OnDisable()
	{
		EventMgr.LanguageChange = (Action)Delegate.Remove(EventMgr.LanguageChange, new Action(LanguageChange));
	}

	private void LanguageChange()
	{
		UpdateInfoLanguangeAndLayout();
	}

	public void Initialize()
	{
		UpdateInfoLanguangeAndLayout();
	}

	public void Select()
	{
		go_Select.SetActive(value: true);
		go_SelectDelete.SetActive(value: false);
	}

	public void SelectDelete()
	{
		go_Select.SetActive(value: false);
		go_SelectDelete.SetActive(value: true);
	}

	public void Unselect()
	{
		go_Select.SetActive(value: false);
		go_SelectDelete.SetActive(value: false);
	}

	public void UpdateInfoLanguangeAndLayout()
	{
		text_Empty.text = 1000008.GetText();
		text_Crystal.text = 1000010.GetText() + ":";
		text_Blood.text = 1000011.GetText() + ":";
		text_Time.text = 1000012.GetText() + ":";
		text_Core.text = 1000020.GetText() + ":";
		if (SelfData.inBattle9)
		{
			switch (SelfData.battleData9.currentStage)
			{
			case 1:
			case 2:
				text_Location_Desc.text = 1001702.GetText();
				break;
			case 3:
			case 4:
				text_Location_Desc.text = 1001703.GetText();
				break;
			case 5:
			case 6:
				text_Location_Desc.text = 1001704.GetText();
				break;
			case 7:
			case 8:
				text_Location_Desc.text = 1001705.GetText();
				break;
			case 9:
			case 10:
				text_Location_Desc.text = 1001706.GetText();
				break;
			case 300:
				text_Location_Desc.text = 1001707.GetText();
				break;
			default:
				Debug.LogError(SelfData.battleData9.currentStage);
				break;
			}
		}
		else
		{
			text_Location_Desc.text = 1001701.GetText();
		}
		if (SelfData.haveUsed)
		{
			text_Empty.gameObject.SetActive(value: false);
			panel_Data.SetActive(value: true);
			btn_Delete.interactable = true;
			text_difficulty.text = 1000022.GetText() + ":";
			if (SelfData.finishedDifficulty.Count == 0)
			{
				text_difficulty_Desc.text = 1000025.GetText();
			}
			else if (SelfData.finishedDifficulty.Count == 1)
			{
				text_difficulty_Desc.text = 1002601.GetText();
			}
			else if (SelfData.finishedDifficulty.Count == 2)
			{
				text_difficulty_Desc.text = 1002602.GetText();
			}
			else if (SelfData.finishedDifficulty.Count == 3)
			{
				text_difficulty_Desc.text = 1002603.GetText();
			}
			else if (SelfData.finishedDifficulty.Count == 4)
			{
				text_difficulty_Desc.text = 1002605.GetText();
			}
			else if (SelfData.finishedDifficulty.Count == 5)
			{
				text_difficulty_Desc.text = 1002606.GetText();
			}
			else if (SelfData.finishedDifficulty.Count == 6)
			{
				text_difficulty_Desc.text = 1002607.GetText();
			}
			text_Crystal_Desc.text = SelfData.GetHistoryGetCrystal().ToString();
			if (SelfData.hadBlood || SelfData.GetHistoryGetBlood() > 0)
			{
				go_Blood.SetActive(value: true);
				text_Blood_Desc.text = SelfData.GetHistoryGetBlood().ToString();
			}
			else
			{
				go_Blood.SetActive(value: false);
			}
			if (SelfData.hadCore || SelfData.GetHistoryGetCore() > 0)
			{
				go_Core.SetActive(value: true);
				text_Core_Desc.text = SelfData.GetHistoryGetCore().ToString();
			}
			else
			{
				go_Core.SetActive(value: false);
			}
			int num = (int)(SelfData.playTime / 3600f);
			int num2 = (int)(SelfData.playTime % 3600f / 60f);
			text_Time_Desc.text = num + 1000013.GetText() + " " + num2 + 1000014.GetText();
		}
		else
		{
			text_Empty.gameObject.SetActive(value: true);
			panel_Data.SetActive(value: false);
			btn_Delete.interactable = false;
		}
		if (languageOffsetsX.Length > (int)DataMgr.settingData.language)
		{
			Infos.anchoredPosition = new Vector2(languageOffsetsX[(int)DataMgr.settingData.language], 0f) + InfosOffset;
			return;
		}
		Debug.LogError("当前语言没有设定偏移");
		Infos.anchoredPosition = InfosOffset;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!UIMainMenuMgr.Inst.uiArchive.canInteract)
		{
			return;
		}
		SEMgr.Inst.uiClick.PlaySE();
		if (go_Select.activeSelf)
		{
			if (SelfData.haveUsed)
			{
				SelfData.PlayTimeRecord();
				if (SelfData.timeStampOnStartUsing == 0L)
				{
					SelfData.timeStampOnStartUsing = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
					DataMgr.SaveWorldData(SelfData);
				}
				UIMainMenuMgr.Inst.uiArchive.CanvasGroup.interactable = false;
				UIMainMenuMgr.Inst.uiArchive.canInteract = false;
				DataMgr.SetSelectedWorldData(DataIndex);
				if (GameMgr.IsMobile_Static)
				{
					Debug.Log("----------登录----------");
					MobileMgr.inst.PluginActivity.UploadUserSnapshot(1);
				}
				UIMgr.Inst.uiFade.Show(delegate
				{
					UIMainMenuMgr.Inst.uiArchive.CanvasGroup.interactable = true;
					UIMainMenuMgr.Inst.uiArchive.canInteract = true;
					if (DataMgr.selectedWorldData.inBattle9)
					{
						SceneManager.LoadScene("Battle");
					}
					else
					{
						DataMgr.selectedWorldData.directEnterCampByLoadSave = true;
						SceneManager.LoadScene("Camp");
					}
				});
			}
			else
			{
				UIMainMenuMgr.Inst.uiArchive.Panel_Skip.SetActive(value: true);
			}
		}
		else
		{
			uiArchive.SlotOnClick(DataIndex);
		}
	}

	public void _DeleteData()
	{
		uiArchive.DeleteData(DataIndex);
	}

	public void NewGameSkip()
	{
		StartGame(skip: true);
	}

	public void NewGameDontSkip()
	{
		StartGame(skip: false);
	}

	private void StartGame(bool skip)
	{
		if (!UIMainMenuMgr.Inst.uiArchive.canInteract)
		{
			return;
		}
		SelfData.haveUsed = true;
		SelfData.timeStampOnStartUsing = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		SelfData.PlayTimeRecord();
		DataMgr.SetSelectedWorldData(DataIndex);
		UIMainMenuMgr.Inst.uiArchive.CanvasGroup.interactable = false;
		UIMainMenuMgr.Inst.uiArchive.canInteract = false;
		UIMgr.Inst.uiFade.Show(delegate
		{
			UIMainMenuMgr.Inst.uiArchive.CanvasGroup.interactable = true;
			UIMainMenuMgr.Inst.uiArchive.canInteract = true;
			if (skip)
			{
				SceneManager.LoadScene("Guide2");
			}
			else
			{
				SceneManager.LoadScene("Guide");
			}
		});
	}
}
