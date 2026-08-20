using UnityEngine;

public class Teammate3FuseBody : MonoBehaviour
{
	public SpriteRenderer idleSr;

	public SpriteRenderer attackSr;

	public GameObject fireIdleSr;

	public GameObject fireAttackSr;

	public GameObject normalAttackEffect;

	public GameObject voidAttackEffect;

	public Animator Anima;

	public Shadow SelfShadow;

	private AnimaEvent animaEvent;

	public Teammate3FuseController Controller { get; set; }

	private void Start()
	{
		if (Anima != null)
		{
			animaEvent = GetComponentInChildren<AnimaEvent>();
			if (animaEvent != null)
			{
				animaEvent.DoAction = AnimaAction;
			}
		}
	}

	public void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "AttackFinish")
			{
				Anima.SetTrigger("Idle");
				Anima.speed = 1f;
				SEMgr.Inst.teammate3Attack.PlaySE();
			}
			else
			{
				Debug.LogError(animaName);
			}
		}
		else if ((bool)Controller)
		{
			Controller.TentacleAttackEnemy(base.transform.position);
		}
	}
}
