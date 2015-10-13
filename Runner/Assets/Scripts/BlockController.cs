using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour {

    private BasePhysics BasePhysics;
     
	// Use this for initialization
	void Start () {
        BasePhysics = GetComponent<BasePhysics>();
    }
	
	// Update is called once per frame
	void Update () {

        BasePhysics.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * 0.001f, 0));

        BasePhysics.SetVelocity(Vector2.zero);

        if (Input.GetKey(KeyCode.LeftArrow))
            BasePhysics.SetVelocity(Vector2.left * 0.1f);

        if (Input.GetKey(KeyCode.RightArrow))
            BasePhysics.SetVelocity(Vector2.right * 0.1f);

        if (Input.GetButtonDown("Jump"))
            BasePhysics.AddForce(Vector2.up*0.15f);

    }
}
