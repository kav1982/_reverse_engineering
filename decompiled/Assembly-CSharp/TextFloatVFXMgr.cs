using System.Text;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class TextFloatVFXMgr : MonoBehaviour
{
	public Color color_Damage;

	public Color color_Critical;

	public Color color_Posion;

	public Color color_Burn;

	public Color color_Normal;

	public Color color_GetKey;

	public Color color_GetCoin;

	public Color color_Crystal;

	public Color color_AncientBlood;

	public Color color_ChaosCore;

	public Color color_Gear;

	public Color color_GetShield;

	public Color color_GetTempShield;

	public Color color_Recovery;

	public Color color_RecoveryMP;

	public Color color_PlayerTakeDamageInjured;

	public Color color_PlayerLostShield;

	public Color color_PlayerLostTempShield;

	public Color color_PlayerLostUmbralle;

	public Color color_DropMP;

	public Color color_DropCoin;

	public Color color_DropKey;

	public VisualEffect vfx;

	public int oneFrameParticleCount;

	public VariableFloat randomX;

	public VariableFloat randomY;

	private GraphicsBuffer posAndTypeBuffer;

	private GraphicsBuffer dataBuffer;

	private GraphicsBuffer colorBuffer;

	private int textFloatCountNameID;

	private EntityManager ettMgr;

	private string charset13 = "0123456789.+-";

	private string charset18 = "0123456789abcdefgh";

	private string charset28 = "0123456789abcdefghijklmnopqr";

	public static TextFloatVFXMgr Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		ettMgr.CreateSingletonBuffer<TextFloatVFXBED>();
		textFloatCountNameID = Shader.PropertyToID("TextFloatCount");
		posAndTypeBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, oneFrameParticleCount, 16);
		dataBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, oneFrameParticleCount, 16);
		Color[] array = new Color[22]
		{
			color_Damage, color_Critical, color_Posion, color_Burn, color_Normal, color_GetKey, color_GetCoin, color_Crystal, color_AncientBlood, color_ChaosCore,
			color_Gear, color_GetShield, color_GetTempShield, color_Recovery, color_RecoveryMP, color_PlayerTakeDamageInjured, color_PlayerLostShield, color_PlayerLostTempShield, color_PlayerLostUmbralle, color_DropMP,
			color_DropCoin, color_DropKey
		};
		colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, array.Length, 16);
		colorBuffer.SetData(array);
		vfx.SetGraphicsBuffer("PosAndTypeBuffer", posAndTypeBuffer);
		vfx.SetGraphicsBuffer("DataBuffer", dataBuffer);
		vfx.SetGraphicsBuffer("ColorBuffer", colorBuffer);
	}

	private void OnDestroy()
	{
		posAndTypeBuffer?.Release();
		dataBuffer?.Release();
		colorBuffer?.Release();
	}

	private void Update()
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(TextFloatVFXBED));
		DynamicBuffer<TextFloatVFXBED> singletonBuffer = entityQuery.GetSingletonBuffer<TextFloatVFXBED>();
		if (singletonBuffer.Length <= 0)
		{
			return;
		}
		int num = math.min(singletonBuffer.Length, oneFrameParticleCount);
		Vector4[] array = new Vector4[num];
		Vector4[] array2 = new Vector4[num];
		for (int i = 0; i < num; i++)
		{
			float3 @float = singletonBuffer[i].worldPos + new float3(randomX.RandomResult(), randomY.RandomResult(), 0f);
			array[i].x = @float.x;
			array[i].y = @float.y;
			array[i].w = (float)singletonBuffer[i].type;
			switch (singletonBuffer[i].type)
			{
			case UITextFloatType.Damage:
			case UITextFloatType.Critical:
			case UITextFloatType.Poison:
			case UITextFloatType.Burn:
				array[i].z = 0f;
				break;
			case UITextFloatType.Normal:
			case UITextFloatType.GetKey:
			case UITextFloatType.GetCoin:
			case UITextFloatType.GetCrystal:
			case UITextFloatType.GetAnchientBlood:
			case UITextFloatType.GetChaosCore:
			case UITextFloatType.GetGear:
			case UITextFloatType.GetShield:
			case UITextFloatType.GetTempShield:
			case UITextFloatType.Recover:
			case UITextFloatType.RecoverMP:
				array[i].z = 1f;
				break;
			case UITextFloatType.PlayerTakeDamage:
			case UITextFloatType.PlayerLostShield:
			case UITextFloatType.PlayerLostTempShield:
			case UITextFloatType.PlayerLostUmbrella:
			case UITextFloatType.DropMP:
			case UITextFloatType.DropCoin:
			case UITextFloatType.DropKey:
				array[i].z = 2f;
				break;
			}
			string unit;
			string text = singletonBuffer[i].number.FormatWithUnit(out unit);
			if (singletonBuffer[i].type == UITextFloatType.GetKey || singletonBuffer[i].type == UITextFloatType.GetCoin || singletonBuffer[i].type == UITextFloatType.GetCrystal || singletonBuffer[i].type == UITextFloatType.GetShield || singletonBuffer[i].type == UITextFloatType.GetTempShield || singletonBuffer[i].type == UITextFloatType.Recover || singletonBuffer[i].type == UITextFloatType.RecoverMP)
			{
				text = "+" + text;
			}
			array2[i].x = Base13ToDecimal(text) + 0.1f;
			array2[i].y = text.Length;
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < text.Length; j++)
			{
				stringBuilder.Append("0");
			}
			if (unit != null)
			{
				stringBuilder.Append("00");
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder[0] = '1';
			}
			switch (singletonBuffer[i].type)
			{
			case UITextFloatType.PlayerLostShield:
				array2[i].z = Base18ToDecimal("01" + stringBuilder);
				break;
			case UITextFloatType.PlayerLostTempShield:
				array2[i].z = Base18ToDecimal("23" + stringBuilder);
				break;
			case UITextFloatType.DropMP:
				array2[i].z = Base18ToDecimal("45" + stringBuilder);
				break;
			case UITextFloatType.Critical:
				array2[i].z = Base18ToDecimal("67" + stringBuilder);
				break;
			case UITextFloatType.Poison:
				array2[i].z = Base18ToDecimal("89" + stringBuilder);
				break;
			case UITextFloatType.Burn:
				array2[i].z = Base18ToDecimal("ab" + stringBuilder);
				break;
			case UITextFloatType.GetCoin:
				array2[i].z = Base18ToDecimal("cd" + stringBuilder);
				break;
			case UITextFloatType.GetCrystal:
				array2[i].z = Base18ToDecimal("ef" + stringBuilder);
				break;
			case UITextFloatType.GetShield:
				array2[i].z = Base18ToDecimal("01" + stringBuilder);
				break;
			case UITextFloatType.GetTempShield:
				array2[i].z = Base18ToDecimal("23" + stringBuilder);
				break;
			case UITextFloatType.GetKey:
				array2[i].z = Base18ToDecimal("gh" + stringBuilder);
				break;
			case UITextFloatType.RecoverMP:
				array2[i].z = Base18ToDecimal("45" + stringBuilder);
				break;
			case UITextFloatType.DropCoin:
				array2[i].z = Base18ToDecimal("cd" + stringBuilder);
				break;
			case UITextFloatType.DropKey:
				array2[i].z = Base18ToDecimal("gh" + stringBuilder);
				break;
			default:
				array2[i].z = -1f;
				break;
			}
			switch (unit)
			{
			case "K":
				array2[i].w = Base28ToDecimal("01");
				break;
			case "M":
				array2[i].w = Base28ToDecimal("23");
				break;
			case "B":
				array2[i].w = Base28ToDecimal("45");
				break;
			case "T":
				array2[i].w = Base28ToDecimal("67");
				break;
			case "Qa":
				array2[i].w = Base28ToDecimal("89");
				break;
			case "Qi":
				array2[i].w = Base28ToDecimal("ab");
				break;
			case "Sx":
				array2[i].w = Base28ToDecimal("cd");
				break;
			case "Sp":
				array2[i].w = Base28ToDecimal("ef");
				break;
			case "万":
				array2[i].w = Base28ToDecimal("gh");
				break;
			case "亿":
				array2[i].w = Base28ToDecimal("ij");
				break;
			case "兆":
				array2[i].w = Base28ToDecimal("kl");
				break;
			case "京":
				array2[i].w = Base28ToDecimal("mn");
				break;
			case "垓":
				array2[i].w = Base28ToDecimal("op");
				break;
			case "秭":
				array2[i].w = Base28ToDecimal("qr");
				break;
			default:
				array2[i].w = -1f;
				break;
			}
		}
		posAndTypeBuffer.SetData(array);
		dataBuffer.SetData(array2);
		vfx.SetInt(textFloatCountNameID, num);
		vfx.Play();
		singletonBuffer.Clear();
	}

	public float Base13ToDecimal(string base13)
	{
		float num = 0f;
		for (int i = 0; i < base13.Length; i++)
		{
			char value = base13[i];
			int num2 = charset13.IndexOf(value);
			if (num2 == -1)
			{
				Debug.LogError("包含非法字符：" + value);
			}
			else
			{
				num = num * 13f + (float)num2;
			}
		}
		return num;
	}

	public float Base18ToDecimal(string base18)
	{
		float num = 0f;
		for (int i = 0; i < base18.Length; i++)
		{
			char value = base18[i];
			int num2 = charset18.IndexOf(value);
			if (num2 == -1)
			{
				Debug.LogError("包含非法字符：" + value);
			}
			else
			{
				num = num * 18f + (float)num2;
			}
		}
		return num;
	}

	public float Base28ToDecimal(string base28)
	{
		float num = 0f;
		for (int i = 0; i < base28.Length; i++)
		{
			char value = base28[i];
			int num2 = charset28.IndexOf(value);
			if (num2 == -1)
			{
				Debug.LogError("包含非法字符：" + value);
			}
			else
			{
				num = num * 28f + (float)num2;
			}
		}
		return num;
	}
}
