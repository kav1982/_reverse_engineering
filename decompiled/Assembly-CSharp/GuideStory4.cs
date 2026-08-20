using UnityEngine;

public class GuideStory4 : MonoBehaviour
{
	private enum StoryState
	{
		Wait,
		WalkToRightPoint
	}

	private StoryState state;

	private void Update()
	{
		StoryState storyState = state;
		if (storyState != 0 && storyState != StoryState.WalkToRightPoint)
		{
			Debug.LogError(state);
		}
	}

	public void StarStory()
	{
		state = StoryState.WalkToRightPoint;
	}
}
