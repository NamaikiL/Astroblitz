using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.CharacterBehavior
{
    public class CharacterAttack : MonoBehaviour
    {
        #region Variables
    
        public enum AttackType
        {
            Light,
            Loud
        }
    
        public enum LightAttacks
        {
            DownLight,
            NeutralLight,
            SideLight,
            UpLight,
            None
        }
    
        public enum LoudAttacks
        {
            DownLoud,
            NeutralLoud,
            SideLoud,
            UpLoud,
            None
        }
    
        [Header("Knock-back values")] 
        public float baseKnockbackSpeed = 5f;
        public float knockbackMultiplier = 0.1f;
        public float knockbackDuration = 0.5f;
    
        [Header("Light Damage")] 
        public float minLightDamage;
        public float maxLightDamage;
    
        [Header("Loud Damage")] 
        public float minLoudDamage; 
        public float maxLoudDamage;
    
        [Header("Smash Out")] 
        public float baseSmashOutProbability = 10f;
        public float smashOutMultiplier = 0.5f;

        [Header("Audio")] 
        public AudioSource hitAudio;

        // Attack Type Variables.
        private AttackType _currentAttackType;
        private LightAttacks _currentLightAttack = LightAttacks.NeutralLight;
        private LoudAttacks _currentLoudAttack;
    
        #endregion

        #region Properties
    
        public AttackType CurrentAttackType
        {
            set => _currentAttackType = value;
        }
        public LightAttacks CurrentLightAttack
        {
            set => _currentLightAttack = value;
        }
        public LoudAttacks CurrentLoudAttack
        {
            set => _currentLoudAttack = value;
        }
    
        #endregion
    
        #region Builtin Methods
    
        /**
         * <summary>
         * When a GameObject collides with another GameObject.
         * </summary>
         * <param name="other">The other Collider involved in this collision.</param>
         */
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // Check if the player the collider is hitting is not its parent self.
                if (other.gameObject.layer != gameObject.transform.root.gameObject.layer)
                {
                    hitAudio.Play();
                    PrepareAttack(other.gameObject);
                }
            }
        }

        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Function that prepare the attack of a player.
         * </summary>
         * <param name="player">The player taking the hit.</param>
         */
        private void PrepareAttack(GameObject player)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            float playerDamage = playerController.Damage; // Getting the damage of the player.
                
            Attack(_currentAttackType, player);
        
            float knockbackSpeed = baseKnockbackSpeed + knockbackMultiplier * playerDamage; // Calculate the speed of the knock-back based on the player damage.

            Vector3 attackPosition = transform.position;
            Vector3 knockbackDirection = 
                new Vector3(0f, playerController.VerticalSpeed += attackPosition.y/2f, 0f);
            knockbackDirection.x = (player.transform.position.x - attackPosition.x); // Get the opposite direction of where the collider hit.

            StartCoroutine(playerController.UpdateHitAnimation(knockbackDirection.x));
        
            // Check if the player can be knockout, or if it's just a regular Knock-back.
            if (playerDamage < 120f)
                StartCoroutine(Knockback(knockbackSpeed, knockbackDirection, player));
            else
            {
                StartCoroutine(CalculateOutSmashProbability(playerDamage) >= Random.Range(50f, 100f)
                    ? Knockout(knockbackDirection, player)
                    : Knockback(knockbackSpeed, knockbackDirection, player));
            }
        }
    
    
        /**
         * <summary>
         * Function that calculate and apply the damage the player get.
         * </summary>
         * <param name="attackType">The attack type of the player.</param>
         * <param name="player">The player taking the damage.</param>
         */
        private void Attack(AttackType attackType, GameObject player)
        {
            float damage = 0f; // Base damage value.
        
            // Check which type of attack it is.
            if (attackType == AttackType.Light)
            {
                damage = Random.Range(minLightDamage, maxLightDamage);
            
                // Check if it's a critical hit.
                if (IsDamageCritical())
                    damage = CalculateLightDamage(damage, _currentLightAttack) * 2f;
                else
                {
                    damage = CalculateLightDamage(damage, _currentLightAttack);
                }
            }
            else if (attackType == AttackType.Loud)
            {
                damage = Random.Range(minLoudDamage, maxLoudDamage);
            
                // Check if it's a critical hit.
                if (IsDamageCritical())
                    damage = CalculateLoudDamage(damage, _currentLoudAttack) * 2f;
                else
                {
                    damage = CalculateLoudDamage(damage, _currentLoudAttack);
                }
            }
        
            player.GetComponent<PlayerController>().PlayerTakeDamage(damage); // Apply the damage.
        }


        /**
         * <summary>
         * Function that calculate the light damage based on its side.
         * </summary>
         * <param name="currentDamage">The current damage of the player.</param>
         * <param name="lightAttack">The current type of attack of the player.</param>
         * <returns>The damage of the attack.</returns>
         */
        private float CalculateLightDamage(float currentDamage, LightAttacks lightAttack)
        {
            switch (lightAttack)
            {
                case LightAttacks.DownLight:
                    return currentDamage * 0.8f;
                case LightAttacks.NeutralLight:
                    return currentDamage;
                case LightAttacks.SideLight:
                    return currentDamage * 1.2f;
                case LightAttacks.UpLight:
                    return currentDamage * 1.1f;
                default:
                    return currentDamage;
            }
        }
    
    
        /**
         * <summary>
         * Function that calculate the loud damage based on its side.
         * </summary>
         * <param name="currentDamage">The current damage of the player.</param>
         * <param name="loudAttack">The current type of attack of the player.</param>
         * <returns>The damage of the attack.</returns>
         */
        private float CalculateLoudDamage(float currentDamage, LoudAttacks loudAttack)
        {
            switch (loudAttack)
            {
                case LoudAttacks.DownLoud:
                    return currentDamage * 0.8f;
                case LoudAttacks.NeutralLoud:
                    return currentDamage;
                case LoudAttacks.SideLoud:
                    return currentDamage * 1.2f;
                case LoudAttacks.UpLoud:
                    return currentDamage * 1.1f;
                default:
                    return currentDamage;
            }
        }
    
    
        /**
         * <summary>
         * Function that calculate the knockout probability.
         * </summary>
         * <param name="playerDamage">The actual damage of the player.</param>
         * <returns>The probability of a knock-out</returns>
         */
        private float CalculateOutSmashProbability(float playerDamage)
        {
            float probability = baseSmashOutProbability + (playerDamage - 120f) * smashOutMultiplier;

            return probability;
        }
    
    
        /**
         * <summary>
         * Function that generate a random number to see if the hit is critical.
         * </summary>
         * <returns>The result of the critical chance.</returns>
         */
        // Return a boolean value.
        private bool IsDamageCritical()
        {
            return Random.Range(0, 10) == 1;
        }


        /**
         * <summary>
         * IEnumerator to apply the knock-back on a player.
         * </summary>
         * <param name="knockbackSpeed">The speed of the player knock-back.</param>
         * <param name="knockbackDirection">The direction of the player knock-back.</param>
         * <param name="player">The actual player of which the knock-back is affected.</param>
         */
        private IEnumerator Knockback(float knockbackSpeed, Vector3 knockbackDirection, GameObject player)
        {
            float timer = 0f;

            while (timer < knockbackDuration)
            {
                player.GetComponent<CharacterController>().Move(knockbackDirection.normalized * (knockbackSpeed * Time.deltaTime));
                timer += Time.deltaTime;
                yield return null;
            }
        }
    
    
        /**
         * <summary>
         * Coroutine to apply the knockout knock-back on a player.
         * </summary>
         * <param name="knockbackDirection">The direction of the knock-out.</param>
         * <param name="player">The player affected by the knock-out.</param>
         */
        private IEnumerator Knockout(Vector3 knockbackDirection, GameObject player)
        {
            Physics.IgnoreLayerCollision(player.layer, 1<<9, true);
            float timer = 0f;
        
            while (timer < knockbackDuration) {
                player.GetComponent<CharacterController>().Move(knockbackDirection.normalized * (100f * Time.deltaTime)); 
                timer += Time.deltaTime;
            }
            yield return null;
        }

        #endregion
    }
}
