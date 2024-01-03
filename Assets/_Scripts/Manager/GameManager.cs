using System.Collections;
using System.Collections.Generic;
using _Scripts.CharacterBehavior;
using _Scripts.Other;
using UnityEngine;

namespace _Scripts.Manager
{
	public class GameManager : MonoBehaviour
	{
		#region Variables
	
		[Header("Game Timer")]
		public float timerInSeconds = 240f;
	
		[Header("Player Parameters")]
		public List<GameObject> playersSpawnPoint;
		public List<GameObject> charactersObjects;
		public List<Material> characterMaterials;
		public List<Material> characterVisorMaterials;
	
		// Player Life Variables.
		private int _p1Life = 3, _p2Life = 3;
	
		// Managers Variables.
		private BattleUIManager _battleUIManager;
		private CameraSmashLike _camera;
		private MapManager _mapManager;
	
		// Player Static Variables.
		private static string _player1Character, _player2Character;
		private static string _winner;
	
		#endregion

		#region Properties

		public string Player1Character
		{
			get => _player1Character;
			set => _player1Character = value;
		}
		public string Player2Character{
			get => _player2Character;
			set => _player2Character = value;
		}
		public string WhoIsWinner => _winner;

		#endregion
    
		#region Builtin Methods

		/**
	     * <summary>
	     * Start is called before the first frame update.
	     * </summary>
	     */
		void Start()
		{
			// Check if it's a battle.
			if (BattleUIManager.Instance) _battleUIManager = BattleUIManager.Instance;
			if(CameraSmashLike.Instance) _camera = CameraSmashLike.Instance;
			if (MapManager.Instance)
			{
				_mapManager = MapManager.Instance;
		    
				SpawnPlayer("Player1");
				SpawnPlayer("Player2");
			}
		}

		/**
	     * <summary>
	     * Update is called once per frame.
	     * </summary>
	     */
		void Update()
		{
			// Check if there's a battleUIManager on the scene. (Which means it's a battle scene.)
			if (!_battleUIManager) return;
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
	
		#endregion

		#region Custom Methods
	
		/**
		 * <summary>
		 * Private Function that spawn a player and configure him so he's ready to go.
		 * </summary>
		 * <param name="actualPlayer">The actual Player who needs to be spawn.</param>
		 */
		// TO-DO: Change the function so we don't have to precise which player it is, that it does it automatically, if we decide to have more than two player. (Find a better way to do it.)
		private void SpawnPlayer(string actualPlayer)
		{
			int playerIndex = actualPlayer.EndsWith("1") ? 0 : 1; 
			Vector3 playerIndexSpawnPosition = playersSpawnPoint[playerIndex].transform.position;
		
			GameObject player = Instantiate(
				charactersObjects[ChooseCharacter(_player1Character)], 
				new Vector3(playerIndexSpawnPosition.x, playerIndexSpawnPosition.y + 1.5f, playerIndexSpawnPosition.z),
				Quaternion.Euler(0f, (playerIndex == 0) ? 90f : -90f, 0f)
			);
				
			player.name = actualPlayer; 
			player.layer = LayerMask.NameToLayer(actualPlayer); 
			Physics.IgnoreLayerCollision(player.layer, LayerMask.NameToLayer("Wall"), false);
		
			// Apply Player materials.
			ApplyMaterial(actualPlayer, player);
		
			// Apply Player 1 Scripts and configure them.
			ApplyScripts(player);
		
			// Camera
			_camera.CameraPlayer(player);
		}


		/**
		 * <summary>
		 * Function to apply the materials to the player.
		 * </summary>
		 * <param name="player">The name of the player.</param>
		 * <param name="actualPlayer">The GameObject of the player.</param>
		 */
		private void ApplyMaterial(string player, GameObject actualPlayer)
		{
			Material modelMaterial =
				actualPlayer.transform.Find("Model").gameObject.GetComponent<Renderer>().material;
			Material visorMaterial = actualPlayer.transform
				.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor")
				.gameObject.GetComponent<Renderer>().material;
		
			switch (player)
			{
				case "Player1":
					if (_player1Character == "Unit 4-S")
					{
						modelMaterial = characterMaterials.Find(obj => obj.name == "Unit 4-S P1");
						visorMaterial = characterVisorMaterials.Find(obj => obj.name == "Visor Unit 4-S P1");
					}
					else if (_player1Character == "Unit VTS")
					{
						modelMaterial = characterMaterials.Find(obj => obj.name == "Unit VTS P1");
						visorMaterial = characterVisorMaterials.Find(obj => obj.name == "Visor Unit VTS P1");
					}
					break;
				case "Player2":
					if (_player2Character == "Unit 4-S")
					{
						modelMaterial = characterMaterials.Find(obj => obj.name == "Unit 4-S P1");
						visorMaterial = characterVisorMaterials.Find(obj => obj.name == "Visor Unit 4-S P1");
					}
					else if (_player2Character == "Unit VTS")
					{
						modelMaterial = characterMaterials.Find(obj => obj.name == "Unit VTS P1");
						visorMaterial = characterVisorMaterials.Find(obj => obj.name == "Visor Unit VTS P1");
					}
					break;
			}
		
		}

		/**
		 * <summary>
		 * Function to apply the scripts to the player.
		 * </summary>
		 * <param name="actualPlayer">The GameObject of the player.</param>
		 */
		private void ApplyScripts(GameObject actualPlayer)
		{
			actualPlayer.AddComponent<CharacterController>();
			ConfigureCharacterControllerForPlayer(actualPlayer, _player1Character);
			actualPlayer.AddComponent<PlayerInputs>();
			actualPlayer.AddComponent<PlayerController>();
			ConfigurePlayerControllerForPlayer(actualPlayer, _player1Character);
		}


		/**
		 * <summary>
		 * Function that return the number of the character needed on the list charactersObjects.
		 * </summary>
		 * <param name="characterName">The name of the character.</param>
		 * <returns>Return the index of the character.</returns>
		 */
		private int ChooseCharacter(string characterName)
		{
			for(int i = 0; i < charactersObjects.Count; i++)
			{
				if (charactersObjects[i].name == characterName) return i;
			}

			return 0;
		}


		/**
		 * <summary>
		 * Function that controls the player CharacterController component based on the character he has.
		 * </summary>
		 * <param name="player">The player.</param>
		 * <param name="character">The character.</param>
		 */
		private void ConfigureCharacterControllerForPlayer(GameObject player, string character)
		{
			CharacterController playerCharacterController = player.GetComponent<CharacterController>();
			switch (character)
			{
				case "Unit 4-S":
					ConfigureCharacterController(
						playerCharacterController, 
						0f, 
						.3f, 
						.0001f, 
						0f, 
						new Vector3(0f, -.1f, 0f), 
						.35f, 
						1.9f
					);
					break;
				case "Unit VTS":ConfigureCharacterController(
						playerCharacterController, 
						0f, 
						.3f, 
						.0001f, 
						0f, 
						new Vector3(0f, -.25f, 0f), 
						.25f, 
						1.7f
					);
					break;
			}
		}


		/**
		 * <summary>
		 * Function to configure the character controller.
		 * </summary>
		 * <param name="characterController">The character controller.</param>
		 * <param name="slopeLimitValue">The slope limit value.</param>
		 * <param name="stepOffsetValue">The step offset value.</param>
		 * <param name="skinWidthValue">The skin width value.</param>
		 * <param name="minMoveDistanceValue">The min move distance value.</param>
		 * <param name="centerValue">The center value of the collider.</param>
		 * <param name="radiusValue">The radius value.</param>
		 * <param name="heightValue">The height value.</param>
		 */
		private void ConfigureCharacterController(CharacterController characterController, float slopeLimitValue, float stepOffsetValue, float skinWidthValue,
			float minMoveDistanceValue, Vector3 centerValue, float radiusValue, float heightValue)
		{
			characterController.slopeLimit = slopeLimitValue;
			characterController.stepOffset = stepOffsetValue;
			characterController.skinWidth = skinWidthValue;
			characterController.minMoveDistance = minMoveDistanceValue;
			characterController.center = centerValue;
			characterController.radius = radiusValue;
			characterController.height = heightValue;
		}


		/**
		 * <summary>
		 * Function that controls the player PlayerController script based on the character he has.
		 * </summary>
		 * <param name="player">The player.</param>
		 * <param name="character">The character name.</param>
		 */
		private void ConfigurePlayerControllerForPlayer(GameObject player, string character)
		{
			PlayerController actualPlayerController = player.GetComponent<PlayerController>();
			switch (character)
			{
				case "Unit 4-S":
					ConfigurePlayerController(
						actualPlayerController, 
						_mapManager.MoveSpeed, 
						_mapManager.JumpForce,
						_mapManager.Gravity, 
						_mapManager.JumpTime
					);
					break;
				case "Unit VTS":
					ConfigurePlayerController(
						actualPlayerController, 
						_mapManager.MoveSpeed + 3f, 
						_mapManager.JumpForce - .5f,
						_mapManager.Gravity - 1f, 
						_mapManager.JumpTime
					);
					break;
			}
		}


		/**
		 * <summary>
		 * Function to configure the player controller.
		 * </summary>
		 * <param name="playerController">The player controller.</param>
		 * <param name="moveSpeedValue">The move speed value.</param>
		 * <param name="jumpForceValue">The jump force value.</param>
		 * <param name="gravityValue">The gravity value.</param>
		 * <param name="jumpTimeValue">The jump time value.</param>
		 */
		private void ConfigurePlayerController(PlayerController playerController, float moveSpeedValue,
			float jumpForceValue, float gravityValue, float jumpTimeValue)
		{
			playerController.moveSpeed = moveSpeedValue;
			playerController.jumpForce = jumpForceValue;
			playerController.gravity = gravityValue;
			playerController.jumpTime = jumpTimeValue;
		}
	
	
		/**
		 * <summary>
		 * Function that take car of a player death.
		 * </summary>
		 * <param name="actualPlayer">The player that died.</param>
		 */
		public void PlayerDeath(GameObject actualPlayer)
		{
			string playerName = actualPlayer.name;
			int remainingLife = 0;
		
			// Check the player.
			if (actualPlayer.name == "Player1")
			{
				_p1Life--;
				remainingLife = _p1Life;
			}
			else if (actualPlayer.name == "Player2")
			{
				_p2Life--;
				remainingLife = _p2Life;
			}
		
			// Check if the players still have enough life.
			if (remainingLife > 0)
			{
				_battleUIManager.UpdatePlayerLife(playerName, remainingLife);
				StartCoroutine(RespawnPlayer(playerName));
			}
			else
			{
				switch (playerName)
				{
					case "Player1":
						EndGame("Player2");
						break;
					case "Player2":
						EndGame("Player1");
						break;
				}
			}
		
			Destroy(actualPlayer);
		}


		/**
		 * <summary>
		 * Coroutine to respawn the player when he die.
		 * </summary>
		 * <param name="player">The player to respawn.</param>
		 */
		private IEnumerator RespawnPlayer(string player)
		{
			yield return new WaitForSeconds(3f);
			SpawnPlayer(player);
		}


		/**
		 * <summary>
		 * Function that decide about who's the winner.
		 * </summary> 
		 */
		private void Winner()
		{
			if (_p1Life > _p2Life) EndGame("Player1"); // If P1 have more lives than P2.
			else if (_p1Life > _p2Life) EndGame("Player2"); // If P2 have more lives than P1.
			else if (GameObject.Find("Player2").GetComponent<PlayerController>().Damage > GameObject.Find("Player1").GetComponent<PlayerController>().Damage) EndGame("Player1"); // If P1 have less damage than P2 but same lives number.
			else if (GameObject.Find("Player1").GetComponent<PlayerController>().Damage > GameObject.Find("Player2").GetComponent<PlayerController>().Damage) EndGame("Player2"); // If P2 have less damage than P1 but same lives number.
			else EndGame("Nobody"); // If p1 & p2 have the same lives number and the same damage. (How did you do it ?)
		}
	
	
		/**
		 * <summary>
		 * Function that end the game.
		 * </summary>
		 * <param name="actualWinner">The winner of the game.</param>
		 */
		private void EndGame(string actualWinner)
		{
			_winner = actualWinner;
			GameObject.Find("SceneManager").GetComponent<SceneScript>().EndScene();
		}
	
		#endregion
	}
}
