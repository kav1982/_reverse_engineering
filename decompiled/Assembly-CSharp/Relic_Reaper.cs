using System.Collections.Generic;
using System.Linq;
using Spine;
using UnityEngine;

public class Relic_Reaper : MonoBehaviour
{
	private Slot slotHandL;

	private Slot slotHandR;

	public RelicConfig RelicCfg { get; private set; }

	private void Update()
	{
		if (slotHandR.A != 0f)
		{
			slotHandL.A = 0f;
			slotHandR.A = 0f;
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		RelicCfg = relicCfg;
		UIPlayerDataMgr.Inst.BagBGToReaper();
		if (slotHandL == null)
		{
			slotHandL = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_L");
			slotHandR = PlayerMgr.Inst.PlayerCtrller.SAnima.skeleton.FindSlot("Hand_R");
		}
	}

	public void CompoundSpell()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
		{
			if (PlayerMgr.Inst.BaData.bagSpellDatas[i] == null || PlayerMgr.Inst.BaData.bagSpellDatas[i].isSealSlot || PlayerMgr.Inst.BaData.bagSpellDatas[i].id == 40202)
			{
				continue;
			}
			SpellConfig spellConfig = SpellConfig.dic[PlayerMgr.Inst.BaData.bagSpellDatas[i].id];
			if ((spellConfig.dropType == ItemDropType.Common || spellConfig.dropType == ItemDropType.Rare) && spellConfig.level < 3)
			{
				if (dictionary.Keys.Contains(spellConfig.id))
				{
					dictionary[spellConfig.id]++;
				}
				else
				{
					dictionary.Add(spellConfig.id, 1);
				}
			}
		}
		int num = 0;
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			int num2 = item.Value;
			num = 0;
			while (true)
			{
				num++;
				if (num >= 50)
				{
					Debug.LogError("死循环！");
					break;
				}
				if (num2 < 3)
				{
					break;
				}
				bool flag = true;
				for (int j = 0; j < 3; j++)
				{
					for (int k = 0; k < PlayerMgr.Inst.BaData.bagSpellDatas.Count; k++)
					{
						if (PlayerMgr.Inst.BaData.bagSpellDatas[k] != null && PlayerMgr.Inst.BaData.bagSpellDatas[k].id == item.Key && !PlayerMgr.Inst.BaData.bagSpellDatas[k].isSealSlot)
						{
							if (flag)
							{
								flag = false;
								PlayerMgr.Inst.BagSpellChange(k, new SlotData(item.Key + 1));
							}
							else
							{
								PlayerMgr.Inst.BagSpellChange(k, null);
							}
							break;
						}
					}
				}
				num2 -= 3;
			}
		}
	}

	public void DestroySelf()
	{
		slotHandL.A = 1f;
		slotHandR.A = 1f;
		UIPlayerDataMgr.Inst.BagBGToDefault();
		Object.Destroy(base.gameObject);
	}
}
