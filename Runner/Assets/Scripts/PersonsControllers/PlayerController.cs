using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

    #region public params
    public Vector2 JumpDirection = Vector2.up * 0.15f;
    public Vector2 Velocity = Vector2.right * 0.1f;
    #endregion

    // Components
    private PlayerPhysics playerPhysics;
	private Animator animator;
    private GameObject playerCamera;
	
	
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
		playerCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0];
        playerPhysics.SetVelocity(Velocity);
    }
	
	void Update () {
        if (playerPhysics.OnGround && Input.GetButtonDown("Jump"))
            playerPhysics.AddForce(JumpDirection);

        if (playerPhysics.OnGround)
            playerPhysics.SetVelocity(Velocity);
        else
            playerPhysics.SetVelocity(Vector2.zero);

        UpdateAnimation();
        UpdateCamera();
    }

    public void UpdateAnimation()
    {
        if (playerPhysics.OnGround && playerPhysics.GetMomentum().x <= 0)
            animator.SetInteger("State", 0);

        if (playerPhysics.OnGround && playerPhysics.GetMomentum().x > 0)
            animator.SetInteger("State", 1);

        if (!playerPhysics.OnGround && playerPhysics.GetMomentum().y > 0)
            animator.SetInteger("State", 2);

        if (!playerPhysics.OnGround && playerPhysics.GetMomentum().y < 0)
            animator.SetInteger("State", 3);
    }

    public void UpdateCamera()
    {
        Vector2 momentum = playerPhysics.GetMomentum();
        Vector3 toPosition = new Vector3(momentum.x * 16 + 2, momentum.y * 6, -10);
        Vector3 cameraPosition = MoveTowards(playerCamera.transform.localPosition, toPosition, 0.1f);
        playerCamera.transform.localPosition = cameraPosition;
    }

    private Vector3 MoveTowards(Vector3 from, Vector3 to, float speed)
    {
        Vector3 result = Vector3.Lerp(from, to, speed);
        return result;
    }
}
