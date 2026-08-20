using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Boss53NormalAttackLineController : MonoBehaviour
{
	private enum State
	{
		Idle,
		Find,
		Ready,
		Attacked
	}

	public Transform StartEffect;

	public Transform TargetEffect;

	public LineRenderer Line;

	public Boss53 Boss;

	public float normalWidth = 0.2f;

	public float attackWidth = 3f;

	public float findTime = 1.5f;

	public float readyTime = 0.6f;

	public float finishTime = 1f;

	public float finishAnimSpeed = 2f;

	public Action OnFinish;

	public Action<Vector3> OnAttack;

	private State state;

	private bool justChangeState;

	private Vector3 offsetPoint1;

	private Vector3 offsetPoint2;

	private Vector3 offsetDir;

	private Entity target;

	private float findTimer;

	private float readyTimer;

	private float finishTimer;

	private void Update()
	{
		Line.SetPosition(0, base.transform.position);
		StartEffect.transform.position = base.transform.position;
		if (state == State.Idle)
		{
			Hide();
		}
		else if (state == State.Find)
		{
			if (JustChangeStateAndUpdate())
			{
				findTimer = 0f;
				TargetEffect.gameObject.SetActive(value: true);
				TargetEffect.localScale = Vector3.one * 0.2f;
				StartEffect.gameObject.SetActive(value: true);
				Line.widthMultiplier = normalWidth;
				Line.positionCount = 2;
				Line.gameObject.SetActive(value: true);
			}
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			if (entityManager.HasComponent<LocalTransform>(target))
			{
				float3 position = entityManager.GetComponentData<LocalTransform>(target).Position;
				TargetEffect.position = position;
				Line.SetPosition(1, position);
			}
			findTimer += Time.deltaTime;
			if (findTimer >= findTime)
			{
				ChangeState(State.Ready);
			}
		}
		else if (state == State.Ready)
		{
			if (JustChangeStateAndUpdate())
			{
				readyTimer = 0f;
			}
			readyTimer += Time.deltaTime;
			TargetEffect.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, readyTimer / readyTime);
			TargetEffect.position = Line.GetPosition(1);
			if (readyTimer >= readyTime)
			{
				ChangeState(State.Attacked);
				TargetEffect.gameObject.SetActive(value: false);
				OnAttack?.Invoke(Line.GetPosition(Line.positionCount - 1));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss53_NormalAttackBoom", Line.GetPosition(Line.positionCount - 1), Vector3.one * Boss.NormalAttackRange, 3f);
			}
		}
		else
		{
			if (state != State.Attacked)
			{
				return;
			}
			if (JustChangeStateAndUpdate())
			{
				StartEffect.gameObject.SetActive(value: false);
				finishTimer = 0f;
			}
			finishTimer += Time.deltaTime;
			if (Line.positionCount < 20)
			{
				Line.positionCount = 20;
				Vector3 position2 = Line.GetPosition(0);
				Vector3 position3 = Line.GetPosition(1);
				for (int i = 0; i < 20; i++)
				{
					float t = (float)i / 19f;
					Line.SetPosition(i, Vector3.Lerp(position2, position3, t));
				}
				Vector3 vector = Line.GetPosition(Line.positionCount - 1) - Line.GetPosition(0);
				int num = UnityEngine.Random.Range(30, 60) * ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
				float3 oldDir = vector;
				offsetDir = DTool.GetDir(in oldDir, num);
				offsetPoint1 = Line.GetPosition(7);
				offsetPoint2 = Line.GetPosition(15);
			}
			offsetPoint1 += Time.deltaTime * finishAnimSpeed * offsetDir;
			offsetPoint2 -= Time.deltaTime * finishAnimSpeed * offsetDir;
			for (int j = 1; j < Line.positionCount; j++)
			{
				float3 oldDir = Line.GetPosition(0);
				float3 v = offsetPoint1;
				float3 v2 = offsetPoint2;
				float3 v3 = Line.GetPosition(Line.positionCount - 1);
				float3 @float = DTool.CubicBezierCurve(in oldDir, in v, in v2, in v3, (float)j / (float)(Line.positionCount - 1));
				Line.SetPosition(j, @float);
			}
			Line.material.SetFloat("_DissolveProcess", finishTimer / finishTime);
			Line.widthMultiplier = Mathf.Lerp(attackWidth, 0f, finishTimer / finishTime);
			if (finishTimer > finishTime)
			{
				Line.material.SetFloat("_DissolveProcess", -1f);
				Line.widthMultiplier = normalWidth;
				OnFinish?.Invoke();
				ChangeState(State.Idle);
				Hide();
			}
		}
	}

	public void UpdateTargetPosition(Vector3 position)
	{
		Vector3 layerPoint = Tool2D.GetLayerPoint(position);
		Line.SetPosition(1, layerPoint);
		TargetEffect.transform.position = layerPoint;
	}

	public void Hide()
	{
		StartEffect.gameObject.SetActive(value: false);
		TargetEffect.gameObject.SetActive(value: false);
		Line.gameObject.SetActive(value: false);
	}

	public void StartLookTarget(Entity target)
	{
		this.target = target;
		ChangeState(State.Find);
	}

	private void ChangeState(State newState)
	{
		state = newState;
		justChangeState = true;
	}

	private bool JustChangeStateAndUpdate()
	{
		if (justChangeState)
		{
			justChangeState = false;
			return true;
		}
		return false;
	}
}
