using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Pig : NPCBase {
		protected override async void Update () {
			base.Update ();

			if (CanBeAction == true) {
				AI.Wandering ( this );

				CanBeAction = false;
				await UniTask.Delay ( TimeSpan.FromSeconds ( UpdateInterval ) );
				CanBeAction = true;
			}
		}

		public override bool TakeDamageProc ( int damage, Vector3 damageFrom ) {
			AI.Escape ( this );
			var b = base.TakeDamageProc ( damage, damageFrom );
			return b;
		}
	}
}
