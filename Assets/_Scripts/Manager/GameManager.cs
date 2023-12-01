using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Variables

	// Public Variables.
	[Header("Game Timer")]
	public float timerInSeconds = 240f;
	
	[Header("Player Parameters")]
	public List<GameObject> playersSpawnPoint;
	public List<GameObject> charactersObjects;
	public List<Material> characterMaterials;
	public List<Material> characterVisorMaterials;
	
	// Private Variables.
	private int _p1Life = 3, _p2Life = 3;
	
	private BattleUIManager _battleUIManager;
	private CameraSmashLike _camera;
	private MapManager _mapManager;
	
	// Private Static Variables.
	private static string _player1Character, _player2Character;
	private static string _winner;
	
    #endregion

    #region Properties

    public string Player1Character
    {
	    get { return _player1Character; }
	    set { _player1Character = value; }
    }
    public string Player2Character{
	    get { return _player2Character; }
	    set { _player2Character = value; }
    }
    public string WhoIsWinner => _winner;

    #endregion
    
    #region Builtin Methods

    // Start is called before the first frame update.
    void Start()
    {
	    // Check if it's a battle.
	    if (BattleUIManager.instance) _battleUIManager = BattleUIManager.instance;
	    if(CameraSmashLike.instance) _camera = CameraSmashLike.instance;
	    if (MapManager.instance)
	    {
		    _mapManager = MapManager.instance;
		    
		    SpawnPlayer("Player1");
		    SpawnPlayer("Player2");
	    }
    }

    // Update is called once per frame.
    void Update()
    {
	    // Check if there's a battleUIManager on the scene. (Which means it's a battle scene.)
	    if(_battleUIManager)
	    {
		    // Check if the timer is finished or not.
		    if (timerInSeconds > 0f)
			{
				_battleUIManager.UpdateTimer(timerInSeconds); // Update the timer on the UI.
				timerInSeconds -= Time.deltaTime;
			}
		    else
		    {
			    Winner();
		    }
	    }
    }
	
	#endregion

	#region Custom Methods
	
	// Private Function that spawn a player and configure him so he's ready to go.
	// TO-DO: Change the function so we don't have to precise which player it is, that it does it automatically, if we decide to have more than two player. (Find a better way to do it.)
	private void SpawnPlayer(string actualPlayer)
	{
		switch (actualPlayer)
		{
			case "Player1":
				GameObject player1 = Instantiate(charactersObjects[ChooseCharacter(_player1Character)], new Vector3(playersSpawnPoint[0].transform.position.x, playersSpawnPoint[0].transform.position.y + 1.5f, playersSpawnPoint[0].transform.position.z), Quaternion.Euler(0f, 90f, 0f));
				player1.name = "Player1";
				player1.layer = LayerMask.NameToLayer("Player1");
				
				// Apply Player 1 materials.
				// TO-DO: Change the way of getting the visor.
				if (_player1Character == "Unit 4-S")
				{
					player1.transform.Find("Model").gameObject.GetComponent<Renderer>().material = characterMaterials.Find(obj => obj.name == "Unit 4-S P1");
					player1.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = characterVisorMaterials.Find(obj => obj.name == "Visor Unit 4-S P1");
				}
				else if (_player1Character == "Unit VTS")
				{
					player1.transform.Find("Model").gameObject.GetComponent<Renderer>().material = characterMaterials.Find(obj => obj.name == "Unit VTS P1");
					player1.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = characterVisorMaterials.Find(obj => obj.name == "Visor Unit VTS P1");
				}
					
			
				// Apply Player 1 Scripts and configure them.
				player1.AddComponent<CharacterController>();
				ConfigureCharacterController(player1, _player1Character);
				player1.AddComponent<PlayerInputs>();
				player1.AddComponent<PlayerController>();
				ConfigurePlayerController(player1, _player1Character);
				
				// Camera
				_camera.Player1 = player1;
				break;
			case "Player2":
				GameObject player2 = Instantiate(charactersObjects[ChooseCharacter(_player2Character)], new Vector3(playersSpawnPoint[1].transform.position.x, playersSpawnPoint[1].transform.position.y + 1.5f, playersSpawnPoint[1].transform.position.z), Quaternion.Euler(0f, -90f, 0f));
				player2.name = "Player2";
				player2.layer = LayerMask.NameToLayer("Player2");
				
				// Apply Player 2 materials.
				// TO-DO: Change the way of gettiing the visor.
				if (_player2Character == "Unit 4-S")
				{
					player2.transform.Find("Model").gameObject.GetComponent<Renderer>().material = characterMaterials.Find(obj => obj.name == "Unit 4-S P2");
					player2.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = characterVisorMaterials.Find(obj => obj.name == "Visor Unit 4-S P2");
				}
				else if (_player2Character == "Unit VTS")
				{
					player2.transform.Find("Model").gameObject.GetComponent<Renderer>().material = characterMaterials.Find(obj => obj.name == "Unit VTS P2");
					player2.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = characterVisorMaterials.Find(obj => obj.name == "Visor Unit VTS P2");
				}
			
				// Apply Player 2 Scripts and configure them.
				player2.AddComponent<CharacterController>();
				ConfigureCharacterController(player2, _player2Character);
				player2.AddComponent<PlayerInputs>();
				player2.AddComponent<PlayerController>();
				ConfigurePlayerController(player2, _player2Character);
				
				// Camera
				_camera.Player2 = player2;
				break;
		}
	}


	// Private Function that return the number of the character needed on the list charactersObjects.
	// Return an integer value.
	private int ChooseCharacter(string characterName)
	{
		for(int i = 0; i < charactersObjects.Count; i++)
		{
			if (charactersObjects[i].name == characterName) return i;
		}

		return 0;
	}


	// Private Function that controls the player CharacterController component based on the character he has.
	private void ConfigureCharacterController(GameObject player, string character)
	{
		CharacterController playerCharacterController = player.GetComponent<CharacterController>();
		switch (character)
		{
			case "Unit 4-S":
				playerCharacterController.slopeLimit = 0f;
				playerCharacterController.stepOffset = .3f;
				playerCharacterController.skinWidth = .0001f;
				playerCharacterController.minMoveDistance = 0;
				playerCharacterController.center = new Vector3(0f, -.1f, 0f);
				playerCharacterController.radius = .35f;
				playerCharacterController.height = 1.9f;
				break;
			case "Unit VTS":
				playerCharacterController.slopeLimit = 0f;
				playerCharacterController.stepOffset = .3f;
				playerCharacterController.skinWidth = .0001f;
				playerCharacterController.minMoveDistance = 0;
				playerCharacterController.center = new Vector3(0f, -.25f, 0f);
				playerCharacterController.radius = .25f;
				playerCharacterController.height = 1.7f;
				break;
		}
	}


	// Private Function that controls the player PlayerController script based on the character he has.
	private void ConfigurePlayerController(GameObject player, string character)
	{
		PlayerController actualPlayerController = player.GetComponent<PlayerController>();
		switch (character)
		{
			case "Unit 4-S":
				actualPlayerController.moveSpeed = _mapManager.MoveSpeed;
				actualPlayerController.jumpForce = _mapManager.JumpForce;
				actualPlayerController.gravity = _mapManager.Gravity;
				actualPlayerController.jumpTime = _mapManager.JumpTime;
				break;
			case "Unit VTS":
				actualPlayerController.moveSpeed = _mapManager.MoveSpeed + 3f;
				actualPlayerController.jumpForce = _mapManager.JumpForce - .5f;
				actualPlayerController.gravity = _mapManager.Gravity - 1f;
				actualPlayerController.jumpTime = _mapManager.JumpTime;
				break;
		}
	}
	
	
	// Public Function that take car of a player death.
	// TO-DO: Change the function so we don't have to precise which player it is, that it does it automatically, if we decide to have more than two player.
	public void PlayerDeath(GameObject actualPlayer)
	{
		// Check the player.
		if (actualPlayer.name == "Player1")
		{
			_p1Life--;
			
			// Check if he still have lives.
			if (_p1Life > 0)
			{
				_battleUIManager.UpdatePlayerLife("Player1", _p1Life);
				StartCoroutine(RespawnPlayer("Player1"));
			}
			else EndGame("Player2");
		}
		else if (actualPlayer.name == "Player2")
		{
			_p2Life--;
			
			// Check if he still have lives.
			if (_p2Life > 0)
			{
				_battleUIManager.UpdatePlayerLife("Player2", _p2Life);
				StartCoroutine(RespawnPlayer("Player2"));
			}
			else EndGame("Player1");
		}
		
		Destroy(actualPlayer);
	}


	// Private Coroutine to respawn the player when he die.
	// TO-DO: Change the coroutine so we don't have to precise which player it is, that it does it automatically, if we decide to have more than two player.
	private IEnumerator RespawnPlayer(string player)
	{
		switch (player)
		{
			case "Player1":
				yield return new WaitForSeconds(3f);
				SpawnPlayer("Player1");
				break;
			case "Player2":
				yield return new WaitForSeconds(3f);
				SpawnPlayer("Player2");
				break;
		}

		yield return null;
	}


	// Private Function that decide about who's the winner.
	// TO-DO: Find a better way to do that.
	private void Winner()
	{
		if (_p1Life > _p2Life) EndGame("Player1"); // If P1 have more lives than P2.
		else if (_p1Life > _p2Life) EndGame("Player2"); // If P2 have more lives than P1.
		else if (GameObject.Find("Player2").GetComponent<PlayerController>().Damage > GameObject.Find("Player1").GetComponent<PlayerController>().Damage) EndGame("Player1"); // If P1 have less damage than P2 but same lives number.
		else if (GameObject.Find("Player1").GetComponent<PlayerController>().Damage > GameObject.Find("Player2").GetComponent<PlayerController>().Damage) EndGame("Player2"); // If P2 have less damage than P1 but same lives number.
		else EndGame("Nobody"); // If p1 & p2 have the same lives number and the same damage. (How did you do it ?)
	}
	
	
	// Private Function that end the game.
	private void EndGame(string actualWinner)
	{
		_winner = actualWinner;
		GameObject.Find("SceneManager").GetComponent<SceneScript>().EndScene();
	}
	
	#endregion
}
