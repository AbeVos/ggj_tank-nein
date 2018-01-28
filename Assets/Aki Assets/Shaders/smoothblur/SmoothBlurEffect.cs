using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SmoothBlurEffect : MonoBehaviour
{

    public Material material;

    //public float intensity = 0f;
    //[Range(0, 1)] 
    //public int _size;
    public int downsample = 2;

    //public float blur = 5f;

    [Range(0.0f, 10.0f)]
    public float blurSize = 3.0f;

    [Range(0.0f, 1.0f)]
    public float multiplyMix = 0.75f;

    /*
    public float multiplyMix
    {
        set
        {
            multiplyMix = value;
            if (this.material != null)
                this.material.SetFloat("_multiplyMix", value);
        }
    }
     */
    
    // Use this for initialization
    void Awake()
    {
        //this.material = new Material(Shader.Find("Hidden/ASCIIEffect"));
       // this.material.SetVector("_symbolSize", new Vector4(sizeW, sizeH, 0, 0));
        //this.material.SetFloat("_symbolScale", symbolScale);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (this.downsample == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }





        /* --- Downscale ---------------------------------------------
         * This automatically downscales since we are rendering onto a smaller texture. */
       
        if (blurSize == 0)
        {
            int rtW = (int)(source.width / downsample);
            int rtH = (int)(source.height / downsample);

            RenderTexture rt1 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            rt1.filterMode = FilterMode.Bilinear;

            // Copies and downscale
            Graphics.Blit(source, rt1);

            /* --- Blur -------------------------------------------
             * Blur in rt2.
             */
            RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            rt2.filterMode = FilterMode.Bilinear;

            // Downsample
            Graphics.Blit(rt1, rt2);
            RenderTexture.ReleaseTemporary(rt1);

            /* --- Multiply -----------------------------------------
             */
            material.SetTexture("_SecondaryTex", rt1);
            Graphics.Blit(source, destination, material, 3);
            //material.SetTexture("_SecondaryTexture", rt2);
            //Graphics.Blit(source, destination, material, 1);
            RenderTexture.ReleaseTemporary(rt2);
        } else
        {
            /* === BLUR ============================================
             */
            float widthMod = 1.0f / (1.0f * (1 << downsample));

            material.SetVector("_Parameter", new Vector4(blurSize * widthMod, -blurSize * widthMod, 0.0f, 0.0f));
            source.filterMode = FilterMode.Bilinear;

            int rtW = source.width >> downsample;
            int rtH = source.height >> downsample;

            // Downsample
            RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

            rt.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, rt, material, 0);

            // Only one iteration
            material.SetVector("_Parameter", new Vector4(blurSize * widthMod, -blurSize * widthMod, 0.0f, 0.0f));

            // vertical blur
            RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            rt2.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt, rt2, material, 1);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;

            // horizontal blur
            rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
            rt2.filterMode = FilterMode.Bilinear;
            Graphics.Blit(rt, rt2, material, 2);
            RenderTexture.ReleaseTemporary(rt);
            rt = rt2;

            /* --- EFFECT -----------------------------------------
            */
            material.SetVector("_Parameter", new Vector4(multiplyMix,0f,0f,0f));
            material.SetTexture("_SecondaryTex", rt);
            Graphics.Blit(source, destination, material, 3);
            //material.SetTexture("_SecondaryTexture", rt2);
            //Graphics.Blit(source, destination, material, 1);
            RenderTexture.ReleaseTemporary(rt);

            /*
            int passOffs = blurType == BlurType.StandardGauss ? 0 : 2;

            for (int i = 0; i < blurIterations; i++)
            {
                float iterationOffs = (i * 1.0f);
                blurMaterial.SetVector("_Parameter", new Vector4(blurSize * widthMod + iterationOffs, -blurSize * widthMod - iterationOffs, 0.0f, 0.0f));

                // vertical blur
                RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, blurMaterial, 1 + passOffs);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;

                // horizontal blur
                rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
                rt2.filterMode = FilterMode.Bilinear;
                Graphics.Blit(rt, rt2, blurMaterial, 2 + passOffs);
                RenderTexture.ReleaseTemporary(rt);
                rt = rt2;
            }
            
            Graphics.Blit(rt, destination);

            RenderTexture.ReleaseTemporary(rt);
            */
            

            //for(int i = 0; i < blurIterations; i++)
                //float iterationOffs = (i * 1.0f);
                //blurMaterial.SetVector ("_Parameter", new Vector4 (blurSize * widthMod + iterationOffs, -blurSize * widthMod - iterationOffs, 0.0f, 0.0f));
            


            //}
        }
        //this.material.SetFloat("_bwBlend", intensity);
        //Graphics.Blit(source, destination, this.material);
    }
}
