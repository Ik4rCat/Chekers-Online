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

    public static event Action ClientInfoUpdated;

    public string DisplayName
    {
        get { return displayName; }

        [Server]
        set { displayName = value; }
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

}
