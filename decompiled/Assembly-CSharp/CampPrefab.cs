using System;
using System.Collections;
using UnityEngine;

public class CampPrefab : MonoBehaviour
{
	public GameObject[] pfb_CampGrasses;

	public Transform tsf_GrassParent;

	public AudioSource as_Water;

	private bool isInitialized;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Water.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Update()
	{
		if (!isInitialized && CampMgr.Inst != null && CampMgr.Inst.isDotsInitialized)
		{
			isInitialized = true;
			StartHandle();
		}
	}

	private void StartHandle()
	{
		StartCoroutine(StartHandleIE());
	}

	private IEnumerator StartHandleIE()
	{
		yield return new WaitForSeconds(0.1f);
		if (GameMgr.IsMobile_Static)
		{
			if (!GameMgr.IsMobile_Static || MobileMgr.inst.Generate_CampPlants)
			{
				for (int num = tsf_GrassParent.childCount - 1; num >= 0; num--)
				{
					Brittleness1 component = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Units/" + 30101), tsf_GrassParent.GetChild(num).position, Quaternion.identity).GetComponent<Brittleness1>();
					component.gameObject.SetActive(value: true);
					component.MarkCreateByOther();
					component.MarkImmuneDamage();
				}
				UnityEngine.Object.Destroy(tsf_GrassParent.gameObject);
			}
		}
		else
		{
			for (int num2 = tsf_GrassParent.childCount - 1; num2 >= 0; num2--)
			{
				Brittleness1 component2 = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Units/" + 30101), tsf_GrassParent.GetChild(num2).position, Quaternion.identity).GetComponent<Brittleness1>();
				component2.gameObject.SetActive(value: true);
				component2.MarkCreateByOther();
				component2.MarkImmuneDamage();
			}
			UnityEngine.Object.Destroy(tsf_GrassParent.gameObject);
		}
	}
}
