public struct DotsKeyframeBlob
{
	public float Time;

	public float Value;

	public float InTangent;

	public float OutTangent;

	public DotsKeyframeBlob(float time, float value, float inTangent = 0f, float outTangent = 0f)
	{
		Time = time;
		Value = value;
		InTangent = inTangent;
		OutTangent = outTangent;
	}
}
