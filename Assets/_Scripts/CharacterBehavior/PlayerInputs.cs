using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    #region Variables
    
    [Header("Controls")] 
    public KeyCode horizontalRight; 
    public KeyCode horizontalLeft;
    public KeyCode verticalDown;
    public KeyCode verticalUp;
    public KeyCode jump;
    public KeyCode attack;
    
    // Player Movements Values Variables.
    private float _horizontal;
    private bool _jumped;
    private bool _attack;
    private bool _down;
    private bool _up;
    
    #endregion

    #region Properties
    
    public float Horizontal => _horizontal;
    public bool Attack => _attack;
    public bool Jumped => _jumped;
    public bool DownMovement => _down;
    public bool UpMovement => _up;
    
    #endregion

    #region Builtin Methods
    
    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    void Start()
    {
        PlayerControls();
    }
    
    
    /**
     * <summary>
     * Update is called once per frame.
     * </summary>
     */
    void Update()
    {
        HorizontalMovementsCalculation();

        _jumped = Input.GetKey(jump);
        _attack = Input.GetKey(attack);
        _down = Input.GetKey(verticalDown);
        _up = Input.GetKey(verticalUp);
    }

    #endregion

    #region Custom Methods

    /**
     * <summary>
     * Function that put the local multiplayer controls respective to each players.
     * </summary>
     */
    private void PlayerControls()
    {
        switch (gameObject.name)
        {
            case "Player1":
                horizontalLeft = KeyCode.A;
                horizontalRight = KeyCode.D;
                verticalDown = KeyCode.S;
                verticalUp = KeyCode.W;
                jump = KeyCode.Space;
                attack = KeyCode.G;
                break;
            case "Player2":
                horizontalLeft = KeyCode.LeftArrow;
                horizontalRight = KeyCode.RightArrow;
                verticalDown = KeyCode.DownArrow;
                verticalUp = KeyCode.Keypad1;
                jump = KeyCode.UpArrow;
                attack = KeyCode.Keypad0;
                break;
        }
    }


    /**
     * <summary>
     * Function to calculate the Horizontal Movements.
     * </summary>
     */
    private void HorizontalMovementsCalculation()
    {
        if (Input.GetKey(horizontalRight)) _horizontal = Mathf.Clamp(_horizontal + Time.deltaTime, 0f, 1f);
        else if (Input.GetKey(horizontalLeft)) _horizontal = Mathf.Clamp(_horizontal - Time.deltaTime, -1f, 0f);
        else _horizontal = 0;
    }

    #endregion
}
