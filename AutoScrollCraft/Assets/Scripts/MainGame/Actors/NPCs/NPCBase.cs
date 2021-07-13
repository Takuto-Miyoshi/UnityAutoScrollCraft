using AutoScrollCraft.Enums;
using AutoScrollCraft.Items;
using AutoScrollCraft.Sound;
using AutoScrollCraft.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AutoScrollCraft.Actors.AI {
	public class NPCBase : MonoBehaviour {
		private NavMeshAgent nav;
		public virtual NavMeshAgent Nav { get => nav; set => nav = value; }
		private Status status;
		public virtual Status Status { get => status; set => status = value; }
		// AI
		private AIPattern ai;
		public virtual AIPattern AI { get => ai; set => ai = value; }
		[SerializeField] private float speed;
		public virtual float Speed { get => speed; set => speed = value; }
		private bool canBeAction = true;
		public virtual bool CanBeAction { get => canBeAction; set => canBeAction = value; }
		[SerializeField] private float updateInterval;
		public virtual float UpdateInterval { get => updateInterval; set => updateInterval = value; }
		// アイテム
		[SerializeField] private GameObject dropItem;
		public virtual GameObject DropItem { get => dropItem; set => dropItem = value; }
		[SerializeField] private int dropAmountMax;
		public virtual int DropAmountMax { get => dropAmountMax; set => dropAmountMax = value; }
		[SerializeField] private int dropAmountMin;
		public virtual int DropAmountMin { get => dropAmountMin; set => dropAmountMin = value; }
		// ダメージ
		private Vector3 damageSource;
		public virtual Vector3 DamageSource { get => damageSource; set => damageSource = value; }

		protected virtual void Awake () {
			ai = AIPattern.Instance;
		}

		// Start is called before the first frame update
		protected virtual void Start () {
			Nav = GetComponent<NavMeshAgent> ();
			Status = GetComponent<Status> ();
		}

		/// <summary>
		/// 行動後に呼び出す
		/// </summary>
		protected async void AfterActionDelay () {
			canBeAction = false;
			await UniTask.Delay ( System.TimeSpan.FromSeconds ( updateInterval ) );
			canBeAction = true;
		}

		/// <summary>
		/// 死んでいるかを判定する
		/// 死んでいる場合はアイテムドロップ、DeathProcessing、Destroyの順番で実行
		/// </summary>
		protected void IsDead () {
			if (status.Hp > 0) return;

			if (DropItem != null) {
				ItemList.Instance.Drop ( transform.position, DropItem, Random.Range ( DropAmountMin, DropAmountMax + 1 ) );
			}

			Destroy ( gameObject );
		}

		/// <summary>
		/// ダメージを受けたときに呼び出される。
		/// </summary>
		/// <param name="damage">受けるダメージ量</param>
		/// <param name="damageFrom">ダメージを受けた場所</param>
		/// <returns>HPが０以下であればtrue</returns>
		public virtual bool TakeDamageProc ( int damage, Vector3 damageFrom ) {
			status.Hp -= damage;
			damageSource = damageFrom;
			SoundManager.Instance.Play ( SE.Damage_NPC );
			IsDead ();
			return status.Hp <= 0;
		}

		protected virtual void OnCollisionEnter ( Collision collision ) {
			var o = collision.gameObject;
			// 発射体に対する判定
			if (o.tag == "Projectile") {
				var p = o.GetComponent<ProjectileBase> ();
				if (p.Master != gameObject) {
					TakeDamageProc ( p.Damage, o.transform.position );
				}
			}
		}

		protected virtual void OnBecameInvisible () {
			Destroy ( gameObject );
		}
	}
}