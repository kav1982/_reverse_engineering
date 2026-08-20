using UnityEngine;

public class UIQuickPanelButton : MonoBehaviour
{
	public GameObject button;

	public GameObject TapParticle;

	public static UIQuickPanelButton Inst;

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
		button.SetActive(DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.DamageRecordBoard) > 0);
	}

	public void ShowQuickPanel()
	{
		GameUISingletonMono<UIQuickPanel>.Inst.Switch();
		PlayButtonPressedEffect();
	}

	public void PlayButtonPressedEffect()
	{
		TapParticle.GetComponent<ParticleSystem>().Play();
		ButtonAnimator.Play("Pressed");
	}
}
