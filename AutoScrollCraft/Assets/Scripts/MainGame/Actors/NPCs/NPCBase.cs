using AutoScrollCraft.Items;
using AutoScrollCraft.Weapons;
using UnityEngine;
using UnityEngine.AI;

namespace AutoScrollCraft.Actors.AI {
	public class NPCBase : MonoBehaviour {
		private NavMeshAgent nav;
		public virtual NavMeshAgent Nav { get => nav; set => nav = value; }
		private Status status;
		public virtual Status Status { get => status; set => status = value; }
		// AI
		private AIPattern aI;
		public virtual AIPattern AI { get => aI; set => aI = value; }
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
			aI = AIPattern.Instance;
		}

		// Start is called before the first frame update
		protected virtual void Start () {
			Nav = GetComponent<NavMeshAgent> ();
			Status = GetComponent<Status> ();
		}

		/// <summary>
		/// overrideした時にbaseも呼び出す
		/// </summary>
		protected virtual void Update () {

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

			DeathProc ();

			Destroy ( gameObject );
		}

		/// <summary>
		/// IsDeadで呼び出される。
		/// </summary>
		protected virtual void DeathProc () { }

		/// <summary>
		/// ダメージを受けたときに呼び出される。
		/// </summary>
		/// <param name="damage">受けるダメージ量</param>
		/// <param name="damageFrom">ダメージを受けた場所</param>
		/// <returns>HPが０以下であればtrue</returns>
		public virtual bool TakeDamageProc ( int damage, Vector3 damageFrom ) {
			status.Hp -= damage;
			damageSource = damageFrom;
			IsDead ();
			return status.Hp <= 0;
		}

		protected virtual void OnCollisionEnter ( Collision collision ) {
			var o = collision.gameObject;
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