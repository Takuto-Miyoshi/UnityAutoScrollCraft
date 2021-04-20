using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Enums;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
	Rigidbody rb;
	float respawnTime;

	// ステータス
	Status status;
	bool isDead;
	// スタミナ
	const float staminaRechargeReady = 3.0f;    // スタミナ回復が始まるまでの時間
	float staminaRechargeTimer;
	// ダッシュ
	const int dashCost = 10;    // 消費スタミナ
	const float dashInterval = 1.0f;    // ダッシュ間隔
	float dashTimer;
	const float dashPower = 30.0f;  // AddForceのパワー
	const float dashRange = 4.0f;   // どれだけダッシュしたら止まるか
	Vector3 dashStart;

	// 移動
	[SerializeField] Transform camTrans;
	Vector2 axis;
	bool canMove;
	[SerializeField] float speed;
	Vector3 latestPos;

	// インタラクト
	[SerializeField] Interact interact;
	[SerializeField] float interactInterval;    // 間隔
	float interactTimer;
	const int interactCost = 5; // 消費スタミナ

	// インベントリ
	const int maxInventory = 3;
	const int maxVolume = 10;
	public struct ItemData {
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
	ItemData[] inventory = new ItemData[maxInventory];
	public ItemData[] Inventory {
		get { return inventory; }
	}
	[SerializeField] Inventory inventoryUI;
	int currentSelect;
	public int CurrentSelect {
		get { return currentSelect; }
	}

	//----------------------------------------------------------------------

	// Start is called before the first frame update
	void Start () {
		rb = GetComponent<Rigidbody> ();
		status = GetComponent<Status> ();

		respawnTime = 5.0f;
		staminaRechargeTimer = staminaRechargeReady;
		latestPos = transform.position;
		isDead = false;
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
			if (transform.position.y <= 0) {
				canMove = false;
				isDead = true;
				Invoke ( "Respawn", respawnTime );
			}
		}
		else if (canMove == false && isDead == false) {
			// 一定距離移動した　ダッシュできるようになったら(スタック対策)
			if ((dashStart - transform.position).magnitude > dashRange || dashTimer < 0) {
				rb.velocity = Vector3.zero;
				canMove = true;
			}
		}

		interactTimer -= Time.deltaTime;
	}

	void Update () {
		dashTimer -= Time.deltaTime;

		// スタミナ回復
		staminaRechargeTimer -= Time.deltaTime;
		if (staminaRechargeTimer < 0 && status.Stamina < status.MaxStamina) {
			staminaRechargeTimer = 0.1f;
			status.Stamina++;
		}
	}

	// 画面の中心に戻す
	void Respawn () {
		var pos = new Vector3 ( camTrans.position.x, 1, 10 );
		transform.position = pos;
		rb.velocity = Vector3.zero;
		canMove = true;
		isDead = false;
	}

	void OnCollisionEnter ( Collision collision ) {
		// アイテム取得
		if (collision.gameObject.tag == "DropItem") {
			var i = collision.gameObject.GetComponent<DropItem> ().Item;
			for (int n = 0; n < maxInventory; n++) {
				if (CanBeTakeItem ( n, i )) {
					inventory[n].Item = i;
					inventory[n].Volume++;
					Destroy ( collision.gameObject );
					inventoryUI.UpdateInventoryUI ( this );
					break;
				}
			}
		}
	}

	// アイテムを取得できるか
	bool CanBeTakeItem ( int num, Items fallenItem ) {
		return ((inventory[num].Item == fallenItem || inventory[num].Item == Items.Null) && inventory[num].Volume < maxVolume);
	}

	// ---------------入力系---------------------------
	public void OnMove ( InputValue value ) {
		// 入力値の更新
		axis = value.Get<Vector2> ();
	}

	public void OnInteract () {
		if (interact.Target == null) return;
		if (interactTimer > 0) return;

		status.Stamina -= interactCost;
		staminaRechargeTimer = staminaRechargeReady;
		interact.Target.GetComponent<Status> ().Hp--;
		interactTimer = interactInterval;
	}

	public void OnDash () {
		if (dashTimer > 0) return;
		if (status.Stamina < dashCost) return;

		canMove = false;
		dashStart = transform.position;
		rb.AddForce ( transform.forward * dashPower, ForceMode.Impulse );
		status.Stamina -= dashCost;
		staminaRechargeTimer = staminaRechargeReady;
		dashTimer = dashInterval;
	}

	public void OnSelectItem ( InputValue value ) {
		var axis = value.Get<float> ();
		if (axis == -1) {
			currentSelect--;
		}
		else if (axis == 1) {
			currentSelect++;
		}
		// 配列で使う値なので最大値は-1しておく
		currentSelect = UIFunctions.RevisionValue ( currentSelect, maxInventory - 1, UIFunctions.RevisionMode.Limit );
		inventoryUI.UpdateCursorUI ( this );
	}
}
