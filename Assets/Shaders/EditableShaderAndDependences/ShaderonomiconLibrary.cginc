//Custom built-in shader source, mainly based in UnityCG.cginc
//Hope that it will work... somehow.

#ifndef SHADEROMONICON_LIBRARY
#define SHADERONOMICON_LIBRARY

#define SHADERONOMICON_PI            3.14159265359f
#define SHADERONOMICON_TWO_PI        6.28318530718f
#define SHADERONOMICON_FOUR_PI       12.56637061436f
#define SHADERONOMICON_INV_PI        0.31830988618f
#define SHADERONOMICON_INV_TWO_PI    0.15915494309f
#define SHADERONOMICON_INV_FOUR_PI   0.07957747155f
#define SHADERONOMICON_HALF_PI       1.57079632679f
#define SHADERONOMICON_INV_HALF_PI   0.636619772367f

#include "UnityShaderVariables.cginc"
#include "UnityShaderUtilities.cginc"
#include "UnityInstancing.cginc"

//-----------Helper Functions that I use (...maybe...) Font: UnityCG.cginc -------------------


//-------Unity World To Clip Pos functions ----------

// Tranforms position from world to homogenous space
inline float4 UnityWorldToClipPos( in float3 pos )
{
    return mul(UNITY_MATRIX_VP, float4(pos, 1.0));
}

// Tranforms position from view to homogenous space
inline float4 UnityViewToClipPos( in float3 pos )
{
    return mul(UNITY_MATRIX_P, float4(pos, 1.0));
}

// Tranforms position from object to camera space
inline float3 UnityObjectToViewPos( in float3 pos )
{
    return mul(UNITY_MATRIX_V, mul(unity_ObjectToWorld, float4(pos, 1.0))).xyz;
}
inline float3 UnityObjectToViewPos(float4 pos) // overload for float4; avoids "implicit truncation" warning for existing shaders
{
    return UnityObjectToViewPos(pos.xyz);
}

// Tranforms position from world to camera space
inline float3 UnityWorldToViewPos( in float3 pos )
{
    return mul(UNITY_MATRIX_V, float4(pos, 1.0)).xyz;
}

// Transforms direction from object to world space
inline float3 UnityObjectToWorldDir( in float3 dir )
{
    return normalize(mul((float3x3)unity_ObjectToWorld, dir));
}

// Transforms direction from world to object space
inline float3 UnityWorldToObjectDir( in float3 dir )
{
    return normalize(mul((float3x3)unity_WorldToObject, dir));
}

// Transforms normal from object to world space
inline float3 UnityObjectToWorldNormal( in float3 norm )
{
#ifdef UNITY_ASSUME_UNIFORM_SCALING
    return UnityObjectToWorldDir(norm);
#else
    // mul(IT_M, norm) => mul(norm, I_M) => {dot(norm, I_M.col0), dot(norm, I_M.col1), dot(norm, I_M.col2)}
    return normalize(mul(norm, (float3x3)unity_WorldToObject));
#endif
}









#endif //SHADERONOMICON_LIBRARY
















