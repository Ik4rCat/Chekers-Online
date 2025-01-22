using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckersNetworkManager : NetworkManager
{
    [SerializeField] GameObject gameOverHandlerPrefab, boardPrefab, 
        turnsHandlerPrefab;

    public List<PlayerNetwork> NetworkPlayers { get; } = new List<PlayerNetwork>();
    public List<Player> Players { get; } = new List<Player>();


    public static event Action ClientConnected;
    public static event Action ServerGameStarted;

    public override void OnStartServer()
    { 
        GameObject board = Instantiate(boardPrefab);
        NetworkServer.Spawn(board);

        GameObject turnsHandler = Instantiate(turnsHandlerPrefab);
        NetworkServer.Spawn(turnsHandler);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientConnected?.Invoke();
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        PlayerNetwork player = Instantiate(playerPrefab).GetComponent<PlayerNetwork>();
        NetworkServer.AddPlayerForConnection(conn,player.gameObject);
        NetworkPlayers.Add(player);
        Players.Add(player);
        player.IsWhite = numPlayers == 1;
        if (MainMenu.UseSteam)
        {
            CSteamID steamID = SteamMatchmaking.GetLobbyMemberByIndex(MainMenu.LobbyID, numPlayers - 1);
            player.SteamID = steamID.m_SteamID;
        }
        else
        {
            player.DisplayName = player.IsWhite ? "Light" : "Dark";
        }

        player.LobbyOwner = player.IsWhite;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        PlayerNetwork player = conn.identity.GetComponent<PlayerNetwork>();
        NetworkPlayers.Remove(player);
        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }


    public override void OnStopServer()
    {
        NetworkPlayers.Clear();
        Players.Clear();
        if (MainMenu.UseSteam)
            SteamMatchmaking.LeaveLobby(MainMenu.LobbyID);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        if(NetworkPlayers.Count == 0)
        {
            SceneManager.LoadScene("Lobby Scene");
            Destroy(gameObject);
        }
        if (MainMenu.UseSteam)
            SteamMatchmaking.LeaveLobby(MainMenu.LobbyID); 
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName == "Game Scene")
        {
            ServerGameStarted?.Invoke();
            GameObject handler =Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(handler);
        }
    }

}
