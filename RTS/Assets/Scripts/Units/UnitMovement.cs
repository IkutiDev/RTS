using Mirror;
using UnityEngine;
using UnityEngine.AI;
namespace RTS.Units
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent navMeshAgent = null;
        [SerializeField] private Targeter targeter = null;
        [SerializeField] private float chaseRange = 10f;

        private Camera mainCamera;
        private NavMeshHit hit;
        #region Server
        [ServerCallback]
        private void Update()
        {
            Targetable target = targeter.GetTarget();
            if (target != null)
            {
                if ((target.transform.position - transform.position).sqrMagnitude > chaseRange*chaseRange)
                {
                    navMeshAgent.SetDestination(target.transform.position);
                }
                else if (navMeshAgent.hasPath)
                {
                    navMeshAgent.ResetPath();
                }

                return;
            }

            if (!navMeshAgent.hasPath) return;

            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return;

            navMeshAgent.ResetPath();
        }

        [Command]
        public void CmdMove(Vector3 position)
        {
            targeter.ClearTarget();

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