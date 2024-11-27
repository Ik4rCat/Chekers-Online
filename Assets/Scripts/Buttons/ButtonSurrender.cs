using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonSurrender : MonoBehaviour
{
    public void Surrender()
    {
        if (LocalGameManager.Instance)
        {
            TurnsHandler.Instance.Surrender();
        }
        else
        {
            List<PlayerNetwork> players = ((CheckersNetworkManager)NetworkManager.singleton).NetworkPlayers;

            for (int i = 0; i < players.Count; i++)
            {
               if( players[i].hasAuthority == true)
                {
                    players[i].CMDSurrender($"Победитель: {players[i == 0 ? 1 : 0].DisplayName}");
                }

            }
        }
    }
}
