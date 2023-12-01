using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterAttack : MonoBehaviour
{
    #region Variables

    // Public Enum.
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
        UpLight
    }
    
    public enum LoudAttacks
    {
        DownLoud,
        NeutralLoud,
        SideLoud,
        UpLoud
    }
    
    // Public Variables.
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

    // Private Variables.
    private AttackType _currentAttackType;
    private LightAttacks _currentLightAttack = LightAttacks.NeutralLight;
    private LoudAttacks _currentLoudAttack;
    
    #endregion

    #region Properties
    
    public AttackType CurrentAttackType
    {
        get { return _currentAttackType; }
        set { _currentAttackType = value; }
    }
    public LightAttacks CurrentLightAttack
    {
        get { return _currentLightAttack; }
        set { _currentLightAttack = value; }
    }
    public LoudAttacks CurrentLoudAttack
    {
        get { return _currentLoudAttack; }
        set { _currentLoudAttack = value; }
    }
    
    #endregion
    
    #region Builtin Methods
    
    // When a GameObject collides with another GameObject.
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

    // Private Function that prepare the attack of a player.
    private void PrepareAttack(GameObject player)
    {
        float playerDamage = player.GetComponent<PlayerController>().Damage; // Getting the damage of the player.
                
        Attack(_currentAttackType, player);
        
        float knockbackSpeed = baseKnockbackSpeed + knockbackMultiplier * playerDamage; // Calculate the speed of the knock-back based on the player damage.
       
        Vector3 knockbackDirection = new Vector3(0f, player.GetComponent<PlayerController>().VerticalSpeed += transform.position.y/2f, 0f);
        knockbackDirection.x = (player.transform.position.x - transform.position.x); // Get the opposite direction of where the collider hit.

        player.GetComponent<PlayerController>().UpdateHitAnimation(knockbackDirection.x);
        
        // Check if the player can be knockout, or if it's just a regular Knock-back.
        if (playerDamage < 120f)
            StartCoroutine(Knockback(knockbackSpeed, knockbackDirection, player));
        else
        {
            if (CalculateOutSmashProbability(playerDamage) >= Random.Range(50f, 100f))
            {
                StartCoroutine(Knockout(knockbackDirection, player));
            }
            else
            {
                StartCoroutine(Knockback(knockbackSpeed, knockbackDirection, player));
            }
        }
    }
    
    
    // Private Function that calculate and apply the damage the player get.
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


    // Private Function that calculate the light damage based on its side.
    // Return a float value.
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
    
    
    // Private Function that calculate the loud damage based on its side.
    // Return a float value.
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
    
    
    // Private Function that calculate the knockout probability.
    // Return a float value.
    private float CalculateOutSmashProbability(float playerDamage)
    {
        float probability = baseSmashOutProbability + (playerDamage - 120f) * smashOutMultiplier;

        return probability;
    }
    
    
    // Private Function that generate a random number to see if the hit is critical.
    // Return a boolean value.
    private bool IsDamageCritical()
    {
        if(Random.Range(0, 10) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // Private IEnumerator to apply the knock-back on a player.
    private IEnumerator Knockback(float knockbackSpeed, Vector3 knockbackDirection, GameObject other)
    {
        float timer = 0f;

        while (timer < knockbackDuration)
        {
            other.GetComponent<CharacterController>().Move(knockbackDirection.normalized * (knockbackSpeed * Time.deltaTime));
            timer += Time.deltaTime;
            yield return null;
        }
    }
    
    
    // Private IEnumerator to apply the knockout knock-back on a player.
    private IEnumerator Knockout(Vector3 knockbackDirection, GameObject other)
    {
        float timer = 0f;
        
        while (timer < knockbackDuration) {
                other.GetComponent<CharacterController>().Move(knockbackDirection.normalized * (100f * Time.deltaTime)); 
                timer += Time.deltaTime;
        }
        yield return null;
    }

    #endregion
}
