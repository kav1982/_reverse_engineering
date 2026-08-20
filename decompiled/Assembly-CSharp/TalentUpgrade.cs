using UnityEngine;

[CreateAssetMenu(fileName = "TalentUpgradeSO", menuName = "ScriptableObjects/TalentUpgradeSO", order = 100)]
public class TalentUpgrade : ScriptableObject
{
	public TalentUpgaradeAttr[] wandLimit;

	public TalentUpgaradeAttr[] bagLimit;

	public TalentUpgaradeAttr[] maxHP;

	public TalentUpgaradeAttr[] initialCoin;

	public int unlock1Cost;

	public TalentUpgaradeAttr[] relicRoom;

	public TalentUpgaradeAttr[] coinRoom;

	public int unlock2Cost;

	public TalentUpgaradeAttr[] hpRoom;

	public TalentUpgaradeAttr[] spellRoom;

	public int unlock3Cost;

	public TalentUpgaradeAttr[] maxMP;

	public TalentUpgaradeAttr[] mpRecover;
}
