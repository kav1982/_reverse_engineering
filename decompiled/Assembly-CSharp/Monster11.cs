using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class Monster11 : UnitBase, IRoomObjExtraData
{
	private enum UnitState
	{
		BornIdle,
		AppearIdle,
		Attack,
		AttackIdle,
		Disappear,
		DisappearIdle,
		Appear
	}

	[Space(50f)]
	public int bodyID;

	public int bodyCount;

	public float bodyInterval;

	public float bodyCorrectSpeed;

	public float headHPRatio;

	public float waveRange;

	public float waveSpeed;

	[Range(0f, 1f)]
	public float attackZScale;

	public float attackDistance;

	public float attackIdleTime;

	public float inOutSpeed;

	public VariableFloat disappearIdleTime;

	public VariableFloat appearIdleTime;

	[Header("和谐版")]
	public Sprite sprite;

	public Sprite sprite_Harmony;

	public MeshRenderer MR;

	private int originalBodyCount;

	private UnitState state;

	private List<Monster11_Body> bodys = new List<Monster11_Body>();

	private Monster11_Body head;

	private float waveValue;

	private bool isShrink;

	private float idleTimer;

	private float disappearIdleTimer;

	public Vector3 AbyssPoint { get; private set; }

	public override void SingleInitialCallback()
	{
		originalBodyCount = bodyCount;
		if (GameMgr.IsHarmony_Static)
		{
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Harmony.texture);
		}
		else
		{
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite.texture);
		}
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.BornIdle;
		waveValue = 0f;
		isShrink = false;
		idleTimer = 0f;
		disappearIdleTimer = 0f;
		myPpt.CanTouch = false;
		bodyCount = originalBodyCount;
	}

	public override void Frame1InitialCallback()
	{
		GetNearestAbyssPoint();
		myPpt.tsf_Layer.position = Tool2D.GetLayerPoint(AbyssPoint);
		bodys.Clear();
		for (int i = 0; i < bodyCount; i++)
		{
			Monster11_Body component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + bodyID, AbyssPoint + new Vector3(0f, 0f, -1f) * bodyInterval * i).GetComponent<Monster11_Body>();
			if (i == bodyCount - 1)
			{
				head = component;
				component.SetMother(this, isHead: true);
			}
			else
			{
				component.SetMother(this, isHead: false);
			}
			bodys.Add(component);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (bodys.Count > 0)
		{
			bodys[bodys.Count - 1].DotsAnnouncedDeath();
		}
	}

	public override void Update()
	{
		base.Update();
		waveValue += waveSpeed * Time.deltaTime;
		for (int i = 0; i < bodys.Count; i++)
		{
			Vector3 vector = AbyssPoint + new Vector3(0f, 0f, -1f) * bodyInterval * i;
			float num = Mathf.Sin(waveValue + vector.z) * waveRange;
			num *= Mathf.Clamp01(Mathf.Abs(vector.z + 0.2f) * 0.5f);
			bodys[i].transform.position = new Vector3(vector.x + num, vector.y, bodys[i].transform.position.z);
			LocalTransform componentData = bodys[i].GetComponentData<LocalTransform>();
			componentData.Position = bodys[i].transform.position;
			float num2 = vector.z;
			if (isShrink)
			{
				num2 *= attackZScale;
			}
			if (bodys[i].transform.position.z != num2)
			{
				Vector3 vector2 = new Vector3(bodys[i].transform.position.x, bodys[i].transform.position.y, num2);
				bodys[i].transform.position = Vector3.MoveTowards(bodys[i].transform.position, vector2, bodyCorrectSpeed * Time.deltaTime);
				componentData.Position = bodys[i].transform.position;
				if (bodys[i].transform.position == vector2 && i != 0)
				{
					bodys[i].Upspring();
				}
			}
			bodys[i].SetComponentData(componentData);
		}
		switch (state)
		{
		case UnitState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestTarget(AbyssPoint);
				if (base.HaveTarget && (base.TargetPoint - AbyssPoint).sqrMagnitude < attackDistance * attackDistance)
				{
					state = UnitState.Attack;
					head.SetAttack(targetEntity);
					isShrink = true;
				}
				else
				{
					state = UnitState.Disappear;
					myPpt.tsf_Layer.gameObject.SetActive(value: false);
				}
			}
			break;
		case UnitState.AttackIdle:
			idleTimer += Time.deltaTime;
			if (idleTimer >= attackIdleTime)
			{
				idleTimer = 0f;
				state = UnitState.Disappear;
				myPpt.tsf_Layer.gameObject.SetActive(value: false);
			}
			break;
		case UnitState.Disappear:
		{
			AbyssPoint += new Vector3(0f, 0f, inOutSpeed * Time.deltaTime);
			bool flag = true;
			for (int j = 0; j < bodys.Count; j++)
			{
				if (bodys[j].IsShow)
				{
					flag = false;
					if (bodys[j].transform.position.z >= 0f)
					{
						bodys[j].Hide();
					}
				}
			}
			if (flag)
			{
				state = UnitState.DisappearIdle;
				disappearIdleTime.RandomResult();
			}
			break;
		}
		case UnitState.DisappearIdle:
			disappearIdleTimer += Time.deltaTime;
			if (disappearIdleTimer >= disappearIdleTime.result)
			{
				disappearIdleTimer = 0f;
				state = UnitState.Appear;
				float z = AbyssPoint.z;
				GetRandomAbyssPoint();
				myPpt.tsf_Layer.position = Tool2D.GetLayerPoint(AbyssPoint, LayerCorrectType.Coordinate);
				AbyssPoint = new Vector3(AbyssPoint.x, AbyssPoint.y, z);
			}
			break;
		case UnitState.Appear:
		{
			AbyssPoint += new Vector3(0f, 0f, (0f - inOutSpeed) * Time.deltaTime);
			bool flag2 = true;
			for (int k = 0; k < bodys.Count; k++)
			{
				if (!bodys[k].IsShow)
				{
					flag2 = false;
					if (bodys[k].transform.position.z <= 0f)
					{
						bodys[k].Show();
					}
				}
			}
			if (flag2)
			{
				AbyssPoint = Tool2D.IgnoreZPoint(AbyssPoint);
				state = UnitState.AppearIdle;
				appearIdleTime.RandomResult();
				myPpt.tsf_Layer.gameObject.SetActive(value: true);
			}
			break;
		}
		case UnitState.AppearIdle:
			idleTimer += Time.deltaTime;
			if (idleTimer >= appearIdleTime.result)
			{
				idleTimer = 0f;
				GetNearestTarget(AbyssPoint);
				if (base.HaveTarget && (base.TargetPoint - AbyssPoint).sqrMagnitude < attackDistance * attackDistance)
				{
					state = UnitState.Attack;
					head.SetAttack(targetEntity);
					isShrink = true;
				}
				else
				{
					state = UnitState.Disappear;
					myPpt.tsf_Layer.gameObject.SetActive(value: false);
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Attack:
			break;
		}
	}

	private void GetNearestAbyssPoint()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots.Count == 0)
		{
			AbyssPoint = Vector3.zero;
			return;
		}
		AbyssPoint = LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots[0];
		for (int i = 1; i < LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots.Count; i++)
		{
			if ((base.transform.position - LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots[i]).sqrMagnitude < (base.transform.position - AbyssPoint).sqrMagnitude)
			{
				AbyssPoint = LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots[i];
			}
		}
	}

	private void GetRandomAbyssPoint()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots.Count == 0)
		{
			AbyssPoint = Vector3.zero;
		}
		else
		{
			for (int i = 0; i < 20; i++)
			{
				int index = Random.Range(0, LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots.Count);
				AbyssPoint = Tool2D.IgnoreZPoint(LevelMgr.Inst.CurrentRoomCtrller.abyssPoints_Dots[index]);
				if (!Physics.CheckSphere(AbyssPoint, 0.45f, LayerMask.GetMask("Monster")))
				{
					break;
				}
			}
		}
		base.transform.position = Tool2D.IgnoreZPoint(AbyssPoint);
		LocalTransform componentData = GetComponentData<LocalTransform>();
		componentData.Position = base.transform.position;
		SetComponentData(componentData);
	}

	public void BodyDead(Monster11_Body body, ref TakeDamageInfo_Dots info)
	{
		bodys.Remove(body);
		if (!(body == head))
		{
			return;
		}
		for (int num = bodys.Count - 1; num >= 0; num--)
		{
			if (EntityIsValid(bodys[num].myPpt.myEntity))
			{
				bodys[num].DotsAnnouncedDeath();
			}
		}
		base.transform.position = AbyssPoint;
		SyncDotsPositionSafe();
		DotsAnnouncedDeath();
		info.isTriggerDeadEvent = true;
	}

	public void SetShrinkFlase()
	{
		isShrink = false;
	}

	public void AttackFinish()
	{
		state = UnitState.AttackIdle;
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 > 0f)
		{
			bodyCount = (int)data1;
		}
	}
}
