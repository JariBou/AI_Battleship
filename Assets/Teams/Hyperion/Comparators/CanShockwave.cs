using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using HyperionTeam.SharedVariables;

namespace HyperionTeam.Comparators
{
    [TaskCategory("Hyperion")]
    public class CanShockwave : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            float distanceToEnemy = (float)Owner.GetVariable("o_DistanceToEnemy").GetValue();
            return distanceToEnemy < 2.2f ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}