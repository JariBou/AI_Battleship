using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.SharedVariables;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class CanShoot : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);
            SpaceShipView spaceShipTarget = gameData.GetSpaceShipForOwner(1 - ownerId);

            bool canHit;
            if (spaceShipTarget.Velocity.sqrMagnitude == 0)
            {
                canHit = AimingHelpers.CanHit(spaceShipForOwner, spaceShipTarget.Position, 5);
            }
            else
            {
                canHit = AimingHelpers.CanHit(spaceShipForOwner,
                             spaceShipTarget.Position,
                             spaceShipTarget.Velocity,
                             0.15f) ||
                         AimingHelpers.CanHit(spaceShipForOwner,
                             spaceShipTarget.Position,
                             5);
            }

            return canHit ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}