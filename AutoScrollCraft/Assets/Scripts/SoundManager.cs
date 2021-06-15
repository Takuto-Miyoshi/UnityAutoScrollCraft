using System;
using System.Collections.Generic;
using System.Linq;
using AutoScrollCraft.Enums;
using UnityEngine;
using UnityEngine.Audio;

namespace AutoScrollCraft.Enums {
	// サウンドファイル名
	public enum SE {
		Cursor, // カーソル移動
		Submit, // 決定
		Cancel, // キャンセル
		Dash,   // ダッシュ
		Damage_NPC, // 被ダメージ：NPC
		Damage_Tree,    // 被ダメージ：木
		Damage_Rock,    // 被ダメージ：岩
		Craft,  // クラフト
		Craft_Cursor,   // クラフト対象変更
		Craft_Detail,   // クラフト詳細表示切り替え
		Hundred_Distance,   // 100m進むごとに
		Damage_Player, // 被ダメージ：プレイヤー
		Inventory_Cursor,   // インベントリカーソル切り替え
	}

	// BGMファイル名
	public enum BGM {
		Title,
		MainGame1,
		MainGame2,
		MainGame3,
		MainGame4,
	}
}

namespace AutoScrollCraft.Sound {
	public class SoundManager : Singleton<SoundManager> {
		private List<AudioClip> SEList = new List<AudioClip> ();
		private List<string> SENameList = new List<string> ();
		private List<AudioClip> BGMList = new List<AudioClip> ();
		private List<string> BGMNameList = new List<string> ();
		private AudioSource audioSource;
		[SerializeField] private AudioMixerGroup seMixer;
		[SerializeField] private AudioMixerGroup bgmMixer;

		public override void Awake () {
			base.Awake ();
			audioSource = GetComponent<AudioSource> ();

			// enumの名前を元にSE読み込み
			SENameList = Enum.GetNames ( typeof ( SE ) ).ToList ();
			foreach (var n in SENameList) {
				SEList.Add ( (AudioClip)Resources.Load ( "Sounds/SE/" + n ) );
			}

			// BGM読み込み
			BGMNameList = Enum.GetNames ( typeof ( BGM ) ).ToList ();
			foreach (var n in BGMNameList) {
				BGMList.Add ( (AudioClip)Resources.Load ( "Sounds/BGM/" + n ) );
			}
		}

		/// <summary>
		/// SEを再生
		/// </summary>
		/// <param name="sound">再生するSE</param>
		static public void Play ( SE sound ) {
			var s = sound.ToString ();
			var i = Instance.SENameList.FindIndex ( x => x == s );
			Instance.audioSource.outputAudioMixerGroup = Instance.seMixer;
			Instance.audioSource.PlayOneShot ( Instance.SEList[i] );
		}

		/// <summary>
		/// BGMを再生
		/// </summary>
		/// <param name="sound">再生するBGM</param>
		static public void Play ( BGM sound ) {
			var s = sound.ToString ();
			var i = Instance.BGMNameList.FindIndex ( x => x == s );
			Instance.audioSource.clip = Instance.BGMList[i];
			Instance.audioSource.outputAudioMixerGroup = Instance.bgmMixer;
			Instance.audioSource.Play ();
		}

		/// <summary>
		/// メインゲームBGMをランダムに再生
		/// </summary>
		static public void PlayMainGameBGM () {
			BGM[] b = { BGM.MainGame1, BGM.MainGame2, BGM.MainGame3, BGM.MainGame4 };
			Play ( b[UnityEngine.Random.Range ( 0, b.Length )] );
		}
	}
}
