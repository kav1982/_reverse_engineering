using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TutorialHandBookMobile : MonoBehaviour
{
	private void OnEnable()
	{
		base.transform.localScale = new Vector3(0f, 0f, 0f);
		StartCoroutine(Delay());
	}

	public void Animation()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOScale(new Vector3(2.5f, 2.5f, 1f), 1.2f).SetEase(Ease.OutBack));
		sequence.Insert(1.2f, base.transform.DOScale(new Vector3(1f, 1f, 1f), 1f));
		sequence.Insert(1.2f, base.transform.DOMove(UIPlayerDataMgr.Inst.goHandBookButton.transform.position, 1f));
		sequence.OnComplete(delegate
		{
			UIPlayerDataMgr.Inst.goHandBookButton.SetActive(value: true);
			UIPlayerDataMgr.Inst.goHandBookGuideParticle.SetActive(value: true);
			UIPlayerDataMgr.Inst.healthTip.SetActive(value: true);
			Object.Destroy(base.gameObject);
		});
	}

	private IEnumerator Delay()
	{
		yield return new WaitForSeconds(2f);
		Animation();
	}
}
