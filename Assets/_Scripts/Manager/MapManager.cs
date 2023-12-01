using UnityEngine;

public class MapManager : MonoBehaviour
{

    #region Variables

    // Public Variables.
    [Header("Map Parameters for players")]
    public float moveSpeedPlayer = 6f;
    public float jumpForcePlayer = 1.5f;
    public float gravityPlayer = 6f;
    public float jumpTimePlayer = 0.1f;

    // Public Static Variables.
    public static MapManager instance;
    
    #endregion
    
    #region Properties

    public float MoveSpeed => moveSpeedPlayer;
    public float JumpForce => jumpForcePlayer;
    public float Gravity => gravityPlayer;
    public float JumpTime => jumpTimePlayer;

    #endregion

    #region Builtin Methods

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    #endregion

}
