using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsJewel : MonoBehaviour
{

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audio.PlayOneShot(audio.clip);
    }
}
