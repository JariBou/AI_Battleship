using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace HyperionTeam.Tasks
{
    [TaskCategory("Hyperion/Tasks")]
    public class TurnTowards : Action
    {
        public SharedVector2 TargetPosition;
        public SharedVector2 TargetSpeed;
        public SharedFloat OvershootAmount;
        
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            SharedVariable targetRotation = Owner.GetVariable("i_TargetOrientation");
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);

            Vector2 directionVec = new (Mathf.Cos(spaceShipForOwner.Orientation * Mathf.Deg2Rad), Mathf.Sin(spaceShipForOwner.Orientation * Mathf.Deg2Rad));

            Vector2 toTargetVec = TargetPosition.Value + TargetSpeed.Value * OvershootAmount.Value - spaceShipForOwner.Position;
            
            float deltaAngle = Vector2.SignedAngle(directionVec, toTargetVec);
            
            float overshootFactor = 1.1f;
            deltaAngle *= overshootFactor;
            float angle = deltaAngle;
            targetRotation.SetValue(spaceShipForOwner.Orientation + angle);
            return TaskStatus.Success;
        }
    }
}