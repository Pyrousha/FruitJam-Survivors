using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool IsInLayermask(int _layer, LayerMask _layermask)
    {
        return _layermask == (_layermask | (1 << _layer));
    }
}
