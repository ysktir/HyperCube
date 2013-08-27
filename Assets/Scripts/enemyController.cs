
using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour {

	// 移動速度.
	private float movingSpeed;
	// 弾丸の発射速度.		
	private float bulletSpeed;
	
	// 取得したプレハブを格納するための変数を宣言.
	private GameObject bulletPrefab;
	// enemyが弾丸を発射し始めるposition.
	public float shotRange;
	// enemyが弾丸の発射を終了するpositionを指定.
	public float closeShotRange = 23.0f;	
	
	// 前回の弾丸を発射してから経過した時間.
	private float progressTime = 0.0f;
	// 弾丸を発射してから、次の弾丸を発射できるようになるまでの時間.
	public float shotInterval = 0.19f;
	// 弾丸を発射できるかどうかのフラグ.
	private bool bulletEnable = true;
	
	// 1度に連射できる弾丸の制限数.
	public int multiShotLimit;
	// 現在何発連射したか.
	private int shotNum = 0;
	// 次に連射できるようになるまでのインターバル.
	public float multiShotInterval = 1.2f;
	// 連射できる状態か否か.
	private bool multiBulletEnable = true;
	
	// 前回の連射を終えてから経過した時間.
	private float multiProgressTime = 0.0f;
	// 爆発用のprefabを格納するための変数宣言.
	private GameObject simpleDetonator;

	
	// playerのrotationを元に戻すかどうか.
	private bool enemyRotFlags = false;
	private float enemyRotTime;
	
	private bool enemySideFlags = false;
	private float enemySideTime;
	
	private bool sideAttackFlags;
	private bool leftOrNot;
	
	// enemyGeneratorのオブジェクトのスクリプトを取得するようの変数.
	public enemyGenerator enemyGenScript;
	
	// 取得したItemプレハブを格納するための変数を宣言.
	public GameObject itemPrefab_1;
	public GameObject itemPrefab_2;
	public GameObject itemPrefab_3;
	
	private int itemAppear;
	private int itemType;
	
	private scoreNum scoreNumScript;
	
	private bool colliderOrNot = false;
	
	private GameObject playerObject;
	

	void Start () {	
		
		enemyGenScript = GameObject.Find("EnemyGenerator").GetComponent<enemyGenerator>();
		leftOrNot = enemyGenScript.leftOrNot;
		sideAttackFlags = enemyGenScript.sideAttackFlags;
		
		// enemyの移動速度をランダム指定.
		movingSpeed = Random.Range (15.0f, 60.0f);
		// enemyが弾丸を発射し始める位置をランダム指定.
		shotRange = Random.Range (35.0f, 55.0f);
		// enemyの場合は弾丸スピードもランダムにする.
		bulletSpeed = Random.Range (45.0f, 65.0f);
		// enemyが一度に連射できる制限数をランダム指定.
		multiShotLimit = Random.Range (2, 5);
		
		// 弾丸プレハブをロード.
		bulletPrefab = (GameObject) Resources.Load("EbulletPrefab");
		// 爆発プレハブをロード.
		simpleDetonator = (GameObject) Resources.Load("MyDetonatorSimple");
		// アイテムプレハブをロード.
		itemPrefab_1 = (GameObject) Resources.Load("Item_1");
		itemPrefab_2 = (GameObject) Resources.Load("Item_2");
		itemPrefab_3 = (GameObject) Resources.Load("Item_3");
		
		// アイテムが出現するか否か.
		itemAppear = Random.Range (1, 3);
		// 出現するアイテムの種類.
		itemType = Random.Range (1, 15);
		
	}

	void Update () {
		
		// bulletにぶつかったらtrueになるので、ここでスコアを加算してから自分自身を破棄する.
		if ( colliderOrNot == true ) {

			scoreNumScript = GameObject.Find("scoreNum").GetComponent<scoreNum>();
			scoreNumScript.score += 100;
			colliderOrNot = false;
			Destroy(gameObject);
			
		}
		
		// enemyがこちらに向かってくる.
		transform.Translate(Vector3.up * movingSpeed * Time.deltaTime);
	
		// playerを通り過ぎたenemyを破棄する.
		if (transform.position.z < -5.0) {
			
			Destroy(gameObject);
			
		}
		
		// playerオブジェクトを取得.
		playerObject = GameObject.Find("Player");
		
		// playerとの距離が攻撃範囲まで近づいたかどうか判定.
		float distancePlayer = transform.position.z - playerObject.transform.position.z;
		
		// まずはplayerが射程距離に入ってるかどうか。それが大前提.
		if ( distancePlayer <= shotRange && distancePlayer >= closeShotRange ) {
			
			shotEnemyBullet();

		}
			
		// 弾の発射間隔を調節する。大体秒間5発ぐらい.
		// 正常に加算される。progressTimeの初期値がどんな桁数でも.
		progressTime += Time.deltaTime;

		if ( progressTime >= shotInterval ) {

			// 経過時間をリセット.
			progressTime = 0.0f;
			bulletEnable = true;

		}
		
		enemyRotationReset();
		
		enemySideAttack();

		if ( sideAttackFlags ) {
			
			if ( leftOrNot && transform.position.x > -9.2f ) {
				
				transform.Translate( Vector3.left * movingSpeed * Time.deltaTime * 0.2f );
			
			} else if ( leftOrNot == false && transform.position.x < 9.2f ) {
				
				transform.Translate( Vector3.right * movingSpeed * Time.deltaTime * 0.2f );
				
			}

		} 
		
	}
	
	
	void OnTriggerEnter (Collider other) {
		
		if ( other.gameObject.tag == "Sbullet" ) {
		
			Destroy(gameObject);
			// 爆発用インスタンスを生成.
			Instantiate( simpleDetonator, transform.position, Quaternion.identity );
			
			if ( itemAppear == 1 ) {
				
				if ( itemType >= 1 && itemType <= 4 ) {
					
					// 弾丸追加用のアイテム.
					Instantiate( itemPrefab_3, transform.position, Quaternion.identity );
					
				} else if ( itemType >= 5 && itemType <= 11 ) {
					
					// 1000ポイントのアイテム.
					Instantiate( itemPrefab_1, transform.position, Quaternion.identity );
					
				} else {
					
					// HP回復.
					Instantiate( itemPrefab_2, transform.position, Quaternion.identity );
					
				}
	
			}
			
		}

		
		if ( other.gameObject.tag == "Bullet" ) {
			
			// bullet自体も破棄する.
			Destroy(other.gameObject);

			// 爆発用インスタンスを生成.
			Instantiate( simpleDetonator, transform.position, Quaternion.identity );
			
			colliderOrNot = true;
			
			if ( itemAppear == 1 ) {
				
				if ( itemType >= 1 && itemType <= 6 ) {
					
					// 弾丸追加用のアイテム.
					Instantiate( itemPrefab_3, transform.position, Quaternion.identity );
					
				} else if ( itemType >= 7 && itemType <= 12 ) {
					
					// 300ポイントのアイテム.
					Instantiate( itemPrefab_1, transform.position, Quaternion.identity );
					
				} else {
					
					// 1000ポイントのアイテム.
					Instantiate( itemPrefab_2, transform.position, Quaternion.identity );
					
				}
	
			}
			
		}
		
	}
	
	
	void shotEnemyBullet() {
		
		// 弾丸を連射可能な状態か否か.
		if ( multiBulletEnable == true ) {
			
			// 次の弾丸を発射可能な状態か否か.
			if ( bulletEnable == true ) {
				
				bulletEnable = false;
				// 弾丸インスタンスを作成　quaternion.identityは親オブジェクトと同じ方向って意味この場合はおそらくスクリプト適用してるenemyインスタンスのはず.
				GameObject Ibullet = (GameObject) Instantiate( bulletPrefab, transform.position, Quaternion.identity );
				// 自身のpositionからplayerのpositionへの方角を取得.
				Vector3 direction = ( transform.position - playerObject.transform.position).normalized;
				// playerの方角に向けて弾丸を発射.
				Ibullet.rigidbody.velocity = direction * bulletSpeed * -1;
				// 連射数を加算.
				shotNum += 1;
				
				// 連射回数が一定値を越えたら、弾丸が発射できない状態に移行する.
				if ( shotNum == multiShotLimit ) {

					multiBulletEnable = false;
					// 連射数をリセット.
					shotNum = 0;

				}
			}
		
		// 弾丸が発射可能じゃない状態の時の処理.
		} else {
			
			// 連射が終了してからの経過時間.
			multiProgressTime += Time.deltaTime;
			// 連射が終了してから一定時間経過したら、連射可能な状態へ移行.
			if ( multiProgressTime >= multiShotInterval ) {
				
				// 経過時間をリセット.
				multiProgressTime = 0.0f;
				// 連射可能な状態へ.
				multiBulletEnable = true;
			
			}
		}
		
	}
	
	
	void enemyRotationReset() {
		
		// enemyの傾きを取得.
		// 一定以上の傾きになってる時間が一定を越えたら、enemyのrotationの値を修正する.
		if ( transform.rotation.z > 0.709f || transform.rotation.z < 0.705f ) {
			
			enemyRotTime += Time.deltaTime;

			
			if ( enemyRotTime > 0.3f ) {

				enemyRotFlags = true;
				rigidbody.angularVelocity = Vector3.zero;
				
			}
		
		} else if ( transform.rotation.z < 0.7072f && transform.rotation.z > 0.7070f ) {
			
			enemyRotFlags = false;
			enemyRotTime = 0.0f;
			
		}
		
		if ( enemyRotFlags && enemySideFlags == false ) {
			
			rotationSlerp(270, 0, 0, 0.1f);
			
			// どの角度に修正したいかを指定.
			// Quaternion toRot = Quaternion.Euler(270, 0, 0);
			// slarpで徐々にターゲット角度へ修正していく.
			// transform.rotation = Quaternion.Slerp (transform.rotation, toRot, 0.1f);

		}
		
	}
	
	void enemySideAttack() {
		
		// enemyが両サイドに一定時間いると横向きになる.
		// -9.32fと9.31fだった.
		if ( transform.position.x < -7.5f || transform.position.x > 7.5f ) {
			
			enemySideTime += Time.deltaTime;
			
			if ( enemySideTime > 0.4f ) {
			
				enemySideFlags = true;
			
			}
			
		} else {
			
			// enemySideFlags = false;
			enemySideTime = 0.0f;
			
		}
		
		if ( enemySideFlags && transform.position.x < -7.0f ) {
			
			rotationSlerp(0, 90, -90, 0.1f);
			
			// どの角度に修正したいかを指定.
			// Quaternion toSideRot = Quaternion.Euler(0, 90, -90);
			// slarpで徐々にターゲット角度へ修正していく.
			// transform.rotation = Quaternion.Slerp (transform.rotation, toSideRot, 0.1f);
			
			// ここで左に寄せれば良い。
			if ( transform.rotation.z < -0.4f && transform.position.x > -10.5f ) {

				transform.Translate(0, 0, -0.1f);
				
			}
			
		} else if ( enemySideFlags && transform.position.x > 7.0f ) {
			
			rotationSlerp(0, 270, -270, -0.1f);
			
			// どの角度に修正したいかを指定.
			// Quaternion toSideRot = Quaternion.Euler(0, 270, -270);
			// slarpで徐々にターゲット角度へ修正していく.
			// transform.rotation = Quaternion.Slerp (transform.rotation, toSideRot, 0.1f);
			
			if ( transform.rotation.z > 0.4f && transform.position.x < 10.5f ) {
				
				transform.Translate(0, 0, -0.1f);

			}
			
		}
		
	}
	
	void rotationSlerp( int eulerX, int eulerY, int eulerZ, float rotationSpeed ) {
		
		// どの角度に修正したいかを指定.
		Quaternion toSideRot = Quaternion.Euler(eulerX, eulerY, eulerZ);
		// slarpで徐々にターゲット角度へ修正していく.
		transform.rotation = Quaternion.Slerp (transform.rotation, toSideRot, rotationSpeed);
		
	}
	

}
