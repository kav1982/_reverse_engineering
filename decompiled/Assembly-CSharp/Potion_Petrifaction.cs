using System;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class Potion_Petrifaction : MonoBehaviour
{
	public float warningTime;

	public float ctrllerOutTime;

	public float shakeInterval;

	public float shakeDistance;

	public AudioSource as_Loop;

	public Text text;

	private PotionConfig potionCfg;

	private float durationTimer;

	private bool isWarning;

	private bool shakeLeft = true;

	private float shakeIntervalTimer;

	private float ctrllerOutTimer;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		if (as_Loop.volume != DataMgr.settingData.GetFinalSound())
		{
			as_Loop.volume = DataMgr.settingData.GetFinalSound();
		}
	}

	private void Update()
	{
		Vector3 vector = ControlMgr.Inst.GetInputWASD();
		if ((double)vector.x > 0.5 || (double)vector.x < -0.5 || (double)vector.y > 0.5 || (double)vector.y < -0.5)
		{
			vector = Vector3.one;
		}
		if (vector == Vector3.zero)
		{
			ctrllerOutTimer = 0f;
			as_Loop.Stop();
		}
		else
		{
			if (!as_Loop.isPlaying)
			{
				as_Loop.Play();
			}
			ctrllerOutTimer += PlayerMgr.Inst.PlayerDeltaTime;
			if (ctrllerOutTimer >= ctrllerOutTime)
			{
				Out();
			}
		}
		durationTimer += PlayerMgr.Inst.PlayerDeltaTime;
		if (durationTimer >= potionCfg.float1)
		{
			Out();
			return;
		}
		if (!isWarning && durationTimer >= potionCfg.float1 - warningTime)
		{
			isWarning = true;
		}
		if (isWarning || ctrllerOutTimer > 0f)
		{
			shakeIntervalTimer += PlayerMgr.Inst.PlayerDeltaTime;
			if (shakeIntervalTimer >= shakeInterval)
			{
				shakeIntervalTimer = 0f;
				if (shakeLeft)
				{
					PlayerMgr.Inst.PlayerT.position += new Vector3(shakeDistance, 0f, 0f);
					shakeLeft = false;
				}
				else
				{
					PlayerMgr.Inst.PlayerT.position += new Vector3(0f - shakeDistance, 0f, 0f);
					shakeLeft = true;
				}
			}
		}
		UpdateUI();
	}

	private void UpdateUI()
	{
		text.text = 1002017.GetText() + " " + Mathf.CeilToInt(potionCfg.float1 - durationTimer);
	}

	private void Out()
	{
		UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_PetrifactionOut"), base.transform.position, Quaternion.identity).GetComponent<AudioSource>().volume = DataMgr.settingData.GetFinalSound();
		SpriteRenderer[] componentsInChildren = PlayerMgr.Inst.PlayerT.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		MeshRenderer[] componentsInChildren2 = PlayerMgr.Inst.PlayerT.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		LineRenderer[] componentsInChildren3 = PlayerMgr.Inst.PlayerT.GetComponentsInChildren<LineRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetFloat("_PetrifactionLerp", 0f);
		}
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].material.SetFloat("_PetrifactionLerp", 0f);
		}
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].material.SetFloat("_PetrifactionLerp", 0f);
		}
		PlayerMgr.Inst.PlayerPpt.SAnima.timeScale = 1f;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		UnitProperty_Dots componentData = entityManager.GetComponentData<UnitProperty_Dots>(PlayerMgr.Inst.PlayerEtt);
		componentData.InvincibleUnregister();
		componentData.ImmuneKnockbackUnregister();
		entityManager.SetComponentData(PlayerMgr.Inst.PlayerEtt, componentData);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Initialize(PotionConfig potionCfg)
	{
		this.potionCfg = potionCfg;
		isWarning = false;
		durationTimer = 0f;
		SpriteRenderer[] componentsInChildren = PlayerMgr.Inst.PlayerT.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
		MeshRenderer[] componentsInChildren2 = PlayerMgr.Inst.PlayerT.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
		LineRenderer[] componentsInChildren3 = PlayerMgr.Inst.PlayerT.GetComponentsInChildren<LineRenderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].material.SetFloat("_PetrifactionLerp", 1f);
		}
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].material.SetFloat("_PetrifactionLerp", 1f);
		}
		for (int k = 0; k < componentsInChildren3.Length; k++)
		{
			componentsInChildren3[k].material.SetFloat("_PetrifactionLerp", 1f);
		}
		UpdateUI();
		PlayerMgr.Inst.PlayerPpt.SAnima.AnimationState.SetAnimation(0, PlayerMgr.Inst.PlayerPpt.SAnima.AnimationState.GetCurrent(0).Animation.Name, loop: true).TimeScale = 0f;
		PlayerMgr.Inst.PlayerPpt.SAnima.AnimationState.SetAnimation(1, PlayerMgr.Inst.PlayerPpt.SAnima.AnimationState.GetCurrent(1).Animation.Name, loop: true).TimeScale = 0f;
		if (PlayerMgr.Inst.ItemCtrller.relic_Fly != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Fly.PointerToPlayerThrougPotionPetrifaction();
		}
		if (PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_InvisibleWing.PointerToPlayerThrougPotionPetrifaction();
		}
	}

	public void OnDestroy()
	{
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.isUsingPetrifaction = false;
		}
	}
}
