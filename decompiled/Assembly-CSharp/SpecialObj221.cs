using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlayerLogger;
using UnityEngine;

public class SpecialObj221 : LayerCorrect, IRoomCtrller
{
	public int RandomCount;

	public int frogCount;

	private int[] someKeys = new int[3] { 1, 2, 3 };

	public float Angle = 120f;

	public int StartAngle;

	private RoomController roomCtrller;

	public SpecialObj221melody currentMelody;

	public Transform TsfKeyParent;

	private List<SpecialObj221Key> keys = new List<SpecialObj221Key>();

	public AudioClip[] audioCLips1_5;

	public bool firstPlay;

	public bool IsComplete;

	public GameObject pfb_keyObj;

	public Vector3 StartPosition;

	public float interval;

	public float radius;

	public float melodyPlaySpeed = 1f;

	public Coroutine iePlayingMelody;

	public Coroutine iePlayingMelodyColorChange;

	public Coroutine iePlayingNoteMusicBox;

	public string CurrentKeys;

	public Sprite spriteKey;

	public Sprite spriteKeyActive;

	public SpriteRenderer spriteRendererMusicBox;

	public Sprite spriteMusicboxNormal;

	public Sprite spriteMusicboxPlaying;

	public string KeyAudioResourceName = "22101_KeyAudioSource";

	public string NoteAppearanceResourceName = "22101_NoteAppearance";

	public List<Color> ColorSequences;

	public Transform NotePosition;

	public Animator soundBoxAnimator;

	public float recoverTime;

	public float timePerformColor1;

	public float timePerformColor2;

	public void ReplayMelody()
	{
		if (iePlayingMelody != null)
		{
			StopCoroutine(iePlayingMelody);
		}
		iePlayingMelody = StartCoroutine(PlayMelody(currentMelody));
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (!firstPlay && collision.IsPlayerTrigger())
		{
			firstPlay = true;
			ReplayMelody();
		}
	}

	public void Start()
	{
		Init();
	}

	private void Init()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		keys.Clear();
		float num = Angle / (float)(frogCount - 1);
		for (int i = 0; i < frogCount; i++)
		{
			float num2 = (float)StartAngle - (float)i * num;
			float x = radius * Mathf.Cos(num2 * (MathF.PI / 180f));
			float y = radius * Mathf.Sin(num2 * (MathF.PI / 180f));
			Vector3 vector = new Vector3(x, y, base.transform.position.z);
			GameObject gameObject = UnityEngine.Object.Instantiate(pfb_keyObj, TsfKeyParent);
			gameObject.transform.localPosition = StartPosition + vector;
			gameObject.GetComponent<SpecialObj221Key>().specialObj221 = this;
			gameObject.GetComponent<SpecialObj221Key>().id = i + 1;
			gameObject.GetComponent<SpecialObj221Key>().spriterenderer.sprite = spriteKey;
			keys.Add(gameObject.GetComponent<SpecialObj221Key>());
		}
		currentMelody = new SpecialObj221melody(140, "", "random");
		for (int j = 0; j < RandomCount; j++)
		{
			char c;
			if (currentMelody.melody.Length == 0)
			{
				c = char.Parse(UnityEngine.Random.Range(1, frogCount + 1).ToString());
			}
			else
			{
				do
				{
					c = char.Parse(UnityEngine.Random.Range(1, frogCount + 1).ToString());
				}
				while (currentMelody.melody[currentMelody.melody.Length - 1] == c);
			}
			currentMelody.melody += c;
		}
		Debug.Log("随机生成:" + currentMelody.melody);
	}

	private void SoundVolumeChange()
	{
	}

	public void OnDestroy()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	public IEnumerator PlayMelodyOnStart(SpecialObj221melody melody)
	{
		yield return new WaitForSeconds(1f);
		StartCoroutine(PlayMelody(melody));
	}

	public IEnumerator PlayMelody(SpecialObj221melody melody)
	{
		yield return new WaitForEndOfFrame();
		string melody2 = melody.melody;
		for (int i = 0; i < melody2.Length; i++)
		{
			if (int.TryParse(melody2[i].ToString(), out var result) && result != 0)
			{
				PlayANote(result);
				yield return new WaitForSeconds(60f / (float)melody.bpm * melodyPlaySpeed);
			}
		}
		iePlayingMelody = null;
	}

	public void PlayANote(int i, bool FromMusicBox = true)
	{
		i--;
		if (FromMusicBox)
		{
			if (iePlayingNoteMusicBox != null)
			{
				StopCoroutine(iePlayingNoteMusicBox);
			}
			iePlayingNoteMusicBox = StartCoroutine(ChangeSpriteMusicBox());
		}
		else
		{
			if (keys[i].IeChangeSprite != null)
			{
				StopCoroutine(keys[i].IeChangeSprite);
			}
			keys[i].IeChangeSprite = StartCoroutine(keys[i].ChangeSprite());
			keys[i].keyAnimator.SetTrigger("Play");
		}
		AudioSource newAudio = ObjPoolMgr.Inst.GetGO("Prefabs/SpecialObjs/" + KeyAudioResourceName, 1.5f).GetComponent<AudioSource>();
		newAudio.volume = DataMgr.settingData.GetFinalSound();
		newAudio.clip = audioCLips1_5[i];
		DOTween.Sequence().AppendInterval(0.05f).AppendCallback(delegate
		{
			newAudio.Play();
		});
	}

	public IEnumerator ChangeSpriteMusicBox()
	{
		soundBoxAnimator.SetTrigger("Bounce");
		spriteRendererMusicBox.sprite = spriteMusicboxNormal;
		yield return new WaitForSeconds(0.1f);
		NoteAppearance(NotePosition.position, 1.33f);
		spriteRendererMusicBox.sprite = spriteMusicboxPlaying;
		yield return new WaitForSeconds(0.5f);
		spriteRendererMusicBox.sprite = spriteMusicboxNormal;
	}

	public void NoteAppearance(Vector3 position, float Size)
	{
		GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/SpecialObjs/" + NoteAppearanceResourceName, 1.5f);
		gO.transform.position = position;
		gO.transform.localScale = new Vector3(Size, Size, 1f);
		gO.transform.GetChild(0).GetComponent<Animator>().Play("NoteAnimation");
	}

	public void DropRward()
	{
		int rewardSpellId = OutputMgr.GetSpecialRoomSpell();
		LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(rewardSpellId));
		if (iePlayingNoteMusicBox != null)
		{
			StopCoroutine(iePlayingNoteMusicBox);
		}
		iePlayingNoteMusicBox = StartCoroutine(ChangeSpriteMusicBox());
		DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
		{
			PlayerMgr.Inst.ItemCtrller.RewardDropFly(rewardSpellId, RollRewardFly.DropType.Spell, NotePosition.position, PlayerMgr.Inst.PlayerCtrller.transform.position + new Vector3(0f, -0.5f, 0f), PlayerMgr.Inst.PlayerCtrller.myPpt.tsf_Layer.transform.position + new Vector3(0f, -0.5f, 0f));
		});
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		this.roomCtrller = roomCtrller;
	}

	public void TryAddKey(string key)
	{
		CurrentKeys += key.ToString();
		if (CurrentKeys.Length > RandomCount)
		{
			CurrentKeys = CurrentKeys.Remove(0, 1);
		}
		CheckComplete();
	}

	public bool CheckComplete()
	{
		if (CurrentKeys == currentMelody.melody.Replace("0", ""))
		{
			DropRward();
			IsComplete = true;
			return true;
		}
		return false;
	}
}
