using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : Player
{
    [SyncVar(hook = nameof(OnDisplayNameUpdated))]
    string displayName;

    [SyncVar(hook = nameof(OnLobbyOwnerUpdated))]
    bool lobbyOwner;

    [SyncVar] ulong steamID; 

    public static event Action ClientInfoUpdated;
    public static event Action<bool> AuthorityLobbyOwnerStateUpdated;

    public string DisplayName
    {
        get { return displayName; }

        [Server]
        set { displayName = value; }
    }

    public bool LobbyOwner
    {
        get { return lobbyOwner; }

        [Server]
        set { lobbyOwner = value; }
    }

    public ulong SteamID
    {
        get { return steamID; }

        [Server]
        set 
        { 
            steamID = value;
            CSteamID cSteamID = new CSteamID(steamID);
            DisplayName = SteamFriends.GetFriendPersonaName(cSteamID);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    public override void OnStartClient()
    {
        if (!isClientOnly) return;

        ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers.Add(this);
    }

    public override void OnStopClient()
    {
        if (isClientOnly) 
            ((CheckersNetworkManager)NetworkManager.singleton)?.NetworkPlayers.Remove(this);

        ClientInfoUpdated?.Invoke();
    }


    private void OnDisplayNameUpdated(string oldname, string newName)
    {
        ClientInfoUpdated?.Invoke();
    }

    private void OnLobbyOwnerUpdated(bool oldState, bool newState)
    {
        if (!hasAuthority) return;

        AuthorityLobbyOwnerStateUpdated?.Invoke(newState);
    }

    [Command]
    public void CmdNextTurn()
    {
        TurnsHandler.Instance.NextTurn();
    }

    [Command]
    public void CMDSurrender(string message)
    {
        TurnsHandler.Instance.Surrender(message);
    }

}
