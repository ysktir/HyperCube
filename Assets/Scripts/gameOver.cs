using UnityEngine;
using System.Collections;

public class gameOver : MonoBehaviour {
	
	private GameObject retryObj;
	private GameObject homeObj;
	private GameObject bousenLeftObj;
	private GameObject bousenRightObj;
	private bool retryOrNot = true;
	private bool permitkey = false;
	
	private float progressTime;
	private float timeAfterSelected;
	
	private AudioSource audioSource;
	public AudioClip selectSound;
	public AudioClip selectPushSound;
	
	private string mainOrTitle;
	
	private bool pushed = false;
	private bool sceneLoad = false;
	
	
	void Start () {
		
		retryObj = GameObject.Find("Retry").gameObject;
		homeObj = GameObject.Find("Home").gameObject;
		bousenLeftObj = GameObject.Find("BousenLeft").gameObject;
		bousenRightObj = GameObject.Find("BousenRight").gameObject;
		
		retryObj.SetActive(false);
		homeObj.SetActive(false);
		bousenLeftObj.SetActive(false);
		bousenRightObj.SetActive(false);
		
		audioSource = gameObject.GetComponent<AudioSource>();
		
	}
	

	void Update () {
		
		progressTime += Time.deltaTime;
		
		if ( transform.position.y >= 0.75f ) {
		
			transform.position = transform.position - new Vector3(0, 0.03f, 0);
		
		}
		
		if ( transform.position.y < 0.755f && progressTime > 1.7f ) {
			
			retryObj.SetActive(true);
			homeObj.SetActive(true);
			bousenLeftObj.SetActive(true);
			permitkey = true;
			
		}
		
		if ( permitkey ) {
		
			if ( Input.GetKeyDown(KeyCode.LeftArrow) ) {
			
				retryOrNot = true;
				audioSource.clip = selectSound;
				audioSource.Play();
				
			} else if ( Input.GetKeyDown(KeyCode.RightArrow) ) {
			
			 	retryOrNot = false;
				audioSource.clip = selectSound;
				audioSource.Play();
			
			}
		
		
			if ( retryOrNot ) {
			
				bousenLeftObj.SetActive(true);
				bousenRightObj.SetActive(false);
			
			} else {
			
				bousenLeftObj.SetActive(false);
				bousenRightObj.SetActive(true);
			
			}
			
			
			if ( Input.GetButton( "Fire1" ) && retryOrNot == true && pushed == false ) {

				audioSource.clip = selectPushSound;
				audioSource.Play();
				pushed = true;
				mainOrTitle = "main";

			} else if ( Input.GetButton( "Fire1" ) && retryOrNot == false && pushed == false ) {

				audioSource.clip = selectPushSound;
				audioSource.Play();
				pushed = true;
				mainOrTitle = "title";
				
			}
			
			if ( pushed ) {
				
				timeAfterSelected += Time.deltaTime;
				
				if ( timeAfterSelected > 0.5f ) {
					
					sceneLoad = true;
					
				}
				
			}
			
			if ( sceneLoad && mainOrTitle == "main" ) {
				
				Application.LoadLevel("main");
				
			} else if ( sceneLoad && mainOrTitle == "title" ) {
				
				Application.LoadLevel("title");
				
			}

		}

	}
}
