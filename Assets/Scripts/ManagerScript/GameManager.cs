using Aws.GameLift.Realtime.Types;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using Aws.GameLift.Realtime.Event;
using System;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour
{
    #region Singletone
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private void SingletoneInit()
    {
        if(instance == null) 
        { 
            instance = this; 
            DontDestroyOnLoad(this.gameObject); 
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    #region Network
    public const int OP_CODE_PLAYER_ACCEPTED = 113;
    public const int GAME_READY_OP = 200;
    public const int GAME_START_OP = 201;
    public const int GAMEOVER_OP = 209;
    public const int PLAYER_ACTION = 300;


    private static readonly IPEndPoint DefaultLoopbackEndpoint = new IPEndPoint(IPAddress.Loopback, port: 0);
    public RealtimeClient _realTimeClient;

    public string _playerId { get; private set; }
    private string _command;
    public string _remotePlayerId { get; private set; }
    //private bool _processGamePlay = false;
    private bool _updateRemotePlayerId = false;
    public bool _findingMatch = false;
    private bool _gameOver = false;
    private MatchResults matchResults = new();

    private ApiGatewayManager apiGatewayManager;

    public async void OnFindMatchPressed()
    {
        Debug.Log("Find match pressed");



        string PollMatchResponse = await apiGatewayManager.PollMatch();

        if (PollMatchResponse != null)
        {
            // The response was for a found game session which also contains info for created player session
            Debug.Log("Game session found!");
            var matchmakingInfo = JsonConvert.DeserializeObject<JObject>(PollMatchResponse);

            var ticket = matchmakingInfo["ticket"];
            if (ticket != null)
            {
                var ttlToken = ticket["ttl"]?["N"];
                var ttl = ttlToken != null ? (string)ttlToken : string.Empty;
                var ticketId = (string)ticket["Id"]?["S"];
                var ipAddress = (string)ticket["GameSessionInfo"]?["M"]?["IpAddress"]?["S"];
                var portToken = ticket["GameSessionInfo"]?["M"]?["Port"]?["N"];
                var port = portToken != null ? int.Parse((string)portToken) : 0;

                var players = ticket["Players"]?["L"];
                if (players != null && players.HasValues)
                {
                    var playerData = players[0]["M"];
                    var playerId = (string)playerData?["PlayerId"]?["S"];
                    var playerSessionId = (string)playerData?["PlayerSessionId"]?["S"];

                    // Debug logs for verification
                    //Debug.Log("Matchmaking Info:");
                    //Debug.Log("TTL: " + ttl);
                    //Debug.Log("IP Address: " + ipAddress);
                    //Debug.Log("Port: " + port);
                    //Debug.Log("Ticket ID: " + ticketId);
                    //Debug.Log("PlayerId: " + playerId);
                    //Debug.Log("PlayerSessionId: " + playerSessionId);

                    _playerId = playerId;

                    // Once connected, the Realtime service moves the Player session from Reserved to Active, which means we're ready to connect.
                    if (_realTimeClient == null)
                    {
                        EstablishConnectionToRealtimeServer(ipAddress, port, playerSessionId);
                        Debug.Log("Connecting...");
                        _findingMatch = true;
                    }
                }
            }
            else
            {
                Debug.Log("Game session response not valid...");
            }

        }
    }

    //Server¿Í Connection ¸ÎÀ½
    private void EstablishConnectionToRealtimeServer(string ipAddress, int port, string playerSessionId)
    {
        int localUdpPort = GetAvailableUdpPort();

        RealtimePayload realtimePayload = new RealtimePayload(_playerId);
        string payload = JsonUtility.ToJson(realtimePayload);

        _realTimeClient = new RealtimeClient(ipAddress, port, localUdpPort, playerSessionId, payload, ConnectionType.RT_OVER_WS_UDP_UNSECURED);
        _realTimeClient.GamePlayedEventHandler += OnGamePlayedEvent;
        _realTimeClient.RemotePlayerIdEventHandler += OnRemotePlayerIdEvent;
        _realTimeClient.GameOverEventHandler += OnGameOverEvent;

        OnREADYCommand();
    }


    void OnGamePlayedEvent(object sender, GamePlayedEventArgs GamePlayedEventArgs)
    {
        Debug.Log($"Played by {GamePlayedEventArgs.PlayerId}, Command: {GamePlayedEventArgs.Command}");
        GamePlayed(GamePlayedEventArgs);
    }

    private void GamePlayed(GamePlayedEventArgs GamePlayedEventArgs)
    {
        Debug.Log($"Unit played: {GamePlayedEventArgs.Command}");
        //OpponentManager.Instance.OnMessageRecieved(GamePlayedEventArgs.Command);
        //if (GamePlayedEventArgs.PlayerId == _playerId)
        //{
        //    Debug.Log("local Unit played");
        //    //_matchStats.localPlayerCardsPlayed.Add(GamePlayedEventArgs.Unit.ToString());

        //}
        //else
        //{
        //    Debug.Log("remote card played");
        //    //_matchStats.remotePlayerCardsPlayed.Add(GamePlayedEventArgs.Unit.ToString());
        //}
        //_processGamePlay = true;
    }


    void OnRemotePlayerIdEvent(object sender, RemotePlayerIdEventArgs remotePlayerIdEventArgs)
    {
        Debug.Log($"Remote player id received: {remotePlayerIdEventArgs.remotePlayerId}.");
        UpdateRemotePlayerId(remotePlayerIdEventArgs);
    }

    private void UpdateRemotePlayerId(RemotePlayerIdEventArgs remotePlayerIdEventArgs)
    {
        _remotePlayerId = remotePlayerIdEventArgs.remotePlayerId;
        _updateRemotePlayerId = true;
    }

    void OnGameOverEvent(object sender, GameOverEventArgs gameOverEventArgs)
    {
        Debug.Log($"Game over event received with winner: {gameOverEventArgs.matchResults.winnerId}.");
        //this._matchResults = gameOverEventArgs.matchResults;
        this._gameOver = true;
    }

    public void OnREADYCommand()
    {
        Debug.Log("READY Command Sent");

        RealtimePayload realtimePayload = new RealtimePayload(_playerId);

        // Use the Realtime client's SendMessage function to pass data to the server
        _realTimeClient.SendMessage(GAME_READY_OP, realtimePayload);
    }

    public void OnPlayerCommand(string command)
    {
        Debug.Log("Command Sent");

        RealtimePayload realtimePayload = new RealtimePayload(_playerId, command);

        // Use the Realtime client's SendMessage function to pass data to the server
        //_realTimeClient.SendMessage(PLAYER_ACTION, realtimePayload);
    }


    public static int GetAvailableUdpPort()
    {
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Bind(DefaultLoopbackEndpoint);
            return ((IPEndPoint)socket.LocalEndPoint).Port;
        }
    }

    void OnApplicationQuit()
    {
        // clean up the connection if the game gets killed
        if (_realTimeClient != null && _realTimeClient.IsConnected())
        {
            _realTimeClient.Disconnect();
        }
    }



    public void OnQuitPressed()
    {
        Debug.Log("OnQuitPressed");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion



    private void Awake()
    {
        SingletoneInit();
    }

    private void Start()
    {
        apiGatewayManager = ApiGatewayManager.Instance;
        apiGatewayManager.onLoginSuccess.AddListener(OnLoginSuccess);
    }

    private void Update()
    {
        if (_realTimeClient != null && _realTimeClient.GameStarted)
        {
            //_playCardButton.gameObject.SetActive(true);
            _realTimeClient.GameStarted = false;

        }
        if (_updateRemotePlayerId)
        {
            _updateRemotePlayerId = false;
            //remoteClientPlayerName.text = _remotePlayerId;
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    OnFindMatchPressed();
        //}
        // determine match results once game is over
        //if (this._gameOver == true)
        //{
        //    this._gameOver = false;
        //    DisplayMatchResults();
        //}
    }

    void OnLoginSuccess() 
    {
        SceneManager.LoadScene("MainScene");
    }
}




[System.Serializable]
public class RealtimePayload
{
    private string PlayerId;
    private string Command;

    // Other fields you wish to pass as payload to the realtime server
    public RealtimePayload() { }
    public RealtimePayload(string playerIdIn)
    {
        this.PlayerId = playerIdIn;
    }
    public RealtimePayload(string PlayerIdin, string CommandIn)
    {
        this.PlayerId = PlayerIdin;
        this.Command = CommandIn;
    }
}

