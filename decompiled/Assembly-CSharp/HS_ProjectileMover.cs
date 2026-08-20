using System.Collections;
using UnityEngine;

public class HS_ProjectileMover : MonoBehaviour
{
	[SerializeField]
	protected float speed = 15f;

	[SerializeField]
	protected float hitOffset;

	[SerializeField]
	protected bool UseFirePointRotation;

	[SerializeField]
	protected Vector3 rotationOffset = new Vector3(0f, 0f, 0f);

	[SerializeField]
	protected GameObject hit;

	[SerializeField]
	protected ParticleSystem hitPS;

	[SerializeField]
	protected GameObject flash;

	[SerializeField]
	protected Rigidbody rb;

	[SerializeField]
	protected Collider col;

	[SerializeField]
	protected Light lightSourse;

	[SerializeField]
	protected GameObject[] Detached;

	[SerializeField]
	protected ParticleSystem projectilePS;

	private bool startChecker;

	[SerializeField]
	protected bool notDestroy;

	protected virtual void Start()
	{
		if (!startChecker && flash != null)
		{
			flash.transform.parent = null;
		}
		if (notDestroy)
		{
			StartCoroutine(DisableTimer(5f));
		}
		else
		{
			Object.Destroy(base.gameObject, 5f);
		}
		startChecker = true;
	}

	protected virtual IEnumerator DisableTimer(float time)
	{
		yield return new WaitForSeconds(time);
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	protected virtual void OnEnable()
	{
		if (startChecker)
		{
			if (flash != null)
			{
				flash.transform.parent = null;
			}
			if (lightSourse != null)
			{
				lightSourse.enabled = true;
			}
			col.enabled = true;
			rb.constraints = RigidbodyConstraints.None;
		}
	}

	protected virtual void FixedUpdate()
	{
		if (speed != 0f)
		{
			rb.linearVelocity = base.transform.forward * speed;
		}
	}

	protected virtual void OnCollisionEnter(Collision collision)
	{
		rb.constraints = RigidbodyConstraints.FreezeAll;
		if (lightSourse != null)
		{
			lightSourse.enabled = false;
		}
		col.enabled = false;
		if ((bool)projectilePS)
		{
			projectilePS.Stop();
			projectilePS.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		ContactPoint contactPoint = collision.contacts[0];
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
		Vector3 position = contactPoint.point + contactPoint.normal * hitOffset;
		if (hit != null)
		{
			hit.transform.rotation = rotation;
			hit.transform.position = position;
			if (UseFirePointRotation)
			{
				hit.transform.rotation = base.gameObject.transform.rotation * Quaternion.Euler(0f, 180f, 0f);
			}
			else if (rotationOffset != Vector3.zero)
			{
				hit.transform.rotation = Quaternion.Euler(rotationOffset);
			}
			else
			{
				hit.transform.LookAt(contactPoint.point + contactPoint.normal);
			}
			hitPS.Play();
		}
		GameObject[] detached = Detached;
		foreach (GameObject gameObject in detached)
		{
			if (gameObject != null)
			{
				gameObject.GetComponent<ParticleSystem>().Stop();
			}
		}
		if (notDestroy)
		{
			StartCoroutine(DisableTimer(hitPS.main.duration));
		}
		else if (hitPS != null)
		{
			Object.Destroy(base.gameObject, hitPS.main.duration);
		}
		else
		{
			Object.Destroy(base.gameObject, 1f);
		}
	}
}
