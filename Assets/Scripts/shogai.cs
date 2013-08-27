using UnityEngine;
using System.Collections;

public class shogai : MonoBehaviour {
	
	private float movingSpeed;
	private float shogaiScale;
	
	// オーディオリソースのコンポーネント格納用の変数を取得.
	private AudioSource audioSource;
	public AudioClip hitBullet;
	
	// 爆発用のprefabを格納するための変数宣言.
	private GameObject smallDetonator;
	private GameObject shogaiDetonator;

	// 明るさ.
	private float brightnessNum;
	
	private Color[] shogaiColors;
	
	private bool addDamageJudge = false;
	private int addDamageNum = 0;

	
	void Start () {

		movingSpeed = Random.Range ( -30.0f, -47.0f );
		shogaiScale = Random.Range ( 5.0f, 11.0f );

		Vector3 playerDestroyPosition = transform.position + new Vector3(0, 0, 2.5f);
		transform.localScale = new Vector3 ( shogaiScale, shogaiScale, shogaiScale);

		audioSource = gameObject.GetComponent<AudioSource>();

		smallDetonator = (GameObject) Resources.Load("SmallDetonator");
		shogaiDetonator = (GameObject) Resources.Load("ShogaiDestroy");
		
		shogaiColors = new Color[6];
		
		shogaiColors[0] = new Color(0.0f, 0.455f, 1.0f, 1.0f); // 青.
		shogaiColors[1] = new Color(1.0f, 0.0f, 0.0f, 1.0f); // 赤.
		shogaiColors[2] = new Color(0.906f, 1.0f, 0.157f, 1.0f); // 黄色.
		shogaiColors[3] = new Color(0.157f, 1.0f, 0.469f, 1.0f); // 緑.
		shogaiColors[4] = new Color(1.0f, 0.629f, 0.052f, 1.0f); // オレンジ.
		shogaiColors[5] = new Color(1.0f, 1.0f, 1.0f, 1.0f); // 白.
		// shogaiColor[6] = new Color(0.42f, 0.0f, 1.0f, 1.0f); // 紫.
		
		int colorNum = Random.Range ( 0, 6 );
		renderer.material.SetColor("_LightColor", shogaiColors[colorNum]);
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		
		if ( addDamageJudge == true ) {
			
			addDamageJudge = false;
			addDamageNum += 1;
			
			if ( addDamageNum == 13 ) {
				
				Instantiate( shogaiDetonator, transform.position, Quaternion.identity );
				Destroy(gameObject);
				
			}
		
		}

		// 1.0は中央値・0.07は揺れのスピード。8.5は揺れの幅.
		brightnessNum = 0.5f + Mathf.Sin (Time.frameCount * 0.055f )*8.5f;

		// 光度を設定.
		renderer.material.SetFloat("_LightBrightness", brightnessNum);
		
		rigidbody.velocity = new Vector3( 0, 0, movingSpeed );
		
		// playerを通り過ぎたenemyを破棄する.
		if (transform.position.z < -5.0) {
		
			Destroy(gameObject);
		
		}

	}
	
	
	void OnTriggerEnter (Collider other) {
	
	
		if ( other.gameObject.tag == "Bullet" ) {
			
			audioSource.clip = hitBullet;
			audioSource.Play();
			Destroy(other.gameObject);
			
			Vector3 smallDetonatorPosition = other.gameObject.transform.position + new Vector3( 0, 0, -3.0f );
			
			// ここで軽く爆発させる.
			Instantiate( smallDetonator, smallDetonatorPosition, Quaternion.identity );
			
			addDamageJudge = true;
				
		} else if ( other.gameObject.tag == "Ebullet" ) {
		
			Destroy(other.gameObject);
		
		} else if ( other.gameObject.tag == "Sbullet" ) {
			
			Instantiate( shogaiDetonator, transform.position, Quaternion.identity );
			Destroy(gameObject);
			
		}

	}
	
}
