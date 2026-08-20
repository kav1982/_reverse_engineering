using UnityEngine;
using UnityEngine.UI;

public class UiRelic_RuneWizard : MonoBehaviour
{
	public Text RedRuneCounterText;

	public Text GreenRuneCounterText;

	public Text BlueRuneCounterText;

	private float BonusCriticalChance;

	private int BonusHp;

	private int BonusMP;

	private int CurrentRecordBonusHp;

	private void LateUpdate()
	{
		UpdateRuneCounter();
	}

	public void UpdateRuneCounter()
	{
		(int, int, int) playerRuneCount = PlayerMgr.Inst.GetPlayerRuneCount();
		RedRuneCounterText.text = playerRuneCount.Item1.ToString();
		GreenRuneCounterText.text = playerRuneCount.Item2.ToString();
		BlueRuneCounterText.text = playerRuneCount.Item3.ToString();
		bool flag = PlayerMgr.Inst.ItemCtrller.uiRelic_RuneWizard != null;
		BonusCriticalChance = (flag ? ((float)playerRuneCount.Item1 * 1.5f) : 0f);
		BonusHp = (flag ? (playerRuneCount.Item2 * 3) : 0);
		BonusMP = (flag ? (playerRuneCount.Item3 * 5) : 0);
		if (CurrentRecordBonusHp != BonusHp)
		{
			PlayerMgr.Inst.ChangeHPMax(BonusHp - CurrentRecordBonusHp);
			CurrentRecordBonusHp = BonusHp;
		}
	}

	public void DestroySelf()
	{
		if (CurrentRecordBonusHp > 0)
		{
			PlayerMgr.Inst.ChangeHPMax(CurrentRecordBonusHp);
		}
		Object.Destroy(base.gameObject);
	}

	public int GetRuneWizardSetBonusHp()
	{
		return CurrentRecordBonusHp;
	}

	public int GetRuneWizardSetBonusMP()
	{
		return BonusMP;
	}

	public float GetRuneWizardSetBonusCriticalChance()
	{
		return BonusCriticalChance;
	}
}
