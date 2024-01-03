using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.AnimationsScripts
{
    public class AnimationsController : MonoBehaviour
    {
        #region Variables
    
        [Header("Hit-box")]
        public List<Collider> attackHitBoxes;

        #endregion
    
        #region Animation Events

        /**
         * <summary>
         * Event that activate the colliders for each attack at the start.
         * </summary>
         */
        public void AttackStart()
        {
            foreach (Collider colliderBox in attackHitBoxes)
            {
                colliderBox.enabled = true;
            }
        }

    
        /**
         * <summary>
         * Event that deactivate the colliders for each attack at the end.
         * </summary>
         */
        public void AttackEnd()
        {
            foreach (Collider colliderBox in attackHitBoxes)
            {
                colliderBox.enabled = false;
            }
        }
        #endregion
    }
}
