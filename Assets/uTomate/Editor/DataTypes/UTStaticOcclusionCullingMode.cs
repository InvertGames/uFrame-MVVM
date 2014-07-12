//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;

/// <summary>
/// Enum type that wraps around <see cref="StaticOcclusionCullingMode"/>
/// </summary>
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
[Serializable]
public class UTStaticOcclusionCullingMode : UTEnum<StaticOcclusionCullingMode>
{
}
#endif
