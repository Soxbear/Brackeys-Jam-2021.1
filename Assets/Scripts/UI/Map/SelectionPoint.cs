using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPoint : MonoBehaviour
{

    [Header("Area Information")]
    public string PlaceName;
    public string PlaceDescription;
    public string MissionName;
    public string MissionDescription;
    public string PossibleEnemies;
    public int Difficulty;
    public string SceneName;

    public MissionSelectPanel MissionInfo;

    public void ChangeSelectedMission()
    {
        MissionInfo.ChangeMission(PlaceName, PlaceDescription, MissionName, MissionDescription, PossibleEnemies, Difficulty, SceneName);
    }

}
