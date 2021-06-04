using System;
using System.Linq;
using UnityEngine;

namespace AutoScrollCraft.Enums {
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
		private Texture[] images;
		public Texture[] Textures { get => images; }
		private GameObject[] objects;
		public GameObject[] Objects { get => objects; }
		private string[] names;
		public string[] Names { get => names; }

		public override void Awake () {
			base.Awake ();
			Array.Resize ( ref images, Enum.GetValues ( typeof ( Enums.Items ) ).Length );
			Array.Resize ( ref objects, images.Length );
			names = Enum.GetNames ( typeof ( Enums.Items ) );
			// 画像、オブジェクトをロード
			for (int i = 0; i < names.Length; i++) {
				images[i] = (Texture)Resources.Load ( "Textures/UI/ItemIcons/" + names[i] );
				objects[i] = (GameObject)Resources.Load ( "Items/" + names[i] );
			}
		}

		public GameObject GetGameObject ( Enums.Items item ) {
			var n = names.ToList ().FindIndex ( x => x == item.ToString () );
			return objects[n];
		}

		public Texture GetTexture ( Enums.Items item ) {
			var n = names.ToList ().FindIndex ( x => x == item.ToString () );
			return images[n];
		}

		// 付近にアイテムを落とす
		public void Drop ( Vector3 pos, GameObject item, int value ) {
			for (int i = 0; i < value; i++) {
				var p = pos;
				p.x += UnityEngine.Random.Range ( -2.0f, 2.0f );
				p.z += UnityEngine.Random.Range ( -2.0f, 2.0f );
				p.y = 0.5f;
				var rot = Quaternion.Euler ( 0, UnityEngine.Random.Range ( 0.0f, 360.0f ), 0 );
				Instantiate ( item, p, rot );
			}
		}
	}
}