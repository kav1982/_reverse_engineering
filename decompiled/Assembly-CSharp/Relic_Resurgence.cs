using System.Collections;
using Unity.Entities;
using UnityEngine;

public class Relic_Resurgence : LayerCorrect
{
	[Space(50f)]
	public Animator anima;

	public float timeScale;

	public float timeEffectDuration;

	public float timeFadeSpeed;

	public float focusSize;

	public float focusTime;

	public AudioSource as_Resurgence;

	[Header("DestroySelf")]
	public GameObject go_EF;

	public float destroySelfDelay;

	private RelicConfig relicCfg;

	public override void LateUpdate()
	{
		base.LateUpdate();
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
	}

	public void Initialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
	}

	public void TriggerResurgence()
	{
		anima.SetTrigger("Trigger");
		TimeScaleMgr.Inst.AddNewTimeScaleModifyRequest(timeScale, timeEffectDuration, timeFadeSpeed);
		CamController.Inst.FocusOn(focusSize, focusTime, PlayerMgr.Inst.PlayerPoint);
		PlayerMgr.Inst.InvincibleRegister();
		PlayerMgr.Inst.ImmuneKnockbackRegister();
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
		PlayerMgr.Inst.PlayerCtrller.SetEmoji(PlayerEmojiType.Normal);
		PlayerMgr.Inst.PlayerCtrller.SetBodyAnima(PlayerBodyAnima.Dead);
		if (as_Resurgence.volume != DataMgr.settingData.GetFinalSound())
		{
			as_Resurgence.volume = DataMgr.settingData.GetFinalSound();
		}
		as_Resurgence.Play();
	}

	public void DestroySelf()
	{
		StartCoroutine(DestroySelfIE());
	}

	private IEnumerator DestroySelfIE()
	{
		PlayerMgr.Inst.ItemCtrller.relic_Resurgence = null;
		base.transform.SetParent(null);
		go_EF.SetActive(value: true);
		yield return new WaitForSeconds(destroySelfDelay);
		Object.Destroy(base.gameObject);
	}

	private void _TriggerFinish()
	{
		CamController.Inst.FocusRecover(focusTime);
		PlayerMgr.Inst.InvincibleUnregister();
		PlayerMgr.Inst.ImmuneKnockbackUnregister();
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
		PlayerMgr.Inst.PlayerCtrller.SetBodyAnima(PlayerBodyAnima.GroundWalkDown);
		PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
		playerPpt.unitCfg.currentHP = playerPpt.unitCfg.maxHP / 100f * (float)relicCfg.int1.result;
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
		UIPlayerDataMgr.Inst.UpdateHP();
		PlayerMgr.Inst.ItemCtrller.RelicRemove(relicCfg.id);
		if (PlayerMgr.Inst.ItemCtrller.potion_Invincible == null)
		{
			PlayerMgr.Inst.ItemCtrller.potion_Invincible = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Invincible"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Invincible>();
		}
		PlayerMgr.Inst.ItemCtrller.potion_Invincible.Initialize(relicCfg.float1.result);
		if (PlayerMgr.Inst.ItemCtrller.relic_Huang != null)
		{
			PlayerMgr.Inst.ItemCtrller.relic_Huang.sr_Face.sprite = PlayerMgr.Inst.ItemCtrller.relic_Huang.sprite_FaceNormal;
			PlayerMgr.Inst.ItemCtrller.relic_Huang.AnimaBigSitIdle();
		}
	}
}
