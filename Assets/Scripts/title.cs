using UnityEngine;
using System.Collections;

public class title : MonoBehaviour {
	
	private float progressTime = 0.0f;
	private GameObject pressCtrl;
	private int activeOrNot = 1;
	private bool pushFlag = false;
	private bool flashed = false;
	public bool takeOff = false;
	private bool startFlag = false;
	
	private GameObject engineLeft;
	private GameObject engineRight;
	
	// オーディオリソースのコンポーネント格納用の変数を取得.
	private AudioSource audioSource;
	public AudioClip audioClip_1;
	public AudioClip audioClip_2;
	
	private int takeOffNum = 1;
		
	
	// Use this for initialization
	void Start () {
	
		pressCtrl = GameObject.Find("pressCtrl").gameObject;
		
		engineLeft = GameObject.Find("Engine Left").gameObject;
		engineRight = GameObject.Find("Engine Right").gameObject;
		engineLeft.SetActive(false);
		engineRight.SetActive(false);
		
		audioSource = gameObject.GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if( Input.GetButton("Fire1") && pushFlag == false ) {
		
			audioSource.clip = audioClip_1;
			pushFlag = true;
			
			if ( activeOrNot == 1 ) {
				
				audioSource.Play();
			
			}
			
		}
		
		
		if ( pushFlag == true && flashed == false ) {
			
			progressTime += Time.deltaTime;
			
			if ( progressTime > 0.09f && activeOrNot % 2 == 1 ) {
			
				pressCtrl.SetActive(false);
				activeOrNot += 1;
				progressTime = 0.0f;
			
			} else if ( progressTime > 0.09f && activeOrNot % 2 == 0 ) {
				
				pressCtrl.SetActive(true);
				activeOrNot += 1;
				progressTime = 0.0f;
				
			}
		
		}
		
		// Debug.Log(activeOrNot);
		
		if ( activeOrNot == 8 ) {
			
			// で、ここで秒数を計算するか、飛行船がどこまで飛ぶか計算してから、startFlagをtrueに.
			flashed = true;
			
			// ここで飛行船のライトを付けて、飛行機を前に動かす.
			progressTime += Time.deltaTime;
			
			if ( progressTime > 0.4f ) {

				takeOff = true;
				progressTime = 0.0f;
				activeOrNot = 0;

			}
			
		}
		
		if ( takeOff == true ) {
			
			if ( takeOffNum == 1 ) {
				
				audioSource.clip = audioClip_2;
				audioSource.Play();
				
				engineLeft.SetActive(true);
				engineRight.SetActive(true);
				
				takeOffNum += 1;
			}
			
			progressTime += Time.deltaTime;
				
			if ( progressTime > 0.85f ) {
			
				takeOff = false;
				startFlag = true;
				
			}

		}
		
		
		if ( startFlag == true ) {
		
			Application.LoadLevel("main");
	
		}
			
		// Debug.Log ("time: " + time);
		
	}
	
}
