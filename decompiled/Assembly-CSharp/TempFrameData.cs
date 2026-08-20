using UnityEngine;

public class TempFrameData<T>
{
	private T data;

	private int frameCount;

	public T Data
	{
		get
		{
			if (!Valid)
			{
				Debug.LogError($"不是当前帧的数据，所获取的 {typeof(T)} 已经过时");
			}
			return data;
		}
	}

	public bool Valid => frameCount == Time.frameCount;

	public TempFrameData(T frameData)
	{
		Update(frameData);
	}

	public void Update(T frameData)
	{
		data = frameData;
		frameCount = Time.frameCount;
	}
}
