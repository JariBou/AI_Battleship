using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class CanMoveToNext : Conditional
    {
        public SharedVector2 Target;
        
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            Vector2 spaceshipPosition = (Vector2)Owner.GetVariable("o_SpaceshipPosition").GetValue();

            Vector2 closestWaypoint = WaypointPathingHelper.Instance.GetClosestWaypoint(spaceshipPosition, ownerId);
            Target.SetValue(closestWaypoint);
            
            if (closestWaypoint != spaceshipPosition)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}