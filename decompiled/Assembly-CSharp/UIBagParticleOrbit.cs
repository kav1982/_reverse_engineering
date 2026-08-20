using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBagParticleOrbit : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	public Sequence sequence;

	public GameObject particle;

	public GameObject gameobjectOrbit;

	public Text textHint;

	public float speed = 5f;

	public Color color;

	public Transform follow;

	private void OnEnable()
	{
		if (!textHint.TryGetComponent<CanvasGroup>(out var component))
		{
			component = textHint.AddComponent<CanvasGroup>();
		}
		component.alpha = 0f;
		particle.SetActive(value: false);
		sequence = DOTween.Sequence().AppendInterval(0.5f).AppendCallback(delegate
		{
			particle.gameObject.SetActive(value: true);
		})
			.Append(component.DOFade(1f, 1f));
	}

	private void OnDisable()
	{
		sequence.Kill();
		base.transform.rotation = Quaternion.identity;
	}

	private void Update()
	{
		if ((bool)canvasGroup)
		{
			if ((double)canvasGroup.alpha > 0.9)
			{
				if (!particle.activeSelf)
				{
					particle.SetActive(value: true);
				}
			}
			else if (particle.activeSelf)
			{
				particle.SetActive(value: false);
			}
		}
		if ((bool)follow)
		{
			base.transform.position = follow.position;
		}
	}
}
