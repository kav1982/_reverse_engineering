using System.Collections;
using UnityEngine;

public class Relic_InjuredAddMoveSpeed : LayerCorrect
{
	[Space(50f)]
	public GameObject go_StartEF;

	public Transform tsf_RotateEF;

	private RelicConfig relicCfg;

	private float timer;

	private bool isSpeedUp;

	public float MoveSpeedRatio { get; private set; } = 1f;


	private void Update()
	{
		if (isSpeedUp)
		{
			base.transform.position = PlayerMgr.Inst.PlayerPoint;
			_ = PlayerMgr.Inst.PlayerCtrller.CurrentMoveDir;
			tsf_RotateEF.up = PlayerMgr.Inst.PlayerCtrller.CurrentMoveDir;
			timer += PlayerMgr.Inst.PlayerDeltaTime;
			if (timer > relicCfg.float1.result)
			{
				timer = relicCfg.float1.result;
			}
			MoveSpeedRatio = 1f + (relicCfg.float1.result - timer) * (float)relicCfg.int1.result / 100f;
			if (timer >= relicCfg.float1.result)
			{
				isSpeedUp = false;
				MoveSpeedRatio = 1f;
				tsf_RotateEF.gameObject.SetActive(value: false);
			}
		}
	}

	public void Initialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
	}

	public void PlayerTakeDamage()
	{
		isSpeedUp = true;
		MoveSpeedRatio = 1f + (float)relicCfg.int1.result / 100f;
		timer = 0f;
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		go_StartEF.SetActive(value: false);
		go_StartEF.SetActive(value: true);
		tsf_RotateEF.gameObject.SetActive(value: true);
		SEMgr.Inst.relic_InjuredAddMoveSpeed.PlaySE();
		if (PlayerMgr.Inst.ItemCtrller.potion_Invincible == null)
		{
			PlayerMgr.Inst.ItemCtrller.potion_Invincible = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Item/Potion_Invincible"), PlayerMgr.Inst.PlayerPoint, Quaternion.identity, PlayerMgr.Inst.PlayerT).GetComponent<Potion_Invincible>();
		}
		PlayerMgr.Inst.ItemCtrller.potion_Invincible.Initialize(relicCfg.float1.result);
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveRegister();
		StartCoroutine(NonInteractiveUnregister());
	}

	private IEnumerator NonInteractiveUnregister()
	{
		yield return new WaitForSecondsRealtime(relicCfg.float1.result);
		PlayerMgr.Inst.PlayerCtrller.NonInteractiveUnregister();
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
