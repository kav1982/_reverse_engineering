using System;
using UnityEngine;

public class GlobalParticleEmitter : MonoBehaviour
{
	private struct SpecialSettings
	{
		public ParticleSystemCurveMode speedMode;

		public float speedConstant;

		public float speedConstantMin;

		public float speedConstantMax;

		public Vector3 shapeScale;

		public float forceOverLifetimeYMin;

		public float forceOverLifetimeYMax;
	}

	public ParticleSystem[] SpecialScaleEmitters = new ParticleSystem[0];

	public ParticleSystem[] SpecialDirectionEmitters = new ParticleSystem[0];

	public bool ResetInheritVelocity = true;

	private SpecialSettings[] _specialScaleSettings;

	private ParticleSystem _rootEmitter;

	private bool _inited;

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		if (_inited)
		{
			return;
		}
		_inited = true;
		_rootEmitter = GetComponent<ParticleSystem>();
		if (_rootEmitter == null)
		{
			Debug.LogError(base.name + " 没有根粒子发射器");
		}
		_specialScaleSettings = new SpecialSettings[SpecialScaleEmitters.Length];
		for (int i = 0; i < SpecialScaleEmitters.Length; i++)
		{
			ParticleSystem particleSystem = SpecialScaleEmitters[i];
			if (particleSystem == null)
			{
				Debug.LogError(base.name + " 上有个空粒子发射器");
				continue;
			}
			_specialScaleSettings[i].speedMode = particleSystem.main.startSpeed.mode;
			_specialScaleSettings[i].shapeScale = particleSystem.shape.scale;
			if (particleSystem.main.startSpeed.mode == ParticleSystemCurveMode.Constant)
			{
				_specialScaleSettings[i].speedConstant = particleSystem.main.startSpeed.constant;
			}
			else
			{
				if (particleSystem.main.startSpeed.mode != ParticleSystemCurveMode.TwoConstants)
				{
					throw new Exception($"{base.name} 中 {particleSystem.name} 的全局粒子的速度不支持 {particleSystem.main.startSpeed.mode} 模式。");
				}
				_specialScaleSettings[i].speedConstantMin = particleSystem.main.startSpeed.constantMin;
				_specialScaleSettings[i].speedConstantMax = particleSystem.main.startSpeed.constantMax;
			}
			if (particleSystem.forceOverLifetime.enabled)
			{
				_specialScaleSettings[i].forceOverLifetimeYMin = particleSystem.forceOverLifetime.y.constantMin;
				_specialScaleSettings[i].forceOverLifetimeYMax = particleSystem.forceOverLifetime.y.constantMax;
			}
		}
		ParticleSystem[] specialDirectionEmitters = SpecialDirectionEmitters;
		foreach (ParticleSystem particleSystem2 in specialDirectionEmitters)
		{
			if (particleSystem2.transform.eulerAngles != new Vector3(0f, 90f, 90f))
			{
				Debug.LogWarning($"按道理讲，带方向的全局粒子应该在编辑器中保持(0,90,90)的旋转角度，并保证在编辑器中效果正常且正面向右，但 {base.name} 中的 {particleSystem2.name} 不符合这个要求。当前角度为{particleSystem2.transform.eulerAngles}");
			}
			particleSystem2.transform.eulerAngles = Vector3.zero;
			particleSystem2.GetComponent<ParticleSystemRenderer>().alignment = ParticleSystemRenderSpace.Velocity;
			if (ResetInheritVelocity)
			{
				ParticleSystem.InheritVelocityModule inheritVelocity = particleSystem2.inheritVelocity;
				inheritVelocity.enabled = true;
				inheritVelocity.mode = ParticleSystemInheritVelocityMode.Initial;
				inheritVelocity.curveMultiplier = 0.001f;
			}
		}
	}

	public void Emit(GlobalParticleEmitParams emitParams)
	{
		if (emitParams.Size.HasValue)
		{
			for (int i = 0; i < SpecialScaleEmitters.Length; i++)
			{
				ParticleSystem particleSystem = SpecialScaleEmitters[i];
				SpecialSettings specialSettings = _specialScaleSettings[i];
				ParticleSystem.MainModule main = particleSystem.main;
				ParticleSystem.MinMaxCurve startSpeed = particleSystem.main.startSpeed;
				if (specialSettings.speedMode == ParticleSystemCurveMode.Constant)
				{
					startSpeed.constant = specialSettings.speedConstant * emitParams.Size.Value;
				}
				else if (specialSettings.speedMode == ParticleSystemCurveMode.TwoConstants)
				{
					startSpeed.constantMin = specialSettings.speedConstantMin * emitParams.Size.Value;
					startSpeed.constantMax = specialSettings.speedConstantMax * emitParams.Size.Value;
				}
				main.startSpeed = startSpeed;
				ParticleSystem.ShapeModule shape = particleSystem.shape;
				shape.scale = specialSettings.shapeScale * emitParams.Size.Value;
				if (particleSystem.forceOverLifetime.enabled)
				{
					ParticleSystem.ForceOverLifetimeModule forceOverLifetime = particleSystem.forceOverLifetime;
					ParticleSystem.MinMaxCurve y = forceOverLifetime.y;
					y.constantMin = specialSettings.forceOverLifetimeYMin * emitParams.Size.Value;
					y.constantMax = specialSettings.forceOverLifetimeYMax * emitParams.Size.Value;
					forceOverLifetime.y = y;
				}
			}
		}
		_rootEmitter.Emit(emitParams.AsEmitParams(_rootEmitter), 1);
	}
}
