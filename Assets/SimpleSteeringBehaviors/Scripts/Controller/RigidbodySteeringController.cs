using UnityEngine;
using System.Collections;

namespace SimpleSteering.Controller
{
    [RequireComponent( typeof( Rigidbody ) ) ]
    public class RigidbodySteeringController : SimpleSteeringController
    {
        protected Rigidbody     m_rigidbody;    // Cached reference to the rigidbody


        protected override void OnAwake()
        {
            m_rigidbody = GetComponent< Rigidbody >();   
        }


        protected override void OnUpdate()
        {

        }


        protected override void OnFixedUpdate()
        {
            m_steeringVector = Vector3.zero;

            for ( int i = 0; i < m_behaviors.Count; i++ )
            {
                m_steeringVector += m_behaviors[ i ].GetSteeringForce( Time.fixedDeltaTime );
            }

            float steeringMagnitude = Mathf.Clamp( m_steeringVector.magnitude, 0.0f, m_maxSteeringForce );
            m_steeringVector = m_steeringVector.normalized * steeringMagnitude;

            m_currentVelocity = m_currentVelocity + ( m_steeringVector * Time.fixedDeltaTime );
            float velocityMagnitude = Mathf.Clamp( m_currentVelocity.magnitude, 0.0f, m_maxVelocity );
            m_currentVelocity = m_currentVelocity.normalized * velocityMagnitude;

            // Orient to target
            //if ( !Mathf.Approximately( velocityMagnitude, 0.0f ) )
            //{
            if ( velocityMagnitude > 0.001f )
            {
                m_rigidbody.MoveRotation( Quaternion.LookRotation( m_currentVelocity, Vector3.up ) );
            }

            // Apply motion
            m_rigidbody.MovePosition( transform.position + ( m_currentVelocity * Time.fixedDeltaTime ) );
        }


        protected override void OnDrawDebugGizmos()
        {
            
        }
    }
}