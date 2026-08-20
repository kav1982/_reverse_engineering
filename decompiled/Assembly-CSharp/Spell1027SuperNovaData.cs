using Unity.Entities;
using UnityEngine;

public struct Spell1027SuperNovaData : IComponentData, IQueryTypeParameter
{
	public bool InitOver;

	public bool CreateFallStarTrailEffected;

	public bool BoomOver;

	public float DestroyTimer;

	public float AddBaseOriginal;

	public const float ScaleAddNum = 0.2f;

	public UnityObjectRef<GameObject> EffectExplosion;

	public UnityObjectRef<GameObject> EffectNormalLevel;

	public UnityObjectRef<GameObject> EffectGroundLevel;

	public UnityObjectRef<GameObject> NormalLevelStage1;

	public UnityObjectRef<GameObject> NormalLevelStage2;

	public UnityObjectRef<GameObject> NormalLevelStage3;

	public UnityObjectRef<GameObject> NormalLevelStage4;

	public UnityObjectRef<GameObject> GroundLevelStage1;

	public UnityObjectRef<GameObject> GroundLevelStage2;

	public UnityObjectRef<GameObject> GroundLevelStage3;

	public UnityObjectRef<GameObject> GroundLevelStage4;
}
