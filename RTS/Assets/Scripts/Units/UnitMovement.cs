using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent = null;

    private Camera mainCamera;
    private NavMeshHit hit; 
    #region Server
    [Command]
    private void CmdMove(Vector3 position)
    {
        if(!ValidatePosition(position)) { return; }
        navMeshAgent.SetDestination(hit.position);
    }

    private bool ValidatePosition(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas)) return false;
        hit = navMeshHit;
        return true;
    }
    #endregion
    #region Client
    public override void OnStartClient()
    {
        base.OnStartClient();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        mainCamera = Camera.main;
    }
    [ClientCallback]
    private void Update()
    {
        //Checking if right click is valid for moving
        if (!hasAuthority) return;
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;
        Ray ray =mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity)) return;

        CmdMove(raycastHit.point);
    }
    #endregion
}
