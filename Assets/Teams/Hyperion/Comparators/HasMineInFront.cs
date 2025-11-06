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
        public SharedFloat MaxDistanceAlowed;
        public SharedFloat MinDistanceAlowed;
        public SharedFloat AngleTolerance = 5f;
        
        public override TaskStatus OnUpdate()
        {
            int ownerId = (int)Owner.GetVariable("o_Owner").GetValue();
            GameData gameData = (GameData)Owner.GetVariable("o_GameData").GetValue();
            SpaceShipView spaceShipForOwner = gameData.GetSpaceShipForOwner(ownerId);

            foreach (MineView gameDataMine in gameData.Mines)
            {
                float magnitude = (gameDataMine.Position - spaceShipForOwner.Position).magnitude;
                if (magnitude > MaxDistanceAlowed.Value || magnitude < MinDistanceAlowed.Value) continue;
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