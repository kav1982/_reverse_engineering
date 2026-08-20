using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageRecorde : MonoBehaviour
{
	private enum DamageChartType
	{
		CurrentRoom,
		Total
	}

	public int maxCount = 6;

	public Text battleTotalDamage;

	public Text currentRoomDamage;

	public float updateInterval = 0.1f;

	private float updateIntervalTimer;

	private DamageChartType _chartType;

	public Button totalDamageButton;

	public Button currentRoomDamageButton;

	public GameObject damageTypeButtonGroup;

	public GameObject totalDamageUI;

	public UIDamageRecordRank rank;

	private DamageChartType chartType
	{
		get
		{
			return _chartType;
		}
		set
		{
			_chartType = value;
			UpdateUI();
			totalDamageButton.interactable = value == DamageChartType.CurrentRoom;
			currentRoomDamageButton.interactable = value == DamageChartType.Total;
		}
	}

	private void UpdateUI()
	{
		damageTypeButtonGroup.SetActive(BattleMgr.Inst);
		totalDamageUI.SetActive(BattleMgr.Inst);
		if (!damageTypeButtonGroup.activeSelf && chartType != 0)
		{
			chartType = DamageChartType.CurrentRoom;
		}
		DamageRecorder recorde = chartType switch
		{
			DamageChartType.Total => DamageRecordeManager.historyDamageRecorder, 
			DamageChartType.CurrentRoom => DamageRecordeManager.currentDamageRecorder, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		rank.SetData(recorde, maxCount);
		battleTotalDamage.text = DamageRecordeManager.historyDamageRecorder.TotalDamage.ToString("N0");
		currentRoomDamage.text = DamageRecordeManager.currentDamageRecorder.TotalDamage.ToString("N0");
	}

	public void OnEnable()
	{
		UpdateUI();
	}

	private void Update()
	{
		if (updateIntervalTimer > 0f)
		{
			updateIntervalTimer -= Time.deltaTime;
			return;
		}
		updateIntervalTimer = updateInterval;
		UpdateUI();
	}

	public void ResetCurrentRoomDamage()
	{
		DamageRecordeManager.ClearCurrentRecorde();
		SEMgr.Inst.UIDamageRecordBoard.PlaySE(SEPlayMode.Replay, 3, 0.1f);
		UpdateUI();
	}

	public void SwitchToCurrentRoomDamageChart()
	{
		chartType = DamageChartType.CurrentRoom;
	}

	public void SwitchToTotalDamageChart()
	{
		chartType = DamageChartType.Total;
	}
}
