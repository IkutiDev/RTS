using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace RTS.Units
{
    public class UnitSelectionHandler : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        private Camera mainCamera;

        public List<Unit> selectedUnits { get; } = new List<Unit>();

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSelectionArea();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
        }
        private void StartSelectionArea()
        {
            foreach (var selectedUnit in selectedUnits)
            {
                selectedUnit.Deselect();
            }
            selectedUnits.Clear();
        }
        private void ClearSelectionArea()
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) return;

            if (!hit.collider.TryGetComponent(out Unit unit)) return;

            if (!unit.hasAuthority) return;

            selectedUnits.Add(unit);

            foreach (var selectedUnit in selectedUnits)
            {
                selectedUnit.Select();
            }
        }
    }
}