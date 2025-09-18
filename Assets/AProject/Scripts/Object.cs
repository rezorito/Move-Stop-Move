using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Renderer rend;

    private Material material = null;
    private void Start() {
        material = rend.materials[0];
    }   

    private void setMaterialInTrigger()
    {
        material.SetFloat("_Mode", 3);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        material.SetColor("_Color", new Color(1f, 1f, 1f, 0.4f));
    }

    private void setMaterialOutTrigger()
    {
        material.SetFloat("_Mode", 0);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;

        material.SetColor("_Color", new Color(0f / 255f, 160f / 255f, 255f / 255f, 1f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Range"))
        {
            setMaterialInTrigger();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Range"))
        {
            setMaterialOutTrigger();
        }
    }
}
