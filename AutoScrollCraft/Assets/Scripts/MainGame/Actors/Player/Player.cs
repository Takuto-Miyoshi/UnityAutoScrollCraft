using System;
using System.Linq;
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
		private bool disabledOperation = false;

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
		[SerializeField] private Transform cameraTrans;
		private bool notMoving;
		public bool NotMoving { get => notMoving; }
		private Vector2 axis;
		private bool canMove;
		[SerializeField] private float speed;
		private Vector3 beforePos;

		// インタラクト
		private const float interactInterval = 0.5f;    // 間隔
		private bool canInteract = true;
		private const int interactCost = 5; // 消費スタミナ
		private bool nearObject = false;    // KeyInfo表示用：オブジェクトが近くにある判定
		public bool NearObject { get => nearObject; }

		// インベントリ
		private const int maxInventory = 3;
		private const int maxVolume = 30;
		public struct ItemData {
			private Enums.Items item;
			public Enums.Items Item { get => item; set => item = value; }
			private int amount;
			public int Amount { get => amount; set => amount = value; }
		}
		private ItemData[] inventory = new ItemData[maxInventory];
		public ItemData[] Inventory { get => inventory; }
		[SerializeField] private Inventory inventoryUI;
		private int currentSelectOnInventory;
		public int CurrentSelectOnInventory { get => currentSelectOnInventory; }
		private bool fullInventory = false; // KeyInfo用：インベントリいっぱいフラグ
		public bool FullInventory { get => fullInventory; }

		// アイテム
		private ItemFunctions itemFunctions;
		private const float UseInterval = 0.5f; // アイテムの使用間隔
		private bool canUsing = true;
		private bool takeItem = false;  // KeyInfo用：アイテム取得フラグ
		public bool TakeItem { get => takeItem; }

		// クラフト
		[SerializeField] private CraftWindow craftWindow;
		private int currentSelectOnRecipe;
		public int CurrentSelectOnRecipe { get => currentSelectOnRecipe; }

		// スコア
		private int killScore;  // 撃破によるスコア
		public int KillScore { get => killScore; set => killScore = value; }
		private int craftScore; // クラフトによるスコア
		public int CraftScore { get => craftScore; set => craftScore = value; }
		private int maxXDistance;   // Ｘ座標の最高到達点
		public int MaxXDistance { get => maxXDistance; set => maxXDistance = value; }
		private const int DistanceBonus = 1000;
		private const int ReceiveBonusDistance = 100;
		private int distanceScore;  // 移動距離によるスコア
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
			if (disabledOperation == true) return;

			// 移動してないフラグ
			notMoving = beforePos == transform.position;

			if (canMove == true) {
				// 移動
				if (axis != Vector2.zero) {
					var vel = cameraTrans.forward * axis.y + cameraTrans.right * axis.x;
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
				if (notMoving == true) {
					canMove = true;
				}
			}

			RegenerateHp ();
			RegenerateStamina ();

			beforePos = transform.position;

			var beforeMax = maxXDistance;
			// 最高到達点を更新
			if (maxXDistance < (int)transform.position.x) maxXDistance = (int)transform.position.x;
			// 前のX最大から次のボーナスポイントまでの距離 < 前のX最大から移動した距離
			if (ReceiveBonusDistance - (beforeMax % ReceiveBonusDistance) < maxXDistance - beforeMax) SoundManager.Instance.Play ( SE.Hundred_Distance );

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
			// 移動中か待機中かによって次の回復までの時間を取得
			var i = (notMoving == true) ? StaminaRegenerateIntervalToWait : StaminaRegenerateInterval;
			await UniTask.Delay ( TimeSpan.FromSeconds ( i ) );
			canRegenerateStamina = true;
		}

		private void Respawn () {
			TakeDamageProc ( FallenDamage );
			// 画面の中心に戻す
			var pos = new Vector3 ( cameraTrans.position.x, 1, 10 );
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
					// アイテムスロットに重ねる
					if (inventory[n].Item == i.Item && inventory[n].Amount < maxVolume) {
						PickUpItem ( n, i );
						return;
					}
				}

				for (int n = 0; n < maxInventory; n++) {
					// 空のスロットに入れる
					if (inventory[n].Item == Enums.Items.Null) {
						PickUpItem ( n, i );
						return;
					}
				}

				// 拾えなかった場合
				fullInventory = true;
			}

			// 発射体に対する判定
			if (o.tag == "Projectile") {
				var p = o.GetComponent<ProjectileBase> ();
				if (p.Master != gameObject) {
					TakeDamageProc ( p.Damage );
				}
			}

			// KeyInfo表示フラグ
			if (o.tag == "Object" || o.tag == "NPC") {
				nearObject = true;
			}
		}

		public async void TakeDamageProc ( int damage ) {
			if (isInvincible == true) return;

			SoundManager.Instance.Play ( SE.Damage_Player );

			status.Hp -= damage;
			// HPが0以下ならゲームオーバー
			if (status.Hp <= 0) {
				isGameOver = true;
				Pause.Instance.TogglePause ( false );
				GameController.Instance.ShowGameOverScreen ();
			}

			if (damage >= InvincibleDamage) {
				// 一定時間無敵に
				isInvincible = true;
				InvincibleFlashing ();
				await UniTask.Delay ( TimeSpan.FromSeconds ( InvincibleInterval ) );
				isInvincible = false;
			}
		}

		// 点滅する
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
			takeItem = true;
			inventory[num].Item = item.Item;
			inventory[num].Amount++;
			Destroy ( item.gameObject );
			inventoryUI.UpdateInventoryUI ( this );
		}

		private void UpdateScore () {
			// 進んだ距離
			distanceScore = maxXDistance * 10;
			// 100ごとに+1000
			distanceScore += maxXDistance / ReceiveBonusDistance * DistanceBonus;

			status.Score = distanceScore;
			status.Score += killScore;
			status.Score += craftScore;
		}

		// ---------------入力系---------------------------
		public void OnMove ( InputValue value ) {
			if (disabledOperation == true) return;
			axis = value.Get<Vector2> ();
		}

		public void OnInteract () {
			if (disabledOperation == true) return;
			Interact ( status.AttackPower, ObjectType.None );
		}

		public async void Interact ( int attackPower, ObjectType criticalTarget ) {
			if (canInteract == false) return;
			if (status.Stamina < interactCost) return;

			Physics.Raycast ( transform.position, transform.forward, out RaycastHit r, 1.5f );
			bool attacked = false;
			if (r.collider == null) return;

			// 適正ツールでないならプレイヤーの攻撃力を適用
			if (r.collider.GetComponent<Status> ()?.ObjectType != criticalTarget || criticalTarget == ObjectType.None) {
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
				// 攻撃した時の挙動
				status.Stamina -= interactCost;
				canInteract = false;
				await UniTask.Delay ( TimeSpan.FromSeconds ( interactInterval ) );
				canInteract = true;
			}
		}

		public async void OnDash () {
			if (disabledOperation == true) return;
			if (canDash == false) return;
			if (canMove == false) return;
			if (isGameOver == true) return;
			if (status.Stamina < DashCost) return;

			SoundManager.Instance.Play ( SE.Dash );

			// 前方向に移動を加える
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
			if (disabledOperation == true) return;
			var v = (int)value.Get<float> ();
			currentSelectOnInventory += v;
			if (v != 0) SoundManager.Instance.Play ( SE.Inventory_Cursor );
			// 配列で使う値なので最大値は-1しておく
			currentSelectOnInventory = UIFunctions.RevisionValue ( currentSelectOnInventory, maxInventory - 1, UIFunctions.RevisionMode.Limit );
			inventoryUI.UpdateCursorUI ( this );
		}

		public void OnTrashItem () {
			if (disabledOperation == true) return;
			if (inventory[currentSelectOnInventory].Item == Enums.Items.Null) return;

			inventory[currentSelectOnInventory].Amount--;
			// アイテムを地面に落とす
			var l = ItemList.Instance.ItemNameList.ToList ();
			var i = l.FindIndex ( x => x == inventory[currentSelectOnInventory].Item.ToString () );
			Instantiate ( ItemList.Instance.Objects[i], transform.position + transform.forward * 1.5f, Quaternion.identity );
			inventoryUI.UpdateInventoryUI ( this );
		}

		public async void OnUseItem () {
			if (disabledOperation == true) return;
			if (inventory[currentSelectOnInventory].Item == Enums.Items.Null) return;
			if (canUsing == false) return;

			if (itemFunctions.Exec ( inventory[currentSelectOnInventory].Item ) == true) {
				inventory[currentSelectOnInventory].Amount--;
			}

			inventoryUI.UpdateInventoryUI ( this );

			canUsing = false;
			await UniTask.Delay ( TimeSpan.FromSeconds ( UseInterval ) );
			canUsing = true;
		}

		public void OnCraft () {
			if (disabledOperation == true) return;
			if (Craft.CanBeCrafting ( inventory, currentSelectOnRecipe )) {
				SoundManager.Instance.Play ( SE.Craft );

				// レシピを取得して成果物をドロップ
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
							inventory[m].Amount -= target.MaterialAmountList[m];
							break;
						}
					}
				}
				inventoryUI.UpdateInventoryUI ( this );
			}
		}

		public void OnCraftDetail () {
			if (disabledOperation == true) return;

			SoundManager.Instance.Play ( SE.Craft_Detail );
			craftWindow.Detail.SetActive ( !craftWindow.Detail.activeSelf );
		}

		public void OnChooseCraft ( InputValue value ) {
			if (disabledOperation == true) return;

			var v = (int)value.Get<float> ();
			currentSelectOnRecipe += v;
			if (v != 0) SoundManager.Instance.Play ( SE.Craft_Cursor );
			currentSelectOnRecipe = UIFunctions.RevisionValue ( currentSelectOnRecipe, Craft.MaxRecipeNumber, UIFunctions.RevisionMode.Loop );
			craftWindow.UpdateCraftUI ( this );
		}

		public void OnPause () {
			if (isGameOver == true) return;

			disabledOperation = Pause.Instance.TogglePause ();
			axis = Vector2.zero;
		}
	}
}