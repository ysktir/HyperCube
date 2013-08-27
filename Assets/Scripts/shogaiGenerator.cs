using UnityEngine;
using System.Collections;

public class shogaiGenerator : MonoBehaviour {
	
	private bool generatAble = false;
	private GameObject[] shogaiPrefab;
	private float generateInterval;
	private float progressTime;
	
	
	void Start () {
		
		shogaiPrefab = new GameObject[3];
		
		shogaiPrefab[0] = (GameObject) Resources.Load("shogaiCube");
		shogaiPrefab[1] = (GameObject) Resources.Load("shogaiSphere");
		shogaiPrefab[2] = (GameObject) Resources.Load("shogaiCylinder");
		
	}
	

	void Update () {
	
		if( generatAble == true ) {
			
			generatAble = false;
			
			int prefabNum = Random.Range ( 0, 3 );
			
			// 障害物の出現座標をランダム指定.
			float shogaiAppearX = Random.Range (-8.5f, 8.5f);
			float shogaiAppearY = Random.Range (1.0f, 19.0f);
			
			// 障害物の出現位置をベクトル形式にする.
			Vector3 shogaiAppearPos = new Vector3(shogaiAppearX, shogaiAppearY, 130.0f);
			Instantiate( shogaiPrefab[prefabNum], shogaiAppearPos, Quaternion.identity );
			
			// 次の障害物生成までの時間をランダム指定.
			generateInterval = Random.Range (1.0f, 3.6f);
			
		}

		// 障害物の生成間隔を指定する.
		progressTime += Time.deltaTime;

		if ( progressTime >= generateInterval ) {

			progressTime = 0.0f;
			generatAble = true;

		}

	}
}