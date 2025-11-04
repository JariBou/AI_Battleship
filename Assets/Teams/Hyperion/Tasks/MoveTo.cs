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
	public SharedFloat targetRotation;

	public override void OnAwake()
	{
		base.OnAwake();
	}

	public override void OnStart()
	{
		// Owner.GetVariable("Owner");
		base.OnStart();
	}

	public override TaskStatus OnUpdate()
	{
		Owner.GetVariable("Owner");
		return TaskStatus.Success;
	}
}