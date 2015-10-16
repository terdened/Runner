using UnityEngine;
using System.Collections;

public class Teset : MonoBehaviour {

	public float skin = 0.01f;
	public bool onGround = false;

	void Start() {
	}

	void Update() {

		onGround = false;
		var vectorBegin = transform.position;
		vectorBegin.y -= 0.51f;

		RaycastHit2D hit = Physics2D.Raycast(vectorBegin, Vector2.down);

		if (hit.collider != null) {
			float distance = Mathf.Abs(hit.point.y - vectorBegin.y);

			if(distance < skin)
				onGround = true;
		}

		if (!onGround)
			transform.Translate (0, -0.1f, 0);
	}
}
