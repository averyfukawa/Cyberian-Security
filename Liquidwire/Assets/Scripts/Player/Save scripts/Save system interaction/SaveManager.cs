using System.Collections.Generic;
using Player.Raycasting;
using Player.Save_scripts.Artificial_dictionaries;
using Player.Save_scripts.Save_and_Load_scripts;
using UI.Browser;
using UI.Translation;
using UI.Tutorial;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Save_scripts.Save_system_interaction
{
    public class SaveManager: MonoBehaviour
    {
        [SerializeField] private float maxInteractDistance;
        /// <summary>
        /// List of all the tab prefabs
        /// </summary>
        [FormerlySerializedAs("tabdict")] public List<TabPrefabDictionary> tabDictList;
        /// <summary>
        /// List of all the email prefabs
        /// </summary>
        [FormerlySerializedAs("mailDict")] public List<EmailListingDictionary> mailDictList;

        [SerializeField] [TextArea(2, 2)] private string[] _languageOptions = new string[2];
        private LanguageScript.Languages _currentLanguage;

        private GameObject _cameraObject;
        private void Start()
        {
            FolderMenu.setLanguageEvent += SetLanguage;
            foreach (var item in tabDictList)
            {
                item.SetId();
            }
            _cameraObject = UnityEngine.Camera.main.gameObject;
        }
        
        private void SetLanguage()
        {
            var languageScript = FindObjectOfType<LanguageScript>();
            _currentLanguage = languageScript.currentLanguage;
        }

        /// <summary>
        /// Get the length of the cases
        /// </summary>
        /// <param name="caseIndex"></param>
        /// <returns></returns>
        public int GetCaseLength(int caseIndex)
        {
            int count = 0;
            foreach (var tab in tabDictList)
            {
                if (caseIndex == Mathf.FloorToInt(tab.GetId()))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// When moused over the gameObject. Check the distance and wait for an interaction.
        /// </summary>
        private void OnMouseOver()
        {
            float theDistance = Vector3.Distance(_cameraObject.transform.position, transform.position);
            if (theDistance <= maxInteractDistance)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!FindObjectOfType<PlayerData>().isInViewMode)
                    {
                        PlayerData pd =  FindObjectOfType<PlayerData>();
                        pd.SavePlayer();
                        if (TutorialManager.Instance._doTutorial && TutorialManager.Instance.currentState == TutorialManager.TutorialState.Save)
                        {
                            TutorialManager.Instance.EndTutorial();
                        }
                        else
                        {
                            FindObjectOfType<MonologueVisualizer>().VisualizeText(
                                _currentLanguage == LanguageScript.Languages.English
                                    ? _languageOptions[0]
                                    : _languageOptions[1]);
                        }
                    }
                }
            }
        }
    }
}