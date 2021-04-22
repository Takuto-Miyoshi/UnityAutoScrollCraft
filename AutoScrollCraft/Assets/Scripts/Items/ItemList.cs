using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;

namespace Enums {
	public enum Items {
		Null,
		Wood,
		Stone,
		Pork,
	};
}

public class ItemList : MonoBehaviour {
	static bool loaded = false;
	static Texture[] textures;
	static public Texture[] Textures {
		get { return textures; }
	}
	static GameObject[] objects;
	static public GameObject[] Objects {
		get { return objects; }
	}
	static string[] names;
	static public string[] Names {
		get { return names; }
	}

	// Start is called before the first frame update
	void Start () {
		if (loaded == false) {
			loaded = true;
			Array.Resize ( ref textures, Enum.GetValues ( typeof ( Items ) ).Length );
			Array.Resize ( ref objects, textures.Length );
			names = Enum.GetNames ( typeof ( Items ) );
			// 画像、オブジェクトをロード
			for (int i = 0; i < names.Length; i++) {
				textures[i] = (Texture)Resources.Load ( "Textures/UI/ItemIcons/" + names[i] );
				objects[i] = (GameObject)Resources.Load ( "Items/" + names[i] );
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}

	static public GameObject GetGameObject ( Items item ) {
		var n = names.ToList ().FindIndex ( x => x == item.ToString () );
		return objects[n];
	}

	static public Texture GetTexture ( Items item ) {
		var n = names.ToList ().FindIndex ( x => x == item.ToString () );
		return textures[n];
	}
}
