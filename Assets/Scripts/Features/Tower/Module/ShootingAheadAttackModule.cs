using Features.Tower.Core.Data;
using UnityEngine;

namespace Features.Tower.Module
{
    public class ShootingAheadAttackModule : TowerAttackModule
    {
        private Vector3 _cachedPosition;
        private Vector3 _shotVector;
        
        protected override Vector3 GetLaunchVelocity(ITarget target)
        {
            var velocity = (GetAimPoint(target) - MuzzlePoint.position).normalized * Data.Speed;
            _shotVector = velocity;
            return velocity;
        }
        
        protected override Vector3 GetAimPoint(ITarget target)
        {
            Vector3 targetPos = target.Position;
            Vector3 targetVelocity = target.Velocity;
            Vector3 shooterPos = MuzzlePoint.position;
            float projectileSpeed = Data.Speed;

            Vector3 toTarget = targetPos - shooterPos;
            float distance = toTarget.magnitude;

            float estimatedTime = distance / projectileSpeed;

            Vector3 futurePos = targetPos;

            // Итеративное уточнение (2-3 итерации обычно достаточно)
            for (int i = 0; i < 3; i++)
            {
                futurePos = targetPos + targetVelocity * estimatedTime;
                distance = (futurePos - shooterPos).magnitude;
                estimatedTime = (distance / projectileSpeed) * 180f;
            }

            _cachedPosition = futurePos;
            return futurePos;
        }
        
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_cachedPosition, 0.25f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(MuzzlePoint.position, _shotVector);
        }
    }
}