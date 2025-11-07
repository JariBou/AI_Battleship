using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;
using Random = UnityEngine.Random;

public class MoveTo : Action
{
	public SharedVector2 targetRotation;
    public SharedFloat thrust;
    public SharedFloat _rotationSpeed;

	public SharedVector2 Target;
	private SharedFloat _raycastRange;
    private SharedLayerMask _layerMask;
    private SharedVector2 _shipPosition;
    private SharedFloat _shipOrientation;

    public GameData gameData;
    private SpaceShipView spaceShipForOwner;

    public override void OnAwake()
	{
        base.OnAwake();
	}

	public override void OnStart()
	{
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

        float forwardAngle = Mathf.Deg2Rad * _shipOrientation.Value;
        Vector2 forward = new Vector2(Mathf.Cos(forwardAngle), Mathf.Sin(forwardAngle));
        Vector2 middleForwardRight = new Vector2(Mathf.Cos(forwardAngle + 22.5f * Mathf.Deg2Rad), Mathf.Sin(forwardAngle + 22.5f * Mathf.Deg2Rad));
        Vector2 middleForwardLeft = new Vector2(Mathf.Cos(forwardAngle - 22.5f * Mathf.Deg2Rad), Mathf.Sin(forwardAngle - 22.5f * Mathf.Deg2Rad));
        Vector2 forwardRight = new Vector2(Mathf.Cos(forwardAngle + 45 * Mathf.Deg2Rad), Mathf.Sin(forwardAngle + 45 * Mathf.Deg2Rad));
        Vector2 forwardLeft = new Vector2(Mathf.Cos(forwardAngle - 45 * Mathf.Deg2Rad), Mathf.Sin(forwardAngle - 45 * Mathf.Deg2Rad));

        Vector2 targetRelativePos = Target.Value - _shipPosition.Value;

        RaycastHit2D circleCast = Physics2D.CircleCast(_shipPosition.Value, spaceShipForOwner.Radius * 1.1f, forward, _raycastRange.Value, _layerMask.Value);
        if (circleCast)
        {
            Debug.DrawLine(_shipPosition.Value, _shipPosition.Value + (forward * _raycastRange.Value), Color.green, Time.deltaTime);

            // Debug.Log(circleCast.transform.gameObject.name);

            float angleNormalRaycast = Vector2.SignedAngle(circleCast.normal, forward * spaceShipForOwner.Radius);

            float angle = 0.0f;

            if (angleNormalRaycast > 0.0f)
            {
                angle = -90;
            }
            else if (angleNormalRaycast < 0.0f)
            {
                angle = 90;
            }
            else if (angleNormalRaycast == 0.0f)
            {
                float rand = Random.Range(0.0f, 1.0f);
                angle = rand > 0.5f ? 90 : -90;
            }

            Owner.SetVariableValue("i_TargetOrientation", _shipOrientation.Value + angle);
        }
        else
        {
            Debug.DrawLine(_shipPosition.Value, _shipPosition.Value + (forward * _raycastRange.Value), Color.red, Time.deltaTime);

            float angle = Vector2.SignedAngle(Vector2.right, targetRelativePos);
            //Debug.Log(angle);
            Owner.SetVariable("i_TargetOrientation", (SharedFloat)AimingHelpers.ComputeSteeringOrient(spaceShipForOwner, Target.Value, 1.5f));
        }


        /*if (!hitForwardRight && !hitForwardLeft *//*&& !hitForward*//* && !hitmiddleForwardRight && !hitmiddleForwardLeft)
        {
            float angle = Vector2.SignedAngle(Vector2.right, targetRelativePos);
            //Debug.Log(angle);
            Owner.SetVariable("i_TargetOrientation", (SharedFloat)AimingHelpers.ComputeSteeringOrient(spaceShipForOwner, Target.Value));
        }*/

        bool isInFront = Vector2.Dot(forward, targetRelativePos) > 0.0f;

        if (isInFront)
        {
            float angle = Vector2.Angle(forward, targetRelativePos);
            float distance = Vector2.Distance(_shipPosition.Value, Target.Value);

            if (distance < 3.0f) 
            {
                thrust = 1 - (angle + distance) / (120 + 3.0f);
            }
            else
            {
                thrust = 1 - angle / 120;
            }
        }
        else
        {
            thrust = 0.0f;
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