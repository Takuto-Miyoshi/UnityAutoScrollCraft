using System;
using System.Collections;
using Enum;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
	Rigidbody rb;
	[SerializeField] Transform camTrans;
	Vector2 axis;
	bool canMove;
	[SerializeField] float speed;
	Vector3 latestPos;

	float respawnTime;

	// インタラクト
	[SerializeField] Interact interact;
	[SerializeField] float interactInterval;
	float interactTimer;

	// インベントリ
	const int MaxInventory = 3;
	const int MaxVolume = 10;
	struct ItemData {
		Items item;
		public Items Item {
			get { return item; }
			set { item = value; }
		}
		int volume;
		public int Volume {
			get { return volume; }
			set { volume = value; }
		}
	}
	ItemData[] inventory = new ItemData[MaxInventory];

	// Start is called before the first frame update
	void Start () {
		latestPos = transform.position;
		rb = GetComponent<Rigidbody> ();
		respawnTime = 5.0f;
		canMove = true;
		interactTimer = interactInterval;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canMove == true) {
			// 移動
			if (axis != Vector2.zero) {
				var vel = camTrans.forward * axis.y + camTrans.right * axis.x;
				vel.y = 0;
				rb.velocity = vel * speed;
			}

			// 移動している方向を向く
			var diff = transform.position - latestPos;
			latestPos = transform.position;
			if (diff.magnitude > 0.01f) {
				var rot = Quaternion.LookRotation ( diff );
				rot.x = 0;
				rot.z = 0;
				transform.rotation = rot;
			}

			// 落ちた場合
			if (transform.position.y <= -1) {
				canMove = false;
				Invoke ( "Respawn", respawnTime );
			}
		}

		interactTimer -= Time.deltaTime;
	}

	// 入力値の更新
	public void OnMove ( InputValue value ) {
		axis = value.Get<Vector2> ();
	}

	public void OnInteract () {
		if (interact.Target == null) return;
		if (interactTimer > 0) return;

		interact.Target.GetComponent<Status> ().Hp--;
		interactTimer = interactInterval;
	}

	// 画面の中心に戻す
	void Respawn () {
		var pos = new Vector3 ( camTrans.position.x, 1.5f, 10.0f );
		transform.position = pos;
		rb.velocity = Vector3.zero;
		canMove = true;
	}

	void OnCollisionEnter ( Collision collision ) {
		if (collision.gameObject.tag == "DropItem") {
			var i = collision.gameObject.GetComponent<DropItem> ().Item;
			for (int n = 0; n < MaxInventory; n++) {
				// 同じアイテムなら, スロットが空なら
				if (inventory[n].Item == i || inventory[n].Item == Items.Null) {
					// 所持数限界未満なら
					if (inventory[n].Volume < MaxVolume) {
						inventory[n].Item = i;
						inventory[n].Volume++;
						Destroy ( collision.gameObject );
						Debug.Log ( "ItemSlot : " + n + ", " + "ItemName : " + inventory[n].Item.ToString () + ", " + "ItemVolume : " + inventory[n].Volume );
						break;
					}
				}
			}
		}
	}
}
