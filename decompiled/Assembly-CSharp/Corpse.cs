using UnityEngine;

public class Corpse : LayerCorrect
{
	private enum CorpseState
	{
		Fly,
		Dark,
		Stay
	}

	[Space(50f)]
	public Transform tsf_Model;

	public Transform tsf_Shadow;

	public Rigidbody rigid;

	public SphereCollider sc;

	public VariableFloat forwardForceNoDirect;

	public VariableFloat forwardForceHaveDirect;

	public VariableFloat upForce;

	public VariableFloat scale;

	public VariableInt bounceTime;

	public VariableFloat rotateSpeed;

	public float angleOffset;

	public float bounceRemainRatio;

	public float gravity;

	public float duration;

	[Header("Dark")]
	public SpriteRenderer sr;

	public float reduceAlphaSpeed;

	public float minAlpha;

	public bool isEnemyCorpse;

	public bool isBulletShell;

	private CorpseState state;

	private float currentUpSpeed;

	private float currentAlpha = 1f;

	private float durationTimer;

	private int bounceTimer;

	private float currentRotateSpeed;

	public bool isEnterTheGungeon;

	public Vector3 rotate;

	private void Update()
	{
		switch (state)
		{
		case CorpseState.Fly:
			currentUpSpeed += gravity * Time.deltaTime;
			base.transform.position += new Vector3(0f, 0f, 0f - currentUpSpeed) * Time.deltaTime;
			if (isEnterTheGungeon)
			{
				sr.transform.rotation = Quaternion.Slerp(sr.transform.rotation, Quaternion.Euler(rotate), Time.deltaTime * 5f);
			}
			else
			{
				sr.transform.Rotate(0f, 0f, currentRotateSpeed * Time.deltaTime);
			}
			tsf_Shadow.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
			tsf_Shadow.localRotation = sr.transform.localRotation;
			if (!(base.transform.position.z >= 0f) || !(currentUpSpeed < 0f))
			{
				break;
			}
			if (bounceTimer >= bounceTime.result)
			{
				state = CorpseState.Dark;
				everyFrame = false;
				tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Corpse);
				rigid.linearVelocity = Vector3.zero;
				tsf_Shadow.gameObject.SetActive(value: false);
				sc.enabled = false;
			}
			else
			{
				bounceTimer++;
				currentUpSpeed = (0f - currentUpSpeed) * bounceRemainRatio;
				if (isBulletShell)
				{
					SEMgr.Inst.monster12Land.PlaySE();
				}
			}
			break;
		case CorpseState.Dark:
			currentAlpha -= reduceAlphaSpeed * Time.deltaTime;
			if (currentAlpha < minAlpha)
			{
				currentAlpha = minAlpha;
				state = CorpseState.Stay;
			}
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, currentAlpha);
			break;
		case CorpseState.Stay:
			if (duration > 0f)
			{
				durationTimer += Time.deltaTime;
				if (durationTimer >= duration)
				{
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public float GetDurationTimer()
	{
		return durationTimer;
	}

	public void Initialize(Vector3 knockback, float upSpeed)
	{
		Initialize(knockback);
		currentUpSpeed = upSpeed;
	}

	public void Initialize(Vector3 knockback)
	{
		tsf_Model.localScale = Vector3.one * scale.RandomResult();
		tsf_Shadow.localScale = tsf_Model.localScale;
		tsf_Shadow.rotation = sr.transform.rotation;
		if (isEnemyCorpse)
		{
			sr.transform.localEulerAngles = Vector3.zero;
		}
		else
		{
			sr.transform.rotation = Tool2D.GetRotation();
		}
		currentRotateSpeed = rotateSpeed.RandomResult() * (float)((Random.Range(0, 2) == 0) ? 1 : (-1));
		state = CorpseState.Fly;
		everyFrame = true;
		durationTimer = 0f;
		bounceTimer = 0;
		tsf_Shadow.gameObject.SetActive(value: true);
		currentAlpha = 1f;
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
		sc.enabled = true;
		if (knockback == Vector3.zero)
		{
			rigid.linearVelocity = Tool2D.GetDir() * forwardForceNoDirect.RandomResult();
		}
		else if (isEnemyCorpse)
		{
			rigid.linearVelocity = Tool2D.GetDir(knockback, Random.Range((0f - angleOffset) / 2f, angleOffset / 2f));
		}
		else
		{
			rigid.linearVelocity = Tool2D.GetDir(knockback.normalized, Random.Range((0f - angleOffset) / 2f, angleOffset / 2f)) * forwardForceHaveDirect.RandomResult();
		}
		if (isEnterTheGungeon)
		{
			rotate = new Vector3(0f, 0f, Random.Range(70f, 110f)) * (Random.Range(0, 2) * 2 - 1);
		}
		currentUpSpeed += upForce.RandomResult();
		bounceTime.RandomResult();
	}
}
