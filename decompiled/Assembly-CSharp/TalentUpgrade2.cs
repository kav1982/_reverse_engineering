using UnityEngine;

[CreateAssetMenu(fileName = "TalentUpgrade2SO", menuName = "ScriptableObjects/TalentUpgrade2SO", order = 100)]
public class TalentUpgrade2 : ScriptableObject
{
	public TalentUpgaradeAttr[] wandLimit;

	public TalentUpgaradeAttr[] bagLimit;

	public TalentUpgaradeAttr[] enterDoorRecovery;

	public int unlock1Cost;

	public TalentUpgaradeAttr[] maxHP;

	public TalentUpgaradeAttr[] hpRoom;

	public int unlock2Cost;

	public TalentUpgaradeAttr[] initialCoin;

	public TalentUpgaradeAttr[] coinRoom;

	public int unlock3Cost;

	public TalentUpgaradeAttr[] spellRoom;

	public TalentUpgaradeAttr[] relicRoom;

	public int unlock4Cost;

	public TalentUpgaradeAttr[] maxMP;

	public TalentUpgaradeAttr[] mpRecover;
}
