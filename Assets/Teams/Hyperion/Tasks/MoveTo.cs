using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.SharedVariables;
using Microsoft.Win32;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public class MoveTo : Action
{
	public SharedGameData gameData;
	public SharedVector2 targetRotation;

	private SharedTransform _target;
	private SharedFloat _raycastRange;
    private SharedLayerMask _layerMask;
    private SharedVector2 _shipPosition;

    private Vector3 _dirForwardRight = new Vector3(-0.75f, 0.75f, 0f);
    private Vector3 _dirForwardLeft = new Vector3(-0.75f, -0.75f, 0f);

    public override void OnAwake()
	{
		base.OnAwake();
	}

	public override void OnStart()
	{
		_target = (SharedTransform)Owner.GetVariable("Target");
		_raycastRange = (SharedFloat)Owner.GetVariable("RaycastDodgingRange");
		_layerMask = (SharedLayerMask)Owner.GetVariable("AsteroidMask");
        _shipPosition = (SharedVector2)Owner.GetVariable("o_ShipPosition");

        base.OnStart();
	}

	public override TaskStatus OnUpdate()
	{
        _shipPosition = (SharedVector2)Owner.GetVariable("o_ShipPosition");

        RaycastHit hitForwardRight;
		bool isHitForwardRight = Physics.Raycast(_shipPosition.Value, _dirForwardRight, out hitForwardRight, _raycastRange.Value, _layerMask.Value);
        if(isHitForwardRight) Debug.DrawRay(_shipPosition.Value, _dirForwardRight    * _raycastRange.Value, Color.green);
		else Debug.DrawRay(_shipPosition.Value, _dirForwardRight * _raycastRange.Value, Color.red);

        RaycastHit hitForwadLeft;
        bool isHitForwardLeft = Physics.Raycast(_shipPosition.Value, _dirForwardLeft, out hitForwadLeft, _raycastRange.Value, _layerMask.Value);
        if (isHitForwardLeft) Debug.DrawRay(_shipPosition.Value, _dirForwardLeft * _raycastRange.Value, Color.green);
        else Debug.DrawRay(_shipPosition.Value, _dirForwardLeft * _raycastRange.Value, Color.blue);

        RaycastHit hitForward;
        bool isHitForward = Physics.Raycast(_shipPosition.Value, transform.right * -1, out hitForward, _raycastRange.Value, _layerMask.Value);
        if (isHitForward) Debug.DrawRay(_shipPosition.Value, transform.right * -1 * _raycastRange.Value, Color.green);
        else Debug.DrawRay(_shipPosition.Value, transform.right * -1 * _raycastRange.Value, Color.magenta);

        RaycastHit hitRight;
        bool isHitRight = Physics.Raycast(_shipPosition.Value, transform.up, out hitRight, _raycastRange.Value, _layerMask.Value);
        if (isHitRight) Debug.DrawRay(_shipPosition.Value, transform.up * _raycastRange.Value, Color.green);
        else Debug.DrawRay(_shipPosition.Value, transform.up * _raycastRange.Value, Color.cyan); 

        RaycastHit hitLeft;
        bool isHitLeft = Physics.Raycast(_shipPosition.Value, transform.up * -1, out hitLeft, _raycastRange.Value, _layerMask.Value);
        if (isHitLeft) Debug.DrawRay(_shipPosition.Value, transform.up * -1 * _raycastRange.Value, Color.green);
        else Debug.DrawRay(_shipPosition.Value, transform.up * -1 * _raycastRange.Value, Color.yellow);

        return TaskStatus.Success;
	}
} 