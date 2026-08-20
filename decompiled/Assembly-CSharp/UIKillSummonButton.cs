using UnityEngine;

public class UIKillSummonButton : MonoBehaviour
{
	public GameObject button;

	public GameObject TapParticle;

	public static UIKillSummonButton Inst;

	public Animator ButtonAnimator;

	private void Start()
	{
		if (Inst == null)
		{
			Inst = this;
		}
	}

	private void Update()
	{
		button.SetActive(DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.CancelSummon) != 0);
	}

	private void KillSummonEffects()
	{
		TapParticle.GetComponent<ParticleSystem>().Play();
		ButtonAnimator.Play("Pressed");
	}

	public static void StaticKillSummon()
	{
		Debug.Log("StaticKillSummon");
		GameMgr.Inst.playerMgr.SummonsAllDead(instanceDeath: true, clearAllAutoWand: false);
		SEMgr.Inst.CancelAllTeammate.PlaySE(SEPlayMode.Replay, 3, 0.1f);
		if (Inst != null)
		{
			Inst.KillSummonEffects();
		}
	}
}
