using AutoScrollCraft.UI;
using ExtentionMethod;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AutoScrollCraft.Option {
	public class Volume : OptionBase {
		private enum Type {
			Master,
			BGM,
			SE,
		};
		[SerializeField] private Type type;
		[SerializeField] private float[] level;
		private int current;
		[SerializeField] private Image background;
		[SerializeField] private Image pin;
		private const int DefaultLevel = 6;
		[SerializeField] private AudioMixer mixer;

		private void Start () {
			// セーブデータから段階を読み込む
			current = PlayerPrefs.GetInt ( type.ToString (), DefaultLevel );
			mixer.SetFloat ( type.ToString (), level[current] );
			PositionAdjustment ();
		}

		public override void AxisAction ( int axis ) {
			base.AxisAction ( axis );

			current = UIFunctions.RevisionValue ( current + axis, level.Length - 1 );
			PositionAdjustment ();
			PlayerPrefs.SetInt ( type.ToString (), current );
			mixer.SetFloat ( type.ToString (), level[current] );
		}

		/// <summary>
		/// ピンの位置を調整
		/// </summary>
		private void PositionAdjustment () {
			// 背景と設定レベルから位置を計算
			var w = background.rectTransform.sizeDelta.x;
			var x = w / (level.Length - 1) * current;
			pin.rectTransform.localPosition = pin.rectTransform.localPosition.SetX ( x - w / 2 );
		}
	}
}
