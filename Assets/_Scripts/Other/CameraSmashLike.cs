using UnityEngine;

namespace _Scripts.Other
{
	public class CameraSmashLike : MonoBehaviour
	{
		#region Variables
	
		[Header("Camera Parameters")]
		public float smoothSpeed = 1f;
		public float positionY;
		public float closeDistancePosY;
		public float minX, maxX;
		public float minY, maxY;
		public float zoomMin, zoomMax, zoomThreshold;
	
		// Private Variables.
		private float _basePositionY;
	
		private GameObject _player1, _player2;
		private Vector3 _cameraPosition;
	
		// Instance Variables.
		private static CameraSmashLike _instance;
	
		#endregion

		#region Properties

		public static CameraSmashLike Instance => _instance;

		#endregion
    
		#region Builtin Methods

		/**
	     * <summary>
	     * Awake is called when the script instance is being loaded.
	     * </summary>
	     */
		void Awake()
		{
			if (_instance != null) Destroy(_instance);
			_instance = this;
		}
    
    
		/**
	     * <summary>
	     * Start is called before the first frame update.
	     * </summary>
	     */
		void Start()
		{
			_cameraPosition = transform.position;
			_basePositionY = positionY;
		}

    
		/**
		 * <summary>
		 * Update is called once per frame.
		 * </summary>
		 */
		void Update()
		{
			Vector3 targetsPosition = CalculateTargetsPositionAndDynamicCamera();
			transform.position = Vector3.Lerp(transform.position, targetsPosition, smoothSpeed * Time.deltaTime); // Apply the dynamic script to the camera.
		}
	
		#endregion

		#region Custom Methods

		/**
		 * <summary>
		 * Private Function that calculate the targets positions and make the camera dynamic.
		 * </summary>
		 * <returns>The position of the camera</returns>
		 */
		private Vector3 CalculateTargetsPositionAndDynamicCamera()
		{
			Vector3 midPoint;
			float zoomFactor;
		
			// Check if both player are on the scene.
			if (_player1 && _player2)
			{
				Vector3 player1Position = _player1.transform.position;
				Vector3 player2Position = _player2.transform.position;
				midPoint = (player1Position + player2Position) / 2f; // Calculate the mid point between the two players.

				float distance = Vector3.Distance(player1Position, player2Position);
		
				// Check if they are close, to change the Y value, in case the zoom is too much.
				positionY = distance < 3 ? closeDistancePosY : _basePositionY;
		
				zoomFactor = Mathf.Lerp(zoomMin, zoomMax, distance / zoomThreshold); // Calculate the zoom based on the distance of the two players

				zoomFactor = Mathf.Clamp(zoomFactor, zoomMin, zoomMax); // Limit the Zoom.
			}
			else if (_player1 && !_player2) // If there's only Player1
			{
				midPoint = _player1.transform.position;
				positionY = closeDistancePosY;
				zoomFactor = -5f;
			}
			else if (!_player1 && _player2) // If there's only Player2
			{
				midPoint = _player2.transform.position;
				positionY = closeDistancePosY;
				zoomFactor = -5f;
			}
			else // When there's no player, reset the camera to it's base position.
			{
				midPoint = _cameraPosition;
				zoomFactor = 0f;
			}
		
			float clampX = Mathf.Clamp(midPoint.x, minX, maxX); // Limit the camera on X.
			float clampY = Mathf.Clamp(midPoint.y, minY, maxY); // Limit the camera on Y.
		
			return new Vector3(clampX, clampY + positionY, _cameraPosition.z - zoomFactor);
		}


		/**
		 * <summary>
		 * Function to add the player to the camera.
		 * </summary>
		 * <param name="player">The player to add.</param>
		 */
		public void CameraPlayer(GameObject player)
		{
			switch (player.name)
			{
				case "Player1":
					_player1 = player;
					break;
				case "Player2":
					_player2 = player;
					break;
			}
		}

		#endregion
	}
}
