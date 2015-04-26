using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleSteering.Behavior;

namespace SimpleSteering.Controller
{
    public abstract class SimpleSteeringController : MonoBehaviour
    {
        public float            maxVelocity         { get { return m_maxVelocity; } set { m_maxVelocity = value; } }
        public float            maxSteeringForce    { get { return m_maxSteeringForce; } set { m_maxSteeringForce = value; } }
        public Vector3          currentVelocity     { get { return m_currentVelocity; } }

        [SerializeField]
        private bool            m_renderDebugInformation;     // If checked, will render debug information (editor only)

        [SerializeField]
        protected float         m_maxVelocity;                // Maximum velocity defines the maximal displacement per second

        [SerializeField]
        protected float         m_maxSteeringForce;           // Maximum steering force defines the maximal magnitude of the cummulative steering vector

        protected Vector3       m_steeringVector;             // Cached steering vector (used for debug visualization)
        protected Vector3       m_currentVelocity;            // Cached current velocity vector        

        // Aggerated list of all active steering behaviors
        // Not happy about the initialization during construction, but this is necessary to avoid getting into Awake() execution order issues
        protected List< SimpleSteeringBehavior >    m_behaviors = new List<SimpleSteeringBehavior>();                  

        protected abstract void OnAwake();
        protected abstract void OnUpdate();
        protected abstract void OnFixedUpdate();
        protected abstract void OnDrawDebugGizmos();


        public T AddBehavior< T >() where T : SimpleSteeringBehavior
        {
            return gameObject.AddComponent<T>();
        }

        
        public void RegisterBehavior( SimpleSteeringBehavior behavior )
        {
            m_behaviors.Add( behavior );
        }


        public void UnregisterBehavior( SimpleSteeringBehavior behavior )
        {
            m_behaviors.Remove( behavior );
        }


        void Awake()
        {            
            m_currentVelocity = Vector3.zero;
            m_steeringVector  = Vector3.zero;

            OnAwake();
        }

        void Update()
        {
            OnUpdate();
        }

        void FixedUpdate()
        {
            OnFixedUpdate();
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if ( m_renderDebugInformation )
            {                
                Gizmos.color = Color.green;
                Gizmos.DrawRay( transform.position, m_currentVelocity );

                Gizmos.color = Color.red;
                Gizmos.DrawRay( transform.position, m_steeringVector );

                OnDrawDebugGizmos();

                for ( int i = 0; i < m_behaviors.Count; i++ )
                {
                    m_behaviors[ i ].DrawGizmos();
                }
            }
        }
#endif

    }
}