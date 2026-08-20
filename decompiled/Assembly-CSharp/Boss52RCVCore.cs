using UnityEngine;

public class Boss52RCVCore : MonoBehaviour
{
	private float timer;

	private float coreDuration;

	private Vector3 direction;

	private float speed;

	private float chasePower;

	private float chasePowerDecaySpeed;

	private bool isCoreActive;

	private Vector3 currentCoreVelocity;

	private float delayStartTimer;

	private bool isCoreStartMove;

	private float scatter;

	private void OnEnable()
	{
		isCoreActive = false;
		isCoreStartMove = false;
	}

	public void InitCoreData(Vector3 initDirection, float moveSpeed, float chasePower, float chasePowerDecayRatio, float coreDuration, float delayStartTimer, float scatter)
	{
		this.coreDuration = coreDuration;
		direction = initDirection;
		speed = moveSpeed;
		this.chasePower = chasePower;
		chasePowerDecaySpeed = chasePowerDecayRatio;
		currentCoreVelocity = initDirection * moveSpeed;
		isCoreActive = true;
		timer = 0f;
		this.delayStartTimer = delayStartTimer;
		this.scatter = scatter;
	}

	private void Update()
	{
		if (!isCoreActive)
		{
			return;
		}
		timer += Time.deltaTime;
		if (!(timer < delayStartTimer))
		{
			if (isCoreStartMove)
			{
				isCoreStartMove = true;
				direction = Tool2D.GetDir(Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position), scatter);
			}
			base.transform.position += currentCoreVelocity * Time.deltaTime;
			if (timer >= coreDuration)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}
}
