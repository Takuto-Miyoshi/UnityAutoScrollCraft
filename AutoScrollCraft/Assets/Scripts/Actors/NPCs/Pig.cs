using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoScrollCraft.Actors.AI {
	public class Pig : NPCBase {
		protected override void Update () {
			base.Update ();

			if (CanBeAction == true) AI.Wandering ( this );

			IsDead ();
		}

		public override void TakeDamageProc () {
			AI.Escape ( this );
		}
	}
}
