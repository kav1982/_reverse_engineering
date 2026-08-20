using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.AI;

public class SpecialObj9 : LayerCorrect, ITrap, IRoomObjExtraData, IDotsPhysicsHolder, IDotsPhysicsReciever
{
	[Space(50f)]
	public Animator anima;

	public UnityEngine.BoxCollider boxCollider;

	public NavMeshObstacle nmo;

	public int bindId = -1;

	public Entity thisEntity { get; set; }

	private void _OpenFinish()
	{
		boxCollider.enabled = false;
		nmo.enabled = false;
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.AccessOpen);
	}

	private void _CloseFinish()
	{
		boxCollider.enabled = true;
		nmo.enabled = true;
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Coordinate);
	}

	private void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 256u;
		collisionFilter.CollidesWith = DTool.GetCollidesWith(256u);
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, boxCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void SetTrapInvalid()
	{
		anima.SetTrigger("Open");
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		if (data1 >= 1f)
		{
			bindId = (int)data1;
		}
	}
}
