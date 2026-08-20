using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "RealeaseNoteImageSO", menuName = "ScriptableObjects/RealeaseNoteImageSO", order = 0)]
public class RealeaseNoteImageSO : ScriptableObject
{
	[Serializable]
	public class SingleLanguangeImage
	{
		public Sprite sprite;

		public VideoClip mp4;

		public Vector2 Res1 = new Vector2(600f, -1f);
	}

	[AssetPath]
	public AssetPath CNSPath;

	[AssetPath]
	public AssetPath ENPath;

	public List<SingleLanguangeImage> ChineseS;

	public List<SingleLanguangeImage> English;
}
