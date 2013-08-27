using UnityEngine;
using System.Collections;

public class cameraController : MonoBehaviour {
	
	// playerに対するcameraの相対的なポジション.
	public float cameraX = 0.0f;
	public float cameraY = 0.55f;
	public float cameraZ = -3.4f;
	
	// playerが移動した際のカメラの移動先ポジション.
	private float cameraFinalX;
	private float cameraFinalY;
	private float cameraFinalZ;
	
	private float cameraMaxWidth = 11.2f;
	private float cameraMaxHeight = 23.0f;
	
	// 敵の距離に応じて得点を計算。intにキャストする.
	GameObject Player;
	
	private Vector3 targetPosition;
	private float damp;
	
	
	void Start () {
		
		Player = GameObject.Find("Player");
		damp = 0.1f;

	}
	
	
	void LateUpdate () {

		if ( Player.transform.position.x < cameraMaxWidth*-1 ) {
		
			cameraFinalX = cameraMaxWidth*-1;
			
		} else if ( Player.transform.position.x > cameraMaxWidth ) {
		
			cameraFinalX = cameraMaxWidth;
			
		} else {
		
			cameraFinalX = Player.transform.position.x;
			
		}
		
		if ( Player.transform.position.y > cameraMaxHeight ) { 
		
			cameraFinalY = cameraMaxHeight;
		
		} else {
			
			cameraFinalY = Player.transform.position.y + cameraY;
			
		}
			
		cameraFinalZ = Player.transform.position.z + cameraZ;
		
		targetPosition = new Vector3( cameraFinalX, cameraFinalY, cameraFinalZ );
		
		transform.position = Vector3.Lerp( transform.position, targetPosition, damp );
		
	}
	
}