using UnityEngine;
using System.Collections;

namespace SimpleSteering.Behavior
{
    public class PursueBehavior : SimpleSteeringBehavior
    {
        public Transform target { get { return m_target; } set { m_target = value; } }

        [SerializeField]
        private Transform   m_target;

        [SerializeField]
        private float       m_predictionTime;

        private Vector3     m_steering;
        private Vector3     m_perceivedVelocity;        

        private Transform   m_cachedTarget;
        private Vector3     m_prevPosition;
        private float       m_prevTimestep;

        protected override Vector3 CalculateSteeringForce()
        {
            if ( m_target == null )
            {
                m_steering = Vector3.zero;
                return m_steering;
            }

            m_perceivedVelocity = Vector3.zero;

            if ( m_cachedTarget == m_target && m_target != null )
            {                
                Vector3 dx = m_target.position - m_prevPosition;
                float dt = Time.time - m_prevTimestep;

                m_perceivedVelocity = dx / dt;
            }

            m_cachedTarget = m_target;
            m_prevTimestep = Time.time;
            m_prevPosition = m_target.position;

            Vector3 wantedVelocity = ( m_target.position + m_perceivedVelocity * m_predictionTime ) - m_controller.transform.position;
            wantedVelocity = wantedVelocity.normalized * m_controller.maxVelocity;

            m_steering = wantedVelocity - m_controller.currentVelocity;
            return m_steering;
        }

        public override void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay( m_controller.transform.position + m_controller.currentVelocity, m_steering );

            if ( m_target != null )
            {
                Gizmos.DrawWireSphere( m_target.position + m_perceivedVelocity * m_predictionTime, 0.25f );
            }
        }
    }
}