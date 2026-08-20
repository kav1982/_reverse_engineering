using UnityEngine;

public class ParticleGroupPlayController : MonoBehaviour
{
	[Tooltip("是否在Awake时缓存所有粒子系统，避免每次查询开销")]
	public bool cacheAtAwake = true;

	[Tooltip("GetComponentsInChildren 是否包含未激活对象")]
	public bool includeInactive = true;

	private ParticleSystem[] _systems;

	private void Awake()
	{
		if (cacheAtAwake)
		{
			RefreshCache();
		}
	}

	public void RefreshCache()
	{
		_systems = GetComponentsInChildren<ParticleSystem>(includeInactive);
	}

	public void SetSimulationSpeed(float speed)
	{
		EnsureSystems();
		ParticleSystem[] systems = _systems;
		for (int i = 0; i < systems.Length; i++)
		{
			ParticleSystem.MainModule main = systems[i].main;
			main.simulationSpeed = speed;
		}
	}

	public void PauseAll()
	{
		EnsureSystems();
		ParticleSystem[] systems = _systems;
		for (int i = 0; i < systems.Length; i++)
		{
			systems[i].Pause(withChildren: true);
		}
	}

	public void ResumeAll()
	{
		EnsureSystems();
		ParticleSystem[] systems = _systems;
		for (int i = 0; i < systems.Length; i++)
		{
			systems[i].Play(withChildren: true);
		}
	}

	public void StopAndClearAll()
	{
		EnsureSystems();
		ParticleSystem[] systems = _systems;
		for (int i = 0; i < systems.Length; i++)
		{
			systems[i].Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
	}

	public void StopKeepParticles()
	{
		EnsureSystems();
		ParticleSystem[] systems = _systems;
		for (int i = 0; i < systems.Length; i++)
		{
			systems[i].Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
		}
	}

	private void EnsureSystems()
	{
		if (_systems == null || _systems.Length == 0)
		{
			_systems = GetComponentsInChildren<ParticleSystem>(includeInactive);
		}
	}
}
