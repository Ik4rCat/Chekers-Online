using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSpawner : MonoBehaviour
{
    [SerializeField] GameObject localGameManagerPrefab, networkManagerPrefab, steamManagerPrefab;

    public void SpawnLocalGameManager()
    {
        Instantiate(localGameManagerPrefab);
    }

    public void SpawnNetworkManager()
    {
        Instantiate(networkManagerPrefab);
        //if(MainMenu.UseSteam)
        //    Instantiate(steamManagerPrefab);
        
    }
    
}
