using DG.Tweening;
using UnityEngine;

public class Elite59LaserSignal : MonoBehaviour
{
	private static readonly int Progress = Shader.PropertyToID("_Progress");

	public LineRenderer SignalLine;

	public LineRenderer ShadowLine;

	public float LinkDuration;

	public float RecycleDuration;

	public void OnEnable()
	{
		SignalLine.material.SetFloat(Progress, 1f);
		ShadowLine.material.SetFloat(Progress, 1f);
	}

	public void StartLink(Vector3 startPos, Vector3 endPos)
	{
		SignalLine.SetPosition(0, Tool2D.GetLayerPoint(startPos));
		SignalLine.SetPosition(1, Tool2D.GetLayerPoint(endPos));
		ShadowLine.SetPosition(0, Tool2D.IgnoreZPoint(startPos, 1.05f));
		ShadowLine.SetPosition(1, Tool2D.IgnoreZPoint(endPos, 1.05f));
		SignalLine.material.SetFloat(Progress, 0f);
		ShadowLine.material.SetFloat(Progress, 0f);
		SignalLine.material.DOFloat(Progress, 1, LinkDuration);
		ShadowLine.material.DOFloat(Progress, 1, LinkDuration);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, RecycleDuration);
	}
}
