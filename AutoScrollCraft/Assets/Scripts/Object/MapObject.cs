using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour {
	Status status;
	[SerializeField] GameObject dropItem;
	[SerializeField] GameObject decoration;

	// Start is called before the first frame update
	void Start () {
		status = GetComponent<Status> ();
	}

	// Update is called once per frame
	void Update () {
		// HPが0になったらアイテムを落としてDestroy
		if (status.Hp <= 0) {
			DropItems ( Random.Range ( 2, 4 ) );
			Destroy ( gameObject );
		}

		// HPが半分になったら装飾を消してアイテムを落とす
		if (decoration != null) {
			if (decoration.activeSelf == true) {
				if (status.Hp <= status.MaxHp / 2) {
					decoration.SetActive ( false );
					DropItems ( Random.Range ( 1, 3 ) );
				}
			}
		}
	}

	// 周りにアイテムを落とす
	void DropItems ( int value ) {
		for (int i = 0; i < value; i++) {
			var pos = transform.position;
			pos.x += Random.Range ( -2.0f, 2.0f );
			pos.z += Random.Range ( -2.0f, 2.0f );
			var rot = Quaternion.Euler ( 0, Random.Range ( 0.0f, 360.0f ), 0 );
			Instantiate ( dropItem, pos, rot );
		}
	}
}
