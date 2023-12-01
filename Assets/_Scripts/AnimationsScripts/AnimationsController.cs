using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    #region Variables

    // Public Variables.
    [Header("Hitbox")]
    public List<Collider> attackHitBoxes;

    #endregion
    
    #region Animation Events

    // Event that activate the colliders for each attack at the start.
    public void AttackStart()
    {
        foreach (Collider collider in attackHitBoxes)
        {
            collider.enabled = true;
        }
    }

    
    // Event that deactivate the colliders for each attack at the end.
    public void AttackEnd()
    {
        foreach (Collider collider in attackHitBoxes)
        {
            collider.enabled = false;
        }
    }
    #endregion
}
