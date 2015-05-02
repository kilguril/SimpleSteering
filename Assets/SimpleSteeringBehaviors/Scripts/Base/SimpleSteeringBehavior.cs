using UnityEngine;
using System.Collections;

using SimpleSteering.Controller;

namespace SimpleSteering.Behavior
{
    [RequireComponent( typeof( SimpleSteeringController ) ) ]
    public abstract class SimpleSteeringBehavior : MonoBehaviour
    {
        public float speed { get { return m_speed; } set { m_speed = Mathf.Clamp01( value ); } }

        [SerializeField]
        [Range( 0.0f, 1.0f )]
        private float                       m_speed;

        protected SimpleSteeringController  m_controller;       // Cached reference to owner controller
        
        public abstract void       DrawGizmos();
        protected abstract Vector3 CalculateSteeringForce( float deltaTime );


        // Since Unity provides us with two timing methods ( update and fixedUpdate ), we will pass the relevant dT value as parameter
        // To abstract the type of timing method used
        public Vector3 GetSteeringForce( float deltaTime )
        {
            return CalculateSteeringForce( deltaTime ) * m_speed;
        }


        void Awake()
        {
            m_controller = GetComponent< SimpleSteeringController >();
        }


        void OnEnable()
        {
            if ( m_controller != null )
            {
                m_controller.RegisterBehavior( this );
            }
        }


        void OnDisable()
        {
            if ( m_controller != null )
            {
                m_controller.UnregisterBehavior( this );
            }
        }
    }
}