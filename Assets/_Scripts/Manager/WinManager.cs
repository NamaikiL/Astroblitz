using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Scripts.Manager
{
    public class WinManager : MonoBehaviour
    {
        #region Variables
    
        [Header("Victory Parameters")]
        public List<GameObject> spawnPoint;
        public List<GameObject> characterModels;
        public List<Material> player2Materials;
        public List<Material> player2VisorMaterials;
        public TMP_Text txtWinner;

        [Header("Audio")] 
        public AudioSource player1Win;
        public AudioSource player2Win;
    
        #endregion

        #region Builtin Methods
    
        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Start()
        {
            CheckWin();
        }
    
        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Function that check who has win and show it to the screen. (Had to do it fast.)
         * </summary>
         */
        private void CheckWin()
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Finding the GameManager.
        
            // Getting the spawn points.
            GameObject soloPoint = spawnPoint[spawnPoint.FindIndex(point => point.name == "SoloPoint")];
            GameObject p1PointNW = spawnPoint[spawnPoint.FindIndex(point => point.name == "P1NWPoint")];
            GameObject p2PointNW = spawnPoint[spawnPoint.FindIndex(point => point.name == "P2NWPoint")];

            switch (gameManager.WhoIsWinner)
            {
                case "Player1":
                    player1Win.Play();
                    txtWinner.text = "Player 1 Win !";
                
                    // Player 1 instantiate.
                    GameObject player1 =
                        characterModels[characterModels.FindIndex(character => character.name == gameManager.Player1Character)];
                    player1 = Instantiate(player1, soloPoint.transform.position, Quaternion.Euler(0f, 180f,0f));
                    player1.GetComponent<Animator>().SetBool("Victory", true);
                
                    break;
                case "Player2":
                    player2Win.Play();
                    txtWinner.text = "Player 2 Win !";
                
                    // Player 2 instantiate.
                    GameObject player2 =
                        characterModels[characterModels.FindIndex(character => character.name == gameManager.Player2Character)];
                    player2 = Instantiate(player2, soloPoint.transform.position, Quaternion.Euler(0f, 180f,0f));
                    player2.GetComponent<Animator>().SetBool("Victory", true);
                
                    // Apply Materials to player 2.
                    if (gameManager.Player2Character == "Unit 4-S")
                    {
                        player2.transform.Find("Model").gameObject.GetComponent<Renderer>().material = player2Materials.Find(obj => obj.name == "Unit 4-S P2");
                        player2.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = player2VisorMaterials.Find(obj => obj.name == "Visor Unit 4-S P2");
                    }
                    else if (gameManager.Player2Character == "Unit VTS")
                    {
                        player2.transform.Find("Model").gameObject.GetComponent<Renderer>().material = player2Materials.Find(obj => obj.name == "Unit VTS P2");
                        player2.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = player2VisorMaterials.Find(obj => obj.name == "Visor Unit VTS P2");
                    }
                
                    break;
                case "Nobody":
                    txtWinner.text = "Nobody Win !";
                
                    // Player 1 & 2 instantiate.
                    GameObject player1NW =
                        characterModels[characterModels.FindIndex(character => character.name == gameManager.Player1Character)];
                    player1NW = Instantiate(player1NW, p1PointNW.transform.position,Quaternion.Euler(0f, 180f,0f));
                    player1NW.GetComponent<Animator>().SetBool("Victory", true);
                
                    GameObject player2NW =
                        characterModels[characterModels.FindIndex(character => character.name == gameManager.Player2Character)];
                    player2NW = Instantiate(player2NW, p2PointNW.transform.position, Quaternion.Euler(0f, 180f,0f));
                    player2NW.GetComponent<Animator>().SetBool("Victory", true);
                
                    // Apply Materials to player 2.
                    if (gameManager.Player2Character == "Unit 4-S")
                    {
                        player2NW.transform.Find("Model").gameObject.GetComponent<Renderer>().material = player2Materials.Find(obj => obj.name == "Unit 4-S P2");
                        player2NW.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = player2VisorMaterials.Find(obj => obj.name == "Visor Unit 4-S P2");
                    }
                    else if (gameManager.Player2Character == "Unit VTS")
                    {
                        player2NW.transform.Find("Model").gameObject.GetComponent<Renderer>().material = player2Materials.Find(obj => obj.name == "Unit VTS P2");
                        player2NW.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Visor").gameObject.GetComponent<Renderer>().material = player2VisorMaterials.Find(obj => obj.name == "Visor Unit VTS P2");
                    }
                
                    break;
            }
            
        }
    
        #endregion
    }
}
