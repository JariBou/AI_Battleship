using System;
using BehaviorDesigner.Runtime;
using DoNotModify;

namespace HyperionTeam.SharedVariables
{
    [Serializable]
    public class SharedBulletViewWrapper : SharedVariable<BulletViewWrapper>
    {
        public static implicit operator SharedBulletViewWrapper(BulletViewWrapper value) { return new SharedBulletViewWrapper { mValue = value }; }

    }
}