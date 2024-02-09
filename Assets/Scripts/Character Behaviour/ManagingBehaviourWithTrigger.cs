using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagingBehaviourWithTrigger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI informationText;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] audioClips;

    [SerializeField]
    private GameObject player;

    //[SerializeField]
    //private GameObject raycastOrigin;

    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();

        // Recherche du GameObject portant le tag "Player" dans la scène
        player = GameObject.FindWithTag("player");

        if (player == null)
        {
            Debug.LogError("Aucun GameObject avec le tag 'player' n'a été trouvé dans la scène.");
        }
        else
        {
            informationText.text = "Hello Player";
            StartCoroutine(WaitingBeforePlaying(3.0f));
        }

    }

    private void Update()
    {
        /*
        RaycastHit hit;
        Vector3 direction = player.transform.position - raycastOrigin.transform.position;

        if (Physics.Raycast(raycastOrigin.transform.position, direction, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(raycastOrigin.transform.position, direction * hit.distance, Color.red);
            Debug.Log("Did Hit");
            if(hit.distance > 0)
            {
                informationText.text = hit.distance.ToString();
            }
        }
        else
        {
            Debug.DrawRay(raycastOrigin.transform.position, direction * 1000, Color.yellow);
            Debug.Log("Did not Hit");
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("player"))
        {
            Debug.Log("Listening Stance");

            informationText.text = "Listening";

            audioSource.PlayOneShot(audioClips[0]);

            animator.SetBool("Listening", true);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            Debug.Log("Waiting Stance");

            audioSource.PlayOneShot(audioClips[1]);

            BasicStance();
        }
    }

    void BasicStance()
    {
        informationText.text = "Waiting";
        if (animator.GetBool("Listening") == true) animator.SetBool("Listening", false);
        if (animator.GetBool("Greeting") == true) animator.SetBool("Greeting", false);
        if (animator.GetBool("Thankful") == true) animator.SetBool("Thankful", false);
        if (animator.GetBool("Rejected") == true) animator.SetBool("Rejected", false);
    }

    public void GreetingStance()
    {
        informationText.text = "Hi ! Nice to see you !";
        animator.SetBool("Greeting", true);
        StartCoroutine(WaitingBeforePlaying(4f));
    }

    public void ThankfulStance()
    {
        informationText.text = "Thank you !";
        animator.SetBool("Thankful", true);
        StartCoroutine(WaitingBeforePlaying(2.5f));
    }

    public void RejectedStance()
    {
        informationText.text = "Oh no, I'm so sorry !";
        animator.SetBool("Rejected", true);
        StartCoroutine(WaitingBeforePlaying(4f));
    }

    IEnumerator WaitingBeforePlaying(float time)
    {
        yield return new WaitForSeconds(time);
        BasicStance();
    }
}
