// Copyright (c) 2018 Jakub Boksansky - All Rights Reserved
// Wilberforce Final Camera Effects Pro Unity Plugin 1.2

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Reflection;

namespace Wilberforce.FinalCameraEffectsPro
{

    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [HelpURL("https://projectwilberforce.github.io/cameralens/manual/")]
    [AddComponentMenu("Image Effects/Rendering/Final Camera Effects Pro")]
    public class FinalCameraEffectsPro : FinalCameraEffectsProCommandBuffer
    {

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            this.PerformOnRenderImage(source, destination);
        }

    }


#if UNITY_EDITOR

    [CustomEditor(typeof(FinalCameraEffectsPro))]
    public class FinalCameraEffectsProEditorImageEffect : FinalCameraEffectsProEditor { }

#endif
}
