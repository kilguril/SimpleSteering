using UnityEngine;
using System.Collections;

namespace SimpleSteering.Behavior
{
    public class ArriveBehavior : SimpleSteeringBehavior
    {
        public Transform target     { get { return m_target; } set { m_target = value; } }        
        public float     slowRadius { get { return m_slowRadius; } set { m_slowRadius = value; } }

        [SerializeField]
        private Transform   m_target;

        [SerializeField]
        private float       m_slowRadius;

        private Vector3     m_steering;

        protected override Vector3 CalculateSteeringForce( float deltaTime )
        {
            if ( m_target == null )
            {
                m_steering = Vector3.zero;
                return m_steering;
            }

            Vector3 wantedVelocity = m_target.position - m_controller.transform.position;
            float   distance = wantedVelocity.magnitude;
            wantedVelocity = wantedVelocity.normalized * m_controller.maxVelocity;

            if ( distance < m_slowRadius )
            {
                wantedVelocity *= ( distance / m_slowRadius );
            }

            m_steering = wantedVelocity - m_controller.currentVelocity;
            return m_steering;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay( m_controller.transform.position + m_controller.currentVelocity, m_steering );

            if ( m_target != null )
            {
                Gizmos.DrawWireSphere( m_target.position, m_slowRadius );
            }
        }
    }
}