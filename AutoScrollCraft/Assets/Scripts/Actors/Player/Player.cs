using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoScrollCraft;
using AutoScrollCraft.Actors.AI;
using AutoScrollCraft.Items;
using AutoScrollCraft.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AutoScrollCraft.Actors {
	public class Player : MonoBehaviour {
		Rigidbody rb;
		float respawnTime;

		// ステータス
		Status status;
		public Status Status {
			get { return status; }
		}
		bool isDead;
		// スタミナ
		const float StaminaRechargeReady = 3.0f;    // スタミナ回復が始まるまでの時間
		float staminaRechargeTimer;
		// ダッシュ
		const int DashCost = 10;    // 消費スタミナ
		const float DashInterval = 1.0f;    // ダッシュ間隔
		float dashTimer;
		const float DashPower = 30.0f;  // AddForceのパワー
		const float DashRange = 4.0f;   // どれだけダッシュしたら止まるか
		Vector3 dashStart;
		// 落下ダメージ
		const int FallenDamage = 30;

		// 移動
		[SerializeField] Transform camTrans;
		Vector2 axis;
		bool canMove;
		[SerializeField] float speed;
		Vector3 latestPos;

		// インタラクト
		// [SerializeField] Interact interact;
		[SerializeField] float interactInterval;    // 間隔
		float interactTimer;
		const int interactCost = 5; // 消費スタミナ

		// インベントリ
		const int maxInventory = 3;
		const int maxVolume = 10;
		public struct ItemData {
			Enums.Items item;
			public Enums.Items Item {
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
		int currentSelectOnInventory;
		public int CurrentSelectOnInventory {
			get { return currentSelectOnInventory; }
		}

		// アイテム
		ItemFunctions itemFunctions;
		const float useInterval = 0.5f; // アイテムの使用間隔
		float useTimer;

		// クラフト
		[SerializeField] CraftWindow craftWindow;
		int currentSelectOnRecipe;
		public int CurrentSelectOnRecipe {
			get { return currentSelectOnRecipe; }
		}

		//----------------------------------------------------------------------

		// Start is called before the first frame update
		void Start () {
			rb = GetComponent<Rigidbody> ();
			status = GetComponent<Status> ();
			itemFunctions = GetComponent<ItemFunctions> ();

			respawnTime = 5.0f;
			staminaRechargeTimer = StaminaRechargeReady;
			latestPos = transform.position;
			isDead = false;
			canMove = true;
			interactTimer = interactInterval;
			useTimer = useInterval;
			StartCoroutine ( StartDelay () );
		}

		IEnumerator StartDelay () {
			yield return null;
			craftWindow.UpdateCraftUI ( this );
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
				if ((dashStart - transform.position).magnitude > DashRange || dashTimer < 0) {
					rb.velocity = Vector3.zero;
					canMove = true;
				}
			}
		}

		void Update () {
			dashTimer -= Time.deltaTime;
			interactTimer -= Time.deltaTime;
			useTimer -= Time.deltaTime;

			// スタミナ回復
			staminaRechargeTimer -= Time.deltaTime;
			if (staminaRechargeTimer < 0 && status.Stamina < status.MaxStamina) {
				staminaRechargeTimer = 0.2f; // 毎フレーム回復するとまずいので
				status.Stamina++;
			}
		}

		// 画面の中心に戻す
		void Respawn () {
			status.Hp -= FallenDamage;
			var pos = new Vector3 ( camTrans.position.x, 1, 10 );
			transform.position = pos;
			rb.velocity = Vector3.zero;
			canMove = true;
			isDead = false;
		}

		void OnCollisionEnter ( Collision collision ) {
			// アイテム取得
			var obj = collision.gameObject;
			if (obj.tag == "DropItem") {
				var i = obj.GetComponent<DropedItem> ().Item;
				for (int n = 0; n < maxInventory; n++) {
					if (inventory[n].Item == i && inventory[n].Volume < maxVolume) {
						PickUpItem ( n, i, obj );
						return;
					}
				}

				for (int n = 0; n < maxInventory; n++) {
					if (inventory[n].Item == Enums.Items.Null) {
						PickUpItem ( n, i, obj );
						return;
					}
				}
			}
		}

		void PickUpItem ( int num, Enums.Items fallenItem, GameObject obj ) {
			inventory[num].Item = fallenItem;
			inventory[num].Volume++;
			Destroy ( obj );
			inventoryUI.UpdateInventoryUI ( this );
		}

		// ---------------入力系---------------------------
		public void OnMove ( InputValue value ) {
			axis = value.Get<Vector2> ();
		}

		public void OnInteract () {
			if (interactTimer > 0) return;
			if (status.Stamina < interactCost) return;

			Physics.Raycast ( transform.position, transform.forward, out RaycastHit r, 1.5f );
			if (r.collider?.tag == "Object") {
				r.collider.GetComponent<Status> ().Hp -= status.AttackPower;
				var n = r.collider.GetComponent<NPCBase> ();
				if (n != null) n.DamageSource = transform.position;
				n?.TakeDamageProc ();
				staminaRechargeTimer = StaminaRechargeReady;
				interactTimer = interactInterval;
				status.Stamina -= interactCost;
			}
		}

		public void OnDash () {
			if (dashTimer > 0) return;
			if (status.Stamina < DashCost) return;

			canMove = false;
			dashStart = transform.position;
			rb.AddForce ( transform.forward * DashPower, ForceMode.Impulse );
			status.Stamina -= DashCost;
			staminaRechargeTimer = StaminaRechargeReady;
			dashTimer = DashInterval;
		}

		public void OnSelectItem ( InputValue value ) {
			var axis = value.Get<float> ();
			if (axis == -1) {
				currentSelectOnInventory--;
			}
			else if (axis == 1) {
				currentSelectOnInventory++;
			}
			// 配列で使う値なので最大値は-1しておく
			currentSelectOnInventory = UIFunctions.RevisionValue ( currentSelectOnInventory, maxInventory - 1, UIFunctions.RevisionMode.Limit );
			inventoryUI.UpdateCursorUI ( this );
		}

		public void OnTrashItem () {
			if (inventory[currentSelectOnInventory].Item == Enums.Items.Null) return;

			inventory[currentSelectOnInventory].Volume--;
			// アイテムを地面に落とす
			var l = ItemList.Instance.Names.ToList ();
			var i = l.FindIndex ( x => x == inventory[currentSelectOnInventory].Item.ToString () );
			Instantiate ( ItemList.Instance.Objects[i], transform.position + transform.forward * 1.5f, Quaternion.identity );
			inventoryUI.UpdateInventoryUI ( this );
		}

		public void OnUseItem () {
			if (inventory[currentSelectOnInventory].Item == Enums.Items.Null) return;
			if (useTimer > 0) return;

			useTimer = useInterval;

			if (itemFunctions.Exec ( inventory[currentSelectOnInventory].Item ) == true) {
				inventory[currentSelectOnInventory].Volume--;
			}

			inventoryUI.UpdateInventoryUI ( this );
		}

		public void OnCraft () {
			if (Craft.CanBeCrafting ( inventory, currentSelectOnRecipe )) {
				var target = Craft.Recipes[currentSelectOnRecipe];
				{
					var i = target.Result;
					var n = target.ResultAmount;
					ItemList.Instance.Drop ( transform.position, ItemList.Instance.GetGameObject ( i ), n );
				}

				// 持ち物を減らす
				for (int m = 0; m < target.Materials.ToArray ().Length; m++) {
					for (int i = 0; i < inventory.Length; i++) {
						if (inventory[m].Item == target.Materials[m]) {
							inventory[m].Volume -= target.MaterialAmountList[m];
							break;
						}
					}
				}
				inventoryUI.UpdateInventoryUI ( this );
			}
		}

		public void OnCraftDetail () {
			craftWindow.Detail.SetActive ( !craftWindow.Detail.activeSelf );
		}

		public void OnChooseCraft ( InputValue value ) {
			var axis = value.Get<float> ();
			if (axis == -1) {
				currentSelectOnRecipe--;
			}
			else if (axis == 1) {
				currentSelectOnRecipe++;
			}
			currentSelectOnRecipe = UIFunctions.RevisionValue ( currentSelectOnRecipe, Craft.MaxRecipeNumber, UIFunctions.RevisionMode.Loop );
			craftWindow.UpdateCraftUI ( this );
		}
	}
}