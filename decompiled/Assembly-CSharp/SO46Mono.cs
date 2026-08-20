using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SO46Mono : MonoBehaviour
{
	public SkeletonAnimation sAnima;

	public SkeletonAnimation sAnima_Outline;

	public Material mat_Original;

	public Material mat_Outline;

	private void OnEnable()
	{
		sAnima_Outline.gameObject.SetActive(value: false);
		if (sAnima_Outline.CustomMaterialOverride.ContainsKey(mat_Original))
		{
			sAnima_Outline.CustomMaterialOverride.Remove(mat_Original);
		}
	}

	public void Selected()
	{
		sAnima_Outline.CustomMaterialOverride.Add(mat_Original, mat_Outline);
		sAnima_Outline.AnimationState.ClearTracks();
		StartCoroutine(DelayOpenOutline());
		TrackEntry trackEntry = sAnima_Outline.AnimationState.SetAnimation(0, sAnima.AnimationName, loop: true);
		trackEntry.AnimationStart = 0f;
		trackEntry.MixDuration = 0f;
		trackEntry.TrackTime = sAnima.AnimationState.GetCurrent(0).AnimationTime;
	}

	private IEnumerator DelayOpenOutline()
	{
		yield return new WaitForEndOfFrame();
		if (GameMgr.IsMobile_Static)
		{
			yield return new WaitForEndOfFrame();
		}
		sAnima_Outline.gameObject.SetActive(value: true);
	}

	public void UnSelected()
	{
		StopAllCoroutines();
		sAnima_Outline.gameObject.SetActive(value: false);
		sAnima_Outline.CustomMaterialOverride.Remove(mat_Original);
	}
}
