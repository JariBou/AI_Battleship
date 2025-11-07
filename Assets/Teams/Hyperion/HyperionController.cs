using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            _behaviorTree.SetVariable("d_ShootCooldown", (SharedFloat)0f);
            _behaviorTree.SetVariable("d_ShockwaveCooldown", (SharedFloat)0f);
			_behaviorTree.SetVariable("d_MineCooldown", (SharedFloat)0f);

            UpdateBlackboardData(spaceship, data);
		}

		public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
			UpdateBlackboardData(spaceship, data);
			SharedVariable shootCooldown = _behaviorTree.GetVariable("d_ShootCooldown");
			SharedVariable shockwaveCooldown = _behaviorTree.GetVariable("d_ShockwaveCooldown");
			SharedVariable mineCooldown = _behaviorTree.GetVariable("d_MineCooldown");
			
			foreach (SharedVariable key in new []{shootCooldown, shockwaveCooldown, mineCooldown})
			{
				if ((float)key.GetValue() > 0f)
				{
					key.SetValue((float)key.GetValue() - Time.deltaTime);
				}
			}
			
            SpaceShipView otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
			Vector2 closestWaypoint = WaypointPathingHelper.Instance.GetClosestWaypoint(spaceship.Position, spaceship.Owner);
			//Debug.Log($"Vector: {closestWaypoint}");
			// float targetRotation = spaceship.Orientation + 90.0f;

			// (float)_behaviorTree.GetVariable("o_ShootCooldown");
			
			// needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
			bool wantsToShoot = (bool)_behaviorTree.GetVariable("i_CanHit").GetValue();
			bool wantsToShockwave = (bool)_behaviorTree.GetVariable("i_WantsToShockwave").GetValue();
			bool wantsToMine = (bool)_behaviorTree.GetVariable("i_WantsToMine").GetValue();
            float thrust = (float)_behaviorTree.GetVariable("i_Thrust").GetValue();
            float targetOrient = (float)_behaviorTree.GetVariable("i_TargetOrientation").GetValue();

            if (wantsToShoot)
            {
	            shootCooldown.SetValue(spaceship.StunPenaltyDuration);
            }
            if (wantsToShockwave)
            {
	            shockwaveCooldown.SetValue(1f);
            }
            if (wantsToMine)
            {
	            mineCooldown.SetValue(1f);
            }
            return new InputData(thrust, targetOrient, wantsToShoot, wantsToMine, wantsToShockwave);
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
			_behaviorTree.SetVariableValue("o_EnemyPosition", otherSpaceship.Position);
			_behaviorTree.SetVariableValue("o_EnemySpeed", otherSpaceship.Velocity);
			_behaviorTree.SetVariableValue("o_EnemyShot", otherSpaceship.HasShot);
			_behaviorTree.SetVariableValue("o_EnemyEnergy", otherSpaceship.Energy);
				
			_behaviorTree.SetVariableValue("d_ShootCooldown", 0f);
			_behaviorTree.SetVariableValue("d_ShockwaveCooldown", 0f);
			_behaviorTree.SetVariableValue("d_MineCooldown", 0f);
			
			_behaviorTree.SetVariableValue("o_WaypointsCount", data.WayPoints.Count);
			_behaviorTree.SetVariableValue("o_CapturedWaypointsCount", data.WayPoints.Count(t => t.Owner == spaceship.Owner));
		}
	}

}
