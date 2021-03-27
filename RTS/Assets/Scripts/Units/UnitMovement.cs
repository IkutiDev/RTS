using Mirror;
using UnityEngine;
using UnityEngine.AI;
namespace RTS.Units
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent = null;

        private Camera mainCamera;
        private NavMeshHit hit;
        #region Server
        [Command]
        public void CmdMove(Vector3 position)
        {
            if (!ValidatePosition(position)) { return; }
            navMeshAgent.SetDestination(hit.position);
        }

        private bool ValidatePosition(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit navMeshHit, 1f, NavMesh.AllAreas)) return false;
            hit = navMeshHit;
            return true;
        }
        #endregion
    }
}