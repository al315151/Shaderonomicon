// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ColorBSShader"
{
	SubShader
	{ 
      Pass { 
         CGPROGRAM 
 
         #pragma vertex vert // vert function is the vertex shader 
         #pragma fragment frag // frag function is the fragment shader
 
		 fixed4 _CustomColor;
		


         // for multiple vertex output parameters an output structure 
         // is defined:
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : TEXCOORD0;
			float4 wPos : TEXCOORD1;
         };
 
		 float3 RGBtoHSV(float3 RGB)
		 {
			 float Epsilon = 1e-10;
			 // Based on work by Sam Hocevar and Emil Persson, conversion from RGB to HCV.
			  float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0/3.0) : float4(RGB.gb, 0.0, -1.0/3.0);
			  float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
			  float C = Q.x - min(Q.w, Q.y);
			  float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);

			  // From http://www.chilliant.com/rgb2hsv.html, conversion from HCV to HSV.
			  float L = Q.x - C * 0.5;
			  float S = C / (1 - abs(L * 2 - 1) + Epsilon);
			  return float3(H,S,L);		 
		 }

		 float3 HSVtoRGB (float3 HSV)
		 {
		  // From http://www.chilliant.com/rgb2hsv.html, conversion from HUE to RGB.
			float R = abs (HSV.x * 6 - 3) - 1;
			float G = 2 - abs (HSV.x * 6 - 2);
			float B = 2 - abs (HSV.x * 6 - 4);
			
			float HSVpostHUE = saturate(float3(R,G,B));

			 // From http://www.chilliant.com/rgb2hsv.html, conversion from HSV to RGB.
			return ((HSVpostHUE - 1) * HSV.y + 1) * HSV.z;

		 }

         vertexOutput vert (float4 vertexPos : POSITION) 
            // vertex shader 
         {
            vertexOutput output; // we don't need to type 'struct' here
 
            output.pos =  UnityObjectToClipPos(vertexPos);
			output.wPos = mul(unity_ObjectToWorld, vertexPos);
            //output.col = vertexPos + float4(0.5, 0.5, 0.5, 0.0); //RGB CUBE EN ESCENA 
			output.col = _CustomColor;


               // Here the vertex shader writes output data
               // to the output structure. We add 0.5 to the 
               // x, y, and z coordinates, because the 
               // coordinates of the cube are between -0.5 and
               // 0.5 but we need them between 0.0 and 1.0. 
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR // fragment shader
         {
			float3 HSVColor = RGBtoHSV(input.col.xyz);
			float3 newRGB = float3 (HSVColor.x - 0.5,HSVColor.y -0.5, HSVColor.z - 0.5 );
			input.col = float4(newRGB.x, newRGB.y, newRGB.z, input.col.w);


            return input.col; 
               // Here the fragment shader returns the "col" input 
               // parameter with semantic TEXCOORD0 as nameless
               // output parameter with semantic COLOR.
         }
 
         ENDCG  
      }
   }
}
