using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Spell4004ChargeStars : MonoBehaviour
{
	private const float SPEED_LERP_RATIO = 12f;

	private const float ROTATE_SPEED = 560f;

	public GameObject StarObj;

	public GameObject EndObj;

	private Vector2 _shiftScale;

	private float _currentAngle;

	private float _randomDirection;

	private float _stackYDynamicShift;

	private Vector3 _shiftPosition = Vector3.zero;

	private Vector3 _lastFrameAroundCenter = Vector3.zero;

	private Wand _targetWand;

	private List<Entity> _keepCastingSpells = new List<Entity>();

	private int _releasedFrame;

	private bool _released;

	private bool _breaked;

	public Entity Entity { get; private set; } = Entity.Null;


	private Vector3 AroundCenter
	{
		get
		{
			if ((bool)_targetWand)
			{
				_lastFrameAroundCenter = _targetWand.GetShootPosition();
			}
			return _lastFrameAroundCenter;
		}
	}

	public void OnEnable()
	{
		_breaked = false;
		_released = false;
		_keepCastingSpells.Clear();
		_releasedFrame = 0;
		_currentAngle = 0f;
		_randomDirection = ((UnityEngine.Random.Range(0, 2) == 0) ? 1 : (-1));
		_shiftPosition = Vector3.zero;
		_stackYDynamicShift = UnityEngine.Random.Range(0f, 360f);
		UnityEngine.Random.Range(0f, 360f);
		StarObj.SetActive(value: true);
		EndObj.SetActive(value: false);
	}

	public void Initialized(Wand wand, Transform followedTransform, Vector2 rotateVector, Vector3 shiftPos)
	{
		_targetWand = wand;
		_shiftScale = rotateVector;
		_shiftPosition = shiftPos;
		base.transform.position = followedTransform.position + shiftPos;
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		Entity = entityManager.CreateEntity(typeof(LocalTransform), typeof(Spell4004StartData));
		entityManager.SetComponentData(Entity, LocalTransform.FromPosition(base.transform.position));
		entityManager.SetComponentData(Entity, new Spell4004StartData
		{
			Star = this,
			Released = false,
			WandShootDirection = _targetWand.ShootDirection
		});
	}

	public void Release()
	{
		_released = true;
		Spell4004StartData componentData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Spell4004StartData>(Entity);
		componentData.Released = true;
		World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(Entity, componentData);
	}

	public void RegisterKeepCastingSpell(Entity entity)
	{
		_keepCastingSpells.Add(entity);
	}

	private void UpdateEntityData()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		entityManager.SetComponentData(Entity, LocalTransform.FromPosition(base.transform.position));
		Spell4004StartData componentData = entityManager.GetComponentData<Spell4004StartData>(Entity);
		componentData.Released = _released;
		componentData.WandShootDirection = _targetWand.ShootDirection;
		componentData.Star = this;
		entityManager.SetComponentData(Entity, componentData);
	}

	private void Update()
	{
		if (Entity != Entity.Null)
		{
			UpdateEntityData();
		}
		if (_released && !_breaked)
		{
			if (_releasedFrame >= 1)
			{
				EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
				if (_keepCastingSpells.All((Entity e) => !em.Exists(e)))
				{
					Break();
				}
			}
			_releasedFrame++;
		}
		if (!_released && !_breaked && World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Spell4004StartData>(Entity).NeedBreak)
		{
			_targetWand.ShootPassiveChargeOne(this);
		}
		RotateUpdate();
	}

	private void RotateUpdate()
	{
		if (!_breaked)
		{
			Vector3 b = AroundCenter + _shiftPosition + new Vector3(Mathf.Sin((_currentAngle - _stackYDynamicShift) * (MathF.PI / 180f)) * _shiftScale.x, Mathf.Cos((_currentAngle + _stackYDynamicShift) * (MathF.PI / 180f)) * _shiftScale.y, 0f);
			base.transform.position = Vector3.Lerp(base.transform.position, b, 12f * Time.deltaTime);
			if (!_released)
			{
				_currentAngle += 560f * _randomDirection * Time.deltaTime;
				_stackYDynamicShift += 56f * _randomDirection * Time.deltaTime;
			}
		}
	}

	public void Break(bool instantBreak = false)
	{
		if (_breaked)
		{
			Debug.LogWarning("这个星星已经捏碎了，不能再调用Break了");
			return;
		}
		_breaked = true;
		StarObj.SetActive(value: false);
		EndObj.SetActive(value: true);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, instantBreak ? 0.1f : 1.5f);
	}

	private void OnDisable()
	{
		if (Entity != Entity.Null)
		{
			World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(Entity);
			Entity = Entity.Null;
		}
	}
}
