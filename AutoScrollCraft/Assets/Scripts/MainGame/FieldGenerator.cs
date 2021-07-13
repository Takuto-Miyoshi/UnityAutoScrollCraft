using System.Linq;
using ExtentionMethod;
using UnityEngine;

namespace AutoScrollCraft.World {
	public class FieldGenerator : MonoBehaviour {
		[SerializeField] private Terrain terrain;
		private UnityEngine.Camera mainCamera;
		[SerializeField] private GameObject[] fieldObjectList;
		[SerializeField] private GameObject[] npcList;
		private float basePosition;
		private const float DefaultSizeZ = 20.0f;
		private const float DefaultSizeX = 300.0f;
		private const float UpdatePositionX = 200.0f; // これだけ移動したら更新する
		private const float MovementAmount = 100.0f; // 移動させる量
		private const float SpawnRateLimit = 5.0f;  // スポーンレート(配置間隔)の限界
		private const float BaseSpawnRate = 15.0f;  // スポーンレートの初期値

		private void Awake () {
			// 地形の位置調整
			mainCamera = UnityEngine.Camera.main;
			terrain.terrainData.size.Set ( DefaultSizeX, 1, DefaultSizeZ );
			terrain.transform.position = Vector3.zero;
			// オブジェクトを配置する
			DeploymentObject ( 1, (int)DefaultSizeX, fieldObjectList );
			DeploymentObject ( 1, (int)DefaultSizeX, npcList, 5.0f, 15.0f );
		}

		private void FixedUpdate () {
			// カメラの場所に応じてベースの地形を移動させる
			if (mainCamera.transform.position.x > basePosition + UpdatePositionX) {
				// 移動させる
				var newX = basePosition + MovementAmount;
				terrain.transform.position = terrain.transform.position.SetX ( newX );
				basePosition = newX;

				// 足場のなくなったオブジェクトを消す
				FindObjectsOfType<GameObject> ().ToList ()
				.Where ( o => o.tag == "Object" || o.tag == "NPC" )
				.Where ( o => o.transform.position.x < basePosition )
				.ToList ()
				.ForEach ( x => Destroy ( x ) );

				// 移動させた先にフィールドオブジェクトを配置する
				var beginPosition = (int)(terrain.transform.position.x + UpdatePositionX);
				var endPosition = (int)(beginPosition + MovementAmount);
				DeploymentObject ( beginPosition, endPosition, fieldObjectList );
				// 進行度に応じてレートを上げ、NPCを配置する
				// 限界値を超えないようにする
				var rate = (BaseSpawnRate - basePosition / 1000 < SpawnRateLimit) ? SpawnRateLimit : BaseSpawnRate - basePosition / 1000;
				DeploymentObject ( beginPosition, endPosition, npcList, SpawnRateLimit, rate );
			}
		}

		/// <summary>
		/// オブジェクトを配置する
		/// </summary>
		/// <param name="begin">配置しはじめるX座標</param>
		/// <param name="end">ここまで配置する</param>
		/// <param name="objectList">配置するオブジェクトリスト</param>
		/// <param name="deploySpaceMin">配置間隔の最小値</param>
		/// <param name="deploySpaceMax">配置間隔の最大値</param>
		private void DeploymentObject ( int begin, int end, GameObject[] objectList, float deploySpaceMin = 1.0f, float deploySpaceMax = 4.0f ) {
			for (float x = begin; x < end; x += Random.Range ( deploySpaceMin, deploySpaceMax )) {
				var posZ = Random.Range ( 1, DefaultSizeZ - 1 );
				var pos = new Vector3 ( x, 0.5f, posZ );
				var placeObject = objectList[Random.Range ( 0, objectList.Length )];
				Instantiate ( placeObject, pos, Quaternion.identity );
			}
		}
	}
}
