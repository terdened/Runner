using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class PlayerPhysics_Tutorial : MonoBehaviour {

	public LayerMask collisionMask;
	
	private BoxCollider2D collider;
	private Vector2 s;
	private Vector2 c;
	
	private Vector2 originalSize;
	private Vector2 originalCentre;
	private float colliderScale;
	
	private int collisionDivisionsX = 3;
	private int collisionDivisionsY =10;
	
	private float skin = .005f;
	
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStopped;
	
	Ray ray;
	RaycastHit hit;
	
	void Start() {
		collider = GetComponent<BoxCollider2D>();
		colliderScale = transform.localScale.x;
		
		originalSize = collider.size;
		originalCentre = collider.offset;
		SetCollider(originalSize,originalCentre);
	}
	
	public void Move(Vector2 moveAmount) {
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position;
		
		// Check collisions above and below
		grounded = false;
		
		for (int i = 0; i<collisionDivisionsX; i ++) {
			float dir = -Mathf.Sign(deltaY);
			float x = (p.x + c.x - s.x/2) + s.x/(collisionDivisionsX-1) * i; // Left, centre and then rightmost point of collider
			float y = p.y + c.y - 0.1f + s.y/2 * dir; // Bottom of collider

			RaycastHit2D hit = Physics2D.Raycast(new Vector2(x,y), new Vector2(0,dir));

			Debug.DrawRay(new Vector3(x,y,0), new Vector3(0,dir));
			
			if (hit.collider != null) {
				// Get Distance between player and ground
				float distance = Mathf.Abs(hit.point.y - y);
				Debug.Log(distance);
				// Stop player's downwards movement after coming within skin width of a collider
				if (distance > skin) {
					deltaY = distance * dir - skin * dir;
				}
				else {
					deltaY = 0;
				}
				
				grounded = true;
				break;
			}
		}
		
		
		// Check collisions left and right
		movementStopped = false;
		for (int i = 0; i<collisionDivisionsY; i ++) {
			float dir = Mathf.Sign(deltaX);
			float x = p.x + c.x + s.x/2 * dir;
			float y = p.y + c.y - s.y/2 + s.y/(collisionDivisionsY-1) * i;
			
			//ray = new Ray(new Vector2(x,y), new Vector2(dir,0));

			RaycastHit2D hit = Physics2D.Raycast(new Vector2(x,y), new Vector2(dir,0));
			//Debug.DrawRay(hit.transform.position, hit.normal);

			if (hit.collider != null) {
				// Get Distance between player and ground
				float distance = Vector2.Distance (new Vector2(x,y), hit.point);

				// Stop player's downwards movement after coming within skin width of a collider
				if (distance > skin) {
					deltaX = distance * dir - skin * dir;
				}
				else {
					deltaX = 0;
				}

				movementStopped = true;
				break;
			}
		}
		
		if (!grounded && !movementStopped) {
			Vector3 playerDir = new Vector3(deltaX,deltaY);
			Vector3 o = new Vector3(p.x + c.x + s.x/2 * Mathf.Sign(deltaX),p.y + c.y + s.y/2 * Mathf.Sign(deltaY));
			ray = new Ray(o,playerDir.normalized);
			
			if (Physics.Raycast(ray,Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY),collisionMask)) {
				grounded = true;
				deltaY = 0;
			}
		}
		
		
		Vector2 finalTransform = new Vector2(deltaX,deltaY);
		
		transform.Translate(finalTransform,Space.World);
	}
	
	// Set collider
	public void SetCollider(Vector3 size, Vector3 centre) {
		collider.size = size;
		collider.offset = centre;
		
		s = size * colliderScale;
		c = centre * colliderScale;
	}
	
	public void ResetCollider() {
		SetCollider(originalSize,originalCentre);	
	}
}
