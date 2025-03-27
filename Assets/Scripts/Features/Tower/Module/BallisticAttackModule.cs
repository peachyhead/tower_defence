using Features.Tower.Core.Data;
using UnityEngine;

namespace Features.Tower.Module
{
    public class BallisticAttackModule : TowerAttackModule
    {
        private Vector3 _cachedPosition;
        private Vector3 _shotVector;
        
        protected override Vector3 GetAimPoint(ITarget target)
        {
            var targetPos = target.Position;
            var targetVelocity = target.Velocity;
            var shooterPos = MuzzlePoint.position;

            var gravity = -Physics.gravity.y;
            var toTarget = targetPos - shooterPos;
            var horizontalDistance = new Vector3(toTarget.x, 0, toTarget.z).magnitude;
            var verticalDistance = toTarget.y;
            
            var projectileSpeed = Mathf.Sqrt(gravity * horizontalDistance);
            var highAngle = CalculateHighAngle(projectileSpeed, horizontalDistance, verticalDistance, gravity);

            if (float.IsNaN(highAngle))
            {
                _cachedPosition = targetPos;
                return targetPos;
            }
            
            var timeToTarget = horizontalDistance / (projectileSpeed * Mathf.Cos(highAngle));
            var lead = timeToTarget * 170f;
            
            Vector3 futureTargetPos = targetPos + targetVelocity * lead;
            
            futureTargetPos.y += 0.5f * gravity * timeToTarget * timeToTarget;

            _cachedPosition = futureTargetPos;
            _cachedPosition.y = targetPos.y;
            return futureTargetPos;
        }
        
        private float CalculateHighAngle(float speed, float distance, float heightDifference, float gravity)
        {
            var v2 = speed * speed;
            var underRoot = v2 * v2 - gravity * (gravity * distance * distance + 2 * heightDifference * v2);

            if (underRoot < 0)
            {
                Debug.LogError("Цель недостижима: недостаточная скорость.");
                return float.NaN;
            }

            var root = Mathf.Sqrt(underRoot);
            var highAngle = Mathf.Atan((v2 + root) / (gravity * distance));

            return highAngle;
        }
        
        protected override Vector3 GetLaunchVelocity(ITarget target)
        {
            var aimPoint = GetAimPoint(target);
            return CalculateBallisticVelocity(aimPoint, MuzzlePoint.position, -Physics.gravity.y);
        }

        private Vector3 CalculateBallisticVelocity(Vector3 target, Vector3 origin, float gravity)
        {
            var toTarget = target - origin;
            var yOffset = toTarget.y;
            toTarget.y = 0;
            var distance = toTarget.magnitude;

            var underRoot = gravity * (gravity * distance * distance + 2 * yOffset * gravity);
            if (underRoot < 0)
            {
                Debug.LogError("Цель недостижима при текущей гравитации.");
                return Vector3.zero;
            }

            var root = Mathf.Sqrt(underRoot);
            var lowAngle = Mathf.Atan((gravity * distance - root) / (gravity * distance));
            var highAngle = Mathf.Atan((gravity * distance + root) / (gravity * distance));
            
            var angle = lowAngle > 0 ? lowAngle : highAngle;
    
            var v2 = gravity * distance / Mathf.Sin(2 * angle);
            if (v2 < 0)
            {
                Debug.LogError("Расчет дал отрицательную скорость.");
                return Vector3.zero;
            }

            var speed = Mathf.Sqrt(v2) / 1.1f;
            
            var velocity = toTarget.normalized * speed * Mathf.Cos(angle);
            velocity.y = speed * Mathf.Sin(angle);
            _shotVector = velocity;
            return velocity;
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