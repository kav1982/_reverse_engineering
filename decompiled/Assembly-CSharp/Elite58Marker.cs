using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Elite58Marker : MonoBehaviour
{
	private static readonly int Start = Animator.StringToHash("Start");

	private static readonly int End = Animator.StringToHash("End");

	public Transform LightPillarTransform;

	public float StartPeriod;

	public float TargetScale;

	public float EndPeriod;

	public Animator Anima;

	public float SEPlayInterval;

	private float SETimer;

	private bool isStart;

	public Transform MarkerRotateTransform;

	private void OnEnable()
	{
		LightPillarTransform.localScale = Vector3.zero;
		SETimer = 0f;
		isStart = false;
	}

	public void StartMarker()
	{
		LightPillarTransform.DOScale(TargetScale, StartPeriod);
		Anima.SetTrigger(Start);
		isStart = true;
	}

	private void Update()
	{
		if (isStart)
		{
			SETimer += Time.deltaTime;
			if (SETimer >= SEPlayInterval)
			{
				SETimer -= SEPlayInterval;
				SEMgr.Inst.elite58PopAlert.PlaySE();
			}
			MarkerRotateTransform.right = (base.transform.position - LevelMgr.Inst.CurrentRoomCtrller.CenterPoint).IgnoreZ().normalized;
		}
	}

	public void UpdateTransform(Vector3 centerPos)
	{
		base.transform.position = centerPos;
	}

	public void EndMarker()
	{
		isStart = false;
		StartCoroutine(EndMarkerChangeLight());
	}

	private IEnumerator EndMarkerChangeLight()
	{
		LightPillarTransform.DOScale(0f, EndPeriod);
		Anima.SetTrigger(End);
		yield return new WaitForSeconds(EndPeriod);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
