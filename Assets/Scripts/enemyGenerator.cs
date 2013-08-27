
using UnityEngine;
using System.Collections;

public class enemyGenerator : MonoBehaviour {

	// 前回の弾丸を発射してから経過した時間.
	private float progressTime = 0.0f;
	// enemyを生成してから、次のenemyを生成できるようになるまでの時間.
	public float EgenerateInterval;
	// enemyを生成できるかどうかのフラグの初期値.
	private bool EgenerateEnable = true;
	
	// サイドからenemyが攻めてくる時間かどうか.
	public bool sideAttackFlags = false;

	// どっちのサイドに向かっていくターンか.
	public bool leftOrNot = true;
	private int leftRightAppear;
	
	// enemyがサイドから攻めてくる時間帯.
	// 経過時間.
	private float timeRecord;
	// サイドからenemyが攻めてくる時間の長さ.
	private float sideAttackTime = 10.0f;
	
	// enemy生成時間のレンジ.
	private float fromEgene = 0.4f;
	private float toEgene = 1.9f;
	
	
	void Update () {
		
		if( EgenerateEnable == true ) {
			
			EgenerateEnable = false;
			// EnemyPrefabを取得.
			GameObject Eprefab = (GameObject) Resources.Load("EnemyPrefab");
			// enemyの出現座標をランダム指定.
			float enemyAppearX = Random.Range (-10.0f, 10.0f);
			float enemyAppearY = Random.Range (0.0f, 20.0f);
			// enemy出現位置をベクトル形式にする.
			Vector3 enemyAppearPos = new Vector3(enemyAppearX, enemyAppearY, 110.0f);
			// Vector3 enemyAppearRot = new Vector3(270, 0, 0);
			Instantiate(Eprefab, enemyAppearPos, Quaternion.Euler(270, 0, 0));
			
			// 次のenemy生成までの時間をランダム指定.
			EgenerateInterval = Random.Range ( fromEgene, toEgene);
			
			if ( toEgene > 0.8f ) {
			
				toEgene -= 0.01f;
			
			}
		
		}
		
		// enemyの生成間隔を指定する.
		// ↓正常に加算される。progressTimeの初期値がどんな桁数でも.
		progressTime += Time.deltaTime;
		// Debug.Log("progressTime: " + progressTime);
		if ( progressTime >= EgenerateInterval ) {
			
			progressTime = 0.0f;
			EgenerateEnable = true;
		
		}
		
		if ( sideAttackFlags == false ) {
			
			timeRecord += Time.deltaTime;
			
			if ( timeRecord > 5.0f ) {
			
				sideAttackFlags = true;
				leftRightAppear += 1;
				timeRecord = 0.0f;
				
			}
			
		} else {
			
			timeRecord += Time.deltaTime;
			
			if ( leftRightAppear % 2 == 1 ) {

				leftOrNot = true;
				
			} else {
				
				leftOrNot = false;
				
			}
			
			
			// 7.0fに戻す.
			if ( timeRecord > 10.0f ) {
			
				sideAttackFlags = false;
				timeRecord = 0.0f;
				
			}
			
		}

	}
}
