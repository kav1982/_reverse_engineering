using UnityEngine;

public class Curse_RandomCurse : MonoBehaviour
{
	public CurseConfig CurseCfg { get; private set; }

	public int RandomCurseID { get; private set; }

	private void GetRandomCurseID()
	{
		int num = 0;
		while (true)
		{
			num++;
			if (num > 100)
			{
				RandomCurseID = 999;
				Debug.Log("没Roll到合适的");
				break;
			}
			RandomCurseID = PlayerMgr.Inst.BaData.GetCurseFromPool(CurseCfg.dropType);
			if (RandomCurseID != CurseCfg.id && RandomCurseID != 38 && RandomCurseID != 61)
			{
				break;
			}
			PlayerMgr.Inst.BaData.BackCurseToPool(RandomCurseID, 1);
		}
	}

	public void Initialize(CurseConfig curseCfg)
	{
		if (curseCfg.abilityType != CurseAbilityType.RandomCurseCommon && curseCfg.abilityType != CurseAbilityType.RandomCurseRare)
		{
			Debug.LogError("传入的cfg只能是上述2个类型");
			return;
		}
		CurseCfg = curseCfg;
		PlayerMgr.Inst.ItemCtrller.CurseRemoveByID(curseCfg.id, 1, textFloat: false);
		GetRandomCurseID();
		PlayerMgr.Inst.ItemCtrller.CurseAdd(RandomCurseID, textFloat: false);
	}

	public void RerollCurse()
	{
		int randomCurseID = RandomCurseID;
		RandomCurseID = 0;
		PlayerMgr.Inst.ItemCtrller.CurseRemoveByID(randomCurseID, 1, textFloat: false);
		GetRandomCurseID();
		PlayerMgr.Inst.ItemCtrller.CurseAdd(RandomCurseID, textFloat: true);
	}

	public void CheckRemoveCurseIsMyCurse(int curseID)
	{
		if (RandomCurseID == curseID)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
