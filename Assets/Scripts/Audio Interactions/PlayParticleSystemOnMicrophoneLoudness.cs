using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayParticleSystemOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public ParticleSystem particles;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;


    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if(loudness <= threshold && particles.isPlaying)
        {
            Debug.Log("particles.Stop()");
            particles.Stop();
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (loudness > threshold && particles.isStopped)
        {
            Debug.Log("particles.Play()");
            particles.Play();
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }

    private void OnDisable()
    {
        particles.Stop();
    }

}
