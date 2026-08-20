using SpriteEffectSystem;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LocalSpriteEffectPlayer : MonoBehaviour
{
	public SpriteEffectAnima Anima;

	private SpriteRenderer _renderer;

	private float _frameTime;

	private float _frameInterval;

	private int _currentFrame;

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
		if (Anima != null)
		{
			Play(Anima);
		}
	}

	private void Update()
	{
		_frameTime += Time.deltaTime;
		if (_frameTime >= _frameInterval)
		{
			_frameTime -= _frameInterval;
			if (_frameTime >= _frameInterval)
			{
				_frameTime = 0f;
			}
			NextFrame();
		}
	}

	private void NextFrame()
	{
		_currentFrame++;
		if (_currentFrame >= Anima.Frames.Length)
		{
			_currentFrame = 0;
		}
		_renderer.sprite = Anima.Frames[_currentFrame];
	}

	public void Play(SpriteEffectAnima anima = null)
	{
		_currentFrame = 0;
		if (anima != null)
		{
			Anima = anima;
			_frameInterval = 1f / (float)Anima.Fps;
		}
	}
}
