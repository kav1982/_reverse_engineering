using UnityEngine;

public class Monster310Authoring : MonoBehaviour
{
	public AIPattern pattern;

	[Header("跳跃")]
	public RandomFloat jumpOffsetRange;

	public float maxJumpDistance;

	public float gravity;

	public float upSpeed;
}
