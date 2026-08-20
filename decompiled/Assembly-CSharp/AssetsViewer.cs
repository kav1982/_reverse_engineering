using System;
using UnityEngine;

public sealed class AssetsViewer : MonoBehaviour
{
	[Serializable]
	public class AssetsGroup
	{
		public string Name;

		public KeyCode SelectGroupKey;

		public GameObject[] Items;
	}

	private enum NavigationButtonMode
	{
		ChangeItem,
		ChangeGroup
	}

	[SerializeField]
	private float m_controlsHeight;

	[SerializeField]
	private Vector2 m_controlsOffset;

	[SerializeField]
	private Vector2 m_controlsSpacing;

	[SerializeField]
	private float m_groupButtonWidth;

	[SerializeField]
	private AssetsGroup[] m_groups;

	[SerializeField]
	private bool m_GUIEnabled;

	[SerializeField]
	private NavigationButtonMode m_navigationButtonMode;

	[SerializeField]
	private float m_navigationButtonWidth;

	[SerializeField]
	private KeyCode m_switchNextItemKey;

	[SerializeField]
	private KeyCode m_switchPrevItemKey;

	[SerializeField]
	private Vector2 m_timeScaleRange;

	[SerializeField]
	private KeyCode m_toggleGUIKey;

	private int m_currentGroupIndex;

	private int m_currentItemIndex;

	private GameObject CurrentItem
	{
		get
		{
			if (m_groups == null || m_groups.Length <= m_currentGroupIndex)
			{
				return null;
			}
			GameObject[] items = m_groups[m_currentGroupIndex].Items;
			if (items == null || items.Length <= m_currentItemIndex)
			{
				return null;
			}
			return items[m_currentItemIndex];
		}
	}

	private int CurrentGroupItemsCount
	{
		get
		{
			if (m_groups == null || m_groups.Length <= m_currentGroupIndex)
			{
				return 0;
			}
			GameObject[] items = m_groups[m_currentGroupIndex].Items;
			if (items == null)
			{
				return 0;
			}
			return items.Length;
		}
	}

	private string CurrentGroupName
	{
		get
		{
			if (m_groups == null || m_groups.Length <= m_currentGroupIndex)
			{
				return "";
			}
			return m_groups[m_currentGroupIndex].Name;
		}
	}

	private void Awake()
	{
		m_currentGroupIndex = 0;
		m_currentItemIndex = 0;
		if (m_groups != null)
		{
			AssetsGroup[] groups = m_groups;
			foreach (AssetsGroup assetsGroup in groups)
			{
				if (assetsGroup.Items == null)
				{
					continue;
				}
				GameObject[] items = assetsGroup.Items;
				foreach (GameObject gameObject in items)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(value: false);
					}
				}
			}
		}
		SetActiveCurrentItem(state: true);
	}

	private void LateUpdate()
	{
		if (m_groups == null || m_groups.Length == 0)
		{
			return;
		}
		if (Input.GetKeyUp(m_toggleGUIKey))
		{
			m_GUIEnabled = !m_GUIEnabled;
		}
		if (Input.GetKeyUp(m_switchPrevItemKey))
		{
			ConditionalSwitchToPrevItem();
		}
		if (Input.GetKeyUp(m_switchNextItemKey))
		{
			ConditionalSwitchToNextItem();
		}
		for (int i = 0; i < m_groups.Length; i++)
		{
			if (Input.GetKeyUp(m_groups[i].SelectGroupKey))
			{
				ChangeCurrentGroup(i);
			}
		}
	}

	private void OnGUI()
	{
		if (!m_GUIEnabled || m_groups == null || m_groups.Length == 0)
		{
			return;
		}
		float y = m_controlsOffset.y;
		GUIContent content = new GUIContent("Time scale");
		Vector2 vector = GUI.skin.label.CalcSize(content);
		GUI.Label(new Rect(m_controlsOffset.x, y, vector.x, vector.y), content);
		y += vector.y + m_controlsSpacing.y;
		Time.timeScale = GUI.HorizontalSlider(new Rect(m_controlsOffset.x, y, m_groupButtonWidth, m_controlsHeight), Time.timeScale, m_timeScaleRange.x, m_timeScaleRange.y);
		y += m_controlsHeight + m_controlsSpacing.y;
		content = new GUIContent(CurrentGroupName);
		vector = GUI.skin.label.CalcSize(content);
		GUI.Label(new Rect(m_controlsOffset.x, y, vector.x, vector.y), content);
		y += vector.y + m_controlsSpacing.y;
		if (GUI.Button(new Rect(m_controlsOffset.x, y, m_navigationButtonWidth, m_controlsHeight), "<"))
		{
			ConditionalSwitchToPrevItem();
		}
		if (GUI.Button(new Rect(m_controlsOffset.x + m_navigationButtonWidth + m_controlsSpacing.x, y, m_navigationButtonWidth, m_controlsHeight), ">"))
		{
			ConditionalSwitchToNextItem();
		}
		for (int i = 0; i < m_groups.Length; i++)
		{
			y += m_controlsHeight + m_controlsSpacing.y;
			if (GUI.Button(new Rect(m_controlsOffset.x, y, m_groupButtonWidth, m_controlsHeight), m_groups[i].Name))
			{
				ChangeCurrentGroup(i);
			}
		}
	}

	private void ChangeCurrentGroup(int index)
	{
		SetActiveCurrentItem(state: false);
		m_currentGroupIndex = Mathf.Clamp(index, 0, (m_groups != null) ? Mathf.Max(m_groups.Length - 1, 0) : 0);
		m_currentItemIndex = 0;
		SetActiveCurrentItem(state: true);
	}

	private void ConditionalSwitchToNextItem()
	{
		if (m_navigationButtonMode == NavigationButtonMode.ChangeGroup)
		{
			ChangeCurrentGroup(m_currentGroupIndex + 1);
		}
		else
		{
			SwitchToNextItem();
		}
	}

	private void ConditionalSwitchToPrevItem()
	{
		if (m_navigationButtonMode == NavigationButtonMode.ChangeGroup)
		{
			ChangeCurrentGroup(m_currentGroupIndex - 1);
		}
		else
		{
			SwitchToPrevItem();
		}
	}

	private void SwitchToNextItem()
	{
		SetActiveCurrentItem(state: false);
		m_currentItemIndex = Mathf.Min(m_currentItemIndex + 1, Mathf.Max(CurrentGroupItemsCount, 1) - 1);
		SetActiveCurrentItem(state: true);
	}

	private void SwitchToPrevItem()
	{
		SetActiveCurrentItem(state: false);
		m_currentItemIndex = Mathf.Max(m_currentItemIndex - 1, 0);
		SetActiveCurrentItem(state: true);
	}

	private void SetActiveCurrentItem(bool state)
	{
		GameObject currentItem = CurrentItem;
		if (currentItem != null)
		{
			currentItem.SetActive(state);
		}
	}
}
