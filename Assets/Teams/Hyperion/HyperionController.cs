using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DoNotModify;

namespace HyperionTeam {

	public class HyperionController : BaseSpaceShipController
	{
		private BehaviorTree _behaviorTree;

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
			_behaviorTree = GetComponent<BehaviorTree>();
			_behaviorTree.SetVariableValue("GameData", data);
			_behaviorTree.SetVariableValue("Owner", spaceship.Owner);
			_behaviorTree.SetVariableValue("RemainingTime", data.timeLeft);
			_behaviorTree.SetVariableValue("Orientation", spaceship.Orientation);
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			_behaviorTree.SetVariableValue("RemainingTime", data.timeLeft);
			_behaviorTree.SetVariableValue("Orientation", spaceship.Orientation);

			SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			float thrust = 1.0f;
			float targetRotation = spaceship.Orientation + 90.0f;
			
			// float targetRotation = (float)_behaviorTree.GetVariable("TargetRotation").GetValue();
			bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			bool canHit = (bool)_behaviorTree.GetVariable("CanHit").GetValue();
			return new InputData(thrust, targetRotation, canHit, false, false);
		}
	}

}
