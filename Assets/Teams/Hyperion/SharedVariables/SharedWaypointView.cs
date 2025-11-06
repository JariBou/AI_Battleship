using System;
using BehaviorDesigner.Runtime;
using DoNotModify;

namespace HyperionTeam.SharedVariables
{
    [Serializable]
    public class SharedWaypointView : SharedVariable<WayPointView>
    {
        public static implicit operator SharedWaypointView(WayPointView value) { return new SharedWaypointView { mValue = value }; }
    }
}