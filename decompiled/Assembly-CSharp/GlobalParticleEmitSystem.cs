using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public class GlobalParticleEmitSystem : SystemBase
{
	private struct TypeHandle
	{
		[ReadOnly]
		public EntityStorageInfoLookup __EntityStorageInfoLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
		}
	}

	private const int MAX_EMIT_IN_FRAME = 60;

	private readonly Dictionary<(GlobalParticleType, FixedString32Bytes), GlobalParticleEmitter> _particleCache = new Dictionary<(GlobalParticleType, FixedString32Bytes), GlobalParticleEmitter>();

	private GameObject _parent;

	private Entity _singleton;

	private static GlobalParticleEmitSystem _instance;

	private List<GlobalParticleEmitParams> monoEmitParams = new List<GlobalParticleEmitParams>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1499889937_0;

	public static void AddEmitParams(List<GlobalParticleEmitParams> particleEmitParams)
	{
		if (_instance == null)
		{
			return;
		}
		foreach (GlobalParticleEmitParams particleEmitParam in particleEmitParams)
		{
			_instance.monoEmitParams.Add(particleEmitParam);
		}
		particleEmitParams.Clear();
	}

	public static void ClearAllEmitter()
	{
		if (_instance == null)
		{
			return;
		}
		foreach (GlobalParticleEmitter value in _instance._particleCache.Values)
		{
			Object.Destroy(value.gameObject);
		}
		_instance._particleCache.Clear();
	}

	[Preserve]
	protected override void OnCreate()
	{
		_parent = new GameObject("GlobalParticles");
		Object.DontDestroyOnLoad(_parent);
		_singleton = base.EntityManager.CreateSingletonBuffer<GlobalParticleEmitParams>();
		_instance = this;
	}

	[Preserve]
	protected override void OnDestroy()
	{
		_particleCache.Clear();
		if (InternalCompilerInterface.DoesEntityExist(ref __TypeHandle.__EntityStorageInfoLookup, ref base.CheckedStateRef, _singleton))
		{
			base.EntityManager.DestroyEntity(_singleton);
		}
		if ((bool)_parent)
		{
			Object.Destroy(_parent);
		}
	}

	[Preserve]
	protected override void OnUpdate()
	{
		DynamicBuffer<GlobalParticleEmitParams> singletonBuffer = __query_1499889937_0.GetSingletonBuffer<GlobalParticleEmitParams>();
		int num = 0;
		foreach (GlobalParticleEmitParams item in singletonBuffer)
		{
			num++;
			GetParticleWithCache(item.Type, item.Name).Emit(item);
			if (num >= 60)
			{
				break;
			}
		}
		foreach (GlobalParticleEmitParams monoEmitParam in monoEmitParams)
		{
			num++;
			GetParticleWithCache(monoEmitParam.Type, monoEmitParam.Name).Emit(monoEmitParam);
			if (num >= 60)
			{
				break;
			}
		}
		singletonBuffer.Clear();
		monoEmitParams.Clear();
	}

	private GlobalParticleEmitter GetParticleWithCache(GlobalParticleType type, FixedString32Bytes particleName)
	{
		if (!_particleCache.TryGetValue((type, particleName), out var value))
		{
			GameObject prefab = LoadParticlePrefab(type, particleName);
			value = NewParticleSystem(prefab);
			_particleCache[(type, particleName)] = value;
		}
		return value;
	}

	private GlobalParticleEmitter NewParticleSystem(GameObject prefab)
	{
		GameObject gameObject = Object.Instantiate(prefab, _parent.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.SetActive(value: true);
		return ConvertNormalParticleSystemToGlobalParticleSystem(gameObject.GetComponent<ParticleSystem>());
	}

	private GameObject LoadParticlePrefab(GlobalParticleType type, FixedString32Bytes particleName)
	{
		string text = ConvertParticleNameToResourcePath(type, particleName);
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogError("全局粒子名称不正确: " + particleName.Value);
			return null;
		}
		GameObject gameObject = ABResources.LoadAsset<GameObject>(text);
		if (!gameObject)
		{
			Debug.LogError($"找不到全局粒子 {particleName} 的Prefab");
		}
		return gameObject;
	}

	private void OnUpdateWithEditor()
	{
		if (Input.GetKeyDown(KeyCode.N))
		{
			ClearAllEmitter();
		}
	}

	private static string ConvertParticleNameToResourcePath(GlobalParticleType type, FixedString32Bytes particleName)
	{
		switch (type)
		{
		case GlobalParticleType.Spell:
		{
			string text = particleName.Value.Split("_")[0];
			string text2 = "Prefabs/Spell/" + text + "/" + particleName.Value;
			if (GameMgr.IsChAge14_Static && particleName.Value.EndsWith("Monster") && ABResources.Exists(text2 + "_H"))
			{
				text2 += "_H";
			}
			return text2;
		}
		case GlobalParticleType.EF:
			return "Prefabs/EF/" + particleName.Value;
		default:
			return null;
		}
	}

	private static GlobalParticleEmitter ConvertNormalParticleSystemToGlobalParticleSystem(ParticleSystem source)
	{
		source.Stop();
		ParticleSystem.MainModule main = source.main;
		main.duration = 1f;
		main.loop = false;
		main.prewarm = false;
		main.startDelay = 0f;
		main.maxParticles = int.MaxValue;
		main.startSpeed = 0f;
		main.gravityModifierMultiplier = 0f;
		main.simulationSpace = ParticleSystemSimulationSpace.World;
		ParticleSystem.EmissionModule emission = source.emission;
		emission.enabled = false;
		ParticleSystem.SubEmittersModule subEmitters = source.subEmitters;
		subEmitters.enabled = true;
		while (subEmitters.subEmittersCount > 0)
		{
			subEmitters.RemoveSubEmitter(0);
		}
		foreach (ParticleSystem item in from e in source.gameObject.GetComponentsInChildren<ParticleSystem>()
			where e != source
			select e)
		{
			ParticleSystem.MainModule main2 = item.main;
			main2.maxParticles = int.MaxValue;
			main2.simulationSpace = ParticleSystemSimulationSpace.World;
			subEmitters.AddSubEmitter(item, ParticleSystemSubEmitterType.Birth, ParticleSystemSubEmitterProperties.InheritColor | ParticleSystemSubEmitterProperties.InheritSize | ParticleSystemSubEmitterProperties.InheritRotation);
		}
		if (!source.gameObject.TryGetComponent<GlobalParticleEmitter>(out var component))
		{
			component = source.gameObject.AddComponent<GlobalParticleEmitter>();
		}
		component.Init();
		return component;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1499889937_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public GlobalParticleEmitSystem()
	{
	}
}
