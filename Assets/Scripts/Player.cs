using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config Params
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 1000;
    [SerializeField] GameObject playerHitVFX;
    [SerializeField] float durationOfExplosion = 0.5f;
    [SerializeField] AudioClip playerHitSFX;

    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float firingDelay = 0.15f;
    [SerializeField] GameObject laserPrefab;

    [Header("SFX")]
    [SerializeField] AudioClip playerDeathSFX;
    [SerializeField] [Range(0, 1)] float SFXvolume = 0.75f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 1)] float shootSFXVolume = 0.75f;

    Coroutine fireCoroutine;

    float xMin, xMax;
    float yMin, yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();   
    }

 
    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        GameObject playerHitParticles = Instantiate(playerHitVFX, transform.position, Quaternion.identity);
        Destroy(playerHitParticles, durationOfExplosion);
        AudioSource.PlayClipAtPoint(playerHitSFX, Camera.main.transform.position, SFXvolume);
        health = health - damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(playerDeathSFX, Camera.main.transform.position, SFXvolume);
        FindObjectOfType<Level>().LoadGameOver();
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(fireCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while(true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
            yield return new WaitForSeconds(firingDelay);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;
    }

}
