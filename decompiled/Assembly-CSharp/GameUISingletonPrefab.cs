using System;

public class GameUISingletonPrefab : Attribute
{
	public readonly string prefabPath;

	public GameUISingletonPrefab(string prefabPath)
	{
		this.prefabPath = prefabPath;
	}
}
