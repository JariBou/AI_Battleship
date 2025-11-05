using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion;
using DoNotModify;
using HyperionTeam.SharedVariables;
using System;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
using Random = UnityEngine.Random;

public class MoveTo : Action
{
	public SharedVector2 targetRotation;
    public SharedFloat thrust;
    public SharedFloat _rotationSpeed;

	public SharedVector2 _target;
	private SharedFloat _raycastRange;
    private SharedLayerMask _layerMask;
    private SharedVector2 _shipPosition;
    private SharedFloat _shipOrientation;

    public GameData gameData;
    private SpaceShipView spaceShipForOwner;

    //private Vector3 _dirForwardRight = new Vector3(-0.75f, 0.75f, 0f);
    //private Vector3 _dirForwardLeft = new Vector3(-0.75f, -0.75f, 0f);

    public override void OnAwake()
	{
        base.OnAwake();
	}

	public override void OnStart()
	{
        _target = (SharedVector2)Owner.GetVariable("Target");
        //_target = GameObject.Find("WayPoint (9)").transform;
		_raycastRange = (SharedFloat)Owner.GetVariable("RaycastDodgingRange");
		_layerMask = (SharedLayerMask)Owner.GetVariable("AsteroidMask");
        _shipPosition = (SharedVector2)Owner.GetVariable("o_ShipPosition");
        _shipOrientation = (SharedFloat)Owner.GetVariable("o_Orientation");
        int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
        gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
        spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);

        base.OnStart();
	}

	public override TaskStatus OnUpdate()
	{
        _shipPosition = (SharedVector2)Owner.GetVariable("o_ShipPosition");
        _shipOrientation = (SharedFloat)Owner.GetVariable("o_Orientation");

        //float angle = Vector2.Angle((Vector2)_target.Value.position, _shipPosition.Value);

        //Owner.SetVariable("i_TargetOrientation", (SharedFloat)(_shipOrientation.Value + angle));

        float forwardAngle = Mathf.Deg2Rad * _shipOrientation.Value;
        Vector2 forward = new Vector2(Mathf.Cos(forwardAngle), Mathf.Sin(forwardAngle));
        Vector2 right = new Vector2(Mathf.Cos(forwardAngle + 90 * Mathf.Deg2Rad), Mathf.Sin(forwardAngle + 90 * Mathf.Deg2Rad));
        Vector2 left = new Vector2(Mathf.Cos(forwardAngle - 90 * Mathf.Deg2Rad), Mathf.Sin(forwardAngle - 90 * Mathf.Deg2Rad));
        Vector2 forwardRight = new Vector2(Mathf.Cos(forwardAngle + 45 * Mathf.Deg2Rad), Mathf.Sin(forwardAngle + 45 * Mathf.Deg2Rad));
        Vector2 forwardLeft = new Vector2(Mathf.Cos(forwardAngle - 45 * Mathf.Deg2Rad), Mathf.Sin(forwardAngle - 45 * Mathf.Deg2Rad));

        float shortestDist = float.MaxValue;
        RaycastWrapper closestRaycast = new RaycastWrapper();

        RaycastHit2D hitForwardRight = Physics2D.Raycast(_shipPosition.Value, forwardRight, _raycastRange.Value, _layerMask.Value);
        if (hitForwardRight)
        {
            Debug.DrawRay(_shipPosition.Value, forwardRight * _raycastRange.Value, Color.green);

            if(hitForwardRight.distance < shortestDist) 
            {
                shortestDist = hitForwardRight.distance;
                closestRaycast = new RaycastWrapper(hitForwardRight, -45);
            }
        }
        else Debug.DrawRay(_shipPosition.Value, forwardRight * _raycastRange.Value, Color.red);


        RaycastHit2D hitForwardLeft = Physics2D.Raycast(_shipPosition.Value, forwardLeft, _raycastRange.Value, _layerMask.Value);
        if (hitForwardLeft)
        {
            Debug.Log((SharedFloat)(_shipOrientation.Value + _rotationSpeed.Value));

            if (hitForwardLeft.distance < shortestDist)
            {
                shortestDist = hitForwardLeft.distance;
                closestRaycast = new RaycastWrapper(hitForwardLeft, 45);
            }
        }
        else Debug.DrawRay(_shipPosition.Value, forwardLeft * _raycastRange.Value, Color.blue);


        RaycastHit2D hitForward = Physics2D.Raycast(_shipPosition.Value, forward, _raycastRange.Value, _layerMask.Value);
        if (hitForward)
        {
            Debug.DrawRay(_shipPosition.Value, forward * _raycastRange.Value, Color.green);

            if (hitForward.distance < shortestDist)
            {
                shortestDist = hitForward.distance;

                float rand = Random.Range(0.0f, 1.0f);
                float angle = rand > 0.5f ? 90 : -90;

                closestRaycast =  new RaycastWrapper(hitForward, angle);
            }
        }
        else Debug.DrawRay(_shipPosition.Value, forward * _raycastRange.Value, Color.magenta);

        RaycastHit2D hitRight = Physics2D.Raycast(_shipPosition.Value, right, _raycastRange.Value, _layerMask.Value);
        if (hitRight)
        {
            Debug.DrawRay(_shipPosition.Value, right * _raycastRange.Value, Color.green);

            if (hitRight.distance < shortestDist)
            {
                shortestDist = hitRight.distance;
                closestRaycast = new RaycastWrapper(hitRight, -90);
            }
        }
        else Debug.DrawRay(_shipPosition.Value, right * _raycastRange.Value, Color.cyan);

        RaycastHit2D hitLeft = Physics2D.Raycast(_shipPosition.Value, left, _raycastRange.Value, _layerMask.Value);
        if (hitLeft)
        {
            Debug.DrawRay(_shipPosition.Value, left * _raycastRange.Value, Color.green);

            if (hitLeft.distance < shortestDist)
            {
                shortestDist = hitLeft.distance;
                closestRaycast = new RaycastWrapper(hitLeft, 90);
            }
        }
        else Debug.DrawRay(_shipPosition.Value, left * _raycastRange.Value, Color.yellow);

            Debug.Log(closestRaycast.IsValid);

        if (closestRaycast.IsValid)
        {
            Owner.SetVariableValue("i_TargetOrientation", _shipOrientation.Value + closestRaycast.Angle);
            thrust = 0;
        }

        if (!hitForwardRight && !hitForwardLeft && !hitForward && !hitRight && !hitLeft)
        {
            float angle = Vector2.SignedAngle(Vector2.right, (Vector2)_target.Value - _shipPosition.Value);
            Debug.Log(angle);
            Owner.SetVariable("i_TargetOrientation", (SharedFloat)AimingHelpers.ComputeSteeringOrient(spaceShipForOwner, _target.Value));
            thrust = 1;
        }

        Owner.SetVariable("i_Thrust", thrust);

        return TaskStatus.Success;
	}

    private class RaycastWrapper
    {
        public bool IsValid { get; private set; }

        public RaycastHit2D Target { get; private set; }

        public float Angle { get; private set; }

        public RaycastWrapper()
        {
            IsValid = false;
        }

        public RaycastWrapper(RaycastHit2D target, float angle)
        {
            Target = target;
            IsValid = true;
            Angle = angle;
        }
    }
} 