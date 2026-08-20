using UnityEngine;

public class Curse_CantShootEnterRoom : LayerCorrect
{
	[Space(50f)]
	public Animator anima;

	private CurseConfig curseCfg;

	public bool CanShoot { get; private set; }

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!CanShoot)
		{
			base.transform.position = PlayerMgr.Inst.PlayerPoint;
			curseCfg.floatTimer -= PlayerMgr.Inst.PlayerDeltaTime;
			if (curseCfg.floatTimer <= 0f)
			{
				CanShoot = true;
				anima.SetTrigger("Disappear");
			}
		}
	}

	public void Initialize(CurseConfig curseCfg)
	{
		this.curseCfg = curseCfg;
	}

	public void EnterDoor()
	{
		CanShoot = false;
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		curseCfg.floatTimer = curseCfg.float1.result;
		anima.SetTrigger("Appear");
	}
}
