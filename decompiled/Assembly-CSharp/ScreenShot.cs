using UnityEngine;

public class ScreenShot : MonoBehaviour
{
	private void Start()
	{
		ScreenCapture.CaptureScreenshot("鹏飞牛鼻.jpg");
	}
}
