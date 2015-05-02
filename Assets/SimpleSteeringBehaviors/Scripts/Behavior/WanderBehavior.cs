using UnityEngine;
using System.Collections;

namespace SimpleSteering.Behavior
{
    public class WanderBehavior : SimpleSteeringBehavior
    {
        private const float TWO_PI_RAD = 6.28318531f; // (360 degrees)

        public float    wanderForce     { get { return m_wanderForce; } set { m_wanderForce = value; } }
        public float    wanderChange    { get { return m_wanderChange; } set { m_wanderChange = value; } }
        public bool     freezeY         { get { return m_freezeY; } set { m_freezeY = value; } }

        [SerializeField]
        private bool        m_freezeY;

        [SerializeField]
        private float       m_wanderForce;

        [SerializeField]
        [Tooltip("Maximal heading change rate in RADIANS!")]
        private float       m_wanderChange;

        private Vector3     m_displacement;
        private Vector3     m_steering;

        protected override Vector3 CalculateSteeringForce( float deltaTime )
        {
            Vector3 targetDisplacement = Vector3.zero;

            if ( m_freezeY )
            {
                float a = Random.value * TWO_PI_RAD;
                targetDisplacement.Set( Mathf.Cos( a ), 0.0f, Mathf.Sin( a ) );
            }
            else
            {
                targetDisplacement = Random.onUnitSphere;
            }

            targetDisplacement *= m_wanderForce;

            if ( Mathf.Approximately( m_displacement.sqrMagnitude, 0.0f ) )
            {
                m_displacement = targetDisplacement;
            }
            else
            {
                m_displacement = Vector3.RotateTowards( m_displacement, targetDisplacement, m_wanderChange * deltaTime, float.MaxValue );
            }

            Vector3 wantedVelocity = ( transform.position + m_controller.currentVelocity + m_displacement ) - m_controller.transform.position;
            wantedVelocity = wantedVelocity.normalized * m_controller.maxVelocity;

            m_steering = wantedVelocity - m_controller.currentVelocity;
            return m_steering;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere( m_controller.transform.position + m_controller.currentVelocity, m_wanderForce );

            Gizmos.color = Color.white;
            Gizmos.DrawRay( m_controller.transform.position + m_controller.currentVelocity, m_displacement );
        }
    }
}