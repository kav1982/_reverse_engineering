using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster47 : UnitBase
{
	public enum UnitState
	{
		Initiiate,
		BornIdle,
		RotateToward,
		Reach,
		Pull,
		Idle,
		dying,
		died
	}

	[Serializable]
	public class PoisonFog
	{
		public GameObject ObjectPoisonFog;

		public float PoisonTimes;

		public Vector3 poisonCenter;
	}

	public float MaxRotateAngle = 30f;

	public Sprite BodySprite;

	public Sprite headSprite;

	public MeshRenderer meshrender;

	public int index;

	public int RotateIndex;

	public bool head;

	public bool pullOnlyOverLength;

	public float maxBoneLength;

	public float boneCount;

	public LineRenderer linerender;

	public List<Monster47> bodys = new List<Monster47>();

	private List<Vector3> _TempBodysPosition = new List<Vector3>();

	public UnitState state = UnitState.BornIdle;

	public int SummonID = 104701;

	private bool _headRotateDirection;

	public Collider MonsterCollider;

	public float eachBoneTurnTime = 2f;

	public float eachStateTime = 1f;

	private float _thisStateTime;

	private float _monsterExistTime;

	private float _eachBoneTurnTime;

	public float timeToDie = 1f;

	public float FogExistTIme;

	public float TimeBetweenAttack;

	public float FogRadius = 1f;

	public float AttackInterval = 1f;

	public float TimeBeforAttack;

	private float thisMonsterExistTime;

	private void Start()
	{
	}

	public override void Update()
	{
		base.Update();
		_monsterExistTime += Time.deltaTime;
		_thisStateTime += Time.deltaTime;
		_eachBoneTurnTime += Time.deltaTime;
		if (!head)
		{
			return;
		}
		switch (state)
		{
		case UnitState.Initiiate:
			GetNearestTargetPlayerFirst();
			if (head)
			{
				meshrender.material.SetTexture(GameConstManaged.shaderTextureIndex, headSprite.texture);
				bodys.Clear();
				bodys.Add(this);
				for (int i = 0; (float)i < boneCount; i++)
				{
					Monster47 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + SummonID, base.transform.position).GetComponent<Monster47>();
					Debug.Log("Create");
					component.index = bodys.Count;
					component.SetAsPosition(bodys[component.index - 1].transform.position);
					component.SetAsChild(base.gameObject);
				}
			}
			SwitchState(UnitState.BornIdle);
			break;
		case UnitState.BornIdle:
			if (_thisStateTime >= 0.1f)
			{
				SwitchState(UnitState.Reach);
				Debug.LogError("StartReach");
			}
			break;
		case UnitState.RotateToward:
			if (_thisStateTime >= eachStateTime)
			{
				SwitchState(UnitState.Reach);
			}
			if (head)
			{
				int sequence2 = GetSequence(0, direction: true);
				GetNearestTargetPlayerFirst();
				RotateToHead(sequence2);
				pullSingleSmoothtoFront(sequence2, 0.3f, 2f, 0.7f);
			}
			break;
		case UnitState.Reach:
			if (_thisStateTime >= eachStateTime)
			{
				SwitchState(UnitState.Pull);
			}
			else if (head)
			{
				int num = GetSequence(bodys.Count - 1, direction: false) - 1;
				GetNearestTargetPlayerFirst();
				Debug.Log($"正在推第几关节{num}");
				_headRotateDirection = GetRotateDirection(bodys[num].gameObject, bodys[num + 1].transform.position, targetPpt.transform);
				if (num == 0)
				{
					RotateSingle1(bodys[num].gameObject, bodys[num + 1].transform.position, 20f, 120 * (bodys.Count - num), targetPpt.transform, reverse: true);
					pullSingleSmoothtoTarget(num, targetPpt.transform, 0.3f, 1.5f, 0.7f);
				}
				else if (num >= 0)
				{
					RotateSingle1(bodys[num].gameObject, bodys[num + 1].transform.position, 20f, 90 * (bodys.Count - num), targetPpt.transform, reverse: true);
					pullSingleSmoothtoTarget(num, targetPpt.transform, 0.1f, 1.5f, 0.7f);
				}
			}
			break;
		case UnitState.Pull:
			if (_thisStateTime >= eachStateTime)
			{
				_headRotateDirection = GetRotateDirection(bodys[0].gameObject, bodys[1].transform.position, targetPpt.transform);
				SwitchState(UnitState.Reach);
			}
			else if (head)
			{
				SetMove(new Vector3(0f, 0f, 0f));
				int sequence = GetSequence(1, direction: true);
				Debug.Log($"正在拉第几关节{sequence}");
				bodys[sequence].RotateSingleDIrection(bodys[sequence].gameObject, bodys[sequence - 1].transform.position, GetRotateDirection(bodys[1].gameObject, bodys[0].transform.position, targetPpt.transform), sequence * 180);
				pullSingleSmoothtoFront(sequence, 10f);
			}
			break;
		case UnitState.Idle:
			if (_thisStateTime >= eachStateTime)
			{
				SwitchState(UnitState.Reach);
			}
			_ = head;
			break;
		}
		RenderLIne();
	}

	public void SetAsChild(GameObject Currenthead)
	{
		head = false;
		bodys = Currenthead.GetComponent<Monster47>().bodys;
		if (bodys.Count > 0)
		{
			if (bodys[0].head)
			{
				bodys.Add(this);
			}
			else if (bodys[bodys.Count - 1].head)
			{
				bodys.Insert(0, this);
			}
			else
			{
				Debug.LogError("不该如此");
			}
		}
	}

	public void RenderLIne()
	{
		if (bodys.Count <= 1)
		{
			return;
		}
		if (bodys[0].head)
		{
			bodys[0].linerender.enabled = false;
			for (int i = 1; i < bodys.Count; i++)
			{
				bodys[i].linerender.enabled = true;
				Vector3 position = bodys[i].transform.position;
				position.z = bodys[i].myPpt.tsf_Layer.transform.position.z;
				bodys[i].linerender.SetPosition(0, position);
				Vector3 position2 = bodys[i - 1].transform.position;
				position2.z = bodys[i - 1].myPpt.tsf_Layer.transform.position.z;
				bodys[i].linerender.SetPosition(1, position2);
			}
		}
		else if (bodys[bodys.Count - 1].head)
		{
			bodys[bodys.Count - 1].linerender.enabled = false;
			for (int num = 0; num < bodys.Count - 2; num--)
			{
				bodys[num].linerender.enabled = true;
				Vector3 position3 = bodys[num].transform.position;
				position3.z = myPpt.tsf_Layer.transform.position.z;
				bodys[num].linerender.SetPosition(0, position3);
				Vector3 position4 = bodys[num + 1].transform.position;
				position4.z = myPpt.tsf_Layer.transform.position.z;
				bodys[num].linerender.SetPosition(1, position4);
			}
		}
		else
		{
			Debug.LogError("不该如此");
		}
	}

	public void SetAsPosition(Vector3 position)
	{
		base.transform.position = RandomCircle(position, maxBoneLength);
	}

	public Vector3 RandomCircle(Vector3 center, float radius)
	{
		float num = UnityEngine.Random.Range(0, 360);
		Vector3 zero = Vector3.zero;
		zero.x = center.x + radius * Mathf.Cos(num * (MathF.PI / 180f));
		zero.y = center.y + radius * Mathf.Sin(num * (MathF.PI / 180f));
		zero.z = center.z;
		return zero;
	}

	public void Initial()
	{
		Debug.Log("SinbleInatial");
	}

	public void asChild()
	{
		Debug.Log("SinbleInatial");
	}

	public override void SingleInitialCallback()
	{
	}

	public override void EveryInitialCallback()
	{
		MonsterCollider.enabled = true;
		thisMonsterExistTime = 0f;
		SwitchState(UnitState.Initiiate);
		EveryInitialCallbackEye();
	}

	public void Squier()
	{
	}

	public void RotateToHead(int Startat)
	{
		int sequence = GetSequence(Startat - 1, direction: true);
		Debug.Log($"正在拉第几关节{sequence}");
		RotateAllToDirection(targetPpt.transform, sequence);
	}

	public void RotateAllToTarget1(Transform targetPpt, int Index)
	{
		if (targetPpt == null)
		{
			Debug.LogError("没有旋转目标");
		}
		else
		{
			if (bodys.Count <= 0)
			{
				return;
			}
			if (bodys[0].head)
			{
				for (int i = Index; i < bodys.Count - 1; i++)
				{
					RotateSingle1(bodys[i + 1].gameObject, bodys[i].transform.position, 60f, 360f, targetPpt);
					pullSingleSmooth(i, eachStateTime / (float)(bodys.Count - Index));
				}
			}
			else if (!bodys[bodys.Count - 1].head)
			{
				Debug.LogError("不该如此");
			}
		}
	}

	public void RotateAllToDirection(Transform targetPpt, int Index)
	{
		if (targetPpt == null)
		{
			Debug.LogError("没有旋转目标");
		}
		else
		{
			if (bodys.Count <= 0)
			{
				return;
			}
			if (bodys[0].head)
			{
				for (int i = Index; i < bodys.Count - 1; i++)
				{
					bodys[i + 1].RotateSingleDIrection(bodys[i + 1].gameObject, bodys[i].transform.position, GetRotateDirection(bodys[1].gameObject, bodys[0].transform.position, targetPpt), (i + 1) * 180);
				}
			}
			else if (!bodys[bodys.Count - 1].head)
			{
				Debug.LogError("不该如此");
			}
		}
	}

	public void RotateAllToTarget2(Transform targetPpt, int Index)
	{
		if (targetPpt == null || bodys.Count <= 0)
		{
			return;
		}
		if (bodys[0].head)
		{
			for (int i = index; i < bodys.Count - 1; i++)
			{
				bodys[i + 1].RotateSingle2(bodys[Index].transform.position, direction: true);
			}
		}
		else if (bodys[bodys.Count - 1].head)
		{
			for (int num = index; num > 0; num--)
			{
				bodys[num - 1].RotateSingle2(bodys[Index].transform.position, direction: true);
			}
		}
		else
		{
			Debug.LogError("不该如此");
		}
	}

	public void RotateSingle1(GameObject rotategameobject, Vector3 CenterPosition, float anglenear = 60f, float speed = 360f, Transform TargetTransform = null, bool reverse = false)
	{
		if (TargetTransform != null)
		{
			Vector3 vector = TargetTransform.position - CenterPosition;
			Vector3 vector2 = rotategameobject.transform.position - CenterPosition;
			float num = Vector3.Angle(vector, vector2);
			if (Vector3.Dot(Vector3.Cross(vector, vector2), Vector3.forward) < 0f)
			{
				num *= -1f;
				num += 360f;
			}
			if (num > 180f && num < 360f - anglenear)
			{
				Vector3 vector3 = rotategameobject.transform.position - CenterPosition;
				vector3 = ((!reverse) ? (Quaternion.AngleAxis((0f - speed) * Time.deltaTime, Vector3.forward) * vector3) : (Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.forward) * vector3));
				rotategameobject.transform.position = CenterPosition + vector3;
			}
			else if (num < 180f && num > anglenear)
			{
				Vector3 vector4 = rotategameobject.transform.position - CenterPosition;
				vector4 = ((!reverse) ? (Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.forward) * vector4) : (Quaternion.AngleAxis((0f - speed) * Time.deltaTime, Vector3.forward) * vector4));
				rotategameobject.transform.position = CenterPosition + vector4;
			}
		}
		else
		{
			Vector3 vector5 = rotategameobject.transform.position - CenterPosition;
			vector5 = Quaternion.AngleAxis(-360f * Time.deltaTime, Vector3.forward) * vector5;
			rotategameobject.transform.position = CenterPosition + vector5;
		}
	}

	public void RotateSingleDIrection(GameObject rotategameobject, Vector3 CenterPosition, bool direction, float speed)
	{
		if (direction)
		{
			Vector3 vector = rotategameobject.transform.position - CenterPosition;
			vector = Quaternion.AngleAxis((0f - speed) * Time.deltaTime, Vector3.forward) * vector;
			base.transform.position = CenterPosition + vector;
		}
		else
		{
			Vector3 vector2 = rotategameobject.transform.position - CenterPosition;
			vector2 = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.forward) * vector2;
			base.transform.position = CenterPosition + vector2;
		}
	}

	public void RotateSingle2(Vector3 CenterPosition, bool direction)
	{
		Vector3 vector = base.transform.position - CenterPosition;
		vector = ((!direction) ? (Quaternion.AngleAxis(-180f * Time.deltaTime, Vector3.forward) * vector) : (Quaternion.AngleAxis(180f * Time.deltaTime, Vector3.forward) * vector));
		base.transform.position = CenterPosition + vector;
	}

	public bool GetRotateDirection(GameObject rotategameobject, Vector3 CenterPosition, Transform TargetTransform = null)
	{
		Vector3 vector = TargetTransform.position - CenterPosition;
		Vector3 vector2 = rotategameobject.transform.position - CenterPosition;
		Vector3.Angle(vector, vector2);
		if (Vector3.Dot(Vector3.Cross(vector, vector2), Vector3.forward) < 0f)
		{
			return false;
		}
		return true;
	}

	public void pullToHead(int Startat)
	{
		int sequence = GetSequence(Startat, direction: true);
		Debug.Log($"正在拉第几关节{sequence}");
		bodys[sequence].RotateSingleDIrection(bodys[sequence].gameObject, bodys[sequence - 1].transform.position, GetRotateDirection(bodys[1].gameObject, bodys[0].transform.position, targetPpt.transform), sequence * 180);
		pullSingleSmooth(sequence, eachStateTime / (float)(bodys.Count - Startat));
	}

	public void pullToHeadReverse(int Startat)
	{
		int sequence = GetSequence(Startat, direction: true);
		Debug.Log($"正在拉第几关节{sequence}");
		bodys[sequence].RotateSingleDIrection(bodys[sequence].gameObject, bodys[sequence - 1].transform.position, GetRotateDirection(bodys[1].gameObject, bodys[0].transform.position, targetPpt.transform), sequence * 180);
		pullSingleSmooth(sequence, eachStateTime / (float)(bodys.Count - Startat));
	}

	public void pullSingleAtSpeed(int Partindex, float speed)
	{
		if (Partindex == 0)
		{
			return;
		}
		if (bodys[0].head)
		{
			if (pullOnlyOverLength)
			{
				float sqrMagnitude = (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).sqrMagnitude;
				if ((double)sqrMagnitude > 1.5 * (double)maxBoneLength * (double)maxBoneLength)
				{
					bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).normalized * Time.deltaTime * speed;
				}
				else if ((double)sqrMagnitude < 0.5 * (double)maxBoneLength * (double)maxBoneLength)
				{
					bodys[Partindex].transform.position += (bodys[Partindex].transform.position - bodys[Partindex - 1].transform.position).normalized * Time.deltaTime * speed;
				}
			}
			else
			{
				bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).normalized * Time.deltaTime * 10f;
			}
		}
		else if (!bodys[bodys.Count - 1].head)
		{
			Debug.LogError("不该如此");
		}
	}

	public void pullSingleAtSpeedTarget(int Partindex, float speed, Transform target)
	{
		if (Partindex == 0)
		{
			return;
		}
		if (bodys[0].head)
		{
			if (pullOnlyOverLength)
			{
				float sqrMagnitude = (target.position - bodys[Partindex].transform.position).sqrMagnitude;
				if ((double)sqrMagnitude > 1.5 * (double)maxBoneLength * (double)maxBoneLength)
				{
					bodys[Partindex].transform.position += (target.position - bodys[Partindex].transform.position).normalized * Time.deltaTime * speed;
				}
				else if ((double)sqrMagnitude < 0.5 * (double)maxBoneLength * (double)maxBoneLength)
				{
					bodys[Partindex].transform.position += (bodys[Partindex].transform.position - target.position).normalized * Time.deltaTime * speed;
				}
			}
			else
			{
				bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).normalized * Time.deltaTime * 10f;
			}
		}
		else if (!bodys[bodys.Count - 1].head)
		{
			Debug.LogError("不该如此");
		}
	}

	public void pullSingleSmooth(int Partindex, float time, bool direction = true)
	{
		if (Partindex == 0)
		{
			return;
		}
		if (bodys[0].head)
		{
			if (pullOnlyOverLength)
			{
				if (direction)
				{
					float sqrMagnitude = (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).sqrMagnitude;
					if ((double)sqrMagnitude > 1.5 * (double)maxBoneLength * (double)maxBoneLength)
					{
						bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position) / time * Time.deltaTime;
					}
					else if ((double)sqrMagnitude < 0.7 * (double)maxBoneLength * (double)maxBoneLength)
					{
						bodys[Partindex].transform.position += (bodys[Partindex].transform.position - bodys[Partindex - 1].transform.position) / time * Time.deltaTime;
					}
				}
				else
				{
					float sqrMagnitude2 = (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).sqrMagnitude;
					if ((double)sqrMagnitude2 > 1.5 * (double)maxBoneLength * (double)maxBoneLength)
					{
						bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position) / time * Time.deltaTime;
					}
					else if ((double)sqrMagnitude2 < 0.7 * (double)maxBoneLength * (double)maxBoneLength)
					{
						bodys[Partindex].transform.position += (bodys[Partindex].transform.position - bodys[Partindex - 1].transform.position) / time * Time.deltaTime;
					}
				}
			}
			else
			{
				bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).normalized * Time.deltaTime * 10f;
			}
		}
		else if (!bodys[bodys.Count - 1].head)
		{
			Debug.LogError("不该如此");
		}
	}

	public void pullSingleSmoothtoTarget(int Partindex, Transform target, float speed = 1f, float pullfrom = 1.5f, float pushfrom = 1f, bool direction = true)
	{
		if (Partindex == 0)
		{
			return;
		}
		if (bodys[0].head)
		{
			if (pullOnlyOverLength)
			{
				if (direction)
				{
					float sqrMagnitude = (target.transform.position - bodys[Partindex].transform.position).sqrMagnitude;
					if (sqrMagnitude > pullfrom * maxBoneLength * maxBoneLength)
					{
						Debug.Log("收缩");
						bodys[Partindex].transform.position += (target.transform.position - bodys[Partindex].transform.position) * speed * Time.deltaTime;
					}
					else if (sqrMagnitude < pushfrom * maxBoneLength * maxBoneLength)
					{
						Debug.Log("延伸");
						bodys[Partindex].transform.position += (bodys[Partindex].transform.position - target.transform.position) * speed * Time.deltaTime;
					}
				}
			}
			else
			{
				bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).normalized * Time.deltaTime * 10f;
			}
		}
		else if (!bodys[bodys.Count - 1].head)
		{
			Debug.LogError("不该如此");
		}
	}

	public void pullSingleSmoothtoFront(int Partindex, float speed = 1f, float pullfrom = 1.5f, float pushfrom = 1f, bool direction = true)
	{
		if (Partindex == 0)
		{
			return;
		}
		if (bodys[0].head)
		{
			if (pullOnlyOverLength)
			{
				if (direction)
				{
					float sqrMagnitude = (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).sqrMagnitude;
					if (sqrMagnitude > pullfrom * maxBoneLength * maxBoneLength)
					{
						Debug.Log("收缩");
						bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position) * speed * Time.deltaTime;
					}
					else if (sqrMagnitude < pushfrom * maxBoneLength * maxBoneLength)
					{
						Debug.Log("延伸");
						bodys[Partindex].transform.position += (bodys[Partindex].transform.position - bodys[Partindex - 1].transform.position) * speed * Time.deltaTime;
					}
				}
			}
			else
			{
				bodys[Partindex].transform.position += (bodys[Partindex - 1].transform.position - bodys[Partindex].transform.position).normalized * Time.deltaTime * 10f;
			}
		}
		else if (!bodys[bodys.Count - 1].head)
		{
			Debug.LogError("不该如此");
		}
	}

	private void SwitchState(UnitState newstate)
	{
		state = newstate;
		_thisStateTime = 0f;
	}

	public void Attack()
	{
		SEMgr.Inst.PlaySE("SE_Monster45_blast");
		new PoisonFog
		{
			ObjectPoisonFog = ObjPoolMgr.Inst.GetGO("Prefabs/EF/Monster45PoisonFog", base.transform.position),
			poisonCenter = base.transform.position,
			PoisonTimes = thisMonsterExistTime
		};
	}

	private void EveryInitialCallbackEye()
	{
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		info.immuneDamage = true;
		if (info.attackerPpt != null && info.attackerPpt.unitCfg.id == 104502)
		{
			info.immuneDamage = false;
		}
		base.BeforeTakeDamage(info);
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		base.AfterTakeDamage(info);
		myPpt.unitCfg.currentHP = 100000000f;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
	}

	public Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
	{
		return Quaternion.AngleAxis(angle, axis) * (position - center) + center;
	}

	public Vector3 RotateRound(Vector3 position, float angle)
	{
		return Quaternion.AngleAxis(angle, Vector3.back) * position;
	}

	private void OnDrawGizmos()
	{
	}

	private int GetSequence(int Startat, bool direction, bool getend = true)
	{
		if (getend)
		{
			if (direction)
			{
				return (int)(_thisStateTime / (eachStateTime / (float)(bodys.Count - Startat)) + 1f);
			}
			return (int)((float)bodys.Count - _thisStateTime / (eachStateTime / (float)Startat));
		}
		if (direction)
		{
			return (int)(_thisStateTime / (eachStateTime / (float)(bodys.Count - Startat)) + 1f);
		}
		return (int)((float)(bodys.Count - 1) - _thisStateTime / (eachStateTime / (float)Startat));
	}
}
