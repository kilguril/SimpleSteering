using UnityEngine;
using System.Collections;

namespace SimpleSteering.Behavior
{
    public class FleeBehavior : SimpleSteeringBehavior
    {
        public Transform target { get { return m_target; } set { m_target = value; } }
        public float     speed  { get { return m_speed; } set { m_speed = Mathf.Clamp01( value ); } }


        [SerializeField]
        private Transform   m_target;

        [SerializeField]        
        [Range(0.0f, 1.0f)]
        private float       m_speed;

        private Vector3     m_steering;

        public override Vector3 CalculateSteeringForce()
        {
            if ( m_target == null )
            {
                m_steering = Vector3.zero;
                return m_steering;
            }

            Vector3 wantedVelocity = m_controller.transform.position - m_target.position;
            wantedVelocity = wantedVelocity.normalized * m_controller.maxVelocity * m_speed;

            m_steering = wantedVelocity - m_controller.currentVelocity;
            return m_steering;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay( m_controller.transform.position + m_controller.currentVelocity, m_steering );
        }
    }
}