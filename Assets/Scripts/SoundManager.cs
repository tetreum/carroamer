using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
 *
 * SoundManager.Instance.playEffect(SoundManager.Effect.ButtonClickOff, this.gameObject);
 * SoundManager.playMusic(SoundManager.Music.Intro, this.gameObject);
 * SoundManager.Instance.playDialog(DialogPanel.Dialogs.NoPower);
*/

public class SoundManager : MonoBehaviour {

	public static SoundManager Instance;
    public delegate void ResponseCallback(AudioSource result);
    public delegate void FinishCallback();

    public GameObject soundNotificatorPrefab;

    public AudioClip[] clips;
    private Dictionary<string, AudioClip> indexedClipsList = new Dictionary<string, AudioClip>();
    private Dictionary<Effect, int> clipsWithNotification = new Dictionary<Effect, int>() {
        {Effect.BreakGlass, 3}
    };

    public AudioClip[] songs;
    private Dictionary<string, AudioClip> indexedMusicList = new Dictionary<string, AudioClip>();
   
    private float musicVolume = 0.1f;

    private AudioSource currentEffect;
    private AudioSource currentMusic;
    private AudioSource externalAmbient;
    private Coroutine ambientCoroutine;
    private bool increaseAmbient = false;
    private float t = 0.0f;

    public enum Effect {
        BreakGlass = 1,
        Eat = 2,
        OpenCarDoor = 3,
        LockCarDoor = 4,
        OpenDoor1 = 5,
        OpenDoor2 = 6,
        OpenDoor3 = 7,
        CloseDoor1 = 8,
        CloseDoor2 = 9,
        CloseDoor3 = 10,
        LockedDoor = 11,
    }

    public enum Music
    {
    }

	void Awake () {
        Instance = this;

        foreach (AudioClip clip in clips) {
            indexedClipsList.Add(clip.name, clip);
        }

        foreach (AudioClip clip in songs) {
            indexedMusicList.Add(clip.name, clip);
        }

        clips = null;
        songs = null;

        try {
            externalAmbient = GameObject.Find("ExternalAmbient").GetComponent<AudioSource>();
        } catch (Exception) { }
    }

	public AudioSource playEffect (Effect effect, GameObject obj, ResponseCallback callback = null, FinishCallback onFinishCallback = null)
    {
		AudioSource audio;
        string effectName = effect.ToString();

        audio = obj.GetComponent<AudioSource>();
		
		if (audio == null) {
			audio = obj.AddComponent<AudioSource>();
		}

        if (!indexedClipsList.ContainsKey(effectName)) {
            Debug.LogError("SoundManager - " + effectName + " is not present in effects list");
            return null;
        }

        audio.clip = indexedClipsList[effectName];

        if (callback != null) {
            callback(audio);
        }
        
        setupEffectAndPlay(effect, audio);

        if (clipsWithNotification.ContainsKey(effect)) {
            var notifier = Instantiate(soundNotificatorPrefab, obj.transform.position, obj.transform.rotation);
            //notifier.GetComponent<Vecinos.NPC.SoundNotificator>().init(effectName, audio.clip.length, obj.transform.position, clipsWithNotification[effect]);
        }

        if (onFinishCallback != null) {
            Instance.StartCoroutine(Instance.OnEffectFinish(audio.clip.length, onFinishCallback));
        }

        return audio;
    }

    public static AudioSource playMusic(Music effect, GameObject obj, ResponseCallback beforeStartCallback = null, FinishCallback onFinishCallback = null)
    {
        AudioSource audio;

        audio = obj.GetComponent<AudioSource>();

        if (audio == null) {
            audio = obj.AddComponent<AudioSource>();
        }

        audio.clip = Instance.indexedMusicList[effect.ToString()];
        audio.volume = Instance.musicVolume;

        if (beforeStartCallback != null) {
            beforeStartCallback(audio);
        }

        if (Instance.currentMusic != null) {
            Instance.currentMusic.Stop();
        }

        Instance.currentMusic = audio;
        audio.Play();

        if (onFinishCallback != null) {
            Instance.StartCoroutine(Instance.OnMusicFinish(audio.clip.length, onFinishCallback));
        }

        return audio;
    }

    public void playEffect(AudioClip effect, GameObject obj, ResponseCallback callback = null)
    {
        AudioSource audio;

        audio = obj.GetComponent<AudioSource>();

        if (audio == null) {
            audio = obj.AddComponent<AudioSource>();
        }

        audio.clip = effect;

        if (callback != null) {
            callback(audio);
        }

        audio.Play();
    }

    private void setupEffectAndPlay(Effect effect, AudioSource audio) {
       /*
        try {
            switch (effect) {
                case Effect.OpenFridge:
                    audio.spatialBlend = 1;
                    audio.minDistance = 0.119f;
                    audio.maxDistance = 0.18f;
                    break;
            }
        } catch (Exception) { }
        */
        currentEffect = audio;
        audio.Play();
    }


    public void stopSoundEffect ()
    {
        try {
            currentEffect.Stop();
        } catch { }
    }

    private IEnumerator OnMusicFinish(float time, FinishCallback callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    private IEnumerator OnEffectFinish(float time, FinishCallback callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

    public void lowerExternalAmbient () {
        try {
            StopCoroutine(ambientCoroutine);
        } catch (Exception) { }
        
        increaseAmbient = false;
        t = 0.0f;
        ambientCoroutine = StartCoroutine(updateAmbientSound());
    }

    public void increaseExternalAmbient() {
        try {
            StopCoroutine(ambientCoroutine);
        } catch (Exception) { }
        increaseAmbient = true;
        t = 0.0f;
        ambientCoroutine = StartCoroutine(updateAmbientSound());
    }

    private IEnumerator updateAmbientSound () {
        if (externalAmbient == null) {
            yield return null;
        }

        float defaultVolume = 0.6f;

        if (increaseAmbient) {
            if (externalAmbient.volume < 0.3f) {
                externalAmbient.volume = 0.3f;
            }

            while (externalAmbient.volume < 0.59f) {
                yield return new WaitForSeconds(0.2f);
                externalAmbient.volume = Mathf.Lerp(externalAmbient.volume, defaultVolume, t);
                t += 0.04f * Time.deltaTime;
            }
        } else if (externalAmbient.volume > 0.01f) {
            if (externalAmbient.volume > 0.3f) {
                externalAmbient.volume = 0.3f;
            }
            
            while (externalAmbient.volume > 0.01f) {
                yield return new WaitForSeconds(0.2f);
                externalAmbient.volume = Mathf.Lerp(externalAmbient.volume, 0, t);
                t += 0.4f * Time.deltaTime;
            }
        }
    }
}
