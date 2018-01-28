using UnityEngine;
using System.Collections;
using System;

public static class BurnExtension
{
    public static void BurnObject<T>
    (   this T obj,
        float duration,
        Action<T> callback = null
    )   where T : MonoBehaviour // TODO: change MonoBehaviour with the name of the enemyclass
    {
        // Makes a private copy of the material
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = new Material(renderer.material);
        obj.StartCoroutine(BurnCoroutine(obj, renderer.sharedMaterial, duration, callback));
    }

    private static IEnumerator BurnCoroutine<T> 
    (   T obj,
        Material material,
        float duration,
        Action<T> callback
    )   where T : MonoBehaviour
    {
        // Fading animation
        float start = Time.time;
        while (Time.time <= start + duration)
        {
            
            float burnValue = Mathf.Clamp01((Time.time - start) / duration);
            material.SetFloat("_BurnValue", burnValue);
            yield return new WaitForEndOfFrame();
        }

        // Callback
        if (callback != null)
            callback(obj);
    }
}
