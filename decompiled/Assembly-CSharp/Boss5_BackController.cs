using UnityEngine;

public class Boss5_BackController : MonoBehaviour
{
	[Header("这个脚本一直执行以提供动画对boss背部的实时控制")]
	public Transform backTransform;

	public Vector3 originLocalPosition;

	public float hideDepth;

	private void Update()
	{
		if (backTransform != null)
		{
			backTransform.localPosition = originLocalPosition - new Vector3(0f, hideDepth, 0f);
		}
	}
}
