using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> clips;
    private AudioSource m_AudioSource;

    private void Start()
    {
         m_AudioSource = GetComponent<AudioSource>();   
    }

    public void PlayAudioClip(int index)
    {
        m_AudioSource.clip = clips[index];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
    }
}
