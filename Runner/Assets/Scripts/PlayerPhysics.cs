using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {

	#region public params
	public float Mass = 0.1f;
	public Vector2 GravityDirection = Vector2.down;
	public bool OnGround = false;
	public float MaxRunSpeed = 0.2f;
	public float RunAcceleration = 0.001f;
	public float FadePower = 0.0001f;
	public float JumpPower = 0.0001f;
	#endregion
	
	#region private params
	private float Skin = 0.001f;
	private float MinRaycastDistance = 0.3f;
	private Animator PlayerAnimator;
	private GameObject PlayerCamera;

	private float _runSpeed = 0f;
	private Vector2 _a = new Vector2(0, 0);
	private Vector2 _size;
	private int _horizontalRasycastCount;
	private int _verticalRasycastCount;
	#endregion

	void Start() {
		_size = GetComponent<BoxCollider2D>().size;
		_horizontalRasycastCount = (int)(_size.y / MinRaycastDistance) + 1;
		_verticalRasycastCount = (int)(_size.x / MinRaycastDistance) + 1;

		PlayerAnimator = GetComponent<Animator> ();
		PlayerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
	}

	// Update is called once per frame
	void Update () {
		
		Vector2 resultMovement = new Vector2(0, 0);
		
		resultMovement = UpdateRaycasts (resultMovement);
		resultMovement = HandleGravity (resultMovement);
		
		_a.x += resultMovement.x;
		_a.y += resultMovement.y;

		WalkUpdate ();
		UpdateAnimation ();
		UpdateCamera ();

		transform.Translate(_a.x + _runSpeed, _a.y, 0);
	}
	
	private Vector2 UpdateRaycasts(Vector2 resultMovement)
	{
		OnGround = false;
		int verticalDirection = 1;
		int horizontalDirection = 1;

		if (_a.y < 0) {
			verticalDirection = -1;
		}

		if (_a.x < 0) {
			horizontalDirection = -1;
		}

		for (int i = 0; i < _verticalRasycastCount; i++)
		{
			var rayStartPosition = new Vector2(0,0);
			rayStartPosition.x = transform.position.x - _size.x/2 + i * MinRaycastDistance;
			rayStartPosition.y = transform.position.y - _size.y/2;

			var raycast = Physics2D.Raycast(rayStartPosition, 
			                                new Vector2(0,verticalDirection), 
			                                10f, 
			                                LayerMask.GetMask("Floor"));

			var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
			Debug.DrawRay(debugLineStart, new Vector3(0, verticalDirection, 0), Color.green);

			if(raycast.collider != null)
			{
				float distance = raycast.point.y - (rayStartPosition.y + (_a.y * verticalDirection));
				if (raycast.point.y > (rayStartPosition.y + _a.y) - Skin)
				{
					transform.position = new Vector3(transform.position.x, raycast.point.y + _size.y/2,transform.position.z);
					//transform.position.y = raycast.point.y + Skin + _size.y/2;
					OnGround = true;
					break;
				}
			}
		}

		for (int i = 0; i < _verticalRasycastCount; i++)
		{
			var rayStartPosition = new Vector2(0,0);
			rayStartPosition.x = transform.position.x - _size.x/2 + i * MinRaycastDistance;
			rayStartPosition.y = transform.position.y - _size.y/2;
			
			var raycast = Physics2D.Raycast(rayStartPosition, 
			                                new Vector2(0,verticalDirection), 
			                                10f, 
			                                LayerMask.GetMask("Floor"));
			
			var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
			Debug.DrawRay(debugLineStart, new Vector3(0, verticalDirection, 0), Color.green);
			
			if(raycast.collider != null)
			{
				float distance = raycast.point.y - (rayStartPosition.y + (_a.y * verticalDirection));
				if (raycast.point.y > (rayStartPosition.y + _a.y) - Skin)
				{
					transform.position = new Vector3(transform.position.x, raycast.point.y + _size.y/2,transform.position.z);
					//transform.position.y = raycast.point.y + Skin + _size.y/2;
					OnGround = true;
					break;
				}
			}
		}

		return resultMovement;
	}
	
	private Vector2 HandleGravity(Vector2 resultMovement)
	{
		if (!OnGround)
		{
			resultMovement.x += Mass * GravityDirection.x * Time.deltaTime;
			resultMovement.y += Mass * GravityDirection.y * Time.deltaTime;
		}
		else if (OnGround && _a.y < 0)
		{
			_a.y = 0;
			resultMovement.y = 0;
		}

		return resultMovement;
	}

	public void AddForce(Vector2 direction)
	{
		_a += direction;
	}

	public void WalkUpdate()
	{
		if (OnGround) {
			if (_runSpeed < MaxRunSpeed)
				_runSpeed += RunAcceleration;
			
			if (_runSpeed > MaxRunSpeed)
				_runSpeed = MaxRunSpeed;
		} else {
			if (_runSpeed > 0)
				_runSpeed -= FadePower;
			
			if (_runSpeed < 0)
				_runSpeed = 0;
		}
	}

	public void Jump()
	{
		AddForce (new Vector2 (0, JumpPower));
	}

 	public void UpdateAnimation()
	{
		if (OnGround && _runSpeed <= 0)
			PlayerAnimator.SetInteger ("State", 0);

		if (OnGround && _runSpeed > 0)
			PlayerAnimator.SetInteger ("State", 1);

		if (!OnGround && _a.y > 0)
			PlayerAnimator.SetInteger ("State", 2);

		if (!OnGround && _a.y < 0)
			PlayerAnimator.SetInteger ("State", 3);
	}

	public void UpdateCamera()
	{
		Vector3 toPosition = new Vector3 (_runSpeed * 6 + 2, _a.y * 3, -10);
		Vector3 cameraPosition = MoveTowards (PlayerCamera.transform.localPosition, toPosition, 0.1f);
		PlayerCamera.transform.localPosition = cameraPosition;
	}

	private Vector3 MoveTowards(Vector3 from, Vector3 to, float speed)
	{
		Vector3 result = Vector3.Lerp (from, to, speed);
		return result;
	}
}
