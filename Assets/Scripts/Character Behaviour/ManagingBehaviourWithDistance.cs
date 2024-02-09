using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManagingBehaviourWithDistance : MonoBehaviour
{
    [SerializeField]
    private string playerTag = "player";

    [SerializeField]
    private TextMeshProUGUI informationText;

    [SerializeField]
    private float activationDistance = 2f;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] audioClips;

    private GameObject player;
    private Animator animator;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        // Recherche du GameObject portant le tag "Player" dans la scène
        player = GameObject.FindWithTag(playerTag);

        if (player == null)
        {
            Debug.LogError("Aucun GameObject avec le tag 'Player' n'a été trouvé dans la scène.");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            Debug.Log(distanceToPlayer);

            // Activer ou désactiver le composant en fonction de la distance
            if (distanceToPlayer < activationDistance)
            {
                Debug.Log("Listening Stance");

                informationText.text = "Listening";

                audioSource.PlayOneShot(audioClips[0]);

                animator.SetBool("Listening", true);
            }
            else
            {
                Debug.Log("Waiting Stance");

                informationText.text = "Waiting";

                audioSource.PlayOneShot(audioClips[1]);

                animator.SetBool("Listening", false);
            }

        }
    }
}
