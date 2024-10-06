using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandlerNetworked : TurnsHandler
{

    public override void OnStartServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned += NextTurn;
    }

    public override void OnStopServer()
    {
        PlayerPiecesHandler.OnPiecesSpawned -= NextTurn;
    }

    protected override void FillMovesList()
    {
        base.FillMovesList();

        RpcGenerateMoves(piecesHandler);
    }

    [ClientRpc]
    private void RpcGenerateMoves(PlayerPiecesHandler playerPieces)
    {
        if (NetworkServer.active)   return;

        GenerateMoves(playerPieces.PiecesParent);
    }
}
