using System.Collections;
using System.Collections.Generic;
using AutoScrollCraft.Actors;
using UnityEngine;

namespace AutoScrollCraft.Weapons.Projectile {
	public class ProjectileBase : MonoBehaviour {
		[SerializeField] float speed;
		[SerializeField] int damage;
		public int Damage { get => damage; }
		private GameObject master;  // 発射した人
		public GameObject Master { get => master; set => master = value; }

		private void Start () {
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
			if (o.tag == "Terrain") {
				gameObject.tag = "DropItem";
			}

			if (gameObject.tag == "Projectile") {
				if (o.tag == "Player" || o.tag == "Object") {
					if (master != o) {
						o.GetComponent<Status> ().Hp -= damage;
						Destroy ( gameObject );
					}
				}
			}
		}
	}
}
