using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISettingMobileToggle : MonoBehaviour
{
	public Button Button;

	private bool on;

	public GameObject activateKnob;

	public Transform tsfActivateKnobActive;

	public Transform tsfActivateKnobDisactive;

	public Image ActivateFill;

	public Image Knob;

	public Image Disabtive;

	private float KnobAnimaTIme = 0.2f;

	public Color disactiveColor;

	public void SetToggle(bool state, bool anime = false)
	{
		if (state)
		{
			if (anime)
			{
				activateKnob.transform.position = tsfActivateKnobDisactive.position;
				activateKnob.transform.DOMove(tsfActivateKnobActive.position, KnobAnimaTIme).SetUpdate(isIndependentUpdate: true);
				ActivateFill.fillAmount = 0f;
				ActivateFill.DOFillAmount(1f, KnobAnimaTIme).SetUpdate(isIndependentUpdate: true);
			}
			else
			{
				activateKnob.transform.position = tsfActivateKnobActive.position;
				ActivateFill.fillAmount = 1f;
			}
		}
		else if (anime)
		{
			activateKnob.transform.position = tsfActivateKnobActive.position;
			activateKnob.transform.DOMove(tsfActivateKnobDisactive.position, KnobAnimaTIme).SetUpdate(isIndependentUpdate: true);
			ActivateFill.fillAmount = 1f;
			ActivateFill.DOFillAmount(0f, KnobAnimaTIme).SetUpdate(isIndependentUpdate: true);
		}
		else
		{
			activateKnob.transform.position = tsfActivateKnobDisactive.position;
			ActivateFill.fillAmount = 0f;
		}
	}

	public void DisabtiveToggle()
	{
		Button.interactable = false;
		Disabtive.color = disactiveColor;
		Knob.color = disactiveColor;
		ActivateFill.color = disactiveColor;
	}

	public void ActiveToggle()
	{
		Button.interactable = true;
		Disabtive.color = Color.white;
		Knob.color = Color.white;
		ActivateFill.color = Color.white;
	}
}
