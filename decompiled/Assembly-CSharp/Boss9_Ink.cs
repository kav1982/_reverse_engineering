using System.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Boss9_Ink : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public float speed;

	public float force;

	public Vector3 dir;

	public ParticleSystem idle;

	public ParticleSystem boom;

	private bool stop;

	public UnityEngine.Collider col;

	public GameObject shadow;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		stop = false;
		col.enabled = true;
		shadow.SetActive(value: true);
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228992u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 256u:
		case 2097152u:
			idle.Stop();
			boom.Play();
			StartCoroutine(SetActiveFalse());
			stop = true;
			col.enabled = false;
			shadow.SetActive(value: false);
			break;
		case 512u:
			if (PlayerMgr.Inst.PlayerEtt == other)
			{
				if (PlayerMgr.Inst.ItemCtrller.curse_DarkView == null)
				{
					Boss9.Inst.AddBlinded();
				}
				idle.Stop();
				boom.Play();
				StartCoroutine(SetActiveFalse());
				stop = true;
				col.enabled = false;
				shadow.SetActive(value: false);
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002045.GetText(), UITextFloatType.Normal, base.transform.position);
			}
			else
			{
				idle.Stop();
				boom.Play();
				StartCoroutine(SetActiveFalse());
				stop = true;
				col.enabled = false;
				shadow.SetActive(value: false);
			}
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	private void Update()
	{
		if (!stop)
		{
			base.transform.Translate(dir * speed * Time.deltaTime);
		}
	}

	private IEnumerator SetActiveFalse()
	{
		yield return new WaitForSeconds(20f);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
