using System;
using DoNotModify;
using UnityEngine;

namespace HyperionTeam.Helpers
{
    public static class ShootingHelpers
    {

        public static bool WillHit(SpaceShipView spaceship, Vector2 bulletPosition, Vector2 bulletVelocity, float hitTimeTolerance, float rangeTolerance = 3f, float minRange = 1f)
        {
            if(hitTimeTolerance <= 0) {
                Debug.LogError("Hit time tolerence must be greater than 0");
                return false;
            }

            float magnitude = (spaceship.Position - bulletPosition).magnitude;
            if (minRange > magnitude || magnitude > rangeTolerance)
            {
                return false;
            }
            
            float bulletMovementAngle = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.right, bulletVelocity);
            Vector2 bulletMovementDirection = new Vector2(Mathf.Cos(bulletMovementAngle), Mathf.Sin(bulletMovementAngle));
            
            if (spaceship.Velocity.magnitude == 0)
            {
                Vector2 bulletToSpaceship = bulletPosition - spaceship.Position;
                // Debug.Log($"Bullet to spaceship: {bulletToSpaceship} | BulletMovementDirection: {bulletMovementDirection} | BulletMovementAngle: {bulletMovementAngle}");
                float angle = Vector2.Angle(-bulletMovementDirection, bulletToSpaceship);
                // Debug.Log($"Angle: {angle}");
                return angle < 4f;
            }
            
            // float shootAngle = Mathf.Deg2Rad * spaceship.Orientation;
            float spaceshipMovementAngle = Vector2.Angle(Vector2.right, spaceship.Velocity);
            // float shootAngle = Math.Atan2();
            Vector2 spaceshipMovementDirection = new Vector2(Mathf.Cos(spaceshipMovementAngle), Mathf.Sin(spaceshipMovementAngle));

            bool canIntersect = AimingHelpers.ComputeIntersection(spaceship.Position, spaceshipMovementDirection, bulletPosition, bulletVelocity, out Vector2 intersection);        
            if (!canIntersect) { // Cannot shoot if directions never cross eachother (parallel)
                return false;
            }

            Vector2 spaceshipToIntersection = intersection - spaceship.Position;        
            if (Vector2.Dot(spaceshipToIntersection, spaceshipMovementDirection) <= 0) // Cannot shoot if target is behind
                return false;

            Vector2 targetToIntersection = intersection - bulletPosition;
            float targetTimeToIntersection = spaceshipToIntersection.magnitude / spaceship.Velocity.magnitude;
            float bulletTimeToIntersection = targetToIntersection.magnitude / Bullet.Speed;
            targetTimeToIntersection *= Vector2.Dot(targetToIntersection, bulletVelocity) > 0 ? 1 : -1;

            float timeDiff = bulletTimeToIntersection - targetTimeToIntersection;        
            return Mathf.Abs(timeDiff) < hitTimeTolerance;
        }
    }
}