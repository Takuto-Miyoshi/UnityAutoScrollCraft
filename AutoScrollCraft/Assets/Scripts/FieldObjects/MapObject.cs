using System.Collections;
using System.Collections.Generic;
using AutoScrollCraft.Actors;
using AutoScrollCraft.Items;
using UnityEngine;

namespace AutoScrollCraft.FieldObjects {
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
				if (dropItem != null) {
					ItemList.Instance.Drop ( transform.position, dropItem, Random.Range ( 2, 4 ) );
				}

				Destroy ( gameObject );
			}

			// HPが半分になったら装飾を消してアイテムを落とす
			if (decoration != null) {
				if (decoration.activeSelf == true && status.Hp <= status.MaxHp / 2) {
					decoration.SetActive ( false );
					if (dropItem != null) {
						ItemList.Instance.Drop ( transform.position, dropItem, Random.Range ( 1, 3 ) );
					}
				}
			}
		}
	}
}
