using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.Manager
{
    public class MainUIManager : MonoBehaviour
    {
        #region Variables
    
        [Header("Menus")] 
        public GameObject menuPlay;
        public GameObject menuOptions;
        public GameObject menuCredits;
        public GameObject menuMain;
        public GameObject menuChooseCharacter;
        public GameObject menuChooseMap;
    
        [Header("Characters Choose Assets")] 
        public GameObject txtCharacterOne;
        public GameObject txtCharacterTwo;
        public GameObject imgCharacterOne;
        public GameObject imgCharacterTwo;
        public List<Sprite> charactersNames;
        public List<Sprite> characterP1Img;
        public List<Sprite> characterP2Img;
    
        [Header("Audio")] 
        public AudioSource button;
    
        // Characters Index Variables.
        private int _characterOneIndex, _characterTwoIndex;
    
        #endregion

        #region Builtin Methods
    
        /**
         * <summary>
         * Start is called before the first frame update.
         * </summary>
         */
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenus();
            }
        }
    
        #endregion

        #region Custom Methods

        /**
         * <summary>
         * Function that go to the play menu.
         * </summary>
         */
        public void Play()
        {
            button.Play();
            menuMain.SetActive(false);
            menuPlay.SetActive(true);
        }


        /**
         * <summary>
         * Function that go to the map menu.
         * </summary> 
         */
        public void GoToMap()
        {
            button.Play();
            GameObject.Find("GameManager").GetComponent<GameManager>().Player1Character = txtCharacterOne.GetComponent<Image>().sprite.name; // Get the Player1 Character.
            GameObject.Find("GameManager").GetComponent<GameManager>().Player2Character = txtCharacterTwo.GetComponent<Image>().sprite.name; // Get the Player2 Character.
        
            menuChooseCharacter.SetActive(false);
            menuChooseMap.SetActive(true);
        }


        /**
         * <summary>
         * Function that go to the Controls menu.
         * </summary>
         */
        public void Options()
        {
            button.Play();
            menuMain.SetActive(false);
            menuOptions.SetActive(true);
        }


        /**
         * <summary>
         * Function that go to the Credits menu.
         * </summary>
         */
        public void Credits()
        {
            button.Play();
            menuMain.SetActive(false);
            menuCredits.SetActive(true);
        }


        /**
         * <summary>
         * Function that Quit the game.
         * </summary>
         */
        public void Quit()
        {
            button.Play();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }


        /**
         * <summary>
         * Function that quit menus based on which they are.
         * </summary> 
         */
        private void CloseMenus()
        {
            button.Play();
            if (menuOptions.activeSelf)
            {
                menuOptions.SetActive(false);
                menuMain.SetActive(true);
            }
            else if (menuCredits.activeSelf)
            {
                menuCredits.SetActive(false);
                menuMain.SetActive(true);
            }
            else if (menuChooseMap.activeSelf)
            {
                menuChooseMap.SetActive(false);
                menuChooseCharacter.SetActive(true);
            }
            else if (menuPlay.activeSelf)
            {
                menuPlay.SetActive(false);
                menuMain.SetActive(true);
            }
        }


        /**
         * <summary>
         * Function that update the character chooser menu when player click on a button.
         * </summary>
         */
        public void BtnCharacterOneChoose()
        {
            button.Play();
            if (EventSystem.current.currentSelectedGameObject.name == "BtnRightArrow")
            {
                _characterOneIndex = (_characterOneIndex + 1) % charactersNames.Count;
                txtCharacterOne.GetComponent<Image>().sprite = charactersNames[_characterOneIndex];
                imgCharacterOne.GetComponent<Image>().sprite = characterP1Img[_characterOneIndex];
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "BtnLeftArrow")
            {
                _characterOneIndex = (_characterOneIndex - 1 + charactersNames.Count) % charactersNames.Count;
                txtCharacterOne.GetComponent<Image>().sprite = charactersNames[_characterOneIndex];
                imgCharacterOne.GetComponent<Image>().sprite = characterP1Img[_characterOneIndex];
            }
        }
    
    
        /**
         * <summary>
         * Function that update the character chooser menu when player click on a button.
         * </summary>
         */
        public void BtnCharacterTwoChoose()
        {
            button.Play();
            if (EventSystem.current.currentSelectedGameObject.name == "BtnRightArrow")
            {
                _characterTwoIndex = (_characterTwoIndex + 1) % charactersNames.Count;
                txtCharacterTwo.GetComponent<Image>().sprite = charactersNames[_characterTwoIndex];
                imgCharacterTwo.GetComponent<Image>().sprite = characterP2Img[_characterTwoIndex];
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "BtnLeftArrow")
            {
                _characterTwoIndex = (_characterTwoIndex - 1 + charactersNames.Count) % charactersNames.Count;
                txtCharacterTwo.GetComponent<Image>().sprite = charactersNames[_characterTwoIndex];
                imgCharacterTwo.GetComponent<Image>().sprite = characterP2Img[_characterTwoIndex];
            }
        }
    
        #endregion

    }
}
