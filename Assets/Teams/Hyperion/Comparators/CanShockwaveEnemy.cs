using BehaviorDesigner.Runtime.Tasks;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class CanShockwaveEnemy : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if ((float)Owner.GetVariable("d_ShockwaveCooldown").GetValue() > 0)
            {
                return TaskStatus.Failure;
            }
            float distanceToEnemy = (float)Owner.GetVariable("o_DistanceToEnemy").GetValue();
            return distanceToEnemy < 2.2f ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}