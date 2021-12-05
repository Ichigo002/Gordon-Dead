using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CoinScript : MonoBehaviour
{
    private PlayerStats playerStats;
    private ParticleSystem particles;
    private SoundManager _audio;
    private Light2D _light;
    private bool isActive = false;

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        particles = GetComponentInChildren<ParticleSystem>();
        _light = GetComponentInChildren<Light2D>();
    }
    private void OnEnable()
    {
        gameObject.SetActive(true);
        isActive = false;
        particles.gameObject.SetActive(false);
        _light.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
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
        playerStats.AddEvilCoin();
        particles.gameObject.SetActive(true);
        particles.Play();
        _audio.PlaySound(0);
        _light.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
