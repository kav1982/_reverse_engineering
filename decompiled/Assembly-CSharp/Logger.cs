using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
	private static Queue<string> queue = new Queue<string>(6);

	private void OnEnable()
	{
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0f, Screen.height - 140, Screen.width, 140f));
		foreach (string item in queue)
		{
			GUILayout.Label(item);
		}
		GUILayout.EndArea();
	}

	private void HandleLog(string message, string stackTrace, LogType type)
	{
		queue.Enqueue(Time.time + " - " + message);
		if (queue.Count > 5)
		{
			queue.Dequeue();
		}
	}
}
