using System;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public Dictionary<string, int> Traps;

    private void Start()
    {
        Traps = new Dictionary<string, int>
        {
        };
    }
}