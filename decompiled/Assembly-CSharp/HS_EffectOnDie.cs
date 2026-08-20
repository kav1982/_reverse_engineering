using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_EffectOnDie : MonoBehaviour
{
	public List<GameObject> pooledObjects;

	public GameObject EffectsOnDie;

	public int poolSize;

	public float poolReturnTimer = 1.5f;

	private void Start()
	{
		pooledObjects = new List<GameObject>();
		for (int i = 0; i < poolSize; i++)
		{
			GameObject gameObject = Object.Instantiate(EffectsOnDie, base.transform);
			gameObject.SetActive(value: false);
			pooledObjects.Add(gameObject);
		}
	}

	public void LateUpdate()
	{
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
		int particles = GetComponent<ParticleSystem>().GetParticles(array);
		for (int i = 0; i < particles; i++)
		{
			if (EffectsOnDie != null && array[i].remainingLifetime < Time.deltaTime)
			{
				GameObject gameObject = GetPooledObjects();
				if (gameObject != null)
				{
					gameObject.transform.position = array[i].position;
					gameObject.SetActive(value: true);
					StartCoroutine(LateCall(gameObject));
				}
			}
		}
	}

	public GameObject GetPooledObjects()
	{
		for (int i = 0; i < poolSize; i++)
		{
			if (!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}
		return null;
	}

	private IEnumerator LateCall(GameObject soundInstance)
	{
		yield return new WaitForSeconds(poolReturnTimer);
		soundInstance.SetActive(value: false);
	}
}
