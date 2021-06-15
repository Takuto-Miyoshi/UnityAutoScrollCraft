using System;
using System.Linq;
using AutoScrollCraft;
using AutoScrollCraft.Actors.AI;
using AutoScrollCraft.Enums;
using AutoScrollCraft.FieldObjects;
using AutoScrollCraft.Items;
using AutoScrollCraft.Sound;
using AutoScrollCraft.UI;
using AutoScrollCraft.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AutoScrollCraft.Actors {
	public class Player : MonoBehaviour {
		private Rigidbody rigidBody;
		private const float RespawnTime = 5.0f;

		// ステータス
		private Status status;
		public Status Status { get => status; }
		private const float InvincibleInterval = 1.0f;
		private bool isInvincible;
		private const int InvincibleDamage = 3; // 無敵時間の発生するダメージ
		private bool isGameOver;
		public bool IsGameOver { get => isGameOver; }
		// HP
		private const float HpRegenerateInterval = 2.0f;  // HP回復の間隔
		private bool canRegenerateHp = true;

		// スタミナ
		private const float StaminaRegenerateInterval = 1.5f; // スタミナ回復の間隔
		private const float StaminaRegenerateIntervalToWait = 0.3f; // 待機時の回復間隔
		private bool canRegenerateStamina = true;

		// ダッシュ
		private const int DashCost = 10;    // 消費スタミナ
		private const float DashInterval = 1.0f;    // ダッシュ間隔
		private bool canDash = true;
		private bool isDashed = false;
		private const float DashPower = 30.0f;  // AddForceのパワー
		private const float DashRange = 4.0f;   // どれだけダッシュしたら止まるか
		private Vector3 dashStart;
		// 落下ダメージ
		private const int FallenDamage = 30;

		// 移動
		[SerializeField] private Transform camTrans;
		private Vector2 axis;
		private bool canMove;
		[SerializeField] private float speed;
		private Vector3 beforePos;

		// インタラクト
		private const float interactInterval = 0.5f;    // 間隔
		private bool canInteract = true;
		private const int interactCost = 5; // 消費スタミナ

		// インベントリ
		private const int maxInventory = 3;
		private const int maxVolume = 30;
		public struct ItemData {
			private Enums.Items item;
			public Enums.Items Item { get => item; set => item = value; }
			private int volume;
			public int Volume { get => volume; set => volume = value; }
		}
		private ItemData[] inventory = new ItemData[maxInventory];
		public ItemData[] Inventory { get => inventory; }
		[SerializeField] private Inventory inventoryUI;
		private int currentSelectOnInventory;
		public int CurrentSelectOnInventory { get => currentSelectOnInventory; }

		// アイテム
		private ItemFunctions itemFunctions;
		private const float UseInterval = 0.5f; // アイテムの使用間隔
		private bool canUsing = true;

		// クラフト
		[SerializeField] private CraftWindow craftWindow;
		private int currentSelectOnRecipe;
		public int CurrentSelectOnRecipe { get => currentSelectOnRecipe; }

		// スコア
		private int killScore;
		public int KillScore { get => killScore; set => killScore = value; }
		private int craftScore;
		public int CraftScore { get => craftScore; set => craftScore = value; }
		private int maxXDistance;
		public int MaxXDistance { get => maxXDistance; set => maxXDistance = value; }
		private int distanceScore;
		public int DistanceScore { get => distanceScore; set => distanceScore = value; }

		//----------------------------------------------------------------------

		private void Awake () {
			rigidBody = GetComponent<Rigidbody> ();
			status = GetComponent<Status> ();
			itemFunctions = GetComponent<ItemFunctions> ();

			beforePos = transform.position;
			canMove = true;
		}

		private async void Start () {
			await UniTask.Delay ( 10 );
			craftWindow.UpdateCraftUI ( this );
		}

		private void FixedUpdate () {
			if (isGameOver == true) return;

			if (canMove == true) {
				// 移動
				if (axis != Vector2.zero) {
					var vel = camTrans.forward * axis.y + camTrans.right * axis.x;
					vel.y = 0;
					rigidBody.velocity = vel * speed;
				}

				// 移動している方向を向く
				var diff = transform.position - beforePos;
				if (diff.magnitude > 0.01f) {
					var rot = Quaternion.LookRotation ( diff );
					rot.x = 0;
					rot.z = 0;
					transform.rotation = rot;
				}

				// 落ちた場合
				if (transform.position.y <= -0) {
					canMove = false;
					Invoke ( "Respawn", RespawnTime );
				}
			}
			else if (isDashed == true) {
				// 一定距離移動した場合
				if ((dashStart - transform.position).magnitude > DashRange) {
					rigidBody.velocity = Vector3.zero;
					canMove = true;
				}

				// ダッシュしたのに停止している場合
				if (beforePos == transform.position) {
					canMove = true;
				}
			}

			RegenerateHp ();
			RegenerateStamina ();

			beforePos = transform.position;

			var beforeMax = maxXDistance;
			if (maxXDistance < (int)transform.position.x) maxXDistance = (int)transform.position.x;
			// 前のX最大から次のボーナスポイントまでの距離 < 前のX最大から移動した距離
			if (100 - (beforeMax % 100) < maxXDistance - beforeMax) SoundManager.Play ( SE.Hundred_Distance );

			UpdateScore ();
		}

		// HP回復
		// スタミナが最大の時に回復する
		private async void RegenerateHp () {
			if (canRegenerateHp == false) return;
			if (status.Stamina != status.MaxStamina) return;
			if (status.Hp >= status.MaxHp) return;

			status.Hp++;
			canRegenerateHp = false;
			await UniTask.Delay ( TimeSpan.FromSeconds ( HpRegenerateInterval ) );
			canRegenerateHp = true;
		}

		// スタミナ回復
		private async void RegenerateStamina () {
			if (canRegenerateStamina == false) return;
			if (status.Stamina >= status.MaxStamina) return;

			status.Stamina++;
			canRegenerateStamina = false;
			var i = (beforePos == transform.position) ? StaminaRegenerateIntervalToWait : StaminaRegenerateInterval;
			await UniTask.Delay ( TimeSpan.FromSeconds ( i ) );
			canRegenerateStamina = true;
		}

		private void Respawn () {
			TakeDamageProc ( FallenDamage );
			// 画面の中心に戻す
			var pos = new Vector3 ( camTrans.position.x, 1, 10 );
			transform.position = pos;
			rigidBody.velocity = Vector3.zero;
			canMove = true;
		}

		private void OnCollisionEnter ( Collision collision ) {
			// アイテム取得
			var o = collision.gameObject;
			if (o.tag == "DropItem") {
				var i = o.GetComponent<DroppedItem> ();
				for (int n = 0; n < maxInventory; n++) {
					// スタックする
					if (inventory[n].Item == i.Item && inventory[n].Volume < maxVolume) {
						PickUpItem ( n, i );
						return;
					}
				}

				for (int n = 0; n < maxInventory; n++) {
					// 新しいスロットに入れる
					if (inventory[n].Item == Enums.Items.Null) {
						PickUpItem ( n, i );
						return;
					}
				}
			}

			if (o.tag == "Projectile") {
				var p = o.GetComponent<ProjectileBase> ();
				if (p.Master != gameObject) {
					TakeDamageProc ( p.Damage );
				}
			}
		}

		public async void TakeDamageProc ( int damage ) {
			if (isInvincible == true) return;

			SoundManager.Play ( SE.Damage_Player );

			status.Hp -= damage;
			// HPが0以下ならゲームオーバー
			if (status.Hp <= 0) {
				isGameOver = true;
				GameController.Instance.ShowGameOverScreen ();
			}

			if (damage >= InvincibleDamage) {
				isInvincible = true;
				InvincibleFlashing ();
				await UniTask.Delay ( TimeSpan.FromSeconds ( InvincibleInterval ) );
				isInvincible = false;
			}
		}

		private async void InvincibleFlashing () {
			var o = GetComponent<MeshRenderer> ();
			if (isInvincible == false) {
				o.enabled = true;
				return;
			}
			else {
				o.enabled = !o.enabled;
			}

			await UniTask.Delay ( TimeSpan.FromSeconds ( 0.1f ) );
			InvincibleFlashing ();
		}

		private void PickUpItem ( int num, DroppedItem item ) {
			inventory[num].Item = item.Item;
			inventory[num].Volume++;
			Destroy ( item.gameObject );
			inventoryUI.UpdateInventoryUI ( this );
		}

		private void UpdateScore () {
			// 進んだ距離
			distanceScore = maxXDistance * 10;
			// 100ごとに+1000
			distanceScore += maxXDistance / 100 * 1000;

			status.Score = distanceScore;
			status.Score += killScore;
			status.Score += craftScore;
		}

		// ---------------入力系---------------------------
		public void OnMove ( InputValue value ) {
			axis = value.Get<Vector2> ();
		}

		public void OnInteract () {
			Interact ( status.AttackPower, ObjectType.None );
		}

		public async void Interact ( int attackPower, ObjectType criticalTarget ) {
			if (canInteract == false) return;
			if (status.Stamina < interactCost) return;

			Physics.Raycast ( transform.position, transform.forward, out RaycastHit r, 1.5f );
			bool attacked = false;
			// 適正ツールでないならプレイヤーの攻撃力を適用
			if (r.collider?.GetComponent<Status> ().ObjectType != criticalTarget || criticalTarget == ObjectType.None) {
				attackPower = status.AttackPower;
			}

			// 攻撃対象：NPC
			if (r.collider?.tag == "NPC") {
				var n = r.collider.GetComponent<NPCBase> ();
				if (n?.TakeDamageProc ( attackPower, transform.position ) == true) killScore += n.Status.Score;
				attacked = true;
			}

			// 攻撃対象：フィールドオブジェクト
			if (r.collider?.tag == "Object") {
				var o = r.collider.GetComponent<MapObject> ();
				if (o?.TakeDamageProc ( attackPower ) == true) killScore += o.Status.Score;
				attacked = true;
			}

			if (attacked == true) {
				status.Stamina -= interactCost;
				canInteract = false;
				await UniTask.Delay ( TimeSpan.FromSeconds ( interactInterval ) );
				canInteract = true;
			}
		}

		public async void OnDash () {
			if (canDash == false) return;
			if (status.Stamina < DashCost) return;

			SoundManager.Play ( SE.Dash );

			canMove = false;
			dashStart = transform.position;
			rigidBody.AddForce ( transform.forward * DashPower, ForceMode.Impulse );
			status.Stamina -= DashCost;
			isDashed = true;
			canDash = false;

			await UniTask.Delay ( TimeSpan.FromSeconds ( DashInterval ) );
			isDashed = false;
			canDash = true;
		}

		public void OnSelectItem ( InputValue value ) {
			SoundManager.Play ( SE.Inventory_Cursor );
			currentSelectOnInventory += (int)value.Get<float> ();
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

		public async void OnUseItem () {
			if (inventory[currentSelectOnInventory].Item == Enums.Items.Null) return;
			if (canUsing == false) return;

			if (itemFunctions.Exec ( inventory[currentSelectOnInventory].Item ) == true) {
				inventory[currentSelectOnInventory].Volume--;
			}

			inventoryUI.UpdateInventoryUI ( this );

			canUsing = false;
			await UniTask.Delay ( TimeSpan.FromSeconds ( UseInterval ) );
			canUsing = true;
		}

		public void OnCraft () {
			if (Craft.CanBeCrafting ( inventory, currentSelectOnRecipe )) {
				SoundManager.Play ( SE.Craft );

				var target = Craft.Recipes[currentSelectOnRecipe];
				{
					var i = target.Result;
					var n = target.ResultAmount;
					ItemList.Instance.Drop ( transform.position, ItemList.Instance.GetGameObject ( i ), n );
				}
				craftScore += target.Score;

				// 持ち物を減らす
				for (int m = 0; m < target.Materials.Count; m++) {
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
			SoundManager.Play ( SE.Craft_Detail );
			craftWindow.Detail.SetActive ( !craftWindow.Detail.activeSelf );
		}

		public void OnChooseCraft ( InputValue value ) {
			SoundManager.Play ( SE.Craft_Cursor );
			currentSelectOnRecipe += (int)value.Get<float> ();
			currentSelectOnRecipe = UIFunctions.RevisionValue ( currentSelectOnRecipe, Craft.MaxRecipeNumber, UIFunctions.RevisionMode.Loop );
			craftWindow.UpdateCraftUI ( this );
		}
	}
}