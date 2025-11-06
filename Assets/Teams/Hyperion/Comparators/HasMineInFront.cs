using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.Helpers;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class HasMineInFront : Conditional
    {
        public SharedFloat Angle;
        public SharedFloat AngleTolerance = 5f;
        
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);

            foreach (MineView gameDataMine in gameData.Mines)
            {
                if (ShootingHelpers.CanHit(spaceShipForOwner, gameDataMine.Position, out float angle) && angle < AngleTolerance.Value)
                {
                    Angle.SetValue(angle);
                    return TaskStatus.Success;
                }
            }
          
            return TaskStatus.Failure;
        }
    }
}