using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _Scripts.CharacterBehavior
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
    
        [Header("Player Parameters")]
        public float moveSpeed = 1f;
        public float turnSmoothSpeed = 0.1f;
        public float jumpForce = 10f;
        public float gravity = 6f;
        public float jumpTimeCounter = 1f;
        public float jumpTime;

        // Player Movements Variables
        private float _currentVelocity;
        private float _verticalSpeed;
        private bool _canMove = true;
        private bool _isJumping;
        private Vector3 _movement = Vector3.zero;
        private Vector3 _direction = Vector2.zero;
        private Vector3 _moveDirection = Vector3.zero;
    
        // Player Damages Variables.
        private float _playerDamage;
        private readonly float _maxDamage = 999f;
    
        // Components Variables.
        private Animator _animator;
        private BattleUIManager _battleUIManager;
        private CharacterController _characterController;
        private PlayerInputs _inputs;
    
        #endregion

        #region Properties

        public float Damage => _playerDamage;
        public float VerticalSpeed
        {
            get => _verticalSpeed;
            set => _verticalSpeed = value;
        }
    
        #endregion

        #region Builtin Methods
    
        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Start()
        {
            _battleUIManager = BattleUIManager.Instance;
        
            _inputs = GetComponent<PlayerInputs>(); // Calling the PlayerInput Component.
            _characterController = GetComponent<CharacterController>(); // Calling the CharacterController component of the GameObject.
            _animator = GetComponent<Animator>(); // Calling the Animator component of the GameObject.
        }

    
        /**
         * <summary>
         * FixedUpdate is called once per fixed frame.
         * </summary>
         */
        void FixedUpdate()
        {
            // Calling movements & animations functions.
            Locomotion();
            CalculateVerticalMovement();
            UpdateAnimation();
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Function used for the player's locomotion, basic movements and view direction.
         * </summary>
         */
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

    
        /**
         * <summary>
         * Function that calculate the vertical movement of the player(The jump).
         * </summary>
         */
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
            else _verticalSpeed -= gravity * Time.deltaTime; // Apply the gravity to the player(In air).

            // Check if the player is jumping.
            if (_isJumping)
            {
                // Used to calculate the height of the jump based on the player holding the jump key.
                if (jumpTimeCounter < jumpTime)
                {
                    float jumpFactor = 1f - jumpTimeCounter / jumpTime; // JumpFactor to calculate the max height of the jump.
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

    
        /**
         * <summary>
         * Function that says if the player should move or not.
         * </summary>
         */
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
    
    
        /**
         * <summary>
         * Function that give player damage when called.
         * </summary>
         */
        public void PlayerTakeDamage(float damage)
        {
            _playerDamage = Mathf.Clamp(_playerDamage + damage, 0f, _maxDamage);
            _battleUIManager.UpdatePlayerPercentage(gameObject, _playerDamage); // Apply the damage on the UI.
        }


        /**
         * <summary>
         * Function to change the attack type of the player attack.
         * </summary>
         * <param name="attackType">The attack type.</param>
         * <param name="lightAttackType">The light attack type.</param>
         * <param name="loudAttackType">The loud attack type.</param>
         */
        private void ChangeAttackType(CharacterAttack.AttackType attackType, CharacterAttack.LightAttacks lightAttackType, CharacterAttack.LoudAttacks loudAttackType)
        {
            foreach (CharacterAttack characterAttackScript in GetComponentsInChildren<CharacterAttack>())
            {
                if (attackType == CharacterAttack.AttackType.Light)
                {
                    characterAttackScript.CurrentAttackType = attackType;
                    characterAttackScript.CurrentLightAttack = lightAttackType;
                }
                else
                {
                    characterAttackScript.CurrentAttackType = attackType;
                    characterAttackScript.CurrentLoudAttack = loudAttackType;
                }
            }
        }
    
    
        /**
         * <summary>
         * Function that update the animations on the animator.
         * </summary>
         */
        private void UpdateAnimation()
        {
            if(moveSpeed != 0f) // Check if the player move.
                _animator.SetFloat($"Velocity", _moveDirection.normalized.magnitude); // Use the running animation when he moves.

            if (!_characterController.isGrounded) // Check if the player is not on the ground.
            {
                _animator.SetBool($"isGrounded", false);
                _animator.SetFloat($"VerticalSpeed", _verticalSpeed);
            }
            else
            {
                _animator.SetBool($"isGrounded", true);
            
                // TO-DO: Apply every animations for each attack type.
                if (_inputs.Attack)
                {
                    if (_moveDirection.normalized.magnitude != 0f)
                    {
                        _animator.SetInteger($"AttackIdle", 2); // Light Side Attack.
                        ChangeAttackType(CharacterAttack.AttackType.Light, CharacterAttack.LightAttacks.SideLight, CharacterAttack.LoudAttacks.None);
                    }
                    else if (_inputs.DownMovement)
                    {
                        _animator.SetInteger($"AttackIdle", 1); // Light Down Attack.
                        ChangeAttackType(CharacterAttack.AttackType.Light, CharacterAttack.LightAttacks.DownLight, CharacterAttack.LoudAttacks.None);
                    }
                    else if (_inputs.UpMovement)
                    {
                        _animator.SetInteger($"AttackIdle", 3); // Light Up Attack.
                        ChangeAttackType(CharacterAttack.AttackType.Light, CharacterAttack.LightAttacks.UpLight, CharacterAttack.LoudAttacks.None);
                    }
                    else
                    {
                        _animator.SetInteger($"AttackIdle", 0); // Light Neutral Attack.
                        ChangeAttackType(CharacterAttack.AttackType.Light, CharacterAttack.LightAttacks.NeutralLight, CharacterAttack.LoudAttacks.None);
                    }
                    _animator.SetTrigger($"Attack");
                }
                else
                {
                    _animator.ResetTrigger($"Attack");
                    _animator.SetInteger($"AttackIdle", -1);
                }
            }
        }

        
        /**
         * <summary>
         * Coroutine for the Hit Animation.
         * </summary>
         */
        public IEnumerator UpdateHitAnimation(float hitY)
        {
            _animator.SetTrigger($"Hit");
            _animator.SetFloat($"HitY", hitY);
            yield return new WaitForSeconds(2f);
            _animator.ResetTrigger($"Hit");
        }
    
        #endregion
    }
}
