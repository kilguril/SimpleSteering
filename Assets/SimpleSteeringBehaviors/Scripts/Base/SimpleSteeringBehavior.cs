using UnityEngine;
using System.Collections;

using SimpleSteering.Controller;

namespace SimpleSteering.Behavior
{
    [RequireComponent( typeof( SimpleSteeringController ) ) ]
    public abstract class SimpleSteeringBehavior : MonoBehaviour
    {
        protected SimpleSteeringController  m_controller;       // Cached reference to owner controller

        public abstract Vector3 CalculateSteeringForce();
        public abstract void    DrawGizmos();


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