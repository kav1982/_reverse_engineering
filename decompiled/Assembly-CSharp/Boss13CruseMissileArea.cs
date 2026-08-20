using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss13CruseMissileArea : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public Vector3 moveDir;

	public float moveSpeed;

	public bool moving;

	public float areaSize;

	public float areaDuration;

	[Space(50f)]
	public MeshRenderer mr_Area;

	public Transform tsf_Fill;

	private float durationTimer;

	private bool zoomDirect;

	public bool destory;

	public Entity thisEntity { get; set; }

	private void Update()
	{
		if (moving)
		{
			base.transform.position += moveDir * moveSpeed * Time.deltaTime;
		}
		if (zoomDirect || destory)
		{
			durationTimer += Time.deltaTime;
			tsf_Fill.localScale = Vector3.one * Mathf.Lerp(0f, areaSize * 2f, durationTimer / areaDuration);
			if (durationTimer > areaDuration)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13CruseMissile", base.transform.position + new Vector3(0f, 0f, -20f)).GetComponent<Boss13CruseMissile>().StartFall();
				destory = false;
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		else if (durationTimer > 0f)
		{
			durationTimer -= Time.deltaTime;
			tsf_Fill.localScale = Vector3.one * Mathf.Lerp(0f, areaSize * 2f, durationTimer / areaDuration);
		}
	}

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228992u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		moving = true;
		moveDir = Tool2D.GetDir();
		zoomDirect = false;
		mr_Area.material.SetFloat("_Radius", areaSize);
		mr_Area.transform.localScale = Vector3.one * areaSize * 2f;
		tsf_Fill.localScale = Vector3.zero;
		destory = false;
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
		mr_Area.transform.localScale = Vector3.zero;
		tsf_Fill.localScale = Vector3.zero;
		durationTimer = 0f;
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 512u:
		case 2097152u:
			zoomDirect = true;
			break;
		case 256u:
			moveDir = Tool2D.GetDir();
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if (layer == 512 || layer == 2097152)
		{
			zoomDirect = false;
		}
	}
}
