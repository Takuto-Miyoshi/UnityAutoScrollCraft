using System.Collections;
using System.Collections.Generic;
using AutoScrollCraft.Items;
using AutoScrollCraft.Weapons.Projectile;
using UnityEngine;
using UnityEngine.AI;

namespace AutoScrollCraft.Actors.AI {
	public class NPCBase : MonoBehaviour {
		NavMeshAgent nav;
		public virtual NavMeshAgent Nav { get => nav; set => nav = value; }
		Status status;
		public virtual Status Status { get => status; set => status = value; }
		// AI
		AIPattern aI;
		public virtual AIPattern AI { get => aI; set => aI = value; }
		[SerializeField] float speed;
		public virtual float Speed { get => speed; set => speed = value; }
		bool canBeAction;
		public virtual bool CanBeAction { get => canBeAction; }
		[SerializeField] float updateInterval;
		public virtual float UpdateInterval { get => updateInterval; set => updateInterval = value; }
		float updateTimer;
		public virtual float UpdateTimer { get => updateTimer; set => updateTimer = value; }
		// アイテム
		[SerializeField] GameObject dropItem;
		public virtual GameObject DropItem { get => dropItem; set => dropItem = value; }
		[SerializeField] int dropAmountMax;
		public virtual int DropAmountMax { get => dropAmountMax; set => dropAmountMax = value; }
		[SerializeField] int dropAmountMin;
		public virtual int DropAmountMin { get => dropAmountMin; set => dropAmountMin = value; }
		// ダメージ
		Vector3 damageSource;
		public virtual Vector3 DamageSource { get => damageSource; set => damageSource = value; }

		protected virtual void Awake () {
			aI = AIPattern.Instance;
		}

		// Start is called before the first frame update
		protected virtual void Start () {
			Nav = GetComponent<NavMeshAgent> ();
			Status = GetComponent<Status> ();
			UpdateTimer = UpdateInterval;
		}

		/// <summary>
		/// overrideした時にbaseも呼び出す
		/// </summary>
		protected virtual void Update () {
			updateTimer -= Time.deltaTime;

			canBeAction = updateTimer < 0;
		}

		/// <summary>
		/// 死んでいるかを判定する
		/// 死んでいる場合はアイテムドロップ、DeathProcessing、Destroyの順番で実行
		/// </summary>
		protected void IsDead ( float destroyTime = 0.01f ) {
			if (status.Hp > 0) return;

			if (DropItem != null) {
				ItemList.Instance.Drop ( transform.position, DropItem, Random.Range ( DropAmountMin, DropAmountMax + 1 ) );
			}

			DeathProc ();

			Destroy ( gameObject, destroyTime );
		}

		/// <summary>
		/// IsDeadで呼び出される。
		/// </summary>
		protected virtual void DeathProc () { }

		/// <summary>
		/// ダメージを受けたときに呼び出される。
		/// </summary>
		public virtual void TakeDamageProc () { }

		protected virtual void OnCollisionEnter ( Collision collision ) {
			var o = collision.gameObject;
			if (o.tag == "Projectile") {
				status.Hp -= o.GetComponent<ProjectileBase> ().Damage;
				damageSource = o.transform.position;
				TakeDamageProc ();
				Destroy ( o );
			}
		}
	}
}