using System.Collections.Generic;
using System.Linq;
using AutoScrollCraft.Enums;
using UnityEngine;

namespace AutoScrollCraft.Enums {
	// SEファイル名
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
		private List<AudioClip> SEList = new List<AudioClip> ();    // ロードしたSEを格納
		private List<string> SENameList = new List<string> ();  // SEの名前を格納
		private List<AudioClip> BGMList = new List<AudioClip> ();   // ロードしたBGMを格納
		private List<string> BGMNameList = new List<string> (); // BGMの名前を格納
		[SerializeField] private AudioSource seAudioSource;
		[SerializeField] private AudioSource bgmAudioSource;

		public override void Awake () {
			base.Awake ();

			SENameList = System.Enum.GetNames ( typeof ( SE ) ).ToList ();
			// SEの名前を元にロード
			SENameList.ForEach ( x => SEList.Add ( (AudioClip)Resources.Load ( "Sounds/SE/" + x ) ) );

			// BGMのロード
			BGMNameList = System.Enum.GetNames ( typeof ( BGM ) ).ToList ();
			BGMNameList.ForEach ( x => BGMList.Add ( (AudioClip)Resources.Load ( "Sounds/BGM/" + x ) ) );
		}

		/// <summary>
		/// SEを再生
		/// </summary>
		/// <param name="sound">再生するSE</param>
		public void Play ( SE sound ) {
			var target = sound.ToString ();
			// 指定されたSEと同じものを探す
			var index = Instance.SENameList.FindIndex ( x => x == target );
			Instance.seAudioSource.PlayOneShot ( Instance.SEList[index] );
		}

		/// <summary>
		/// BGMを再生
		/// </summary>
		/// <param name="sound">再生するBGM</param>
		public void Play ( BGM sound ) {
			var target = sound.ToString ();
			// 指定されたBGMと同じものを探す
			var index = Instance.BGMNameList.FindIndex ( x => x == target );
			Instance.bgmAudioSource.clip = Instance.BGMList[index];
			Instance.bgmAudioSource.Play ();
		}

		/// <summary>
		/// メインゲームBGMをランダムに再生
		/// </summary>
		public void PlayMainGameBGM () {
			BGM[] bgm = { BGM.MainGame1, BGM.MainGame2, BGM.MainGame3, BGM.MainGame4 };
			Play ( bgm[Random.Range ( 0, bgm.Length )] );
		}

		/// <summary>
		/// オブジェクトタイプを元にSEを取得
		/// </summary>
		/// <param name="type">オブジェクトの種類</param>
		/// <returns>enum SE</returns>
		public SE GetSEByObjectType ( ObjectType type ) {
			return type switch
			{
				ObjectType.Rock => SE.Damage_Rock,
				ObjectType.Tree => SE.Damage_Tree,
				_ => SE.Damage_Rock // 適当に返す
			};
		}
	}
}
