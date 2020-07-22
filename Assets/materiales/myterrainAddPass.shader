Shader "Custom/myterrainAddPass"{
	Properties{
		// used in fallback on old cards & base map
		[HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "white" {}
		_Color("Main Color", Color) = (1,0,0,1)
	}
	CGINCLUDE
#pragma surface surf Lambert decal:add vertex:SplatmapVert finalcolor:SplatmapFinalColor finalprepass:SplatmapFinalPrepass finalgbuffer:SplatmapFinalGBuffer fullforwardshadows nometa
#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
#pragma multi_compile_fog

#define TERRAIN_SPLAT_ADDPASS
#include "TerrainSplatmapCommon.cginc"

		fixed4 _Color;
		void surf(Input IN, inout SurfaceOutput o)
	{
		half4 splat_control;
		half weight;
		fixed4 mixedDiffuse;
		SplatmapMix(IN, splat_control, weight, mixedDiffuse, o.Normal);
		o.Albedo = mixedDiffuse.rgb*_Color;
		o.Alpha = weight;
	}
	ENDCG

		Category{
			Tags {
				"Queue" = "Geometry-99"
				"IgnoreProjector" = "True"
				"RenderType" = "Opaque"
			}
		// TODO: Seems like "#pragma target 3.0 _NORMALMAP" can't fallback correctly on less capable devices?
		// Use two sub-shaders to simulate different features for different targets and still fallback correctly.
		SubShader { // for sm3.0+ targets
			CGPROGRAM
				#pragma target 3.0
				#pragma multi_compile_local __ _NORMALMAP
			ENDCG
		}
		SubShader { // for sm2.0 targets
			CGPROGRAM
			ENDCG
		}
	}

		Fallback off
}