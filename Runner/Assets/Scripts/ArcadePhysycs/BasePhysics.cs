using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePhysics : MonoBehaviour {

    #region public params
    public float G = 0.15f;
    public Vector2 GravityDirection = Vector2.down;
    public bool OnGround = false;
    public float AirResistance = 0.0001f;
    public float FloorResistance = 0.001f;
    #endregion

    #region private params
    //private float Skin = 0.001f;
    protected float MinRaycastDistance = 0.33f;
    protected Vector2 _momentum = new Vector2(0, 0);
    protected Vector2 _velocity = new Vector2(0, 0);
    protected float _stairHeight = 0.2f;
    protected float _aVelocity = 0.002f;
    protected Vector2 _resistance;
    protected Vector2 _size;
    protected int _horizontalRasycastCount;
    protected int _verticalRasycastCount;
    public bool _forceVelocity = false;
    #endregion

    void Start()
    {
        _size = GetComponent<BoxCollider2D>().size;
        _horizontalRasycastCount = (int)(_size.y / MinRaycastDistance) + 1;
        _verticalRasycastCount = (int)(_size.x / MinRaycastDistance) + 1;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _forceVelocity = false;
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
        var wasOnGround = OnGround;
        OnGround = false;
        _resistance = new Vector2(AirResistance, AirResistance);

        var hitedVerticalRaycasts = new List<RaycastHit2D>();
        var unhitedVerticalRaycasts = new List<RaycastHit2D>();

        for (int i = 0; i < _verticalRasycastCount; i++)
        {
            var rayStartPosition = new Vector2(0, 0);
            rayStartPosition.x = transform.position.x - _size.x / 2 + i * MinRaycastDistance;
            rayStartPosition.y = transform.position.y + (_size.y / 2) * Mathf.Sign(nextA.y);

            var raycast = Physics2D.Raycast(rayStartPosition,
                                                    new Vector2(0, Mathf.Sign(nextA.y)),
                                                    2f,
                                                    LayerMask.GetMask("Floor"));

            if (raycast.collider != null && raycast.point.y != 0.0f &&
                ((nextA.y < 0) && (raycast.point.y > rayStartPosition.y + nextA.y)) || 
                ((nextA.y > 0) && (raycast.point.y < rayStartPosition.y + nextA.y)))
            {
                if (nextA.y < 0 || raycast.point.y != 0.0f)
                    hitedVerticalRaycasts.Add(raycast);
                else
                    unhitedVerticalRaycasts.Add(raycast);
            }                
            else if (raycast.collider != null)
                unhitedVerticalRaycasts.Add(raycast);

            var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
            Debug.DrawLine(debugLineStart, debugLineStart + new Vector3(0, Mathf.Sign(nextA.y) * 2, 0), Color.green);
        }

        if (nextA.y > 0 && hitedVerticalRaycasts.Count > 0)
            Debug.Log(hitedVerticalRaycasts[0].point.y);

        OnGround = false;

        if (nextA.y < 0 && hitedVerticalRaycasts.Count > 0)
        {

            RaycastHit2D theNearestToFloor = hitedVerticalRaycasts[0];
            float highestPoint = theNearestToFloor.point.y;

            foreach (var raycast in hitedVerticalRaycasts)
            {
                if (highestPoint < raycast.point.y)
                {
                    highestPoint = raycast.point.y;
                    theNearestToFloor = raycast;
                }
            }

            var deepRayStartPosition = new Vector2(0, 0);
            deepRayStartPosition.x = theNearestToFloor.point.x;
            deepRayStartPosition.y = transform.position.y + _size.y / 2;

            var deepRaycast = Physics2D.Raycast(deepRayStartPosition,
                                            new Vector2(0, -_size.y + 0.1f),
                                            10f,
                                            LayerMask.GetMask("Floor"));

            var debugDeepLineStart = new Vector3(deepRayStartPosition.x, deepRayStartPosition.y, 0);
            Debug.DrawLine(debugDeepLineStart, debugDeepLineStart + new Vector3(0, -_size.y + 0.1f, 0), Color.cyan);

            if (deepRaycast.collider != null)
            {
                transform.position = new Vector3(transform.position.x, deepRaycast.point.y + _size.y / 2, transform.position.z);
            }

            _momentum.y = 0;
            OnGround = true;
            resultMovement.y = 0;
            _resistance.x = FloorResistance;
        }
        else if (nextA.y < 0 && hitedVerticalRaycasts.Count == 0 && wasOnGround && unhitedVerticalRaycasts.Count > 0)
        {
            RaycastHit2D theNearestToFloor = unhitedVerticalRaycasts[0];
            float highestPoint = theNearestToFloor.point.y;

            foreach (var raycast in unhitedVerticalRaycasts)
            {
                if (highestPoint < raycast.point.y)
                {
                    highestPoint = raycast.point.y;
                    theNearestToFloor = raycast;
                }
            }

            if (highestPoint > transform.position.y + nextA.y - 1)
            {
                transform.position = new Vector3(transform.position.x, theNearestToFloor.point.y + _size.y / 2, transform.position.z);

                _momentum.y = 0;
                OnGround = true;
                resultMovement.y = 0;
                _resistance.x = FloorResistance;

            }
        }
        else if (nextA.y > 0 && hitedVerticalRaycasts.Count > 0)
        {
            _momentum.y = 0;
            OnGround = true;
            resultMovement.y = 0;
        }


        #region right
        if (nextA.x > 0)
        {
            for (int i = 1; i < _horizontalRasycastCount; i++)
            {
                var rayStartPosition = new Vector2(0, 0);
                rayStartPosition.x = transform.position.x + _size.x / 2;
                rayStartPosition.y = transform.position.y - _size.y / 2 + i * MinRaycastDistance + 0.001f;

                var raycast = Physics2D.Raycast(rayStartPosition,
                                                new Vector2(1, 0),
                                                10f,
                                                LayerMask.GetMask("Floor"));

                var debugLineStart = new Vector3(rayStartPosition.x, rayStartPosition.y, 0);
                Debug.DrawLine(debugLineStart, debugLineStart + new Vector3(1, 0, 0), Color.blue);

                if (raycast.collider != null)
                {
                    float distance = raycast.point.x - (rayStartPosition.x + nextA.x); //TODO: Extra param
                    if (raycast.point.x < rayStartPosition.x + nextA.x)
                    {
                        //transform.position = new Vector3(raycast.point.x + _size.x / 2, transform.position.y, transform.position.z);
                        
                        _momentum.x = 0;
                        resultMovement.x = 0;

                        break;

                    }
                }
            }
        }
        #endregion

        #region left
        if (nextA.x < 0)
        {
            for (int i = 1; i < _horizontalRasycastCount; i++)
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
        }
        #endregion

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
        if(!_forceVelocity)
            _velocity = velocity;
    }

    public Vector2 GetMomentum()
    {
        return _momentum;
    }
}
