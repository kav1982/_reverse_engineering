using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss13Device : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public ParticleSystem fire;

	public Vector3 currentDir;

	public Vector3 targetDir;

	public float rotateSpeed;

	public float damage;

	public LineRenderer warningLine;

	public Transform layer;

	public Transform motion;

	public SpriteRenderer spriteRenderer;

	public float currentAlpha;

	public float fadeSpeed;

	public float positionOffset;

	public float spawnMoveSpeed;

	public float aimTime;

	public float aimTimer;

	public Vector3 endPoint;

	public float moveSpeed;

	public float knockbackForce;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2231040u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		Vector3 v = PlayerMgr.Inst.PlayerPoint + new Vector3(Random.insideUnitCircle.x, Random.insideUnitCircle.y, 0f) * Random.Range(0f, 2f);
		targetDir = Tool2D.IgnoreZV2ToV1Normal(v, base.transform.position);
		warningLine.gameObject.SetActive(value: true);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
		positionOffset = -1f;
		aimTimer = 0f;
		currentAlpha = 0f;
		motion.localPosition = new Vector3(0f, 0f, 0f);
		warningLine.SetPosition(0, Tool2D.GetLayerPoint(motion.transform.position));
		warningLine.SetPosition(1, Tool2D.GetLayerPoint(motion.transform.position));
		warningLine.SetPosition(2, Tool2D.GetLayerPoint(motion.transform.position));
		warningLine.gameObject.SetActive(value: false);
		fire.Stop();
	}

	private void Update()
	{
		if (aimTimer < aimTime)
		{
			currentDir = Vector3.Slerp(currentDir, targetDir, rotateSpeed * Time.deltaTime);
			aimTimer += Time.deltaTime;
			positionOffset += spawnMoveSpeed * Time.deltaTime;
			if (currentAlpha < 1f)
			{
				currentAlpha += fadeSpeed * Time.time;
			}
			layer.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, currentDir) + 180f);
			motion.localPosition = new Vector3(0f, positionOffset, 0f);
			spriteRenderer.color = new Color(1f, 1f, 1f, currentAlpha);
			warningLine.SetPosition(0, Tool2D.GetLayerPoint(motion.transform.position));
			UnitDotsSyncSystem.Raycast(motion.transform.position, currentDir, 999f, GameConst.Filter_Wall, out var result);
			warningLine.SetPosition(1, motion.position + Tool2D.IgnoreZV2ToV1Normal(result.point, motion.position) * Tool2D.IgnoreZDistance(result.point, motion.position) / 2f);
			warningLine.SetPosition(2, result.point);
		}
		else
		{
			if (fire.isStopped)
			{
				fire.Play();
			}
			base.transform.position += currentDir * moveSpeed * Time.deltaTime;
		}
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 2097152u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			info.damage = damage;
			info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position, base.transform.position) * knockbackForce;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
		case 256u:
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
