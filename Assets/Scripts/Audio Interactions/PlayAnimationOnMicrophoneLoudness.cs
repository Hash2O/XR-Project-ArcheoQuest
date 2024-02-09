using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnMicrophoneLoudness : MonoBehaviour
{
    public AudioLoudnessDetection detector;
    public Animator animator;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    private WaitingBehaviour _behaviour;


    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness <= threshold)
        {
            Debug.Log("Animation : Play");
            animator.SetBool("Talking", true);
        }
        else if (loudness > threshold)
        {
            Debug.Log("Animation : Stop");
            animator.SetBool("Talking", false);
        }
    }

    private void OnDisable()
    {
        animator.SetBool("Talking", true);
    }
}
