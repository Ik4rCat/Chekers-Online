using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNetwork : Board
{

    private readonly SyncList<int[]> boardList = new SyncList<int[]>();
    //теперь можно просто [SyncVar]


    public override IList<int[]> BoardList => boardList;

    public override event Action<Vector3> OnPieceCaptured;

    public override void OnStartServer()
    {
        FillBoardList(boardList);
        PieceMovementHandlerNetwork.ServerPieceReachedBackline += TryPromotePieceOnBoard;
    }

    public override void OnStopServer()
    {
        PieceMovementHandlerNetwork.ServerPieceReachedBackline -= TryPromotePieceOnBoard;
    }

    [Server]
    public override void MoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        MoveOnBoard(BoardList, oldPosition, newPosition);
        RpcMoveOnBoard(oldPosition, newPosition, nextTurn);
    }

    [ClientRpc]
    private void RpcMoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        if (NetworkServer.active) return;
        MoveOnBoard(BoardList, oldPosition, newPosition);
        if(nextTurn)
        {
            NetworkClient.connection.identity.GetComponent<PlayerNetwork>().CmdNextTurn();
        }
    }

    [Server]
    public override void CaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(BoardList, piecePosition);
        RpcCaptureOnBoard(piecePosition);

        OnPieceCaptured?.Invoke(new Vector3(piecePosition.x,0,piecePosition.y));
    }

    [ClientRpc]
    private void RpcCaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList,piecePosition);
    }

    [Server]
    private bool TryPromotePieceOnBoard(PiecePromotionHandler promotedPiece, int x, int z)
    {
        PromotePieceOnBoard(boardList, x, z);
        RPCPromotePieceOnBoard(x, z);
        return true;
    }

    [ClientRpc]
    private void RPCPromotePieceOnBoard(int x, int z)
    {
        if(NetworkServer.active) return;

        PromotePieceOnBoard(boardList,x,z);
    }

}
