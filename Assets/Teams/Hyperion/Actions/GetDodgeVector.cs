using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.SharedVariables;
using UnityEngine;

namespace HyperionTeam.Actions
{
    [TaskCategory("Hyperion")]
    public class GetDodgeVector : Action
    {
        public SharedVector2 DodgeVector;
        public SharedBulletView BulletView;

        public override TaskStatus OnUpdate()
        {
            Vector2 vector = new();
            
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);

            Vector2 perpendicular = Vector2.Perpendicular(spaceShipForOwner.Velocity) * (Random.Range(0, 100)>50 ? -1 : 1);
            DodgeVector.SetValue(perpendicular);
            
            return TaskStatus.Success;
        }
    }
}