using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    #region Variables

    // Public Variables.
    [Header("Controls")] 
    public KeyCode horizontalRight; 
    public KeyCode horizontalLeft;
    public KeyCode jump;
    public KeyCode attack;
    
    // Private Variables.
    private float _horizontal;
    private bool _jumped;
    private bool _attack;
    
    #endregion

    #region Properties
    
    public float Horizontal => _horizontal;
    public bool Attack => _attack;
    public bool Jumped => _jumped;
    
    #endregion

    #region Builtin Methods
    
    // Start is called before the first frame update.
    void Start()
    {
        PlayerControls();
    }
    
    
    // Update is called once per frame.
    void Update()
    {
        // Give the values for the directional variables based on Time.deltaTime.
        if (Input.GetKey(horizontalRight)) _horizontal = Mathf.Clamp(_horizontal + Time.deltaTime, 0f, 1f);
        else if (Input.GetKey(horizontalLeft)) _horizontal = Mathf.Clamp(_horizontal - Time.deltaTime, -1f, 0f);
        else _horizontal = 0;

        _jumped = Input.GetKey(jump);
        _attack = Input.GetKey(attack);
    }

    #endregion

    #region Custom Methods

    // Private Function that put the local multiplayer controls respective to each players.
    private void PlayerControls()
    {
        switch (gameObject.name)
        {
            case "Player1":
                horizontalLeft = KeyCode.A;
                horizontalRight = KeyCode.D;
                jump = KeyCode.Space;
                attack = KeyCode.G;
                break;
            case "Player2":
                horizontalLeft = KeyCode.LeftArrow;
                horizontalRight = KeyCode.RightArrow;
                jump = KeyCode.UpArrow;
                attack = KeyCode.Keypad0;
                break;
        }
    }

    #endregion
}
