using UnityEngine;

public class Boss3_Stage2_Ball : EffectController
{
	private enum BallState
	{
		None,
		Fly
	}

	[Space(50f)]
	public GameObject go_Sprite;

	public GameObject go_LandEF;

	public Rigidbody rigid;

	public Shadow shadow;

	private BallState state;

	private Boss3_Stage2 boss3Stage2;

	private float upSpeed;

	private void Start()
	{
		shadow.ShadowGO.SetActive(value: false);
	}

	private void Update()
	{
		switch (state)
		{
		case BallState.None:
			rigid.linearVelocity = Vector2.zero;
			break;
		case BallState.Fly:
			upSpeed += boss3Stage2.attack2Gravity * Time.deltaTime;
			base.transform.position += new Vector3(0f, 0f, 0f - upSpeed) * Time.deltaTime;
			if (base.transform.position.z >= 0f)
			{
				boss3Stage2.BallLand();
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void Initialize(Boss3_Stage2 boss3Stage2, Vector3 dir, Vector3 landPoint)
	{
		this.boss3Stage2 = boss3Stage2;
		state = BallState.Fly;
		ECStartEffect();
		go_Sprite.SetActive(value: true);
		go_LandEF.SetActive(value: false);
		shadow.ShadowGO.SetActive(value: true);
		upSpeed = boss3Stage2.attack2UpSpeed;
		float num = GeneralTool.CannonSpeed(upSpeed, 0f - base.transform.position.z, boss3Stage2.attack2Gravity, Tool2D.IgnoreZDistance(base.transform.position, landPoint));
		rigid.linearVelocity = dir * num;
	}

	public void SetLand()
	{
		state = BallState.None;
		base.transform.position = Tool2D.IgnoreZPoint(base.transform, 0f - boss3Stage2.laserHeight);
		rigid.linearVelocity = Vector2.zero;
		ECStopEffect();
		go_Sprite.SetActive(value: false);
		go_LandEF.SetActive(value: true);
		shadow.ShadowGO.SetActive(value: false);
	}
}
