using UnityEngine;

public class Curse_InjuredCantShoot : LayerCorrect
{
	[Space(50f)]
	public Animator anima;

	private CurseConfig curseCfg;

	public bool CanShoot { get; private set; } = true;


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

	public void Injured()
	{
		if (!CanShoot)
		{
			curseCfg.floatTimer = curseCfg.float1.result;
			return;
		}
		CanShoot = false;
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		curseCfg.floatTimer = curseCfg.float1.result;
		anima.SetTrigger("Appear");
	}
}
