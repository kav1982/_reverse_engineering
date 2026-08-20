using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPotionUse : MonoBehaviour
{
	public Animator anima;

	public Image image_Outline;

	public Image image_Icon;

	public AudioSource as_UseLoop;

	public bool showing { get; private set; }

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		if (GameMgr.IsMobile_Static)
		{
			base.transform.GetChild(0).localScale = Vector3.one * 1.5f;
		}
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_UseLoop.volume = DataMgr.settingData.GetFinalSound();
	}

	public void Show(int potionID)
	{
		image_Outline.sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[potionID].GetIconPath());
		image_Icon.sprite = image_Outline.sprite;
		image_Outline.fillAmount = 0f;
		anima.SetTrigger("Show");
		as_UseLoop.Play();
		showing = true;
	}

	public void Hide()
	{
		showing = false;
		anima.SetTrigger("Hide");
		as_UseLoop.Stop();
	}

	public void SetOutline(float value)
	{
		image_Outline.fillAmount = value;
	}

	public void UseSuccess()
	{
		showing = false;
		anima.SetTrigger("Idle");
		ObjPoolMgr.Inst.GetGO("Prefabs/UI/CanvasPotionUseSuccess", base.transform.parent.position, 2f).GetComponentInChildren<Image>().sprite = image_Icon.sprite;
		as_UseLoop.Stop();
		SEMgr.Inst.potionUseFinish.PlaySE();
	}
}
