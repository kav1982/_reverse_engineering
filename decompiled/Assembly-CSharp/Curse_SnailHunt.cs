using Unity.Entities;
using UnityEngine;

public class Curse_SnailHunt : UnitBase
{
	private enum CurseState
	{
		Idle,
		Move,
		Sleep
	}

	[Space(50f)]
	public Transform tsf_Modle;

	public Animator anima;

	public float moveSpeed;

	public float checkPathInterval;

	public VariableFloat refreshDistance;

	private CurseState state;

	private float checkPathIntervalTimer;

	private float damageDistanceSqr;

	private TakeDamageInfo_Dots damageInfo;

	private void Start()
	{
		EnterDoor();
		damageInfo = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
		damageInfo.damage = 6f;
	}

	public override void Update()
	{
		if (state == CurseState.Sleep)
		{
			return;
		}
		if (!PlayerMgr.Inst.PlayerCtrller.IsVisible)
		{
			if (state != 0)
			{
				state = CurseState.Idle;
				anima.Play("Idle");
			}
			return;
		}
		if (state != CurseState.Move)
		{
			state = CurseState.Move;
			anima.Play("Move");
		}
		checkPathIntervalTimer += Time.deltaTime;
		if (checkPathIntervalTimer >= checkPathInterval)
		{
			checkPathIntervalTimer = 0f;
			GetNavInfo(PlayerMgr.Inst.PlayerPoint);
		}
		Vector3 vector = ToPointDir(navInfo.ToGoPoint);
		SetMove(vector * moveSpeed);
		tsf_Modle.localScale = new Vector3((!(vector.x < 0f)) ? 1 : (-1), 1f, 1f);
		if (base.CurrentMotion != Vector3.zero)
		{
			base.transform.Translate(base.CurrentMotion * Time.deltaTime, Space.World);
		}
		if (ToPointDistanceSqr(PlayerMgr.Inst.PlayerPoint) <= damageDistanceSqr)
		{
			Entity targetEtt = PlayerMgr.Inst.PlayerEtt;
			UnitDotsSyncSystem.TryAttackEntity(in targetEtt, in damageInfo, World.DefaultGameObjectInjectionWorld.EntityManager);
		}
	}

	public void EnterDoor()
	{
		checkPathIntervalTimer = 0f;
		damageDistanceSqr = PlayerMgr.Inst.PlayerPpt.CC_Self.radius * PlayerMgr.Inst.PlayerT.localScale.x * PlayerMgr.Inst.PlayerPpt.CC_Self.radius * PlayerMgr.Inst.PlayerT.localScale.x;
		base.transform.position = Tool2D.GetNavMeshPointIngoreZ(PlayerMgr.Inst.PlayerPoint, refreshDistance);
		GetNavInfo(PlayerMgr.Inst.PlayerPoint);
		if (LevelMgr.Inst.CurrentRoomCfg.id == 201 || LevelMgr.Inst.CurrentRoomCfg.id == 203 || LevelMgr.Inst.CurrentRoomCfg.id == 205 || LevelMgr.Inst.CurrentRoomCfg.id == 207 || LevelMgr.Inst.CurrentRoomCfg.id == 202 || LevelMgr.Inst.CurrentRoomCfg.id == 204 || LevelMgr.Inst.CurrentRoomCfg.id == 206 || LevelMgr.Inst.CurrentRoomCfg.id == 208 || LevelMgr.Inst.CurrentRoomCfg.id == 211 || LevelMgr.Inst.CurrentRoomCfg.id == 213 || LevelMgr.Inst.CurrentRoomCfg.id == 215 || LevelMgr.Inst.CurrentRoomCfg.id == 217 || LevelMgr.Inst.CurrentRoomCfg.id == 212 || LevelMgr.Inst.CurrentRoomCfg.id == 214 || LevelMgr.Inst.CurrentRoomCfg.id == 216 || LevelMgr.Inst.CurrentRoomCfg.id == 218 || LevelMgr.Inst.CurrentRoomCfg.id == 231 || LevelMgr.Inst.CurrentRoomCfg.id == 232 || LevelMgr.Inst.CurrentRoomCfg.id == 233 || LevelMgr.Inst.CurrentRoomCfg.id == 234 || LevelMgr.Inst.CurrentRoomCfg.id == 235)
		{
			state = CurseState.Sleep;
			anima.Play("Sleep");
		}
		else
		{
			state = CurseState.Idle;
			anima.Play("Idle");
		}
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
