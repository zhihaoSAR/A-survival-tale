Shader "Hidden/Custom/ColorBlindCorrection"
{
	HLSLINCLUDE

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

	half3 LMS(half3 input) {
		half3 lms = 0;
		lms[0] = (17.8824 * input.r) + (43.5161 * input.g) + (4.11935 * input.b);
		lms[1] = (3.45565 * input.r) + (27.1554 * input.g) + (3.86714 * input.b);
		lms[2] = (0.0299566 * input.r) + (0.184309 * input.g) + (1.46709 * input.b);
		return lms;
	}

	half3 Correction(float3 input, float3 dlms) {
		half3 err = 0;
		err.r = (0.0809444479 * dlms[0]) + (-0.130504409 * dlms[1]) + (0.116721066 * dlms[2]);
		err.g = (-0.0102485335 * dlms[0]) + (0.0540193266 * dlms[1]) + (-0.113614708 * dlms[2]);
		err.b = (-0.000365296938 * dlms[0]) + (-0.00412161469 * dlms[1]) + (0.693511405 * dlms[2]);
		err = (input - err);
		float3 correction = 0;
		correction.g = (err.r * 0.7) + (err.g * 1.0);
		correction.b = (err.r * 0.7) + (err.b * 1.0);
		return input + correction;
	}

	half4 FragProtanopia(VaryingsDefault i) : SV_Target
	{
		half4 input = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		half3 lms = LMS(input.rgb);
		float3 dlms = 0;
		dlms[0] = 0.0 * lms[0] + 2.02344 * lms[1] + -2.52581 * lms[2];
		dlms[1] = 0.0 * lms[0] + 1.0 * lms[1] + 0.0 * lms[2];
		dlms[2] = 0.0 * lms[0] + 0.0 * lms[1] + 1.0 * lms[2];
		return half4(Correction(input, dlms), input.a);
	}

		half4 FragDeuteranopia(VaryingsDefault i) : SV_Target
	{
		half4 input = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		half3 lms = LMS(input.rgb);
		float3 dlms = 0;
		dlms[0] = 1.0 * lms[0] + 0.0 * lms[1] + 0.0 * lms[2];
		dlms[1] = 0.494207 * lms[0] + 0.0 * lms[1] + 1.24827 * lms[2];
		dlms[2] = 0.0 * lms[0] + 0.0 * lms[1] + 1.0 * lms[2];
		return half4(Correction(input, dlms), input.a);
	}

		half4 FragTritanopia(VaryingsDefault i) : SV_Target
	{
		half4 input = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		half3 lms = LMS(input.rgb);
		float3 dlms = 0;
		dlms[0] = 1.0 * lms[0] + 0.0 * lms[1] + 0.0 * lms[2];
		dlms[1] = 0.0 * lms[0] + 1.0 * lms[1] + 0.0 * lms[2];
		dlms[2] = -0.395913 * lms[0] + 0.801109 * lms[1] + 0.0 * lms[2];
		return half4(Correction(input, dlms), input.a);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDefault
				#pragma fragment FragProtanopia
			ENDHLSL
		}

			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDefault
				#pragma fragment FragDeuteranopia
			ENDHLSL
		}

			Pass
		{
			HLSLPROGRAM
				#pragma vertex VertDefault
				#pragma fragment FragTritanopia
			ENDHLSL
		}
	}
}