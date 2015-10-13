using UnityEngine;
using System.Collections;

public class BasePhysics : MonoBehaviour {

    #region public params
    public float G = 0.15f;
    public Vector2 GravityDirection = Vector2.down;
    public bool OnGround = false;
    public float AirResistance = 0.0001f;
    public float FloorResistance = 0.001f;
    #endregion

    #region private params
    //private float Skin = 0.001f;
    private float MinRaycastDistance = 0.33f;
    private Vector2 _momentum = new Vector2(0, 0);
    private Vector2 _velocity = new Vector2(0, 0);
    private float _aVelocity = 0.001f;
    private Vector2 _resistance;
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
        aIncrease = HandleVelocity(aIncrease);
        
        aIncrease = UpdateRaycasts(aIncrease);

        _momentum.x += aIncrease.x;
        _momentum.y += aIncrease.y;

        HandleFade();

        transform.Translate(_momentum.x, _momentum.y, 0);
    }

    private Vector2 UpdateRaycasts(Vector2 resultMovement)
    {
        var nextA = _momentum + resultMovement;

        OnGround = false;
        _resistance = new Vector2(AirResistance, AirResistance);

        if (nextA.y < 0)
        for (int i = 0; i < _verticalRasycastCount; i++)
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x - _size.x / 2 + i * MinRaycastDistance;
            rayStartPosition.y = transform.position.y - _size.y / 2;

            var raycast = Physics2D.Raycast(rayStartPosition,
                                            new Vector2(0, -1),
                                            10f,
                                            LayerMask.GetMask("Floor"));

            var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
            Debug.DrawLine(debugLineStart, debugLineStart + new Vector3(0, -1, 0), Color.green);


            if (raycast.collider != null)
            {
                float distance = raycast.point.y - (rayStartPosition.y - nextA.y);
                if (raycast.point.y > (rayStartPosition.y + nextA.y))
                {
                    //transform.position = new Vector3(transform.position.x, raycast.point.y + _size.y / 2, transform.position.z);
                    _momentum.y = 0;
                    resultMovement.y = 0;
                    OnGround = true;
                    _resistance.x = FloorResistance;

                    break;
                }
            }
        }

        if(nextA.y > 0)
        for (int i = 0; i < _verticalRasycastCount; i++)
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x - _size.x / 2 + i * MinRaycastDistance;
            rayStartPosition.y = transform.position.y + _size.y / 2;

            var raycast = Physics2D.Raycast(rayStartPosition,
                                            new Vector2(0, 1),
                                            10f,
                                            LayerMask.GetMask("Floor"));

            var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
            Debug.DrawLine(debugLineStart, debugLineStart + new Vector3(0, 1, 0), Color.green);
            

            if (raycast.collider != null)
            {
                float distance = raycast.point.y - (rayStartPosition.y + (nextA.y * 1));
                if (raycast.point.y < (rayStartPosition.y + nextA.y))
                {
                    //transform.position = new Vector3(transform.position.x, raycast.point.y + _size.y / 2, transform.position.z);

                    _momentum.y = 0;
                    resultMovement.y = 0;

                    break;
                }
            }
        }

        if (nextA.x > 0)
        for (int i = 0; i < _horizontalRasycastCount; i++)
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x + _size.x / 2 ;
            rayStartPosition.y = transform.position.y - _size.y / 2 + i * MinRaycastDistance + 0.001f;

            var raycast = Physics2D.Raycast(rayStartPosition,
                                            new Vector2(1, 0),
                                            10f,
                                            LayerMask.GetMask("Floor"));

            var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
            Debug.DrawLine(debugLineStart, debugLineStart + new Vector3(1, 0, 0), Color.blue);

            if (raycast.collider != null)
            {
                float distance = raycast.point.x - (rayStartPosition.x + nextA.x );
                if (raycast.point.x < rayStartPosition.x + nextA.x)
                {
                    //transform.position = new Vector3(raycast.point.x + _size.x / 2, transform.position.y, transform.position.z);

                    _momentum.x = 0;
                    resultMovement.x = 0;

                    break;
                }
            }
        }

        if (nextA.x < 0)
        for (int i = 0; i < _horizontalRasycastCount; i++)
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x - _size.x / 2;
            rayStartPosition.y = transform.position.y - _size.y / 2 + i * MinRaycastDistance + 0.001f;

            var raycast = Physics2D.Raycast(rayStartPosition,
                                            new Vector2(-1, 0),
                                            10f,
                                            LayerMask.GetMask("Floor"));

            var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
            Debug.DrawLine(debugLineStart, debugLineStart + new Vector3(-1, 0, 0), Color.blue);

            if (raycast.collider != null)
            {
                float distance = raycast.point.x - (rayStartPosition.x - nextA.x);
                if (raycast.point.x > rayStartPosition.x + nextA.x)
                {
                    //transform.position = new Vector3(raycast.point.x + _size.x / 2, transform.position.y, transform.position.z);

                    _momentum.x = 0;
                    resultMovement.x = 0;

                    break;
                }
            }
        }

        return resultMovement;
    }

    private Vector2 HandleGravity(Vector2 resultMovement)
    {
        resultMovement.x += G * GravityDirection.x * Time.deltaTime;
        resultMovement.y += G * GravityDirection.y * Time.deltaTime;

        return resultMovement;
    }

    private Vector2 HandleVelocity(Vector2 resultMovement)
    {
        if(_velocity.x > 0)
        {
            if (_momentum.x < _velocity.x)
                resultMovement.x += _aVelocity;
        }

        if (_velocity.x < 0)
        {
            if (_momentum.x > _velocity.x)
                resultMovement.x -= _aVelocity;
        }

        if (_velocity.y > 0)
        {
            if (_momentum.y < _velocity.y)
                resultMovement.y += _aVelocity;
        }

        if (_velocity.y < 0)
        {
            if (_momentum.y > _velocity.y)
                resultMovement.y -= _aVelocity;
        }

        return resultMovement;
    }

    private void HandleFade()
    {
        if (_momentum.x > 0)
        {
            _momentum.x -= _resistance.x;
            if (_momentum.x < 0)
                _momentum.x = 0;
        }

        if (_momentum.x < 0)
        {
            _momentum.x += _resistance.x;
            if (_momentum.x > 0)
                _momentum.x = 0;
        }

        if (_momentum.y > 0)
        {
            _momentum.y -= _resistance.y;
            if (_momentum.y < 0)
                _momentum.y = 0;
        }

        if (_momentum.y < 0)
        {
            _momentum.y += _resistance.y;
            if (_momentum.y > 0)
                _momentum.y = 0;
        }
    }

    public void AddForce(Vector2 direction)
    {
        _momentum += direction;
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }
}
