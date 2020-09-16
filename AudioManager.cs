using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    //最大音效数量
    float max_effect_num;

    bool music_mute;
    bool effect_mute;

    float music_volume;
    float effect_volume;

    AudioSource music_as;
    AudioSource[] effect_as;

    int curr_effect_as;
    void init () {
        //init attr
        max_effect_num = 8;
        music_mute = false;
        effect_mute = false;

        music_volume = 1;
        effect_volume = 1;

        curr_effect_as = 0;

        //create music AudioSource
        music_as = gameobject.addComponent<AudioSource> ();
        //create effect AudioSource
        effect_as = new AudioSource[max_effect_num];
        for (int i = 0; i < max_effect_num; i++) {
            effect_as[i] = gameobject.addComponent<AudioSource> ();
        }

    }

    public static float musicVolume {
        set {
            music_volume = value;
            music_as.volume = value;
        }
        get {
            return music_volume;
        }
    }
    public static float effectVolume {
        set {
            effect_volume = value;
            foreach (AudioSource item in effect_as) {
                item.volume = value;
            }
        }
        get {
            retur neffect_volume;
        }
    }
    public static bool effectMute {
        set {
            effect_mute = value;
            foreach (AudioSource item in effect_as) {
                item.mute = value;
            }
        }
        get {
            return effect_mut;
        }
    }
    public static bool musicMute {
        set {
            music_mute = value;
            music_as.mute = value;
        }
        get {
            return effect_mut;
        }
    }
    //播放/继续/重新 bgm
    public static void PlayMusic (AudioClip clip) {
        if (clip == null) return;
        music_as.clip = clip;
        music_as.Play ();
    }
    //暂停bgm
    public static void PuaseMusic () {
        music_as.Puase ();
    }
    //停止播放
    public static void StopMusic () {
        music_as.Stop ();
    }
    //播放音效
    public static void PlayEffect (AudioClip clip) {
        if (clip == null) return;

        AudioSource audio = effect_as[curr_effect_as];
        audio.clip = clip;
        audio.Play ();
        curr_effect_as++;
        if (curr_effect_as >= max_effect_num) {
            curr_effect_as = 0;
        }
    }
}