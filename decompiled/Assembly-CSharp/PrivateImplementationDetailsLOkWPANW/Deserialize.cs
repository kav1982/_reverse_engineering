using System.Runtime.InteropServices;
using System.Text;

namespace PrivateImplementationDetailsLOkWPANW;

[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
public class Deserialize
{
	public static string DeserializeLiteral(byte[] k, byte[] d)
	{
		for (int i = 0; i < d.Length; i++)
		{
			d[i] = (byte)(d[i] ^ k[i % 64]);
		}
		string @string = Encoding.UTF8.GetString(d);
		return @string.Substring(0, @string.IndexOf('\ue44f'));
	}
}
