using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementHandlerNetwork : PieceMovementHandler
{

    public static event Func<PiecePromotionHandler, int, int, bool> ServerPieceReachedBackline;

    public override void OnStartAuthority()
    {
        TilesSelectionHandler.OnTileSelected += HandleTileSelected;
        TurnsHandler.Instance.OnGameOver += RPCGameOver;
    }

    public override void OnStopAuthority()
    {
        TilesSelectionHandler.OnTileSelected -= HandleTileSelected;
        TurnsHandler.Instance.OnGameOver -= RPCGameOver;
    }

    protected override void Move(Vector3 position, bool nextTurn)
    {
        CMDMove(position,nextTurn);
    }

    [Command]
    private void CMDMove(Vector3 position, bool nextTurn)
    {
        base.Move(position,nextTurn);
    }


    protected override void Capture(Vector2Int piecePosition)
    {
        CMDCapture(piecePosition);
    }

    [Command]
    private void CMDCapture(Vector2Int piecePosition)
    {
        base.Capture(piecePosition);
    }

    protected override void PlayAudio()
    {
        RPCPlayAudio();
    }

    [ClientRpc]
    private void RPCPlayAudio()
    {
        base.PlayAudio();
    }

    protected override void ReachedBackline(Vector2Int newPosition)
    {
        ServerPieceReachedBackline.Invoke(promotionHandler, newPosition.x, newPosition.y);

    }

    [ClientRpc]
    private void RPCGameOver(string result)
    {
        LockSelectPiece();
    }

}
