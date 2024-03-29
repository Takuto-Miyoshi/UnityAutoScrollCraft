using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Cow : NPCBase {
		[SerializeField] private SearchObject attackArea;
		private bool attacking;
		protected void Update () {
			if (CanBeAction == true) {
				if (attacking == true && attackArea.Detected == true) {
					AI.Rush ( this, attackArea.Target.transform.position );
				}
				else {
					AI.Wandering ( this );
				}

				AfterActionDelay ();
			}
		}

		public override bool TakeDamageProc ( int damage, Vector3 damageFrom ) {
			var b = base.TakeDamageProc ( damage, damageFrom );
			AttackDelay ( 1.0f );
			return b;
		}

		private async void AttackDelay ( float waitTime ) {
			await UniTask.Delay ( System.TimeSpan.FromSeconds ( waitTime ) );
			attacking = true;
			CanBeAction = true;
		}

		protected override void OnCollisionEnter ( Collision collision ) {
			base.OnCollisionEnter ( collision );
			var o = collision.gameObject;
			// 攻撃する
			if (o.tag == "Player" && attacking == true) {
				o.GetComponent<Player> ().TakeDamageProc ( Status.AttackPower );
				attacking = false;
			}
		}
	}
}
