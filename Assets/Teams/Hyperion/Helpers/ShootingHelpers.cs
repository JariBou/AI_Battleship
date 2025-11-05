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
            RaycastHit2D[] raycastAll = Physics2D.RaycastAll(spaceship.Position, spaceshipToIntersection.normalized, spaceshipToIntersection.magnitude);
            foreach (RaycastHit2D hit2D in raycastAll)
            {
                if (hit2D.collider.CompareTag("Asteroid") || hit2D.collider.CompareTag("Mine"))
                {
                    return false;
                }
            }
            if (Vector2.Dot(spaceshipToIntersection, spaceshipMovementDirection) <= 0) // Cannot shoot if target is behind
                return false;

            Vector2 targetToIntersection = intersection - bulletPosition;
            float targetTimeToIntersection = spaceshipToIntersection.magnitude / spaceship.Velocity.magnitude;
            float bulletTimeToIntersection = targetToIntersection.magnitude / Bullet.Speed;
            targetTimeToIntersection *= Vector2.Dot(targetToIntersection, bulletVelocity) > 0 ? 1 : -1;

            float timeDiff = bulletTimeToIntersection - targetTimeToIntersection;        
            return Mathf.Abs(timeDiff) < hitTimeTolerance;
        }
        
        public static bool CanHit(SpaceShipView spaceship, Vector2 targetPosition, float angleTolerance)
        {
            float shootAngle = Mathf.Deg2Rad * spaceship.Orientation;
            Vector2 shootDirection = new Vector2(Mathf.Cos(shootAngle), Mathf.Sin(shootAngle));
            Vector2 spaceshipToTarget = targetPosition - spaceship.Position;
            
            RaycastHit2D[] raycastAll = Physics2D.RaycastAll(spaceship.Position, spaceshipToTarget.normalized, spaceshipToTarget.magnitude);
            foreach (RaycastHit2D hit2D in raycastAll)
            {
                if (hit2D.collider.CompareTag("Asteroid"))
                {
                    return false;
                }
            }

            return Vector2.Angle(shootDirection, spaceshipToTarget) < angleTolerance;
        }
        
        public static bool CanHit(SpaceShipView spaceship, Vector2 targetPosition, Vector2 targetVelocity, float hitTimeTolerance)
        {                
            if(hitTimeTolerance <= 0) {
                Debug.LogError("Hit time tolerence must be greater than 0");
                return false;
            }

            float shootAngle = Mathf.Deg2Rad * spaceship.Orientation;
            Vector2 shootDirection = new Vector2(Mathf.Cos(shootAngle), Mathf.Sin(shootAngle));

            Vector2 intersection;
            bool canIntersect = AimingHelpers.ComputeIntersection(spaceship.Position, shootDirection, targetPosition, targetVelocity, out intersection);        
            if (!canIntersect) { // Cannot shoot if directions never cross eachother (parallel)
                return false;
            }

            Vector2 spaceshipToIntersection = intersection - spaceship.Position;
            RaycastHit2D[] raycastAll = Physics2D.RaycastAll(spaceship.Position, spaceshipToIntersection.normalized, spaceshipToIntersection.magnitude);
            foreach (RaycastHit2D hit2D in raycastAll)
            {
                if (hit2D.collider.CompareTag("Asteroid"))
                {
                    return false;
                }
            }
            if (Vector2.Dot(spaceshipToIntersection, shootDirection) <= 0) // Cannot shoot if target is behind
                return false;

            Vector2 targetToIntersection = intersection - targetPosition;
            float bulletTimeToIntersection = spaceshipToIntersection.magnitude / Bullet.Speed;
            float targetTimeToIntersection = targetToIntersection.magnitude / targetVelocity.magnitude;
            targetTimeToIntersection *= Vector2.Dot(targetToIntersection, targetVelocity) > 0 ? 1 : -1;

            float timeDiff = bulletTimeToIntersection - targetTimeToIntersection;        
            return Mathf.Abs(timeDiff) < hitTimeTolerance;
        }
    }
}