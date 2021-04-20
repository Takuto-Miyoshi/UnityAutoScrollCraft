using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class DropItem : MonoBehaviour {
	[SerializeField] Items item;
	public Items Item {
		get { return item; }
		set { item = value; }
	}

	// Start is called before the first frame update
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	// アイテムを落とす
	static public void Drop ( Vector3 pos, GameObject item, int value ) {
		for (int i = 0; i < value; i++) {
			pos.x += Random.Range ( -2.0f, 2.0f );
			pos.z += Random.Range ( -2.0f, 2.0f );
			var rot = Quaternion.Euler ( 0, Random.Range ( 0.0f, 360.0f ), 0 );
			Instantiate ( item, pos, rot );
		}
	}
}
