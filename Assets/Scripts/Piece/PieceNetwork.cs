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
    }

    private void SynchookSelfSetParent(PlayerPiecesHandler oldOwner, PlayerPiecesHandler newOwner )
    {
        transform.parent = newOwner.PiecesParent;
    }

}
