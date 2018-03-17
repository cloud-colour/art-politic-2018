using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public List<AudioClip> RawBGM;
    public List<AudioClip> RawSFX;

    public static SoundManager inst;

    [SerializeField] AudioSource BGMAudioSource;
    [SerializeField] AudioSource SFXAudioSource;

    void Awake()
    {

        DontDestroyOnLoad(transform.gameObject);

        if (inst == null)
        {
            inst = this;
        }

        if(FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);
    }

    public void PlayBGM(int index)
    {
        BGMAudioSource.clip = RawBGM[index];
        BGMAudioSource.Play();
    }

    public void StopBGM()
    {
        BGMAudioSource.Stop();
    }

    public void StopFadeBGM()
    {
        StartCoroutine (FadeOut (BGMAudioSource, 0.5f, null));
    }

    public void PlayFadeBGM(int index)
    {
        StartCoroutine (FadeOut (BGMAudioSource, 0.5f, RawBGM[index]));
    }

    public void PlaySFX(int index)
    {
        SFXAudioSource.clip = RawSFX[index];
        SFXAudioSource.Play();
    }

	public void PlaySFXOneShot(int index)
	{
		SFXAudioSource.clip = RawSFX[index];
		SFXAudioSource.PlayOneShot(RawSFX[index]);
	}

    public void StopSFX()
    {
        SFXAudioSource.Stop();
    }

    public bool IsSFXPlaying()
    {
        return SFXAudioSource.isPlaying;
    }



    public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime,AudioClip clip) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop ();
        audioSource.volume = startVolume;
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
