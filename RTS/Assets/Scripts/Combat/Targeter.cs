using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    private Targetable target;
    public Targetable GetTarget()
    {
        return target;
    }
    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent(out Targetable newTarget)) return;

        target = newTarget;
    }
    [Server]
    public void ClearTarget()
    {
        target = null;
    }
}
