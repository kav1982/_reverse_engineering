using System;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIEndlessBattle")]
public class UIEndlessBattle : GameUISingletonMono<UIEndlessBattle>
{
	public GameObject show;

	public Text nowStage;

	public Text nowTime;

	public Text damageReduce;

	public Image coinCollector;

	public Animator Anima;

	public Text infoText;

	private bool bagOffsetActivated;

	private bool bossUIHide;

	[Header("打开背包偏移")]
	public RectTransform showRect;

	public float openBagOffsetX;

	private bool checkBagOffest;

	[Header("boss血条偏移")]
	public float bossHpOffsetX;

	public Vector3 GetCoinPoint()
	{
		return Tool2D.IgnoreZPoint(coinCollector.rectTransform.position + Vector3.up * 100f + CamController.Inst.transform.position);
	}

	protected override void OnShow(object obj = null)
	{
		show.SetActive(value: true);
		checkBagOffest = true;
	}

	protected override void OnHide()
	{
		show.SetActive(value: false);
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	private void Start()
	{
		EventMgr.EndlessStageStart = (Action)Delegate.Combine(EventMgr.EndlessStageStart, new Action(UpdateWaveInfo));
		EventMgr.EndlessStageClear = (Action)Delegate.Combine(EventMgr.EndlessStageClear, new Action(StageClear));
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		EventMgr.EndlessStageStart = (Action)Delegate.Remove(EventMgr.EndlessStageStart, new Action(UpdateWaveInfo));
		EventMgr.EndlessStageClear = (Action)Delegate.Remove(EventMgr.EndlessStageClear, new Action(StageClear));
	}

	private void UpdateWaveInfo()
	{
		float num = SpecialObj301EndlessMonsterSpawner.Inst.percentDamageReduce * 100f;
		bool flag = Mathf.Floor(num) != num;
		damageReduce.text = "百分比减伤:" + (flag ? num.ToString("F2") : num.ToString()) + "%";
	}

	private void StageClear()
	{
		infoText.text = 1007201.GetText();
		Anima.Play("EndlessStageClear", 0, 0f);
	}

	public void NewLevelWandHint()
	{
		infoText.text = 1007202.GetText();
		Anima.Play("EndlessStageClear", 0, 0f);
	}

	public void ProcessEnableHint()
	{
		infoText.text = 1007203.GetText();
		Anima.Play("EndlessStageClear", 0, 0f);
	}

	private void CheckBagOffset()
	{
		if (bagOffsetActivated || ((bool)GameUISingletonMono<UIBossHP>.Inst && GameUISingletonMono<UIBossHP>.Inst.IsOpen))
		{
			showRect.anchorMax = new Vector2(1f, 1f);
			showRect.anchorMin = new Vector2(1f, 1f);
			showRect.anchoredPosition = new Vector2(openBagOffsetX, 0f);
		}
		else
		{
			showRect.anchorMax = new Vector2(0.5f, 1f);
			showRect.anchorMin = new Vector2(0.5f, 1f);
			showRect.anchoredPosition = new Vector2(-100f, 0f);
		}
	}

	private void Update()
	{
		if (!GameMgr.InEndlessMode)
		{
			OnHide();
			return;
		}
		if (checkBagOffest)
		{
			CheckBagOffset();
			checkBagOffest = false;
		}
		if ((!GameMgr.IsMobile_Static && UIPlayerDataMgr.Inst.IsBagOpen != bagOffsetActivated) || ((bool)GameUISingletonMono<UIBossHP>.Inst && GameUISingletonMono<UIBossHP>.Inst.IsOpen != bossUIHide))
		{
			bagOffsetActivated = UIPlayerDataMgr.Inst.IsBagOpen;
			bossUIHide = GameUISingletonMono<UIBossHP>.Inst.IsOpen;
			checkBagOffest = true;
		}
		nowStage.text = "第" + BattleMgr.Inst.CurrentLevel + "波";
		nowTime.text = "⏱:" + Mathf.CeilToInt(SpecialObj301EndlessMonsterSpawner.Inst.RemainTime);
	}
}
