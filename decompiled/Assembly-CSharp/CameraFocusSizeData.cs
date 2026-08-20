public class CameraFocusSizeData
{
	public float extraFocusSize;

	public int buffLevel = 1;

	public float buffDuration;

	public bool hasDuration;

	public float focusSizeSmoothDuration = 0.33f;

	public float OnExitFocusSizeSmoothChangeDuratio = 0.33f;

	public CameraFocusSizeData(float focusSize, int level = 1, float buffDuration = 0f, float focusProgressDuration = 0.33f, float ExitFocusProgressSpeed = -1f)
	{
		extraFocusSize = focusSize;
		buffLevel = level;
		this.buffDuration = buffDuration;
		hasDuration = buffDuration > 0f;
		focusSizeSmoothDuration = focusProgressDuration;
		OnExitFocusSizeSmoothChangeDuratio = ((ExitFocusProgressSpeed >= 0f) ? ExitFocusProgressSpeed : focusProgressDuration);
	}
}
