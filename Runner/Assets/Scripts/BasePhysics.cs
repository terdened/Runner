using UnityEngine;
using System.Collections;

public class BasePhysics : MonoBehaviour {

    #region public params
    public float G = 0.1f;
    public Vector2 GravityDirection = Vector2.down;
    public bool OnGround = false;
    public float FadePower = 0.0001f;
    #endregion

    #region private params
    private float Skin = 0.001f;
    private float MinRaycastDistance = 0.3f;
    private Vector2 _a = new Vector2(0, 0);
    private Vector2 _size;
    private int _horizontalRasycastCount;
    private int _verticalRasycastCount;
    #endregion

    void Start()
    {
        _size = GetComponent<BoxCollider2D>().size;
        _horizontalRasycastCount = (int)(_size.y / MinRaycastDistance) + 1;
        _verticalRasycastCount = (int)(_size.x / MinRaycastDistance) + 1;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 aIncrease = new Vector2(0, 0);

        aIncrease = HandleGravity(aIncrease);
        aIncrease = UpdateRaycasts(aIncrease);

        _a.x += aIncrease.x;
        _a.y += aIncrease.y;

        transform.Translate(_a.x, _a.y, 0);
    }

    private Vector2 UpdateRaycasts(Vector2 resultMovement)
    {
        OnGround = false;
        int verticalDirection = 1;
        int horizontalDirection = 1;

        if (_a.y < 0)
        {
            verticalDirection = -1;
        }

        if (_a.x < 0)
        {
            horizontalDirection = -1;
        }

        for (int i = 0; i < _verticalRasycastCount; i++)
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x - _size.x / 2 + i * MinRaycastDistance + Skin;
            rayStartPosition.y = transform.position.y - _size.y / 2;

            var raycast = Physics2D.Raycast(rayStartPosition,
                                            new Vector2(0, verticalDirection),
                                            10f,
                                            LayerMask.GetMask("Floor"));

            var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
            Debug.DrawRay(debugLineStart, new Vector3(0, verticalDirection, 0), Color.green);

            if (raycast.collider != null)
            {
                float distance = raycast.point.y - (rayStartPosition.y + (_a.y * verticalDirection));
                if (raycast.point.y > (rayStartPosition.y + _a.y) - Skin)
                {
                    transform.position = new Vector3(transform.position.x, raycast.point.y + _size.y / 2, transform.position.z);

                    _a.y = 0;
                    resultMovement.y = 0;

                    if (verticalDirection < 0)
                        OnGround = true;

                    break;
                }
            }
        }

        for (int i = 0; i < _horizontalRasycastCount; i++)
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x - _size.x / 2;
            rayStartPosition.y = transform.position.y - _size.y / 2 + i * MinRaycastDistance + Skin;

            var raycast = Physics2D.Raycast(rayStartPosition,
                                            new Vector2(horizontalDirection, 0),
                                            10f,
                                            LayerMask.GetMask("Floor"));

            var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
            Debug.DrawRay(debugLineStart, new Vector3(horizontalDirection, 0, 0), Color.blue);

            if (raycast.collider != null)
            {
                float distance = raycast.point.x - (rayStartPosition.x + (_a.x * horizontalDirection));
                if (raycast.point.x > (rayStartPosition.x + _a.x) - Skin)
                {
                    transform.position = new Vector3(raycast.point.x + _size.x / 2, transform.position.y, transform.position.z);

                    _a.x = 0;
                    resultMovement.x = 0;

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
            resultMovement.x += G * GravityDirection.x * Time.deltaTime;
            resultMovement.y += G * GravityDirection.y * Time.deltaTime;
        }

        return resultMovement;
    }

    public void AddForce(Vector2 direction)
    {
        _a += direction;
    }
}
