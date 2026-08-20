using UnityEngine;

public class Monster327Authoring : MonoBehaviour
{
	[Header("Missile Prefab")]
	public GameObject missilePrefab;

	[Tooltip("只旋转这个炮台子物体，不旋转发射器根节点。")]
	[Header("Turret")]
	public GameObject turretRoot;

	[Tooltip("炮台每秒最大旋转角度。")]
	public float turretRotateSpeed = 180f;

	[Tooltip("炮台初始朝向。")]
	public Vector3 defaultTurretDirection = Vector3.up;

	[Tooltip("炮台朝向与目标方向误差小于该角度时，才允许开始一轮发射。")]
	public float maxFireAngleError = 10f;

	[Header("Muzzles")]
	[Tooltip("左侧发射孔标记物。建议作为 turretRoot 的子物体，直接摆在炮口位置。")]
	public GameObject leftMuzzle;

	[Tooltip("右侧发射孔标记物。建议作为 turretRoot 的子物体，直接摆在炮口位置。")]
	public GameObject rightMuzzle;

	[Tooltip("导弹生成时额外叠加的 Y 轴偏移，用于修正导弹原点在影子位置、炮口有视觉高度的问题。")]
	public float missileSpawnYOffset;

	[Header("Fire")]
	public float firstFireDelay = 1f;

	public float fireInterval = 3f;

	[Tooltip("每一轮总共发射多少枚导弹。导弹会从左右两个发射孔轮流发射。")]
	public int missilesPerVolley = 4;

	[Tooltip("同一轮中，每两枚导弹之间的发射间隔。")]
	public float missileFireInterval = 0.15f;

	[Tooltip("最后一枚导弹发射后，炮台继续保持锁定方向的时间。建议与导弹 straightTime 接近。")]
	public float afterVolleyLockTime = 0.35f;

	[Header("Debug Gizmos")]
	public bool drawLaunchGizmos = true;

	public float gizmoPointRadius = 0.08f;

	public float gizmoDirectionLength = 0.8f;

	public Color gizmoPointColor = new Color(0.1f, 0.8f, 1f, 1f);

	public Color gizmoDirectionColor = new Color(1f, 0.35f, 0.1f, 1f);

	private void OnDrawGizmosSelected()
	{
		if (drawLaunchGizmos)
		{
			Vector3 direction = ((defaultTurretDirection.sqrMagnitude <= 0.0001f) ? Vector3.up : defaultTurretDirection.normalized);
			DrawMuzzleGizmo(leftMuzzle, direction);
			DrawMuzzleGizmo(rightMuzzle, direction);
		}
	}

	private void DrawMuzzleGizmo(GameObject muzzle, Vector3 direction)
	{
		if (!(muzzle == null))
		{
			Vector3 vector = muzzle.transform.position + new Vector3(0f, missileSpawnYOffset, 0f);
			Vector3 vector2 = vector + direction * gizmoDirectionLength;
			Gizmos.color = gizmoPointColor;
			Gizmos.DrawSphere(vector, gizmoPointRadius);
			Gizmos.color = gizmoDirectionColor;
			Gizmos.DrawLine(vector, vector2);
			Vector3 vector3 = Quaternion.AngleAxis(150f, Vector3.forward) * direction;
			Vector3 vector4 = Quaternion.AngleAxis(-150f, Vector3.forward) * direction;
			float num = gizmoDirectionLength * 0.2f;
			Gizmos.DrawLine(vector2, vector2 + vector3 * num);
			Gizmos.DrawLine(vector2, vector2 + vector4 * num);
		}
	}
}
