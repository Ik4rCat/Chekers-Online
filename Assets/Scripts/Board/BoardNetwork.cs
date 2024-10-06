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

    public override void OnStartServer()
    {
        FillBoardList(boardList);
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

}
