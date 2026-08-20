using System.Text;
using Steamworks;
using UnityEngine;

public class SteamHTTPTest : MonoBehaviour
{
	private Vector2 m_ScrollPos;

	private HTTPRequestHandle m_RequestHandle;

	private ulong m_ContextValue;

	private uint m_Offset;

	private uint m_BufferSize;

	private HTTPCookieContainerHandle m_CookieContainer;

	protected Callback<HTTPRequestHeadersReceived_t> m_HTTPRequestHeadersReceived;

	protected Callback<HTTPRequestDataReceived_t> m_HTTPRequestDataReceived;

	private CallResult<HTTPRequestCompleted_t> OnHTTPRequestCompletedCallResult;

	public void OnEnable()
	{
		m_RequestHandle = HTTPRequestHandle.Invalid;
		m_CookieContainer = HTTPCookieContainerHandle.Invalid;
		m_HTTPRequestHeadersReceived = Callback<HTTPRequestHeadersReceived_t>.Create(OnHTTPRequestHeadersReceived);
		m_HTTPRequestDataReceived = Callback<HTTPRequestDataReceived_t>.Create(OnHTTPRequestDataReceived);
		OnHTTPRequestCompletedCallResult = CallResult<HTTPRequestCompleted_t>.Create(OnHTTPRequestCompleted);
	}

	public void OnDisable()
	{
		ReleaseCookieContainer();
	}

	private void ReleaseCookieContainer()
	{
		if (m_CookieContainer != HTTPCookieContainerHandle.Invalid)
		{
			HTTPCookieContainerHandle cookieContainer = m_CookieContainer;
			MonoBehaviour.print("SteamHTTP.ReleaseCookieContainer(" + cookieContainer.ToString() + ") - " + SteamHTTP.ReleaseCookieContainer(m_CookieContainer));
			m_CookieContainer = HTTPCookieContainerHandle.Invalid;
		}
	}

	public void RenderOnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0f, 200f, Screen.height));
		GUILayout.Label("Variables:");
		HTTPRequestHandle requestHandle = m_RequestHandle;
		GUILayout.Label("m_RequestHandle: " + requestHandle.ToString());
		GUILayout.Label("m_ContextValue: " + m_ContextValue);
		GUILayout.Label("m_Offset: " + m_Offset);
		GUILayout.Label("m_BufferSize: " + m_BufferSize);
		HTTPCookieContainerHandle cookieContainer = m_CookieContainer;
		GUILayout.Label("m_CookieContainer: " + cookieContainer.ToString());
		GUILayout.EndArea();
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));
		if (GUILayout.Button("CreateHTTPRequest(EHTTPMethod.k_EHTTPMethodGET, \"http://httpbin.org/get\")"))
		{
			HTTPRequestHandle hTTPRequestHandle = (m_RequestHandle = SteamHTTP.CreateHTTPRequest(EHTTPMethod.k_EHTTPMethodGET, "http://httpbin.org/get"));
			string text = EHTTPMethod.k_EHTTPMethodGET.ToString();
			requestHandle = hTTPRequestHandle;
			MonoBehaviour.print("SteamHTTP.CreateHTTPRequest(" + text + ", \"http://httpbin.org/get\") : " + requestHandle.ToString());
		}
		if (GUILayout.Button("SetHTTPRequestContextValue(m_RequestHandle, 1)"))
		{
			bool flag = SteamHTTP.SetHTTPRequestContextValue(m_RequestHandle, 1uL);
			string[] obj = new string[6] { "SteamHTTP.SetHTTPRequestContextValue(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj[1] = requestHandle.ToString();
			obj[2] = ", ";
			obj[3] = 1.ToString();
			obj[4] = ") : ";
			obj[5] = flag.ToString();
			MonoBehaviour.print(string.Concat(obj));
		}
		if (GUILayout.Button("SetHTTPRequestNetworkActivityTimeout(m_RequestHandle, 30)"))
		{
			bool flag2 = SteamHTTP.SetHTTPRequestNetworkActivityTimeout(m_RequestHandle, 30u);
			string[] obj2 = new string[6] { "SteamHTTP.SetHTTPRequestNetworkActivityTimeout(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj2[1] = requestHandle.ToString();
			obj2[2] = ", ";
			obj2[3] = 30.ToString();
			obj2[4] = ") : ";
			obj2[5] = flag2.ToString();
			MonoBehaviour.print(string.Concat(obj2));
		}
		if (GUILayout.Button("SetHTTPRequestHeaderValue(m_RequestHandle, \"From\", \"support@rileylabrecque.com\")"))
		{
			bool flag3 = SteamHTTP.SetHTTPRequestHeaderValue(m_RequestHandle, "From", "support@rileylabrecque.com");
			requestHandle = m_RequestHandle;
			MonoBehaviour.print("SteamHTTP.SetHTTPRequestHeaderValue(" + requestHandle.ToString() + ", \"From\", \"support@rileylabrecque.com\") : " + flag3);
		}
		if (GUILayout.Button("SetHTTPRequestGetOrPostParameter(m_RequestHandle, \"testing\", \"Steamworks.NET\")"))
		{
			bool flag4 = SteamHTTP.SetHTTPRequestGetOrPostParameter(m_RequestHandle, "testing", "Steamworks.NET");
			requestHandle = m_RequestHandle;
			MonoBehaviour.print("SteamHTTP.SetHTTPRequestGetOrPostParameter(" + requestHandle.ToString() + ", \"testing\", \"Steamworks.NET\") : " + flag4);
		}
		if (GUILayout.Button("SendHTTPRequest(m_RequestHandle, out handle)"))
		{
			SteamAPICall_t pCallHandle;
			bool flag5 = SteamHTTP.SendHTTPRequest(m_RequestHandle, out pCallHandle);
			OnHTTPRequestCompletedCallResult.Set(pCallHandle);
			string[] obj3 = new string[6] { "SteamHTTP.SendHTTPRequest(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj3[1] = requestHandle.ToString();
			obj3[2] = ", out handle) : ";
			obj3[3] = flag5.ToString();
			obj3[4] = " -- ";
			SteamAPICall_t steamAPICall_t = pCallHandle;
			obj3[5] = steamAPICall_t.ToString();
			MonoBehaviour.print(string.Concat(obj3));
		}
		if (GUILayout.Button("SendHTTPRequestAndStreamResponse(m_RequestHandle, out handle)"))
		{
			SteamAPICall_t pCallHandle2;
			bool flag6 = SteamHTTP.SendHTTPRequestAndStreamResponse(m_RequestHandle, out pCallHandle2);
			OnHTTPRequestCompletedCallResult.Set(pCallHandle2);
			string[] obj4 = new string[6] { "SteamHTTP.SendHTTPRequestAndStreamResponse(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj4[1] = requestHandle.ToString();
			obj4[2] = ", out handle) : ";
			obj4[3] = flag6.ToString();
			obj4[4] = " -- ";
			SteamAPICall_t steamAPICall_t = pCallHandle2;
			obj4[5] = steamAPICall_t.ToString();
			MonoBehaviour.print(string.Concat(obj4));
		}
		if (GUILayout.Button("DeferHTTPRequest(m_RequestHandle)"))
		{
			bool flag7 = SteamHTTP.DeferHTTPRequest(m_RequestHandle);
			requestHandle = m_RequestHandle;
			MonoBehaviour.print("SteamHTTP.DeferHTTPRequest(" + requestHandle.ToString() + ") : " + flag7);
		}
		if (GUILayout.Button("PrioritizeHTTPRequest(m_RequestHandle)"))
		{
			bool flag8 = SteamHTTP.PrioritizeHTTPRequest(m_RequestHandle);
			requestHandle = m_RequestHandle;
			MonoBehaviour.print("SteamHTTP.PrioritizeHTTPRequest(" + requestHandle.ToString() + ") : " + flag8);
		}
		if (GUILayout.Button("GetHTTPResponseHeaderSize(m_RequestHandle, \"User-Agent\", out ResponseHeaderSize)"))
		{
			uint unResponseHeaderSize;
			bool hTTPResponseHeaderSize = SteamHTTP.GetHTTPResponseHeaderSize(m_RequestHandle, "User-Agent", out unResponseHeaderSize);
			string[] obj5 = new string[6] { "SteamHTTP.GetHTTPResponseHeaderSize(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj5[1] = requestHandle.ToString();
			obj5[2] = ", \"User-Agent\", out ResponseHeaderSize) : ";
			obj5[3] = hTTPResponseHeaderSize.ToString();
			obj5[4] = " -- ";
			obj5[5] = unResponseHeaderSize.ToString();
			MonoBehaviour.print(string.Concat(obj5));
		}
		if (GUILayout.Button("GetHTTPResponseHeaderValue(m_RequestHandle, \"User-Agent\", HeaderValueBuffer, ResponseHeaderSize)"))
		{
			SteamHTTP.GetHTTPResponseHeaderSize(m_RequestHandle, "User-Agent", out var unResponseHeaderSize2);
			byte[] array = new byte[unResponseHeaderSize2];
			bool hTTPResponseHeaderValue = SteamHTTP.GetHTTPResponseHeaderValue(m_RequestHandle, "User-Agent", array, unResponseHeaderSize2);
			string[] obj6 = new string[8] { "SteamHTTP.GetHTTPResponseHeaderValue(", null, null, null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj6[1] = requestHandle.ToString();
			obj6[2] = ", \"User-Agent\", ";
			obj6[3] = array?.ToString();
			obj6[4] = ", ";
			obj6[5] = unResponseHeaderSize2.ToString();
			obj6[6] = ") : ";
			obj6[7] = hTTPResponseHeaderValue.ToString();
			MonoBehaviour.print(string.Concat(obj6));
			MonoBehaviour.print("HeaderValueBuffer:\n" + Encoding.UTF8.GetString(array));
		}
		if (GUILayout.Button("GetHTTPResponseBodySize(m_RequestHandle, out BodySize)"))
		{
			uint unBodySize;
			bool hTTPResponseBodySize = SteamHTTP.GetHTTPResponseBodySize(m_RequestHandle, out unBodySize);
			string[] obj7 = new string[6] { "SteamHTTP.GetHTTPResponseBodySize(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj7[1] = requestHandle.ToString();
			obj7[2] = ", out BodySize) : ";
			obj7[3] = hTTPResponseBodySize.ToString();
			obj7[4] = " -- ";
			obj7[5] = unBodySize.ToString();
			MonoBehaviour.print(string.Concat(obj7));
		}
		if (GUILayout.Button("GetHTTPResponseBodyData(m_RequestHandle, BodyDataBuffer, BodySize)"))
		{
			SteamHTTP.GetHTTPResponseBodySize(m_RequestHandle, out var unBodySize2);
			byte[] array2 = new byte[unBodySize2];
			bool hTTPResponseBodyData = SteamHTTP.GetHTTPResponseBodyData(m_RequestHandle, array2, unBodySize2);
			string[] obj8 = new string[8] { "SteamHTTP.GetHTTPResponseBodyData(", null, null, null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj8[1] = requestHandle.ToString();
			obj8[2] = ", ";
			obj8[3] = array2?.ToString();
			obj8[4] = ", ";
			obj8[5] = unBodySize2.ToString();
			obj8[6] = ") : ";
			obj8[7] = hTTPResponseBodyData.ToString();
			MonoBehaviour.print(string.Concat(obj8));
			MonoBehaviour.print("BodyDataBuffer:\n" + Encoding.UTF8.GetString(array2));
		}
		if (GUILayout.Button("GetHTTPStreamingResponseBodyData(m_RequestHandle, m_Offset, BodyDataBuffer, m_BufferSize)"))
		{
			byte[] array3 = new byte[m_BufferSize];
			bool hTTPStreamingResponseBodyData = SteamHTTP.GetHTTPStreamingResponseBodyData(m_RequestHandle, m_Offset, array3, m_BufferSize);
			string[] obj9 = new string[10] { "SteamHTTP.GetHTTPStreamingResponseBodyData(", null, null, null, null, null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj9[1] = requestHandle.ToString();
			obj9[2] = ", ";
			obj9[3] = m_Offset.ToString();
			obj9[4] = ", ";
			obj9[5] = array3?.ToString();
			obj9[6] = ", ";
			obj9[7] = m_BufferSize.ToString();
			obj9[8] = ") : ";
			obj9[9] = hTTPStreamingResponseBodyData.ToString();
			MonoBehaviour.print(string.Concat(obj9));
			MonoBehaviour.print("BodyDataBuffer:\n" + Encoding.UTF8.GetString(array3));
		}
		if (GUILayout.Button("ReleaseHTTPRequest(m_RequestHandle)"))
		{
			bool flag9 = SteamHTTP.ReleaseHTTPRequest(m_RequestHandle);
			requestHandle = m_RequestHandle;
			MonoBehaviour.print("SteamHTTP.ReleaseHTTPRequest(" + requestHandle.ToString() + ") : " + flag9);
		}
		GUILayout.Label("GetHTTPDownloadProgressPct(m_RequestHandle, out PercentOut) : " + SteamHTTP.GetHTTPDownloadProgressPct(m_RequestHandle, out var pflPercentOut) + " -- " + pflPercentOut);
		if (GUILayout.Button("SetHTTPRequestRawPostBody(m_RequestHandle, \"application/x-www-form-urlencoded\", buffer, (uint)buffer.Length)"))
		{
			string text2 = "parameter=value&also=another";
			byte[] array4 = new byte[Encoding.UTF8.GetByteCount(text2) + 1];
			Encoding.UTF8.GetBytes(text2, 0, text2.Length, array4, 0);
			bool flag10 = SteamHTTP.SetHTTPRequestRawPostBody(m_RequestHandle, "application/x-www-form-urlencoded", array4, (uint)array4.Length);
			string[] obj10 = new string[8] { "SteamHTTP.SetHTTPRequestRawPostBody(", null, null, null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj10[1] = requestHandle.ToString();
			obj10[2] = ", \"application/x-www-form-urlencoded\", ";
			obj10[3] = array4?.ToString();
			obj10[4] = ", ";
			obj10[5] = ((uint)array4.Length).ToString();
			obj10[6] = ") : ";
			obj10[7] = flag10.ToString();
			MonoBehaviour.print(string.Concat(obj10));
		}
		if (GUILayout.Button("CreateCookieContainer(true)"))
		{
			m_CookieContainer = SteamHTTP.CreateCookieContainer(bAllowResponsesToModify: true);
			string text3 = true.ToString();
			cookieContainer = m_CookieContainer;
			MonoBehaviour.print("SteamHTTP.CreateCookieContainer(" + text3 + ") : " + cookieContainer.ToString());
		}
		if (GUILayout.Button("ReleaseCookieContainer(m_CookieContainer)"))
		{
			ReleaseCookieContainer();
		}
		if (GUILayout.Button("SetCookie(m_CookieContainer, \"http://httpbin.org\", \"http://httpbin.org/cookies\", \"TestCookie=Testing\")"))
		{
			bool flag11 = SteamHTTP.SetCookie(m_CookieContainer, "http://httpbin.org", "http://httpbin.org/cookies", "TestCookie=Testing");
			cookieContainer = m_CookieContainer;
			MonoBehaviour.print("SteamHTTP.SetCookie(" + cookieContainer.ToString() + ", \"http://httpbin.org\", \"http://httpbin.org/cookies\", \"TestCookie=Testing\") : " + flag11);
		}
		if (GUILayout.Button("SetHTTPRequestCookieContainer(m_RequestHandle, m_CookieContainer)"))
		{
			bool flag12 = SteamHTTP.SetHTTPRequestCookieContainer(m_RequestHandle, m_CookieContainer);
			string[] obj11 = new string[6] { "SteamHTTP.SetHTTPRequestCookieContainer(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj11[1] = requestHandle.ToString();
			obj11[2] = ", ";
			cookieContainer = m_CookieContainer;
			obj11[3] = cookieContainer.ToString();
			obj11[4] = ") : ";
			obj11[5] = flag12.ToString();
			MonoBehaviour.print(string.Concat(obj11));
		}
		if (GUILayout.Button("SetHTTPRequestUserAgentInfo(m_RequestHandle, \"TestUserAgentInfo\")"))
		{
			bool flag13 = SteamHTTP.SetHTTPRequestUserAgentInfo(m_RequestHandle, "TestUserAgentInfo");
			requestHandle = m_RequestHandle;
			MonoBehaviour.print("SteamHTTP.SetHTTPRequestUserAgentInfo(" + requestHandle.ToString() + ", \"TestUserAgentInfo\") : " + flag13);
		}
		if (GUILayout.Button("SetHTTPRequestRequiresVerifiedCertificate(m_RequestHandle, false)"))
		{
			bool flag14 = SteamHTTP.SetHTTPRequestRequiresVerifiedCertificate(m_RequestHandle, bRequireVerifiedCertificate: false);
			string[] obj12 = new string[6] { "SteamHTTP.SetHTTPRequestRequiresVerifiedCertificate(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj12[1] = requestHandle.ToString();
			obj12[2] = ", ";
			obj12[3] = false.ToString();
			obj12[4] = ") : ";
			obj12[5] = flag14.ToString();
			MonoBehaviour.print(string.Concat(obj12));
		}
		if (GUILayout.Button("SetHTTPRequestAbsoluteTimeoutMS(m_RequestHandle, 20000)"))
		{
			bool flag15 = SteamHTTP.SetHTTPRequestAbsoluteTimeoutMS(m_RequestHandle, 20000u);
			string[] obj13 = new string[6] { "SteamHTTP.SetHTTPRequestAbsoluteTimeoutMS(", null, null, null, null, null };
			requestHandle = m_RequestHandle;
			obj13[1] = requestHandle.ToString();
			obj13[2] = ", ";
			obj13[3] = 20000.ToString();
			obj13[4] = ") : ";
			obj13[5] = flag15.ToString();
			MonoBehaviour.print(string.Concat(obj13));
		}
		GUILayout.Label("GetHTTPRequestWasTimedOut(m_RequestHandle, out WasTimedOut) : " + SteamHTTP.GetHTTPRequestWasTimedOut(m_RequestHandle, out var pbWasTimedOut) + " -- " + pbWasTimedOut);
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void OnHTTPRequestCompleted(HTTPRequestCompleted_t pCallback, bool bIOFailure)
	{
		string[] obj = new string[12]
		{
			"[",
			2101.ToString(),
			" - HTTPRequestCompleted] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HTTPRequestHandle hRequest = pCallback.m_hRequest;
		obj[3] = hRequest.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_ulContextValue.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_bRequestSuccessful.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_eStatusCode.ToString();
		obj[10] = " -- ";
		obj[11] = pCallback.m_unBodySize.ToString();
		Debug.Log(string.Concat(obj));
		m_ContextValue = pCallback.m_ulContextValue;
	}

	private void OnHTTPRequestHeadersReceived(HTTPRequestHeadersReceived_t pCallback)
	{
		string[] obj = new string[6]
		{
			"[",
			2102.ToString(),
			" - HTTPRequestHeadersReceived] - ",
			null,
			null,
			null
		};
		HTTPRequestHandle hRequest = pCallback.m_hRequest;
		obj[3] = hRequest.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_ulContextValue.ToString();
		Debug.Log(string.Concat(obj));
	}

	private void OnHTTPRequestDataReceived(HTTPRequestDataReceived_t pCallback)
	{
		string[] obj = new string[10]
		{
			"[",
			2103.ToString(),
			" - HTTPRequestDataReceived] - ",
			null,
			null,
			null,
			null,
			null,
			null,
			null
		};
		HTTPRequestHandle hRequest = pCallback.m_hRequest;
		obj[3] = hRequest.ToString();
		obj[4] = " -- ";
		obj[5] = pCallback.m_ulContextValue.ToString();
		obj[6] = " -- ";
		obj[7] = pCallback.m_cOffset.ToString();
		obj[8] = " -- ";
		obj[9] = pCallback.m_cBytesReceived.ToString();
		Debug.Log(string.Concat(obj));
		m_Offset = pCallback.m_cOffset;
		m_BufferSize = pCallback.m_cBytesReceived;
	}
}
