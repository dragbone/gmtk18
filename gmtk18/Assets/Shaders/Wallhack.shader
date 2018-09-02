Shader "Wallhack"{

     Properties
     {
         _Color ("Color", Color) = (1,1,1,1)
     }
     
     Category
     {
         SubShader
         {
             Tags { "Queue"="Geometry+1"
                 "RenderType"="Transparent"}
      
             Pass
             {
                 Name "Wallhack"
                 Blend SrcAlpha OneMinusSrcAlpha
                 ZWrite Off
                 ZTest Always
                 Lighting Off
                 Color [_Color]
                 
                 /*CGPROGRAM
                 #pragma vertex vert            
                 #pragma fragment frag
                 #pragma fragmentoption ARB_precision_hint_fastest
      
                 float4 vert(float4 pos : POSITION) : SV_POSITION
                 {
                     float4 viewPos = UnityObjectToClipPos(pos);
                     return viewPos;
                 }
      
                  half4 _Color;
                     half4 frag(float4 pos : SV_POSITION) : COLOR
                 {
                     return half4(_Color.x, _Color.y, _Color.z, 1.0);
                 }
      
                 ENDCG*/
             }
         }
     }
      
     FallBack "Diffuse"
 }