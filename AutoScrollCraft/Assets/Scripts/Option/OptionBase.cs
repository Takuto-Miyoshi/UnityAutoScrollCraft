using UnityEngine;

namespace AutoScrollCraft.Option {
	public class OptionBase : MonoBehaviour {
		private bool operatable = false;
		public virtual bool Operatable { get => operatable; set => operatable = value; }

		/// <summary>
		/// 軸による挙動
		/// </summary>
		public virtual void AxisAction ( int axis ) {
			if (Operatable == false) return;
		}

		/// <summary>
		/// 呼ばれたときの挙動
		/// </summary>
		public virtual void CallAction () {
			if (Operatable == false) return;
		}
	}
}
