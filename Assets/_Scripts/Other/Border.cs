using UnityEngine;

public class Border : MonoBehaviour
{
	#region Variables

	// Private Variables.
	private GameManager _gameManager;
	
    #endregion

    #region Builtin Methods

    // Start is called before the first frame update.
    void Start()
    {
	    _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Get the GameManager component.
    }

    
    // When a GameObject collides with another GameObject.
	void OnTriggerEnter(Collider other)
    {
	    // Check if a player touch the border.
	    if (other.gameObject.CompareTag("Player"))
	    {
		    _gameManager.PlayerDeath(other.gameObject); // Call his death function.
	    }
    }

    #endregion
}
