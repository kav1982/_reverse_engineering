using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Boss6_Laser : MonoBehaviour
{
	public enum LaserState
	{
		Stop,
		Aim,
		Attack,
		AttackAfter
	}

	[Header("状态")]
	public LaserState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private float stateExistTime;

	[Header("判定")]
	public int damage;

	public float checkInterval;

	public float damageInterval;

	public float laserWidth;

	public LayerMask attackLayer;

	private float checkIntervalTimer;

	private List<UnitProperty> laserAttackPpt = new List<UnitProperty>();

	[Header("预警")]
	public float laserWarningTime;

	public float warningMaxAlpha;

	public AnimationCurve warningTransparencyCurve;

	[Header("伤害")]
	public AnimationCurve laserWidthCurve;

	public AnimationCurve laserTransparencyCurve;

	public float laserExistTime;

	public float startDamageWidth;

	private float originLaserWidth;

	private float originShadowWidth;

	[Header("表现")]
	public Boss6 master;

	public Transform tsf_AimStart;

	public ParticleSystem startParticle;

	public ParticleSystem endParticle;

	public ParticleSystem endParticleGround;

	public float bubbleCountPerMeter;

	public VisualEffect ve_Bubble;

	public int lrPoints;

	public float height;

	public LineRenderer lr_Aim;

	public LineRenderer lr_Laser;

	public LineRenderer lr_LaserShadow;

	public Vector3 attackStartPoint;

	public Vector3 attackEndPoint;

	public Vector3 startPoint;

	public Vector3 endPoint;

	public static float roomWidth;

	public static float roomHeight;

	public static Vector3 roomCenter;

	[Header("音效")]
	public AudioSource as_laserLoop;

	public AudioSource as_LaserLoopBG;

	public LaserState state
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
			varMgr.Clear();
		}
	}

	public void Initialize(Boss6 master, Transform startRoot)
	{
		this.master = master;
		tsf_AimStart = startRoot;
		state = LaserState.Stop;
		lr_Laser.positionCount = lrPoints;
		lr_Aim.positionCount = lrPoints;
		lr_LaserShadow.positionCount = lrPoints;
		ve_Bubble.transform.position = Vector3.zero;
	}

	public void SetAimPoint(Vector3 attackStartPoint)
	{
		this.attackStartPoint = attackStartPoint;
		state = LaserState.Aim;
	}

	public void StartAttack(Vector3 endPoint)
	{
		attackEndPoint = endPoint;
		state = LaserState.Attack;
	}

	private void Start()
	{
		originLaserWidth = lr_Laser.widthMultiplier;
		originShadowWidth = lr_LaserShadow.widthMultiplier;
	}

	private void Update()
	{
		startPoint = tsf_AimStart.transform.position;
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case LaserState.Stop:
			if (changedState)
			{
				startParticle.Stop();
				endParticle.Stop();
				endParticleGround.Stop();
				lr_Aim.enabled = false;
				lr_Laser.enabled = false;
				lr_LaserShadow.enabled = false;
			}
			break;
		case LaserState.Aim:
			if (changedState)
			{
				lr_LaserShadow.enabled = false;
				lr_Laser.enabled = false;
				lr_Aim.enabled = true;
				endPoint = attackStartPoint;
			}
			lr_Aim.material.SetFloat("_Transparency", stateExistTime / laserWarningTime * warningMaxAlpha);
			lr_LaserShadow.material.SetFloat("_Transparency", stateExistTime / laserWarningTime * warningMaxAlpha);
			if (stateExistTime > laserWarningTime)
			{
				stateExistTime = laserWarningTime;
			}
			break;
		case LaserState.Attack:
			if (changedState)
			{
				laserAttackPpt.Clear();
				startParticle.Play();
				endParticle.Play();
				endParticleGround.Play();
				lr_Laser.enabled = true;
				lr_Aim.enabled = false;
				lr_LaserShadow.enabled = true;
				checkIntervalTimer = 0f;
			}
			endPoint = Vector3.Lerp(attackStartPoint, attackEndPoint, stateExistTime / laserExistTime);
			DealGroundDamage();
			if (stateExistTime > laserExistTime)
			{
				state = LaserState.AttackAfter;
			}
			break;
		case LaserState.AttackAfter:
			if (changedState)
			{
				startParticle.Stop();
				endParticle.Stop();
				endParticleGround.Stop();
				lr_Aim.enabled = false;
				lr_Laser.enabled = false;
				lr_LaserShadow.enabled = false;
				ve_Bubble.gameObject.SetActive(value: false);
				ve_Bubble.SetFloat("Count", Tool2D.IgnoreZDistance(lr_Laser.GetPosition(0), lr_Laser.GetPosition(lrPoints - 1)) * bubbleCountPerMeter);
				ve_Bubble.SetVector3("Position0", lr_Laser.GetPosition(0));
				ve_Bubble.SetVector3("Position1", lr_Laser.GetPosition(lrPoints - 1));
				if (!GameMgr.IsMobile_Static)
				{
					ve_Bubble.gameObject.SetActive(value: true);
				}
			}
			break;
		}
		Vector3 vector = startPoint;
		Vector3 vector2 = endPoint;
		for (int i = 0; i < lr_Aim.positionCount; i++)
		{
			lr_Aim.SetPosition(i, Vector3.Lerp(vector, Tool2D.GetLayerPoint(vector2), (float)i / (float)(lrPoints - 1)));
			lr_Laser.SetPosition(i, Vector3.Lerp(vector, Tool2D.GetLayerPoint(vector2), (float)i / (float)(lrPoints - 1)));
			if (master.recordBezierPoints.Count > 0)
			{
				lr_LaserShadow.SetPosition(i, Tool2D.GetLayerPoint(Vector3.Lerp(Tool2D.IgnoreZPoint(master.recordBezierPoints[0]), vector2, (float)i / (float)(lrPoints - 1)), LayerCorrectType.Shadow));
			}
		}
		startParticle.transform.position = vector;
		endParticle.transform.position = Tool2D.GetLayerPoint(endPoint);
		endParticleGround.transform.position = Tool2D.GetLayerPoint(endPoint, LayerCorrectType.GroundEffect);
	}

	public void DealGroundDamage()
	{
		checkIntervalTimer += Time.deltaTime;
		if (!(checkIntervalTimer >= checkInterval))
		{
			return;
		}
		checkIntervalTimer -= checkInterval;
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(endPoint, laserWidth, "Player", "Teammate");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
			UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
			takeDamageInfo.teammateTakeDamageRatio = 4f;
			if (!laserAttackPpt.Contains(component))
			{
				laserAttackPpt.Add(component);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite9_Hit", collidersByTag[i].transform.position, 2f);
				component.TakeDamage(damage, null, takeDamageInfo);
			}
		}
	}

	public void DealDamage()
	{
		checkIntervalTimer += Time.deltaTime;
		if (!(checkIntervalTimer >= checkInterval))
		{
			return;
		}
		checkIntervalTimer -= checkInterval;
		RaycastHit[] array = Physics.SphereCastAll(startPoint, laserWidth, endPoint - startPoint, (endPoint - startPoint).magnitude, attackLayer);
		for (int i = 0; i < array.Length; i++)
		{
			UnitProperty component = array[i].transform.GetComponent<UnitProperty>();
			if (!(component == null))
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite11_LaserHit", array[i].point, 3f);
				if (component.PlayerCtrller != null)
				{
					component.TakeDamage(damage, AttackerType.NothingSpecial);
				}
				else
				{
					component.TakeDamage(damage * 4, AttackerType.NothingSpecial);
				}
			}
		}
	}
}
