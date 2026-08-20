using UnityEngine;

public class RTCamController : MonoBehaviour
{
	public int MaxFps;

	public Camera cam;

	public bool isChapter3;

	public bool delayFrame;

	public static bool chapter3Delay;

	private bool _needRender;

	private float _renderCounter;

	private bool rendered;

	private void Start()
	{
		if (delayFrame)
		{
			_renderCounter += 1f / (float)MaxFps;
		}
	}

	private void Update()
	{
		_renderCounter += Time.deltaTime;
		if (MaxFps > 0 && _renderCounter >= 1f / (float)MaxFps)
		{
			if (isChapter3)
			{
				if (delayFrame && chapter3Delay)
				{
					_needRender = true;
					_renderCounter = 0f;
					rendered = true;
				}
				if (!delayFrame && !chapter3Delay)
				{
					_needRender = true;
					_renderCounter = 0f;
					rendered = true;
				}
			}
			else
			{
				_needRender = true;
				_renderCounter = 0f;
			}
		}
		cam.enabled = _needRender;
		_needRender = false;
	}

	private void LateUpdate()
	{
		if (rendered)
		{
			if (delayFrame)
			{
				chapter3Delay = false;
			}
			else
			{
				chapter3Delay = true;
			}
			rendered = false;
		}
	}

	public void RenderOnce()
	{
		_needRender = true;
	}
}
