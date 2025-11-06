using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.SharedVariables;
using UnityEngine;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class IsCloseToCP : Conditional
    {
        public SharedFloat inDistanceTolerance;
        public SharedBool inIgnoreCaptured;
        public SharedFloat outDistance;
        public SharedWaypointView outClosestWaypoint;
        
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            Vector2 spaceshipPosition = (Vector2)Owner.GetVariable("o_ShipPosition").GetValue();

            float distance = float.MaxValue;
            WayPointView waypoint = null;
            foreach (WayPointView gameDataWayPoint in gameData.WayPoints)
            {
                if (gameDataWayPoint.Owner == ownerId && inIgnoreCaptured.Value) continue;
                float magnitude = (gameDataWayPoint.Position - spaceshipPosition).magnitude;
                if (magnitude < distance && magnitude < inDistanceTolerance.Value)
                {
                    distance = magnitude;
                    waypoint = gameDataWayPoint;
                }
            }

            if (waypoint == null)
            {
                return TaskStatus.Failure;
            }
            
            outDistance.SetValue(distance);
            outClosestWaypoint.SetValue(waypoint);

            return TaskStatus.Success;
        }
    }
}