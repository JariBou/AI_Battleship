using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.SharedVariables;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class MoveTo : Action
{
	public SharedGameData gameData;
	public SharedVector2 targetRotation;

	private SharedTransform _target;
	private SharedFloat _raycastRange;
    private SharedLayerMask _layerMask;

    public override void OnAwake()
	{
		base.OnAwake();
	}

	public override void OnStart()
	{
		_target = (SharedTransform)Owner.GetVariable("Target");
		_raycastRange = (SharedFloat)Owner.GetVariable("RaycastDodgingRange");
		_layerMask = (SharedLayerMask)Owner.GetVariable("AsteroidMask");

        base.OnStart();
	}

	public override TaskStatus OnUpdate()
	{
		RaycastHit hitForward;
		bool isHitForward = Physics.Raycast(transform.position, transform.forward, out hitForward, _raycastRange.Value, _layerMask.Value);
        if(isHitForward)Debug.DrawRay(transform.position, transform.forward * _raycastRange.Value, Color.red);
		else Debug.DrawRay(transform.position, transform.forward * _raycastRange.Value, Color.green);

        RaycastHit hitRight;
        bool isHitRight = Physics.Raycast(transform.position, transform.right, out hitRight, _raycastRange.Value, _layerMask.Value);
        if (isHitRight) Debug.DrawRay(transform.position, transform.right * _raycastRange.Value, Color.red);
        else Debug.DrawRay(transform.position, transform.right * _raycastRange.Value, Color.green);

        RaycastHit hitLeft;
        bool isHitLeft = Physics.Raycast(transform.position, transform.right * -1, out hitLeft, _raycastRange.Value, _layerMask.Value);
        if (isHitLeft) Debug.DrawRay(transform.position, transform.right * -1 * _raycastRange.Value, Color.red);
        else Debug.DrawRay(transform.position, transform.right * -1 * _raycastRange.Value, Color.green);

        Vector3 dir = new Vector3(0, 0, 45);

        RaycastHit hitForwardRight;
        bool isHitForwardRight = Physics.Raycast(transform.position, dir, out hitForwardRight, _raycastRange.Value, _layerMask.Value);
        if (isHitForwardRight) Debug.DrawRay(transform.position, dir * _raycastRange.Value, Color.red);
        else Debug.DrawRay(transform.position, dir * _raycastRange.Value, Color.green);

        RaycastHit hitForwadLeft;
        bool isHitForwardLeft = Physics.Raycast(transform.position, dir * -1, out hitForwadLeft, _raycastRange.Value, _layerMask.Value);
        if (isHitForwardLeft) Debug.DrawRay(transform.position, dir * -1 * _raycastRange.Value, Color.red);
        else Debug.DrawRay(transform.position, dir * -1 * _raycastRange.Value, Color.green);

        return TaskStatus.Success;
	}
} 