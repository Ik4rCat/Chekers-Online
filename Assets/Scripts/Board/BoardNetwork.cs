using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardNetwork : Board
{

    private readonly SyncList<int[]> boardList = new SyncList<int[]>();
    //������ ����� ������ [SyncVar]


    public override IList<int[]> BoardList => boardList;

    public override void OnStartServer()
    {
        FillBoardList(boardList);
    }


}
