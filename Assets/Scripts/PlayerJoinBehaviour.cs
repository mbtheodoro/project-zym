using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerJoinBehaviour : MonoBehaviour
{
    #region REFERENCES
    #endregion

    #region PARAMETERS
    public List<Vector3> playerSpawnPositions;

    public List<Vector2> playerSpawnRotations;
    #endregion

    #region EVENTS
    public void OnPlayerJoin(PlayerInput playerInput)
    {
        playerInput.transform.position = playerSpawnPositions[playerInput.playerIndex];
        playerInput.GetComponent<PlayerController>().lookDir = playerSpawnRotations[playerInput.playerIndex];
    }
    #endregion

    #region UNITY
    #endregion
}
