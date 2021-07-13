using UnityEngine;

namespace AutoScrollCraft.Weapons {
	public class ProjectileBase : MonoBehaviour {
		[SerializeField] private float speed;
		[SerializeField] private int damage;
		public int Damage { get => damage; }
		private GameObject master;  // 発射した人
		public GameObject Master { get => master; set => master = value; }
		private const float HellLine = -10.0f;  // 落ちたとする高さ

		private void Awake () {
			// 射出速度を設定
			GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		}

		private void FixedUpdate () {
			if (transform.position.y < HellLine) Destroy ( gameObject );
		}

		private void OnCollisionEnter ( Collision collision ) {
			// 地面に落ちたら拾えるように変更
			var o = collision.gameObject;
			if (o.tag == "Terrain" || o.tag == "Object") {
				gameObject.tag = "DropItem";
			}

			// アクターに当たるとダメージを与える
			if (gameObject.tag == "Projectile") {
				if (o.tag == "Player" || o.tag == "NPC") {
					if (master != o) {
						// ダメージ処理等はダメージを受けた側で行う
						Destroy ( gameObject );
					}
				}
			}
		}
	}
}
