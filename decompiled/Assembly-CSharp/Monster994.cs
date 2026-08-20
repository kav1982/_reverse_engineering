using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Monster994 : UnitBase
{
	public enum SeagullState
	{
		ReadyToLand,
		Patrol,
		Eat,
		Fly,
		Idle,
		EscapeToLand,
		Freeze
	}

	private Vector3 _prePos;

	private Vector3 _nextPos;

	public SeagullState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private bool _flipSelf;

	private SeagullSpawner _spawner;

	private float exsitTimer;

	public VariableFloat EatTime = new VariableFloat(VariableType.Random, 0.8f, 1.2f);

	public VariableFloat landTime = new VariableFloat(VariableType.Random, 1.25f, 1.55f);

	public VariableFloat idleTime = new VariableFloat(VariableType.Random, 1.8f, 2.5f);

	public VariableFloat flyTime = new VariableFloat(VariableType.Random, 1f, 1.33f);

	public VariableFloat patrolTime = new VariableFloat(VariableType.Random, 1.6f, 1.8f);

	public VariableFloat freezeTime = new VariableFloat(VariableType.Random, 0.4f, 0.6f);

	public VariableFloat EscapeTime = new VariableFloat(VariableType.Random, 1.2f, 1.6f);

	private Vector2 roomXEdge;

	private bool shouldCheckIsSpawnInLand;

	public Transform modelLayer;

	private Vector3 escapeMidPoint;

	public SeagullState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
		}
	}

	public bool CanInteract
	{
		get
		{
			if (state != SeagullState.Eat && state != SeagullState.Patrol)
			{
				return state == SeagullState.Idle;
			}
			return true;
		}
	}

	private Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
	{
		float num = 1f - t;
		return num * num * a + 2f * num * t * b + t * t * c;
	}

	private float EaseInOutQuad(float t)
	{
		if (!(t < 0.5f))
		{
			return 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
		}
		return 2f * t * t;
	}

	private float EaseOutCubic(float t, float pow = 3f)
	{
		return 1f - Mathf.Pow(1f - t, pow);
	}

	private float EaseInCubic(float t)
	{
		return t * t * t;
	}

	public void Init(Vector3 spawnPos, Vector3 targetLandPos, SeagullSpawner spawner, Vector2 roomXEdge, bool isInLand)
	{
		state = SeagullState.ReadyToLand;
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		_flipSelf = targetLandPos.x < spawnPos.x;
		SetFlip((!_flipSelf) ? 1 : (-1));
		_spawner = spawner;
		_prePos = spawnPos;
		base.transform.position = spawnPos;
		_nextPos = targetLandPos;
		exsitTimer = UnityEngine.Random.Range(45f, 90f);
		this.roomXEdge = roomXEdge;
		if (isInLand)
		{
			state = SeagullState.Freeze;
			base.transform.position = (_prePos = (_nextPos = targetLandPos));
			shouldCheckIsSpawnInLand = true;
		}
		base.gameObject.name = "Seagull";
		SyncDotsPositionSafe();
	}

	protected override void SetFlip(float motionX)
	{
		modelLayer.transform.localScale = new Vector3((!(motionX < 0f)) ? 1 : (-1), 1f, 1f);
	}

	public override void Update()
	{
		base.Update();
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		if (shouldCheckIsSpawnInLand)
		{
			if (NavMesh.SamplePosition(base.transform.position, out var hit, 99999f, navAreaMask))
			{
				base.transform.position = (_prePos = (_nextPos = Tool2D.IgnoreZPoint(hit.position)));
				shouldCheckIsSpawnInLand = false;
			}
			return;
		}
		stateExistTime += Time.deltaTime;
		exsitTimer -= Time.deltaTime;
		switch (state)
		{
		case SeagullState.Freeze:
		{
			if (changedState)
			{
				base.Anima.Play("Idle");
				freezeTime.RandomResult();
			}
			if (!(stateExistTime >= freezeTime.result))
			{
				break;
			}
			if (exsitTimer <= 0f)
			{
				state = SeagullState.Fly;
				break;
			}
			float num3 = UnityEngine.Random.Range(0f, 100f);
			if (num3 <= 40f)
			{
				state = SeagullState.Idle;
			}
			else if (num3 <= 70f)
			{
				state = SeagullState.Eat;
			}
			else if (num3 <= 85f)
			{
				_flipSelf = !_flipSelf;
				SetFlip((!_flipSelf) ? 1 : (-1));
				state = SeagullState.Freeze;
			}
			else
			{
				state = SeagullState.Patrol;
			}
			break;
		}
		case SeagullState.ReadyToLand:
		{
			if (changedState)
			{
				base.Anima.Play("Fly");
				landTime.RandomResult();
				_flipSelf = _nextPos.x < base.transform.position.x;
				SetFlip((!_flipSelf) ? 1 : (-1));
			}
			float value = stateExistTime / landTime.result;
			value = Mathf.Clamp01(value);
			float num5 = Mathf.Sin(value * MathF.PI) * 1.5f;
			Vector3 position = Vector3.Lerp(_prePos, _nextPos, value);
			position.y += num5;
			base.transform.position = position;
			SyncDotsPositionSafe();
			if (stateExistTime >= landTime.result)
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				state = SeagullState.Freeze;
			}
			break;
		}
		case SeagullState.Fly:
		{
			if (changedState)
			{
				_flipSelf = UnityEngine.Random.Range(0, 100) > 50;
				base.Anima.Play("Fly");
				float x = (_flipSelf ? roomXEdge.x : roomXEdge.y);
				float y = base.transform.position.y;
				float z = 0f - UnityEngine.Random.Range(4f, 7f);
				_nextPos = new Vector3(x, y, z);
				_prePos = base.transform.position;
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				flyTime.RandomResult();
				SetFlip((!_flipSelf) ? 1 : (-1));
				float num4 = UnityEngine.Random.Range(0f, 1f);
				if (num4 <= 0.5f)
				{
					SEMgr.Inst.PlaySE("SE_Monster994_01");
				}
				else if (num4 <= 1f)
				{
					SEMgr.Inst.PlaySE("SE_Monster994_02");
				}
			}
			Vector3 b = (_prePos + _nextPos) * 0.5f + Vector3.up * 2f;
			base.transform.position = QuadraticBezier(_prePos, b, _nextPos, EaseOutCubic(Mathf.Clamp01(stateExistTime / flyTime.result), 1.5f));
			SyncDotsPositionSafe();
			if (stateExistTime >= flyTime.result)
			{
				_spawner.DestroySegull(this);
			}
			break;
		}
		case SeagullState.Patrol:
		{
			if (changedState)
			{
				base.Anima.Play("Jump");
				patrolTime.RandomResult();
				_prePos = base.transform.position;
				float num6 = UnityEngine.Random.Range(0.6f, 0.9f) * patrolTime.result;
				_nextPos = Tool2D.GetNavMeshPointIngoreZ(_prePos + Tool2D.GetDir() * num6);
				_flipSelf = _nextPos.x < base.transform.position.x;
				SetFlip((!_flipSelf) ? 1 : (-1));
			}
			CheckIfPlayerIsNearby();
			Vector3 position2 = Vector3.Lerp(_prePos, _nextPos, Mathf.Clamp01(stateExistTime / patrolTime.result));
			base.transform.position = position2;
			SyncDotsPositionSafe();
			if (stateExistTime >= patrolTime.result)
			{
				state = SeagullState.Freeze;
			}
			break;
		}
		case SeagullState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				_prePos = base.transform.position;
				idleTime.RandomResult();
			}
			CheckIfPlayerIsNearby();
			if (stateExistTime >= idleTime.result)
			{
				state = SeagullState.Freeze;
			}
			break;
		case SeagullState.Eat:
			if (changedState)
			{
				base.Anima.Play("Eat");
				EatTime.RandomResult();
			}
			CheckIfPlayerIsNearby();
			if (stateExistTime >= EatTime.result)
			{
				state = SeagullState.Freeze;
			}
			break;
		case SeagullState.EscapeToLand:
		{
			if (changedState)
			{
				base.Anima.Play("Fly");
				EscapeTime.RandomResult();
				_prePos = base.transform.position;
				float num = UnityEngine.Random.Range(3.5f, 5f);
				_nextPos = Tool2D.GetNavMeshPointIngoreZ(_prePos + Tool2D.GetDir() * num);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				_flipSelf = _nextPos.x < base.transform.position.x;
				SetFlip((!_flipSelf) ? 1 : (-1));
				escapeMidPoint = (_prePos + _nextPos) * 0.5f + Vector3.forward * (-1f * UnityEngine.Random.Range(1.75f, 2.5f));
				float num2 = UnityEngine.Random.Range(0f, 1f);
				if (num2 <= 0.5f)
				{
					SEMgr.Inst.PlaySE("SE_Monster994_01");
				}
				else if (num2 <= 1f)
				{
					SEMgr.Inst.PlaySE("SE_Monster994_02");
				}
			}
			float t = Mathf.Clamp01(stateExistTime / EscapeTime.result);
			base.transform.position = QuadraticBezier(_prePos, escapeMidPoint, _nextPos, EaseOutCubic(t, 2.5f));
			SyncDotsPositionSafe();
			if (stateExistTime >= EscapeTime.result)
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				state = SeagullState.Freeze;
			}
			break;
		}
		}
	}

	private void CheckIfPlayerIsNearby()
	{
		Vector3 playerPointIgnoreZ = PlayerMgr.Inst.PlayerPointIgnoreZ;
		if (Tool2D.IgnoreZDistance(base.transform.position, playerPointIgnoreZ) <= 1f)
		{
			state = SeagullState.EscapeToLand;
			_prePos = base.transform.position;
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		info.immuneDamage = true;
		ScaredSelf();
	}

	public void ScaredSelf()
	{
		if (state != SeagullState.Fly && state != SeagullState.EscapeToLand)
		{
			state = SeagullState.Fly;
			_prePos = base.transform.position;
			StartCoroutine(ScareNearlySeagull());
		}
		IEnumerator ScareNearlySeagull()
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.12f, 0.2f));
			_spawner?.ScareNearlySeagull(this);
		}
	}
}
