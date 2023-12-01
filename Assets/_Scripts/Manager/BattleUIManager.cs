using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class BattleUIManager : MonoBehaviour
{
	#region Variables

	// Public Variables.
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

	// Public Static Variables;
	public static BattleUIManager instance;
	
	// Private Variables.
	private Color _colorLowDamage = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
	private Color _colorHighDamage = new Color(255f / 255f, 18f / 255f, 0f / 255f, 255f / 255f);
	
	// Private Static Variables.
	private GameManager _gameManager;
	
    #endregion

    #region Builtin Methods

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
	    if (instance != null)
	    {
		    Destroy(gameObject);
		    return;
	    }

	    instance = this;
    }


    // Start is called before the first frame update.
    void Start()
    {
	    _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Get GameManager component.
	    UpdatePlayersName();
    }
	
	#endregion

	#region Custom Methods

	// Public Function that update the player percentage on the UI.
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


	// Private Function that calculate the color of the damage percentage of the player.
	private void CalculateDamagePercentageColor(TMP_Text playerDamageText, float damage)
	{
		Color damageColor = Color.Lerp(_colorLowDamage, _colorHighDamage, damage / 100f); // Calculate the color of his percentage.
		
		StartCoroutine(ApplyDamageColor(playerDamageText, damageColor));
	}
	
	
	// Private Coroutine that apply the damage color to the player.
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


	// Public Function that update the timer.
	public void UpdateTimer(float timerInSeconds)
	{
		TimeSpan timer = TimeSpan.FromSeconds(timerInSeconds);
		txtTimer.text = timer.ToString("mm':'ss'.'fff"); // Transform the timer from seconds to a format.
	}

	
	// Public Function that update the player's lives on the UI.
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

	
	// Private Function that update the players names at the start of a match.
	private void UpdatePlayersName()
	{
		imgP1Name.GetComponent<Image>().sprite =
			charactersNames.Find(sprite => sprite.name == _gameManager.Player1Character);
		imgP2Name.GetComponent<Image>().sprite =
			charactersNames.Find(sprite => sprite.name == _gameManager.Player2Character);
	}

	#endregion
}
