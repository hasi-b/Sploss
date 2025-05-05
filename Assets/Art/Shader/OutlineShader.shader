Shader "Custom/SpriteOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.01
        _OutlineEnabled ("Outline Enabled", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _OutlineEnabled;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                if (_OutlineEnabled > 0.5) {
                    // Sample the texture in the surrounding pixels
                    float2 pixelSize = _OutlineWidth * _MainTex_TexelSize.xy;
                    
                    float alpha = col.a;
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(pixelSize.x, 0)).a);
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(-pixelSize.x, 0)).a);
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(0, pixelSize.y)).a);
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(0, -pixelSize.y)).a);
                    
                    // Diagonals for better outline
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(pixelSize.x, pixelSize.y)).a);
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(-pixelSize.x, pixelSize.y)).a);
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(pixelSize.x, -pixelSize.y)).a);
                    alpha = max(alpha, tex2D(_MainTex, i.uv + float2(-pixelSize.x, -pixelSize.y)).a);
                    
                    // Creating outline effect
                    if (alpha > 0.1 && col.a < 0.1) {
                        return _OutlineColor;
                    }
                }
                
                return col;
            }
            ENDCG
        }
    }
}