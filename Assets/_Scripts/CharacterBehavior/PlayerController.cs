using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Public variables.
    [Header("Player Parameters")]
    public float moveSpeed = 1f;
    public float turnSmoothSpeed = 0.1f;
    public float jumpForce = 10f;
    public float gravity = 6f;
    public float jumpTimeCounter = 1f;
    public float jumpTime;

    // Private variables
    private float _currentVelocity;
    private float _verticalSpeed;
    private float _playerDamage;
    private float _maxDamage = 999f;
    private bool _canMove = true;
    private bool _isJumping;

    private Vector3 _movement = Vector3.zero;
    private Vector3 _direction = Vector2.zero;
    private Vector3 _moveDirection = Vector3.zero;
    
    private Animator _animator;
    private BattleUIManager _battleUIManager;
    private CharacterController _characterController;
    private PlayerInputs _inputs;
    
    #endregion

    #region Properties

    public float Damage => _playerDamage;
    public float VerticalSpeed
    {
        get { return _verticalSpeed; }
        set { _verticalSpeed = value; }
    }
    
    #endregion

    #region Builtin Methods
    
    // Start is called before the first frame update.
    void Start()
    {
        _battleUIManager = BattleUIManager.instance;
        
        _inputs = GetComponent<PlayerInputs>(); // Calling the PlayerInput Component.
        _characterController = GetComponent<CharacterController>(); // Calling the CharacterController component of the GameObject.
        _animator = GetComponent<Animator>(); // Calling the Animator component of the GameObject.
    }

    
    // FixedUpdate is called once per fixed frame.
    void FixedUpdate()
    {
        // Calling movements & animations functions.
        Locomotion();
        CalculateVerticalMovement();
        UpdateAnimation();
    }

    #endregion

    #region Custom Methods

    // Function used for the player's locomotion, basic movements and view direction.
    private void Locomotion()
    {
        if (!_inputs) return; // Check if no input, then return nothing.

        if (_canMove) // Check if player is moving.
        {
            _direction.Set(_inputs.Horizontal, 0f, 0f); // Set the player X movements to the Horizontal variable.

            if (_direction.normalized.magnitude >= 0.1f) // Check if the player moves.
            {
                float targetAngle = Mathf.Atan2(_direction.x, 0f) * Mathf.Rad2Deg; // Return the target angle of the player.
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity,
                    turnSmoothSpeed); // Return the calculation of the smooth movement.

                transform.rotation = Quaternion.Euler(0f, angle, 0f); // Apply the smooth when the player change his pov side.

                _moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Return the direction movement of the player.
                _moveDirection = new Vector3(_moveDirection.x, _moveDirection.y, 0f); // Make it that player can't move on the Z axe.
            }
            else
            {
                _moveDirection = Vector3.zero; // If the player doesn't move, put his position to Zero.
            }
        }

        _movement = _moveDirection.normalized * (moveSpeed * Time.deltaTime); // Calculate the movements of the player based on the multiples variables.
    }

    
    // Function that calculate the vertical movement of the player(The jump).
    private void CalculateVerticalMovement()
    {
        if (_characterController.isGrounded)
        {
            //Debug.Log("Grounded");
            _verticalSpeed = -gravity * 0.6f * Time.deltaTime; // Apply the gravity to the player(On ground).

            // Check if the player can jump and move.
            if (_inputs.Jumped && _canMove)
            {
                //Debug.Log("Je vole");
                _isJumping = true;
                jumpTimeCounter = 0f; // Reset the jump timer.
                _verticalSpeed = jumpForce;
            }
        }
        else
        {
            _verticalSpeed -= gravity * Time.deltaTime; // Apply the gravity to the player(In air).
            //Debug.Log("isNotGrounded");
        }

        // Check if the player is jumping.
        if (_isJumping)
        {
            // Used to calculate the height of the jump based on the player holding the jump key.
            if (jumpTimeCounter < jumpTime)
            {
                float jumpFactor = 1 - jumpTimeCounter / jumpTime; // JumpFactor to calculate the max height of the jump.
                _verticalSpeed += jumpForce * jumpFactor; // Apply the factor to the verticalSpeed.
                jumpTimeCounter += Time.deltaTime;
            }
            else _isJumping = false;
        }

        // If the player is not holding the jump key, release the jump.
        if (!_inputs.Jumped) _isJumping = false;
        
        _movement += Vector3.up * (_verticalSpeed * Time.deltaTime); // Apply the jump to the movement.
        _characterController.Move(_movement); // Apply the movement to the CharacterController component.
    }

    
    // Function that says if the player should move or not.
    private void CanMove(bool canMove)
    {
        if (canMove)
        {
            _canMove = true;
        }
        else
        {
            _canMove = false;
            _moveDirection = Vector3.zero; // Freeze the player.
        }
    }
    
    
    // Function that give player damage when called.
    public void PlayerTakeDamage(float damage)
    {
        _playerDamage = Mathf.Clamp(_playerDamage + damage, 0f, _maxDamage);
        _battleUIManager.UpdatePlayerPercentage(gameObject, _playerDamage); // Apply the damage on the UI.
    }
    
    
    // Function that update the animations on the animator.
    // TO-DO: Apply every animations for each attack type.
    private void UpdateAnimation()
    {
        if(moveSpeed != 0f) // Check if the player move.
            _animator.SetFloat("Velocity", _moveDirection.normalized.magnitude); // Use the running animation when he moves.

        if (!_characterController.isGrounded) // Check if the player is not on the ground.
        {
            _animator.SetBool("isGrounded", false);
            _animator.SetFloat("VerticalSpeed", _verticalSpeed);
        }
        else
        {
            _animator.SetBool("isGrounded", true);
            
            // TO-DO: Apply every animations for each attack type. (Reminder)
            if (_inputs.Attack)
            {
                _animator.SetTrigger("Attack");
                _animator.SetInteger("AttackIdle", 0); // Light Neutral Attack.
            }
            else
            {
                _animator.ResetTrigger("Attack");
                _animator.SetInteger("AttackIdle", -1);
            }
        }
    }

    public IEnumerator UpdateHitAnimation(float hitY)
    {
        _animator.SetTrigger("Hit");
        _animator.SetFloat("HitY", hitY);
        yield return new WaitForSeconds(2f);
        _animator.ResetTrigger("Hit");
    }
    
    #endregion
}
