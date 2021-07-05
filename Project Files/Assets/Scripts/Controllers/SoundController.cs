using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource[] audioSources;

    public AudioClip[] walkClips;
    public AudioClip[] itemClips;

    public AudioClip npcClip, shoutClip, fireClip;

    public static int walk, item, talk, shout, fire;

    void Start() {
        walk = item = talk = shout = fire = -1;
    }

    void Update() {
        //Play dragon fire
        if (fire >= 0) {
            play(fireClip, 0);
            fire = -1;
        }
        else if (walk >= 0) { //Playing walking sound
            int randClip = Random.Range(0, walkClips.Length);

            play(walkClips[randClip], 0);
            walk = -1;
        }

        //Play item action sound
        if (item >= 0) {
            play(itemClips[item], 1);
            item = -1;
        }
        else if (shout >= 0) { //Play shout
            play(shoutClip, 1);
            shout = -1;
        }
        else if (talk >= 0) { //Play npc talk sound
            //Adjust pitch of npc
            audioSources[1].pitch = Random.Range(0.5f, 1.5f);
            play(npcClip, 1);

            audioSources[1].pitch = 1f;
            talk = -1;
        }
    }

    void play(AudioClip clip, int sourceIndex) {
        if (!audioSources[sourceIndex].isPlaying) {
            audioSources[sourceIndex].clip = clip;
            audioSources[sourceIndex].Play();
        }
    }
}
