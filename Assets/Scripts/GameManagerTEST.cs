using Aws.GameLift.Realtime.Types;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;

public class GameManagerTEST : MonoBehaviour
{
    public const int OP_CODE_PLAYER_ACCEPTED = 113;
    public const int GAME_READY_OP = 200;
    public const int GAME_START_OP = 201;
    public const int GAMEOVER_OP = 209;
    public const int PLAYER_ACTION = 300;


    private static readonly IPEndPoint DefaultLoopbackEndpoint = new IPEndPoint(IPAddress.Loopback, port: 0);
    private RealtimeClient _realTimeClient;
    private MatchStats _matchStats = new MatchStats();
    private MatchResults _matchResults = new MatchResults();

    private string _playerId;
    private string _remotePlayerId = "";
    private string _ticketId;
    private bool _processGamePlay = false;
    private bool _updateRemotePlayerId = false;
    private bool _findingMatch = false;
    private bool _gameOver = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("GO");
            OnFindMatchPressed();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Send Message!: ");
            OnPlayCardPressed();
        }
    }


    private List<GamePlayed> cardsPlayed = new List<GamePlayed>();

    public async void OnFindMatchPressed()
    {
        Debug.Log("Find match pressed");
        _findingMatch = true;

        ApiGatewayManager apiGatewayManager = new ApiGatewayManager();

        string PollMatchResponse = await apiGatewayManager.PollMatch(_ticketId);

        if (PollMatchResponse != null)
        {
            // The response was for a found game session which also contains info for created player session
            Debug.Log("Game session found!");
            // Debug.Log(gameSessionPlacementInfo.GameSessionId);

            var matchmakingInfo = JsonConvert.DeserializeObject<dynamic>(PollMatchResponse);

            var ticket = matchmakingInfo["ticket"];

            var ttl = (string)ticket["ttl"]["N"];
            var ticketId = (string)ticket["Id"]["S"];
            var ipAddress = (string)ticket["GameSessionInfo"]["M"]["IpAddress"]["S"];
            var port = (int)ticket["GameSessionInfo"]["M"]["Port"]["N"];

            var players = ticket["Players"]["L"];
            var playerData = players[0]["M"];
            var playerId = (string)playerData["PlayerId"]["S"];
            var playerSessionId = (string)playerData["PlayerSessionId"]["S"];

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
            EstablishConnectionToRealtimeServer(ipAddress, port, playerSessionId);
            Debug.Log("Connecting...");
        }
            else
            {
                Debug.Log("Game session response not valid...");
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
    }


    void OnGamePlayedEvent(object sender, GamePlayedEventArgs GamePlayedEventArgs)
    {
        Debug.Log($"The Unit: {GamePlayedEventArgs.Unit} was played by {GamePlayedEventArgs.PlayerId}, to the position from ({GamePlayedEventArgs.ObjLocX}, {GamePlayedEventArgs.ObjLocY}) to ({GamePlayedEventArgs.TargetLocX}, {GamePlayedEventArgs.TargetLocY}).");
        GamePlayed(GamePlayedEventArgs);
    }

    private void GamePlayed(GamePlayedEventArgs GamePlayedEventArgs)
    {
        Debug.Log($"Unit played: {GamePlayedEventArgs.Unit}");

        if (GamePlayedEventArgs.PlayerId == _playerId)
        {
            Debug.Log("local Unit played");
            _matchStats.localPlayerCardsPlayed.Add(GamePlayedEventArgs.Unit.ToString());

        }
        else
        {
            Debug.Log("remote card played");
            _matchStats.remotePlayerCardsPlayed.Add(GamePlayedEventArgs.Unit.ToString());
        }

        _processGamePlay = true;
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
        this._matchResults = gameOverEventArgs.matchResults;
        this._gameOver = true;
    }


    private void DisplayMatchResults()
    {
        string localPlayerResults = "";
        string remotePlayerResults = "";

        if (_matchResults.winnerId == _playerId)
        {
            localPlayerResults = "You WON! Score ";
            remotePlayerResults = "Loser. Score ";
        }
        else
        {
            remotePlayerResults = "WINNER! Score ";
            localPlayerResults = "You Lost. Score ";
        }

        if (_matchResults.playerOneId == _playerId)
        {
            // our local player matches player one data
            localPlayerResults += _matchResults.playerOneScore;
            remotePlayerResults += _matchResults.playerTwoScore;
        }
        else
        {
            // our local player matches player two data
            localPlayerResults += _matchResults.playerTwoScore;
            remotePlayerResults += _matchResults.playerOneScore;
        }

        //Player1Result.text = localPlayerResults;
        //Player2Result.text = remotePlayerResults;
    }


    public void OnPlayCardPressed()
    {
        Debug.Log("Play card pressed");

        RealtimePayload realtimePayload = new RealtimePayload(_playerId);

        // Use the Realtime client's SendMessage function to pass data to the server
        _realTimeClient.SendMessage(PLAYER_ACTION, realtimePayload);
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting...");

    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (_realTimeClient != null && _realTimeClient.GameStarted)
    //    {
    //        //_playCardButton.gameObject.SetActive(true);
    //        _realTimeClient.GameStarted = false;
    //    }
    //    if (_updateRemotePlayerId)
    //    {
    //        _updateRemotePlayerId = false;
    //        //remoteClientPlayerName.text = _remotePlayerId;
    //    }
    //    if (_processGamePlay)
    //    {
    //        _processGamePlay = false;

    //        ProcessGamePlay();
    //    }

    //    // determine match results once game is over
    //    if (this._gameOver == true)
    //    {
    //        this._gameOver = false;
    //        DisplayMatchResults();
    //    }

    //}

    private void ProcessGamePlay()
    {
        for (int cardIndex = 0; cardIndex < _matchStats.localPlayerCardsPlayed.Count; cardIndex++)
        {
            //cardUIObjects[cardIndex].text = _matchStats.localPlayerCardsPlayed[cardIndex];
        }

        for (int cardIndex = 0; cardIndex < _matchStats.remotePlayerCardsPlayed.Count; cardIndex++)
        {
            // Added + 2 because cardUIObjects holds all UI cards, first 2 are local, last 2 are remote 
            //cardUIObjects[cardIndex + 2].text = _matchStats.remotePlayerCardsPlayed[cardIndex];
        }
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
}



public class MatchStats
{
    public List<string> localPlayerCardsPlayed = new List<string>();
    public List<string> remotePlayerCardsPlayed = new List<string>();
}

[System.Serializable]
public class RealtimePayload
{
    public string playerId;
    // Other fields you wish to pass as payload to the realtime server
    public RealtimePayload() { }
    public RealtimePayload(string playerIdIn)
    {
        this.playerId = playerIdIn;
    }
}

[System.Serializable]
public class MatchResults
{
    public string playerOneId;
    public string playerTwoId;

    public string playerOneScore;
    public string playerTwoScore;

    public string winnerId;

    public MatchResults() { }
    public MatchResults(string playerOneIdIn, string playerTwoIdIn, string playerOneScoreIn, string playerTwoScoreIn, string winnerIdIn)
    {
        this.playerOneId = playerOneIdIn;
        this.playerTwoId = playerTwoIdIn;
        this.playerOneScore = playerOneScoreIn;
        this.playerTwoScore = playerTwoScoreIn;
        this.winnerId = winnerIdIn;
    }
}