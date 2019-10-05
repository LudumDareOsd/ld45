using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
	private Material material;

    void Start()
    {
		material = GetComponent<MeshRenderer>().material;    
    }

    void Update()
    {
		material.mainTextureOffset = new Vector2(0, (Time.deltaTime * 0.3f) + material.mainTextureOffset.y);
    }
}
