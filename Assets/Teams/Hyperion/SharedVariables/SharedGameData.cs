using System;
using BehaviorDesigner.Runtime;
using DoNotModify;

namespace HyperionTeam.SharedVariables
{
    [Serializable]
    public class SharedGameData : SharedVariable<GameData>
    {
        public static implicit operator SharedGameData(GameData value) { return new SharedGameData { mValue = value }; }
    }
}