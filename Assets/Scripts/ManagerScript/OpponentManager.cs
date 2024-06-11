using Amazon.GameLift.Model;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class OpponentManager : CommonManager
{
    #region UnitList
    
    #endregion

    public bool isMyTurn;
    #region Singletone
    private static OpponentManager instance = null;
    public static OpponentManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("OpponentManager").AddComponent<OpponentManager>();
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




    #region Unity Monobehaviour LifeCycle

    private void Awake()
    {
        SingletoneInit();
    }
    private void Start()
    {
        MatchManager.Instance.onTurnEnd.AddListener(OnTurnStart);

        foreach (Unit unit in UnitList)
        {
            SetupFacingDirection(unit);
        }
    }

    private void UpdateFacingDirection(Unit unit, bool isFacingRight)
    {
        Vector3 newScale = unit.transform.localScale;
        if (isFacingRight)
        {
            newScale.x = Mathf.Abs(newScale.x);
        }
        else
        {
            newScale.x = -Mathf.Abs(newScale.x);
        }
        unit.transform.localScale = newScale;
    }

    private void OnDestroy()
    {
        for (int i = UnitList.Count - 1; i >= 0; i--)
        {
            Destroy(UnitList[i]);
        }
    }

    #endregion

    public override void OnTurnStart()
    {
        if(isMyTurn)
        {
            (int weight, string command) action = (-1, "");
            foreach (Unit unit in UnitList)
            {
                unit.OnTurnStart();
                if (action.weight <= unit.mostValuedAction.weight)
                {
                    action = unit.mostValuedAction;
                }
            }
            if (action.command != "")
            {
                StartCoroutine(DepackCommand(action.command));
            }
        }
    }

    #region Command Handler
    IEnumerator DepackCommand(string commandString)
    {
        //Each command is: "Command/ objectCoordinate/ subjectCoordinate@Command..."
        Coroutine actionCoroutine = null;

        string[] commands = commandString.Trim('@').Split('@');
        foreach (string command in commands)
        {
            string[] parts = command.Split('/');

            if (parts.Length != 3)
            {
                Debug.Log("Wrong Command: " + command);
                continue;
            }

            string commandType = parts[0];
            Vector3 objectLocation = DepackLocation(parts[1]);
            Vector3 subjectLocation = DepackLocation(parts[2]);

            yield return new WaitUntil(() => actionCoroutine == null);

            actionCoroutine = StartCoroutine(ExecuteCommand(commandType, objectLocation, subjectLocation));
        }

        // Inner Methods and Coroutines
        Vector3 DepackLocation(string coordinate)
        {
            string[] coords = coordinate.Trim('(', ')').Split(',');
            if (coords.Length != 2)
            {
                Debug.LogError("Wrong Coordinate: " + coordinate);
                return Vector3.zero;
            }

            float x = float.Parse(coords[0]);
            float y = float.Parse(coords[1]);
            return new Vector3(x, y);
        }

        IEnumerator ExecuteCommand(string command, Vector3 objLocation, Vector3 subjectLocation)
        {
            Unit obj = MapManager.Instance.stage.NodeArray[(int)objLocation.x, (int)objLocation.y].unitOn;
            Unit target = MapManager.Instance.stage.NodeArray[(int)subjectLocation.x, (int)subjectLocation.y].unitOn;

            if (obj == null)
            {
                Debug.Log("Wrong Action Object");
                actionCoroutine = null;
                MatchManager.Instance.ChangeTurn();
                yield break;
            }
            if (obj == target)
            {
                switch (command)
                {
                    case "Ability1":
                        obj.Ability1();
                        break;
                    case "Ability2":
                        obj.Ability2();
                        break;
                    case "Attack":
                        obj.Attack(target);
                        break;
                    case "Idle":
                        MatchManager.Instance.ChangeTurn();
                        break;
                    default:
                        Debug.Log("Wrong Command: " + command);
                        MatchManager.Instance.ChangeTurn();
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case "Move":
                        obj.MoveTo(subjectLocation);
                        break;
                    case "Attack":
                        obj.Attack(target);
                        break;
                    case "Ability1":
                        obj.Ability1(target);
                        break;
                    case "Ability2":
                        obj.Ability2(target);
                        break;
                    default:
                        Debug.Log("Wrong Command: " + command);
                        MatchManager.Instance.ChangeTurn();
                        break;
                }
            }

            yield return new WaitUntil(() => obj.IsinAction() == false);
            actionCoroutine = null;
        }

    }
    #endregion

    public void CreateSampleSet()
    {
        // Captain on (24, 5)
        string path = $"Prefabs/Character/Unit_TEST/";
        Deploy("Priest", new Vector3(24, 3));
        Deploy("Knight", new Vector3(12, 5));
        Deploy("Archer", new Vector3(24, 7));



        void Deploy(string unitType, Vector3 Location)
        {
            GameObject prefab = Resources.Load<GameObject>(path + unitType);
            if (prefab == null) { Debug.LogError("Failed to load prefab from path: " + path); return; }

            GameObject unit= Instantiate(prefab, Location, Quaternion.identity, this.transform);
            MapManager.Instance.stage.NodeArray[(int)Location.x, (int)Location.y].isBlocked = true;
            MapManager.Instance.stage.NodeArray[(int)Location.x, (int)Location.y].unitOn = unit.GetComponent<Unit>();
            unit.GetComponent<BoxCollider2D>().enabled = true;
            RegisterUnit(unit.GetComponent<Unit>());
        }
    }
}
