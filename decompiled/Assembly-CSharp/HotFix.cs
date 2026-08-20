using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class HotFix
{
	public const char RootPathPrefix = '$';

	public const string AssetsResourcesPathPrefix = "Assets/Resources/";

	public const string HotFixBundlesDir = "hotfix";

	public const string ManifestFileName = "manifest";

	public const string VersionFileName = "version";

	public const string IndexFileName = "index";

	public const string CSHotFixFileName = "fix";

	public const string InHotFixBundleAssetsDirName = "assets";

	private const string HotFixBundleDCSKey = "hotfix-key";

	private static DESCryptoServiceProvider _desCryptoServiceProvider;

	private static DESCryptoServiceProvider _desProviderInst
	{
		get
		{
			if (_desCryptoServiceProvider == null)
			{
				_desCryptoServiceProvider = new DESCryptoServiceProvider();
				byte[] array = new byte[8];
				byte[] bytes = Encoding.UTF8.GetBytes("hotfix-key");
				Array.Copy(bytes, array, Mathf.Min(bytes.Length, 8));
				Debug.Log(string.Join(", ", array));
				_desCryptoServiceProvider.Key = array;
				_desCryptoServiceProvider.IV = _desCryptoServiceProvider.Key;
				return _desCryptoServiceProvider;
			}
			return _desCryptoServiceProvider;
		}
	}

	public static string DCSEncrypt(string source)
	{
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, _desProviderInst.CreateEncryptor(), CryptoStreamMode.Write);
		byte[] bytes = Encoding.Default.GetBytes(source);
		cryptoStream.Write(bytes, 0, bytes.Length);
		cryptoStream.FlushFinalBlock();
		return Convert.ToBase64String(memoryStream.ToArray());
	}

	public static string DCSDecrypt(string ciphertext)
	{
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, _desProviderInst.CreateDecryptor(), CryptoStreamMode.Write);
		byte[] array = Convert.FromBase64String(ciphertext);
		cryptoStream.Write(array, 0, array.Length);
		cryptoStream.FlushFinalBlock();
		return Encoding.Default.GetString(memoryStream.ToArray());
	}
}
