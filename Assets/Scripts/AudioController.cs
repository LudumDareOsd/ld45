using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	private AudioSource source;
	private List<GameObject> sources = new List<GameObject>();

	public void Start()
	{
		source = GetComponent<AudioSource>();
	}

	public AudioSource PlaySingle(AudioClip clip, float volume)
	{
		
		var audioSrc = GetSource().GetComponent<AudioSource>();
		audioSrc.clip = clip;
		audioSrc.volume = volume;
		audioSrc.spatialBlend = 0;
		audioSrc.dopplerLevel = 0;
		audioSrc.spread = 0;
		audioSrc.pitch = 1f;
		audioSrc.Play();

		return audioSrc;
	}

	private GameObject GetSource()
	{
		var source = sources.Find(it => !it.GetComponent<AudioSource>().isPlaying);

		if (source == null)
		{
			var newSource = new GameObject("AudioSource");
			newSource.AddComponent<AudioSource>();
			source = newSource;
			sources.Add(source);
		}

		return source;
	}


}
