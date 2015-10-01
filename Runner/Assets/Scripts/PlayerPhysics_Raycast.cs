using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Raycast : MonoBehaviour {

	#region public params
	public float Mass = 1;
    #endregion

    #region private params
    private int HorizontalRaysCount = 3;
	private int VerticalRaysCount = 3;
	private float Skin = 0.001f;

    private Vector2 _a = new Vector2(0, 0);
    private bool _onGround = false;
    #endregion

    // Update is called once per frame
    void Update () {

        Vector2 resultMovement = new Vector2(0, 0);

		UpdateRaycasts ();
        HandleGravity (resultMovement);

        _a.x += resultMovement.x;
        _a.y += resultMovement.y;

        transform.position.Set(transform.position.x + _a.x, 
            transform.position.y + _a.y, 
            transform.position.z);
    }

    private void UpdateRaycasts()
    {
        for (int i = 0; i < VerticalRaysCount; i++)
        {
            var raycast = Physics2D.Raycast(transform.position, new Vector2(0,-1));

            if(raycast.collider != null)
            {
                float distance = Mathf.Abs(raycast.point.y - transform.position.y);
                if (distance < Skin)
                    _onGround = true;
            }

            Debug.DrawRay(raycast.point, raycast.normal, Color.green);
        }
    }

    private void HandleGravity(Vector2 resultMovement)
    {
        if (!_onGround)
        {
            resultMovement.y += Mass * Time.deltaTime;
        }
        else if (_onGround && _a.y < 0)
        {
            _a.y = 0;
            resultMovement.y = 0;
        }
    }

}
