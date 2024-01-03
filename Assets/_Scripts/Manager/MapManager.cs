using UnityEngine;

namespace _Scripts.Manager
{
    public class MapManager : MonoBehaviour
    {

        #region Variables
        
        [Header("Map Parameters for players")]
        public float moveSpeedPlayer = 6f;
        public float jumpForcePlayer = 1.5f;
        public float gravityPlayer = 6f;
        public float jumpTimePlayer = 0.1f;

        // Instance Variables.
        private static MapManager _instance;
    
        #endregion
    
        #region Properties

        public float MoveSpeed => moveSpeedPlayer;
        public float JumpForce => jumpForcePlayer;
        public float Gravity => gravityPlayer;
        public float JumpTime => jumpTimePlayer;

        public static MapManager Instance => _instance;

        #endregion

        #region Builtin Methods

        /**
         * <summary>
         * Awake is called when an enabled script instance is being loaded.
         * </summary>
         */
        private void Awake()
        {
            if (_instance != null) Destroy(this);
            _instance = this;
        }

        #endregion

    }
}
