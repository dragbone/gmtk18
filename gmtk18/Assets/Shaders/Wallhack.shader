Shader "Wallhack"{

     Properties
     {
         _Color ("Color", Color) = (1,1,1,1)
         _Color2 ("Color2", Color) = (1,1,1,1)
     }
     
     Category
     {
         SubShader
         {
             Tags { "Queue"="Geometry+1"
                 "RenderType"="Transparent"}
      
             Pass
             {
                 Blend SrcAlpha OneMinusSrcAlpha
                 ZWrite Off
                 ZTest Greater
                 Lighting Off
                 Color [_Color]
             }
         }
     }
      
     FallBack "Diffuse"
 }