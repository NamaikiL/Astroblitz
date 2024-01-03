using _Scripts.Manager;
using UnityEngine;

namespace _Scripts.Other
{
	public class Border : MonoBehaviour
	{
		#region Variables

		// Managers Variables.
		private GameManager _gameManager;
	
		#endregion

		#region Builtin Methods

		/**
	     * <summary>
	     * Start is called before the first frame update.
	     * </summary>
	     */
		void Start()
		{
			_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Get the GameManager component.
		}

    
		/**
	     * <summary>
	     * When a GameObject collides with another GameObject.
	     * </summary>
	     * <param name="other">The other Collider involved in this collision.</param>
	     */
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
}
