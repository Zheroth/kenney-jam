using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourTint : MonoBehaviour
{
    [SerializeField]
    private Color flashColor = Color.red;
    [SerializeField]
    private float flashDuration = 0.5f;

    List<MaterialProperties> matPropBlock = new List<MaterialProperties>();
    Coroutine currentFlashCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        GetAllMaterials();   
    }

    void GetAllMaterials()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;
            for (int j = 0; j < materials.Length; j++)
            {
                matPropBlock.Add(new MaterialProperties(materials[j]));
            }
        }
    }

    public void ColourFlash()
    {
        ColourFlash(flashColor, flashDuration);
    }

    public void ColourFlash(Color color, float duration)
    {
        if(currentFlashCoroutine != null)
        {
            StopCoroutine(currentFlashCoroutine);
        }
        StartCoroutine(ColourFlash_Coroutine(color, duration));
    }

    public IEnumerator ColourFlash_Coroutine(Color color, float duration)
    {
        float timer = 0;
        float completion = 0;
        while (completion < 1)
        {
            timer += Time.deltaTime;
            completion = timer/duration;
            for (int i = 0; i < matPropBlock.Count; i++)
            {
                matPropBlock[i].Material.color = Color.Lerp(color, matPropBlock[i].OriginalColor, completion);
            }
            yield return null;
        }

        for (int i = 0; i < matPropBlock.Count; i++)
        {
            matPropBlock[i].Material.color = matPropBlock[i].OriginalColor;
        }
    }

    class MaterialProperties
    {
        public Material Material
        {
            get;
            private set;
        }
        public Color OriginalColor
        {
            get;
            private set;
        }

        public MaterialProperties(Material material)
        {
            Material = material;
            OriginalColor = material.color;
        }
    }
}