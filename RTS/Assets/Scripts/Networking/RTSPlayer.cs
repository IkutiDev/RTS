using Mirror;
using RTS.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTS.Networking {
    public class RTSPlayer : NetworkBehaviour
    {
        [SerializeField] private List<Unit> myUnits = new List<Unit>();
        #region Server
        public override void OnStartServer()
        {
            Unit.ServerOnUnitSpawned += ServerHandlerUnitSpawned;
            Unit.ServerOnUnitDespawned += ServerHandlerUnitDespawned;
        }
        public override void OnStopServer()
        {
            Unit.ServerOnUnitSpawned -= ServerHandlerUnitSpawned;
            Unit.ServerOnUnitDespawned -= ServerHandlerUnitDespawned;
        }
        private void ServerHandlerUnitSpawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

            myUnits.Add(unit);
        }
        private void ServerHandlerUnitDespawned(Unit unit)
        {
            if (unit.connectionToClient.connectionId != connectionToClient.connectionId) return;

            myUnits.Remove(unit);
        }
        #endregion
        #region Client
        public override void OnStartClient()
        {
            if (!isClientOnly) return;
            Unit.AuthorityOnUnitDespawned += AuthorityHandlerUnitSpawned;
            Unit.AuthorityOnUnitDespawned += AuthorityHandlerUnitDespawned;
        }
        public override void OnStopClient()
        {
            if (!isClientOnly) return;
            Unit.AuthorityOnUnitDespawned -= AuthorityHandlerUnitSpawned;
            Unit.AuthorityOnUnitDespawned -= AuthorityHandlerUnitDespawned;
        }
        private void AuthorityHandlerUnitSpawned(Unit unit)
        {
            if (!hasAuthority) return;

            myUnits.Add(unit);
        }
        private void AuthorityHandlerUnitDespawned(Unit unit)
        {
            if (!hasAuthority) return;

            myUnits.Remove(unit);

        }
        #endregion
    }
    }