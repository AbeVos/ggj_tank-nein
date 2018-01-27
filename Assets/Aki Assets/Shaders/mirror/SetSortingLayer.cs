using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetSortingLayer : MonoBehaviour {

    public string layerName = "Background";
    public int sortingOrder = 0;

	// Use this for initialization
	void Start () {
        SortLayer();
    }
    public void SortLayer ()
    {
        Renderer renderer = this.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = layerName;
            renderer.sortingOrder = sortingOrder;
        }
	}
}
