using System.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj218_PointObj : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider thisCollider;

	public GameObject LightFrame;

	public SpecialObj218 Special218;

	public SpecialObj218.Point thisPoint;

	public SpriteRenderer SpriteRenterer;

	private Coroutine lightup;

	private Coroutine lightdown;

	public bool interacting;

	public bool Danger;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void LightUpCenter(int index)
	{
		Special218.AllPoitObjs[index].SpriteRenterer.material.SetColor("_MainColor", Special218.ColorSelected);
	}

	private IEnumerator LightUpCenterAndFrame(int index)
	{
		Special218.AllPoitObjs[index].LightFrame.gameObject.SetActive(value: true);
		float time = 0f;
		Color thiscolor = Special218.AllPoitObjs[index].SpriteRenterer.material.GetColor("_MainColor");
		while (time < Special218.ColorChangeTime)
		{
			time += Time.deltaTime;
			if (time > Special218.ColorChangeTime)
			{
				time = Special218.ColorChangeTime;
			}
			Special218.AllPoitObjs[index].SpriteRenterer.material.SetColor("_MainColor", Color.Lerp(thiscolor, Special218.ColorSelected, time / Special218.ColorChangeTime));
			yield return new WaitForFixedUpdate();
		}
		lightup = null;
	}

	private IEnumerator iLightDown(int index)
	{
		Special218.AllPoitObjs[index].LightFrame.gameObject.SetActive(value: false);
		Special218.AllPoitObjs[index].SpriteRenterer.material.SetColor("_MainColor", Special218.ColorNormal);
		yield return new WaitForFixedUpdate();
	}

	public void LightDown()
	{
		StartCoroutine(iLightDown(thisPoint.id));
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (interacting || !(other == PlayerMgr.Inst.PlayerEtt))
		{
			return;
		}
		interacting = true;
		if (!Special218.LineCheck(thisPoint.id))
		{
			return;
		}
		if (Special218.inderactingPointIndex != -1 && Special218.inderactingPointIndex != thisPoint.id)
		{
			Special218.AllPoitObjs[Special218.inderactingPointIndex].interacting = false;
			if (Special218.CheckCompletePoint(Special218.inderactingPointIndex))
			{
				lightup = StartCoroutine(LightUpCenterAndFrame(Special218.inderactingPointIndex));
			}
			Special218.inderactingPointIndex = thisPoint.id;
		}
		if (Special218.CheckCompletePoint(thisPoint.id))
		{
			lightup = StartCoroutine(LightUpCenterAndFrame(Special218.inderactingPointIndex));
		}
		else
		{
			LightUpCenter(thisPoint.id);
		}
		SEMgr.Inst.puzzleClick.PlaySE();
		Special218.CheckComplete();
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
