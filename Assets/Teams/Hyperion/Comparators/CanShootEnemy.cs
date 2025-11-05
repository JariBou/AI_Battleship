using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.Helpers;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class CanShootEnemy : Conditional
    {
        public SharedFloat HitTimeTolerance = 0.15f;
        public SharedFloat MinEnergyLevel = 0.5f;
        public SharedFloat AngleTolerance = 5f;
        
        public override TaskStatus OnUpdate()
        {
            if ((float)Owner.GetVariable("d_ShootCooldown").GetValue() > 0f)
            {
                return TaskStatus.Failure;
            }
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);
            if (spaceShipForOwner.Energy <= MinEnergyLevel.Value)
            {
                return TaskStatus.Failure;
            }
            SpaceShipView spaceShipTarget = gameData.GetSpaceShipForOwner(1 - ownerId);

            bool canHit;
            if (spaceShipTarget.Velocity.sqrMagnitude == 0)
            {
                canHit = ShootingHelpers.CanHit(spaceShipForOwner, spaceShipTarget.Position, AngleTolerance.Value);
            }
            else
            {
                canHit = ShootingHelpers.CanHit(spaceShipForOwner,
                             spaceShipTarget.Position,
                             spaceShipTarget.Velocity,
                             HitTimeTolerance.Value) ||
                         ShootingHelpers.CanHit(spaceShipForOwner,
                             spaceShipTarget.Position,
                             AngleTolerance.Value);
            }

            return canHit ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}