using UnityEngine;

public class Boss13_GravityBombBullet : MonoBehaviour
{
	public Rigidbody Rigid;

	private bool baseIsJumping;

	private float baseJumpUpForce;

	private float baseJumpGravity;

	private Vector3 moveDir;

	private float distance;

	public float gravity;

	private void Update()
	{
		if (baseIsJumping)
		{
			baseJumpUpForce += baseJumpGravity * Time.deltaTime;
			if (baseJumpUpForce != 0f)
			{
				base.transform.position -= new Vector3(0f, 0f, baseJumpUpForce * Time.deltaTime);
			}
		}
		if (base.transform.position.z > 0f)
		{
			ParabolaStop();
			ObjPoolMgr.Inst.GetGO("Prefabs/Units/501341", base.transform.position);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void StartParabola(Vector3 landPoint, float upForce)
	{
		moveDir = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position);
		distance = Vector3.Distance(base.transform.position, landPoint);
		float num = GeneralTool.CannonSpeed(upForce, 0f, gravity, distance);
		Rigid.linearVelocity = moveDir * num;
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
