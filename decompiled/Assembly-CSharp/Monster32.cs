using Unity.Transforms;
using UnityEngine;

public class Monster32 : UnitBase, IRoomObjExtraData
{
	[Space(50f)]
	public int bodyCount;

	public float bodyInterval;

	public float bodyMoveSpeedRatio;

	public int invisibleID;

	public float closeLerp;

	[Header("Fly")]
	public VariableFloat flyRotateSpeed;

	public float flyLocateRadius;

	public VariableFloat flyRelocateInterval;

	private bool dragged;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	[Header("Pattern2")]
	public AIPattern pattern;

	public int boneDamage;

	public float boneSpeed;

	public float boneUpSpeed;

	public float boneGravity;

	public Monster32_Invisible invisiblePpt;

	private float flyRelocateIntervalTimer;

	private bool waitAttack;

	private float waitAttackDelay;

	private float waitAttackDelayTimer;

	private int finalBodyCount;

	public MeshRenderer mr;

	private int shaderCenterIndex = Shader.PropertyToID("_Center");

	public Sprite sprite_Head;

	public Sprite sprite_Body;

	public Sprite sprite_Tail;

	[Header("困难变异")]
	public Sprite sprite_HeadBorder;

	public MeshRenderer borderRenderer;

	public float nowSpeedBuff;

	[Header("和谐")]
	public Sprite sprite_Head_H;

	public Sprite sprite_Tail_H;

	public Sprite sprite_Body_H;

	public Sprite sprite_HeadBorder_H;

	private SpellSpawnParams ssp;

	public Monster32 Front { get; set; }

	public Vector3 CurrentDir { get; private set; }

	public Vector3 FlyPoint { get; private set; }

	private float MoveSpeedBuffed => base.MoveSpeed * nowSpeedBuff;

	private float flyRotateSpeedBuffed => flyRotateSpeed.result * nowSpeedBuff;

	public override void SingleInitialCallback()
	{
		if (pattern == AIPattern.Pattern1)
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90171);
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.Speed = spellSpeed;
			sSPModifier.Duration = spellDuration;
			sSPModifier.Damage = spellDamage;
			sSPModifier.Shooter = myPpt.myEntity;
			sSPModifier.ApplyToSSP(ref ssp);
		}
		else if (pattern == AIPattern.Pattern2)
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90021);
			UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in ssp);
			sSPModifier2.Speed = boneSpeed;
			sSPModifier2.CurrentFallSpeed = 0f - boneUpSpeed;
			sSPModifier2.Gravity = 0f - boneGravity;
			sSPModifier2.Damage = boneDamage;
			sSPModifier2.Shooter = myPpt.myEntity;
			sSPModifier2.ReboundCount = 100;
			sSPModifier2.ApplyToSSP(ref ssp);
		}
	}

	public override void EveryInitialCallback()
	{
		if (GameMgr.IsHarmony_Static)
		{
			sprite_Head = sprite_Head_H;
			sprite_Body = sprite_Body_H;
			sprite_Tail = sprite_Tail_H;
			sprite_HeadBorder = sprite_HeadBorder_H;
		}
		myPpt.RemoveMRFromArray(borderRenderer);
		Front = null;
		flyRelocateIntervalTimer = 0f;
		waitAttack = false;
		waitAttackDelay = 0f;
		waitAttackDelayTimer = 0f;
		SetMR(sprite_Body);
		finalBodyCount = bodyCount;
		flyRotateSpeed.RandomResult();
		if (borderRenderer != null)
		{
			borderRenderer.enabled = false;
			SetBorderMR(sprite_HeadBorder);
		}
		dragged = false;
	}

	private void SetMR(Sprite sprite)
	{
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite.texture);
		mr.transform.localScale = new Vector3((float)sprite.texture.width / sprite.pixelsPerUnit, (float)sprite.texture.height / sprite.pixelsPerUnit, 1f);
		Vector2 vector = -new Vector2(sprite.pivot.x / (float)sprite.texture.width, sprite.pivot.y / (float)sprite.texture.height) + Vector2.one * 0.5f;
		vector.x *= mr.transform.localScale.x;
		vector.y *= mr.transform.localScale.y;
		mr.transform.localPosition = vector;
	}

	private void SetBorderMR(Sprite sprite)
	{
		borderRenderer.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite.texture);
		borderRenderer.transform.localScale = new Vector3((float)sprite.texture.width / sprite.pixelsPerUnit, (float)sprite.texture.height / sprite.pixelsPerUnit, 1f);
		Vector2 vector = -new Vector2(sprite.pivot.x / (float)sprite.texture.width, sprite.pivot.y / (float)sprite.texture.height) + Vector2.one * 0.5f;
		vector.x *= borderRenderer.transform.localScale.x;
		vector.y *= borderRenderer.transform.localScale.y;
		borderRenderer.transform.localPosition = new Vector3(vector.x, vector.y, -0.001f);
	}

	public override void Frame1InitialCallback()
	{
		if (Front == null)
		{
			dragged = true;
			invisiblePpt = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + invisibleID).GetComponent<Monster32_Invisible>();
			invisiblePpt.BodyRegister(this);
			Monster32 front = this;
			for (int i = 0; i < finalBodyCount; i++)
			{
				Monster32 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + myPpt.unitCfg.id, base.transform.position).GetComponent<Monster32>();
				component.InitialBody(front, invisiblePpt);
				front = component;
				invisiblePpt.BodyRegister(component);
			}
			SetMR(sprite_Head);
			if (borderRenderer != null)
			{
				SetBorderMR(sprite_HeadBorder);
			}
			flyRelocateInterval.RandomResult();
			LocateFlyPoint();
		}
	}

	public override void Update()
	{
		if (Front != null)
		{
			if (!dragged)
			{
				if ((base.transform.position - Front.transform.position).sqrMagnitude > base.CC_Self.radius * base.CC_Self.radius * 4f)
				{
					dragged = true;
				}
			}
			else
			{
				Vector3 v = Vector3.Lerp(base.transform.position, Front.transform.position + (-Front.transform.position + base.transform.position).normalized * bodyInterval, closeLerp);
				if ((base.transform.position - Front.transform.position).sqrMagnitude < bodyInterval * bodyInterval)
				{
					base.transform.position = Tool2D.IgnoreZPoint(v);
				}
				else if ((base.transform.position - (Front.transform.position + (-Front.transform.position + base.transform.position).normalized * bodyInterval)).sqrMagnitude > Mathf.Pow(base.MoveSpeed * bodyMoveSpeedRatio * Time.deltaTime, 2f))
				{
					base.transform.position += (Front.transform.position - base.transform.position).normalized * base.MoveSpeed * bodyMoveSpeedRatio * Time.deltaTime;
				}
				else
				{
					base.transform.position = Tool2D.IgnoreZPoint(v);
				}
				LocalTransform componentData = GetComponentData<LocalTransform>();
				componentData.Position = base.transform.position;
				SetComponentData(componentData);
			}
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (Front != null && !Front.gameObject.activeSelf)
		{
			do
			{
				if (Front.Front == null)
				{
					FlyPoint = Front.FlyPoint;
					CurrentDir = Front.CurrentDir;
					SetMR(sprite_Head);
					if (borderRenderer != null)
					{
						SetBorderMR(sprite_HeadBorder);
					}
					Front = null;
					break;
				}
				Front = Front.Front;
			}
			while (!Front.gameObject.activeSelf);
		}
		if (waitAttack)
		{
			waitAttackDelayTimer += Time.deltaTime;
			if (waitAttackDelayTimer >= waitAttackDelay)
			{
				waitAttackDelayTimer = 0f;
				waitAttack = false;
				Vector3 oldDir = CurrentDir;
				if (Front != null)
				{
					oldDir = ToPointDir(Front.transform.position);
				}
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = Tool2D.GetDir(oldDir, 90f);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				sSPModifier.Direction = Tool2D.GetDir(oldDir, -90f);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
		if (Front == null)
		{
			flyRelocateIntervalTimer += Time.deltaTime;
			if (flyRelocateIntervalTimer >= flyRelocateInterval.result)
			{
				checkTargetIntervalTimer = 0f;
				flyRelocateInterval.RandomResult();
				LocateFlyPoint();
				flyRotateSpeed.RandomResult();
			}
			CurrentDir = Tool2D.DirMoveTowards(CurrentDir, ToPointDir(FlyPoint), flyRotateSpeedBuffed * Time.deltaTime);
			base.transform.Translate(CurrentDir * MoveSpeedBuffed * Time.deltaTime);
			LocalTransform componentData2 = GetComponentData<LocalTransform>();
			componentData2.Position = base.transform.position;
			SetComponentData(componentData2);
		}
	}

	public void SplitHeadReset()
	{
		FlyPoint = Front.FlyPoint;
		CurrentDir = Front.CurrentDir;
		SetMR(sprite_Head);
		if (borderRenderer != null)
		{
			SetBorderMR(sprite_HeadBorder);
		}
		Front = null;
	}

	private void LocateFlyPoint()
	{
		GetNearestTarget();
		if (base.HaveTarget)
		{
			FlyPoint = Tool2D.IgnoreZPoint(base.TargetPoint) + Tool2D.GetDir() * Random.Range(0f, flyLocateRadius);
		}
		else
		{
			FlyPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Tool2D.GetDir() * Random.Range(0f, flyLocateRadius);
		}
	}

	public void InitialBody(Monster32 front, Monster32_Invisible invisiblePpt)
	{
		Front = front;
		this.invisiblePpt = invisiblePpt;
	}

	public void Attack(float delay)
	{
		waitAttack = true;
		waitAttackDelay = delay;
	}

	public void ChangeToTail()
	{
		SetMR(sprite_Tail);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (invisiblePpt == null)
		{
			Frame1Initial();
			invisiblePpt.BodyUnregister(this, ref info);
		}
		else
		{
			invisiblePpt.BodyUnregister(this, ref info);
		}
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 >= 1f)
		{
			finalBodyCount = (int)data1;
		}
	}
}
