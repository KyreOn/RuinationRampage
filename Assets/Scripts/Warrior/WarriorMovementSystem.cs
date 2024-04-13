using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorMovementSystem : MovementSystem
{
    [SerializeField] private LayerMask dashLayer;

    public void OnChargeStart()
    {
        _controller.excludeLayers = dashLayer;
    }

    public void OnChargeEnd()
    {
        _controller.excludeLayers = aimLayer;
    }
}
