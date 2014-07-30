using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class RoverViewModel
{
    

    public Vector3 RoverTargetPosition
    {
        get
        {
            return new Vector3(TileX * 10f, 0f, TileY * 10f);
        }
    }
}
