using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GalleryContains
{
	public enum GalleryGroupType
	{
		None,
		Rare,
		Chapter,
		SpellType
	}

	public GalleryCategory galleryType;

	public GalleryGroupType groupType;

	public GameObject PanelLists;

	public GameObject PanelInfos;

	public GameObject ContentRoot;

	public List<Text> Titles { get; set; }

	public List<LayoutGroup> GridLayoutGroups { get; set; }

	public List<ContentSizeFitter> ContentSizeFitters { get; set; }

	public void Init()
	{
		GridLayoutGroups = (from lg in ContentRoot.GetComponentsInChildren<LayoutGroup>(includeInactive: true)
			where lg.gameObject != ContentRoot
			select lg).ToList();
		ContentSizeFitters = (from lg in ContentRoot.GetComponentsInChildren<ContentSizeFitter>(includeInactive: true).ToList()
			where lg.gameObject != ContentRoot
			select lg).ToList();
		Titles = (from lg in ContentRoot.GetComponentsInChildren<Text>(includeInactive: true).ToList()
			where lg.gameObject != ContentRoot
			select lg).ToList();
		Titles.ForEach(delegate(Text x)
		{
			x.fontSize = 23;
			if (!GameMgr.IsMobile_Static)
			{
				x.rectTransform.anchoredPosition += new Vector2(0f, 3f);
			}
			x.fontStyle = FontStyle.Normal;
		});
		UpdateLanguate();
	}

	public void UpdateLanguate()
	{
		switch (groupType)
		{
		case GalleryGroupType.Rare:
			Titles[0].text = 1001601.GetText();
			Titles[1].text = 1001602.GetText();
			if (Titles.Count > 2)
			{
				Titles[2].text = 1001603.GetText();
			}
			if (Titles.Count > 3)
			{
				Titles[3].text = 1001604.GetText();
			}
			break;
		case GalleryGroupType.Chapter:
			Titles[0].text = 1001702.GetText();
			Titles[1].text = 1001703.GetText();
			Titles[2].text = 1001704.GetText();
			Titles[3].text = (DataMgr.selectedWorldData.isReachChatper4 ? 1001705.GetText() : "???");
			Titles[4].text = (DataMgr.selectedWorldData.isReachChatper5 ? 1001706.GetText() : "???");
			break;
		case GalleryGroupType.SpellType:
			Titles[0].text = 1002205.GetText();
			Titles[1].text = 1002206.GetText();
			Titles[2].text = 1002207.GetText();
			Titles[3].text = 1002208.GetText();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case GalleryGroupType.None:
			break;
		}
	}
}
