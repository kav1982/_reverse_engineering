using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMobileReturnAndRess : MonoBehaviour
{
	public Text text_Name;

	public CanvasGroup canvasGroup;

	public Button returnButton;

	public GameObject Core;

	public Text CoreText;

	public GameObject Crystal;

	public Text CrystalText;

	public GameObject Blood;

	public Text BloodText;

	public GameObject Coin;

	public Text CoinText;

	private GameUI gameUI;

	private bool firstSet;

	public void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			if (!firstSet)
			{
				HideImmediate();
			}
			gameUI = base.transform.GetComponentInParent<GameUI>();
			if (gameUI != null)
			{
				SetGameUIBind(gameUI);
			}
			EventMgr.ChaosCoreChange = (Action)Delegate.Combine(EventMgr.ChaosCoreChange, new Action(UpdateCoreCout));
			EventMgr.AncienBloodChange = (Action)Delegate.Combine(EventMgr.AncienBloodChange, new Action(UpdateBloodCount));
			EventMgr.MagicCrystalChange = (Action)Delegate.Combine(EventMgr.MagicCrystalChange, new Action(UpdateCrystalCount));
		}
		else
		{
			base.gameObject.SetActive(value: false);
			Debug.LogWarning("非手机的这个预制体忘关了?");
		}
	}

	private void SetGameUIBind(GameUI gameUI, bool overrideButtonListener = false)
	{
		this.gameUI = gameUI;
		if (returnButton.onClick.GetPersistentEventCount() == 0 || overrideButtonListener)
		{
			returnButton.onClick.RemoveAllListeners();
			returnButton.onClick.AddListener(delegate
			{
				gameUI._Close();
				SEMgr.Inst.uiSwitch.PlaySE(SEPlayMode.Unique);
			});
		}
		UpdateResShow();
	}

	private void OnDestroy()
	{
		EventMgr.ChaosCoreChange = (Action)Delegate.Remove(EventMgr.ChaosCoreChange, new Action(UpdateCoreCout));
		EventMgr.AncienBloodChange = (Action)Delegate.Remove(EventMgr.AncienBloodChange, new Action(UpdateBloodCount));
		EventMgr.MagicCrystalChange = (Action)Delegate.Remove(EventMgr.MagicCrystalChange, new Action(UpdateCrystalCount));
	}

	public void Show(GameUI gameUI = null, bool overrideButtonListener = false)
	{
		firstSet = true;
		if (gameUI != null)
		{
			SetGameUIBind(gameUI, overrideButtonListener);
		}
		canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	private void Update()
	{
		UpdateCoinCount();
	}

	public void Hide()
	{
		firstSet = true;
		canvasGroup.DOFade(0f, 0.5f).SetUpdate(isIndependentUpdate: true);
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void HideImmediate()
	{
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void UpdateResShow()
	{
		Core.SetActive(value: false);
		Crystal.SetActive(value: false);
		Blood.SetActive(value: false);
		Coin.SetActive(value: false);
		GameUI gameUI = this.gameUI;
		if (!(gameUI is UITalent))
		{
			if (!(gameUI is UIResearch))
			{
				if (!(gameUI is UISet))
				{
					if (!(gameUI is UIActivateGirl))
					{
						if (!(gameUI is UIResourceChanger))
						{
							if (!(gameUI is UISpellDisable))
							{
								if (!(gameUI is UIReroll))
								{
									if (!(gameUI is UISell))
									{
										if (!(gameUI is UICampMirror))
										{
											if (!(gameUI is UICampSkinChanger))
											{
												if (!(gameUI is UITraining))
												{
													if (!(gameUI is UICompound))
													{
														if (!(gameUI is UIMoreInOne))
														{
															if (!(gameUI is UIGallery))
															{
																if (!(gameUI is UISetting))
																{
																	if (!(gameUI is UIHandbook))
																	{
																		if (!(gameUI is UIMenu))
																		{
																			if (!(gameUI is UISpellDisableHistory))
																			{
																				if (!(gameUI is UIChapterThrough))
																				{
																					if (!(gameUI is UIProcessInOne_Controller))
																					{
																						if (!(gameUI is UIUnlockSystem))
																						{
																							if (gameUI is UIAchievement)
																							{
																								text_Name.text = "成就";
																							}
																						}
																						else
																						{
																							text_Name.text = "功能解锁";
																						}
																					}
																					else
																					{
																						switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
																						{
																						case UIProcessInOne_Controller.UIProcessInOneType.Compound:
																							text_Name.text = 1000901.GetText();
																							break;
																						case UIProcessInOne_Controller.UIProcessInOneType.Reroll:
																							text_Name.text = 1000904.GetText();
																							break;
																						case UIProcessInOne_Controller.UIProcessInOneType.MoreInOne:
																							text_Name.text = 1000906.GetText();
																							break;
																						case UIProcessInOne_Controller.UIProcessInOneType.RerollRelic:
																							text_Name.text = "重铸遗物";
																							break;
																						case UIProcessInOne_Controller.UIProcessInOneType.Sell:
																							text_Name.text = 1000910.GetText();
																							break;
																						default:
																							throw new ArgumentOutOfRangeException();
																						}
																						text_Name.text = 1002621.GetText();
																					}
																				}
																				else
																				{
																					text_Name.text = 1002621.GetText();
																				}
																			}
																			else
																			{
																				text_Name.text = 1003506.GetText();
																			}
																		}
																		else
																		{
																			text_Name.text = 1000147.GetText();
																		}
																	}
																	else
																	{
																		text_Name.text = 1000222.GetText();
																	}
																}
																else
																{
																	text_Name.text = 1000002.GetText();
																}
															}
															else
															{
																text_Name.text = 1000417.GetText();
															}
														}
														else
														{
															text_Name.text = 1000906.GetText();
														}
													}
													else
													{
														text_Name.text = 1000901.GetText();
													}
												}
												else
												{
													text_Name.text = 1002406.GetText();
												}
											}
											else
											{
												text_Name.text = 1004151.GetText();
											}
										}
										else
										{
											text_Name.text = 1000029.GetText();
										}
									}
									else
									{
										text_Name.text = 1000910.GetText();
										Coin.SetActive(value: true);
									}
								}
								else
								{
									text_Name.text = 1000904.GetText();
									Coin.SetActive(value: true);
								}
							}
							else
							{
								text_Name.text = 1003501.GetText();
								Core.SetActive(value: true);
								Crystal.SetActive(value: true);
								Blood.SetActive(value: true);
							}
						}
						else
						{
							text_Name.text = 1002111.GetText();
							Core.SetActive(value: true);
							Crystal.SetActive(value: true);
							Blood.SetActive(value: true);
						}
					}
					else
					{
						text_Name.text = 1003301.GetText();
						Core.SetActive(value: true);
					}
				}
				else
				{
					text_Name.text = 1002102.GetText();
					Blood.SetActive(value: true);
				}
			}
			else
			{
				text_Name.text = 1002101.GetText();
				Blood.SetActive(value: true);
			}
		}
		else
		{
			text_Name.text = 1000501.GetText();
			Crystal.SetActive(value: true);
			Blood.SetActive(value: true);
		}
		UpdateCoreCout();
		UpdateBloodCount();
		UpdateCrystalCount();
	}

	private void UpdateCoreCout()
	{
		if (CoreText.gameObject.activeInHierarchy)
		{
			CoreText.text = DataMgr.selectedWorldData.chaosCoreCount.ToString();
		}
	}

	private void UpdateBloodCount()
	{
		if (BloodText.gameObject.activeInHierarchy)
		{
			BloodText.text = DataMgr.selectedWorldData.ancientBloodCount.ToString();
		}
	}

	private void UpdateCrystalCount()
	{
		if (CrystalText.gameObject.activeInHierarchy)
		{
			CrystalText.text = DataMgr.selectedWorldData.magicCrystalCount.ToString();
		}
	}

	private void UpdateCoinCount()
	{
		if (Coin.gameObject.activeInHierarchy && DataMgr.selectedWorldData.battleData9 != null)
		{
			CoinText.text = DataMgr.selectedWorldData.battleData9.coinCount.ToString();
		}
	}
}
