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
    public class FinalCameraEffectsProCommandBuffer : MonoBehaviour
    {

        #region Effect Parameters

        #region Distortion

        public bool DistortionEnabled = false;

        public int DistortionOrder = 1;

        /// <summary>
        /// Lower than 0.0f creates pincusion, higher barrel 
        /// </summary>
        [Range(-1.0f, 1.0f)]
        public float DistortionPower = 0.01f;

        public bool IsAnamorphicDistortion = false;

        #endregion

        #region Chromatic Aberrations

        public bool ChromaEnabled = false;

        public int ChromaOrder = 0;

        [Range(0.0f, 1.0f)]
        public float ChromaWeight = 0.8f;

        [Range(0.0f, 1.0f)]
        public float ChromaSize = 0.5f;

        [Range(0.0f, 5.0f)]
        public float ChromaRadialness = 2.0f;

        public float ChromaSpectrumMin = 0.05f;

        public float ChromaSpectrumMax = 0.95f;

        public bool IsAnamorphicChroma = true;

        public Quality ChromaQuality = Quality.Ultra;

        #endregion

        #region Depth of Field

        public bool DofEnabled = true;
        public int DofOrder = 2;

        public float FocalPlaneDistance = 10.0f;

        public float ApertureDiameter = 50.0f;

        public DofApertureType DofAperture = DofApertureType.Hexagonal;

        public DofAlgorithmType DofAlgorithm = DofAlgorithmType.Fast;

        public float DofRotation = 36.0f;

        public DofRangeType DofRange = DofRangeType.ForegroundAndBackground;

        public Quality DofQuality = Quality.High;

        public DofCoCModelType DofCoCModel = DofCoCModelType.RealisticThinLensModel;

        public FocalLengthSourceType FocalLengthSource = FocalLengthSourceType.CameraFOV;

        public float FocalLength = 50.0f;

        public float SensorHeight = 24.0f;

        public DofAnamorphicType DofAnamorphic = DofAnamorphicType.NoStretch;

        public float DofPower = 1.0f;

        public float DofSoftness = 0.98f;

        public float DofAmount = 1.0f;

        public bool DofLumaFilterEnabled = false;
        public bool DofPushHighlightsEnabled = false;
        public bool DofColorTintEnabled = false;

        public float DofLumaThreshold = 0.7f;
        public float DofLumaKneeWidth = 3.0f;
        public float DofLumaKneeLinearity = 0.3f;
        public Color DofColorTint = Color.magenta;

        public DofCoCFromParametersModeType DofCoCFromParametersMode = DofCoCFromParametersModeType.RelativeToFocalPlane;

        public float DofForegroundCoCDistance = 1.0f;
        public float DofBackgroundCoCDistance = 2.0f;

        public float DofForegroundCoCDiameter = 1.0f;
        public float DofBackgroundCoCDiameter = 1.0f;

        public float DofForegroundCoCLinearity = 0.7f;
        public float DofBackgroundCoCLinearity = 0.7f;

        #endregion

        #region Bloom

        public bool BloomEnabled = false;
        public int BloomOrder = 3;

        public bool BloomAntiflickerEnabled = false;

        [Range(0.1f, 2.0f)]
        public float BloomAntiflickerStrength = 1.0f;

        public LengthType BloomAntiflickerLength = LengthType.Medium;

        public FadeType BloomAntiflickerFade = FadeType.FadeInAndOut;

        [Range(0.25f, 1.0f)]
        private float BloomGaussianStrength = 1.0f;

        [Range(0.01f, 4.0f)]
        public float BloomPower = 1.7f;

        [Range(0.01f, 10.0f)]
        public float BloomMultiplier = 1.5f;

        public float BloomThreshold = 0.4f;

        [Range(0.0f, 1.0f)]
        public float BloomSaturation = 1.0f;

        [Range(0.0f, 1.0f)]
        public float BloomRadius = 0.9f;

        [Range(0.0f, 1.0f)]
        public float MaxBloomRadius = 0.4f;

        public Quality BloomQuality = Quality.Ultra;

        #endregion

        #region Vignette

        public bool VignetteEnabled = false;

        public int VignetteOrder = 4;

        [Range(0.0f, 5.0f)]
        public float VignetteFalloff = 2.2f;

        [Range(0.0f, 1.0f)]
        public float VignetteOuterValue = 1.0f;

        [Range(0.0f, 1.0f)]
        public float VignetteInnerValue = 0.0f;

        public float VignetteInnerValueDistance = 0.0f;
        public float VignetteOuterValueDistance = 0.7f;

        public VignetteModeType VignetteMode = VignetteModeType.Standard;

        public Vector2 VignetteCenter = new Vector2(0.5f, 0.5f);

        public Color VignetteInnerColor = new Color(0, 0, 0, 0);

        public Color VignetteOuterColor = new Color(0, 0, 0, 1);

        public float VignetteInnerSaturation = 1.0f;
        public float VignetteOuterSaturation = 0.0f;

        public bool VignetteDebugEnabled = false;
        public bool IsAnamorphicVignette = false;

        #endregion

        #region Color Correction

        public bool ColorGradingEnabled = false;

        public ColorCorrectionModeType ColorCorrectionMode = ColorCorrectionModeType.LUTTexture;

        public float ColorCorrectionIntensity = 1.0f;

        public Texture2D ColorCorrectionLutTexture = null;

        public AnimationCurve ColorCorrectionRedCurve = null;

        public AnimationCurve ColorCorrectionGreenCurve = null;

        public AnimationCurve ColorCorrectionBlueCurve = null;

        public TonemappingModeType TonemappingMode = TonemappingModeType.ACES;

        public float ExposureAdjustment = 0.0f;

        public float TonemappingIntensity = 0.8f;

        public float TonemappingSaturation = 1.0f;

        public float TonemappingContrast = 0.0f;

        public float TonemappingGamma = 1.0f;

        public float TonemappingColorTint = 0.0f;
        
        public float TonemappingColorTemperature = 0.0f;
        
        public int ColorGradingOrder = 5;

        public int manualLUTTextureSize = 32;

        #endregion

        #region Rendering Pipeline

        public Integration IntegrationType = Integration.ImageEffect;

        public CameraEventType IntegrationStage = CameraEventType.BeforeImageEffects;

        public bool UseGBuffer = true;

        public bool UsePreciseDepthBuffer = true;

        public FarPlaneSourceType FarPlaneSource = FarPlaneSourceType.Camera;

        public int Downsampling = 1;

        #endregion

        #endregion

        #region Effect Enums

        public enum FarPlaneSourceType
        {
            ProjectionParams,
            Camera
        }

        public enum DofAnamorphicType
        {
            NoStretch,
            StretchHorizontally,
            StretchVertically
        }

        public enum ScreenTextureFormat
        {
            Auto,
            ARGB32,
            ARGBHalf,
            ARGBFloat,
            Default,
            DefaultHDR,
        }

        public enum DofRangeType
        {
            ForegroundOnly = 1,
            BackgroundOnly = 2,
            ForegroundAndBackground = 3
        }

        public enum DofAlgorithmType
        {
            Fast = 1,
            Precise = 2
        }

        public enum DofApertureType
        {
            Circular = 1,
            Rectangular = 4,
            Pentagonal = 5,
            Hexagonal = 6,
            Octagonal = 8
        }

        public enum DofCoCModelType
        {
            RealisticThinLensModel = 1,
            FromParameters = 2
        }

        public enum DofCoCFromParametersModeType
        {
            Absolute = 1,
            RelativeToFocalPlane = 2
        }

        public enum Quality
        {
            Low = 1,
            High = 2,
            Ultra = 4
        }

        public enum ColorCorrectionModeType
        {
            Off = 0,
            LUTTexture = 1,
            Manual = 2,
        }

        public enum TonemappingModeType
        {
            Off = 0,
            ACES,
        }
        
        public enum FocalLengthSourceType
        {
            CameraFOV = 1,
            Manual = 2
        }

        public enum VignetteModeType
        {
            Standard = 1,
            CustomColors = 2,
            Saturation = 3
        }

        public enum Integration
        {
            ImageEffect = 1,
            CommandBuffer = 2,
            //ScriptablePipeline = 3
        }

        public enum DebugModeType
        {
            None = 0,
            Vignette = 1
        }

        public enum LensEffectType
        {
            Distortion = 1,
            ChromaticAberrations = 2,
            DepthOfField = 3,
            Bloom = 4,
            Vignette = 5,
            ColorGrading = 6
        }

        public enum BloomDofOrderType
        {
            BloomBeforeDepthOfField = 1,
            DepthOfFieldBeforeBloom = 2
        }

        public enum VignetteOrderType
        {
            VignetteBeforeBloom = 1,
            VignetteAfterBloom = 2
        }

        public enum DistortionOrderType
        {
            DistortionBeforeDepthOfField = 1,
            DistortionAfterDepthOfField = 2
        }

        public enum LengthType
        {
            Short = 1,
            Medium = 2,
            Long = 3,
        }

        public enum FadeType
        {
            FadeIn = 1,
            FadeOut = 2,
            FadeInAndOut = 3,
        }

        private enum ShaderPass
        {
            ChromaticAberrationOnly = 0,
            VignetteOnly,
            BloomPrepass,
            BloomPrepassAntiflicker,
            BloomPrepassAntiflickerCmdBuffer,
            BloomMainPass0X,
            BloomMainPass0MixingY,
            BloomMainPass1X,
            BloomMainPass1Y,
            BloomMainPass2X,
            BloomMainPass2Y,
            BloomMainPass3X,
            BloomMainPass3Y,
            BloomMainPass4X,
            BloomMainPass4Y,
            BloomMainPass5X,
            BloomMainPass5Y,
            BloomMainPass6X,
            BloomMainPass6Y,
            BloomMainPass7X,
            BloomMainPass7Y,
            BloomMainPassBottomX,
            BloomMainPassBottomY,
            DistortionOnly,
            CoCMapPass,
            CoCMapPassCmdBuffer,
            CocBlurPass0X,
            CocBlurPass0Y,
            CocBlurPass0MixingY,
            CocBlurPass1X,
            CocBlurPass1Y,
            CocBlurPass2X,
            CocBlurPass2Y,
            CocBlurUpscale,
            DepthOfFieldSeparableBlurPass0,
            DepthOfFieldSeparableBlurPass1,
            DepthOfFieldSeparableBlurPass2,
            DepthOfFieldSeparableBlurPass3,
            DepthOfFieldSeparableBlurPass4,
            DepthOfFieldSeparableBlurPass5,
            DepthOfFieldSeparableGaussianBlurPass0,
            DepthOfFieldSeparableBlurWithMixingPass1,
            DepthOfFieldSeparableBlurWithMixingPass2,
            DepthOfFieldSeparableGaussianBlurWithMixingPass1,
            DepthOfFieldFastMixingPass,
            DebugDisplayPass,
            TexCopy,
            TexCopyImageEffectSPSR,
            ColorCorrectionLut,
            ColorCorrectionLutAces,
            ColorCorrectionAces,
            GenerateColorLUT
        }

        public enum CameraEventType
        {
            BeforeImageEffects,
            AfterImageEffects,
        }

        #endregion

        #region Effect Private Variables


        #region Foldouts

        // Needs to be public so editor won't forget these

        public bool orderFoldout = false;
        public bool pipelineFoldout = false;
        public bool aboutFoldout = false;
        public bool bloomFoldout = false;
        public bool vignetteFoldout = false;
        public bool bokehFoldout = true;
        public bool chromaAbrrFoldout = false;
        public bool distortionFoldout = false;
        public bool lensModelFoldout = false;
        public bool colorCorrectionFoldout = false;

        #endregion

        #region Command Buffer Variables

        private Dictionary<CameraEvent, CommandBuffer> cameraEventsRegistered = new Dictionary<CameraEvent, CommandBuffer>();
        private bool isCommandBufferAlive = false;

        private Mesh screenQuad;

        private int destinationWidth;
        private int destinationHeight;

        private bool onDestroyCalled = false;

        private CameraEvent lastCameraEvent;

        #endregion

        #region Shader, Material, Camera

        public Shader finalCameraEffectsProShader;

        private Camera myCamera = null;
        private bool isSupported;
        private Material FinalCameraEffectsProMaterial;

        #endregion

        #region Warning Flags

        public bool ForcedSwitchPerformedSinglePassStereo = false;
        public bool ForcedSwitchPerformedSinglePassStereoGBuffer = false;

        #endregion

        #region Previous controls values

        private bool ColorLutNeedsRebuild = true;
        private bool lastColorLutNeedsRebuild = false;
        private ColorCorrectionModeType lastColorCorrectionMode;
        private TonemappingModeType lastTonemappingMode;

        private bool isHDR;
        public bool isSPSR;

        private int lastDownsampling;
        private bool lastBloomAntiflickerEnabled;
        private LengthType lastBloomAntiflickerLength;
        private bool lastIsHDR;
        private bool lastIsSPSR;
        private bool lastUseGBuffer;
        private Texture2D lastColorCorrectionLutTexture;

        int lastScreenTextureHeight;
        int lastScreenTextureWidth;

        bool lastDistortionEnabledCmdBuffer;
        bool lastChromaEnabledCmdBuffer;
        bool lastDofEnabledCmdBuffer;
        bool lastBloomEnabledCmdBuffer;
        bool lastVignetteEnabledCmdBuffer;
        bool lastVignetteDebugEnabled;
        bool lastColorGradingEnabledCmdBuffer;

        int lastDistortionOrderCmdBuffer;
        int lastChromaOrderCmdBuffer;
        int lastDofOrderCmdBuffer;
        int lastBloomOrderCmdBuffer;
        int lastVignetteOrderCmdBuffer;
        int lastColorGradingOrderCmdBuffer;

        DofRangeType lastDofRange;
        DofApertureType lastDofAperture;
        DofAlgorithmType lastDofAlgorithm;
        int lastBloomDownSamplingLevels;

        bool lastDistortionEnabled;
        bool lastChromaEnabled;
        bool lastDofEnabled;
        bool lastBloomEnabled;
        bool lastVignetteEnabled;
        bool lastColorGradingEnabled;
        
        int lastDistortionOrder;
        int lastChromaOrder;
        int lastDofOrder;
        int lastBloomOrder;
        int lastVignetteOrder;
        int lastColorGradingOrder;

        int lastDestinationWidth;
        int lastDestinationHeight;

        #endregion

        #region Effects Private Data

        private List<LensEffectType> ActiveEffects = null;

        // chromatic aberration
        private Texture2D chromaSamplesTexture;

        private int lastSpectralSamplesCount = 0;
        private float lastChromaSpectrumMin;
        private float lastChromaSpectrumMax;

        private Vector3 chromaBigWeightRcp;
        private Color[] spectralSamples = null;

        // bloom
        private Vector4[] bloomGaussianNormalized = null;
        private float bloomLastDeviation;
        private const int maxBloomDownsamplingLevels = 8;
        private const int minBloomDownsamplingPixelSize = 7;
        private int bloomDownSamplingLevels;
        private int bloomSamplesCount = 15;

        // depth of field
        private DofApertureType lastDofApertureType = 0;
        private float lastDofRotation;
        private float lastDofAspectRatio;
        private DofAnamorphicType lastDofAnamorphic;
        private int? blurMixingPass1DirectionVectorIdx;
        private int? blurMixingPass2DirectionVectorIdx;
        private int dofMixTypeMixingPass1;
        private int dofMixTypeMixingPass2;
        private int isDofAuxInputHotPass1;
        private int isDofAuxInputHotPass2;

        private Vector4[] bloomGaussianNormalizedBuffer = new Vector4[33];
        int lastBloomGaussianNormalizedBufferLength = 0;

        private Vector4[] separableBlurDirectionsBuffer = new Vector4[6];


        #endregion

        #endregion

        #region Unity Events

        void Start()
        {
            if (finalCameraEffectsProShader == null) finalCameraEffectsProShader = Shader.Find("Hidden/Wilberforce/FinalCameraEffectsPro");
            if (finalCameraEffectsProShader == null)
            {
                ReportError("Could not locate Wilberforce Final Camera Effects Pro Shader. Make sure there is 'FinalCameraEffectsPro.shader' file added to the project.");
                isSupported = false;
                enabled = false;
                return;
            }

            if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth) || SystemInfo.graphicsShaderLevel < 30)
            {
                if (!SystemInfo.supportsImageEffects) ReportError("System does not support backgroundImage effects.");
                if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth)) ReportError("System does not support depth texture.");
                if (SystemInfo.graphicsShaderLevel < 30) ReportError("This effect needs at least Shader Model 3.0.");

                isSupported = false;
                enabled = false;
                return;
            }

            EnsureMaterials();

            if (!FinalCameraEffectsProMaterial || FinalCameraEffectsProMaterial.passCount != Enum.GetValues(typeof(ShaderPass)).Length)
            {
                ReportError("Could not create shader.");
                isSupported = false;
                enabled = false;
                return;
            }

            EnsureChromaSamplesTexture();
            lumaHistoryReady = false;
            bloomLumaHistoryOffset = 0;

            isSupported = true;
        }

        void OnEnable()
        {
            this.myCamera = GetComponent<Camera>();
            TeardownCommandBuffer();

            // See if we are in single pass rendering
#if UNITY_EDITOR
            if (myCamera != null && (ShouldUseCommandBuffer() == false || myCamera.actualRenderingPath == RenderingPath.DeferredShading))
            {
                if (isCameraSPSR(myCamera))
                {
                    if (!ShouldUseCommandBuffer())
                    {
                        if (ForcedSwitchPerformedSinglePassStereo)
                        {
                            ReportWarning("You are running in single pass stereo mode! We recommend switching to command buffer pipeline if you encounter black screen problems.");
                        }
                        else
                        {
                            ReportWarning("You are running in single pass stereo mode! Switching to command buffer pipeline (recommended setting)!");
                            IntegrationType = Integration.CommandBuffer;
                            ForcedSwitchPerformedSinglePassStereo = true;
                        }
                    }

                }

#if UNITY_2017_1_OR_NEWER
                if (isCameraSPSR(myCamera)
                    && myCamera.actualRenderingPath == RenderingPath.DeferredShading)
                {
                    if (!ForcedSwitchPerformedSinglePassStereoGBuffer)
                    {
                        UseGBuffer = true;
                        ForcedSwitchPerformedSinglePassStereoGBuffer = true;
                    }
                }
#endif
            }
#endif

        }

        void OnPreRender()
        {
            EnsureEffectIntegrationVersion();

            bool forceDepthTexture = false;
            bool forceDepthNormalsTexture = false;

            DepthTextureMode currentMode = myCamera.depthTextureMode;
            if (myCamera.actualRenderingPath == RenderingPath.DeferredShading && UseGBuffer)
            {
                forceDepthTexture = true;
            }
            else
            {
                forceDepthNormalsTexture = true;
            }

            if (UsePreciseDepthBuffer && (myCamera.actualRenderingPath == RenderingPath.Forward || myCamera.actualRenderingPath == RenderingPath.VertexLit))
            {
                forceDepthTexture = true;
                forceDepthNormalsTexture = true;

            }

            if (forceDepthTexture)
            {
                if ((currentMode & DepthTextureMode.Depth) != DepthTextureMode.Depth)
                {
                    myCamera.depthTextureMode |= DepthTextureMode.Depth;
                }
            }

            if (forceDepthNormalsTexture)
            {
                if ((currentMode & DepthTextureMode.DepthNormals) != DepthTextureMode.DepthNormals)
                {
                    myCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
                }
            }

            EnsureMaterials();
            EnsureChromaSamplesTexture();

            // Ensure supported aperture type
            if (DofAperture != DofApertureType.Rectangular &&
                DofAperture != DofApertureType.Pentagonal &&
                DofAperture != DofApertureType.Hexagonal &&
                DofAperture != DofApertureType.Octagonal &&
                DofAperture != DofApertureType.Circular) DofAperture = DofApertureType.Hexagonal;

            // Ensure color correction curves
            if (ColorCorrectionRedCurve == null || ColorCorrectionRedCurve.length == 0) ColorCorrectionRedCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
            if (ColorCorrectionGreenCurve == null || ColorCorrectionGreenCurve.length == 0) ColorCorrectionGreenCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
            if (ColorCorrectionBlueCurve == null || ColorCorrectionBlueCurve.length == 0) ColorCorrectionBlueCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

            TrySetUniforms();
            EnsureCommandBuffer(CheckSettingsChanges());

        }

        void OnPostRender()
        {

            if (myCamera == null || myCamera.activeTexture == null) return;

            // Check if cmd. buffer was created with correct target texture sizes and rebuild if necessary
            if (this.destinationWidth != myCamera.activeTexture.width || this.destinationHeight != myCamera.activeTexture.height || !isCommandBufferAlive)
            {
                this.destinationWidth = myCamera.activeTexture.width;
                this.destinationHeight = myCamera.activeTexture.height;

                TeardownCommandBuffer();
                EnsureCommandBuffer();
            }
            else
            {
                // Remember destination texture dimensions for use in command buffer (there are different values in camera.pixelWidth/Height which do not work in Single pass stereo)
                this.destinationWidth = myCamera.activeTexture.width;
                this.destinationHeight = myCamera.activeTexture.height;
            }

        }

        void OnDisable()
        {
            TeardownCommandBuffer();
        }

        void OnDestroy()
        {
            TeardownCommandBuffer();
            onDestroyCalled = true;
        }

        #endregion

        #region Image Effect Implementation

        protected void PerformOnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!isSupported || !finalCameraEffectsProShader.isSupported)
            {
                enabled = false;
                return;
            }

            EnsureMaterials();

            if (ShouldUseCommandBuffer())
            {
                return; //< Return here, drawing will be done in command buffer
            }
            else
            {
                TeardownCommandBuffer();
            }

            int screenTextureWidth = source.width / Downsampling;
            int screenTextureHeight = source.height / Downsampling;
            
            DoEffect(null, source, destination, -1, screenTextureWidth, screenTextureHeight);

        }

        #endregion

        #region Command Buffer Implementation

        private bool ShouldUseCommandBuffer()
        {
            if (IntegrationType != Integration.ImageEffect) return true;
            return false;
        }

        private bool IsColorGradingEnabled()
        {
            if (!ColorGradingEnabled) return false;
            if (ColorCorrectionMode == ColorCorrectionModeType.Off &&
                TonemappingMode == TonemappingModeType.Off) return false;

            if (TonemappingMode == TonemappingModeType.ACES) return true;
            if (ColorCorrectionMode == ColorCorrectionModeType.LUTTexture &&
                ColorCorrectionLutTexture == null) return false;

            return true;
        }

        private void EnsureCommandBuffer(bool settingsDirty = false)
        {
            if ((!settingsDirty && isCommandBufferAlive) || !ShouldUseCommandBuffer()) return;
            if (onDestroyCalled) return;

            try
            {
                CreateCommandBuffer();
                lastCameraEvent = GetCameraEvent(IntegrationStage);
                isCommandBufferAlive = true;
            }
            catch (Exception ex)
            {
                ReportError("There was an error while trying to create command buffer. " + ex.Message);
            }
        }

        private void CreateCommandBuffer()
        {
            CommandBuffer commandBuffer;

            FinalCameraEffectsProMaterial = null;
            EnsureMaterials();

            TrySetUniforms();
            CameraEvent cameraEvent = GetCameraEvent(IntegrationStage);

            if (cameraEventsRegistered.TryGetValue(cameraEvent, out commandBuffer))
            {
                commandBuffer.Clear();
            }
            else
            {
                commandBuffer = new CommandBuffer();
                myCamera.AddCommandBuffer(cameraEvent, commandBuffer);

                commandBuffer.name = "Final Camera Effects Pro";

                // Register
                cameraEventsRegistered[cameraEvent] = commandBuffer;
            }

            int cameraWidth = this.destinationWidth;
            int cameraHeight = this.destinationHeight;

            if (cameraWidth <= 0) cameraWidth = myCamera.pixelWidth;
            if (cameraHeight <= 0) cameraHeight = myCamera.pixelHeight;

            int screenTextureWidth = cameraWidth / Downsampling;
            int screenTextureHeight = cameraHeight / Downsampling;

            RenderTextureFormat screenTextureFormat = GetRenderTextureFormat(ScreenTextureFormat.Auto, isHDR);

            int screenTexture = Shader.PropertyToID("screenTextureRT");
            commandBuffer.GetTemporaryRT(screenTexture, cameraWidth, cameraHeight, 0, FilterMode.Bilinear, screenTextureFormat, RenderTextureReadWrite.Linear);

            // Remember input
            commandBuffer.Blit(BuiltinRenderTextureType.CameraTarget, screenTexture);

            // Perform actual effects
            DoEffect(commandBuffer, null, null, screenTexture, screenTextureWidth, screenTextureHeight);

            // Cleanup
            commandBuffer.ReleaseTemporaryRT(screenTexture);
        }

        private void TeardownCommandBuffer()
        {
            if (!isCommandBufferAlive) return;

            releaseLumaHistory();

            if (customLUTTexture != null)
            {
                customLUTTexture.Release();
                customLUTTexture = null;
            }

            try
            {
                isCommandBufferAlive = false;

                if (myCamera != null)
                {
                    foreach (var e in cameraEventsRegistered)
                    {
                        myCamera.RemoveCommandBuffer(e.Key, e.Value);
                    }
                }

                cameraEventsRegistered.Clear();
                FinalCameraEffectsProMaterial = null;
                EnsureMaterials();
            }
            catch (Exception ex)
            {
                ReportError("There was an error while trying to destroy command buffer. " + ex.Message);
            }
        }

        private CameraEvent GetCameraEvent(CameraEventType cameraEvent)
        {

            switch (cameraEvent)
            {
                case CameraEventType.BeforeImageEffects:
                    return CameraEvent.BeforeImageEffects;
                case CameraEventType.AfterImageEffects:
                    return CameraEvent.AfterImageEffects;
                default:
                    return CameraEvent.BeforeImageEffects;
            }
        }

        #endregion

        #region Shader Utilities

        protected Mesh GetScreenQuad()
        {
            if (screenQuad == null)
            {
                screenQuad = new Mesh()
                {
                    vertices = new Vector3[] { new Vector3(-1, -1, 0), new Vector3(-1, 1, 0), new Vector3(1, 1, 0), new Vector3(1, -1, 0) },
                    triangles = new int[] { 0, 1, 2, 0, 2, 3 },
                    uv = new Vector2[] { new Vector2(0, 1), new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) }
                };
            }

            return screenQuad;
        }

        private void releaseLumaHistory()
        {
            if (lumaHistory != null)
            {
                if (lumaHistory.Length > 0 && lumaHistory[0] != null) lumaHistory[0].Release();
                if (lumaHistory.Length > 1 && lumaHistory[1] != null) lumaHistory[1].Release();

                lumaHistory = null;
            }

            if (lumaHistoryTemp != null)
            {
                lumaHistoryTemp.Release();
                lumaHistoryTemp = null;
            }
        }

        private void TrySetUniforms()
        {
            if (FinalCameraEffectsProMaterial == null) return;

            ResolveActiveEffects();

            FinalCameraEffectsProMaterial.SetInt("isDistortionBeforeDof", (DistortionEnabled && (DistortionOrder < DofOrder)) ? 1 : 0);

            FinalCameraEffectsProMaterial.SetFloat("cameraFarPlane", myCamera.farClipPlane);
            FinalCameraEffectsProMaterial.SetInt("useCameraFarPlane", FarPlaneSource == FarPlaneSourceType.Camera ? 1 : 0);

            FinalCameraEffectsProMaterial.SetInt("isImageEffectMode", IntegrationType == Integration.ImageEffect ? 1 : 0);
            FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

            //CameraLensMaterial.SetInt("mustFlipDistortionInput", 0);
            //CameraLensMaterial.SetInt("mustFlipChromaticAberrationsInput", 0);
            //CameraLensMaterial.SetInt("mustFlipVignetteInput", 0);
            //CameraLensMaterial.SetInt("mustFlipCoCMapInput", 0);
            //CameraLensMaterial.SetInt("mustFlipBloomInput", 0);

            //if (IntegrationType != Integration.ImageEffect)
            //{
            //    if (ActiveEffects != null && ActiveEffects.Count > 0)
            //    {
            //        switch (ActiveEffects[0])
            //        {
            //            case LensEffectType.Distortion:
            //                CameraLensMaterial.SetInt("mustFlipDistortionInput", 1);
            //                break;
            //            case LensEffectType.ChromaticAberrations:
            //                CameraLensMaterial.SetInt("mustFlipChromaticAberrationsInput", 1);
            //                break;
            //            case LensEffectType.Vignette:
            //                CameraLensMaterial.SetInt("mustFlipVignetteInput", 1);
            //                break;
            //            case LensEffectType.DepthOfField:
            //                CameraLensMaterial.SetInt("mustFlipCoCMapInput", 1);
            //                break;
            //            case LensEffectType.Bloom:
            //                CameraLensMaterial.SetInt("mustFlipBloomInput", 1);
            //                break;
            //        }
            //    }
            //}

            int screenTextureWidth = myCamera.pixelWidth / Downsampling;
            int screenTextureHeight = myCamera.pixelHeight / Downsampling;

            if (screenTextureWidth != lastScreenTextureWidth ||
                screenTextureHeight != lastScreenTextureHeight)
            {
                releaseLumaHistory();

                lastScreenTextureWidth = screenTextureWidth;
                lastScreenTextureHeight = screenTextureHeight;
            }

            // Common
            Vector2 texSizeDof = new Vector2(1.0f / screenTextureWidth, 1.0f / screenTextureHeight);

            float aspectRatio = ((float)screenTextureHeight) / ((float)screenTextureWidth);

            //Vector2 texSizeFull = new Vector2(1.0f / myCamera.pixelWidth, 1.0f / myCamera.pixelHeight);

            float minTexelSize = Mathf.Min(texSizeDof.x, texSizeDof.y);
            FinalCameraEffectsProMaterial.SetFloat("minTexelSize", minTexelSize);
            FinalCameraEffectsProMaterial.SetFloat("minTexelSizeDofQuality", minTexelSize * 2.0f);

            FinalCameraEffectsProMaterial.SetFloat("fastBlurMixingCoeff", 1.0f / (minTexelSize * 9.0f));
            FinalCameraEffectsProMaterial.SetVector("dofTexelSize", texSizeDof);
            FinalCameraEffectsProMaterial.SetVector("dofFastPassTexelSize", texSizeDof * 2.0f);

            FinalCameraEffectsProMaterial.SetInt("useGBuffer", ShouldUseGBuffer() ? 1 : 0);

            // Debug
            if (VignetteDebugEnabled)
            {
                FinalCameraEffectsProMaterial.SetInt("debugMode", (int)DebugModeType.Vignette);
            }
            else
            {
                FinalCameraEffectsProMaterial.SetInt("debugMode", (int)DebugModeType.None);
            }
           
            // Color Correction
            if (IsColorGradingEnabled() && ColorCorrectionMode != ColorCorrectionModeType.Off)
            {
                float lutHeight = manualLUTTextureSize;
                float lutWidth = manualLUTTextureSize * manualLUTTextureSize;

                if (ColorCorrectionMode == ColorCorrectionModeType.LUTTexture)
                {
                    if (ColorCorrectionLutTexture != null) {
                        if (ColorCorrectionLutTexture != lastColorCorrectionLutTexture)
                        {
                            ColorCorrectionLutTexture.filterMode = FilterMode.Bilinear;
                            ColorCorrectionLutTexture.wrapMode = TextureWrapMode.Clamp;
                            lastColorCorrectionLutTexture = ColorCorrectionLutTexture;
                        }

                        FinalCameraEffectsProMaterial.SetTexture("lutTexture", ColorCorrectionLutTexture);

                        lutHeight = ColorCorrectionLutTexture.height;
                        lutWidth = ColorCorrectionLutTexture.width;
                    }
                }
                else if (ColorCorrectionMode == ColorCorrectionModeType.Manual && customLUTTexture != null)
                {
                    // Manual LUT mode
                    FinalCameraEffectsProMaterial.SetTexture("lutTexture", customLUTTexture);

                    lutHeight = customLUTTexture.height;
                    lutWidth = customLUTTexture.width;
                }

                Vector4 lutScale = new Vector4((lutHeight - 1.0f) / lutWidth,
                                               (lutHeight - 1.0f) / lutHeight,
                                               (lutHeight - 1.0f),
                                               1.0f / lutHeight);

                Vector2 lutOffset = new Vector2(1.0f / (2.0f * lutWidth), 1.0f / (2.0f * lutHeight));

                FinalCameraEffectsProMaterial.SetVector("lutScale", lutScale);
                FinalCameraEffectsProMaterial.SetVector("lutOffset", lutOffset);

            }

            FinalCameraEffectsProMaterial.SetFloat("colorCorrectionIntensity", ColorCorrectionIntensity);
            FinalCameraEffectsProMaterial.SetFloat("exposureAdjustment", Mathf.Pow(2.0f, Mathf.Sign(ExposureAdjustment) * (ExposureAdjustment * ExposureAdjustment) * 10.0f));
            
            FinalCameraEffectsProMaterial.SetFloat("manualLUTTextureSize", (float) manualLUTTextureSize);
            FinalCameraEffectsProMaterial.SetFloat("tonemappingIntensity", TonemappingIntensity);
            FinalCameraEffectsProMaterial.SetFloat("tonemappingSaturation", TonemappingSaturation);

            FinalCameraEffectsProMaterial.SetFloat("tonemappingGamma", TonemappingGamma);
            FinalCameraEffectsProMaterial.SetFloat("tonemappingContrast", (Mathf.Sign(TonemappingContrast) * (TonemappingContrast * TonemappingContrast))+ 1.0f);
            
            Vector3 colorBalance = new Vector3();

            float colorTint = Mathf.Sign(TonemappingColorTint) * (TonemappingColorTint * TonemappingColorTint);
            float colorTemperature = Mathf.Sign(TonemappingColorTemperature) * (TonemappingColorTemperature * TonemappingColorTemperature);

            colorBalance.x = 1.0f + colorTemperature;
            colorBalance.y = 1.0f + colorTint;
            colorBalance.z = 1.0f - colorTemperature;

            FinalCameraEffectsProMaterial.SetVector("tonemappingColorBalance", colorBalance);

            if (!ColorLutNeedsRebuild) ColorLutNeedsRebuild = CheckColorCorrectionCurvesChanges();

            if (ColorLutNeedsRebuild || customLUTTexture == null)
            {
                SetupLUTGenerator("LUTGeneratorRedValues", ColorCorrectionRedCurve);
                SetupLUTGenerator("LUTGeneratorGreenValues", ColorCorrectionGreenCurve);
                SetupLUTGenerator("LUTGeneratorBlueValues", ColorCorrectionBlueCurve);
            }

            // Lens parameters
            FinalCameraEffectsProMaterial.SetFloat("aspectRatio", aspectRatio);
            FinalCameraEffectsProMaterial.SetFloat("apertureDiameter", ApertureDiameter * 0.001f);
            FinalCameraEffectsProMaterial.SetFloat("focalPlaneDistance", -FocalPlaneDistance);
            float dofFocalLength;

            if (FocalLengthSource == FocalLengthSourceType.CameraFOV)
            {
                dofFocalLength = GetFocalLengthFromVerticalFoV(myCamera.fieldOfView, SensorHeight * 0.001f);
            }
            else
            {
                dofFocalLength = FocalLength * 0.001f;
            }

            FinalCameraEffectsProMaterial.SetFloat("focalLength", dofFocalLength);
            FinalCameraEffectsProMaterial.SetFloat("sensorHeight", SensorHeight * 0.001f);

            float cocCoefficient = ((ApertureDiameter * 0.001f) * (dofFocalLength / ((-FocalPlaneDistance) - dofFocalLength))) / (SensorHeight * 0.001f);
            FinalCameraEffectsProMaterial.SetFloat("cocCoefficient", cocCoefficient);

            // Chromatic aberration
            float chromaQualityCoeff = 2.0f;

            if (ChromaQuality == Quality.High)
            {
                chromaQualityCoeff = 3.0f;
            } else if (ChromaQuality == Quality.Ultra) {
                chromaQualityCoeff = 4.0f;
            }

            FinalCameraEffectsProMaterial.SetFloat("chromaTexSize", ChromaSize * 0.02f);
            FinalCameraEffectsProMaterial.SetFloat("chromaWeight", ChromaWeight);
            FinalCameraEffectsProMaterial.SetFloat("chromaRadialness", ChromaRadialness);
            FinalCameraEffectsProMaterial.SetFloat("chromaQuality", chromaQualityCoeff);
            FinalCameraEffectsProMaterial.SetFloat("chromaSpectrumMin", ChromaSpectrumMin);
            FinalCameraEffectsProMaterial.SetFloat("chromaSpectrumMax", ChromaSpectrumMax);
            EnsureChromaSamplesTexture();

            FinalCameraEffectsProMaterial.SetVector("chromaBigWeightRcp", chromaBigWeightRcp);

            FinalCameraEffectsProMaterial.SetInt("isAnamorphicChroma", IsAnamorphicChroma ? 1 : 0);

            // Vignette
            FinalCameraEffectsProMaterial.SetFloat("vignetteFalloff", VignetteFalloff);
            FinalCameraEffectsProMaterial.SetFloat("vignetteMax", VignetteOuterValue);
            FinalCameraEffectsProMaterial.SetFloat("vignetteMin", VignetteInnerValue);
            FinalCameraEffectsProMaterial.SetFloat("vignetteMaxDistance", VignetteOuterValueDistance);
            FinalCameraEffectsProMaterial.SetFloat("vignetteMinDistance", VignetteInnerValueDistance);

            FinalCameraEffectsProMaterial.SetVector("vignetteCenter", VignetteCenter);
            FinalCameraEffectsProMaterial.SetVector("vignetteInnerColor", VignetteInnerColor);
            FinalCameraEffectsProMaterial.SetVector("vignetteOuterColor", VignetteOuterColor);
            FinalCameraEffectsProMaterial.SetFloat("vignetteSaturationMin", VignetteInnerSaturation);
            FinalCameraEffectsProMaterial.SetFloat("vignetteSaturationMax", VignetteOuterSaturation);
            FinalCameraEffectsProMaterial.SetInt("isAnamorphicVignette", IsAnamorphicVignette ? 1 : 0);
            FinalCameraEffectsProMaterial.SetInt("vignetteMode", (int)VignetteMode);

            // Bloom
            TrySetBloomUniforms(screenTextureWidth, screenTextureHeight);

            // Distortion
            FinalCameraEffectsProMaterial.SetFloat("barrelPower", DistortionPower + 1);
            FinalCameraEffectsProMaterial.SetVector("distortionMaxUV", GetDistortionMaxUV(aspectRatio, IsAnamorphicDistortion, DistortionPower + 1));
            FinalCameraEffectsProMaterial.SetInt("isAnamorphicDistortion", IsAnamorphicDistortion ? 1 : 0);

            // Depth of Field
            FinalCameraEffectsProMaterial.SetInt("dofRange", (int)DofRange);
            FinalCameraEffectsProMaterial.SetInt("dofQuality", (int)DofQuality);

            switch (DofQuality)
            {
                case Quality.High:
                    FinalCameraEffectsProMaterial.SetFloat("dofQualityCoeff", 1.0f);
                    break;
                case Quality.Low:
                    FinalCameraEffectsProMaterial.SetFloat("dofQualityCoeff", 0.75f);
                    break;
                case Quality.Ultra:
                    FinalCameraEffectsProMaterial.SetFloat("dofQualityCoeff", 1.5f);
                    break;
            }

            Vector2 tempTexSizeRcp = new Vector2(1.0f / screenTextureWidth, 1.0f / screenTextureHeight);
            int cocBlurMipLevels = getDofMipLevelsCount(screenTextureWidth, screenTextureHeight);

            // Downsample and blur
            for (int i = 0; i < cocBlurMipLevels; i++)
            {
                FinalCameraEffectsProMaterial.SetVector("cocBlurTexSizeRcp" + i, tempTexSizeRcp);
                tempTexSizeRcp *= 2.0f;
            }

            DofPass[] dofPasses = GetAperturePasses(DofAperture);

            float rotationToUse = DofRotation;
            if (DofAperture == DofApertureType.Circular) rotationToUse = 0.0f;

            if (!blurMixingPass1DirectionVectorIdx.HasValue || DofPassesChanged(DofAperture) || DofSettingsChanged(DofRotation, aspectRatio, DofAnamorphic))
            {
                blurMixingPass1DirectionVectorIdx = null;
                blurMixingPass2DirectionVectorIdx = null;

                for (int i = 0; i < dofPasses.Length; i++)
                {
                    separableBlurDirectionsBuffer[i] = GetSeparableBlurDirectionalVector(dofPasses[i].Angle + rotationToUse, dofPasses[i].Min, dofPasses[i].Max, aspectRatio, DofAnamorphic);

                    if (dofPasses[i].IsMixingPass)
                    {
                        if (!blurMixingPass1DirectionVectorIdx.HasValue)
                        {
                            blurMixingPass1DirectionVectorIdx = i;
                            dofMixTypeMixingPass1 = (int)dofPasses[i].MixingOperation;
                            isDofAuxInputHotPass1 = (dofPasses[i].AuxSource == DofPassTextureType.None ? 0 : 1);

                        }
                        else if (!blurMixingPass2DirectionVectorIdx.HasValue)
                        {
                            blurMixingPass2DirectionVectorIdx = i;
                            dofMixTypeMixingPass2 = (int)dofPasses[i].MixingOperation;
                            isDofAuxInputHotPass2 = (dofPasses[i].AuxSource == DofPassTextureType.None ? 0 : 1);
                        }
                        else
                        {
                            throw new Exception("Wilberforce Final Camera Effects Pro depth of field failed - too many mixing passes");
                        }
                    }
                }

                lastDofApertureType = DofAperture;
                lastDofRotation = DofRotation;
                lastDofAspectRatio = aspectRatio;
                lastDofAnamorphic = DofAnamorphic;
            }

            FinalCameraEffectsProMaterial.SetInt("dofMixTypeMixingPass1", dofMixTypeMixingPass1);
            FinalCameraEffectsProMaterial.SetInt("dofMixTypeMixingPass2", dofMixTypeMixingPass2);
            FinalCameraEffectsProMaterial.SetInt("isDofAuxInputHotPass1", isDofAuxInputHotPass1);
            FinalCameraEffectsProMaterial.SetInt("isDofAuxInputHotPass2", isDofAuxInputHotPass2);

            FinalCameraEffectsProMaterial.SetFloat("dofPower", DofPower);

            if (DofAperture == DofApertureType.Circular)
                FinalCameraEffectsProMaterial.SetFloat("dofSoftness", 0.0f);
            else
                FinalCameraEffectsProMaterial.SetFloat("dofSoftness", (1.0f - DofSoftness) * 4.0f);

            FinalCameraEffectsProMaterial.SetFloat("dofAmount", DofAmount);

            FinalCameraEffectsProMaterial.SetInt("isDofLumaFilter", DofLumaFilterEnabled ? 1 : 0);
            FinalCameraEffectsProMaterial.SetInt("isDofColortint", DofColorTintEnabled ? 1 : 0);
            FinalCameraEffectsProMaterial.SetInt("isDofPushHighlights", DofPushHighlightsEnabled ? 1 : 0);

            FinalCameraEffectsProMaterial.SetFloat("DofLumaThreshold", DofLumaThreshold);
            FinalCameraEffectsProMaterial.SetFloat("DofLumaKneeWidth", DofLumaKneeWidth);
            FinalCameraEffectsProMaterial.SetFloat("DofLumaTwiceKneeWidthRcp", 1.0f / (DofLumaKneeWidth * 2.0f));
            FinalCameraEffectsProMaterial.SetFloat("DofLumaKneeLinearity", DofLumaKneeLinearity);
            FinalCameraEffectsProMaterial.SetColor("dofColorTint", DofColorTint);

            float dofForegroundCoCDistance = DofForegroundCoCDistance;
            float dofBackgroundCoCDistance = DofBackgroundCoCDistance;
            float dofForegroundCoCDiameter = DofForegroundCoCDiameter;
            float dofBackgroundCoCDiameter = DofBackgroundCoCDiameter;

            if (DofCoCFromParametersMode == DofCoCFromParametersModeType.RelativeToFocalPlane)
            {
                dofForegroundCoCDistance = FocalPlaneDistance - DofForegroundCoCDistance;
                dofBackgroundCoCDistance = FocalPlaneDistance + DofBackgroundCoCDistance;
            }

            FinalCameraEffectsProMaterial.SetFloat("dofForegroundMaxCoCDistance", -dofForegroundCoCDistance);
            FinalCameraEffectsProMaterial.SetFloat("dofBackgroundMaxCoCDistance", -dofBackgroundCoCDistance);
            FinalCameraEffectsProMaterial.SetFloat("dofForegroundMaxCoCDiameter", dofForegroundCoCDiameter);
            FinalCameraEffectsProMaterial.SetFloat("dofBackgroundMaxCoCDiameter", dofBackgroundCoCDiameter);

            FinalCameraEffectsProMaterial.SetFloat("dofForegroundCoCLinearity", DofForegroundCoCLinearity);
            FinalCameraEffectsProMaterial.SetFloat("dofBackgroundCoCLinearity", DofBackgroundCoCLinearity);

            FinalCameraEffectsProMaterial.SetInt("isDofThinLensModel", DofCoCModel == DofCoCModelType.RealisticThinLensModel ? 1 : 0);

            SetVectorArrayNoBuffer("separableBlurDirections", FinalCameraEffectsProMaterial, separableBlurDirectionsBuffer);

            if (blurMixingPass1DirectionVectorIdx.HasValue) FinalCameraEffectsProMaterial.SetInt("blurMixingPass1DirectionVectorIdx", blurMixingPass1DirectionVectorIdx.Value);
            if (blurMixingPass2DirectionVectorIdx.HasValue) FinalCameraEffectsProMaterial.SetInt("blurMixingPass2DirectionVectorIdx", blurMixingPass2DirectionVectorIdx.Value);

        }

        AnimationCurve lastColorCorrectionRedCurve;
        AnimationCurve lastColorCorrectionGreenCurve;
        AnimationCurve lastColorCorrectionBlueCurve;

        private bool IsSameCurve(AnimationCurve a, AnimationCurve b)
        {
            if (a == null || b == null) return false;

            if (a.length != b.length) return false;

            for (int i = 0; i < a.length; i++) {

                var keyA = a.keys[i];
                var keyB = b.keys[i];

                if (keyA.value != keyB.value) return false;
                if (keyA.time != keyB.time) return false;
                if (keyA.inTangent != keyB.inTangent) return false;
                if (keyA.outTangent != keyB.outTangent) return false;
            }

            return true;
        }

        private bool CheckColorCorrectionCurvesChanges()
        {
            bool result = false;

            if (!IsSameCurve(ColorCorrectionRedCurve, lastColorCorrectionRedCurve)) result = true;
            if (!IsSameCurve(ColorCorrectionGreenCurve, lastColorCorrectionGreenCurve)) result = true;
            if (!IsSameCurve(ColorCorrectionBlueCurve, lastColorCorrectionBlueCurve)) result = true;

            if (result)
            {
                lastColorCorrectionRedCurve = new AnimationCurve(ColorCorrectionRedCurve.keys);
                lastColorCorrectionGreenCurve = new AnimationCurve(ColorCorrectionGreenCurve.keys);
                lastColorCorrectionBlueCurve = new AnimationCurve(ColorCorrectionBlueCurve.keys);
            }

            return result;
        }

        private void SetupLUTGenerator(string uniformName, AnimationCurve colorCorrectionCurve)
        {

            float[] colorValues = new float[manualLUTTextureSize];

            for (int i = 0; i < manualLUTTextureSize; i++)
            {
                colorValues[i] = colorCorrectionCurve.Evaluate(((float)i) / (manualLUTTextureSize - 1));
            }

            FinalCameraEffectsProMaterial.SetFloatArray(uniformName, colorValues);
        }

        private void SetKeywords(string offState, string onState, bool state)
        {
            if (state)
            {
                FinalCameraEffectsProMaterial.DisableKeyword(offState);
                FinalCameraEffectsProMaterial.EnableKeyword(onState);
            }
            else
            {
                FinalCameraEffectsProMaterial.DisableKeyword(onState);
                FinalCameraEffectsProMaterial.EnableKeyword(offState);
            }
        }

        private void EnsureMaterials()
        {
            if (finalCameraEffectsProShader == null) finalCameraEffectsProShader = Shader.Find("Hidden/Wilberforce/FinalCameraEffectsPro");

            if (!FinalCameraEffectsProMaterial && finalCameraEffectsProShader.isSupported)
            {
                FinalCameraEffectsProMaterial = CreateMaterial(finalCameraEffectsProShader);
            }

            if (!finalCameraEffectsProShader.isSupported)
            {
                ReportError("Could not create shader (Shader not supported).");
            }
        }

        private static Material CreateMaterial(Shader shader)
        {
            if (!shader) return null;

            Material m = new Material(shader);
            m.hideFlags = HideFlags.HideAndDontSave;

            return m;
        }

        private static void DestroyMaterial(Material mat)
        {
            if (mat)
            {
                DestroyImmediate(mat);
                mat = null;
            }
        }

        private void SetVectorArrayNoBuffer(string name, Material Material, Vector4[] samples)
        {
#if UNITY_5_4_OR_NEWER

            Material.SetVectorArray(name, samples);
#else
            for (int i = 0; i < samples.Length; ++i)
            {
                Material.SetVector(name + i.ToString(), samples[i]);
            }
#endif
        }

        private void SetVectorArray(string name, Material Material, Vector4[] samples, ref Vector4[] samplesBuffer, ref int lastBufferLength, bool needsUpdate)
        {
#if UNITY_5_4_OR_NEWER

            if (needsUpdate || lastBufferLength != samples.Length)
            {
                Array.Copy(samples, samplesBuffer, samples.Length);
                lastBufferLength = samples.Length;
            }

            Material.SetVectorArray(name, samplesBuffer);
#else
            for (int i = 0; i < samples.Length; ++i)
            {
                Material.SetVector(name + i.ToString(), samples[i]);
            }
#endif
        }

        #region Common Functionality for Image Effect and Command Buffer

        private class ShaderTextureInfo
        {
            public ShaderTextureInfo()
            {
                CmdBufferTextureInt = -1;
            }

            public Texture Texture { get; set; }
            public RenderTargetIdentifier CmdBufferTexture { get; set; }
            public int CmdBufferTextureInt { get; set; }

            public static implicit operator ShaderTextureInfo(Texture tex)
            {
                return new ShaderTextureInfo()
                {
                    Texture = tex
                };
            }

            public static implicit operator ShaderTextureInfo(int tex)
            {
                return new ShaderTextureInfo()
                {
                    CmdBufferTexture = tex,
                    CmdBufferTextureInt = tex
                };
            }

            public static implicit operator ShaderTextureInfo(BuiltinRenderTextureType tex)
            {
                return new ShaderTextureInfo()
                {
                    CmdBufferTexture = tex,
                };
            }
        }

        private void SetShaderTexture(CommandBuffer commandBuffer, string texName, ShaderTextureInfo textureInfo, Material material)
        {
            if (commandBuffer == null)
            {
                Texture texToUse = null;
                if (textureInfo != null && textureInfo.Texture != null) texToUse = textureInfo.Texture;
                material.SetTexture(texName, texToUse);
            }
            else
            {
                if (textureInfo != null && textureInfo.Texture != null)
                {
                    commandBuffer.SetGlobalTexture(texName, textureInfo.Texture);
                }
                else
                {
                    if (textureInfo != null && textureInfo.CmdBufferTextureInt != -1)
                    {
                        commandBuffer.SetGlobalTexture(texName, textureInfo.CmdBufferTexture);
                    }
                    else
                    {
                        //commandBuffer.SetGlobalTexture(texName, default(RenderTargetIdentifier));
                    }
                }
            }
        }

        private static ShaderTextureInfo GetTemporaryTexture(CommandBuffer commandBuffer, string name, int width, int height, FilterMode filterMode, RenderTextureFormat textureFormat)
        {

            if (commandBuffer != null)
            {
                int cmdTexture = Shader.PropertyToID(name);

                commandBuffer.GetTemporaryRT(cmdTexture, width, height, 0, filterMode, textureFormat, RenderTextureReadWrite.Linear);

                return cmdTexture;
            }
            else
            {
                var tex = RenderTexture.GetTemporary(width, height, 0, textureFormat);
                tex.filterMode = filterMode;
                return tex;
            }
        }

        private static void ReleaseTempTexture(CommandBuffer commandBuffer, ref ShaderTextureInfo textureInfo)
        {
            if (textureInfo == null) return;

            if (commandBuffer != null)
            {
                commandBuffer.ReleaseTemporaryRT(textureInfo.CmdBufferTextureInt);
                textureInfo.CmdBufferTextureInt = -1;
            }
            else
            {
                RenderTexture.ReleaseTemporary(textureInfo.Texture as RenderTexture);
                textureInfo.Texture = null;
            }
        }

        private void DoShaderBlitCopy(CommandBuffer commandBuffer, ShaderTextureInfo sourceTexture, ShaderTextureInfo destinationTexture)
        {
            if (isSPSR && IntegrationType == Integration.ImageEffect)
            {
                FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 1);
                SetShaderTexture(commandBuffer, "texCopySource", sourceTexture, FinalCameraEffectsProMaterial);
                DoShaderBlit(commandBuffer, sourceTexture, destinationTexture, FinalCameraEffectsProMaterial, (int)ShaderPass.TexCopyImageEffectSPSR);
                FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);
            }
            else
            {
                DoShaderBlit(commandBuffer, sourceTexture, destinationTexture, null, 0);
            }
        }

        private void DoShaderBlit(CommandBuffer commandBuffer, ShaderTextureInfo sourceTexture, ShaderTextureInfo destinationTexture, Material material, int passNum, bool forceDestinationRenderTexture = true)
        {
            if (commandBuffer != null)
            {
                if (material != null)
                {
                    if (destinationTexture.CmdBufferTextureInt != -1 || forceDestinationRenderTexture)
                    {
                        commandBuffer.Blit(sourceTexture.CmdBufferTexture, destinationTexture.CmdBufferTexture, material, passNum);
                    }
                    else
                    {
                        commandBuffer.Blit(sourceTexture.CmdBufferTexture, destinationTexture.Texture, material, passNum);
                    }
                }
                else
                {
                    if (sourceTexture.CmdBufferTextureInt == -1)
                    {
                        if (destinationTexture.CmdBufferTextureInt == -1)
                        {
                            commandBuffer.Blit(sourceTexture.Texture, destinationTexture.Texture);
                        }
                        else
                        {
                            commandBuffer.Blit(sourceTexture.Texture, destinationTexture.CmdBufferTexture);
                        }
                    }
                    else
                    {
                        if (destinationTexture.CmdBufferTextureInt == -1)
                        {
                            commandBuffer.Blit(sourceTexture.CmdBufferTexture, destinationTexture.Texture);
                        }
                        else
                        {
                            commandBuffer.Blit(sourceTexture.CmdBufferTexture, destinationTexture.CmdBufferTexture);
                        }
                    }
                }
            }
            else
            {
                if (material != null)
                {
                    Graphics.Blit(sourceTexture.Texture, destinationTexture.Texture as RenderTexture, material, passNum);
                }
                else
                {
                    Graphics.Blit(sourceTexture.Texture, destinationTexture.Texture as RenderTexture);
                }
            }
        }

        private void DoShaderBlitMRT(CommandBuffer commandBuffer, ShaderTextureInfo sourceTexture, ShaderTextureInfo[] destinationTextures, Material material, int passNum)
        {
            if (destinationTextures == null || destinationTextures.Length == 0) return;

            if (commandBuffer != null)
            {
                var target = new RenderTargetIdentifier[destinationTextures.Length];

                for (int i = 0; i < destinationTextures.Length; i++)
                {
                    if (destinationTextures[i].Texture != null)
                    {
#if UNITY_2017_0_OR_NEWER
                        target[i] = (destinationTextures[i].Texture as RenderTexture).colorBuffer;
#else
                        target[i] = (destinationTextures[i].Texture as RenderTexture);
#endif
                    }
                    else
                    {
                        target[i] = destinationTextures[i].CmdBufferTexture;
                    }
                }

                commandBuffer.SetRenderTarget(target, destinationTextures[0].CmdBufferTexture);

                commandBuffer.DrawMesh(GetScreenQuad(), Matrix4x4.identity, material, 0, passNum);
            }
            else
            {
                RenderBuffer[] renderBuffer = new RenderBuffer[destinationTextures.Length];

                for (int i = 0; i < destinationTextures.Length; i++)
                {
                    if (destinationTextures[i] == null || destinationTextures[i].Texture == null) return;
                    renderBuffer[i] = (destinationTextures[i].Texture as RenderTexture).colorBuffer;
                }

                Graphics.SetRenderTarget(renderBuffer, (destinationTextures[0].Texture as RenderTexture).depthBuffer);

                Graphics.Blit(sourceTexture.Texture, material, passNum);
            }
        }

        #endregion

        #endregion

        #region Effect Data Utilities

        private void EnsureChromaSamplesTexture(int SpectralSamplesCount = 64)
        {

            bool chromaRangeChanged = (ChromaSpectrumMin != lastChromaSpectrumMin || ChromaSpectrumMax != lastChromaSpectrumMax);

            lastChromaSpectrumMin = ChromaSpectrumMin;
            lastChromaSpectrumMax = ChromaSpectrumMax;

            if (chromaSamplesTexture == null || lastSpectralSamplesCount != SpectralSamplesCount || chromaRangeChanged)
            {

                if (spectralSamples == null || lastSpectralSamplesCount != SpectralSamplesCount || chromaRangeChanged)
                {
                    spectralSamples = GenerateSpectralSamples(SpectralSamplesCount, out chromaBigWeightRcp);
                    lastSpectralSamplesCount = SpectralSamplesCount;
                }

                chromaSamplesTexture = new Texture2D(SpectralSamplesCount, 1, TextureFormat.RGBAFloat, false, true);
                chromaSamplesTexture.SetPixels(spectralSamples);
                chromaSamplesTexture.filterMode = FilterMode.Bilinear;
                chromaSamplesTexture.wrapMode = TextureWrapMode.Clamp;
                chromaSamplesTexture.Apply();
            }
        }

        public Texture GetChromaTexture()
        {
            return chromaSamplesTexture;
        }

        private static Vector4[] GenerateGaussian(int size, float d, bool normalize = true)
        {
            Vector4[] result = new Vector4[size];
            float norm = 0.0f;

            double twodd = 2.0 * d * d;
            double sqrt2ddpi = Math.Sqrt(twodd * Math.PI);

            float phase = (1.0f / (size + 1));
            for (int i = 0; i < size; i++)
            {
                float u = i / (float)(size + 1);
                u += phase;
                u *= 6.0f;
                float uminus3 = (u - 3.0f);

                float temp = (float)(-(Math.Exp(-(uminus3 * uminus3) / twodd)) / sqrt2ddpi);

                result[i].x = temp;
                norm += temp;
            }

            if (normalize)
            {
                for (int i = 0; i < size; i++)
                {
                    result[i].x /= norm;
                }
            }

            return result;
        }

        private Color[] GenerateSpectralSamples(int samplesCount, out Vector3 bigWeightRcp)
        {
            var result = new Color[samplesCount];
            Vector3 weight = new Vector3(0.0f, 0.0f, 0.0f);

            float chromaMin = ChromaSpectrumMin;
            float chromaMax = ChromaSpectrumMax;
            float minDifference = 0.35f;

            if (ChromaSpectrumMax - ChromaSpectrumMin < minDifference)
                ChromaSpectrumMax = ChromaSpectrumMin + minDifference;

            if (ChromaSpectrumMax > 1.0f)
            {
                ChromaSpectrumMax = 1.0f;
                ChromaSpectrumMin = 1.0f - minDifference;
            }

            for (int i = 0; i < samplesCount; ++i)
            {
                // Filter out red from beginning and end of spectrum
                float temphue = Mathf.Lerp(chromaMin, chromaMax, ((float)(i) / (float)(samplesCount - 1)));

                Color hue = new Color(Math.Abs(temphue * 6.0f - 3.0f) - 1.0f,
                    2.0f - Math.Abs(temphue * 6.0f - 2.0f),
                    2.0f - Math.Abs(temphue * 6.0f - 4.0f), 0.0f);

                hue.r = Mathf.Clamp01(hue.r);
                hue.g = Mathf.Clamp01(hue.g);
                hue.b = Mathf.Clamp01(hue.b);

                hue.a = (((float)(i) / (float)(samplesCount - 1)) - 0.5f) * 10;

                result[i] = hue;

                weight.x += hue.r;
                weight.y += hue.g;
                weight.z += hue.b;
            }

            bigWeightRcp = new Vector3(1.0f / weight.x, 1.0f / weight.y, 1.0f / weight.z);

            return result;
        }

        #endregion

        #region Effect Implementation Utilities

        public bool ShouldUseGBuffer()
        {
            if (myCamera == null) return UseGBuffer;

            if (myCamera.actualRenderingPath != RenderingPath.DeferredShading) return false;

            return UseGBuffer;
        }

        public bool ShouldDoTonemapping()
        {
            if (!ColorGradingEnabled) return false;
            if (TonemappingMode == TonemappingModeType.Off) return false;

            return true;
        }

        protected void EnsureEffectIntegrationVersion()
        {
            if (ShouldUseCommandBuffer() && (this is FinalCameraEffectsProCommandBuffer) && !(this is FinalCameraEffectsPro) && !(this is FinalCameraEffectsProWithTonemapping)) return;
            if (!ShouldUseCommandBuffer() && (((this is FinalCameraEffectsPro) && !ShouldDoTonemapping()) || ((this is FinalCameraEffectsProWithTonemapping) && ShouldDoTonemapping()))) return;

            var allComponents = GetComponents<Component>();
            var parameters = GetParameters();

            int oldComponentIndex = -1;
            Component newComponent = null;

            for (int i = 0; i < allComponents.Length; i++)
            {
                if (ShouldUseCommandBuffer() && (allComponents[i] == this))
                {
                    var oldGameObject = gameObject;
                    DestroyImmediate(this);
                    newComponent = oldGameObject.AddComponent<FinalCameraEffectsProCommandBuffer>();
                    (newComponent as FinalCameraEffectsProCommandBuffer).SetParameters(parameters);
                    oldComponentIndex = i;
                    break;
                }

                if (!ShouldUseCommandBuffer() && ((allComponents[i] == this)))
                {
                    var oldGameObject = gameObject;
                    TeardownCommandBuffer();
                    DestroyImmediate(this);

                    if (ShouldDoTonemapping())
                    {
                        newComponent = oldGameObject.AddComponent<FinalCameraEffectsProWithTonemapping>();
                        (newComponent as FinalCameraEffectsProWithTonemapping).SetParameters(parameters);
                    }
                    else
                    {
                        newComponent = oldGameObject.AddComponent<FinalCameraEffectsPro>();
                        (newComponent as FinalCameraEffectsPro).SetParameters(parameters);
                    }
                    oldComponentIndex = i;
                    break;
                }
            }

            if (oldComponentIndex >= 0 && newComponent != null)
            {
#if UNITY_EDITOR
                allComponents = newComponent.gameObject.GetComponents<Component>();
                int currentIndex = 0;

                for (int i = 0; i < allComponents.Length; i++)
                {
                    if (allComponents[i] == newComponent)
                    {
                        currentIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < currentIndex - oldComponentIndex; i++)
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(newComponent);
                }
#endif
            }
        }

        private bool CheckSettingsChanges()
        {
            bool settingsDirty = false;

            if (GetCameraEvent(IntegrationStage) != lastCameraEvent)
            {
                TeardownCommandBuffer();
            }

            if (Downsampling != lastDownsampling)
            {
                lastDownsampling = Downsampling;
                settingsDirty = true;
            }

            if (BloomAntiflickerEnabled != lastBloomAntiflickerEnabled)
            {
                lastBloomAntiflickerEnabled = BloomAntiflickerEnabled;
                settingsDirty = true;
            }

            if (BloomAntiflickerLength != lastBloomAntiflickerLength)
            {
                lastBloomAntiflickerLength = BloomAntiflickerLength;
                settingsDirty = true;
            }            

            if (UseGBuffer != lastUseGBuffer)
            {
                lastUseGBuffer = UseGBuffer;
                settingsDirty = true;
            }

            isHDR = isCameraHDR(myCamera);
            if (isHDR != lastIsHDR)
            {
                lastIsHDR = isHDR;
                settingsDirty = true;
            }

            if (TonemappingMode != lastTonemappingMode)
            {
                lastTonemappingMode = TonemappingMode;
                settingsDirty = true;
            }

            if (ColorLutNeedsRebuild || ColorLutNeedsRebuild != lastColorLutNeedsRebuild)
            {
                lastColorLutNeedsRebuild = ColorLutNeedsRebuild;
                settingsDirty = true;
            }

            if (ColorCorrectionMode != lastColorCorrectionMode)
            {
                if (ColorCorrectionMode == ColorCorrectionModeType.Manual) ColorLutNeedsRebuild = true;

                lastColorCorrectionMode = ColorCorrectionMode;
                settingsDirty = true;
            }

#if UNITY_EDITOR
            isSPSR = isCameraSPSR(myCamera);
            if (isSPSR != lastIsSPSR)
            {
                lastIsSPSR = isSPSR;
                settingsDirty = true;
            }
#endif

            if (destinationWidth != lastDestinationWidth)
            {
                lastDestinationWidth = destinationWidth;
                settingsDirty = true;
            }

            if (destinationHeight != lastDestinationHeight)
            {
                lastDestinationHeight = destinationHeight;
                settingsDirty = true;
            }

            if (DofRange != lastDofRange)
            {
                lastDofRange = DofRange;
                settingsDirty = true;
            }

            if (DofAperture != lastDofAperture)
            {
                lastDofAperture = DofAperture;
                settingsDirty = true;
            }

            if (DofAlgorithm != lastDofAlgorithm)
            {
                lastDofAlgorithm = DofAlgorithm;
                settingsDirty = true;
            }

            if (bloomDownSamplingLevels != lastBloomDownSamplingLevels)
            {
                lastBloomDownSamplingLevels = bloomDownSamplingLevels;
                settingsDirty = true;
            }

            if (DistortionEnabled != lastDistortionEnabledCmdBuffer)
            {
                lastDistortionEnabledCmdBuffer = DistortionEnabled;
                settingsDirty = true;
            }

            if (ChromaEnabled != lastChromaEnabledCmdBuffer)
            {
                lastChromaEnabledCmdBuffer = ChromaEnabled;
                settingsDirty = true;
            }

            if (DofEnabled != lastDofEnabledCmdBuffer)
            {
                lastDofEnabledCmdBuffer = DofEnabled;
                settingsDirty = true;
            }

            if (BloomEnabled != lastBloomEnabledCmdBuffer)
            {
                lastBloomEnabledCmdBuffer = BloomEnabled;
                settingsDirty = true;
            }

            if (VignetteEnabled != lastVignetteEnabledCmdBuffer)
            {
                lastVignetteEnabledCmdBuffer = VignetteEnabled;
                settingsDirty = true;
            }

            if (IsColorGradingEnabled() != lastColorGradingEnabledCmdBuffer)
            {
                lastColorGradingEnabledCmdBuffer = IsColorGradingEnabled();
                settingsDirty = true;
            }

            if (DistortionOrder != lastDistortionOrderCmdBuffer)
            {
                lastDistortionOrderCmdBuffer = DistortionOrder;
                settingsDirty = true;
            }

            if (ChromaOrder != lastChromaOrderCmdBuffer)
            {
                lastChromaOrderCmdBuffer = ChromaOrder;
                settingsDirty = true;
            }

            if (DofOrder != lastDofOrderCmdBuffer)
            {
                lastDofOrderCmdBuffer = DofOrder;
                settingsDirty = true;
            }

            if (BloomOrder != lastBloomOrderCmdBuffer)
            {
                lastBloomOrderCmdBuffer = BloomOrder;
                settingsDirty = true;
            }

            if (VignetteOrder != lastVignetteOrderCmdBuffer)
            {
                lastVignetteOrderCmdBuffer = VignetteOrder;
                settingsDirty = true;
            }

            if (ColorGradingOrder != lastColorGradingOrderCmdBuffer)
            {
                lastColorGradingOrderCmdBuffer = ColorGradingOrder;
                settingsDirty = true;
            }

            if (VignetteDebugEnabled != lastVignetteDebugEnabled)
            {
                lastVignetteDebugEnabled = VignetteDebugEnabled;
                settingsDirty = true;
            }

            return settingsDirty;
        }

        private RenderTextureFormat GetRenderTextureFormat(ScreenTextureFormat format, bool isHDR)
        {
            switch (format)
            {
                case ScreenTextureFormat.Default:
                    return RenderTextureFormat.Default;
                case ScreenTextureFormat.DefaultHDR:
                    return RenderTextureFormat.DefaultHDR;
                case ScreenTextureFormat.ARGB32:
                    return RenderTextureFormat.ARGB32;
                case ScreenTextureFormat.ARGBFloat:
                    return RenderTextureFormat.ARGBFloat;
                case ScreenTextureFormat.ARGBHalf:
                    return RenderTextureFormat.ARGBHalf;
                default:
                    return isHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
            }
        }

        private class MipmappedRenderBuffer
        {
            ~MipmappedRenderBuffer()
            {
                Release(this.cachedCmdBuffer);
            }

            public void Prepare(CommandBuffer commandBuffer, string name, int width, int height, RenderTextureFormat format, int levelsCount, FilterMode filterMode, bool includeTopLevel = true, bool includeDepth = true, bool includeNormals = false)
            {
                if (colorBufferTextures == null || levelsCount > colorBufferTextures.Length) colorBufferTextures = new ShaderTextureInfo[levelsCount];
                if (includeDepth && (depthBufferTextures == null || levelsCount > depthBufferTextures.Length)) depthBufferTextures = new ShaderTextureInfo[levelsCount];
                if (includeNormals && (normalsBufferTextures == null || levelsCount > normalsBufferTextures.Length)) normalsBufferTextures = new ShaderTextureInfo[levelsCount];

                Release(this.cachedCmdBuffer);

                cachedCmdBuffer = commandBuffer;

                int divider = includeTopLevel ? 1 : 2;

                for (int i = 0; i < levelsCount; i++)
                {

                    colorBufferTextures[i] = GetTemporaryTexture(commandBuffer, name + "_" + divider, width / divider, height / divider, filterMode, format);
                    if (includeDepth) depthBufferTextures[i] = GetTemporaryTexture(commandBuffer, name + "_depth_" + divider, width / divider, height / divider, filterMode, format);
                    if (includeNormals) normalsBufferTextures[i] = GetTemporaryTexture(commandBuffer, name + "_normals_" + divider, width / divider, height / divider, filterMode, format);

                    divider *= 2;
                }
            }

            public void Release(CommandBuffer commandBuffer)
            {
                TryReleseTextures(commandBuffer, colorBufferTextures);
                TryReleseTextures(commandBuffer, depthBufferTextures);
                TryReleseTextures(commandBuffer, normalsBufferTextures);

                cachedCmdBuffer = null;
            }

            private void TryReleseTextures(CommandBuffer commandBuffer, ShaderTextureInfo[] textures)
            {
                if (textures == null) return;
                for (int i = 0; i < textures.Length; i++)
                {
                    if (textures[i] != null) ReleaseTempTexture(commandBuffer, ref textures[i]);
                    textures[i] = null;
                }
            }

            public ShaderTextureInfo[] colorBufferTextures { get; private set; }
            public ShaderTextureInfo[] depthBufferTextures { get; private set; }
            public ShaderTextureInfo[] normalsBufferTextures { get; private set; }

            private CommandBuffer cachedCmdBuffer;
        }

        #endregion

        #region Unity Utilities

        private void ReportError(string error)
        {
            if (Debug.isDebugBuild) Debug.LogError("Wilberforce Final Camera Effects Pro Effect Error: " + error);
        }

        private void ReportWarning(string error)
        {
            if (Debug.isDebugBuild) Debug.LogWarning("Wilberforce Final Camera Effects Pro Effect Warning: " + error);
        }


        private bool isCameraSPSR(Camera camera)
        {
            if (camera == null) return false;

#if UNITY_5_5_OR_NEWER
            if (camera.stereoEnabled)
            {
#if UNITY_2017_2_OR_NEWER

                return (UnityEngine.XR.XRSettings.eyeTextureDesc.vrUsage == VRTextureUsage.TwoEyes);
#else

#if !UNITY_WEBGL
#if UNITY_EDITOR
                if (camera.stereoEnabled && PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass)
                    return true;
#endif
#endif

#endif
            }
#endif

            return false;
        }

        private bool isCameraHDR(Camera camera)
        {

#if UNITY_5_6_OR_NEWER
            if (camera != null) return camera.allowHDR;
#else
            if (camera != null) return camera.hdr;
#endif
            return false;
        }

        protected List<KeyValuePair<FieldInfo, object>> GetParameters()
        {
            var result = new List<KeyValuePair<FieldInfo, object>>();

            var fields = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var field in fields)
            {
                result.Add(new KeyValuePair<FieldInfo, object>(field, field.GetValue(this)));
            }

            return result;
        }

        protected void SetParameters(List<KeyValuePair<FieldInfo, object>> parameters)
        {
            foreach (var parameter in parameters)
            {
                parameter.Key.SetValue(this, parameter.Value);
            }
        }

        #endregion

        #region Effect Implementations

        void DoEffect(CommandBuffer commandBuffer, RenderTexture source, RenderTexture destination, int cmdBufferScreenTexture, int screenTextureWidth, int screenTextureHeight)
        {

            RenderTextureFormat screenTextureFormat = GetRenderTextureFormat(ScreenTextureFormat.Auto, isHDR);

            ShaderTextureInfo sourceTexture = source;
            ShaderTextureInfo destinationTexture = destination;

            if (commandBuffer != null)
            {
                sourceTexture = cmdBufferScreenTexture;
                destinationTexture = BuiltinRenderTextureType.CameraTarget;
            }

            if (VignetteEnabled && VignetteDebugEnabled)
            {
                FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);
                DoShaderBlit(commandBuffer, sourceTexture, destinationTexture, FinalCameraEffectsProMaterial, (int)ShaderPass.DebugDisplayPass);
                FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);
                return;
            }

            if (ActiveEffects != null && ActiveEffects.Count > 0)
            {
                ShaderTextureInfo tempTexture1 = null;
                ShaderTextureInfo tempTexture2 = null;

                if (ActiveEffects.Count > 1)
                {
                    tempTexture1 = GetTemporaryTexture(commandBuffer, "LensTempTexture1_RT", screenTextureWidth, screenTextureHeight, FilterMode.Point, screenTextureFormat);
                    if (ActiveEffects.Count > 2) tempTexture2 = GetTemporaryTexture(commandBuffer, "LensTempTexture2_RT", screenTextureWidth, screenTextureHeight, FilterMode.Point, screenTextureFormat);
                }

                ShaderTextureInfo[] destinationTextures = {
                    tempTexture1,
                    tempTexture2
                    };

                int currentDestinationIdx = 0;

                for (int i = 0; i < ActiveEffects.Count; i++)
                {
                    destinationTexture = destinationTextures[currentDestinationIdx];
                    if (i == ActiveEffects.Count - 1)
                    {
                        if (commandBuffer != null)
                        {
                            destinationTexture = BuiltinRenderTextureType.CameraTarget;
                        }
                        else
                        {
                            destinationTexture = destination;
                        }
                    }

                    ProcessEffect(commandBuffer, screenTextureWidth, screenTextureHeight, ActiveEffects[i], sourceTexture, destinationTexture, screenTextureFormat);

                    sourceTexture = destinationTexture;
                    currentDestinationIdx = (currentDestinationIdx + 1) % 2;
                }

                ReleaseTempTexture(commandBuffer, ref tempTexture1);
                ReleaseTempTexture(commandBuffer, ref tempTexture2);
            }
            else
            {
                if (commandBuffer == null)
                {
                    Graphics.Blit(source, destination);
                }
            }

        }

        private int getDofMipLevelsCount(int width, int height)
        {
            // Determine amount of foreground coc blur mip levels
            int cocBlurMipLevels = getMipLevelsCountForScreenSize(width, height, 0.05f, 11);

            cocBlurMipLevels = Math.Min(3, cocBlurMipLevels);
            cocBlurMipLevels = Math.Max(1, cocBlurMipLevels);

            return cocBlurMipLevels;
        }

        private RenderTexture customLUTTexture = null;

        private void ProcessEffect(CommandBuffer commandBuffer, int width, int height, LensEffectType lensEffectType, ShaderTextureInfo sourceTexture, ShaderTextureInfo destinationTexture, RenderTextureFormat screenTextureFormat)
        {
            switch (lensEffectType)
            {
                case LensEffectType.Distortion:

                    // Distortion
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);
                    DoShaderBlit(commandBuffer, sourceTexture, destinationTexture, FinalCameraEffectsProMaterial, (int)ShaderPass.DistortionOnly);
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

                    break;
                case LensEffectType.ChromaticAberrations:

                    // Chromatic aberration
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);
                    DoShaderBlit(commandBuffer, sourceTexture, destinationTexture, FinalCameraEffectsProMaterial, (int)ShaderPass.ChromaticAberrationOnly);
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

                    break;
                case LensEffectType.Vignette:

                    // Vignette
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);
                    DoShaderBlit(commandBuffer, sourceTexture, destinationTexture, FinalCameraEffectsProMaterial, (int)ShaderPass.VignetteOnly);
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

                    break;
                case LensEffectType.Bloom:

                    // Bloom  
                    ExecuteBloomBlits(commandBuffer, sourceTexture, width, height, screenTextureFormat, destinationTexture);

                    break;
                case LensEffectType.DepthOfField:

                    // Depth of field 

                    ShaderTextureInfo dofInputTexture = GetTemporaryTexture(commandBuffer, "dofInputTexture", width, height, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR);
                    ShaderTextureInfo circleOfConfusionTexture = GetTemporaryTexture(commandBuffer, "circleOfConfusionTexture", width, height, FilterMode.Bilinear, RenderTextureFormat.RHalf);

                    ShaderTextureInfo[] cocPassRTs = new ShaderTextureInfo[2]
                    {
                        dofInputTexture,
                        circleOfConfusionTexture
                    };
                    SetShaderTexture(commandBuffer, "cocInputTexture", sourceTexture, FinalCameraEffectsProMaterial);
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 1);

                    DoShaderBlitMRT(commandBuffer, sourceTexture, cocPassRTs, FinalCameraEffectsProMaterial, commandBuffer != null ? (int)ShaderPass.CoCMapPassCmdBuffer : (int)ShaderPass.CoCMapPass);
                    ShaderTextureInfo cocFinalTemp = null;
                    ShaderTextureInfo cocHalfTemp = null;

                    SetShaderTexture(commandBuffer, "circleOfConfusionTexture", circleOfConfusionTexture, FinalCameraEffectsProMaterial);
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

                    bool downsampleCoCBlurInput = true;

                    if (DofRange == DofRangeType.ForegroundOnly || DofRange == DofRangeType.ForegroundAndBackground)
                    {
                        // Determine amount of foreground coc blur mip levels
                        int cocBlurMipLevels = getDofMipLevelsCount(width, height);

                        if (downsampleCoCBlurInput)
                        {
                            // Downsample coc input for blurring
                            cocHalfTemp = GetTemporaryTexture(commandBuffer, "cocHalfTempTexture", width / 2, height / 2, FilterMode.Bilinear, RenderTextureFormat.RHalf);

                            DoShaderBlitCopy(commandBuffer, circleOfConfusionTexture, cocHalfTemp);
                        }

                        int cocBlurWidth = width;
                        int cocBlurHeight = height;

                        if (downsampleCoCBlurInput)
                        {
                            cocBlurWidth /= 2;
                            cocBlurHeight /= 2;
                        }

                        cocFinalTemp = GetTemporaryTexture(commandBuffer, "cocFinalTempTexture", width, height, FilterMode.Bilinear, RenderTextureFormat.RHalf);

                        // Blur foreground coc texture - fix sharp shilouette of blurred doreground against in-focus background
                        MipmappedRenderBuffer downsamplingCocTextures = new MipmappedRenderBuffer();
                        downsamplingCocTextures.Prepare(commandBuffer, "blurCoCTextures", cocBlurWidth, cocBlurHeight, RenderTextureFormat.RHalf, cocBlurMipLevels, FilterMode.Bilinear, false, false, false);

                        FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);

                        // Downsample and blur
                        for (int i = 0; i < cocBlurMipLevels; i++)
                        {
                            bool isXPass = (i % 2 == 0);

                            ShaderTextureInfo sourceTex = circleOfConfusionTexture;
                            if (downsampleCoCBlurInput)
                                sourceTex = cocHalfTemp;

                            if (i != 0) sourceTex = downsamplingCocTextures.colorBufferTextures[i - 1];

                            DoShaderBlit(commandBuffer, sourceTex, downsamplingCocTextures.colorBufferTextures[i], FinalCameraEffectsProMaterial, (int)GetCoCBlurShaderPass(i, isXPass));
                        }

                        // Upscale and blur
                        for (int i = cocBlurMipLevels - 1; i >= 0; i--)
                        {
                            bool isXPass = (i % 2 == 1);

                            ShaderTextureInfo passDestinationTex = cocFinalTemp;

                            if (downsampleCoCBlurInput)
                                passDestinationTex = cocHalfTemp;

                            if (i != 0) passDestinationTex = downsamplingCocTextures.colorBufferTextures[i - 1];

                            int shaderPass = (int)GetCoCBlurShaderPass(i, isXPass);
                            if (i == 0)
                            {
                                if (downsampleCoCBlurInput)
                                    shaderPass = (int)ShaderPass.CocBlurPass0Y;
                                else
                                    shaderPass = (int)ShaderPass.CocBlurPass0MixingY;
                            }

                            DoShaderBlit(commandBuffer, downsamplingCocTextures.colorBufferTextures[i], passDestinationTex, FinalCameraEffectsProMaterial, shaderPass);
                        }

                        if (downsampleCoCBlurInput)
                        {
                            DoShaderBlit(commandBuffer, cocHalfTemp, cocFinalTemp, FinalCameraEffectsProMaterial, (int)ShaderPass.CocBlurUpscale);
                            if (cocHalfTemp != null) ReleaseTempTexture(commandBuffer, ref cocHalfTemp);
                        }

                        FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

                        downsamplingCocTextures.Release(commandBuffer);
                    }

                    if (cocFinalTemp != null)
                        SetShaderTexture(commandBuffer, "circleOfConfusionTexture", cocFinalTemp, FinalCameraEffectsProMaterial);
                    else
                        SetShaderTexture(commandBuffer, "circleOfConfusionTexture", circleOfConfusionTexture, FinalCameraEffectsProMaterial);

                    SetShaderTexture(commandBuffer, "dofInputTexture", dofInputTexture, FinalCameraEffectsProMaterial);

                    // Do effect blits
                    if (DofAlgorithm == DofAlgorithmType.Fast)
                    {

                        ShaderTextureInfo dofInputTextureHalf = GetTemporaryTexture(commandBuffer, "dofInputTextureHalf", width / 2, height / 2, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR);
                        ShaderTextureInfo dofOutputTextureHalf = GetTemporaryTexture(commandBuffer, "dofInputTextureHalf", width / 2, height / 2, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR);

                        DoShaderBlitCopy(commandBuffer, dofInputTexture, dofInputTextureHalf);

                        ExecuteDofImageEffectBlits(commandBuffer, GetAperturePasses(DofAperture), width / 2, height / 2, screenTextureFormat, dofInputTextureHalf, dofOutputTextureHalf);

                        FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);

                        DoShaderBlit(commandBuffer, dofOutputTextureHalf, destinationTexture, FinalCameraEffectsProMaterial, (int)ShaderPass.DepthOfFieldFastMixingPass);

                        FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

                        ReleaseTempTexture(commandBuffer, ref dofInputTextureHalf);
                        ReleaseTempTexture(commandBuffer, ref dofOutputTextureHalf);
                    }
                    else
                    {
                        ExecuteDofImageEffectBlits(commandBuffer, GetAperturePasses(DofAperture), width, height, screenTextureFormat, dofInputTexture, destinationTexture);
                    }

                    // Cleanup
                    if (cocFinalTemp != null) ReleaseTempTexture(commandBuffer, ref cocFinalTemp);
                    ReleaseTempTexture(commandBuffer, ref circleOfConfusionTexture);
                    ReleaseTempTexture(commandBuffer, ref dofInputTexture);

                    break;
                case LensEffectType.ColorGrading:
                    
                    // Generate LUT if necessary
                    if (ColorCorrectionMode == ColorCorrectionModeType.Manual && (ColorLutNeedsRebuild || customLUTTexture == null))
                    {
                        if (customLUTTexture == null)
                            customLUTTexture = new RenderTexture(manualLUTTextureSize * manualLUTTextureSize, manualLUTTextureSize, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);

                        customLUTTexture.filterMode = FilterMode.Bilinear;

                        FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 1);
                        DoShaderBlit(commandBuffer, sourceTexture, customLUTTexture, FinalCameraEffectsProMaterial, (int) ShaderPass.GenerateColorLUT, false);
                        FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);
                    }
                    ColorLutNeedsRebuild = false;

                    if (ColorCorrectionMode == ColorCorrectionModeType.Manual)
                    {
                        SetShaderTexture(commandBuffer, "lutTexture", customLUTTexture, FinalCameraEffectsProMaterial);
                    }

                    int colorGradingPass = (int)ShaderPass.ColorCorrectionLut;

                    if (TonemappingMode == TonemappingModeType.ACES)
                    {
                        if (ColorCorrectionMode == ColorCorrectionModeType.Manual || (ColorCorrectionMode == ColorCorrectionModeType.LUTTexture && ColorCorrectionLutTexture != null))
                        {
                            colorGradingPass = (int)ShaderPass.ColorCorrectionLutAces;
                        } else {
                            colorGradingPass = (int)ShaderPass.ColorCorrectionAces;
                        }
                    } else {
                        colorGradingPass = (int)ShaderPass.ColorCorrectionLut;
                    }

                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 1);
                    DoShaderBlit(commandBuffer, sourceTexture, destinationTexture, FinalCameraEffectsProMaterial, colorGradingPass);
                    FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

                    break;
            }
        }

        private void ResolveActiveEffects()
        {
            if (ActiveEffects == null)
                ActiveEffects = new List<LensEffectType>();

            // End early if no changes
            if (lastDistortionEnabled == DistortionEnabled &&
                lastChromaEnabled == ChromaEnabled &&
                lastDofEnabled == DofEnabled &&
                lastBloomEnabled == BloomEnabled &&
                lastVignetteEnabled == VignetteEnabled &&
                lastColorGradingEnabled == IsColorGradingEnabled() &&
                lastDistortionOrder == DistortionOrder &&
                lastChromaOrder == ChromaOrder &&
                lastDofOrder == DofOrder &&
                lastBloomOrder == BloomOrder &&
                lastVignetteOrder == VignetteOrder &&
                lastColorGradingOrder == ColorGradingOrder) return;

            lastDistortionEnabled = DistortionEnabled;
            lastChromaEnabled = ChromaEnabled;
            lastDofEnabled = DofEnabled;
            lastBloomEnabled = BloomEnabled;
            lastVignetteEnabled = VignetteEnabled;
            lastColorGradingEnabled = IsColorGradingEnabled();

            lastDistortionOrder = DistortionOrder;
            lastChromaOrder = ChromaOrder;
            lastDofOrder = DofOrder;
            lastBloomOrder = BloomOrder;
            lastVignetteOrder = VignetteOrder;
            lastColorGradingOrder = ColorGradingOrder;

            ActiveEffects.Clear();

            for (int i = 0; i < 6; i++)
            {
                if (i == DistortionOrder && DistortionEnabled) ActiveEffects.Add(LensEffectType.Distortion);
                if (i == ChromaOrder && ChromaEnabled) ActiveEffects.Add(LensEffectType.ChromaticAberrations);
                if (i == DofOrder && DofEnabled) ActiveEffects.Add(LensEffectType.DepthOfField);
                if (i == BloomOrder && BloomEnabled) ActiveEffects.Add(LensEffectType.Bloom);
                if (i == VignetteOrder && VignetteEnabled) ActiveEffects.Add(LensEffectType.Vignette);
                if (i == ColorGradingOrder && IsColorGradingEnabled()) ActiveEffects.Add(LensEffectType.ColorGrading);
            }
        }

        #region Bloom

        private RenderTexture[] lumaHistory = null;
        private int lumaHistoryCurrentIdx = 0;
        private bool lumaHistoryReady = false;
        private RenderTexture lumaHistoryTemp = null;

        private void ExecuteBloomBlits(CommandBuffer commandBuffer, ShaderTextureInfo sourceTexture, int width, int height, RenderTextureFormat screenTextureFormat, ShaderTextureInfo destinationTexture)
        {
            ShaderTextureInfo tempFilterPassTexture = GetTemporaryTexture(commandBuffer, "BloomFilterPassTexture_RT", width, height, FilterMode.Point, screenTextureFormat);

            FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 1);

            // Filtering pre-pass
            if (BloomAntiflickerEnabled)
            {
                int historyFramesNeeded = (int)BloomAntiflickerLength;

                if (commandBuffer != null) historyFramesNeeded = 1;

                if (commandBuffer != null) lumaHistoryCurrentIdx = 0;

                if (lumaHistory == null || lumaHistory.Length != historyFramesNeeded)
                {

                    releaseLumaHistory();

                    lumaHistoryReady = false;
                    bloomLumaHistoryOffset = 0;
                    lumaHistory = new RenderTexture[historyFramesNeeded];

                    for (int i = 0; i < historyFramesNeeded; i++)
                        lumaHistory[i] = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);
                }

                if (lumaHistoryTemp == null)
                    lumaHistoryTemp = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);

                ShaderTextureInfo[] prepassRTs = new ShaderTextureInfo[2]
                    {
                        tempFilterPassTexture,
                        lumaHistoryTemp
                    };

                SetShaderTexture(commandBuffer, "bloomPrepassSource", sourceTexture, FinalCameraEffectsProMaterial);

                SetShaderTexture(commandBuffer, "lumaHistoryBuffer3", lumaHistory[0], FinalCameraEffectsProMaterial);
                if (lumaHistory.Length > 1) SetShaderTexture(commandBuffer, "lumaHistoryBuffer2", lumaHistory[1], FinalCameraEffectsProMaterial);
                if (lumaHistory.Length > 2) SetShaderTexture(commandBuffer, "lumaHistoryBuffer", lumaHistory[2], FinalCameraEffectsProMaterial);

                DoShaderBlitMRT(commandBuffer, sourceTexture, prepassRTs, FinalCameraEffectsProMaterial, (int)(commandBuffer == null ? ShaderPass.BloomPrepassAntiflicker : ShaderPass.BloomPrepassAntiflickerCmdBuffer));

                if (commandBuffer != null)
                {
                    SetShaderTexture(commandBuffer, "texCopySource", lumaHistoryTemp, FinalCameraEffectsProMaterial);
                    commandBuffer.SetRenderTarget(lumaHistory[0]);
                    commandBuffer.DrawMesh(GetScreenQuad(), Matrix4x4.identity, FinalCameraEffectsProMaterial, 0, (int)ShaderPass.TexCopy);
                }
                else
                {
                    // Put current target as next source
                    var tempBuffer = lumaHistory[0];
                    lumaHistory[0] = lumaHistoryTemp;
                    lumaHistoryTemp = tempBuffer;

                    // Round robin
                    if (lumaHistoryCurrentIdx == 3 && lumaHistory.Length > 1)
                    {
                        var tempBuffer2 = lumaHistory[0];

                        for (int i = 0; i < lumaHistory.Length - 1; i++)
                            lumaHistory[i] = lumaHistory[i + 1];

                        lumaHistory[lumaHistory.Length - 1] = tempBuffer2;
                    }
                   
                }

                lumaHistoryCurrentIdx = (lumaHistoryCurrentIdx + 1) % 4;
            }
            else
            {
                DoShaderBlit(commandBuffer, sourceTexture, tempFilterPassTexture, FinalCameraEffectsProMaterial, (int)ShaderPass.BloomPrepass);
            }

            // Allocate downsampling textures
            MipmappedRenderBuffer downsamplingTextures = new MipmappedRenderBuffer();
            downsamplingTextures.Prepare(commandBuffer, "bloomTextures", width, height, screenTextureFormat, bloomDownSamplingLevels, FilterMode.Bilinear, false, false, false);

            SetShaderTexture(commandBuffer, "bloomSourceTexture", sourceTexture, FinalCameraEffectsProMaterial);
            FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);

            // Downsample and blur
            for (int i = 0; i < bloomDownSamplingLevels; i++)
            {
                bool isXPass = (i % 2 == 0);

                ShaderTextureInfo sourceTex = tempFilterPassTexture;
                if (i != 0) sourceTex = downsamplingTextures.colorBufferTextures[i - 1];

                DoShaderBlit(commandBuffer, sourceTex, downsamplingTextures.colorBufferTextures[i], FinalCameraEffectsProMaterial, (int)GetBloomShaderPass(i, isXPass));
            }

            // Upscale and blur
            for (int i = bloomDownSamplingLevels - 1; i >= 0; i--)
            {
                bool isXPass = (i % 2 == 1);

                ShaderTextureInfo passDestinationTex = destinationTexture;
                if (i != 0) passDestinationTex = downsamplingTextures.colorBufferTextures[i - 1];

                if (bloomDownSamplingLevels > 1 &&
                    i == bloomDownSamplingLevels - 1)
                {
                    int shaderPass = (int)(isXPass ? ShaderPass.BloomMainPassBottomX : ShaderPass.BloomMainPassBottomY);

                    // Scaling fix
                    DoShaderBlit(commandBuffer, downsamplingTextures.colorBufferTextures[i], passDestinationTex, FinalCameraEffectsProMaterial, shaderPass);
                }
                else
                {
                    int shaderPass = (int)GetBloomShaderPass(i, isXPass);
                    if (i == 0) shaderPass = (int)ShaderPass.BloomMainPass0MixingY;

                    DoShaderBlit(commandBuffer, downsamplingTextures.colorBufferTextures[i], passDestinationTex, FinalCameraEffectsProMaterial, shaderPass);
                }
            }

            FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);

            downsamplingTextures.Release(commandBuffer);
            ReleaseTempTexture(commandBuffer, ref tempFilterPassTexture);
        }

        private ShaderPass GetCoCBlurShaderPass(int level, bool isXPass)
        {
            if (isXPass)
            {
                switch (level)
                {
                    case 0: return ShaderPass.CocBlurPass0X;
                    case 1: return ShaderPass.CocBlurPass1X;
                    case 2: return ShaderPass.CocBlurPass2X;
                    default: throw new Exception("Wilberforce Final Camera Effects Pro: invalid coc blur mip level " + level);
                }
            }
            else
            {
                switch (level)
                {
                    case 0: return ShaderPass.CocBlurPass0MixingY;
                    case 1: return ShaderPass.CocBlurPass1Y;
                    case 2: return ShaderPass.CocBlurPass2Y;
                    default: throw new Exception("Wilberforce Final Camera Effects Pro: invalid coc blur mip level " + level);
                }
            }
        }

        private ShaderPass GetBloomShaderPass(int level, bool isXPass)
        {
            if (isXPass)
            {
                switch (level)
                {
                    case 0: return ShaderPass.BloomMainPass0X;
                    case 1: return ShaderPass.BloomMainPass1X;
                    case 2: return ShaderPass.BloomMainPass2X;
                    case 3: return ShaderPass.BloomMainPass3X;
                    case 4: return ShaderPass.BloomMainPass4X;
                    case 5: return ShaderPass.BloomMainPass5X;
                    case 6: return ShaderPass.BloomMainPass6X;
                    case 7: return ShaderPass.BloomMainPass7X;
                    default: throw new Exception("Wilberforce Final Camera Effects Pro: invalid bloom mip level " + level);
                }
            }
            else
            {
                switch (level)
                {
                    case 0: return ShaderPass.BloomMainPass0MixingY;
                    case 1: return ShaderPass.BloomMainPass1Y;
                    case 2: return ShaderPass.BloomMainPass2Y;
                    case 3: return ShaderPass.BloomMainPass3Y;
                    case 4: return ShaderPass.BloomMainPass4Y;
                    case 5: return ShaderPass.BloomMainPass5Y;
                    case 6: return ShaderPass.BloomMainPass6Y;
                    case 7: return ShaderPass.BloomMainPass7Y;
                    default: throw new Exception("Wilberforce Final Camera Effects Pro: invalid bloom mip level " + level);
                }
            }
        }

        private int getMipLevelsCountForScreenSize(int width, int height, float screenSize, int pixelsRangePerLevel)
        {
            int rangeInPixels = (int)Math.Ceiling(Math.Max(width, height) * screenSize);
            return (int)Math.Ceiling(Mathf.Log(rangeInPixels / pixelsRangePerLevel, 2));
        }

        private int CalculateBloomLevels(int width, int height, float bloomRadius, int pixelsRangePerLevel)
        {
            int levelNeeded = getMipLevelsCountForScreenSize(width, height, bloomRadius, pixelsRangePerLevel);

            int maxAllowedLevels = (int)Mathf.Floor(Mathf.Log(Mathf.Min(width, height) / minBloomDownsamplingPixelSize, 2));
            int downSamplingLevels = Math.Min(levelNeeded, maxBloomDownsamplingLevels);
            downSamplingLevels = Math.Min(downSamplingLevels, maxAllowedLevels);
            downSamplingLevels = Math.Max(downSamplingLevels, 2);

            return downSamplingLevels;
        }

        public int GetBloomMipLevels()
        {
            return bloomDownSamplingLevels;
        }

        private int bloomLumaHistoryOffset = 0;

        private void TrySetBloomUniforms(int width, int height)
        {
            // Range calculations
            float bloomRadius = (1.0f - Mathf.Sqrt(1.0f - (BloomRadius * BloomRadius))) * MaxBloomRadius;
            int pixelsRangePerLevel = bloomSamplesCount;

            if (BloomPower > 3.0f)
            {
                bloomRadius += (BloomPower - 3.0f) * 0.02f;
            }

            bloomRadius = Math.Min(bloomRadius, MaxBloomRadius);

            // Limit number of downscaling levels
            bloomDownSamplingLevels = CalculateBloomLevels(width, height, bloomRadius, pixelsRangePerLevel);

            Vector2 texSizeRcp = new Vector2(1.0f / width, 1.0f / height);

            // Calculate bottom level range
            float bottomLevelMax = Mathf.Min(texSizeRcp.x, texSizeRcp.y) * (pixelsRangePerLevel * (int)Math.Pow(2, bloomDownSamplingLevels));
            float bottomLevelRange = bottomLevelMax - (bottomLevelMax * 0.5f);
            float bottomLevelRadius = (bloomRadius - (bottomLevelMax * 0.5f)) / bottomLevelRange;
            bottomLevelRadius = Mathf.Clamp01(bottomLevelRadius);

            // Downsample and blur
            for (int i = 0; i < bloomDownSamplingLevels; i++)
            {
                if (i == bloomDownSamplingLevels - 1)
                {
                    FinalCameraEffectsProMaterial.SetVector("bloomTexSizeRcp" + i, texSizeRcp * bottomLevelRadius);
                    FinalCameraEffectsProMaterial.SetVector("bloomTexSizeRcpBottom", texSizeRcp * bottomLevelRadius);
                }
                else
                {
                    FinalCameraEffectsProMaterial.SetVector("bloomTexSizeRcp" + i, texSizeRcp);
                }

                texSizeRcp *= 2.0f;
            }

            // Antiflicker
            if (!BloomAntiflickerEnabled)
            {
                lumaHistoryReady = false;
                bloomLumaHistoryOffset = 0;
            }
            else
            {
                TrySetAntiflickerUniforms();
            }

            FinalCameraEffectsProMaterial.SetFloat("bloomPower", 1.0f / BloomPower);
            FinalCameraEffectsProMaterial.SetFloat("bloomMultiplier", BloomMultiplier);
            FinalCameraEffectsProMaterial.SetFloat("bottomLevelRadius", bottomLevelRadius);

            switch (BloomQuality)
            {
                case Quality.Low:
                    bloomSamplesCount = 7;
                    break;
                case Quality.High:
                    bloomSamplesCount = 11;
                    break;
                case Quality.Ultra:
                    bloomSamplesCount = 15;
                    break;
            }

            FinalCameraEffectsProMaterial.SetFloat("bloomThreshold", BloomThreshold);
            FinalCameraEffectsProMaterial.SetFloat("bloomSaturation", BloomSaturation);
            FinalCameraEffectsProMaterial.SetInt("bloomSize", bloomSamplesCount);
            //FinalLensMaterial.SetColor("bloomColorTint", BloomColorTint);

            bool bloomGaussianNormalizedNeedUpdate = false;
            if (bloomGaussianNormalized == null || bloomGaussianNormalized.Length != bloomSamplesCount || BloomGaussianStrength != bloomLastDeviation)
            {
                bloomGaussianNormalized = GenerateGaussian(bloomSamplesCount, BloomGaussianStrength);
                bloomLastDeviation = BloomGaussianStrength;
                bloomGaussianNormalizedNeedUpdate = true;
            }

            SetVectorArray("bloomGauss", FinalCameraEffectsProMaterial, bloomGaussianNormalized, ref bloomGaussianNormalizedBuffer, ref lastBloomGaussianNormalizedBufferLength, bloomGaussianNormalizedNeedUpdate);
        }

        private void TrySetAntiflickerUniforms()
        {
            float slope = BloomAntiflickerStrength;

            Vector4 lumaHistoryMask = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

            float currentLumaWeight = 0.0f;
            lumaHistoryMask[bloomLumaHistoryOffset % 4] = 1.0f;

            int historyFramesNeeded = (int)BloomAntiflickerLength;

            if (ShouldUseCommandBuffer()) historyFramesNeeded = 1;

            Vector4[] lumaHistoryWeights = new Vector4[historyFramesNeeded];

            float temp = slope;
            float allWeights = 0.0f;

            for (int i = 0; i < historyFramesNeeded; i++)
            {
                lumaHistoryWeights[i] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

                if (lumaHistoryReady)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        allWeights += temp;
                        lumaHistoryWeights[i][j] = temp;
                        temp += slope;
                    }

                    currentLumaWeight = temp;
                } 
            }
            allWeights += temp;

            if (lumaHistoryReady)
            {
                Vector4 tempVectorOrdered = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

                for (int j = 0; j < 4; j++)
                    tempVectorOrdered[(j+bloomLumaHistoryOffset) % 4] = lumaHistoryWeights[0][j];

                lumaHistoryWeights[0] = tempVectorOrdered;
            }
            else
            {
                currentLumaWeight = 1.0f;
            }

            for (int i = 0; i < historyFramesNeeded; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    lumaHistoryWeights[i][j] /= allWeights;
                }
            }
            currentLumaWeight /= allWeights;

            bloomLumaHistoryOffset++;
            if (bloomLumaHistoryOffset == historyFramesNeeded) lumaHistoryReady = true;
            bloomLumaHistoryOffset %= (historyFramesNeeded * 4);

            FinalCameraEffectsProMaterial.SetVector("lumaHistoryMask", lumaHistoryMask);
            FinalCameraEffectsProMaterial.SetVector("lumaHistoryWeights3", lumaHistoryWeights[0]);
            if (historyFramesNeeded > 1) FinalCameraEffectsProMaterial.SetVector("lumaHistoryWeights2", lumaHistoryWeights[1]);
            if (historyFramesNeeded > 2) FinalCameraEffectsProMaterial.SetVector("lumaHistoryWeights", lumaHistoryWeights[2]);
            FinalCameraEffectsProMaterial.SetFloat("currentLumaWeight", currentLumaWeight);
            FinalCameraEffectsProMaterial.SetInt("lumaHistoryLength", historyFramesNeeded);

            FinalCameraEffectsProMaterial.SetInt("bloomAntiflickerFade", (int)BloomAntiflickerFade);

            
        }

        #endregion

        #region Depth of Field

        private enum DofPassTextureType
        {
            None = 0,
            CocTexture,
            Temp0,
            Temp1,
            Temp2,
            Temp3,
            Temp4,
            Destination
        }

        private enum DofMixingOperationType
        {
            Union = 1,
            Intersection = 2
        }

        private class DofPass
        {
            public DofPass()
            {
                Min = -1.0f;
                Max = 1.0f;
                IsMixingPass = false;
                UseGaussianWeights = false;
            }

            public float Angle { get; set; }
            public float Min { get; set; }
            public float Max { get; set; }
            public DofPassTextureType Source { get; set; }
            public DofPassTextureType Target { get; set; }
            public DofPassTextureType AuxSource { get; set; }
            public bool IsMixingPass { get; set; }
            public bool UseGaussianWeights { get; set; }
            public DofMixingOperationType MixingOperation { get; set; }
        }

        private const int dofTempTexturesCount = 5;
        private ShaderTextureInfo[] dofTempTextures = new ShaderTextureInfo[dofTempTexturesCount];

        private float GetFocalLengthFromVerticalFoV(float verticalFovDegrees, float sensorHeight)
        {
            return (0.5f * sensorHeight) / (Mathf.Tan((verticalFovDegrees * Mathf.Deg2Rad) * 0.5f));
        }

        private bool DofPassesChanged(DofApertureType newApertureType)
        {
            return newApertureType != lastDofApertureType;
        }

        private bool DofSettingsChanged(float dofRotation, float aspectRatio, DofAnamorphicType dofAnamorphic)
        {
            return dofRotation != lastDofRotation ||
                    aspectRatio != lastDofAspectRatio ||
                    dofAnamorphic != lastDofAnamorphic;
        }

        private Vector4 GetSeparableBlurDirectionalVector(float degreesAngle, float min, float max, float aspectRatio, DofAnamorphicType dofAnamorphic)
        {
            float radiansAngle = degreesAngle * Mathf.Deg2Rad;
            Vector4 result = new Vector4(Mathf.Cos(radiansAngle), Mathf.Sin(radiansAngle), 0.0f, 0.0f);

            Vector2 minPoint = new Vector2(min * result.x, min * result.y);
            result.z = minPoint.x;
            result.w = minPoint.y;

            result.x *= (max - min);
            result.y *= (max - min);

            if (dofAnamorphic != DofAnamorphicType.NoStretch)
            {
                if (dofAnamorphic == DofAnamorphicType.StretchHorizontally)
                {
                    // Do nothing
                }
                else if (dofAnamorphic == DofAnamorphicType.StretchVertically)
                {
                    result.x *= aspectRatio;
                    result.z *= aspectRatio;
                    result.x *= aspectRatio;
                    result.z *= aspectRatio;
                }
            }
            else
            {
                result.x *= aspectRatio;
                result.z *= aspectRatio;
            }

            return result;
        }

        private float getCocDiameter(float distance, float apertureDiameter, float viewSpaceDepth, float focusedPlaneDistance, float focalLength)
        {
            return apertureDiameter * (Math.Abs(viewSpaceDepth - focusedPlaneDistance) / viewSpaceDepth) * (focalLength / (focusedPlaneDistance - focalLength));
        }

        private int GetTempDofTextureIndex(DofPassTextureType textureType)
        {
            switch (textureType)
            {
                case DofPassTextureType.Temp0:
                    return 0;
                case DofPassTextureType.Temp1:
                    return 1;
                case DofPassTextureType.Temp2:
                    return 2;
                case DofPassTextureType.Temp3:
                    return 3;
                case DofPassTextureType.Temp4:
                    return 4;
                default:
                    throw new Exception("Invalid parameter " + textureType + " passed to GetTempDofTextureIndex function");
            }
        }

        private ShaderTextureInfo GetOrCreateTempDofRenderTexture(CommandBuffer commandBuffer, DofPassTextureType textureType, int width, int height, RenderTextureFormat screenTextureFormat, ShaderTextureInfo cocTexture, ShaderTextureInfo destinationTexture)
        {
            if (textureType == DofPassTextureType.Temp0 ||
                textureType == DofPassTextureType.Temp1 ||
                textureType == DofPassTextureType.Temp2 ||
                textureType == DofPassTextureType.Temp3 ||
                textureType == DofPassTextureType.Temp4)
            {
                int tempTextureIndex = GetTempDofTextureIndex(textureType);

                if (dofTempTextures[tempTextureIndex] == null)
                {
                    dofTempTextures[tempTextureIndex] = GetTemporaryTexture(commandBuffer, "tempDofTexture" + textureType, width, height, FilterMode.Bilinear, screenTextureFormat);
                }

                return dofTempTextures[tempTextureIndex];
            }

            switch (textureType)
            {
                case DofPassTextureType.CocTexture:
                    return cocTexture;
                case DofPassTextureType.Destination:
                    return destinationTexture;
                case DofPassTextureType.None:
                    return null;
                default:
                    throw new Exception("Invalid parameter " + textureType + " passed to GetOrCreateTempDofRenderTexture function");
            }
        }

        private ShaderPass GetDofShaderPass(int passNumber, bool isMixingPass, bool useGaussianWeights, ref int mixingPassesCount)
        {
            if (isMixingPass)
            {
                mixingPassesCount++;

                if (useGaussianWeights)
                    if (mixingPassesCount == 1) return ShaderPass.DepthOfFieldSeparableGaussianBlurWithMixingPass1;

                if (mixingPassesCount == 1) return ShaderPass.DepthOfFieldSeparableBlurWithMixingPass1;
                if (mixingPassesCount == 2) return ShaderPass.DepthOfFieldSeparableBlurWithMixingPass2;
                throw new Exception("Wilberforce Final Camera Effects Pro depth of field failed - too many mixing passes");
            }

            switch (passNumber)
            {
                case 0:
                    if (useGaussianWeights)
                        return ShaderPass.DepthOfFieldSeparableGaussianBlurPass0;

                    return ShaderPass.DepthOfFieldSeparableBlurPass0;
                case 1:
                    return ShaderPass.DepthOfFieldSeparableBlurPass1;
                case 2:
                    return ShaderPass.DepthOfFieldSeparableBlurPass2;
                case 3:
                    return ShaderPass.DepthOfFieldSeparableBlurPass3;
                case 4:
                    return ShaderPass.DepthOfFieldSeparableBlurPass4;
                case 5:
                    return ShaderPass.DepthOfFieldSeparableBlurPass5;
                default:
                    throw new Exception("Invalid parameter " + passNumber + " passed to getDofShaderPass function");
            }
        }

        private void ExecuteDofImageEffectBlits(CommandBuffer commandBuffer, DofPass[] dofPasses, int width, int height, RenderTextureFormat screenTextureFormat, ShaderTextureInfo cocTexture, ShaderTextureInfo destinationTexture)
        {
            if (dofPasses != null)
            {
                int mixingPassesCount = 0;
                int nextBokehAuxTextureNum = 1;

                FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 2);

                for (int i = 0; i < dofPasses.Length; i++)
                {
                    DofPass dofPass = dofPasses[i];

                    ShaderTextureInfo sourceTexture = GetOrCreateTempDofRenderTexture(commandBuffer, dofPass.Source, width, height, screenTextureFormat, cocTexture, destinationTexture);
                    ShaderTextureInfo targetTexture = GetOrCreateTempDofRenderTexture(commandBuffer, dofPass.Target, width, height, screenTextureFormat, cocTexture, destinationTexture);
                    ShaderTextureInfo auxTexture = GetOrCreateTempDofRenderTexture(commandBuffer, dofPass.AuxSource, width, height, screenTextureFormat, cocTexture, destinationTexture);

                    if (dofPass.IsMixingPass)
                    {
                        SetShaderTexture(commandBuffer, "bokehAuxTexture" + nextBokehAuxTextureNum, auxTexture, FinalCameraEffectsProMaterial);
                        nextBokehAuxTextureNum++;
                    }

                    ShaderPass dofShaderPassToUse = GetDofShaderPass(i, dofPass.IsMixingPass, dofPass.UseGaussianWeights, ref mixingPassesCount);
                    
                    DoShaderBlit(commandBuffer, sourceTexture, targetTexture, FinalCameraEffectsProMaterial, (int)dofShaderPassToUse);
                }

                FinalCameraEffectsProMaterial.SetInt("useSPSRFriendlyTransform", 0);
            }

            // Release textures
            for (int i = 0; i < dofTempTexturesCount; i++)
            {
                if (dofTempTextures[i] != null)
                {
                    ReleaseTempTexture(commandBuffer, ref dofTempTextures[i]);
                    dofTempTextures[i] = null;
                }
            }
        }

        private DofPass[] GetAperturePasses(DofApertureType apertureType)
        {
            switch (apertureType)
            {
                case DofApertureType.Circular:
                    return circularAperturePasses;
                case DofApertureType.Rectangular:
                    return rectangularAperturePasses;
                case DofApertureType.Pentagonal:
                    return pentagonalAperturePasses;
                case DofApertureType.Hexagonal:
                    return hexagonalAperturePasses;
                case DofApertureType.Octagonal:
                    return octagonalAperturePasses;
                default:
                    return hexagonalAperturePasses;
            }
        }

#region Aperture Types Data

        private DofPass[] circularAperturePasses = new DofPass[] {

            new DofPass()
            {
                Angle = 0.0f,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp0,
                AuxSource = DofPassTextureType.None,
                UseGaussianWeights = true,
            },

            new DofPass()
            {
                Angle = 90.0f,
                Source = DofPassTextureType.Temp0,
                Target = DofPassTextureType.Destination,
                AuxSource = DofPassTextureType.None,
                IsMixingPass = true,
                UseGaussianWeights = true
            }
        };

        private DofPass[] rectangularAperturePasses = new DofPass[] {

            new DofPass()
            {
                Angle = 0.0f,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp0,
                AuxSource = DofPassTextureType.None,
            },

            new DofPass()
            {
                Angle = 90.0f,
                Source = DofPassTextureType.Temp0,
                Target = DofPassTextureType.Destination,
                AuxSource = DofPassTextureType.None,
                IsMixingPass = true
            }
        };

        private static float pentagon_x = 1.175570504585f;
        private static float pentagon_c = Mathf.Cos(36.0f * Mathf.Deg2Rad) * pentagon_x;
        private static float pentagon_d = (pentagon_x * 0.5f) / Mathf.Cos(54.0f * Mathf.Deg2Rad);
        private static float pentagon_b = pentagon_d * Mathf.Sin(18.0f * Mathf.Deg2Rad);
        private static float pentagon_e = (2.0f * pentagon_c) * Mathf.Cos(18.0f * Mathf.Deg2Rad);
        private static float pentagon_a = pentagon_x * 0.5f * Mathf.Tan(54.0f * Mathf.Deg2Rad);

        private DofPass[] pentagonalAperturePasses = new DofPass[] {

            new DofPass()
            {
                Angle = 36.0f,
                Min = -pentagon_b,
                Max = pentagon_a,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp0,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = 144.0f,
                Min = -pentagon_b,
                Max = pentagon_a,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp1,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = 108.0f,
                Min = -pentagon_c,
                Max = pentagon_c,
                Source = DofPassTextureType.Temp0,
                Target = DofPassTextureType.Temp2,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = 72.0f,
                Min = -pentagon_c,
                Max = pentagon_c,
                Source = DofPassTextureType.Temp1,
                Target = DofPassTextureType.Temp3,
                AuxSource = DofPassTextureType.Temp2,
                MixingOperation = DofMixingOperationType.Union,
                IsMixingPass = true
            },

            new DofPass()
            {
                Angle = 90.0f,
                Min = -pentagon_d * 1.15f,
                Max = pentagon_e - pentagon_d * 1.15f,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp0,
            },

            new DofPass()
            {
                Angle = 0.0f,
                Min = -pentagon_c,
                Max = pentagon_c,
                Source = DofPassTextureType.Temp0,
                Target = DofPassTextureType.Destination,
                AuxSource = DofPassTextureType.Temp3,
                MixingOperation = DofMixingOperationType.Intersection,
                IsMixingPass = true
            }
        };

        private DofPass[] hexagonalAperturePasses = new DofPass[] {

            new DofPass()
            {
                Angle = 0.0f,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp0,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = 60.0f,
                Source = DofPassTextureType.Temp0,
                Target = DofPassTextureType.Temp1,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = 120.0f,
                Source = DofPassTextureType.Temp0,
                Target = DofPassTextureType.Destination,
                AuxSource = DofPassTextureType.Temp1,
                MixingOperation = DofMixingOperationType.Intersection,
                IsMixingPass = true
            }
        };

        private DofPass[] octagonalAperturePasses = new DofPass[] {

            new DofPass()
            {
                Angle = 0.0f,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp0,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = 45.0f,
                Source = DofPassTextureType.CocTexture,
                Target = DofPassTextureType.Temp1,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = 90.0f,
                Source = DofPassTextureType.Temp0,
                Target = DofPassTextureType.Temp2,
                AuxSource = DofPassTextureType.None
            },

            new DofPass()
            {
                Angle = -45.0f,
                Source = DofPassTextureType.Temp1,
                Target = DofPassTextureType.Destination,
                AuxSource = DofPassTextureType.Temp2,
                MixingOperation = DofMixingOperationType.Intersection,
                IsMixingPass = true
            }
        };

#endregion

#endregion

#region Distortion

        private Vector2 Distort(Vector2 input, float aspectRatio, bool isAnamorphic, float distortionPower)
        {
            input.y = input.y * aspectRatio;

            float distanceFromCenter = input.magnitude;
            float distortedDistanceFromCenter = Mathf.Pow(distanceFromCenter, distortionPower);

            input = input.normalized * distortedDistanceFromCenter;

            input.y = input.y / aspectRatio;

            return input;
        }

        private Vector2 GetDistortionMaxUV(float aspectRatio, bool isAnamorphic, float distortionPower)
        {
            if (distortionPower < 1.0) aspectRatio = 1.0f / aspectRatio;

            return new Vector2(Distort(new Vector2(1.0f, 0.0f), aspectRatio, isAnamorphic, distortionPower).x,
                               Distort(new Vector2(0.0f, 1.0f), aspectRatio, isAnamorphic, distortionPower).y);
        }

#endregion


#endregion
        
    }

#if UNITY_EDITOR


    [CustomEditor(typeof(FinalCameraEffectsProCommandBuffer))]
    public class FinalCameraEffectsProEditorCmdBuffer : FinalCameraEffectsProEditor { }

    public class FinalCameraEffectsProEditor : Editor
    {

#region Labels

        private readonly GUIContent pipelineLabelContent = new GUIContent("Rendering Pipeline Settings:", "");
        private readonly GUIContent integrationLabelContent = new GUIContent("Integration Type:", "Way of integration of effect within Unity.");
        private readonly GUIContent gBufferLabelContent = new GUIContent("G-Buffer Depth&Normals:", "Take depth&normals from GBuffer of deferred rendering path, use this to overcome compatibility issues or for better precision");
        private readonly GUIContent dedicatedDepthBufferLabelContent = new GUIContent("High precision depth buffer:", "Uses higher precision depth buffer (forward path only). This may also fix some materials that work normally in deferred path.");
        private readonly GUIContent farPlaneSourceLabelContent = new GUIContent("Far plane source:", "Where to take far plane distance from. Camera is needed for post-processing stack temporal AA compatibility. Use Projection Params option for compatibility with other effects.");

        private readonly GUIContent anamorphicLensLabel = new GUIContent("Anamorphic Lens (Aspect Ratio):", "Turning on will simulate effect of anamorphic lens.");

        private readonly GUIContent distortionLabel = new GUIContent("", "Type and amount of lens distortion (pincushion/barrel).");
        private readonly GUIContent chromaWeightLabel = new GUIContent("Intensity: ", "Intensity of chromatic aberrations.");
        private readonly GUIContent chromaSizeLabel = new GUIContent("Size: ", "Size of chromatic aberrations.");
        private readonly GUIContent chromaRadialnessLabel = new GUIContent("Linearity: ", "Size based on distance from backgroundImage center (makes aberrations larger in corners).");
        
        private readonly GUIContent dofAlgorithmTypeLabel = new GUIContent("Algorithm: ", "Choice of faster/more precise algorithm. Faster version uses downsampling and additional blur step to achieve higher speed.");
        private readonly GUIContent focusDistanceLabel = new GUIContent("Focus Distance: ", "Distance of focal plane from camera - distance in which sharpest focus is achieved.");
        private readonly GUIContent apertureDiameterLabel = new GUIContent("Aperture Diameter: ", "Diameter of aperture opening - larger will create more defocused foreground and background. Only available for realistic thin lens model - disabled when 'from parameters' setting used!");
        private readonly GUIContent apertureShapeLabel = new GUIContent("Aperture Shape: ", "Shape (number of blades) of the aperture opening.");
        private readonly GUIContent apertureRotationLabel = new GUIContent("Aperture Rotation: ", "Rotation angle of the aperture.");
        private readonly GUIContent dofRangeLabel = new GUIContent("Defocus Areas: ", "Limit defocused areas to background/foreground or both based on focal plane distance.");
        private readonly GUIContent dofSoftnessLabel = new GUIContent("Softness: ", "Lower value makes out of focus areas appear sharper - especially bright highlights in HDR. (Only available for non-circular aperture shapes!)");

        private readonly GUIContent lensModelLabelContent = new GUIContent("Lens Parameters:", "");
        private readonly GUIContent cocModelLabelContent = new GUIContent("Lens Model: ", "Thin lens model is realistic and lets you set parameters as for physical lens. 'From Parameters' option lets you set distances for maximum defocus from focal plane and curve shape.");
        private readonly GUIContent sensorHeightLabelContent = new GUIContent("Sensor Height (mm): ", "Height of camera sensor/film frame in millimeters.");
        private readonly GUIContent focalLengthLabel = new GUIContent("Focal Length (mm): ", "Focal length of lens in millimeters.");
        private readonly GUIContent focalLengthSourceLabelContent = new GUIContent("Focal Length Source: ", "Calculate focal length from Unity's camera vertical field of view (FOV) or set manually.");

        private readonly GUIContent cocParametersUnitsLabelContent = new GUIContent("Distance Parameters Mode: ", "Following parameters are either in absolute distances from camera or relative to focal plane.");

        private readonly GUIContent defocusSizeLabelContent = new GUIContent("Defocus Size: ", "Size of defocused areas on the screen.");
        private readonly GUIContent defocusSizeDistanceLabelContent = new GUIContent("Max Defocus Distance: ", "Distance in which maximum defocus size is achieved.");
        private readonly GUIContent defocusCurveLienarityLabelContent = new GUIContent("Curve Linearity: ", "Controls shape of the curve for defocus size based on distance from focal plane.");

        private readonly GUIContent vignetteFalloffLinearityLabelContent = new GUIContent("Falloff Linearity: ", "Falloff curve of vignette intensity (based on distance from backgroundImage center).");
        private readonly GUIContent vignetteMinMaxLabelContent = new GUIContent("Min/Max Intensity: ", "Maximum brightness value (in the center), minimum brightness value (in the corners).");
        private readonly GUIContent vignetteMinMaxDistanceLabelContent = new GUIContent("Min/Max Distance: ", "Distance from center where minimum/maximum intensity is achieved.");
        private readonly GUIContent vignetteModeLabelContent = new GUIContent("Mode: ", "Vignetting mode - Standard mode adjusts brightness, Saturation mode adjusts color saturation.");
        private readonly GUIContent vignetteInnerSaturationLabelContent = new GUIContent("Center Saturation: ", "Color saturation in the center.");
        private readonly GUIContent vignetteOuterSaturationLabelContent = new GUIContent("Corners Saturation: ", "Color saturation in corners.");
        private readonly GUIContent vignetteInnerColorLabelContent = new GUIContent("Center Color: ", "Color in the center.");
        private readonly GUIContent vignetteOuterColorLabelContent = new GUIContent("Corners Color: ", "Color in corners.");
        private readonly GUIContent vignetteCenterLabelContent = new GUIContent("Center Position: ", "Position of vignetting center on the screen.");

        private readonly GUIContent bloomQualityLabelContent = new GUIContent("Quality: ", "Quality of bloom effect (lower improves performance).");
        private readonly GUIContent bloomRadiusLabelContent = new GUIContent("Radius: ", "Size of bloom effect (How far away will bloom reach from its source).");
        private readonly GUIContent bloomThresholdLabelContent = new GUIContent("Threshold: ", "Brightness threshold - areas with higher brightness will bloom.");
        private readonly GUIContent bloomSaturationLabelContent = new GUIContent("Saturation: ", "Color saturation of bloom effect.");
        private readonly GUIContent bloomMipLevelsLabelContent = new GUIContent("Mip Levels: ", "Number of mip levels used (selected automatically based on radius setting).");
        private readonly GUIContent bloomAntiflickerLabelContent = new GUIContent("Antiflicker: ", "Reduces flickering of moving objects by using temporal filtering of luminance.");
        private readonly GUIContent bloomSoftLabelContent = new GUIContent("Soft: ", "Strength of the bloom effect - Exponentiation part creates 'soft' bloom making it stronger near the source.");
        private readonly GUIContent bloomStrongLabelContent = new GUIContent("Strong: ", "Strength of the bloom effect - Multiplication part creates 'strong' bloom making it stronger overall.");
        private readonly GUIContent antiflickerStrengthLabelContent = new GUIContent("Strength: ", "Higher value puts more emphasis on more recent frames, causing less 'motion blur' but weakens antiflicker effect.");
        private readonly GUIContent antiflickerLengthLabelContent = new GUIContent("Length: ", "Number of previous frames to consider for antiflicker filter (4/8/12).");
        private readonly GUIContent antiflickerFadeLabelContent = new GUIContent("Fade: ", "Fade in = flickering areas appear slowly and disappear instantly. Fade out = appear instantly and disappear slowly.");

        private readonly GUIContent colorCorrectionModeLabelContent = new GUIContent("Color Grading Mode: ", "Color correction method to use (either custom LUT texture or manual setting using RGB curves).");
        private readonly GUIContent colorCorrectionIntensityLabelContent = new GUIContent("Intensity: ", "Blends between color corrected and original backgroundImage.");
        private readonly GUIContent colorCorrectionLutTextureLabelContent = new GUIContent("LUT Texture: ", "LUT Texture to use for color correction.");

        private readonly GUIContent exposureAdjustmentLabelContent = new GUIContent("Exposure: ", "Exposure compensation before tone mapping.");
        private readonly GUIContent tonemappingModeLabelContent = new GUIContent("HDR Tonemapping: ", "ACES (Academy Color Encoding System) Tonemapping toggle (maps HDR values to standard range for display).");
        private readonly GUIContent tonemappingIntensityLabelContent = new GUIContent("Intensity: ", "Tonemapping curve slope - lower value means linear mapping, higher value is ACES standard curve.");
        private readonly GUIContent tonemappingSaturationLabelContent = new GUIContent("Saturation: ", "Saturation adjustment after tone mapping.");
        private readonly GUIContent tonemappingContrastLabelContent = new GUIContent("Contrast: ", "Contrast adjustment after tone mapping.");
        private readonly GUIContent tonemappingGammaLabelContent = new GUIContent("Gamma: ", "Gamma adjustment after tone mapping.");
        private readonly GUIContent tonemappingColorTintLabelContent = new GUIContent("Tint: ", "Color tint adjustment before tone mapping (purple - green range).");
        private readonly GUIContent tonemappingColorTemperatureLabelContent = new GUIContent("Temperature: ", "Color temperature adjustment before tone mapping (blue - yellow range).");
        
        #endregion

        #region Previous Settings Cache

        private float lumaMaxFx = 3.0f;
        private float lastDofLumaMaxFx;
        private float lastDofLumaThreshold;
        private float lastDofLumaKneeWidth;
        private float lastDofLumaKneeLinearity;

        private FinalCameraEffectsPro.DofCoCFromParametersModeType lastDofCoCFromParametersMode;
        private float lastFocalPlaneDistance;
        private float lastDofForegroundCoCDistance;
        private float lastDofBackgroundCoCDistance;
        private float lastDofForegroundCoCDiameter;
        private float lastDofBackgroundCoCDiameter;
        private FinalCameraEffectsPro.DofCoCModelType lastDofCoCModel;
        private float lastNearPlane;
        private float lastFarPlane;
        private float lastApertureDiameter;
        private float lastSensorHeight;
        private float lastFocalLength;
        private float lastDofBackgroundCoCLinearity;
        private float lastDofForegroundCoCLinearity;
        private FinalCameraEffectsPro.FocalLengthSourceType lastFocalLengthSource;

        private float lastVignetteInnerValueDistance;
        private float lastVignetteOuterValueDistance;
        private float lastVignetteFalloff;
        private float lastVignetteOuterValue;
        private float lastVignetteInnerValue;
        private float lastVignetteInnerSaturation;
        private float lastVignetteOuterSaturation;
        private FinalCameraEffectsPro.VignetteModeType lastVignetteMode;

#endregion


#region Custom header styles

        // Custom header foldout styles
        private static GUIStyle _customFoldoutStyle;
        private static GUIStyle customFoldoutStyle
        {
            get
            {
                if (_customFoldoutStyle == null)
                {
                    _customFoldoutStyle = new GUIStyle(EditorStyles.foldout)
                    {
                        fixedWidth = 12.0f
                    };
                }

                return _customFoldoutStyle;
            }
        }

        private static GUIStyle _customFoldinStyle;
        private static GUIStyle customFoldinStyle
        {
            get
            {
                if (_customFoldinStyle == null)
                {
                    _customFoldinStyle = new GUIStyle(EditorStyles.foldout)
                    {
                        fixedWidth = 12.0f
                    };

                    _customFoldinStyle.normal = _customFoldinStyle.onNormal;
                }

                return _customFoldinStyle;
            }
        }

        private static GUIStyle _customToggleStyle;
        private static GUIStyle customToggleStyle
        {
            get
            {
                if (_customToggleStyle == null)
                {
                    _customToggleStyle = new GUIStyle(EditorStyles.toggle)
                    {
                        fixedWidth = 12.0f
                    };
                }

                return _customToggleStyle;
            }
        }

        private static GUIStyle _customIconStyle;
        private static GUIStyle customIconStyle
        {
            get
            {
                if (_customIconStyle == null)
                {
                    _customIconStyle = new GUIStyle(EditorStyles.label)
                    {
                        fixedWidth = 18.0f,
                        fixedHeight = 18.0f
                    };
                }

                return _customIconStyle;
            }
        }


        private static GUIStyle _customLabelStyle;
        private static GUIStyle customLabelStyle
        {
            get
            {
                if (_customLabelStyle == null)
                {
                    _customLabelStyle = new GUIStyle(EditorStyles.label)
                    {
                        fontStyle = FontStyle.Bold
                    };
                }

                return _customLabelStyle;
            }
        }

        private Texture2D bloomIcon;
        private Texture2D vignetteIcon;
        private Texture2D distortionIcon;
        private Texture2D barrelIcon;
        private Texture2D chromaAbrrIcon;
        private Texture2D bokehIcon;
        private Texture2D filmIcon;

#endregion

#region Graph Widgets Params

        // Luma graph sidget

        private GraphWidget dofLumaGraphWidget;
        private GraphWidgetDrawingParameters lumaGraphWidgetParams;
      
        private GraphWidgetDrawingParameters GetDofLumaGraphWidgetParameters(FinalCameraEffectsProCommandBuffer script)
        {
            if (lumaGraphWidgetParams != null &&
                lastDofLumaThreshold == script.DofLumaThreshold &&
                lastDofLumaKneeWidth == script.DofLumaKneeWidth &&
                lastDofLumaMaxFx == lumaMaxFx &&
                lastDofLumaKneeLinearity == script.DofLumaKneeLinearity) return lumaGraphWidgetParams;

            lastDofLumaThreshold = script.DofLumaThreshold;
            lastDofLumaKneeWidth = script.DofLumaKneeWidth;
            lastDofLumaKneeLinearity = script.DofLumaKneeLinearity;
            lastDofLumaMaxFx = lumaMaxFx;

            lumaGraphWidgetParams = new GraphWidgetDrawingParameters()
            {
                GraphSegmentsCount = 128,
                GraphColor = Color.white,
                GraphThickness = 2.0f,
                GraphFunction = ((float x) =>
                {
                    float Y = (x - (script.DofLumaThreshold - script.DofLumaKneeWidth)) * (1.0f / (2.0f * script.DofLumaKneeWidth));
                    x = Mathf.Min(1.0f, Mathf.Max(0.0f, Y));
                    return ((-Mathf.Pow(x, script.DofLumaKneeLinearity) + 1));
                }),
                YScale = 0.65f,
                MinY = 0.1f,
                MaxFx = lumaMaxFx,
                GridLinesXCount = 4,
                LabelText = "Luminance sensitivity curve",
                Lines = new List<GraphWidgetLine>()
                {
                    new GraphWidgetLine() {
                        Color = Color.red,
                        Thickness = 2.0f,
                        From = new Vector3(script.DofLumaThreshold / lumaMaxFx, 0.0f, 0.0f),
                        To = new Vector3(script.DofLumaThreshold / lumaMaxFx, 1.0f, 0.0f)
                    },
                    new GraphWidgetLine() {
                        Color = Color.blue * 0.7f,
                        Thickness = 2.0f,
                        From = new Vector3((script.DofLumaThreshold - script.DofLumaKneeWidth) / lumaMaxFx, 0.0f, 0.0f),
                        To = new Vector3((script.DofLumaThreshold - script.DofLumaKneeWidth) / lumaMaxFx, 1.0f, 0.0f)
                    },
                    new GraphWidgetLine() {
                        Color = Color.blue * 0.7f,
                        Thickness = 2.0f,
                        From = new Vector3((script.DofLumaThreshold + script.DofLumaKneeWidth) / lumaMaxFx, 0.0f, 0.0f),
                        To = new Vector3((script.DofLumaThreshold + script.DofLumaKneeWidth) / lumaMaxFx, 1.0f, 0.0f)
                    }
                }
            };

            return lumaGraphWidgetParams;
        }

        // CoC graph widget

        private GraphWidget dofCocGraphWidget;
        private GraphWidgetDrawingParameters cocGraphWidgetParams;

        private GraphWidgetDrawingParameters GetDofCocGraphWidgetParameters(FinalCameraEffectsProCommandBuffer script)
        {
            if (cocGraphWidgetParams != null &&
                lastDofCoCFromParametersMode == script.DofCoCFromParametersMode &&
                lastFocalPlaneDistance == script.FocalPlaneDistance &&
                lastDofForegroundCoCDistance == script.DofForegroundCoCDistance &&
                lastDofBackgroundCoCDistance == script.DofBackgroundCoCDistance &&
                lastDofForegroundCoCDiameter == script.DofForegroundCoCDiameter &&
                lastDofBackgroundCoCDiameter == script.DofBackgroundCoCDiameter &&
                lastDofCoCModel == script.DofCoCModel &&
                lastNearPlane == camera.nearClipPlane &&
                lastApertureDiameter == script.ApertureDiameter &&
                lastSensorHeight == script.SensorHeight &&
                lastFocalLength == script.FocalLength &&
                lastDofBackgroundCoCLinearity == script.DofBackgroundCoCLinearity &&
                lastDofForegroundCoCLinearity == script.DofForegroundCoCLinearity &&
                lastFocalLengthSource == script.FocalLengthSource &&
                lastFarPlane == camera.farClipPlane) return cocGraphWidgetParams;

            lastDofCoCFromParametersMode = script.DofCoCFromParametersMode;
            lastDofCoCModel = script.DofCoCModel;
            lastFocalPlaneDistance = script.FocalPlaneDistance;
            lastDofForegroundCoCDistance = script.DofForegroundCoCDistance;
            lastDofBackgroundCoCDistance = script.DofBackgroundCoCDistance;
            lastDofForegroundCoCDiameter = script.DofForegroundCoCDiameter;
            lastDofBackgroundCoCDiameter = script.DofBackgroundCoCDiameter;
            lastDofForegroundCoCLinearity = script.DofForegroundCoCLinearity;
            lastDofBackgroundCoCLinearity = script.DofBackgroundCoCLinearity;
            lastNearPlane = camera.nearClipPlane;
            lastFarPlane = camera.farClipPlane;
            lastApertureDiameter = script.ApertureDiameter;
            lastSensorHeight = script.SensorHeight;
            lastFocalLength = script.FocalLength;
            lastFocalLengthSource = script.FocalLengthSource;

            float dofForegroundCoCDistance = script.DofForegroundCoCDistance;
            float dofBackgroundCoCDistance = script.DofBackgroundCoCDistance;

            float focalLengthToUse = lastFocalLength * 0.001f;

            if (script.FocalLengthSource == FinalCameraEffectsPro.FocalLengthSourceType.CameraFOV)
                focalLengthToUse = GetFocalLengthFromVerticalFoV(camera.fieldOfView, script.SensorHeight * 0.001f);

            if (script.DofCoCFromParametersMode == FinalCameraEffectsPro.DofCoCFromParametersModeType.RelativeToFocalPlane)
            {
                dofForegroundCoCDistance = script.FocalPlaneDistance - script.DofForegroundCoCDistance;
                dofBackgroundCoCDistance = script.FocalPlaneDistance + script.DofBackgroundCoCDistance;
            }

            const float maxCoC = 0.1f; //< Ensure same as in shader
            float focalPlaneLinePos = (script.FocalPlaneDistance - lastNearPlane) / (lastFarPlane - lastNearPlane);

            cocGraphWidgetParams = new GraphWidgetDrawingParameters()
            {
                GraphSegmentsCount = 128,
                GraphColor = Color.white,
                GraphThickness = 2.0f,
                GraphFunction = ((float viewSpaceDepth) =>
                {
                    viewSpaceDepth += lastNearPlane;
                    viewSpaceDepth = -viewSpaceDepth;

                    float cocDiameter;

                    if (lastDofCoCModel == FinalCameraEffectsPro.DofCoCModelType.FromParameters)
                    {
                        if (viewSpaceDepth > -script.FocalPlaneDistance)
                        {
                            // foreground
                            float u = Mathf.Clamp01((-script.FocalPlaneDistance - viewSpaceDepth) / (-script.FocalPlaneDistance + dofForegroundCoCDistance));

                            if (script.DofForegroundCoCLinearity > 0.0f)
                                u = Mathf.Lerp(u, Mathf.Pow(u, 5.0f), script.DofForegroundCoCLinearity);
                            else
                                u = Mathf.Lerp(u, 1.0f - Mathf.Pow(1.0f - u, 5.0f), -script.DofForegroundCoCLinearity);

                            cocDiameter = Mathf.Lerp(0.0f, script.DofForegroundCoCDiameter * maxCoC, u);
                        }
                        else
                        {
                            // background
                            float u = Mathf.Clamp01((viewSpaceDepth - -script.FocalPlaneDistance) / (-dofBackgroundCoCDistance + script.FocalPlaneDistance));

                            if (script.DofBackgroundCoCLinearity > 0.0f)
                                u = Mathf.Lerp(u, Mathf.Pow(u, 5.0f), script.DofBackgroundCoCLinearity);
                            else
                                u = Mathf.Lerp(u, 1.0f - Mathf.Pow(1.0f - u, 5.0f), -script.DofBackgroundCoCLinearity);

                            cocDiameter = Mathf.Lerp(0.0f, script.DofBackgroundCoCDiameter * maxCoC, u);
                        }
                    }
                    else
                    {
                        cocDiameter = (lastApertureDiameter * 0.001f) * (Mathf.Abs(viewSpaceDepth + script.FocalPlaneDistance) / viewSpaceDepth) * (focalLengthToUse / (-script.FocalPlaneDistance - focalLengthToUse));
                        cocDiameter = cocDiameter / (lastSensorHeight * 0.001f);
                    }
                    cocDiameter = Mathf.Max(0.0f, Mathf.Min(cocDiameter, maxCoC)) * (1.0f / maxCoC);

                    return cocDiameter;
                }),
                YScale = 0.65f,
                MinY = 0.1f,
                MaxFx = (lastFarPlane - lastNearPlane),
                GridLinesXCount = 4,
                LabelText = "Depth of field curve",
                Lines = new List<GraphWidgetLine>()
                {
                    new GraphWidgetLine() {
                        Color = Color.red,
                        Thickness = 2.0f,
                        From = new Vector3(focalPlaneLinePos, 0.0f, 0.0f),
                        To = new Vector3(focalPlaneLinePos, 1.0f, 0.0f)
                    }
                }
            };

            if (lastDofCoCModel != FinalCameraEffectsPro.DofCoCModelType.RealisticThinLensModel)
            {
                float foregroundLinePos = (dofForegroundCoCDistance - lastNearPlane) / (lastFarPlane - lastNearPlane);
                float backgroundLinePos = (dofBackgroundCoCDistance - lastNearPlane) / (lastFarPlane - lastNearPlane);

                cocGraphWidgetParams.Lines.Add(new GraphWidgetLine()
                {
                    Color = Color.blue * 0.7f,
                    Thickness = 2.0f,
                    From = new Vector3(foregroundLinePos, 0.0f, 0.0f),
                    To = new Vector3(foregroundLinePos, 1.0f, 0.0f)
                });

                cocGraphWidgetParams.Lines.Add(new GraphWidgetLine()
                {
                    Color = Color.blue * 0.7f,
                    Thickness = 2.0f,
                    From = new Vector3(backgroundLinePos, 0.0f, 0.0f),
                    To = new Vector3(backgroundLinePos, 1.0f, 0.0f)
                });
            }

            return cocGraphWidgetParams;
        }

        // Vignette graph widget

        private GraphWidget vignetteGraphWidget;
        private GraphWidgetDrawingParameters vignetteWidgetParams;
     
        private GraphWidgetDrawingParameters GetVignetteGraphWidgetParameters(FinalCameraEffectsProCommandBuffer script)
        {
            if (vignetteWidgetParams != null &&
                lastVignetteFalloff == script.VignetteFalloff &&
                lastVignetteOuterValueDistance == script.VignetteOuterValueDistance &&
                lastVignetteOuterValue == script.VignetteOuterValue &&
                lastVignetteInnerValue == script.VignetteInnerValue &&
                lastVignetteMode == script.VignetteMode &&
                lastVignetteInnerSaturation == script.VignetteInnerSaturation &&
                lastVignetteOuterSaturation == script.VignetteOuterSaturation &&
                lastVignetteInnerValueDistance == script.VignetteInnerValueDistance) return vignetteWidgetParams;

            lastVignetteInnerValueDistance = script.VignetteInnerValueDistance;
            lastVignetteOuterValueDistance = script.VignetteOuterValueDistance;
            lastVignetteFalloff = script.VignetteFalloff;
            lastVignetteOuterValue = script.VignetteOuterValue;
            lastVignetteInnerValue = script.VignetteInnerValue;
            lastVignetteMode = script.VignetteMode;
            lastVignetteInnerSaturation = script.VignetteInnerSaturation;
            lastVignetteOuterSaturation = script.VignetteOuterSaturation;

            vignetteWidgetParams = new GraphWidgetDrawingParameters()
            {
                GraphSegmentsCount = 128,
                GraphColor = Color.white,
                GraphThickness = 2.0f,
                GraphFunction = ((float distance) =>
                {
                    float u = Mathf.Clamp01((distance - script.VignetteInnerValueDistance) / (script.VignetteOuterValueDistance - script.VignetteInnerValueDistance));
                    u = Mathf.Clamp01(Mathf.Pow(u, script.VignetteFalloff));

                    if (script.VignetteMode == FinalCameraEffectsPro.VignetteModeType.Standard)
                    {
                        u = Mathf.Lerp(script.VignetteOuterValue, script.VignetteInnerValue, u);
                    }
                    else if (script.VignetteMode == FinalCameraEffectsPro.VignetteModeType.Saturation)
                    {
                        u = Mathf.Lerp(script.VignetteInnerSaturation, script.VignetteOuterSaturation, u);
                    }

                    return u;
                }),
                //GraphFunction = ((float x) =>
                //{
                //    int size = 33;
                //    var gauss = FinalLens.GenerateGaussian(size, script.BloomGaussianStrength);
                //    int idx = (int)Math.Floor(x * (size - 1));
                //    float max = gauss[size / 2].x;
                //    float a = gauss[idx].x / max;
                //    float b;
                //    if (idx == size - 1) b = a;
                //    else b = gauss[idx + 1].x / max;

                //    float range = 1.0f / (size - 1);
                //    float idxX = range * (float)idx;

                //    return Mathf.Lerp(a, b,(x - idxX) / range);
                //}),
                YScale = 0.65f,
                //YScale = 1.0f,
                MinY = 0.1f,
                //MinY = 0.0f,
                MaxFx = 1.5f,
                //MaxFx = 1.0f,
                GridLinesXCount = 4,
                LabelText = "Vignette intensity",
            };

            return vignetteWidgetParams;
        }


#endregion

#region Effect Implementation Utilities

        private float GetFocalLengthFromVerticalFoV(float verticalFovDegrees, float sensorHeight)
        {
            return (0.5f * sensorHeight) / (Mathf.Tan((verticalFovDegrees * Mathf.Deg2Rad) * 0.5f));
        }

#endregion

#region Unity Utilities

        private Camera camera;

        private void SetIcon()
        {
            try
            {
                Texture2D icon = (Texture2D)Resources.Load("wilberforce_script_icon");
                Type editorGUIUtilityType = typeof(UnityEditor.EditorGUIUtility);
                System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
                object[] args = new object[] { target, icon };
                editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
            }
            catch (Exception ex)
            {
                if (Debug.isDebugBuild) Debug.Log("Wilberforce Final Camera Effects Pro Effect Error: There was an exception while setting icon to Wilberforce Final Camera Effects Pro script: " + ex.Message);
            }
        }

        void OnEnable()
        {
            if (dofLumaGraphWidget == null) dofLumaGraphWidget = new GraphWidget();
            if (dofCocGraphWidget == null) dofCocGraphWidget = new GraphWidget();
            if (vignetteGraphWidget == null) vignetteGraphWidget = new GraphWidget();

            FinalCameraEffectsProCommandBuffer effect = (target as FinalCameraEffectsProCommandBuffer);
            camera = effect.GetComponent<Camera>();

            SetIcon();

            // Load effect icons
            try
            {
                if (bloomIcon == null) bloomIcon = (Texture2D)Resources.Load("wilberforce_bloom");
                if (vignetteIcon == null) vignetteIcon = (Texture2D)Resources.Load("wilberforce_vignette");
                if (filmIcon == null) filmIcon = (Texture2D)Resources.Load("wilberforce_film");
                if (bokehIcon == null) bokehIcon = (Texture2D)Resources.Load("wilberforce_bokeh");
                if (chromaAbrrIcon == null) chromaAbrrIcon = (Texture2D)Resources.Load("wilberforce_chromaAbrr");
                if (distortionIcon == null) distortionIcon = (Texture2D)Resources.Load("wilberforce_distortion");
                if (barrelIcon == null) barrelIcon = (Texture2D)Resources.Load("wilberforce_barrel");
            }
            catch (Exception ex)
            {
                if (Debug.isDebugBuild) Debug.Log("Wilberforce Final Camera Effects Pro Effect Error: There was an exception while loading icons for Wilberforce Final Camera Effects Pro script: " + ex.Message);
            }
        }

        private bool isCameraHDR(Camera camera)
        {

#if UNITY_5_6_OR_NEWER
            if (camera != null) return camera.allowHDR;
#else
            if (camera != null) return camera.hdr;
#endif
            return false;
        }

        void DisplayGBufferStateMessage(FinalCameraEffectsProCommandBuffer script)
        {

            if (!script.UseGBuffer && script.ShouldUseGBuffer())
            {
                string reason = "";

#if UNITY_2017_1_OR_NEWER
                if (camera != null && camera.stereoEnabled
                    && PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass
                    && camera.actualRenderingPath == RenderingPath.DeferredShading)
                {
                    reason = " You are running in single pass stereo mode which requires G-Buffer inputs.";
                }
#endif
                EditorGUILayout.HelpBox("Cannot turn G-Buffer depth&normals off, because current configuration requires it to be enabled." + reason, MessageType.Warning);
            }

        }

        override public void OnInspectorGUI()
        {
            var script = target as FinalCameraEffectsProCommandBuffer;

            EditorGUILayout.Space();

            EditorGUI.indentLevel++;
            script.pipelineFoldout = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), script.pipelineFoldout, pipelineLabelContent, true, EditorStyles.foldout);
            if (script.pipelineFoldout)
            {
                script.IntegrationType = (FinalCameraEffectsPro.Integration)EditorGUILayout.EnumPopup(integrationLabelContent, script.IntegrationType);
                EditorGUILayout.Space();

                script.FarPlaneSource = (FinalCameraEffectsProCommandBuffer.FarPlaneSourceType)EditorGUILayout.EnumPopup(farPlaneSourceLabelContent, script.FarPlaneSource);

                //script.IntegrationStage = (FinalCameraEffectsPro.CameraEventType)EditorGUILayout.EnumPopup(integrationLabelContent, script.IntegrationStage);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Deferred rendering path" + (camera.actualRenderingPath == RenderingPath.DeferredShading ? " (active):" : ":"));
                EditorGUI.indentLevel++;
                script.UseGBuffer = EditorGUILayout.Toggle(gBufferLabelContent, script.UseGBuffer);

                DisplayGBufferStateMessage(script);

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Forward rendering path" + (camera.actualRenderingPath == RenderingPath.Forward ? " (active):" : ":"));
                EditorGUI.indentLevel++;
                script.UsePreciseDepthBuffer = EditorGUILayout.Toggle(dedicatedDepthBufferLabelContent, script.UsePreciseDepthBuffer);
                EditorGUI.indentLevel--;

                EditorGUILayout.Space();

            }

            EditorGUI.indentLevel--;

            EditorGUI.indentLevel++;

            FinalCameraEffectsPro.BloomDofOrderType bloomDofOrder = script.BloomOrder < script.DofOrder ? FinalCameraEffectsPro.BloomDofOrderType.BloomBeforeDepthOfField : FinalCameraEffectsPro.BloomDofOrderType.DepthOfFieldBeforeBloom;
            FinalCameraEffectsPro.VignetteOrderType vignetteOrder = script.BloomOrder < script.VignetteOrder ? FinalCameraEffectsPro.VignetteOrderType.VignetteAfterBloom : FinalCameraEffectsPro.VignetteOrderType.VignetteBeforeBloom;
            FinalCameraEffectsPro.DistortionOrderType distortionOrder = script.DistortionOrder < script.DofOrder ? FinalCameraEffectsPro.DistortionOrderType.DistortionBeforeDepthOfField : FinalCameraEffectsPro.DistortionOrderType.DistortionAfterDepthOfField;
            var newBloomDofOrder = bloomDofOrder;
            var newVignetteOrder = vignetteOrder;
            var newDistortionOrder = distortionOrder;

            script.orderFoldout = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), script.orderFoldout, "Effects Order Settings: ", true, EditorStyles.foldout);
            if (script.orderFoldout)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                newBloomDofOrder = (FinalCameraEffectsPro.BloomDofOrderType)EditorGUILayout.EnumPopup("Bloom & Depth of Field Order: ", bloomDofOrder);
                newVignetteOrder = (FinalCameraEffectsPro.VignetteOrderType)EditorGUILayout.EnumPopup("Vignette Order: ", vignetteOrder);
                newDistortionOrder = (FinalCameraEffectsPro.DistortionOrderType)EditorGUILayout.EnumPopup("Distortion Order: ", distortionOrder);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            if (!isCameraHDR(camera) && (script.DofEnabled || script.BloomEnabled))
                EditorGUILayout.HelpBox("We recommend turning HDR camera mode on for best results of bloom and depth of field effects.", MessageType.Warning);

            EditorGUILayout.Space();

            if (newBloomDofOrder != bloomDofOrder || newVignetteOrder != vignetteOrder || newDistortionOrder != distortionOrder)
            {
                if (newBloomDofOrder == FinalCameraEffectsPro.BloomDofOrderType.BloomBeforeDepthOfField)
                {
                    if (newVignetteOrder == FinalCameraEffectsPro.VignetteOrderType.VignetteBeforeBloom)
                    {
                        if (newDistortionOrder == FinalCameraEffectsPro.DistortionOrderType.DistortionBeforeDepthOfField)
                        {
                            script.VignetteOrder = 1;
                            script.BloomOrder = 2;
                            script.DistortionOrder = 3;
                            script.DofOrder = 4;
                        }
                        else
                        {
                            script.VignetteOrder = 1;
                            script.BloomOrder = 2;
                            script.DofOrder = 3;
                            script.DistortionOrder = 4;
                        }
                    }
                    else
                    {
                        if (newDistortionOrder == FinalCameraEffectsPro.DistortionOrderType.DistortionBeforeDepthOfField)
                        {
                            script.BloomOrder = 1;
                            script.VignetteOrder = 2;
                            script.DistortionOrder = 3;
                            script.DofOrder = 4;
                        }
                        else
                        {
                            script.BloomOrder = 1;
                            script.VignetteOrder = 2;
                            script.DofOrder = 3;
                            script.DistortionOrder = 4;
                        }
                    }
                }
                else
                {
                    if (newVignetteOrder == FinalCameraEffectsPro.VignetteOrderType.VignetteBeforeBloom)
                    {
                        if (newDistortionOrder == FinalCameraEffectsPro.DistortionOrderType.DistortionBeforeDepthOfField)
                        {
                            script.DistortionOrder = 1;
                            script.DofOrder = 2;
                            script.VignetteOrder = 3;
                            script.BloomOrder = 4;
                        }
                        else
                        {
                            script.DofOrder = 1;
                            script.DistortionOrder = 2;
                            script.VignetteOrder = 3;
                            script.BloomOrder = 4;
                        }
                    }
                    else
                    {
                        if (newDistortionOrder == FinalCameraEffectsPro.DistortionOrderType.DistortionBeforeDepthOfField)
                        {
                            script.DistortionOrder = 1;
                            script.DofOrder = 2;
                            script.BloomOrder = 3;
                            script.VignetteOrder = 4;
                        }
                        else
                        {
                            script.DofOrder = 1;
                            script.DistortionOrder = 2;
                            script.BloomOrder = 3;
                            script.VignetteOrder = 4;
                        }
                    }
                }
            }

            DistortionGUI(script);
            ChromaticAbbrGUI(script);
            BokehGUI(script);
            BloomGUI(script);
            VignetteGUI(script);
            ColorCorrectionGUI(script);

            EditorGUILayout.Space();

            script.aboutFoldout = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), script.aboutFoldout, "About", true, EditorStyles.foldout);
            if (script.aboutFoldout)
            {
                EditorGUILayout.HelpBox("Final Camera Effects Pro v1.2 by Project Wilberforce.\n\nThank you for your purchase and if you have any questions, issues or suggestions, feel free to contact us at <projectwilberforce@gmail.com>.", MessageType.Info);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Contact Support"))
                {
                    Application.OpenURL("mailto:projectwilberforce@gmail.com");
                }
           
                if (GUILayout.Button("Asset Store Page"))
                {
                    Application.OpenURL("http://u3d.as/Sgg");
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUI.changed)
            {
                script.FocalLength = Mathf.Clamp(script.FocalLength, 1.0f, 1000.0f);

                // Mark as dirty
                EditorUtility.SetDirty(target);
            }

            Undo.RecordObject(target, "Wilberforce Final Camera Effects Pro change");
        }

        private void DistortionGUI(FinalCameraEffectsProCommandBuffer script)
        {
            HeaderWithToggle("Lens Distortion", ref script.distortionFoldout, ref script.DistortionEnabled, distortionIcon);

            if (script.distortionFoldout)
            {
                EditorGUI.BeginDisabledGroup(!script.DistortionEnabled);
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();

                if (distortionIcon != null)
                {
                    GUILayout.Space(15.0f);
                    GUILayout.Label(distortionIcon);
                }

                Rect sliderRect = EditorGUILayout.GetControlRect();
                script.DistortionPower = GUI.HorizontalSlider(sliderRect, script.DistortionPower, -1.0f, 1.0f);

                if (barrelIcon != null)
                {
                    GUILayout.Label(barrelIcon);
                    GUILayout.Space(-15.0f);
                }

                script.DistortionPower = EditorGUILayout.FloatField(distortionLabel, script.DistortionPower);

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                script.IsAnamorphicDistortion = EditorGUILayout.Toggle(anamorphicLensLabel, script.IsAnamorphicDistortion);

                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }
        }

        private void ChromaticAbbrGUI(FinalCameraEffectsProCommandBuffer script)
        {
            HeaderWithToggle("Chromatic Aberrations", ref script.chromaAbrrFoldout, ref script.ChromaEnabled, chromaAbrrIcon);

            if (script.chromaAbrrFoldout)
            {
                EditorGUI.BeginDisabledGroup(!script.ChromaEnabled);
                EditorGUI.indentLevel++;

                script.ChromaWeight = EditorGUILayout.Slider(chromaWeightLabel, script.ChromaWeight, 0.0f, 1.0f);
                script.ChromaSize = EditorGUILayout.Slider(chromaSizeLabel, script.ChromaSize, 0.0f, 1.0f);
                script.ChromaRadialness = EditorGUILayout.Slider(chromaRadialnessLabel, script.ChromaRadialness, 0.0f, 5.0f);

                EditorGUILayout.Space();

                script.ChromaQuality = (FinalCameraEffectsPro.Quality)EditorGUILayout.EnumPopup("Quality: ", script.ChromaQuality);

                EditorGUILayout.Space();

                EditorGUILayout.MinMaxSlider("Spectrum: ", ref script.ChromaSpectrumMin, ref script.ChromaSpectrumMax, 0.0f, 1.0f);

                var controlRect = EditorGUILayout.GetControlRect();

                Rect texRect = new Rect(new Vector2(controlRect.x + 35.0f, controlRect.y), new Vector2(controlRect.width - 35.0f, controlRect.height));

                EditorGUI.DrawPreviewTexture(texRect, script.GetChromaTexture());

                EditorGUILayout.Space();
                script.IsAnamorphicChroma = EditorGUILayout.Toggle(anamorphicLensLabel, script.IsAnamorphicChroma);

                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }
        }

        private void BokehGUI(FinalCameraEffectsProCommandBuffer script)
        {

            HeaderWithToggle("Depth of Field (Bokeh)", ref script.bokehFoldout, ref script.DofEnabled, bokehIcon);

            if (script.bokehFoldout)
            {
                EditorGUI.BeginDisabledGroup(!script.DofEnabled);
                EditorGUI.indentLevel++;

                EditorGUILayout.Space();

                script.DofAlgorithm = (FinalCameraEffectsPro.DofAlgorithmType)EditorGUILayout.EnumPopup(dofAlgorithmTypeLabel, script.DofAlgorithm);

                EditorGUILayout.Space();
                script.FocalPlaneDistance = EditorGUILayout.FloatField(focusDistanceLabel, script.FocalPlaneDistance);
                script.FocalPlaneDistance = Math.Max(0.0f, script.FocalPlaneDistance);

                EditorGUI.BeginDisabledGroup(script.DofCoCModel != FinalCameraEffectsPro.DofCoCModelType.RealisticThinLensModel);
                script.ApertureDiameter = EditorGUILayout.Slider(apertureDiameterLabel, script.ApertureDiameter, 1.0f, 250.0f);
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(script.DofAperture == FinalCameraEffectsPro.DofApertureType.Circular);
                script.DofSoftness = EditorGUILayout.Slider(dofSoftnessLabel, script.DofSoftness, 0.0f, 1.0f);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space();

                script.DofAperture = (FinalCameraEffectsPro.DofApertureType)EditorGUILayout.EnumPopup(apertureShapeLabel, script.DofAperture);
                script.DofRotation = EditorGUILayout.Slider(apertureRotationLabel, script.DofRotation, 0, 360.0f);

                EditorGUILayout.Space();

                script.DofRange = (FinalCameraEffectsPro.DofRangeType)EditorGUILayout.EnumPopup(dofRangeLabel, script.DofRange);
                EditorGUILayout.Space();

                script.DofQuality = (FinalCameraEffectsPro.Quality)EditorGUILayout.EnumPopup("Quality: ", script.DofQuality);

                EditorGUILayout.Space();

                //script.DofGaussianDeviation = EditorGUILayout.FloatField("DofGaussianDeviation", script.DofGaussianDeviation);

                //script.DofLumaFilterEnabled = EditorGUILayout.Toggle("Filter by luminance", script.DofLumaFilterEnabled);

                //if (script.DofLumaFilterEnabled)
                //{
                //    if (lastDofLumaFilterEnabled == false) lumaFoldout = true;

                //    EditorGUILayout.Space();
                //    EditorGUI.indentLevel++;
                //    lumaFoldout = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), lumaFoldout, "Luma label TODO", true, EditorStyles.foldout);

                //    if (lumaFoldout)
                //    {

                //        float tresholdMax = 1.0f;
                //        float kneeWidthMax = 1.0f;
                //        GUIContent tresholdLabel = lumaThresholdLabelContent;
                //        if (camera != null)
                //        {
                //            if (isCameraHDR(camera))
                //            {
                //                lumaMaxFx = EditorGUILayout.FloatField("Luma HDR Max", lumaMaxFx);
                //                tresholdMax = lumaMaxFx;
                //                kneeWidthMax = lumaMaxFx;
                //                tresholdLabel = lumaThresholdHDRLabelContent;

                //                if (!isHDR)
                //                {
                //                    script.DofLumaThreshold *= lumaMaxFx;
                //                    script.DofLumaKneeWidth *= lumaMaxFx;
                //                    isHDR = true;
                //                }
                //            }
                //            else
                //            {
                //                if (isHDR)
                //                {
                //                    script.DofLumaThreshold /= lumaMaxFx;
                //                    script.DofLumaKneeWidth /= lumaMaxFx;
                //                    isHDR = false;
                //                }
                //            }
                //        }

                //        script.DofLumaThreshold = EditorGUILayout.Slider(tresholdLabel, script.DofLumaThreshold, 0.0f, tresholdMax);
                //        script.DofLumaKneeWidth = EditorGUILayout.Slider(lumaWidthLabelContent, script.DofLumaKneeWidth, 0.0f, kneeWidthMax);
                //        script.DofLumaKneeLinearity = EditorGUILayout.Slider(lumaSoftnessLabelContent, script.DofLumaKneeLinearity, 1.0f, 10.0f);
                //        EditorGUILayout.Space();
                //        dofLumaGraphWidget.Draw(GetDofLumaGraphWidgetParameters(script));
                //    }
                //    EditorGUI.indentLevel--;
                //}
                //EditorGUILayout.Space();

                //script.DofPushHighlightsEnabled = EditorGUILayout.Toggle("Push highlights", script.DofPushHighlightsEnabled);

                //if (script.DofPushHighlightsEnabled)
                //{
                //    EditorGUILayout.Space();
                //    EditorGUI.indentLevel++;

                //    script.DofSoftness = EditorGUILayout.Slider("DofSoftness", script.DofSoftness, 0.0f, 1.0f);
                //    script.DofPower = EditorGUILayout.Slider("DofPower", script.DofPower, 1.0f, 10.0f);
                //    script.DofAmount = EditorGUILayout.Slider("DofAmount", script.DofAmount, 0.0f, 1.0f);

                //    EditorGUI.indentLevel--;
                //    EditorGUILayout.Space();
                //}
                //EditorGUILayout.Space();

                //script.DofColorTintEnabled = EditorGUILayout.Toggle("Enable Color tint", script.DofColorTintEnabled);
                //script.DofColorTint = EditorGUILayout.ColorField(colorTintLabelContent, script.DofColorTint);

                //EditorGUILayout.Space();

                script.lensModelFoldout = EditorGUI.Foldout(EditorGUILayout.GetControlRect(), script.lensModelFoldout, lensModelLabelContent, true, EditorStyles.foldout);
                if (script.lensModelFoldout)
                {

                    script.DofCoCModel = (FinalCameraEffectsPro.DofCoCModelType)EditorGUILayout.EnumPopup(cocModelLabelContent, script.DofCoCModel);

                    if (script.DofCoCModel == FinalCameraEffectsPro.DofCoCModelType.FromParameters)
                    {
                        EditorGUILayout.Space();

                        script.DofCoCFromParametersMode = (FinalCameraEffectsPro.DofCoCFromParametersModeType)EditorGUILayout.EnumPopup(cocParametersUnitsLabelContent, script.DofCoCFromParametersMode);
                        EditorGUILayout.Space();

                        EditorGUILayout.LabelField("Foreground:");
                        EditorGUI.indentLevel++;
                        script.DofForegroundCoCDiameter = EditorGUILayout.Slider(defocusSizeLabelContent, script.DofForegroundCoCDiameter, 0.0f, 1.0f);
                        script.DofForegroundCoCDistance = EditorGUILayout.FloatField(defocusSizeDistanceLabelContent, script.DofForegroundCoCDistance);
                        script.DofForegroundCoCLinearity = EditorGUILayout.Slider(defocusCurveLienarityLabelContent, script.DofForegroundCoCLinearity, -1.0f, 1.0f);
                        EditorGUI.indentLevel--;
                        EditorGUILayout.Space();

                        EditorGUILayout.LabelField("Background:");
                        EditorGUI.indentLevel++;
                        script.DofBackgroundCoCDiameter = EditorGUILayout.Slider(defocusSizeLabelContent, script.DofBackgroundCoCDiameter, 0.0f, 1.0f);
                        script.DofBackgroundCoCDistance = EditorGUILayout.FloatField(defocusSizeDistanceLabelContent, script.DofBackgroundCoCDistance);
                        script.DofBackgroundCoCLinearity = EditorGUILayout.Slider(defocusCurveLienarityLabelContent, script.DofBackgroundCoCLinearity, -1.0f, 1.0f);
                        EditorGUI.indentLevel--;

                    }
                    else
                    {

                        EditorGUILayout.Space();

                        script.FocalLengthSource = (FinalCameraEffectsPro.FocalLengthSourceType)EditorGUILayout.EnumPopup(focalLengthSourceLabelContent, script.FocalLengthSource);

                        if (script.FocalLengthSource == FinalCameraEffectsPro.FocalLengthSourceType.Manual)
                        {
                            script.FocalLength = EditorGUILayout.FloatField(focalLengthLabel, script.FocalLength);
                            script.SensorHeight = EditorGUILayout.Slider(sensorHeightLabelContent, script.SensorHeight, 1.0f, 100.0f);
                        }
                        else
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.FloatField(focalLengthLabel, GetFocalLengthFromVerticalFoV(camera.fieldOfView, script.SensorHeight * 0.001f) * 1000.0f);
                            script.SensorHeight = EditorGUILayout.Slider(sensorHeightLabelContent, script.SensorHeight, 1.0f, 100.0f);
                            EditorGUI.EndDisabledGroup();
                        }

                    }
                }
                EditorGUILayout.Space();

                script.DofAnamorphic = (FinalCameraEffectsPro.DofAnamorphicType) EditorGUILayout.EnumPopup(anamorphicLensLabel, script.DofAnamorphic);

                EditorGUILayout.Space();
                dofCocGraphWidget.Draw(GetDofCocGraphWidgetParameters(script));
                EditorGUILayout.Space();

                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }

        }

        private void BloomGUI(FinalCameraEffectsProCommandBuffer script)
        {
            HeaderWithToggle("Bloom", ref script.bloomFoldout, ref script.BloomEnabled, bloomIcon);

            if (script.bloomFoldout)
            {
                EditorGUI.BeginDisabledGroup(!script.BloomEnabled);
                EditorGUI.indentLevel++;

                script.BloomQuality = (FinalCameraEffectsPro.Quality)EditorGUILayout.EnumPopup(bloomQualityLabelContent, script.BloomQuality);
                EditorGUILayout.Space();

                script.BloomRadius = EditorGUILayout.Slider(bloomRadiusLabelContent, script.BloomRadius, 0.0f, 1.0f);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.indentLevel++;
                EditorGUILayout.IntField(bloomMipLevelsLabelContent, script.GetBloomMipLevels());
                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Intensity: ");
                EditorGUI.indentLevel++;

                script.BloomPower = EditorGUILayout.Slider(bloomSoftLabelContent, script.BloomPower, 0.01f, 4.0f);
                script.BloomMultiplier = EditorGUILayout.Slider(bloomStrongLabelContent, script.BloomMultiplier, 0.0f, 10.0f);

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();

                EditorGUILayout.Space();

                if (isCameraHDR(camera))
                {
                    script.BloomThreshold = EditorGUILayout.FloatField(bloomThresholdLabelContent, script.BloomThreshold);
                }
                else
                {
                    script.BloomThreshold = EditorGUILayout.Slider(bloomThresholdLabelContent, script.BloomThreshold, 0.01f, 1.0f);
                }
                EditorGUILayout.Space();

                script.BloomSaturation = EditorGUILayout.Slider(bloomSaturationLabelContent, script.BloomSaturation, 0.0f, 1.0f);

                //script.BloomColorTintEnabled = EditorGUILayout.Toggle("Enable Color tint", script.BloomColorTintEnabled);
                //script.BloomColorTint = EditorGUILayout.ColorField(colorTintLabelContent, script.BloomColorTint);

                EditorGUILayout.Space();

                script.BloomAntiflickerEnabled = EditorGUILayout.Toggle(bloomAntiflickerLabelContent, script.BloomAntiflickerEnabled);

                if (script.BloomAntiflickerEnabled)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space();

                    script.BloomAntiflickerLength = (FinalCameraEffectsPro.LengthType)EditorGUILayout.EnumPopup(antiflickerLengthLabelContent, script.BloomAntiflickerLength);

                    if (script.BloomAntiflickerLength != FinalCameraEffectsProCommandBuffer.LengthType.Short &&
                        script.IntegrationType == FinalCameraEffectsProCommandBuffer.Integration.CommandBuffer)
                    {
                        EditorGUILayout.HelpBox("Only 'Short' length of antiflicker filter is available in 'command buffer' mode.", MessageType.Warning);
                    }

                    script.BloomAntiflickerFade = (FinalCameraEffectsPro.FadeType)EditorGUILayout.EnumPopup(antiflickerFadeLabelContent, script.BloomAntiflickerFade);

                    script.BloomAntiflickerStrength = EditorGUILayout.Slider(antiflickerStrengthLabelContent, script.BloomAntiflickerStrength, 0.1f, 2.0f);

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();

                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }

        }

        private void VignetteGUI(FinalCameraEffectsProCommandBuffer script)
        {
            HeaderWithToggle("Vignette", ref script.vignetteFoldout, ref script.VignetteEnabled, vignetteIcon);

            if (script.vignetteFoldout)
            {
                EditorGUI.BeginDisabledGroup(!script.VignetteEnabled);

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                EditorGUILayout.MinMaxSlider(vignetteMinMaxLabelContent, ref script.VignetteInnerValue, ref script.VignetteOuterValue, 0.0f, 1.0f);

                script.VignetteFalloff = EditorGUILayout.Slider(vignetteFalloffLinearityLabelContent, script.VignetteFalloff, 1.0f, 10.0f);

                EditorGUILayout.Space();

                EditorGUILayout.MinMaxSlider(vignetteMinMaxDistanceLabelContent, ref script.VignetteInnerValueDistance, ref script.VignetteOuterValueDistance, 0.0f, 2.0f);

                EditorGUILayout.Space();
                script.VignetteMode = (FinalCameraEffectsPro.VignetteModeType)EditorGUILayout.EnumPopup(vignetteModeLabelContent, script.VignetteMode);

                if (script.VignetteMode == FinalCameraEffectsPro.VignetteModeType.CustomColors)
                {
                    EditorGUI.indentLevel++;
                    script.VignetteInnerColor = EditorGUILayout.ColorField(vignetteInnerColorLabelContent, script.VignetteInnerColor);
                    script.VignetteOuterColor = EditorGUILayout.ColorField(vignetteOuterColorLabelContent, script.VignetteOuterColor);
                    EditorGUI.indentLevel--;
                }
                else if (script.VignetteMode == FinalCameraEffectsPro.VignetteModeType.Saturation)
                {
                    EditorGUI.indentLevel++;
                    script.VignetteInnerSaturation = EditorGUILayout.Slider(vignetteInnerSaturationLabelContent, script.VignetteInnerSaturation, 0.0f, 1.0f);
                    script.VignetteOuterSaturation = EditorGUILayout.Slider(vignetteOuterSaturationLabelContent, script.VignetteOuterSaturation, 0.0f, 1.0f);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();

                script.VignetteCenter = EditorGUILayout.Vector2Field(vignetteCenterLabelContent, script.VignetteCenter);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(40.0f);

                if (GUILayout.Button("Reset Center", new GUILayoutOption[] {
                   GUILayout.Width(100.0f),
                }))
                {
                    script.VignetteCenter.x = script.VignetteCenter.y = 0.5f;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                script.IsAnamorphicVignette = EditorGUILayout.Toggle(anamorphicLensLabel, script.IsAnamorphicVignette);
                EditorGUILayout.Space();

                EditorGUILayout.Space();
                vignetteGraphWidget.Draw(GetVignetteGraphWidgetParameters(script));
                EditorGUILayout.Space();

                script.VignetteDebugEnabled = EditorGUILayout.Toggle("Show Debug Output (Vignette): ", script.VignetteDebugEnabled);

                EditorGUI.indentLevel--;

                EditorGUI.EndDisabledGroup();
            }
        }

        private void ColorCorrectionGUI(FinalCameraEffectsProCommandBuffer script)
        {
            HeaderWithToggle("Color Grading & Tonemapping", ref script.colorCorrectionFoldout, ref script.ColorGradingEnabled, filmIcon);

            if (script.colorCorrectionFoldout)
            {
                EditorGUI.BeginDisabledGroup(!script.ColorGradingEnabled);

                EditorGUI.indentLevel++;
                EditorGUILayout.Space();

                script.TonemappingMode = (FinalCameraEffectsPro.TonemappingModeType)EditorGUILayout.EnumPopup(tonemappingModeLabelContent, script.TonemappingMode);
                
                EditorGUI.BeginDisabledGroup(script.TonemappingMode == FinalCameraEffectsProCommandBuffer.TonemappingModeType.Off);
                EditorGUI.indentLevel++;

                script.TonemappingIntensity = EditorGUILayout.Slider(tonemappingIntensityLabelContent, script.TonemappingIntensity, 0.0f, 1.0f);

                EditorGUILayout.Space();

                script.ExposureAdjustment = EditorGUILayout.Slider(exposureAdjustmentLabelContent, script.ExposureAdjustment, -1.0f, 1.0f);
                script.TonemappingGamma = EditorGUILayout.Slider(tonemappingGammaLabelContent, script.TonemappingGamma, 0.05f, 4.0f);

                EditorGUILayout.Space();

                script.TonemappingContrast = EditorGUILayout.Slider(tonemappingContrastLabelContent, script.TonemappingContrast, -1.0f, 1.0f);
                script.TonemappingSaturation = EditorGUILayout.Slider(tonemappingSaturationLabelContent, script.TonemappingSaturation, 0.0f, 1.0f);

                EditorGUILayout.Space();

                script.TonemappingColorTemperature = EditorGUILayout.Slider(tonemappingColorTemperatureLabelContent, script.TonemappingColorTemperature, -1.0f, 1.0f);
                script.TonemappingColorTint = EditorGUILayout.Slider(tonemappingColorTintLabelContent, script.TonemappingColorTint, -1.0f, 1.0f);


                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Reset to Neutral", new GUILayoutOption[] {
                   GUILayout.Width(120.0f),
                }))
                {
                    script.TonemappingIntensity = 0.8f;
                    script.ExposureAdjustment = 0.0f;
                    script.TonemappingGamma = 1.0f;
                    script.TonemappingContrast = 0.0f;
                    script.TonemappingSaturation = 1.0f;
                    script.TonemappingColorTemperature = 0.0f;
                    script.TonemappingColorTint = 0.0f;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.Space();

                script.ColorCorrectionMode = (FinalCameraEffectsPro.ColorCorrectionModeType)EditorGUILayout.EnumPopup(colorCorrectionModeLabelContent, script.ColorCorrectionMode);

                EditorGUI.indentLevel++;

                EditorGUI.BeginDisabledGroup(script.ColorCorrectionMode == FinalCameraEffectsProCommandBuffer.ColorCorrectionModeType.Off);
                script.ColorCorrectionIntensity = EditorGUILayout.Slider(colorCorrectionIntensityLabelContent, script.ColorCorrectionIntensity, 0.0f, 1.0f);
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(script.ColorCorrectionMode != FinalCameraEffectsProCommandBuffer.ColorCorrectionModeType.LUTTexture);

                var rect = EditorGUILayout.GetControlRect();
                script.ColorCorrectionLutTexture = (Texture2D)EditorGUI.ObjectField(rect, colorCorrectionLutTextureLabelContent, script.ColorCorrectionLutTexture, typeof(Texture2D), false);

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(script.ColorCorrectionMode != FinalCameraEffectsProCommandBuffer.ColorCorrectionModeType.Manual);

                // Ensure color correction curves
                if (script.ColorCorrectionRedCurve == null || script.ColorCorrectionRedCurve.length == 0) script.ColorCorrectionRedCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
                if (script.ColorCorrectionGreenCurve == null || script.ColorCorrectionGreenCurve.length == 0) script.ColorCorrectionGreenCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
                if (script.ColorCorrectionBlueCurve == null || script.ColorCorrectionBlueCurve.length == 0) script.ColorCorrectionBlueCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

                script.ColorCorrectionRedCurve = EditorGUILayout.CurveField("Red:", script.ColorCorrectionRedCurve);
                script.ColorCorrectionGreenCurve = EditorGUILayout.CurveField("Green:", script.ColorCorrectionGreenCurve);
                script.ColorCorrectionBlueCurve = EditorGUILayout.CurveField("Blue:", script.ColorCorrectionBlueCurve);

                EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }
        }
               

        private static void HeaderWithToggle(string label, ref bool isFoldout, ref bool isEnabled, Texture icon = null, bool clickableIcon = true)
        {

            EditorGUILayout.BeginHorizontal();
            isFoldout = GUILayout.Toggle(isFoldout, "", isFoldout ? customFoldinStyle : customFoldoutStyle);
            isEnabled = GUILayout.Toggle(isEnabled, "", customToggleStyle);

            if (icon != null)
            {
                if (clickableIcon)
                    isFoldout = GUILayout.Toggle(isFoldout, icon, customLabelStyle);
                else
                    GUILayout.Label(icon);

                GUILayout.Space(-5.0f);
            }

            isFoldout = GUILayout.Toggle(isFoldout, label, customLabelStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

#endregion

    }

#region Graph Widget

    public class GraphWidgetLine
    {
        public Vector3 From { get; set; }
        public Vector3 To { get; set; }
        public Color Color { get; set; }
        public float Thickness { get; set; }
    }

    public class GraphWidgetDrawingParameters
    {

        public IList<GraphWidgetLine> Lines { get; set; }

        /// <summary>
        /// Number of line segments that will be used to approximate function shape
        /// </summary>
        public uint GraphSegmentsCount { get; set; }

        /// <summary>
        /// Function to draw (X -> Y) 
        /// </summary>
        public Func<float, float> GraphFunction { get; set; }

        public Color GraphColor { get; set; }
        public float GraphThickness { get; set; }

        public float YScale { get; internal set; }
        public float MinY { get; internal set; }

        public int GridLinesXCount { get; set; }
        public float MaxFx { get; internal set; }

        public string LabelText { get; set; }
    }

    public class GraphWidget
    {
        private Vector3[] transformedLinePoints = new Vector3[2];
        private Vector3[] graphPoints;

        void TransformToRect(Rect rect, ref Vector3 v)
        {
            v.x = Mathf.Lerp(rect.x, rect.xMax, v.x);
            v.y = Mathf.Lerp(rect.yMax, rect.y, v.y);
        }

        private void DrawLine(Rect rect, float x1, float y1, float x2, float y2, Color color)
        {
            transformedLinePoints[0].x = x1;
            transformedLinePoints[0].y = y1;
            transformedLinePoints[1].x = x2;
            transformedLinePoints[1].y = y2;

            TransformToRect(rect, ref transformedLinePoints[0]);
            TransformToRect(rect, ref transformedLinePoints[1]);

            Handles.color = color;
            Handles.DrawPolyLine(transformedLinePoints);
        }

        private void DrawAALine(Rect rect, float thickness, float x1, float y1, float x2, float y2, Color color)
        {
            transformedLinePoints[0].x = x1;
            transformedLinePoints[0].y = y1;
            transformedLinePoints[1].x = x2;
            transformedLinePoints[1].y = y2;

            TransformToRect(rect, ref transformedLinePoints[0]);
            TransformToRect(rect, ref transformedLinePoints[1]);

            Handles.color = color;
            Handles.DrawPolyLine(transformedLinePoints);
        }

        public void Draw(GraphWidgetDrawingParameters drawingParameters)
        {
            Handles.color = Color.white; //< Reset to white to avoid Unity bugs

            Rect bgRect = GUILayoutUtility.GetRect(128, 70);
            Handles.DrawSolidRectangleWithOutline(bgRect, Color.grey, Color.black);

            // Draw grid lines
            Color gridColor = Color.black * 0.1f;
            DrawLine(bgRect, 0.0f, drawingParameters.MinY + drawingParameters.YScale,
                             1.0f, drawingParameters.MinY + drawingParameters.YScale, gridColor);

            DrawLine(bgRect, 0.0f, drawingParameters.MinY,
                             1.0f, drawingParameters.MinY, gridColor);

            float gridXStep = 1.0f / (drawingParameters.GridLinesXCount + 1);
            float gridX = gridXStep;
            for (int i = 0; i < drawingParameters.GridLinesXCount; i++)
            {
                DrawLine(bgRect, gridX, 0.0f,
                                 gridX, 1.0f, gridColor);

                gridX += gridXStep;
            }

            if (drawingParameters.GraphSegmentsCount > 0)
            {
                if (graphPoints == null || graphPoints.Length < drawingParameters.GraphSegmentsCount + 1)
                    graphPoints = new Vector3[drawingParameters.GraphSegmentsCount + 1];

                float x = 0.0f;
                float xStep = 1.0f / drawingParameters.GraphSegmentsCount;

                for (int i = 0; i < drawingParameters.GraphSegmentsCount + 1; i++)
                {
                    float y = drawingParameters.GraphFunction(x * drawingParameters.MaxFx);

                    y *= drawingParameters.YScale;
                    y += drawingParameters.MinY;

                    graphPoints[i].x = x;
                    graphPoints[i].y = y;
                    TransformToRect(bgRect, ref graphPoints[i]);
                    x += xStep;
                }

                Handles.color = drawingParameters.GraphColor;
                Handles.DrawAAPolyLine(drawingParameters.GraphThickness, graphPoints);
            }

            if (drawingParameters != null && drawingParameters.Lines != null)
            {
                foreach (var line in drawingParameters.Lines)
                {
                    DrawAALine(bgRect, line.Thickness, line.From.x, line.From.y, line.To.x, line.To.y, line.Color);
                }
            }

            // Label
            Vector3 labelPosition = new Vector3(0.01f, 0.99f);
            TransformToRect(bgRect, ref labelPosition);
            Handles.Label(labelPosition, drawingParameters.LabelText, EditorStyles.miniLabel);

        }

    }

#endregion

#endif
}