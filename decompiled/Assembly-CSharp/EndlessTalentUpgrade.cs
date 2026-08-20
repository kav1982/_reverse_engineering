using UnityEngine;

[CreateAssetMenu(fileName = "EndlessTalentUpgradeSO", menuName = "ScriptableObjects/EndlessTalentUpgradeSO", order = 100)]
public class EndlessTalentUpgrade : ScriptableObject
{
	public TalentUpgaradeAttr[] supplyBox;

	public TalentUpgaradeAttr[] goodsExtraCount;

	public TalentUpgaradeAttr[] gallery;

	public TalentUpgaradeAttr[] finishCoin;

	public TalentUpgaradeAttr[] lockMachine;

	public TalentUpgaradeAttr[] hightLevelSpell;

	public TalentUpgaradeAttr[] processSpell;

	public TalentUpgaradeAttr[] maxHP;

	public TalentUpgaradeAttr[] extraDamage;
}
