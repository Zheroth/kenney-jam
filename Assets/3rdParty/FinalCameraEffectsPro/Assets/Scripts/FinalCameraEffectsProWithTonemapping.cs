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
    [ImageEffectTransformsToLDR]
    public class FinalCameraEffectsProWithTonemapping : FinalCameraEffectsProCommandBuffer
    {

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            this.PerformOnRenderImage(source, destination);
        }

    }


#if UNITY_EDITOR

    [CustomEditor(typeof(FinalCameraEffectsProWithTonemapping))]
    public class FinalCameraEffectsProWithTonemappingEditorImageEffect : FinalCameraEffectsProEditor { }

#endif
}
