using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
	public void Awake()
	{
		Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
	}
}
