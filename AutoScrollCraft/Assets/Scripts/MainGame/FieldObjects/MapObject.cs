using AutoScrollCraft.Actors;
using AutoScrollCraft.Enums;
using AutoScrollCraft.Items;
using UnityEngine;

namespace AutoScrollCraft.FieldObjects {
	public class MapObject : MonoBehaviour {
		private Status status;
		public Status Status { get => status; set => status = value; }
		[SerializeField] private GameObject dropItem;
		[SerializeField] private GameObject decoration;

		private void Start () {
			Status = GetComponent<Status> ();
		}

		public bool TakeDamageProc ( int damage ) {
			Status.Hp -= damage;

			// HPが0になったらアイテムを落としてDestroy
			if (Status.Hp <= 0) {
				if (dropItem != null) {
					ItemList.Instance.Drop ( transform.position, dropItem, Random.Range ( 2, 4 ) );
				}

				Destroy ( gameObject, 0.01f );
				return true;
			}

			// HPが半分になったら装飾を消してアイテムを落とす
			if (decoration != null) {
				if (decoration.activeSelf == true && Status.Hp <= Status.MaxHp / 2) {
					decoration.SetActive ( false );
					if (dropItem != null) {
						ItemList.Instance.Drop ( transform.position, dropItem, Random.Range ( 1, 3 ) );
					}
				}
			}

			return false;
		}

		private void OnBecameInvisible () {
			Destroy ( gameObject, 3.0f );
		}
	}
}
