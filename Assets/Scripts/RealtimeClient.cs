using UnityEngine;
using Aws.GameLift.Realtime;
using System;
using Aws.GameLift.Realtime.Event;
using Aws.GameLift.Realtime.Types;
using Newtonsoft.Json;
using System.Text;



public class RealtimeClient
{


    public Aws.GameLift.Realtime.Client Client { get; private set; }

    public bool OnCloseReceived { get; private set; }
    public bool GameStarted = false;

    public event EventHandler<RemotePlayerIdEventArgs> RemotePlayerIdEventHandler;
    public event EventHandler<GameOverEventArgs> GameOverEventHandler;
    public event EventHandler<GamePlayedEventArgs> GamePlayedEventHandler;

    public RealtimeClient(string endpoint, int tcpPort, int localUdpPort, string playerSessionId, string connectionPayload, ConnectionType connectionType)
    {
        this.OnCloseReceived = false;

        // Create a client configuration to specify a secure or unsecure connection type
        // Best practice is to set up a secure connection using the connection type RT_OVER_WSS_DTLS_TLS12.
        ClientConfiguration clientConfiguration = new ClientConfiguration()
        {
            // C# notation to set the field ConnectionType in the new instance of ClientConfiguration
            ConnectionType = connectionType
        };

        // Create a Realtime client with the client configuration            
        Client = new Client(clientConfiguration);

        Client = new Aws.GameLift.Realtime.Client(clientConfiguration);
        Client.ConnectionOpen += new EventHandler(OnOpenEvent);
        Client.ConnectionClose += new EventHandler(OnCloseEvent);
        Client.GroupMembershipUpdated += new EventHandler<GroupMembershipEventArgs>(OnGroupMembershipUpdate);
        Client.DataReceived += new EventHandler<DataReceivedEventArgs>(OnDataReceived);

        ConnectionToken token = new ConnectionToken(playerSessionId, Encoding.UTF8.GetBytes((connectionPayload)));
        Client.Connect(endpoint, tcpPort, localUdpPort, token);
    }

    public virtual void OnDataReceived(object sender, DataReceivedEventArgs e)
    {
        Debug.Log("On data received");

        // handle message based on OpCode
        switch (e.OpCode)
        {
            case GameManager.OP_CODE_PLAYER_ACCEPTED:
                // This tells our client that the player has been accepted into the Game Session as a new player session.
                Debug.Log("Player accepted into game session!");

                // If you need to test and you don't have two computers, you can mark GameStarted true here to enable the Draw card button
                // and comment it out in the GAME_START_OP case.
                // This only works because game play is asynchronous and doesn't care if both players are active at the same time.
                //GameStarted = true;

                break;

            case GameManager.GAME_START_OP:
                // The game start op tells our game clients that all players have joined and the game should start
                Debug.Log("Start game op received...");

                string startGameData = Encoding.Default.GetString(e.Data);
                // Debug.Log(startGameData);

                // Sets the opponent's id, in production should use their public username, not id.
                StartMatch startMatch = JsonConvert.DeserializeObject<StartMatch>(startGameData);
                OnRemotePlayerIdReceived(startMatch);

                // This enables the draw card button so the game can be played.
                GameStarted = true;

                break;

            case GameManager.PLAYER_ACTION:
                // A player has drawn a card.  To be received as an acknowledgement that a card was played,
                // regardless of who played it, and update the UI accordingly.
                Debug.Log("Player Action Detected");

                string data = BytesToString(e.Data);
                // Debug.Log(data);

                GamePlayed GamePlayedMessage = JsonConvert.DeserializeObject<GamePlayed>(data);
                // Debug.Log(GamePlayedMessage.playedBy);
                // Debug.Log(GamePlayedMessage.card);

                OnGamePlayed(GamePlayedMessage);
                    
                break;

            case GameManager.GAMEOVER_OP:
                // gives us the match results
                Debug.Log("Game over op...");

                string gameoverData = BytesToString(e.Data);
                // Debug.Log(gameoverData);

                MatchResults matchResults = JsonConvert.DeserializeObject<MatchResults>(gameoverData);

                OnGameOver(matchResults);

                break;

            default:
                Debug.Log("OpCode not found: " + e.OpCode);
                break;
        }
    }

    public static byte[] StringToBytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    private string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    protected virtual void OnGamePlayed(GamePlayed GamePlayed)
    {
        Debug.Log("OnGamePlayed");

        GamePlayedEventArgs GamePlayedEventArgs = new GamePlayedEventArgs(GamePlayed);

        EventHandler<GamePlayedEventArgs> handler = GamePlayedEventHandler;
        if (handler != null)
        {
            handler(this, GamePlayedEventArgs);
        }
    }


    protected virtual void OnRemotePlayerIdReceived(StartMatch startMatch)
    {
        Debug.Log("OnRemotePlayerIdReceived");

        RemotePlayerIdEventArgs remotePlayerIdEventArgs = new RemotePlayerIdEventArgs(startMatch);

        EventHandler<RemotePlayerIdEventArgs> handler = RemotePlayerIdEventHandler;
        if (handler != null)
        {
            handler(this, remotePlayerIdEventArgs);
        }
    }


    protected virtual void OnGameOver(MatchResults matchResults)
    {
        Debug.Log("OnGameOver");

        
        GameOverEventArgs gameOverEventArgs = new GameOverEventArgs(matchResults);

        EventHandler<GameOverEventArgs> handler = GameOverEventHandler;
        if (handler != null)
        {
            handler(this, gameOverEventArgs);
        }
    }

    public void SendMessage(int opcode, RealtimePayload realtimePayload)
    {

        string payload = JsonUtility.ToJson(realtimePayload);
        // Debug.Log(payload);

        Client.SendMessage(Client.NewMessage(opcode)
            .WithDeliveryIntent(DeliveryIntent.Reliable)
            .WithTargetGroup(-1)
            .WithPayload(StringToBytes(payload)));
    }



    public void OnOpenEvent(object sender, EventArgs e)
    {
    }

    public void OnCloseEvent(object sender, EventArgs e)
    {
        OnCloseReceived = true;
    }

    public void OnGroupMembershipUpdate(object sender, GroupMembershipEventArgs e)
    {
    }

    public void Disconnect()
    {
        if (Client.Connected)
        {
            Client.Disconnect();
        }
    }

    public bool IsConnected()
    {
        return Client.Connected;
    }

    //[System.Serializable]
    //public class MatchResults
    //{
    //    public string playerOneId;
    //    public string playerTwoId;

    //    public string winnerId;

    //    public MatchResults() { }
    //    public MatchResults(string playerOneIdIn, string playerTwoIdIn, string winnerIdIn)
    //    {
    //        this.playerOneId = playerOneIdIn;
    //        this.playerTwoId = playerTwoIdIn;
    //        this.winnerId = winnerIdIn;
    //    }
    //}
}

public class GamePlayedEventArgs : EventArgs
{
    public string PlayerId { get; set; }
    public string Command { get; set; }



    public GamePlayedEventArgs(GamePlayed GamePlayed)
    {
        this.PlayerId = GamePlayed.PlayerId;
        this.Command = GamePlayed.Command;

    }
}


public class RemotePlayerIdEventArgs : EventArgs
{
    public string remotePlayerId { get; set; }

    public RemotePlayerIdEventArgs(StartMatch startMatch)
    {
        this.remotePlayerId = startMatch.remotePlayerId;
    }
}

[System.Serializable]
public class GamePlayed
{
    public string PlayerId;
    public string Command;

    public GamePlayed() { }
    public GamePlayed(string PlayerIdin, string CommandIn)
    {
        this.PlayerId = PlayerIdin;
        this.Command = CommandIn;
    }
}

public class GameOverEventArgs : EventArgs
{
    public MatchResults matchResults { get; set; }

    public GameOverEventArgs(MatchResults matchResults)
    {
        this.matchResults = matchResults;
    }
}


[System.Serializable]
public class StartMatch
{
    public string remotePlayerId;
    public StartMatch() { }
    public StartMatch(string remotePlayerIdIn)
    {
        this.remotePlayerId = remotePlayerIdIn;
    }
}