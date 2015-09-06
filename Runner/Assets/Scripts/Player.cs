using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int State = 0;
	public int Speed = 3;
	public int JumpForce = 200;
	public int JerkForce = 300;
	public Animator animator;
	public bool isGame = false;
	public bool isOnFloor = false;
	static int MoveState = 1835144160; 
	static int JumpState = -1663028631;
	static int FallState = -1073484941;
	static int StayState = 1700444131;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		HandleMove ();
		HandleFall ();
	}

	public void Run()
	{
		SetState (1);
		isGame = true;
	}

	public void Stop()
	{
		SetState (0);
		isGame = false;
	}

	public void Jump()
	{
		if (isGame && isOnFloor) {
			SetState (2);
			GetComponent<Rigidbody2D> ().velocity = Vector2.right * Speed;
			GetComponent<Rigidbody2D> ().AddForce (Vector2.up * JumpForce);
		}
	}

	public void Jerk()
	{
		if (isGame) {
			GetComponent<Rigidbody2D> ().AddForce (Vector2.right * JerkForce);
		}
	}

	private void SetState(int state)
	{
		State = state;
		animator.SetInteger ("State", State);
	}

	private void HandleFall()
	{
		var currentBaseState = animator.GetCurrentAnimatorStateInfo(0);

		if (GetComponent<Rigidbody2D> ().velocity.y < 0
		    && State != 3
		    && State != 0
		    && !isOnFloor) {
			SetState(3);
		}else
		if (GetComponent<Rigidbody2D> ().velocity.y >= 0 
			&& State == 3
			&& State != 0
			&& isOnFloor) {
			SetState(1);
		}
	}

	private void HandleMove()
	{
		var currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
		
		if (isGame && isOnFloor && State == 1) {
			GetComponent<Rigidbody2D> ().position += Vector2.right * Speed* Time.deltaTime;
		}
	}
}
