using UnityEngine;

public class Elite14_Bullet : MonoBehaviour
{
	public enum BulletState
	{
		Hide,
		Fly,
		Recycle
	}

	[Header("状态机")]
	public BulletState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("数值")]
	public float waitTime;

	public float speed;

	public float duration;

	public int damage;

	public float recycleTime;

	public Vector3 direction;

	public float knockBack;

	[Header("表现")]
	public ParticleSystem flyParticle;

	public ParticleSystem explodeParticle;

	public CapsuleCollider thisCollider;

	public BulletState state
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

	public void Initialize(float waitTime, Vector3 direction, float speed, float duration)
	{
		this.waitTime = waitTime;
		this.speed = speed;
		this.duration = duration;
		this.direction = direction;
		thisCollider.enabled = false;
	}

	private void Update()
	{
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
		case BulletState.Hide:
			_ = changedState;
			break;
		case BulletState.Fly:
			if (changedState)
			{
				flyParticle.Play();
				thisCollider.enabled = true;
			}
			base.transform.position += Time.deltaTime * speed * direction;
			break;
		case BulletState.Recycle:
			if (changedState)
			{
				flyParticle.Stop();
				flyParticle.Clear();
				explodeParticle.Play();
			}
			if (stateExistTime > recycleTime)
			{
				Elite14.MiniPool.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (state == BulletState.Recycle)
		{
			return;
		}
		TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
		takeDamageInfo.damage = damage;
		takeDamageInfo.knockbackForce = direction * knockBack;
		takeDamageInfo.teammateTakeDamageRatio = 3f;
		string text = "EF_Monster51_Hit";
		if (GameMgr.IsHarmony_Static)
		{
			text = "EF_Monster51_Hit_H";
		}
		switch (other.tag)
		{
		case "Player":
			if (other.IsPlayerTrigger())
			{
				UnitProperty component = other.GetComponent<UnitProperty>();
				Elite14.MiniPool.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
				SEMgr.Inst.elite9BladeHit.PlaySE();
				component.TakeDamage(damage, Elite14.Inst.myPpt, takeDamageInfo);
				state = BulletState.Recycle;
			}
			break;
		case "Teammate":
		{
			UnitProperty component = other.GetComponent<UnitProperty>();
			Elite14.MiniPool.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
			SEMgr.Inst.elite9BladeHit.PlaySE();
			component.TakeDamage(damage, Elite14.Inst.myPpt, takeDamageInfo);
			state = BulletState.Recycle;
			break;
		}
		case "Brittleness":
		{
			UnitProperty component = other.GetComponent<UnitProperty>();
			component.TakeDamage(damage, Elite14.Inst.myPpt, takeDamageInfo);
			break;
		}
		case "Cliff":
			Elite14.MiniPool.GetGO("Prefabs/EF/" + text, base.transform.position, 3f);
			state = BulletState.Recycle;
			break;
		case "Wall":
			Elite14.MiniPool.GetGO("Prefabs/EF/" + text, other.transform.position, 3f);
			state = BulletState.Recycle;
			break;
		case "Destructible":
		{
			UnitProperty component = other.GetComponent<UnitProperty>();
			Elite14.MiniPool.GetGO("Prefabs/EF/" + text, component.transform.position, 3f);
			SEMgr.Inst.elite9BladeHit.PlaySE();
			component.TakeDamage(999f, Elite14.Inst.myPpt, takeDamageInfo);
			if (!component.AlreadyDead)
			{
				state = BulletState.Recycle;
			}
			break;
		}
		}
	}
}
