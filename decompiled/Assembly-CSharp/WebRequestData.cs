using System;

public class WebRequestData
{
	[Serializable]
	public class FileDownloadInfo
	{
		public string url;

		public string version;
	}

	[Serializable]
	public class Response<T>
	{
		public int code;

		public string message;

		public T data;
	}

	[Serializable]
	public class HarmoniousData
	{
		public bool harmonious;
	}

	[Serializable]
	public class AnotherData
	{
		public int someValue;

		public string someString;
	}
}
