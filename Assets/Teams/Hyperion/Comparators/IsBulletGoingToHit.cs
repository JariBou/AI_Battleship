using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.Helpers;
using HyperionTeam.SharedVariables;
using UnityEngine;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class IsBulletGoingToHit : Conditional
    {
        public SharedFloat HitTimeTolerance = 0.15f;
        public SharedFloat RangeTolerance = 3f;
        public SharedBulletViewWrapper IncomingBulletView;
        public SharedFloat IncomingBulletAngle;
        
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);

            IncomingBulletView.SetValue(null);

            foreach (BulletView bulletView in gameData.Bullets)
            {
                if (ShootingHelpers.WillHit(spaceShipForOwner, bulletView.Position, bulletView.Velocity, HitTimeTolerance.Value, RangeTolerance.Value))
                {
                    float shipOrientation = spaceShipForOwner.Orientation * Mathf.Deg2Rad;
                    Vector2 orientation = new Vector2(Mathf.Cos(shipOrientation), Mathf.Sin(shipOrientation));
                    float angle = Vector2.Angle(orientation, -bulletView.Velocity);
                    IncomingBulletAngle.SetValue(angle);
                    IncomingBulletView.SetValue(new BulletViewWrapper(bulletView));
                    return TaskStatus.Success; 
                }
            }
            
            return TaskStatus.Failure;
        }
    }
}