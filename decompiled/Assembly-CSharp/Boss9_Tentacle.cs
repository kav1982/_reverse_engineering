using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss9_Tentacle : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.BoxCollider BC;

	public float damage;

	public float TVPackageDamageFactor;

	public float knockBack;

	public ShockParam shockParam;

	public AnimaEvent animaEvent;

	public Transform shadow;

	public Transform shadowParent;

	public LineRenderer lineRenderer;

	public LineRenderer shadowLineRenderer;

	public Transform rotateObj;

	public int segmentCount = 10;

	public float tentacleLength = 7f;

	public float waveAmplitude = 0.5f;

	public float waveSpeed = 5f;

	public float waveFrequency = 2f;

	[SerializeField]
	private List<Vector3> points = new List<Vector3>();

	[SerializeField]
	private List<Vector3> shadowPoints = new List<Vector3>();

	public bool bubbleOn;

	public ParticleSystem bubbleEffect;

	public GameObject bubbleParent;

	public Transform controlPoint1;

	public Transform controlPoint2;

	public Transform startPoint;

	public Transform endPoint;

	public LineRenderer warningLine;

	public LineRenderer warningLine_H;

	public bool warningLineOn;

	public Transform waringLineStart;

	public Transform waringLineEnd;

	public GameObject readyEffect;

	public float height;

	public SpriteRenderer holeSpriteRenderer;

	public Sprite[] holeSprites;

	public bool isUp;

	public Entity thisEntity { get; set; }

	public void Start()
	{
		lineRenderer.positionCount = segmentCount;
		shadowLineRenderer.positionCount = segmentCount;
		warningLine.positionCount = segmentCount;
		warningLine_H.positionCount = segmentCount;
		shadowLineRenderer.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(shadowParent.position), LayerCorrectType.Shadow);
		bubbleEffect.transform.position = bubbleParent.transform.position;
		if (ScriptableObjMgr.Inst.testCtrller.isBW)
		{
			damage *= TVPackageDamageFactor;
		}
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void OnEnable()
	{
		if (animaEvent.DoAction == null)
		{
			animaEvent.DoAction = AnimaAction;
		}
		holeSpriteRenderer.sprite = holeSprites[Random.Range(0, holeSprites.Length)];
		Quaternion.Euler(0f, 0f, 180f);
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2261504u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		BC.enabled = false;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, BC);
	}

	private void Update()
	{
		AnimateTentacle();
		bubbleEffect.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(bubbleParent.transform.position, height));
		_ = bubbleOn;
		if (!warningLineOn)
		{
			return;
		}
		if (GameMgr.IsHarmony_Static)
		{
			for (int i = 0; i < warningLine_H.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(waringLineStart.position, waringLineEnd.position, (float)i / (float)(warningLine_H.positionCount - 1));
				warningLine_H.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
		}
		else
		{
			for (int j = 0; j < warningLine.positionCount; j++)
			{
				Vector3 rootPoint2 = Vector3.Lerp(waringLineStart.position, waringLineEnd.position, (float)j / (float)(warningLine.positionCount - 1));
				warningLine.SetPosition(j, Tool2D.GetLayerPoint(rootPoint2, LayerCorrectType.GroundEffect));
			}
		}
	}

	private void AnimateTentacle()
	{
		for (int i = 0; i < segmentCount; i++)
		{
			float num = (float)i / (float)(segmentCount - 1);
			Vector3 vector = BezierCurve(endPoint.position, startPoint.position, num);
			float num2 = Mathf.Lerp(0f, 1f, num);
			float num3 = Mathf.Sin((num + Time.time * waveSpeed) * waveFrequency) * waveAmplitude * num2;
			vector += Vector3.Cross(Tool2D.IgnoreZV2ToV1Normal(startPoint, endPoint), Vector3.forward).normalized * num3;
			points[i] = Tool2D.GetLayerPoint(vector - new Vector3(0f, 0f, height), LayerCorrectType.Coordinate);
			if (i == 0 && !isUp)
			{
				points[i] = Tool2D.GetLayerPoint(vector, LayerCorrectType.Coordinate) + new Vector3(0f, 0f, 0.1f);
			}
			shadowPoints[i] = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(vector), LayerCorrectType.Shadow) + new Vector3(0f, 0.1f, 0f);
		}
		lineRenderer.SetPositions(points.ToArray());
		shadowLineRenderer.SetPositions(shadowPoints.ToArray());
	}

	private Vector3 BezierCurve(Vector3 p0, Vector3 p1, float t)
	{
		float num = 1f - t;
		float num2 = num * num;
		float num3 = num2 * num;
		float num4 = t * t;
		float num5 = num4 * t;
		return num3 * p0 + 3f * num2 * t * p1 + 3f * num * num4 * controlPoint1.position + num5 * controlPoint2.position;
	}

	public void Recycle()
	{
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void ShakeCamera()
	{
		CamController.Inst.SetShock(shockParam);
	}

	public void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "ShakeCamera":
			ShakeCamera();
			SEMgr.Inst.boss9_tentacleAttack.PlaySE();
			break;
		case "Recycle":
			BC.enabled = false;
			Recycle();
			break;
		case "BubbleOn":
			bubbleEffect.Play();
			bubbleOn = true;
			BC.enabled = true;
			break;
		case "BubbleOff":
			bubbleEffect.Stop();
			bubbleOn = false;
			bubbleEffect.transform.position = lineRenderer.GetPosition(0);
			break;
		case "WarningLineOn":
			SEMgr.Inst.boss9_tentacleWarning.PlaySE();
			if (GameMgr.IsHarmony_Static)
			{
				warningLine_H.enabled = true;
			}
			else
			{
				warningLine.enabled = true;
			}
			warningLineOn = true;
			break;
		case "WarningLineOff":
			warningLine.enabled = false;
			warningLineOn = false;
			warningLine_H.enabled = false;
			break;
		case "CheckRecycle":
			if (Boss9.Inst.myPpt.AlreadyDead)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss9.Inst.myPpt.myEntity);
		info.damage = damage;
		info.knockbackForce = rotateObj.transform.up.normalized * knockBack;
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 2097152u:
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position, 3f);
			break;
		case 32768u:
		case 131072u:
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	public void SetReadyEffectPosition(bool isUp, bool isTilt)
	{
		if (isUp)
		{
			this.isUp = true;
			Vector3 position = holeSpriteRenderer.transform.position;
			position -= new Vector3(0f, 0f, 5f);
			readyEffect.transform.position = position;
		}
		else if (isTilt)
		{
			Vector3 localPosition = Vector3.zero + base.transform.up * 0.4f;
			localPosition.z = 1f;
			readyEffect.transform.localPosition = localPosition;
		}
		else
		{
			readyEffect.transform.localPosition = Vector3.zero;
		}
	}
}
