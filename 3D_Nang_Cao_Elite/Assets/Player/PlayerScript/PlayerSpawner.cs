using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    // khi vào mạng thì tạo nhân vật cho người chơi
    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            // tạo nhân vật ở vị trí (0, 1, 0)
            var position = new Vector3(0, 1, 0);

            // spawn nhân vật ở vị trí này
            Runner.Spawn(PlayerPrefab, position, Quaternion.identity);
        }
    }
}
