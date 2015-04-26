using UnityEngine;
using System.Collections;

namespace SimpleSteering.Behavior
{
    public class AvoidByLayerBehavior : SimpleSteeringBehavior
    {        
        public float        speed           { get { return m_speed; } set { m_speed = Mathf.Clamp01( value ); } }
        public float        detectionRadius { get { return m_detectionRadius; } set { m_detectionRadius = value; } }
        public float        detectionRange  { get { return m_detectionRange; } set { m_detectionRange = value; } }
        public float        smoothTime      { get { return m_smoothTime; } set { m_smoothTime = value; } }

        [SerializeField]
        private bool        m_freezeY;

        [SerializeField]
        private float       m_detectionRadius;

        [SerializeField]
        private float       m_detectionRange;

        [SerializeField]
        private LayerMask   m_layers;

        [SerializeField]
        private float       m_smoothTime;

        [SerializeField]        
        [Range(0.0f, 1.0f)]
        private float       m_speed;

        private Vector3     m_steering;
        private Vector3     m_hitPoint;

        private Vector3     m_smoothSteering;

        public override Vector3 CalculateSteeringForce()
        {
            Ray        ray = new Ray( m_controller.transform.position, m_controller.currentVelocity );
            RaycastHit hit;
            
            m_hitPoint = Vector3.zero;

            if ( Physics.SphereCast( ray, m_detectionRadius, out hit, m_detectionRange, m_layers.value ) )
            {
                m_hitPoint = hit.point;

                Vector3 displacement = ( m_hitPoint - hit.collider.transform.position ).normalized;

                if ( m_freezeY )
                {
                    displacement.y = 0.0f;
                    displacement.Normalize();
                }

                Vector3 wantedSteering = displacement * m_speed * m_controller.maxVelocity;
                m_steering = Vector3.SmoothDamp( m_steering, wantedSteering, ref m_smoothSteering, m_smoothTime );
            }
            else
            {
                m_steering = Vector3.SmoothDamp( m_steering, Vector3.zero, ref m_smoothSteering, m_smoothTime );
            }

            return m_steering;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay( m_controller.transform.position + m_controller.currentVelocity, m_steering );

            Vector3 visionRange = m_controller.currentVelocity.normalized * m_detectionRange;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere( m_controller.transform.position, m_detectionRadius );
            Gizmos.DrawWireSphere( m_controller.transform.position + visionRange, m_detectionRadius );
            Gizmos.DrawLine( m_controller.transform.position + m_controller.transform.right * m_detectionRadius, m_controller.transform.position + visionRange + m_controller.transform.right * m_detectionRadius );
            Gizmos.DrawLine( m_controller.transform.position - m_controller.transform.right * m_detectionRadius, m_controller.transform.position + visionRange - m_controller.transform.right * m_detectionRadius );

            if ( m_hitPoint != Vector3.zero )
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere( m_hitPoint, 0.1f );
                Gizmos.DrawRay( m_hitPoint, m_steering );
            }
        }
    }
}