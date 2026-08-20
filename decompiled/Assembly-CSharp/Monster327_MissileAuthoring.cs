using Unity.Mathematics;
using UnityEngine;

public class Monster327_MissileAuthoring : MonoBehaviour
{
	[Header("Visual")]
	public GameObject rotateRoot;

	public GameObject rotateShadow;

	[Header("Straight Launch")]
	public float straightTime = 0.35f;

	public float straightSpeed = 4.5f;

	[Header("Homing")]
	public float homingSpeed = 5.5f;

	public RandomFloat maxTurnAnglePerSecond;

	[Header("Life")]
	public float lifeTime = 5f;

	[Tooltip("命中玩家时播放的爆炸特效缩放。")]
	[Header("Explosion")]
	public float explosionEffectScale = 0.4f;

	[Tooltip("导弹命中并播放爆炸特效后，临时扩大的 Capsule Physics Shape 半径。小于等于 0 时不修改碰撞半径。")]
	public float explosionColliderRadius = 0.8f;

	public float explosionTouchDuration = 0.1f;

	public float3 explosionOffset;
}
