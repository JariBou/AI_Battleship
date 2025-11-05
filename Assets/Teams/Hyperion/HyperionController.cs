using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using DoNotModify;

namespace HyperionTeam {

	public class HyperionController : BaseSpaceShipController
	{
		private BehaviorTree _behaviorTree;
		private Dictionary<string, float> _actionCooldowns = new();

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
            WaypointPathingHelper.Instance.Initialize(data);
			_behaviorTree = GetComponent<BehaviorTree>();
			_behaviorTree.SetVariableValue("o_GameData", data);
			_behaviorTree.SetVariableValue("o_Owner", spaceship.Owner);
			_actionCooldowns.Add("shoot", 0f);
			_actionCooldowns.Add("mine", 0f);
			_actionCooldowns.Add("shockwave", 0f);
			UpdateBlackboardData(spaceship, data);
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			UpdateBlackboardData(spaceship, data);

			foreach (string key in _actionCooldowns.Keys)
			{
				if (_actionCooldowns[key] > 0f)
				{
					_actionCooldowns[key] -= Time.deltaTime;
				}
			}
			
            SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			Vector2 closestWaypoint = WaypointPathingHelper.Instance.GetClosestWaypoint(spaceship.Position, spaceship.Owner);
			Debug.Log($"Vector: {closestWaypoint}");
			// float targetRotation = spaceship.Orientation + 90.0f;
			
			bool canShoot = _actionCooldowns["shoot"] <= 0f;
			
			// needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			bool canHit = (bool)_behaviorTree.GetVariable("i_CanHit").GetValue();
            float thrust = (float)_behaviorTree.GetVariable("i_Thrust").GetValue();
            float targetOrient = (float)_behaviorTree.GetVariable("i_TargetOrientation").GetValue();

            bool shouldShoot = canHit && canShoot;
            if (shouldShoot)
            {
	            _actionCooldowns["shoot"] = spaceship.StunPenaltyDuration;
            }
            return new InputData(thrust, targetOrient, shouldShoot, false, false);
		}

		private void UpdateBlackboardData(SpaceShipView spaceship, GameData data)
		{
            SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			
            _behaviorTree.SetVariableValue("o_RemainingTime", data.timeLeft);
			_behaviorTree.SetVariableValue("o_Orientation", spaceship.Orientation);
			_behaviorTree.SetVariableValue("o_ShipPosition", spaceship.Position);
			_behaviorTree.SetVariableValue("o_CurrentEnergy", spaceship.Energy);
			_behaviorTree.SetVariableValue("o_CurrentScore", GameManager.Instance.GetScoreForPlayer(spaceship.Owner));
			_behaviorTree.SetVariableValue("o_DistanceToEnemy", (spaceship.Position - otherSpaceship.Position).magnitude);
			_behaviorTree.SetVariableValue("o_EnemyEnergy", otherSpaceship.Energy);
		}
	}

}
