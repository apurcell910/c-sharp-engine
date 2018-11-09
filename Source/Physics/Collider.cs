using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Physics
{
    public abstract class Collider
    {
        public bool IsTrigger { get; set; }

        public abstract bool IsTouching(Collider other);

        public delegate void ColliderEvent(Collider other);

        protected event ColliderEvent _colliderEnter;
        /// <summary>
        /// Called when another collider enters this collider
        /// </summary>
        public event ColliderEvent ColliderEnter
        {
            add => _colliderEnter += value;
            remove => _colliderEnter -= value;
        }

        protected event ColliderEvent _colliderLeave;
        /// <summary>
        /// Called when another collider leaves this collider
        /// </summary>
        public event ColliderEvent ColliderLeave
        {
            add => _colliderLeave += value;
            remove => _colliderLeave -= value;
        }

        protected event ColliderEvent _colliderStay;
        /// <summary>
        /// Called continuously while another collider is touching this collider
        /// </summary>
        public event ColliderEvent ColliderStay
        {
            add => _colliderStay += value;
            remove => _colliderStay -= value;
        }
    }
}
