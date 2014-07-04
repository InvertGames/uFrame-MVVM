using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using UnityEngine;
using System.Collections;

public static class uFramePlaymakerExtensions
{
    public static IEnumerable<T> GetVariables<T>(this IEnumerable<PlayMakerFSM> fsms, Func<PlayMakerFSM, T> getVar) where T : NamedVariable
    {
        foreach (var playMakerFsm in fsms)
        {
            var v = getVar(playMakerFsm);
            if (v == null)
                continue;
            yield return v;
        }
    }

    public static void Each<T>(this T[] vars, Action<T> t)
    {
        for (int index = 0; index < vars.Length; index++)
        {
            var v = vars[index];
            t(v);
        }
    }
}
