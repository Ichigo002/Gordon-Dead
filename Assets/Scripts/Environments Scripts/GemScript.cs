using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    private PlayerStats playerStats;
    private ParticleSystem particles;
    private SoundManager _audio;
    private Animator anim;
    private bool isActive = false;

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        particles = GetComponentInChildren<ParticleSystem>();
        anim = GetComponent<Animator>();
        _audio = GetComponent<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            StartCoroutine(PickedUp());
        }
    }

    IEnumerator PickedUp()
    {
        playerStats.AddGem();
        particles.Play();
        anim.Play("PickedUp");
        _audio.PlaySound(0);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
