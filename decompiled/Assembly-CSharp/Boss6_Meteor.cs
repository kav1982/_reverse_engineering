using UnityEngine;

public class Boss6_Meteor : MonoBehaviour
{
	public enum meteorState
	{
		Prepare,
		Shoot,
		Fade
	}

	[Header("数值")]
	public VariableFloat speed;

	public int damage;

	public float knockBack;

	public Collider thisCollider;

	[Header("表现")]
	public Shadow thisShadow;

	public float height;

	public Transform tsf_Bullet;

	public ParticleSystem ballParticle;

	public AnimationCurve fadeCurve;

	public float fadeTime;

	[Header("状态")]
	public meteorState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("杂项")]
	public float warningTime;

	private Vector3 diration;

	private float roomWidth;

	private float roomHeight;

	private Vector3 roomCenter;

	public meteorState state
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

	public void Initialize(Vector3 dir, float forceSpeed = 0f)
	{
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		diration = dir;
		state = meteorState.Prepare;
		ballParticle.Clear();
		thisShadow.Show();
		thisCollider.enabled = true;
		speed.RandomResult();
		if (forceSpeed > 0f)
		{
			speed.result = forceSpeed;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		tsf_Bullet.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0f - height), LayerCorrectType.Coordinate);
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
		case meteorState.Prepare:
			_ = changedState;
			if (stateExistTime > warningTime)
			{
				state = meteorState.Shoot;
			}
			break;
		case meteorState.Shoot:
			if (changedState)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_MeteorShoot", base.transform.position, 3f);
				ballParticle.Play();
			}
			base.transform.position += Time.deltaTime * speed.result * diration;
			if ((diration == Vector3.right && base.transform.position.x > roomCenter.x + roomWidth / 2f) || (diration == Vector3.left && base.transform.position.x < roomCenter.x - roomWidth / 2f))
			{
				state = meteorState.Fade;
			}
			break;
		case meteorState.Fade:
			if (changedState)
			{
				ballParticle.Stop();
				thisShadow.Hide();
				thisCollider.enabled = false;
			}
			if (stateExistTime > fadeTime)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
			break;
		}
	}

	public void Mute()
	{
		if (state != meteorState.Fade)
		{
			state = meteorState.Fade;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
		takeDamageInfo.damage = damage;
		takeDamageInfo.knockbackForce = diration * knockBack;
		takeDamageInfo.teammateTakeDamageRatio = 3f;
		switch (other.tag)
		{
		case "Player":
			other.GetComponent<UnitProperty>().TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
			state = meteorState.Fade;
			break;
		case "Teammate":
			other.GetComponent<UnitProperty>().TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
			state = meteorState.Fade;
			break;
		case "Brittleness":
			other.GetComponent<UnitProperty>().TakeDamage(damage, AttackerType.NothingSpecial, takeDamageInfo);
			state = meteorState.Fade;
			break;
		case "Destructible":
			other.GetComponent<UnitProperty>().TakeDamage(999f, AttackerType.NothingSpecial, takeDamageInfo);
			state = meteorState.Fade;
			break;
		case "Wall":
			break;
		}
	}
}
