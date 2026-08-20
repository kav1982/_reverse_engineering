using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveDataCompressor
{
	public static string CompressSaveData(StringSaveData data)
	{
		string s = JsonConvert.SerializeObject(data);
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		using MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
		{
			gZipStream.Write(bytes, 0, bytes.Length);
		}
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public static StringSaveData DecompressSaveData(string compressedString)
	{
		try
		{
			using MemoryStream stream = new MemoryStream(Convert.FromBase64String(compressedString));
			using GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress);
			using MemoryStream memoryStream = new MemoryStream();
			gZipStream.CopyTo(memoryStream);
			return JsonConvert.DeserializeObject<StringSaveData>(Encoding.UTF8.GetString(memoryStream.ToArray()));
		}
		catch (Exception ex)
		{
			Debug.LogError("解压存档失败: " + ex.Message);
			return null;
		}
	}
}
