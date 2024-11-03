using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceNetwork : NetworkBehaviour
{
    [SyncVar(hook = nameof(SynchookSelfSetParent))]
    PlayerPiecesHandler owner;

    public override void OnStartServer()
    {
        owner = connectionToClient.identity.GetComponent<PlayerPiecesHandler>();
        Board.Instance.OnPieceCaptured += DestroySelfOnCaptured;
    }

    public override void OnStopServer()
    {
        Board.Instance.OnPieceCaptured -= DestroySelfOnCaptured;
    }


    private void SynchookSelfSetParent(PlayerPiecesHandler oldOwner, PlayerPiecesHandler newOwner )
    {
        transform.parent = newOwner.PiecesParent;
    }

    private void DestroySelfOnCaptured(Vector3 capturedPiecePos)
    {
        if (capturedPiecePos != transform.position) return;

        NetworkServer.Destroy(gameObject);
    }

}
