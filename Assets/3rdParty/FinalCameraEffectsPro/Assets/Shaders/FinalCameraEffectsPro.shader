// Copyright (c) 2018 Jakub Boksansky - All Rights Reserved
// Wilberforce Final Camera Effects Pro Unity Plugin 1.2

Shader "Hidden/Wilberforce/FinalCameraEffectsPro"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		CGINCLUDE

#pragma target 3.0

#include "UnityCG.cginc"

		// ========================================================================
		// Uniform definitions 
		// ========================================================================

		sampler2D _CameraDepthNormalsTexture;
		float4 _CameraDepthNormalsTexture_ST;
		sampler2D _CameraGBufferTexture2;
		sampler2D _CameraDepthTexture;

		uniform int useGBuffer;

		float4 _ProjInfo;

		sampler2D _MainTex;
		float4 _MainTex_TexelSize;
		float4 _MainTex_ST;

		uniform int debugMode;
		uniform int isImageEffectMode;
		uniform int useSPSRFriendlyTransform;
		sampler2D texCopySource; 

		// Lens Parameters ========================================================
		uniform float aspectRatio;
		uniform float apertureDiameter;
		uniform float focalPlaneDistance;
		uniform float focalLength;
		uniform float sensorHeight;

		// Chroma =================================================================
		uniform float chromaWeight;
		uniform float chromaRadialness;
		uniform float chromaQuality;
		uniform float3 chromaBigWeightRcp;
		uniform float chromaTexSize;
		uniform float chromaSpectrumMin;
		uniform float chromaSpectrumMax;
		uniform float minTexelSize;
		uniform float minTexelSizeDofQuality;
		uniform float fastBlurMixingCoeff; 
		
		sampler2D chromaSamplesTexture;
		uniform int isAnamorphicChroma;

		// Vignette ===============================================================
		uniform float vignetteFalloff;
		uniform float vignetteMin;
		uniform float vignetteMax;
		uniform float2 vignetteCenter;
		uniform float4 vignetteInnerColor;
		uniform float4 vignetteOuterColor;
		uniform float vignetteSaturationMin;
		uniform float vignetteSaturationMax;
		uniform float vignetteMaxDistance;
		uniform float vignetteMinDistance;
		uniform int isAnamorphicVignette;
		uniform int vignetteMode;

		// Bloom ==================================================================
		sampler2D bloomSourceTexture;
		sampler2D bloomScalingFixTexture;
		sampler2D bloomPrepassSource;

		uniform float bottomLevelRadius;
		uniform float bloomPower;
		uniform float bloomMultiplier;
		uniform float bloomThreshold;
		uniform float bloomSaturation;
		uniform int bloomSize;
		uniform float4 bloomGauss[99];
		uniform	float4 bloomColorTint;
		uniform float2 bloomTexSizeRcp0;
		uniform float2 bloomTexSizeRcp1;
		uniform float2 bloomTexSizeRcp2;
		uniform float2 bloomTexSizeRcp3;
		uniform float2 bloomTexSizeRcp4;
		uniform float2 bloomTexSizeRcp5;
		uniform float2 bloomTexSizeRcp6;
		uniform float2 bloomTexSizeRcp7;
		uniform float2 bloomTexSizeRcpBottom;

		// Antiflicker for bloom
		sampler2D lumaHistoryBuffer;
		sampler2D lumaHistoryBuffer2;
		sampler2D lumaHistoryBuffer3;
		uniform float4 lumaHistoryWeights;
		uniform float4 lumaHistoryWeights2;
		uniform float4 lumaHistoryWeights3;
		uniform float4 lumaHistoryMask;
		uniform float currentLumaWeight;
		uniform int lumaHistoryLength;
		uniform int bloomAntiflickerFade;

		// Distortion =============================================================
		uniform	float barrelPower;
		uniform float2 distortionMaxUV;
		uniform int isAnamorphicDistortion;

		// Depth of field =========================================================
		static const float maxCoC = 0.1f; //< Maximum allowed circle of confusion size wrt. screen size
		static const float maxCoCRcp = 10.0f; 

		uniform float4 separableBlurDirections[6];
		uniform int blurMixingPass1DirectionVectorIdx;
		uniform int blurMixingPass2DirectionVectorIdx;
		sampler2D dofInputTexture;
		sampler2D circleOfConfusionTexture;
		sampler2D cocInputTexture;
		
		sampler2D bokehAuxTexture1;
		sampler2D bokehAuxTexture2;
		uniform	float dofPower;
		uniform	float dofAmount;
		uniform float dofSoftness;
		uniform	float4 dofColorTint;
		uniform float2 dofTexelSize;

		uniform	float DofLumaThreshold;
		uniform	float DofLumaKneeWidth;
		uniform	float DofLumaTwiceKneeWidthRcp;
		uniform	float DofLumaKneeLinearity;
		uniform float4 dofGauss[33];
		uniform	int dofQuality;
		uniform	float dofQualityCoeff;
		uniform float cocCoefficient;
		
		uniform	float dofForegroundMaxCoCDistance;
		uniform	float dofBackgroundMaxCoCDistance;
		uniform	float dofForegroundMaxCoCDiameter;
		uniform	float dofBackgroundMaxCoCDiameter;
		uniform	float dofForegroundCoCLinearity;
		uniform	float dofBackgroundCoCLinearity;
		uniform int dofRange;
		uniform int isDofThinLensModel;
		uniform int isDofLumaFilter;
		uniform int isDofColortint;
		uniform int isDofPushHighlights;

		uniform int dofMixTypeMixingPass1;
		uniform int dofMixTypeMixingPass2;

		uniform int isDofAuxInputHotPass1;
		uniform int isDofAuxInputHotPass2;
		uniform int isDistortionBeforeDof;

		uniform float2 dofFastPassTexelSize;

		uniform int mustFlipDistortionInput;
		uniform int mustFlipChromaticAberrationsInput;
		uniform int mustFlipVignetteInput;
		uniform int mustFlipCoCMapInput;
		uniform int mustFlipBloomInput;

		uniform int useCameraFarPlane;
		uniform float cameraFarPlane;

		uniform float2 cocBlurTexSizeRcp0;
		uniform float2 cocBlurTexSizeRcp1;
		uniform float2 cocBlurTexSizeRcp2;

		// Color correction =======================================================
		sampler2D lutTexture;
		uniform float colorCorrectionIntensity;
		uniform float exposureAdjustment;
		uniform float tonemappingSaturation;
		uniform float tonemappingIntensity;
		uniform float tonemappingGamma;
		uniform float tonemappingContrast;
		
		uniform float3 tonemappingColorBalance;
		
		uniform float4 lutScale;
		uniform float2 lutOffset;
		
		uniform float manualLUTTextureSize;
		uniform float LUTGeneratorRedValues[32];
		uniform float LUTGeneratorGreenValues[32];
		uniform float LUTGeneratorBlueValues[32];

		// ========================================================================
		// Structs definitions 
		// ========================================================================

		struct v2fDouble {
			float4 pos : SV_POSITION;
			float2 uv[2] : TEXCOORD0;
		};

		struct CoCOutput
		{
			float4 colorAndDepth : SV_Target0;
			#ifdef SHADER_API_D3D9
			half4 cocDiameter : SV_Target1;
			#else
			half cocDiameter : SV_Target1;
			#endif
		};

		struct BloomPrepassOutput
		{
			float4 color : SV_Target0;
			float4 lumaHistory : SV_Target1;
		};

		// ========================================================================
		// Helper functions
		// ========================================================================

		static const float gauss25[25] = { 0.001997721, 0.003685601, 0.006446936, 0.01069229, 0.01681355, 0.02506803, 0.03543667, 0.04749605, 0.06035787, 0.07272476, 0.08308116, 0.0899901, 0.09241847, 0.08999009, 0.08308115, 0.07272474, 0.06035786, 0.04749604, 0.03543664, 0.02506803, 0.01681353, 0.01069228, 0.006446932, 0.003685599, 0.001997721 };
		static const float gauss19[19] = { 0.003139551, 0.006746877, 0.01325109, 0.02378559, 0.03902022, 0.05850312, 0.08016445, 0.1003917, 0.114902, 0.1201907, 0.114902, 0.1003917, 0.08016443, 0.05850313, 0.03902022, 0.02378558, 0.01325108, 0.006746877, 0.003139553 };
		static const float gauss17[17] = { 0.003815528, 0.008779441, 0.0180769, 0.03330628, 0.05491277, 0.08101504, 0.1069555, 0.126353, 0.1335712, 0.126353, 0.1069554, 0.08101504, 0.05491275, 0.03330626, 0.0180769, 0.008779436, 0.003815525 };
		static const float gauss15[15] = { 0.004793951, 0.0119582, 0.02591586, 0.04879693, 0.07982645, 0.1134563, 0.1400998, 0.1503051, 0.1400998, 0.1134563, 0.07982645, 0.04879693, 0.02591586, 0.0119582, 0.004793951 };
		static const float gauss13[13] = { 0.006299106, 0.01729837, 0.03953327, 0.07518861, 0.1190071, 0.1567565, 0.1718342, 0.1567565, 0.1190071, 0.07518861, 0.03953325, 0.01729837, 0.006299103 };
		static const float gauss11[11] = { 0.008812229, 0.02714358, 0.06511406, 0.1216491, 0.1769983, 0.2005654, 0.1769983, 0.1216491, 0.06511406, 0.02714358, 0.008812229 };
		static const float gauss9[9] = { 0.01351957, 0.04766219, 0.1172301, 0.2011676, 0.2408413, 0.2011676, 0.11723, 0.04766217, 0.01351957 };
		static const float gauss7[7] = { 0.02397741, 0.09784279, 0.2274913, 0.301377, 0.2274913, 0.09784279, 0.02397741 };
		static const float gauss5[5] = { 0.05448868, 0.2442013, 0.40262, 0.2442013, 0.05448868 };

		float luma(half3 color) {
			return 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
		}

		float3 setSaturation(float3 color, float saturation) {
			return lerp(luma(color), color, saturation);
		}

		float3 gammaToLinear(float3 gamma) {
			// Use Unity's built-in helper
			return GammaToLinearSpace(gamma);
		}

		float3 linearToGamma(float3 lin) {
			// Use Unity's built-in helper
			return LinearToGammaSpace(lin);
		}

		float getFarPlane() {
			if (useCameraFarPlane != 0) {
				return cameraFarPlane;
			}
			else {
				return _ProjectionParams.z;
			}
		}

		#if defined(SHADER_API_GLES)
		#define WFORCE_UNROLL(n)
		#else
		#define WFORCE_UNROLL(n) [unroll(n)]
		#endif

		#ifndef SHADER_API_GLCORE
		#ifndef SHADER_API_OPENGL
		#ifndef SHADER_API_GLES
		#ifndef SHADER_API_GLES3
		#ifndef SHADER_API_VULKAN
		#define WFORCE_VAO_OPENGL_OFF
		#endif
		#endif
		#endif
		#endif
		#endif

		// ========================================================================
		// Vertex shaders 
		// ========================================================================

		v2fDouble vertDouble(appdata_img v)
		{
			v2fDouble o;

			#ifdef UNITY_SINGLE_PASS_STEREO
			if (isImageEffectMode != 0 && useSPSRFriendlyTransform != 0) {
				o.pos = float4((v.vertex.xy * 2.0f) - 1.0f, v.vertex.zw);

#if defined(WFORCE_VAO_OPENGL_OFF)
				v.texcoord.y = 1.0f - v.texcoord.y;
#endif
			}
			else {
				o.pos = UnityObjectToClipPos(v.vertex);
			}
			#else
			o.pos = UnityObjectToClipPos(v.vertex);
			#endif

			float2 temp = v.texcoord;

			if(isImageEffectMode != 0 && useSPSRFriendlyTransform != 2)
				temp = UnityStereoTransformScreenSpaceTex(v.texcoord);
			
			o.uv[0] = temp;
			o.uv[1] = temp;

	#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0)
				o.uv[1].y = 1.0f - o.uv[1].y;
	#endif
			
			return o;
		}

		v2fDouble vertDoubleSPSRAware(appdata_img v)
		{
			v2fDouble o;
			
#ifdef UNITY_SINGLE_PASS_STEREO
			if (isImageEffectMode != 0 && useSPSRFriendlyTransform != 0) {
				o.pos = (v.vertex);
				o.pos.xy *= 2.0f;
				o.pos.xy -= 1.0f;

#if defined(WFORCE_VAO_OPENGL_OFF)
				v.texcoord.y = 1.0f - v.texcoord.y;
#endif

			}
			else {
				o.pos = UnityObjectToClipPos(v.vertex);
			}
#else
			o.pos = UnityObjectToClipPos(v.vertex);
#endif

			float2 temp = UnityStereoTransformScreenSpaceTex(v.texcoord);

			o.uv[0] = temp;
			o.uv[1] = temp;

#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0)
				o.uv[1].y = 1.0f - o.uv[1].y;
#endif

			return o;
		}

		v2fDouble vertDoubleTexCopy(appdata_img v)
		{
			v2fDouble o;
			o.pos = v.vertex;

			float2 temp = UnityStereoTransformScreenSpaceTex(v.texcoord);

			o.uv[0] = temp;
			o.uv[1] = temp;

#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0)
				o.uv[1].y = 1.0f - o.uv[1].y;
#endif

#if !defined(WFORCE_VAO_OPENGL_OFF)
			o.uv[0].y = 1.0f - o.uv[0].y;
			o.uv[1].y = o.uv[0].y;
#endif
			
			return o;
		}

		// ========================================================================
		// Image effects 
		// ========================================================================

		// 0. Distortion ==========================================================

		float2 getDistortedUVs(float2 uv) : SV_Target
		{
			uv = uv * 2.0f - 1.0f;

			if (isAnamorphicDistortion != 0) {
				uv.y = uv.y * aspectRatio;
			}

			float distanceFromCenter = length(uv);

			float distortedDistanceFromCenter = pow(distanceFromCenter, barrelPower);

			uv = normalize(uv) * distortedDistanceFromCenter;

			if (isAnamorphicDistortion != 0) {
				uv.y = uv.y / aspectRatio;
			}

			uv *= distortionMaxUV;

			uv = 0.5f * (uv + 1.0f);

			return uv;
		}

		half4 distort(v2fDouble input) : SV_Target
		{

			/*if (mustFlipDistortionInput != 0)
			input.uv[1].y = 1.0f - input.uv[1].y;*/

			half4 color = tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(getDistortedUVs(input.uv[1])));

			return color;

		}

		// 1. Bokeh ===============================================================

		float getCoCForViewSpaceDepth(float viewSpaceDepth) {

			float cocDiameter;
		
			// Thin lens model circle of confusion
			if (isDofThinLensModel != 0) {
				cocDiameter = cocCoefficient * (abs(viewSpaceDepth - focalPlaneDistance) / viewSpaceDepth);
			} else {

				if (viewSpaceDepth > focalPlaneDistance) {
					// foreground
					float u = saturate((focalPlaneDistance - viewSpaceDepth) / (focalPlaneDistance - dofForegroundMaxCoCDistance));

					if (dofForegroundCoCLinearity != 0.0f) {
						if (dofForegroundCoCLinearity > 0.0f) {
							u = lerp(u, pow(u, 5.0f), dofForegroundCoCLinearity);
						}
						else {
							u = lerp(u, 1.0f - pow(1.0f - u, 5.0f), -dofForegroundCoCLinearity);
						}
					}

					cocDiameter = lerp(0.0f, dofForegroundMaxCoCDiameter * maxCoC, u);
				}
				else {
					// background
					float u = saturate((viewSpaceDepth - focalPlaneDistance) / (dofBackgroundMaxCoCDistance - focalPlaneDistance));

					if (dofBackgroundCoCLinearity != 0.0f) {
						if (dofBackgroundCoCLinearity > 0.0f) {
							u = lerp(u, pow(u, 5.0f), dofBackgroundCoCLinearity);
						}
						else {
							u = lerp(u, 1.0f - pow(1.0f - u, 5.0f), -dofBackgroundCoCLinearity);
						}
					}

					cocDiameter = lerp(0.0f, dofBackgroundMaxCoCDiameter * maxCoC, u);
				}

			}

			// Limit circle of confusion to maxCoC (10%) of image size and scale to <0;1> interval
			cocDiameter = max(0.0f, min(cocDiameter, maxCoC));

			return cocDiameter;
		}

		CoCOutput generateCoCMap(v2fDouble input)
		{
			CoCOutput output;

			float2 colormapUV = input.uv[0];
			//if (mustFlipCoCMapInput != 0)
			//	colormapUV.y = 1.0f - colormapUV.y;
			
			float3 color = tex2Dlod(cocInputTexture, float4(colormapUV, 0, 0)).rgb;
			float viewSpaceDepth;
			float viewSpaceDepth01;
			float cocDiameter;
			float2 depthUVs;

			if (isDistortionBeforeDof != 0) {
				depthUVs = getDistortedUVs(input.uv[1]);
			} else {
				depthUVs = input.uv[0];
			}

			if (useGBuffer == 0) {
				viewSpaceDepth01 = DecodeFloatRG(tex2Dlod(_CameraDepthNormalsTexture, float4(depthUVs, 0, 0)).zw);
			} else {
				viewSpaceDepth01 = Linear01Depth(tex2Dlod(_CameraDepthTexture, float4(depthUVs, 0, 0)).r);
			}

			viewSpaceDepth = -viewSpaceDepth01 * getFarPlane();
			cocDiameter = getCoCForViewSpaceDepth(viewSpaceDepth);

			// Scale to <0;1> interval
			cocDiameter = cocDiameter * maxCoCRcp;
			
			// Force foreground coc only
			if (dofRange != 2) {
				if (viewSpaceDepth < focalPlaneDistance) cocDiameter = 0;
			} else {
				// Background bokeh only
				if (viewSpaceDepth > focalPlaneDistance) cocDiameter = 0;
			}

			output.colorAndDepth = half4(color, viewSpaceDepth01);
			output.cocDiameter = cocDiameter;
			
			return output;
		}
		
		half4 upscaleCoCMap(v2fDouble input) 
		{
			float2 uvs = UnityStereoTransformScreenSpaceTex(input.uv[1]);

			float originalCoC = tex2Dlod(circleOfConfusionTexture, float4(uvs, 0, 0)).r;
			float blurCoC = tex2Dlod(_MainTex, float4(uvs, 0, 0)).r;
			
			float result = 2.0f * max(blurCoC, originalCoC) - originalCoC;

			float2 depthUVs;
			if (isDistortionBeforeDof != 0) {
				depthUVs = UnityStereoTransformScreenSpaceTex(getDistortedUVs(input.uv[1]));
			}
			else {
				depthUVs = UnityStereoTransformScreenSpaceTex(input.uv[0]);
			}

			if (dofRange != 1) {

				float viewSpaceDepth01;
				float viewSpaceDepth;

				if (useGBuffer == 0) {
					viewSpaceDepth01 = DecodeFloatRG(tex2Dlod(_CameraDepthNormalsTexture, float4(depthUVs, 0, 0)).zw);
				} else {
					viewSpaceDepth01 = Linear01Depth(tex2Dlod(_CameraDepthTexture, float4(depthUVs, 0, 0)).r);
				}

				viewSpaceDepth = -viewSpaceDepth01 * getFarPlane();

				// Add background bokeh
				if (viewSpaceDepth < focalPlaneDistance) {

					float cocDiameter = getCoCForViewSpaceDepth(viewSpaceDepth);
					result = max(result, (cocDiameter * maxCoCRcp));
				}
			}

			return result;
		}

		half4 blurCoCMap(v2fDouble input, float2 texelSize, int isXPass, int isMixing) {
			float acc = 0.0f;
			int idx = 0;

			WFORCE_UNROLL(11)
			for (int i = -5; i <= 5; ++i) {

				float2 offset;

				if (isXPass == 1)
					offset = input.uv[1] + float2(float(i) * texelSize.x, 0.0f);
				else
					offset = input.uv[1] + float2(0.0f, float(i) * texelSize.y);
				
				float tapSample = tex2Dlod(_MainTex, float4(UnityStereoTransformScreenSpaceTex(offset), 0, 0)).r;

				acc += tapSample * gauss11[idx];

				idx++;
			}
			
			if (isMixing == 0) {
				return acc;
			} else {
				float originalCoC = tex2Dlod(circleOfConfusionTexture, float4(UnityStereoTransformScreenSpaceTex(input.uv[1]), 0, 0)).r;
				
				float result = 2.0f * max(acc, originalCoC) - originalCoC;

				if (dofRange != 1) {

					float viewSpaceDepth01;
					float viewSpaceDepth;

					if (useGBuffer == 0) {
						viewSpaceDepth01 = DecodeFloatRG(tex2Dlod(_CameraDepthNormalsTexture, float4(UnityStereoTransformScreenSpaceTex(input.uv[1]), 0, 0)).zw);
					}
					else {
						viewSpaceDepth01 = Linear01Depth(tex2Dlod(_CameraDepthTexture, float4(UnityStereoTransformScreenSpaceTex(input.uv[1]), 0, 0)).r);
					}

					viewSpaceDepth = -viewSpaceDepth01 * getFarPlane();

					// Add background bokeh
					if (viewSpaceDepth < focalPlaneDistance) {
						
						float cocDiameter = getCoCForViewSpaceDepth(viewSpaceDepth);
						result = max(result, (cocDiameter * maxCoCRcp));
					}
				}

				return result;
			}
		}

		half4 bokehSeparableBlurPass(v2fDouble input, float4 separableBlurDirection, float pixelDepth, float cocDiameter, int separableBlurSize, int useGaussianWeights)
		{
			float4 result = float4(0.0f, 0.0f, 0.0f, 0.0f);
			float totalWeight = 0.0f;

			float blurSizeRcp = (cocDiameter * 0.5f) / float(separableBlurSize - 1); //< multiply by half to get radius from diameter
			float2 offsetStart = separableBlurDirection.zw * (cocDiameter * 0.5f);
			float farPlane = getFarPlane();

			WFORCE_UNROLL(25) //< Max possible size
			for (int i = 0; i < separableBlurSize; ++i) {
				float2 offset = offsetStart + separableBlurDirection.xy * (float(i) * blurSizeRcp);
				float2 sampleUV = UnityStereoTransformScreenSpaceTex(input.uv[1] + offset);

				float4 sampleColor = tex2Dlod(_MainTex, float4(sampleUV, 0, 0)).rgba;
				float sampleDepth = -sampleColor.a * farPlane;
				float sampleCoc = tex2Dlod(circleOfConfusionTexture, float4(sampleUV, 0, 0)).r * maxCoC;

				// Intensity leakage fix (focused objects shall not influence blurred backgrounds)
				float weight = sampleDepth > pixelDepth ? sampleCoc * 35.0f : 1.0f; //< 35 sets falloff speed to prevent popping 
				weight = saturate(weight);
				
				// Apply softness
				weight += luma(sampleColor.rgb) * dofSoftness;

				if (useGaussianWeights != 0) {
					if (separableBlurSize == 5) {
						weight *= gauss5[i];
					} else if (separableBlurSize == 7) {
						weight *= gauss7[i];
					} else if (separableBlurSize == 9) {
						weight *= gauss9[i];
					} else if (separableBlurSize == 11) {
						weight *= gauss11[i];
					} else if (separableBlurSize == 15) {
						weight *= gauss15[i];
					} else if (separableBlurSize == 19) {
						weight *= gauss19[i];
					} else if (separableBlurSize == 25) {
						weight *= gauss25[i];
					}
				}

				result += sampleColor * weight;

				totalWeight += weight;
			}

			if (totalWeight == 0.0f) return result;

			return half4(result.rgba / totalWeight);
		}

		half4 bokehSeparableBlurPassQualitySelect(v2fDouble input, float4 separableBlurDirection, int useGaussianWeights)
		{
			float2 uvs = UnityStereoTransformScreenSpaceTex(input.uv[1]);

			float4 pixelColor = tex2Dlod(_MainTex, float4(uvs, 0, 0)).rgba;
			float pixelDepth = -pixelColor.a * getFarPlane();

			float cocDiameter = tex2Dlod(circleOfConfusionTexture, float4(uvs, 0, 0)).r * maxCoC;

			if (cocDiameter < minTexelSize) return pixelColor;	

			int pixelsCoverage = (cocDiameter / (minTexelSizeDofQuality)) * dofQualityCoeff;

			if (pixelsCoverage < 10)
				return bokehSeparableBlurPass(input, separableBlurDirection, pixelDepth, cocDiameter, 5, useGaussianWeights);

			if (pixelsCoverage < 20)
				return bokehSeparableBlurPass(input, separableBlurDirection, pixelDepth, cocDiameter, 7, useGaussianWeights);

			if (pixelsCoverage < 30)
				return bokehSeparableBlurPass(input, separableBlurDirection, pixelDepth, cocDiameter, 9, useGaussianWeights);

			if (pixelsCoverage < 40)
				return bokehSeparableBlurPass(input, separableBlurDirection, pixelDepth, cocDiameter, 11, useGaussianWeights);

			if (pixelsCoverage < 50)
				return bokehSeparableBlurPass(input, separableBlurDirection, pixelDepth, cocDiameter, 15, useGaussianWeights);

			if (pixelsCoverage < 80 || dofQuality == 1)
				return bokehSeparableBlurPass(input, separableBlurDirection, pixelDepth, cocDiameter, 19, useGaussianWeights);

			return bokehSeparableBlurPass(input, separableBlurDirection, pixelDepth, cocDiameter, 25, useGaussianWeights);
		}

		half4 bokehSeparableBlurWithMixingPass(v2fDouble input, float4 separableBlurDirection, int dofMixType, int isDofAuxInputHot, int useGaussianWeights, half4 previousPass)
		{
			half4 currentPass = bokehSeparableBlurPassQualitySelect(input, separableBlurDirection, useGaussianWeights);
			float lumaCurrent = luma(currentPass.rgb);
			half4 result = currentPass;
			float pixelDepth;
			half4 sourceColor;
			float Y;

			if (isDofAuxInputHot != 0) {
				float lumaPrevious = luma(previousPass.rgb);

				if (dofMixType == 2) {
					if (lumaPrevious < lumaCurrent) result = previousPass;
				}
				else {
					if (lumaPrevious > lumaCurrent) result = previousPass;
				}
			}

			//if (isDofLastPass != 0) {

			//	if (isDofLumaFilter != 0 || isDofPushHighlights != 0) {
			//		sourceColor = tex2Dlod(dofInputTexture, float4(input.uv[0], 0, 0)).rgba;
			//	}

			//	if (isDofLumaFilter != 0 || isDofColortint != 0) {

			//		if (isDofLumaFilter != 0 || isDofColortint != 0) {
			//			Y = luma(result.rgb);
			//		}

			//		if (dofRange == 1 || dofRange == 2) {
			//			if (isDofPushHighlights != 0 || isDofColortint != 0) {
			//				if (useGBuffer == 0) {
			//					pixelDepth = DecodeFloatRG(tex2Dlod(_CameraDepthNormalsTexture, float4(input.uv[0], 0, 0)).zw);
			//				}
			//				else {
			//					pixelDepth = Linear01Depth(tex2Dlod(_CameraDepthTexture, float4(input.uv[0], 0, 0)).r);
			//				}
			//			}
			//		}

			//		if (isDofPushHighlights != 0) {
			//			float3 powResult = pow(result.rgb, dofPower);
			//			result.rgb = lerp(result.rgb, powResult, 1.0f - pow(pixelDepth, dofAmount));
			//		}

			//		if (isDofColortint != 0) {

			//			float colorTintWeight = result.a;

			//			if (dofRange == 1) {
			//				// Foreground bokeh only
			//				if (-pixelDepth * _ProjectionParams.z < focalPlaneDistance) colorTintWeight = 0.0f;
			//			}

			//			if (dofRange == 2) {
			//				// Background bokeh only
			//				if (-pixelDepth * _ProjectionParams.z > focalPlaneDistance) colorTintWeight = 0.0f;
			//			}

			//			dofColorTint = lerp(dofColorTint, lerp(dofColorTint, 1.0f, saturate(Y)), dofColorTint.a);
			//			result.rgb *= lerp(Y, dofColorTint.rgb, colorTintWeight);
			//		}

			//		// Filter by luminance
			//		if (isDofLumaFilter != 0) {

			//			Y = (Y - (DofLumaThreshold - DofLumaKneeWidth)) * DofLumaTwiceKneeWidthRcp;
			//			float x = min(1.0f, max(0.0f, Y));
			//			float n = ((-pow(x, DofLumaKneeLinearity) + 1));

			//			result = lerp(result, sourceColor, n);
			//		}

			//	}

			//}

			return result;
		}

		static const float gauss3x3[9] = { 0.0625f, 0.125f, 0.0625f, 0.125f, 0.25f, 0.125f, 0.0625f, 0.125f, 0.0625f };

		half4 bokehUpscale(v2fDouble input) {

			float2 uvs = UnityStereoTransformScreenSpaceTex(input.uv[0]);

			float4 unblurred = tex2Dlod(dofInputTexture, float4(uvs, 0, 0));
			float coc = tex2Dlod(circleOfConfusionTexture, float4(uvs, 0, 0)).r * maxCoC;

			float3 dof = float3(0.0f, 0.0f, 0.0f);
			int idx = 0;
		
			WFORCE_UNROLL(3)
			for (int i = -1; i <= 1; i++) {
				WFORCE_UNROLL(3)
				for (int j = -1; j <= 1; j++) {

					float2 offset = input.uv[1] + float2(float(i), float(j)) * dofFastPassTexelSize;

					float3 tap = tex2Dlod(_MainTex, float4(UnityStereoTransformScreenSpaceTex(offset), 0, 0)) * gauss3x3[idx++];

					dof += tap.rgb;
				}
			}

			// Mix with unblurred input
			float3 result = lerp(unblurred, dof, saturate((coc - minTexelSize) * fastBlurMixingCoeff));

			return half4(result, 1.0f);
		}

		// 2. Chromatic Aberration ====================================================

		float3 getHue(float u) {

			float temphue = lerp(chromaSpectrumMin, chromaSpectrumMax, u);

			float3 hue = float3(abs(temphue * 6.0f - 3.0f) - 1.0f,
				2.0f - abs(temphue * 6.0f - 2.0f),
				2.0f - abs(temphue * 6.0f - 4.0f));

			return saturate(hue);
		}

		half4 chromaAberration(v2fDouble input, float2 dir, float4 color, int samplesCount) : SV_Target
		{

			//if (mustFlipChromaticAberrationsInput != 0)
			//	input.uv[0].y = 1.0f - input.uv[0].y;

			float3 acc = float3(0, 0, 0);
			float3 weight = float3(0, 0, 0);

			float u = 0.0f;
			float uIncr = (1.0f / (float)(samplesCount - 1));

			WFORCE_UNROLL(50)
			for (int i = 0; i < samplesCount; i++) {
				
				//float4 hue = tex2Dlod(chromaSamplesTexture, float4(u, 0.0f, 0.0f, 0.0f)); //< .a channel contains offset
				float4 hue = float4(getHue(u), (u - 0.5f) * 10.0f); //< .a channel contains offset
				
				float2 offset = dir * hue.a;
			
				float3 tex = tex2Dlod(_MainTex, float4(UnityStereoTransformScreenSpaceTex(input.uv[0] + offset), 0, 0)).rgb;
					
				// Mask by hue
				acc += hue * tex;
				weight += hue;
				u += uIncr;
			}
			
			// Mix with original and return
			return float4(lerp(color.rgb, acc / weight, chromaWeight), color.a);
		}

		half4 chromaAberrationAutoQuality(v2fDouble input) : SV_Target
		{
			float4 color = tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(input.uv[0]));
			float2 inputUVs = input.uv[0];

			if (isAnamorphicChroma == 0) {
				inputUVs.y = (((input.uv[0].y * 2.0f - 1.0f) * aspectRatio) + 1.0f) * 0.5f;
			}

			// get distance from screen center
			float2 center = float2(0.5f, 0.5f);
			float2 dir = center - inputUVs;

			// Raise to the power of "radialness"
			float distanceFromCenter = pow(length(dir), chromaRadialness);
			dir = normalize(dir) * (chromaTexSize * distanceFromCenter);

			// Select between 3-50 samples
			float screenSpaceRadius = chromaTexSize * distanceFromCenter * chromaQuality;
			int pixelsCoverage = screenSpaceRadius / minTexelSize;

			if (pixelsCoverage < 2)
				return chromaAberration(input, dir, color, 3);

			if (pixelsCoverage < 5)
				return chromaAberration(input, dir, color, 7);

			if (pixelsCoverage < 10)
				return chromaAberration(input, dir, color, 13);

			if (pixelsCoverage < 15)
				return chromaAberration(input, dir, color, 17);

			if (pixelsCoverage < 25)
				return chromaAberration(input, dir, color, 27);

			if (pixelsCoverage < 35)
				return chromaAberration(input, dir, color, 39);

			return chromaAberration(input, dir, color, 50);
		}

		// 3. Vignette ============================================================

		half4 vignette(v2fDouble input) : SV_Target
		{
			//if (mustFlipVignetteInput != 0)
			//	input.uv[0].y = 1.0f - input.uv[0].y;
			
			float4 color = tex2D(_MainTex, UnityStereoTransformScreenSpaceTex(input.uv[0]));
				
			if (isAnamorphicVignette == 0) {
				input.uv[0].y = (((input.uv[0].y * 2.0f - 1.0f) * aspectRatio) + 1.0f) * 0.5f;
			}

			// get distance from  screen center
			float2 dir = vignetteCenter - input.uv[0];
					
			// Raise to the power of "radialness"
			float u = saturate((length(dir) - vignetteMinDistance) / (vignetteMaxDistance - vignetteMinDistance));
			u = saturate(pow(u, vignetteFalloff));

			if (vignetteMode == 1) {
				float result = lerp(vignetteMax, vignetteMin, u);

				if (debugMode == 1) return result;

				return float4(color.rgb * result, color.a);
			}

			if (vignetteMode == 2) {
				float4 mixingColor = lerp(vignetteInnerColor, vignetteOuterColor, u);

				if (debugMode == 1) return mixingColor;

				return float4(lerp(color.rgb, mixingColor.rgb, mixingColor.a), color.a);
			}

			//if (vignetteMode == 3) {
				u = lerp(vignetteSaturationMin, vignetteSaturationMax, u);

				if (debugMode == 1) return u;

				color.rgb = setSaturation(color.rgb, u);
				return color;
			//}
		}

		// 4. Bloom ===============================================================

		half4 bloomPrepass(v2fDouble input) : SV_Target
		{
			//if (mustFlipBloomInput != 0)
			//	input.uv[1].y = 1.0f - input.uv[1].y;

			float4 tex = tex2Dlod(_MainTex, float4(input.uv[1], 0, 0));

			return max(tex - bloomThreshold, 0.0f);
		}

		BloomPrepassOutput bloomPrepassAntiflicker(v2fDouble input) : SV_Target
		{
			BloomPrepassOutput result;

			//if (mustFlipBloomInput != 0)
			//	input.uv[1].y = 1.0f - input.uv[1].y;
			
			float2 uvs = input.uv[1];

			float4 tex = tex2Dlod(bloomPrepassSource, float4(uvs, 0, 0));
		
			float currentLuma = luma(tex.rgb);
			float4 lumaHistory3 = tex2Dlod(lumaHistoryBuffer3, float4(uvs, 0, 0));

			float historyCoeff = dot(lumaHistory3, lumaHistoryWeights3);
			if (lumaHistoryLength > 1) historyCoeff += dot(tex2Dlod(lumaHistoryBuffer2, float4(uvs, 0, 0)), lumaHistoryWeights2);
			if (lumaHistoryLength > 2) historyCoeff += dot(tex2Dlod(lumaHistoryBuffer, float4(uvs, 0, 0)), lumaHistoryWeights);

			float lumaCoeff = (historyCoeff + currentLumaWeight * currentLuma) / currentLuma;

			if (bloomAntiflickerFade == 2) lumaCoeff = max(lumaCoeff, 1.0f);
			if (bloomAntiflickerFade == 1) lumaCoeff = min(lumaCoeff, 1.0f);

			result.color = max(tex * lumaCoeff - bloomThreshold, 0.0f);
			result.lumaHistory = (lumaHistory3 * (1.0f - lumaHistoryMask)) + (lumaHistoryMask * currentLuma);

			return result;
		}
			
		half4 bloomMainPass(v2fDouble input, float2 bloomTexSizeRcp, int isBloomScalingFix, int isBloomMixingPass, int isBloomXPass, int bloomSamplesCount) : SV_Target
		{
			float4 result = float4(0.0f, 0.0f, 0.0f, 0.0f);
			int idx = 0;
			
			WFORCE_UNROLL(17)
			for (int i = -bloomSamplesCount; i <= bloomSamplesCount; ++i) {
				float2 offset;

				if (isBloomXPass != 0) {
					offset = float2(float(i) * bloomTexSizeRcp.x, 0);
				} else {
					offset = float2(0, float(i) * bloomTexSizeRcp.y);
				}
				
				result += tex2Dlod(_MainTex, float4(UnityStereoTransformScreenSpaceTex(input.uv[1] + offset), 0, 0)) * bloomGauss[idx++].x;
			}

			if (isBloomMixingPass == 0) {
				if (isBloomScalingFix == 1) {
					result.a = bottomLevelRadius;
				} else {
					result.a = 1.0f;
				}

				return result;
			} else {

				//if (mustFlipBloomInput != 0)
				//	input.uv[0].y = 1.0f - input.uv[0].y;

				float4 tex = tex2Dlod(bloomSourceTexture, float4(UnityStereoTransformScreenSpaceTex(input.uv[0]), 0, 0));

				result.rgb = setSaturation(result.rgb, bloomSaturation);
				return tex + (pow(result, bloomPower) * bloomMultiplier);
			}
		}

		half4 bloomMainPassQualitySelect(v2fDouble input, float2 bloomTexSizeRcp, int isBloomScalingFix, int isBloomMixingPass, int isBloomXPass) : SV_Target
		{

			if (bloomSize == 7) {
				return bloomMainPass(input, bloomTexSizeRcp, isBloomScalingFix, isBloomMixingPass, isBloomXPass, 3);
			}

			if (bloomSize == 11) {
				return bloomMainPass(input, bloomTexSizeRcp, isBloomScalingFix, isBloomMixingPass, isBloomXPass, 5);
			}

			return bloomMainPass(input, bloomTexSizeRcp, isBloomScalingFix, isBloomMixingPass, isBloomXPass, 7);
		}

		// 5. Color Correction ===============================================================

		float3 colorCorrectionLut(float2 uvs, float3 input)
		{
			
			float blueOffsetPixels = input.b * lutScale.b;
			float blueL = floor(blueOffsetPixels);
			float blueH = blueL + 1.0f;
			float blueWeight = blueOffsetPixels - blueL;

			float2 lutUVs = input.rg * lutScale.rg + lutOffset;

			float3 output = lerp(tex2Dlod(lutTexture, float4(lutUVs.x + blueL * lutScale.a, lutUVs.y, 0, 0)),
								 tex2Dlod(lutTexture, float4(lutUVs.x + blueH * lutScale.a, lutUVs.y, 0, 0)),
								 blueWeight);

			return lerp(input.rgb, output, colorCorrectionIntensity);
		}

		// ACES curve fitting was originally written by Stephen Hill (@self_shadow), used here with his kind permission

		// sRGB => XYZ => D65_2_D60 => AP1 => RRT_SAT
		static const float3x3 ACESInputMat =
		{
			{ 0.59719, 0.35458, 0.04823 },
			{ 0.07600, 0.90834, 0.01566 },
			{ 0.02840, 0.13383, 0.83777 }
		};

		// ODT_SAT => XYZ => D60_2_D65 => sRGB
		static const float3x3 ACESOutputMat =
		{
			{ 1.60475, -0.53108, -0.07367 },
			{ -0.10208,  1.10813, -0.00605 },
			{ -0.00327, -0.07276,  1.07602 }
		};

		float3 RRTAndODTFit(float3 v)
		{
			float3 a = v * (v + 0.0245786f) - 0.000090537f;
			float3 b = v * (0.983729f * v + 0.4329510f) + 0.238081f;
			return a / b;
		}

		float3 aces(float3 color) {

			color = mul(ACESInputMat, color);

			// Apply RRT and ODT
			color = RRTAndODTFit(color);

			color = mul(ACESOutputMat, color);

			// Clamp to [0, 1]
			color = saturate(color);

			return color;
		}

		float4 colorGrading(v2fDouble i, int acesEnabled, int lutEnabled) : SV_Target
		{
			float4 input = tex2Dlod(_MainTex, float4(i.uv[0], 0, 0));

			if (acesEnabled != 0) {

#ifdef UNITY_COLORSPACE_GAMMA
				// ACES needs to be done in linear
				input.rgb = gammaToLinear(input.rgb);
#endif
				// Apply color temperature & tint
				input.rgb = input.rgb * tonemappingColorBalance;

				// Adjust exposure
				input.rgb *= exposureAdjustment;

				// Apply ACES curve
				input.rgb = lerp(saturate(input.rgb), aces(input.rgb), tonemappingIntensity);

				// Adjust contrast & saturation
				input.rgb = saturate((tonemappingContrast * (input.rgb - 0.18f)) + 0.18f); // 0.18 = Mid level grey
				input.rgb = setSaturation(input.rgb, tonemappingSaturation);
				
				// Apply gamma 
				input.rgb = pow(input.rgb, tonemappingGamma);

#ifdef UNITY_COLORSPACE_GAMMA
				// Return to gamma
				input.rgb = linearToGamma(input.rgb);
#endif
			} else {
				input.rgb = saturate(input.rgb);
			}

			if (lutEnabled != 0) {
	#ifndef UNITY_COLORSPACE_GAMMA
				// LUTs are in sRGB, convert to gamma and back when we're in linear
				input.rgb = linearToGamma(input.rgb);
	#endif

				input.rgb = colorCorrectionLut(i.uv[0], input);

	#ifndef UNITY_COLORSPACE_GAMMA
				input.rgb = gammaToLinear(input.rgb);
	#endif
			}

			return input;
		}

		float4 generateColorLUT(float2 uv) {

			float red = LUTGeneratorRedValues[(int)((frac(uv.x * manualLUTTextureSize)) * manualLUTTextureSize)];
			float green = LUTGeneratorGreenValues[(int)(uv.y * manualLUTTextureSize)];
			float blue = LUTGeneratorBlueValues[(int)floor(uv.x * manualLUTTextureSize)];

			return float4(red, green, blue, 1);
		}

		// ========================================================================
		// Utilities
		// ========================================================================

		half4 displayDebug(v2fDouble input)
		{	
			if (debugMode == 1) return vignette(input);

			return float4(0.0f, 0.0f, 0.0f, 1.0f);
		}

	ENDCG
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		// 0 - Chromatic aberration only
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return chromaAberrationAutoQuality(i); }
			ENDCG}

		// 1 - Vignette only
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return vignette(i); }
			ENDCG}

		// 2 - Bloom prepass
		Pass{CGPROGRAM
			#pragma vertex vertDoubleSPSRAware #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomPrepass(i); }
			ENDCG}

		// 3 - Bloom prepass with antiflicker
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleSPSRAware #pragma fragment frag
			BloomPrepassOutput frag(v2fDouble i) { return bloomPrepassAntiflicker(i); }
			ENDCG }

		// 4 - Bloom prepass with antiflicker cmd buffer
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleTexCopy #pragma fragment frag
			BloomPrepassOutput frag(v2fDouble i) { return bloomPrepassAntiflicker(i); }
			ENDCG }
		
		// 5 - BloomMainPass0X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp0, 0, 0, 1); }
			ENDCG}

		// 6 - BloomMainPass0MixingY
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp0, 0, 1, 0); }
			ENDCG }

		// 7 - BloomMainPass1X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp1, 0, 0, 1); }
			ENDCG}

		// 8 - BloomMainPass1Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp1, 0, 0, 0); }
			ENDCG }

		// 9 - BloomMainPass2X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp2, 0, 0, 1); }
			ENDCG}

		// 10 - BloomMainPass2Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp2, 0, 0, 0); }
			ENDCG }

		// 11 - BloomMainPass3X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp3, 0, 0, 1); }
			ENDCG}

		// 12 - BloomMainPass3Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp3, 0, 0, 0); }
			ENDCG }

		// 13 - BloomMainPass4X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp4, 0, 0, 1); }
			ENDCG}

		// 14 - BloomMainPass4Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp4, 0, 0, 0); }
			ENDCG }

		// 15 - BloomMainPass5X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp5, 0, 0, 1); }
			ENDCG}

		// 16 - BloomMainPass5Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp5, 0, 0, 0); }
			ENDCG }

		// 17 - BloomMainPass6X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp6, 0, 0, 1); }
			ENDCG}

		// 18 - BloomMainPass6Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp6, 0, 0, 0); }
			ENDCG }

		// 19 - BloomMainPass7X
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcp7, 0, 0, 1); }
			ENDCG}

		// 20 - BloomMainPass7Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcp7, 0, 0, 0); }
			ENDCG }

		// 21 - BloomMainPassBottomX
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bloomMainPassQualitySelect(i, bloomTexSizeRcpBottom, 1, 0, 1); }
			ENDCG}

		// 22 - BloomMainPassBottomY
		Pass{
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bloomMainPassQualitySelect(i, bloomTexSizeRcpBottom, 1, 0, 0); }
			ENDCG }

		// 23 - DistortionOnly
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return distort(i); }
			ENDCG}

		// 24 - CoCMapPass
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			CoCOutput frag(v2fDouble i) { return generateCoCMap(i); }
			ENDCG}

		// 25 - CoCMapPassCmdBuffer
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleTexCopy #pragma fragment frag
			CoCOutput frag(v2fDouble i) { return generateCoCMap(i); }
			ENDCG }

		// 26 - CocBlurPass0X
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return blurCoCMap(i, cocBlurTexSizeRcp0, 1, 0); }
			ENDCG }
							
		// 27 - CocBlurPass0Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return blurCoCMap(i, cocBlurTexSizeRcp0, 0, 0); }
			ENDCG }

		// 28 - CocBlurPass0MixingY
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return blurCoCMap(i, cocBlurTexSizeRcp0, 0, 1); }
			ENDCG }

		// 29 - CocBlurPass1X
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return blurCoCMap(i, cocBlurTexSizeRcp1, 1, 0); }
			ENDCG }

		// 30 - CocBlurPass1Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return blurCoCMap(i, cocBlurTexSizeRcp1, 0, 0); }
			ENDCG }

		// 31 - CocBlurPass2X
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return blurCoCMap(i, cocBlurTexSizeRcp2, 1, 0); }
			ENDCG }

		// 32 - CocBlurPass2Y
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return blurCoCMap(i, cocBlurTexSizeRcp2, 0, 0); }
			ENDCG }

		// 33 - CocBlurUpscale
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return upscaleCoCMap(i); }
			ENDCG }
		
		// 34 - DepthOfFieldSeparableBlurPass - 0
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurPassQualitySelect(i, separableBlurDirections[0], 0); }
			ENDCG}

		// 35 - DepthOfFieldSeparableBlurPass - 1
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurPassQualitySelect(i, separableBlurDirections[1], 0); }
			ENDCG}

		// 36 - DepthOfFieldSeparableBlurPass - 2
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurPassQualitySelect(i, separableBlurDirections[2], 0); }
			ENDCG}

		// 37 - DepthOfFieldSeparableBlurPass - 3
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurPassQualitySelect(i, separableBlurDirections[3], 0); }
			ENDCG}

		// 38 - DepthOfFieldSeparableBlurPass - 4
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurPassQualitySelect(i, separableBlurDirections[4], 0); }
			ENDCG}

		// 39 - DepthOfFieldSeparableBlurPass - 5
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurPassQualitySelect(i, separableBlurDirections[5], 0); }
			ENDCG}
			
		// 40 - DepthOfFieldSeparableGaussianBlurPass0 - 0
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bokehSeparableBlurPassQualitySelect(i, separableBlurDirections[0], 1); }
			ENDCG }
		
		// 41 - DepthOfFieldSeparableBlurWithMixingPass - 1
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurWithMixingPass(i, separableBlurDirections[blurMixingPass1DirectionVectorIdx], dofMixTypeMixingPass1, isDofAuxInputHotPass1, 0, tex2Dlod(bokehAuxTexture1, float4(UnityStereoTransformScreenSpaceTex(i.uv[0]), 0, 0)).rgba); }
			ENDCG}

		// 42 - DepthOfFieldSeparableBlurWithMixingPass - 2
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return bokehSeparableBlurWithMixingPass(i, separableBlurDirections[blurMixingPass2DirectionVectorIdx], dofMixTypeMixingPass2, isDofAuxInputHotPass2, 0, tex2Dlod(bokehAuxTexture2, float4(UnityStereoTransformScreenSpaceTex(i.uv[0]), 0, 0)).rgba); }
			ENDCG}

		// 43 - DepthOfFieldSeparableGaussianBlurWithMixingPass1 - 1
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bokehSeparableBlurWithMixingPass(i, separableBlurDirections[blurMixingPass1DirectionVectorIdx], dofMixTypeMixingPass1, isDofAuxInputHotPass1, 1, tex2Dlod(bokehAuxTexture1, float4(UnityStereoTransformScreenSpaceTex(i.uv[0]), 0, 0)).rgba); }
			ENDCG }

		// 44 - DepthOfFieldFastMixingPass
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return bokehUpscale(i); }
			ENDCG }

		// 45 - DebugDisplayPass
		Pass{CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return displayDebug(i); }
			ENDCG}

		// 46 - TexCopy
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleTexCopy #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target { return tex2Dlod(texCopySource, float4(i.uv[0], 0, 0)); }
		ENDCG }

		// 47 - TexCopyImageEffectSPSR
		Pass{ CGPROGRAM
			#pragma vertex vertDouble #pragma fragment frag
			half4 frag(v2fDouble i) : SV_Target{ return tex2Dlod(texCopySource, float4(i.uv[0], 0, 0)); }
			ENDCG }
			
		// 48 - ColorCorrectionLut
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleSPSRAware #pragma fragment frag
			float4 frag(v2fDouble i) : SV_Target{ return colorGrading(i, 0, 1); }
			ENDCG }
			
		// 48 - ColorCorrectionLutAces
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleSPSRAware #pragma fragment frag
			float4 frag(v2fDouble i) : SV_Target{ return colorGrading(i, 1, 1); }
			ENDCG }

		// 49 - ColorCorrectionAces
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleSPSRAware #pragma fragment frag
			float4 frag(v2fDouble i) : SV_Target{ return colorGrading(i, 1, 0); }
			ENDCG }

		// 50 - GenerateColorLUT
		Pass{ CGPROGRAM
			#pragma vertex vertDoubleSPSRAware #pragma fragment frag
			float4 frag(v2fDouble i) : SV_Target{ return generateColorLUT(i.uv[0]); }
			ENDCG }
			
	}
}
