using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Singletone
    private static MapManager instance = null;
    public static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("MapManager").AddComponent<MapManager>();
            }
            return instance;
        }
    }

    private void SingletoneInit()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion


    #region Parameter
    private string stageSize;
    private string stageName;

    public Stage stage;
    #endregion


    private void Awake()
    {
        SingletoneInit();
        stageSize = "3x3"; //Predefined for test
        SelectStage();
    }
    private void Start()
    {
        LoadStage();
    }


    void SelectStage()
    {
        if (stageList.TryGetValue(stageSize, out List<string> stages))
        {
            //stageName = stages[Random.Range(0, stages.Count)]; 
            stageName = "Standard"; // Fix Stage for TEST
        }// Random select
        else { Debug.LogError($"No stages found for size: {stageSize}"); }
    }

    public void InitWeather(PlayerManager player, OpponentManager opponent)
    {
        Weather weather = new SampleWeather(); //weatherList[Random.Range(0, weatherList.Count)];
        
        weather.ApplyWeatherEffect(player.UnitList);
        weather.ApplyWeatherEffect(opponent.UnitList);
    }

    void LoadStage()
    {
        string path = $"Prefabs/Map/{stageSize}/{stageName}";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null) { stage = Instantiate(prefab, new Vector3(-0.5f, -0.5f), Quaternion.identity).GetComponent<Stage>(); }
        else { Debug.LogError("Failed to load prefab from path: " + path);}
    }


    private Dictionary<string, List<string>> stageList = new Dictionary<string, List<string>>()
    {
        {"3x3", new List<string> {"Standard", "Dogbone", "GoAround"}},
        // Add other sizes and stage names as needed
    };

    private List<Weather> weatherList = new() { new SampleWeather(), new Rainy(), new Foggy(), new Sunny() };  
}
