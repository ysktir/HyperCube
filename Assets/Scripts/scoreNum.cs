using UnityEngine;
using System.Collections;

public class scoreNum : MonoBehaviour {
	
	public int score = 0;
	
	
	void Update () {
	
		guiText.text = score.ToString();
		
	}
	
	
}
