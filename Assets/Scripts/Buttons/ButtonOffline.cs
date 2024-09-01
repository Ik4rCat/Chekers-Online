using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Steamworks;

public class ButtonOffline : MonoBehaviour
{
    private const string LobbySceneName = "Lobby Scene";
    public void GoOffline()
    {
        if (LocalGameManager.Instance)
        {
            SceneManager.LoadScene(LobbySceneName);
            Destroy(LocalGameManager.Instance.gameObject);
            Destroy(TurnsHandler.Instance.gameObject);
            Destroy(Board.Instance.gameObject);
        }
        else
        {
            NetworkManager.singleton.ServerChangeScene(LobbySceneName);

            if (NetworkServer.active  && NetworkClient.isConnected )
                NetworkManager.singleton.StopHost();
            else
                NetworkManager.singleton.StopClient();

            Destroy(NetworkManager.singleton.gameObject);
        }
    }
}
