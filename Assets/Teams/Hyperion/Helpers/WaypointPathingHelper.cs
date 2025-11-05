using System.Collections.Generic;
using System.Linq;
using DoNotModify;
using UnityEngine;

namespace HyperionTeam
{
    public class WaypointPathingHelper
    {
        private static WaypointPathingHelper instance;

        public static WaypointPathingHelper Instance => instance ??= new WaypointPathingHelper();

        private GameData _gameData;
        private Dictionary<WayPointView, Dictionary<WayPointView, float>> _waypointPathing = new();

        public void Initialize(GameData gameData)
        {
            _gameData = gameData;
            List<WayPointView> gameDataWayPoints = gameData.WayPoints;
            for (int i = 0; i < gameDataWayPoints.Count; i++)
            {
                _waypointPathing.Add(gameDataWayPoints[i], new Dictionary<WayPointView, float>());
                for (int j = 0; j < gameDataWayPoints.Count; j++)
                {
                    if (i == j) continue;
                    float magnitude = (gameDataWayPoints[j].Position - gameDataWayPoints[i].Position).magnitude;
                    _waypointPathing[gameDataWayPoints[i]].Add(gameDataWayPoints[j], magnitude);
                }
            }
        }

        public Vector2 GetClosestWaypoint(Vector2 position, int owner, int depth = 4)
        {
            Path nextPath = GetNextPath(position, owner, depth);
            return nextPath.IsValid ? nextPath.First().Position : position;
        }
        
        public Path GetNextPath(Vector2 position, int owner, int depth = 4)
        {
            Dictionary<WayPointView, Path> paths = new();
            for (int i = 0; i < _waypointPathing.Keys.Count; i++)
            {
                WayPointView waypointPathingKey = _waypointPathing.Keys.ToArray()[i];
                if (waypointPathingKey.Owner == owner) continue;
                
                Path path = new(); 
                path.WayPoints.Add(waypointPathingKey);
                path.PathLength += (waypointPathingKey.Position - position).magnitude;
                paths.Add(waypointPathingKey, path);
                for (int j = 0; j < depth; j++)
                {
                    AddNextClosestWaypointToPAth(ref path, owner);
                }
            }
            float maxDistance = float.MaxValue;
            Path closestPath = null;
            foreach (var (waypoint, path) in paths)
            {
                if (path.PathLength < maxDistance)
                {
                    maxDistance = path.PathLength;
                    closestPath = path;
                }
            }
            
            return closestPath ?? new Path();
        }

        private void AddNextClosestWaypointToPAth(ref Path path, int owner)
        {
            WayPointView pathWayPoint = path.WayPoints.Last();
            float distance = float.MaxValue;
            WayPointView nextWaypoint = null;
            for (int i = 0; i < _waypointPathing.Keys.Count; i++)
            {
                WayPointView waypointPathingKey = _waypointPathing.Keys.ToArray()[i];
                if (waypointPathingKey.Owner == owner) continue;
                if (waypointPathingKey == pathWayPoint) continue;
                float magnitude = (pathWayPoint.Position - waypointPathingKey.Position).magnitude;

                if (magnitude < distance)
                {
                    distance = magnitude;
                    nextWaypoint = waypointPathingKey;
                    continue;
                }
            }
            if (nextWaypoint == null) return;
            path.WayPoints.Add(nextWaypoint);
            path.PathLength += distance;
        }
    }

    public class Path
    {
        public List<WayPointView> WayPoints { get; set; } = new();
        public float PathLength { get; set; } = 0;
        
        public bool IsValid => WayPoints.Count > 0;

        public WayPointView First()
        {
            return WayPoints[0];
        }
    }
}