using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellEffectSettings
{
	public enum RecycleTimingType
	{
		OnRecycle,
		OnStart,
		OnFlyFinish,
		Manual
	}

	public enum CreateTimingType
	{
		OnFirstFrame,
		OnEnable,
		OnFlyFinish,
		OnRecycle,
		Manual
	}

	public enum PositionType
	{
		Target,
		Manual
	}

	public enum RotationType
	{
		Manual,
		LookDirection,
		TargetRotation
	}

	public enum ScaleType
	{
		TargetLossyScale,
		TargetLocalScale,
		Manual
	}

	[Tooltip("特效名称，代码中通过这个名称来创建或引用这个特效。")]
	public string Name;

	[Tooltip("附着目标，特效会根据下面的设置，移动到这个目标的位置或者跟着旋转等")]
	public Transform AttachTarget;

	[Tooltip("回收延迟，如果特效需要一些回收后播放一些效果，可以设置这个东西")]
	public float RecycleDelay = 0.5f;

	[Tooltip("特效回收的时机")]
	public RecycleTimingType RecycleTiming;

	[Tooltip("特效创建的时机，推荐使用 OnFirstFrame")]
	public CreateTimingType CreateTiming = CreateTimingType.OnEnable;

	[Tooltip("特效位置更新模式：\n - Target: 附着到 AttachTarget 位置\n - Manual：需要手动更新特效位置，或者说是忽略位置")]
	public PositionType PositionMode;

	[Tooltip("特效的旋转模式：\n - Manual: 手动更新角度，或者说是忽略角度 \n - LookDirection: Z轴正方向朝向法术飞行方向\n - TargetRotation: 特效角度与 AttachTarget 同步")]
	public RotationType RotationMode;

	[Tooltip("特效的缩放模式，可以设置为跟随法术整体缩放、跟随 AttachTarget 局部缩放、或者忽略缩放")]
	public ScaleType ScaleMode;

	[Tooltip("是否忽略染色类型，如果忽略，预制体名称后面不需要后缀颜色。")]
	public bool IgnoreColorType;

	[SerializeField]
	public List<SpellColorType> HarmonizedColorTypes = new List<SpellColorType>();

	private HashSet<SpellColorType> _harmonizedColors;

	public HashSet<SpellColorType> HarmonizedColors => _harmonizedColors ?? (_harmonizedColors = new HashSet<SpellColorType>(HarmonizedColorTypes));
}
