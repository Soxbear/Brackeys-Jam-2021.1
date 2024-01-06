using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSelectPanel : MonoBehaviour
{
    string MissionSceneName;
    Transform Elements;
    public int AllowableDifficulty = 0;
    int SelectedLevels;
    float LevelChance;

    public int AllowableLevelsOfEachType = 3;

    public GameObject TutorialLevel;
    public GameObject[] PotentialEasyLevels;
    public GameObject[] PotentialMediumLevels;
    public GameObject[] PotentialHardLevels;

    public Color[] DifficulyColors;



    void Start()
    {
        Elements = transform.Find("Elements").transform;
    }

    void Awake()
    {
        //AllowableDifficulty = 1; //PlayerPrefs.GetInt("AllowableDifficulty", 0);
        if (AllowableDifficulty >= 1)
        {
            SelectedLevels = AllowableLevelsOfEachType;
            LevelChance = (float) AllowableLevelsOfEachType / (float) PotentialEasyLevels.Length;

            while (SelectedLevels > 0)
            {
                foreach (GameObject Level in PotentialEasyLevels)
                {
                    if (Random.Range(0f, 1f) < LevelChance && SelectedLevels > 0)
                    {
                        // Level.SetActive(true);
                        SelectedLevels -= 1;
                    }
                }
            }
            
            
        }
        if (AllowableDifficulty >= 2)
        {
            SelectedLevels = AllowableLevelsOfEachType;
            LevelChance = (float)AllowableLevelsOfEachType / (float)PotentialMediumLevels.Length;

            while (SelectedLevels > 0)
            {
                foreach (GameObject Level in PotentialMediumLevels)
                {
                    if (Random.Range(0f, 1f) < LevelChance && SelectedLevels > 0)
                    {
                        // Level.SetActive(true);
                        SelectedLevels -= 1;
                    }
                }
            }


        }
        if (AllowableDifficulty >= 3)
        {
            SelectedLevels = AllowableLevelsOfEachType;
            LevelChance = (float) AllowableLevelsOfEachType / (float) PotentialHardLevels.Length;

            while (SelectedLevels > 0)
            {
                foreach (GameObject Level in PotentialHardLevels)
                {
                    if (Random.Range(0f, 1f) < LevelChance && SelectedLevels > 0)
                    {
                        // Level.SetActive(true);
                        SelectedLevels -= 1;
                    }
                }
            }


        }
        if (AllowableDifficulty == 0)
        {
            TutorialLevel.SetActive(true);
        }
    }

    public void ChangeMission(string PlaceName, string PlaceDescription, string MissionName, string MissionDescription, string PossibleEnemies, int difficulty, string SceneName)
    {
        Elements.gameObject.SetActive(true);
        Elements.Find("PlaceName").GetComponent<TextMeshProUGUI>().text = PlaceName;
        Elements.Find("PlaceDescription").GetComponent<TextMeshProUGUI>().text = PlaceDescription;
        Elements.Find("MissionName").GetComponent<TextMeshProUGUI>().text = MissionName;
        Elements.Find("MissionDescription").GetComponent<TextMeshProUGUI>().text = MissionDescription;
        Elements.Find("PossibleEnemiesList").GetComponent<TextMeshProUGUI>().text = PossibleEnemies;
        Elements.Find("DifficultyLight").GetComponent<SpriteRenderer>().color = DifficulyColors[difficulty-1];
        MissionSceneName = SceneName;

    }

    public void StartMission()
    {
        SceneManager.LoadScene(MissionSceneName);
    }
}
