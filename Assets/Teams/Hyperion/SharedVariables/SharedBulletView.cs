using System;
using BehaviorDesigner.Runtime;
using DoNotModify;

namespace HyperionTeam.SharedVariables
{
    [Serializable]
    public class SharedBulletView : SharedVariable<BulletView>
    {
        public static implicit operator SharedBulletView(BulletView value) { return new SharedBulletView { mValue = value }; }
    }
}