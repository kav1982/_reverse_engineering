using UnityEngine;

public class Boss13CruseMissile : MonoBehaviour
{
	public Transform spriteTransform;

	public Rigidbody Rigid;

	private bool baseIsJumping;

	private float baseJumpUpForce;

	private float baseJumpGravity;

	private Vector3 moveDir;

	private float forwardSpeed;

	private float distance;

	public float gravity;

	public bool isUpward;

	public GameObject warningLaser;

	public void SetLaser(Vector3 landPoint)
	{
		Boss13FCMissileWarningLaser component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13WarningLaser", base.transform.position).GetComponent<Boss13FCMissileWarningLaser>();
		component.monster9Laser.SetLaser(base.transform.position, landPoint);
		warningLaser = component.gameObject;
	}

	private void OnDisable()
	{
		ObjPoolMgr.Inst.RecycleGO(warningLaser);
	}

	public void StartParabola(Vector3 landPoint, float upForce)
	{
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -0.1f);
		moveDir = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position);
		distance = Vector3.Distance(base.transform.position, landPoint);
		forwardSpeed = GeneralTool.CannonSpeed(upForce, 0f, gravity, distance);
		Rigid.linearVelocity = moveDir * forwardSpeed;
		ParabolaStart(upForce, gravity);
	}

	public void ParabolaStart(float upForce, float gravity)
	{
		if (!baseIsJumping)
		{
			baseIsJumping = true;
			baseJumpUpForce = upForce;
			baseJumpGravity = gravity;
		}
	}

	public void StartFall()
	{
		baseIsJumping = true;
		baseJumpUpForce = -10f;
		baseJumpGravity = gravity;
		Rigid.linearVelocity = Vector3.zero;
	}

	public void ParabolaStop()
	{
		if (baseIsJumping)
		{
			baseIsJumping = false;
			baseJumpUpForce = 0f;
			baseJumpGravity = 0f;
		}
	}
}
