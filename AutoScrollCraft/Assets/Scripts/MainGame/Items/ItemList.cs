using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoScrollCraft.Enums {
	// ↓アイテムはここに追加
	public enum Items {
		Null,
		Wood,
		Stone,
		Pork,
		Beef,
		Arrow,
		Axe,
		Pickaxe,
		Sword,
	};
}

namespace AutoScrollCraft.Items {
	public class ItemList : Singleton<ItemList> {
		private List<Texture> images = new List<Texture> ();
		public List<Texture> Images { get => images; }
		private List<GameObject> objects = new List<GameObject> ();
		public List<GameObject> Objects { get => objects; }
		private List<string> itemNameList = new List<string> ();
		public List<string> ItemNameList { get => itemNameList; }

		public override void Awake () {
			base.Awake ();

			// アイテム名を取得
			itemNameList = Enum.GetNames ( typeof ( Enums.Items ) ).ToList ();

			// 画像、オブジェクトをロード
			itemNameList.ForEach ( x => {
				images.Add ( (Texture)Resources.Load ( "Textures/UI/ItemIcons/" + x ) );
				objects.Add ( (GameObject)Resources.Load ( "Items/" + x ) );
			} );
		}

		public GameObject GetGameObject ( Enums.Items item ) {
			// 同名のオブジェクトを取得
			var index = itemNameList.ToList ().FindIndex ( x => x == item.ToString () );
			return objects[index];
		}

		public Texture GetTexture ( Enums.Items item ) {
			// 同名のテクスチャを取得
			var index = itemNameList.ToList ().FindIndex ( x => x == item.ToString () );
			return images[index];
		}

		// 付近にアイテムを落とす
		public void Drop ( Vector3 pos, GameObject item, int value ) {
			// ドロップ数だけ周りにまき散らす
			for (int i = 0; i < value; i++) {
				pos.x += UnityEngine.Random.Range ( -2.0f, 2.0f );
				pos.z += UnityEngine.Random.Range ( -2.0f, 2.0f );
				pos.y = 0.5f;
				var rot = Quaternion.Euler ( 0, UnityEngine.Random.Range ( 0.0f, 360.0f ), 0 );
				Instantiate ( item, pos, rot );
			}
		}
	}
}