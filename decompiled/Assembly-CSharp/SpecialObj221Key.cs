using System.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class SpecialObj221Key : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever, IDotsCollisionReceiver
{
	public UnityEngine.CapsuleCollider thisTrigger;

	public UnityEngine.CapsuleCollider thisCollider;

	public SpecialObj221 specialObj221;

	public SpriteRenderer spriterenderer;

	public Animator keyAnimator;

	public int id;

	public Coroutine IeChangeSprite;

	public Transform noteApearPosition;

	public Entity thisEntity { get; set; }

	private void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisTrigger);
	}

	public void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public IEnumerator ChangeSprite()
	{
		spriterenderer.sprite = specialObj221.spriteKey;
		yield return new WaitForSeconds(0.1f);
		specialObj221.NoteAppearance(noteApearPosition.transform.position, 1f);
		spriterenderer.sprite = specialObj221.spriteKeyActive;
		yield return new WaitForSeconds(0.5f);
		spriterenderer.sprite = specialObj221.spriteKey;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (!specialObj221.IsComplete && other == PlayerMgr.Inst.PlayerEtt)
		{
			specialObj221.PlayANote(id, FromMusicBox: false);
			if (!specialObj221.IsComplete)
			{
				specialObj221.TryAddKey(id.ToString());
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
