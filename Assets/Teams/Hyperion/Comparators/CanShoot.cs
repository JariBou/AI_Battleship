using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.SharedVariables;

namespace HyperionTeam.Comparators
{
    public class CanShoot : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("GameData").GetValue();
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);
            SpaceShipView spaceShipTarget = gameData.GetSpaceShipForOwner(1 - ownerId);

            bool canHit = AimingHelpers.CanHit(spaceShipForOwner, spaceShipTarget.Position, spaceShipTarget.Velocity, 0.15f);

            return canHit ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}