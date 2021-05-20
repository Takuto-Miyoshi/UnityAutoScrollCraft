using System.Collections;
using System.Collections.Generic;
using AutoScrollCraft.Weapons.Projectile;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class AIPattern : Singleton<AIPattern> {

		/// <summary>
		/// ランダムな場所を目的地にする
		/// </summary>
		/// <param name="npc">動かす対象</param>
		/// <param name="maxRange">Random.Rangeの値</param>
		public void Wandering ( NPCBase npc, float maxRange = 5.0f ) {
			npc.Nav.speed = npc.Speed;
			var p = npc.transform.position;
			p.x += Random.Range ( -maxRange, maxRange );
			p.z += Random.Range ( -maxRange, maxRange );
			npc.Nav.SetDestination ( p );
			npc.UpdateTimer = npc.UpdateInterval;
		}

		/// <summary>
		/// 攻撃された逆方向へ向かう
		/// </summary>
		/// <param name="npc">動かす対象</param>
		/// <param name="distance">逃げる距離</param>
		public void Escape ( NPCBase npc, float distance = 5.0f ) {
			npc.Nav.speed = npc.Speed * 2;
			var d = npc.transform.position - npc.DamageSource;
			var p = npc.transform.position + d * distance;
			p.y = npc.transform.position.y;
			npc.Nav.SetDestination ( p );
			npc.UpdateTimer = npc.UpdateInterval;
		}

		/// <summary>
		/// オブジェクトを追いかける
		/// </summary>
		/// <param name="npc">動かす対象</param>
		/// <param name="target">追いかける対象</param>
		public void Chase ( NPCBase npc, Vector3 target ) {
			npc.Nav.speed = npc.Speed;
			npc.Nav.SetDestination ( target );
			npc.UpdateTimer = npc.UpdateInterval;
		}

		/// <summary>
		/// オブジェクトに突進する
		/// </summary>
		/// <param name="npc">動かす対象</param>
		/// <param name="target">攻撃する対象</param>
		/// <param name="dashPower">ダッシュの力</param>
		public void Rush ( NPCBase npc, Vector3 target, float dashPower = 30.0f ) {
			npc.Nav.speed = npc.Speed;
			var d = target - npc.transform.position;
			npc.transform.rotation = Quaternion.LookRotation ( d, Vector3.up );
			npc.GetComponent<Rigidbody> ().AddForce ( d.normalized * dashPower, ForceMode.Impulse );
			npc.Nav.SetDestination ( target );
			npc.UpdateTimer = npc.UpdateInterval;
		}

		/// <summary>
		/// 飛翔体を飛ばす
		/// </summary>
		/// <param name="npc">発射するNPC</param>
		/// <param name="target">攻撃する対象</param>
		/// <param name="projectile">発射するもの</param>
		public void ShootProjectile ( NPCBase npc, Vector3 target, GameObject projectile ) {
			var d = target - npc.transform.position;
			npc.transform.rotation = Quaternion.LookRotation ( d, Vector3.up );
			var p = npc.transform.position;
			p += npc.transform.forward;
			var o = Instantiate ( projectile, p, npc.transform.rotation );
			o.GetComponent<ProjectileBase> ().Master = npc.gameObject;
			npc.UpdateTimer = npc.UpdateInterval;
		}
	}
}