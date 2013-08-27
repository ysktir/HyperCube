
using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	// 移動速度.
	public float movingSpeed = 28.0f;
	// 弾丸の発射速度.	
	public float bulletSpeed = 100.0f;	
	// 取得したプレハブを格納するための変数を宣言.
	public GameObject Bprefab;
	public GameObject superBprefab;
	
	// 前回の弾丸を発射してから経過した時間.
	private float progressTime = 0.0f;
	// 弾丸を発射してから、次の弾丸を発射できるようになるまでの時間.
	public float shotInterval = 0.11f;
	// 弾丸を発射できるかどうかのフラグ.
	private bool bulletEnable = true;
	// 弾丸数.
	private int bulletNum = 1;
	// 弾丸が飛ぶ方向.
	private Vector3 direction = new Vector3(0, 0, 1);
	
	// playerのrotationを元に戻すかどうか.
	private bool playerRotFlags = false;
	private float playerRotTime;
	
	// playerが壁際にいるかどうか.
	private bool playerSideFlags = false;
	private float playerSideTime;
	
	// リソースを紐付ける変数をインスペクター上に用意.
	public AudioClip audioClip_1;
	public AudioClip audioClip_2;
	public AudioClip audioClip_3;
	// オーディオリソースのコンポーネント格納用の変数を取得.
	private AudioSource audioSource;
	
	// playerが壁際で回転する際の音.
	public AudioClip audioClip_4;
	// energyゲージがmaxになった時の音.
	public AudioClip audioClip_5;
	
	// 弾丸をゲットした時のflag.
	private bool addBullet = false;
	
	// アイテムをゲットした時のflag.
	private bool item1_get = false;
	private bool item2_get = false;
	
	// scoreのスクリプトのコンポーネントを格納する変数.
	private scoreNum scoreNumScript;
	// HPゲージのスクリプトのコンポーネントを格納する変数.
	private MonoHealthbar healthBar;	
	private MonoHealthbar EnergyBar;
	
	// 被弾したか否か.
	private bool addDamage = false;
	private int addDamageTurn = 0;
	private int damageIncriment = 1;
	
	// player爆発用のprefabを格納するための変数宣言.
	public GameObject playerDestroy;
	
	private textureControllerBottom textureBottomScript;
	private textureControllerTop textureTopScript;
	private textureControllerSide textureLeftScript;
	private textureControllerSide textureRightScript;
	
	// gameover時にポイントライトを消す方法.
	private GameObject pointLightBase;
	private GameObject pointLightPlus;
	
	// gameover時に表示するテキストプレハブ格納用変数.
	public GameObject gameOverPrefab;

	// 爆発用のprefabを格納するための変数宣言.
	public GameObject bbSimpleDetonator;
	
	// hpを回復する.
	private bool hpRecoveryBool;
	private int hpIncriment;
	
	// energyゲージに関する変数.
	private bool energyMaxBool = false;
	private int energyIncriment = 1;
	private int energyDecrement = 1;
	
	// superBulletの発射中かどうか.
	private bool whileFiring;
	
	// enemybulletの攻撃力.
	public int damageVolume = 10;
	// Item2をgetした際の回復量.
	public int recoveryVolume = 40;
	

	void Start () {
		
		// 各種プレハブをロード・取得.
		Bprefab = (GameObject) Resources.Load("BulletPrefab");
		superBprefab = (GameObject) Resources.Load("SuperBulletPrefab");
		gameOverPrefab = (GameObject) Resources.Load("GameOver");
		
		// オーディオソースのコンポーネントを取得.
		audioSource = gameObject.GetComponent<AudioSource>();
		playerDestroy = (GameObject) Resources.Load("PlayerDestroy");
		
		pointLightBase = GameObject.Find("PointLightBase").gameObject;
		pointLightPlus = GameObject.Find("PointLightPlus").gameObject;
		
		// 爆発プレハブをロード.
		bbSimpleDetonator = (GameObject) Resources.Load("MyDetonatorTiny");
		
	}
	
	
	void Update () {
		
		// 弾丸数増加.
		if ( addBullet == true ) {
			
			bulletNum += 1;
			addBullet = false;
		
		}
		
		// scoreアップ.
		if ( item1_get == true ) {
			
			scoreNumScript = GameObject.Find("scoreNum").GetComponent<scoreNum>();
			scoreNumScript.score += 1000;
			item1_get = false;
			
		}
		
		// 1発被弾するとtrueになり、addDamageNumが1加算される.
		if ( addDamage == true ) {
			
			addDamageTurn += 1;
			addDamage = false;
		
		}
		
		// playerがダメージ受けた時の処理.
		if ( addDamageTurn >= 1 ) {
			
			hpDecrease();
		
		}
		
		// hp回復の処理.
		if ( hpRecoveryBool == true ) {
			
			hpRecovery();
			
		}
		
		// energyゲージがmaxになったかどうか.
		if ( energyMaxBool == false ) {
		
			energyRecovery();
		
		}
		
		// superBulletを発射する処理.
		if ( energyMaxBool == true && Input.GetButton( "Fire2" ) && whileFiring == false ) {
			
			whileFiring = true;
			
			// ここでスーパーブレットを生成。インスタンス化 .
			Vector3 superShotPosition = transform.position + new Vector3(0, 0, 2.5f);
			Instantiate( superBprefab, superShotPosition, Quaternion.identity );
			
		}
		
		if ( whileFiring == true ) {
			
			EnergyBar.Health -= 2;
			
			if ( energyDecrement == 50 ) {
				
				whileFiring = false;
				energyMaxBool = false;
				energyDecrement = 0;

			}
			
			energyDecrement += 1;
			
		}
		
		playerMoveControl();
		playerRotationReset();

		// ctrlキーを押した時の挙動.
		if( Input.GetButton( "Fire1" ) && bulletEnable == true ) {
			
			bulletEnable = false;
			shotBullet();
				
		}
		
		// 弾の発射間隔を調節する。大体秒間5発ぐらい.
		// ↓正常に加算される。progressTimeの初期値がどんな桁数でも.
		progressTime += Time.deltaTime;
		// Debug.Log("progressTime: " + progressTime);
		if ( progressTime >= shotInterval ) {
			
			// 経過時間をリセット.
			progressTime = 0.0f;
			bulletEnable = true;
		
		}
		
	}
	
	
	void OnCollisionEnter(Collision other) {		
		
		if ( other.gameObject.tag == "Enemy" ) {
			
			playerDamageDetonator();
			
		} else if ( other.gameObject.tag == "Shogai" ) {
		
			playerDamageDetonator();
			
		}
		
	}
	

	void OnTriggerEnter (Collider other) {		
		
		if ( other.gameObject.tag == "Ebullet" ) {
			
			addDamage = true;
			
		}
		
		if ( other.gameObject.tag == "Item" ) {
		
			if ( other.gameObject.name == "Item_1(Clone)" ) {
				
				audioSource.clip = audioClip_1;
				item1_get = true;
				
			} else if ( other.gameObject.name == "Item_2(Clone)" ) {

				audioSource.clip = audioClip_2;
				item2_get = true;
				hpRecoveryBool = true;
				
			}  else if ( other.gameObject.name == "Item_3(Clone)" ) {
				
				audioSource.clip = audioClip_3;
				addBullet = true;
			
			}
			
			Destroy(other.gameObject);
			audioSource.Play();
			
		}

	}
	
	
	void gameOverJudge() {
		
		if ( healthBar.Health <= damageVolume ) {
					
			Vector3 playerDestroyPosition = transform.position + new Vector3(0, 0, 2.5f);
			// 爆発用インスタンスを生成.
			Instantiate( playerDestroy, playerDestroyPosition, Quaternion.identity );
			
			textureBottomScript = GameObject.Find("RunwayBottom").GetComponent<textureControllerBottom>();
			textureTopScript = GameObject.Find("RunwayTop").GetComponent<textureControllerTop>();
			textureLeftScript = GameObject.Find("RunwayLeft").GetComponent<textureControllerSide>();
			textureRightScript = GameObject.Find("RunwayRight").GetComponent<textureControllerSide>();
			
			textureBottomScript.stopScroll = true;
			textureTopScript.stopScroll = true;
			textureLeftScript.stopScroll = true;
			textureRightScript.stopScroll = true;
			
			// 照明を消す.
			pointLightBase.SetActive(false);
			pointLightPlus.SetActive(false);
			
			// ゲームオーバー用のテキストのプレハブをインスタンス化.
			Instantiate( gameOverPrefab, new Vector3(0.3f, 2.5f, 0), Quaternion.identity );
			
			// 自分自身を破棄.
			Destroy(gameObject);
					
		}
		
	}
	
	
	void hpDecrease() {
		
		// 一番最初のみ、ヘルスバーへの参照を取得する処理.
		if ( damageIncriment == 1 ) {
			
			healthBar = GameObject.Find("HealthBar").GetComponent<MonoHealthbar>();
			gameOverJudge();
			
		}
		
		// ダメージが10加わるまで処理を繰り返す.
		if ( damageIncriment <= damageVolume ) {
			
			// ヘルスバーから1ずつ減算.
			healthBar.Health -= 1;
			
		}

		// 10減算したら、addDamageTurnの数を1減らす.
		if ( damageIncriment == damageVolume ) {
		
			addDamageTurn -= 1;
			damageIncriment = 1;
			return;
			
		}

		damageIncriment += 1;
		
	}
	
	
	void hpRecovery() {
		
		// 一番最初のみ、ヘルスバーへの参照を取得する処理.
		if ( hpIncriment == 1 ) {
			
			healthBar = GameObject.Find("HealthBar").GetComponent<MonoHealthbar>();
				
		}
		
		// hpが10回復するまで処理を繰り返す.
		if ( hpIncriment <= recoveryVolume ) {
			
			// ヘルスバーに1ずつ加算.
			healthBar.Health += 1;
			
		}
		
		// 10加算したら、処理を終了して初期化.
		if ( hpIncriment == recoveryVolume ) {
	
			hpRecoveryBool = false;
			hpIncriment = 1;
			return;

		}
		
		hpIncriment += 1;
		
	}
	
	
	void energyRecovery() {
			
		if ( energyIncriment == 1 ) {
		
			EnergyBar = GameObject.Find("EnergyBar").GetComponent<MonoHealthbar>();
		
		}

		if ( energyIncriment % 4 == 0 ) {
			
			EnergyBar.Health += 1;
			
		}
		
		energyIncriment += 1;
		
		if ( energyIncriment == 400 ) {

			energyMaxBool = true;
			energyIncriment = 0;
			audioSource.clip = audioClip_5;
			audioSource.Play();

		}
	
	}
	
	
	void playerMoveControl() {
		
		// 十字キーの入力に応じてplayerを移動.
		rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * movingSpeed, Input.GetAxis("Vertical") * movingSpeed, 0);
			
		// 壁の突き抜け対策.
		if ( transform.position.x < -11.5f ) {
			
			rigidbody.velocity = new Vector3( movingSpeed, 0, 0);
			
		} else if ( transform.position.x > 11.5f ) {
			
			rigidbody.velocity = new Vector3( movingSpeed*-1.0f, 0, 0);
			
		} else if ( transform.position.y < 0.5f ) {
		
			rigidbody.velocity = new Vector3( 0, movingSpeed, 0);
			
		} else if ( transform.position.y > 23.6f ) {
			
			rigidbody.velocity = new Vector3( 0, movingSpeed*-1, 0);
		
		}
	
	}
	
	
	void shotBullet() {
		
		// ↓ここの中は配列にしてループで回す。あとでリファクタリングする。いや、まあ良いか。このぐらいの数なら。.
		if ( bulletNum == 1 ) {
			
			// bulletを発射するpositionのzをplayerより前にずらす.
			Vector3 shotPosition_1 = transform.position + new Vector3(0, 0, 2.5f);
			GameObject Ibullet_1 = (GameObject) Instantiate( Bprefab, shotPosition_1, Quaternion.identity );
			// 弾丸に力を加える　↑の(0, 0, 1)にbulletSpeedをかけることで、z方向にだけ進む.
			Ibullet_1.rigidbody.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);		

		} else if ( bulletNum == 2 ) {
			
			// bulletを発射するpositionのzをplayerより前にずらす.
			Vector3 shotPosition_1 = transform.position + new Vector3(-0.37f, 0, 2.5f);
			Vector3 shotPosition_2 = transform.position + new Vector3(0.37f, 0, 2.5f);
			
			GameObject Ibullet_1 = (GameObject) Instantiate( Bprefab, shotPosition_1, Quaternion.identity );
			GameObject Ibullet_2 = (GameObject) Instantiate( Bprefab, shotPosition_2, Quaternion.identity );

			// 弾丸に力を加える　↑の(0, 0, 1)にbulletSpeedをかけることで、z方向にだけ進む.
			Ibullet_1.rigidbody.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
			Ibullet_2.rigidbody.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
			
		} else if ( bulletNum >= 3 ) {
			
			// bulletを発射するpositionのzをplayerより前にずらす.
			Vector3 shotPosition_1 = transform.position + new Vector3(-0.5f, -0.35f, 2.5f);
			Vector3 shotPosition_2 = transform.position + new Vector3(0.5f, -0.35f, 2.5f);
			Vector3 shotPosition_3 = transform.position + new Vector3(0, 0.33f, 2.5f);
			
			// instantiate(prehabname, transform.position, transform.rotation)で、弾丸prehabをplayerと同じpojitionに生成.
			// 右辺だけでもcloneは生成される。戻り値として、生成したオブジェクトを取得してる.
			// GameObject Ibullet = (GameObject) Instantiate( Bprefab, transform.position, transform.rotation );
			GameObject Ibullet_1 = (GameObject) Instantiate( Bprefab, shotPosition_1, Quaternion.identity );
			GameObject Ibullet_2 = (GameObject) Instantiate( Bprefab, shotPosition_2, Quaternion.identity );
			GameObject Ibullet_3 = (GameObject) Instantiate( Bprefab, shotPosition_3, Quaternion.identity );
			
			// 弾丸に力を加える　↑の(0, 0, 1)にbulletSpeedをかけることで、z方向にだけ進む.
			Ibullet_1.rigidbody.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
			Ibullet_2.rigidbody.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
			Ibullet_3.rigidbody.AddForce(direction * bulletSpeed, ForceMode.VelocityChange);
			
		}

	}
	
	
	void playerRotationReset() {
	
		// playerの傾きを取得.
		// 一定以上の傾きになってる時間が一定を越えたら、playerのrotationの値を修正する.
		if ( transform.rotation.z > 0.709f || transform.rotation.z < 0.705f ) {
			
			playerRotTime += Time.deltaTime;

			
			if ( playerRotTime > 0.3f ) {

				playerRotFlags = true;
				rigidbody.angularVelocity = Vector3.zero;
				
			}
		
		} else if ( transform.rotation.z < 0.7072f && transform.rotation.z > 0.7070f ) {
			
			playerRotFlags = false;
			playerRotTime = 0.0f;
			
		}
		
		if ( playerRotFlags && playerSideFlags == false ) {
			
			rotationSlerp( 270, 180, 0, 0.12f );
			
		}
		
		// playerが両サイドに一定時間いると横向きになる.
		if ( transform.position.x < -9.3f || transform.position.x > 9.3f ) {
			
			playerSideTime += Time.deltaTime;
			
			if ( playerSideTime > 0.38f ) {
				
				if ( playerSideFlags == false ) {
					// ここでサウンド鳴らす.
					audioSource.clip = audioClip_4;
					audioSource.Play();
					
				}
				
				playerSideFlags = true;
							
			}
			
		} else {
			
			playerSideFlags = false;
			playerSideTime = 0.0f;
			
		}
		
		if ( playerSideFlags && transform.position.x < -9.0f ) {
			
			rotationSlerp( 360, 90, 90, 0.2f );
			
		} else if ( playerSideFlags && transform.position.x > 9.0f ) {
			
			rotationSlerp( 360, 270, -90, 0.2f );
			
		}
		
	}
	
		
	void rotationSlerp( int eulerX, int eulerY, int eulerZ, float rotationSpeed ) {
		
		// どの角度に修正したいかを指定.
		Quaternion toSideRot = Quaternion.Euler(eulerX, eulerY, eulerZ);
		// slarpで徐々にターゲット角度へ修正していく.
		transform.rotation = Quaternion.Slerp (transform.rotation, toSideRot, rotationSpeed);
		
	}
	
	
	void playerDamageDetonator() {
	
		addDamage = true;
		Vector3 struckPosition = transform.position + new Vector3(0, 0, 3.5f);
		Instantiate( bbSimpleDetonator, struckPosition, Quaternion.identity );
	
	}

}
