using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Spell2006RopeController : MonoBehaviour
{
	public LineRenderer RopeLine;

	public Transform HookTransform;

	private Transform ownerTransform;

	private Transform targetTransform;

	private Entity targetEntity;

	private Vector3 ownerPosition;

	private Vector3 targetPosition;

	public AnimationCurve HookProcessCurve;

	private float hookProcessTime;

	private float hookTimer;

	private bool startProgress;

	public Transform HookSpriteRight;

	public Transform HookSpriteLeft;

	public Transform HookCenterTransform;

	private void OnEnable()
	{
		hookTimer = 0f;
		hookProcessTime = 0f;
		ownerTransform = null;
		targetEntity = Entity.Null;
		targetTransform = null;
		startProgress = false;
		ownerPosition = Vector3.zero;
		targetPosition = Vector3.zero;
		HookTransform.gameObject.SetActive(value: false);
		RopeLine.gameObject.SetActive(value: false);
	}

	public void InitialHookEffect(Transform startTrans, Transform endTrans, float duration)
	{
		ownerTransform = startTrans;
		ownerPosition = Tool2D.IgnoreZPoint(ownerTransform);
		targetTransform = endTrans;
		targetPosition = Tool2D.IgnoreZPoint(targetTransform);
		hookProcessTime = duration;
		startProgress = true;
		Update();
		HookTransform.gameObject.SetActive(value: true);
		RopeLine.gameObject.SetActive(value: true);
	}

	public void InitialHookEffect(Transform startTrans, Entity targetTeammateEntity, float duration)
	{
		ownerTransform = startTrans;
		ownerPosition = Tool2D.IgnoreZPoint(ownerTransform);
		targetEntity = targetTeammateEntity;
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (entityManager.HasComponent<LocalTransform>(targetEntity))
		{
			targetPosition = entityManager.GetComponentData<LocalTransform>(targetEntity).Position;
		}
		hookProcessTime = duration;
		startProgress = true;
		Update();
		HookTransform.gameObject.SetActive(value: true);
		RopeLine.gameObject.SetActive(value: true);
	}

	private void Update()
	{
		if (!startProgress)
		{
			return;
		}
		if (ownerTransform.gameObject.activeInHierarchy)
		{
			ownerPosition = Tool2D.IgnoreZPoint(ownerTransform.position);
		}
		if (targetEntity != Entity.Null)
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			if (entityManager.HasComponent<LocalTransform>(targetEntity))
			{
				targetPosition = entityManager.GetComponentData<LocalTransform>(targetEntity).Position;
			}
		}
		else if (targetTransform.gameObject.activeInHierarchy)
		{
			targetPosition = Tool2D.IgnoreZPoint(targetTransform.position);
		}
		RopeLine.SetPosition(0, ownerPosition);
		Vector3 position = ownerPosition + (targetPosition - ownerPosition) * HookProcessCurve.Evaluate(Mathf.Clamp(hookTimer / hookProcessTime, 0f, 1f));
		Vector3 normalized = (targetPosition - ownerPosition).normalized;
		RopeLine.SetPosition(1, position);
		HookTransform.position = position;
		HookTransform.right = Tool2D.GetDir(normalized, 0f);
		bool flag = normalized.x >= 0f;
		HookSpriteRight.gameObject.SetActive(flag);
		HookSpriteLeft.gameObject.SetActive(!flag);
		hookTimer += Time.deltaTime;
	}
}
