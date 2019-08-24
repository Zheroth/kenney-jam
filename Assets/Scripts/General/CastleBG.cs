using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBG : MonoBehaviour
{
    Coroutine currentCoroutine;

    [SerializeField]
    private MeshRenderer renderer;
    [SerializeField]
    private int materialIndex = 0;
  
    public void SetColour(Color color)
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        this.currentCoroutine = StartCoroutine(SetColour(color, 3));
    }

    IEnumerator SetColour(Color color, float time)
    {
        Color prevColour = renderer.materials[materialIndex].color;

        float timer = 0;
        while(timer < time)
        {
            timer += Time.deltaTime;

            float completion = timer / time;
            renderer.materials[materialIndex].color = Color.Lerp(prevColour, color, completion);

            yield return null;
        }

        renderer.materials[materialIndex].color = color;
    }
}
