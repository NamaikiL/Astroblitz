using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class BattleUIManager : MonoBehaviour
{
	#region Variables
	
	[Header("Players Damage Texts")] 
	public TMP_Text txtP1Damage;
	public TMP_Text txtP2Damage;
	
	[Header("Battle Timer")]
	public TMP_Text txtTimer;
	
	[Header("Characters Names")] 
	public GameObject imgP1Name;
	public GameObject imgP2Name;
	public List<Sprite> charactersNames;
	
	[Header("PlayersLife")] 
	public List<GameObject> p1Life; 
	public List<GameObject> p2Life;
	
	// Color Percentage Variables.
	private Color _colorLowDamage = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	private Color _colorHighDamage = new Color(255f / 255f, 18f / 255f, 0f / 255f, 255f / 255f);
	
	// Managers Variables.
	private GameManager _gameManager;
	
	// Instance Variable;
	private static BattleUIManager _instance;
	
    #endregion

    #region Properties

    public static BattleUIManager Instance => _instance;

    #endregion
    
    #region Builtin Methods

    /**
     * <summary>
     * Awake is called when the script instance is being loaded.
     * </summary>
     */
    void Awake()
    {
	    if (_instance) Destroy(gameObject);
	    _instance = this;
    }


    /**
     * <summary>
     * Start is called before the first frame update.
     * </summary>
     */
    void Start()
    {
	    _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Get GameManager component.
	    UpdatePlayersName();
    }
	
	#endregion

	#region Custom Methods

	/**
	 * <summary>
	 * Function that update the player percentage on the UI.
	 * </summary>
	 * <param name="player">The player affected by the changes.</param>
	 * <param name="damage">The damage of the player.</param>
	 */
	public void UpdatePlayerPercentage(GameObject player, float damage)
	{
		if (player.name == "Player1")
		{
			txtP1Damage.text = (int)damage + "%";
			CalculateDamagePercentageColor(txtP1Damage, (int)damage);
		}
		else if (player.name == "Player2")
		{
			txtP2Damage.text = (int)damage + "%";
			CalculateDamagePercentageColor(txtP2Damage, (int)damage);
		}
	}


	/**
	 * <summary>
	 * Function that calculate the color of the damage percentage of the player.
	 * </summary>
	 * <param name="playerDamageText">The text of the player.</param>
	 * <param name="damage">The damage of the player.</param>
	 */
	private void CalculateDamagePercentageColor(TMP_Text playerDamageText, float damage)
	{
		Color damageColor = Color.Lerp(_colorLowDamage, _colorHighDamage, damage / 100f); // Calculate the color of his percentage.
		
		StartCoroutine(ApplyDamageColor(playerDamageText, damageColor));
	}
	
	
	/**
	 * <summary>
	 * Coroutine that apply the damage color to the player.
	 * </summary>
	 * <param name="playerDamageText">The text of the player.</param>
	 * <param name="damageColor">The damage color.</param>
	 */
	private IEnumerator ApplyDamageColor(TMP_Text playerDamageText, Color damageColor)
	{
		float timer = 0f;

		while (timer < 2f)
		{
			playerDamageText.color = Color.Lerp(playerDamageText.color, damageColor, timer/2f); // Apply the color of his percentage based on his actual color.
			timer += Time.deltaTime;
			yield return null;
		}
	}


	/**
	 * <summary>
	 * Function that update the timer.
	 * </summary>
	 * <param name="timerInSeconds">The timer of the game.</param>
	 */
	public void UpdateTimer(float timerInSeconds)
	{
		TimeSpan timer = TimeSpan.FromSeconds(timerInSeconds);
		txtTimer.text = timer.ToString("mm':'ss'.'fff"); // Transform the timer from seconds to a format.
	}

	
	/**
	 * <summary>
	 * Function that update the player's lives on the UI.
	 * </summary>
	 * <param name="actualPlayer">The player.</param>
	 * <param name="playerLife">The life of the player.</param>
	 */
	public void UpdatePlayerLife(string actualPlayer, int playerLife)
	{
		switch (actualPlayer)
		{
			case "Player1":
				p1Life[playerLife].SetActive(false);
				txtP1Damage.text = "0%";
				txtP1Damage.color = _colorLowDamage;
				break;
			case "Player2":
				p2Life[playerLife].SetActive(false);
				txtP2Damage.text = "0%";
				txtP2Damage.color = _colorLowDamage;
				break;
			default:
				Debug.Log("Error Lives");
				break;
		}
	}

	
	/**
	 * <summary>
	 * Function that update the players names at the start of a match.
	 * </summary> 
	 */
	private void UpdatePlayersName()
	{
		imgP1Name.GetComponent<Image>().sprite =
			charactersNames.Find(sprite => sprite.name == _gameManager.Player1Character);
		imgP2Name.GetComponent<Image>().sprite =
			charactersNames.Find(sprite => sprite.name == _gameManager.Player2Character);
	}

	#endregion
}
