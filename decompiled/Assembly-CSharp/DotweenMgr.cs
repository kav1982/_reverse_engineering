using DG.Tweening;
using UnityEngine;

public class DotweenMgr : MonoBehaviour
{
	public void Initialize()
	{
		DOTween.SetTweensCapacity(2000, 500);
	}
}
