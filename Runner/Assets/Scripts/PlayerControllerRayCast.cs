using UnityEngine;
using System.Collections;

public class PlayerControllerRayCast : MonoBehaviour {

	private PlayerPhysics _PlayerPhysics;
	// Use this for initialization
	void Start () {
		_PlayerPhysics = GetComponent<PlayerPhysics> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump") && _PlayerPhysics.OnGround)
			_PlayerPhysics.Jump ();
	}
}
