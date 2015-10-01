using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {

	#region public params
	public float Mass = 0.1f;
	#endregion
	
	#region private params
	private int HorizontalRaysCount = 3;
	private int VerticalRaysCount = 3;
	private float Skin = 0.001f;
	
	private Vector2 _a = new Vector2(0, 0);
	private bool _onGround = false;
	private Vector2 _size;
	#endregion

	void Start() {
		_size = GetComponent<BoxCollider2D>().size;
	}

	// Update is called once per frame
	void Update () {
		
		Vector2 resultMovement = new Vector2(0, 0);
		
		resultMovement = UpdateRaycasts (resultMovement);
		resultMovement = HandleGravity (resultMovement);
		
		_a.x += resultMovement.x;
		_a.y += resultMovement.y;

		transform.Translate(_a.x, _a.y, 0);

		var rayStartPosition = new Vector3(0,0,0);
		rayStartPosition.x = transform.position.x;
		rayStartPosition.y = transform.position.y - _size.y/2;
		
		//Debug.DrawRay(rayStartPosition, Vector2.down, Color.green);
	}
	
	private Vector2 UpdateRaycasts(Vector2 resultMovement)
	{
		_onGround = false;

		for (int i = 0; i < VerticalRaysCount; i++)
		{
			var rayStartPosition = new Vector2(0,0);
			rayStartPosition.x = transform.position.x;
			rayStartPosition.y = transform.position.y - _size.y/2;

			var raycast = Physics2D.Raycast(rayStartPosition , new Vector2(0,-1), 10f, LayerMask.GetMask("Floor"));

			//var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
			//Debug.DrawRay(debugLineStart, new Vector3(0, -1, 0), Color.green);

			if(raycast.collider != null)
			{
				Debug.DrawLine(new Vector3(raycast.point.x,raycast.point.y,0),
				               new Vector3(raycast.point.x,raycast.point.y + 1,0),
				               Color.red);
				float distance = raycast.point.y - (rayStartPosition.y + _a.y);
				if (raycast.point.y > (rayStartPosition.y + _a.y) - Skin)
				{
					transform.position = new Vector3(transform.position.x, raycast.point.y + _size.y/2,transform.position.z);
					//transform.position.y = raycast.point.y + Skin + _size.y/2;
					_onGround = true;
					break;
				}
			}
		}

		return resultMovement;
	}
	
	private Vector2 HandleGravity(Vector2 resultMovement)
	{
		if (!_onGround)
		{
			resultMovement.y -= Mass * Time.deltaTime;
		}
		else if (_onGround && _a.y < 0)
		{
			_a.y = 0;
			resultMovement.y = 0;
		}

		return resultMovement;
	}
}
