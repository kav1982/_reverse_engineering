using DG.Tweening;
using UnityEngine;

public class TutorialHpShow : MonoBehaviour
{
	private void OnEnable()
	{
		base.transform.localScale = new Vector3(0f, 0f, 0f);
		Sequence sequence = DOTween.Sequence();
		sequence.SetUpdate(isIndependentUpdate: true);
		sequence.Append(base.transform.DOScale(new Vector3(0f, 0f, 0f), 0.35f));
		sequence.Insert(0.36f, base.transform.DOScale(new Vector3(1f, 1f, 1f), 0.75f).SetEase(Ease.OutBack));
		sequence.Insert(1.3f, base.transform.DOMove(UIPlayerDataMgr.Inst.playerinfoNormal.transform.position, 1.25f));
		sequence.OnComplete(delegate
		{
			TimeScaleMgr.Inst.ForceRecover();
			DataMgr.selectedWorldData.isTriggerTutorialHpShow = false;
			UIPlayerDataMgr.Inst.playerinfoNormal.transform.localScale = Vector3.one;
			Object.Destroy(base.gameObject);
		});
	}
}
