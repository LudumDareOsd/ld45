using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	private AudioSource source;

	public void Start()
	{
		source = GetComponent<AudioSource>();
	}

	public AudioSource PlaySingle(AudioClip clip, float volume)
	{
		var audioSrc = source.GetComponent<AudioSource>();
		audioSrc.clip = clip;
		audioSrc.volume = volume;
		audioSrc.spatialBlend = 0;
		audioSrc.dopplerLevel = 0;
		audioSrc.spread = 0;
		audioSrc.pitch = 1f;
		audioSrc.Play();

		return audioSrc;
	}
}
