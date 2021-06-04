using UnityEngine;

namespace AutoScrollCraft.Weapons {
	public class ProjectileBase : MonoBehaviour {
		[SerializeField] private float speed;
		[SerializeField] private int damage;
		public int Damage { get => damage; }
		private GameObject master;  // 発射した人
		public GameObject Master { get => master; set => master = value; }

		private void Awake () {
			GetComponent<Rigidbody> ().velocity = transform.forward * speed;
		}

		private void Update () {
			if (transform.position.y < -50) {
				Destroy ( gameObject );
			}
		}

		// 地面に落ちたら拾えるようにする
		private void OnCollisionEnter ( Collision collision ) {
			var o = collision.gameObject;
			if (o.tag == "Terrain" || o.tag == "Object") {
				gameObject.tag = "DropItem";
			}

			if (gameObject.tag == "Projectile") {
				if (o.tag == "Player" || o.tag == "NPC") {
					if (master != o) {
						Destroy ( gameObject );
					}
				}
			}
		}
	}
}
