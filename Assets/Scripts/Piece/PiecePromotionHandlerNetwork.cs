using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePromotionHandlerNetwork : PiecePromotionHandler
{

    public override void OnStartServer()
    {
        PieceMovementHandlerNetwork.ServerPieceReachedBackline += TryPromotePiece;
    }

    public override void OnStopServer()
    {
        PieceMovementHandlerNetwork.ServerPieceReachedBackline -= TryPromotePiece;
    }

    protected override bool TryPromotePiece(PiecePromotionHandler promotedPiece, int x, int z)
    {
        if (base.TryPromotePiece(promotedPiece, x, z) == false) return false;

        RPCPromotePiece();
        return true;
    }

    [ClientRpc]
    private void RPCPromotePiece()
    {
        if (NetworkServer.active) return;
        PromotePiece();
    }

}
