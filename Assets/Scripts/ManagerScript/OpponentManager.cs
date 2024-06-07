using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    #region EnemyList

    public List<Unit> EnemyList = new();

    public void RegisterUnit(Unit unit) => EnemyList.Add(unit);

    public void RemoveUnit(Unit unit) => EnemyList.Remove(unit);
    
    #endregion

    public bool isMyTurn;

    #region Unity Monobehaviour LifeCycle
    private void Start()
    {
        MatchManager.Instance.onTurnEnd.AddListener(OnTurnStart);
    }

    private void OnDestroy()
    {
        for (int i = EnemyList.Count - 1; i >= 0; i--)
        {
            Destroy(EnemyList[i]);
        }
    }

    #endregion

    public void OnTurnStart()
    {
        if(EnemyList != null && isMyTurn)
        {
            (int weight, string command) action = (0, "");
            foreach (Unit unit in EnemyList)
            {
                //unit.OnTurnStart();
                //if (action.weight < unit.mostValuedAction.weight)
                //{
                //    action = unit.mostValuedAction;
                //}
                Debug.Log("林!籍!贸!府!");
            }
            StartCoroutine(DepackCommand(action.command));
        }
    }

    #region Command Handler
    IEnumerator DepackCommand(string commandString)
    {
        //Each command is: "Command/ objectCoordinate/ subjectCoordinate@Command..."
        Coroutine actionCoroutine = null;

        string[] commands = commandString.Split('@');
        foreach (string command in commands)
        {
            string[] parts = command.Split('/');

            if (parts.Length < 3)
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
        MatchManager.Instance.ChangeTurn();

        // Inner Methods and Coroutines
        Vector3 DepackLocation(string coordinate)
        {
            string[] coords = coordinate.Trim('(', ')').Split(' ');
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
                actionCoroutine = null;
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
                    case "Idle":
                        break;
                    default:
                        Debug.Log("Wrong Command: " + command);
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
                        //obj.Attack(target);
                        Debug.Log("林!籍!贸!府!");
                        break;
                    case "Ability1":
                        //obj.Ability1(target);
                        Debug.Log("林!籍!贸!府!");
                        break;
                    case "Ability2":
                        //obj.Ability2(target);
                        Debug.Log("林!籍!贸!府!");
                        break;
                    default:
                        Debug.Log("Wrong Command: " + command);
                        break;
                }
            }

            actionCoroutine = null;
        }

    }
    #endregion
}
