using UnityEngine;

public class Boss12_Show : UnitBase
{
	public ShockParam shockParam;

	public SpriteRenderer shadow;

	public GameObject gatlingInNest;

	public override void EveryInitialCallback()
	{
		myPpt.CC_Self.enabled = false;
		myPpt.CanTouch = false;
		gatlingInNest = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_Nest", base.transform.position + new Vector3(0f, 0f, 0.2f)).transform.GetChild(0).GetChild(0).gameObject;
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		myPpt.RemoveSRFromArray(shadow);
		shadow.color = new Color(0f, 0f, 0f, 0.4f);
		myPpt.InvincibleRegister();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "PickUpGatling":
			gatlingInNest.SetActive(value: false);
			break;
		case "AnimaEnd":
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/501201", base.transform.position);
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
			myPpt.InvincibleUnregister();
			myPpt.AnnouncedDeath();
			break;
		case "ShakeCamera":
			CamController.Inst.SetShock(shockParam);
			break;
		}
	}
}
