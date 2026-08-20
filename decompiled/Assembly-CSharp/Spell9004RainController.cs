using UnityEngine;

public class Spell9004RainController : MonoBehaviour
{
	public SpriteRenderer sr;

	public float widthRatio = 1f;

	private float initialRingWidth;

	private float initialOutlineWidth1;

	private float initialOutlineWidth2;

	private float initialSize;

	private static readonly int RingWidthID = Shader.PropertyToID("_RingWidth");

	private static readonly int OutlineWidth1ID = Shader.PropertyToID("_OutlineWidth1");

	private static readonly int OutlineWidth2ID = Shader.PropertyToID("_OutlineWidth2");

	private void Awake()
	{
		initialRingWidth = sr.material.GetFloat(RingWidthID);
		initialOutlineWidth1 = sr.material.GetFloat(OutlineWidth1ID);
		initialOutlineWidth2 = sr.material.GetFloat(OutlineWidth2ID);
		initialSize = base.transform.lossyScale.x;
	}

	private void Update()
	{
		float num = base.transform.lossyScale.x / initialSize;
		sr.material.SetFloat(RingWidthID, initialRingWidth / num * widthRatio);
		sr.material.SetFloat(OutlineWidth1ID, initialOutlineWidth1 / num * widthRatio);
		sr.material.SetFloat(OutlineWidth2ID, initialOutlineWidth2 / num * widthRatio);
	}
}
