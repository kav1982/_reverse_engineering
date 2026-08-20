using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class AndroidLog : MonoBehaviour
{
	private readonly string[] IGNORE_LOG_KEYS = new string[3] { "There are no audio listeners in the scene.", "has invalid Materials and will not render correctly at runtime.", "Registering material null at index" };

	private StreamWriter _logWriter;

	private void Awake()
	{
		_logWriter?.Close();
		_logWriter = new StreamWriter(CreateFileStream());
		Application.logMessageReceivedThreaded += OnLog;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnDestroy()
	{
		Application.logMessageReceivedThreaded -= OnLog;
		_logWriter.Close();
	}

	private void OnLog(string condition, string stackTrace, LogType type)
	{
		if (!IGNORE_LOG_KEYS.Any(condition.Contains))
		{
			_logWriter.WriteLine($"[{type}] ({DateTime.Now}) {condition}");
			if (type == LogType.Assert || type == LogType.Error || type == LogType.Exception)
			{
				_logWriter.WriteLine(stackTrace);
				_logWriter.WriteLine();
			}
			_logWriter.Flush();
		}
	}

	private static FileStream CreateFileStream()
	{
		string text = Path.Join(Application.persistentDataPath, "Logs");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = Path.Join(text, "log.log");
		string text3 = Path.Join(text, "log_bak.log");
		if (File.Exists(text2))
		{
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			File.Move(text2, text3);
		}
		return File.Create(Path.Join(text, "log.log"));
	}
}
