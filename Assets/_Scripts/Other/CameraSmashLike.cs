using UnityEngine;

public class CameraSmashLike : MonoBehaviour
{
	#region Variables

	// Public Variables.
	[Header("Camera Parameters")]
	public float smoothSpeed = 1f;
	public float positionY;
	public float closeDistancePosY;
	public float minX, maxX;
	public float minY, maxY;
	public float zoomMin, zoomMax, zoomThreshold;

	// Public Static Variables.
	public static CameraSmashLike instance;
	
	// Private Variables.
	private float _basePositionY;
	
	private GameObject _player1, _player2;
	private Vector3 _cameraPosition;
	
    #endregion

    #region Properties

    public GameObject Player1
    {
	    get { return _player1; }
	    set { _player1 = value; }
    }
    public GameObject Player2
    {
	    get { return _player2; }
	    set { _player2 = value; }
    }

    #endregion
    
    #region Builtin Methods

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
	    if (instance != null)
	    {
		    Destroy(instance);
		    return;
	    }

	    instance = this;
    }
    
    
    // Start is called before the first frame update.
    void Start()
    {
	    _cameraPosition = transform.position;
	    _basePositionY = positionY;
    }

    
    // Update is called once per frame.
    void Update()
    {
	    Vector3 targetsPosition = CalculateTargetsPositionAndDynamicCamera();
	    transform.position = Vector3.Lerp(transform.position, targetsPosition, smoothSpeed * Time.deltaTime); // Apply the dynamic script to the camera.
    }
	
	#endregion

	#region Custom Methods

	// Private Function that calculate the targets positions and make the camera dynamic.
	// Return a Vector3 value.
	private Vector3 CalculateTargetsPositionAndDynamicCamera()
	{
		Vector3 midPoint = new Vector3();
		float zoomFactor = 0f;
		
		// Check if both player are on the scene.
		if (_player1 && _player2)
		{
			midPoint = (_player1.transform.position + _player2.transform.position) / 2f; // Calculate the mid point between the two players.

			float distance = Vector3.Distance(_player1.transform.position, _player2.transform.position);
		
			// Check if they are close, to change the Y value, in case the zoom is too much.
			if (distance < 3) positionY = closeDistancePosY;
			else positionY = _basePositionY;
		
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

	#endregion
}
