using UnityEngine;
using System.Collections;

public class FloorDetector : MonoBehaviour {

	public Player _Player;
	public int CollisionCount = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Floor") {
			CollisionCount++;
			_Player.isOnFloor = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Floor") {
			CollisionCount--;
			if(CollisionCount <= 0)
			 _Player.isOnFloor = false;
		}
	}
}
