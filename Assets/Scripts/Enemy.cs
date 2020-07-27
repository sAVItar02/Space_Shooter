using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] int health = 500;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.1f;
    [SerializeField] float maxTimeBetweenShots = 0.5f;
    [SerializeField] float enemyProjectileSpeed = -10f;

    [Header("VFX")]
    [SerializeField] GameObject enemyDeathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] GameObject enemyLaser;

    [Header("SFX")]
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] [Range(0, 1)] float SFXvolume = 0.75f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 1)] float SFXVolume = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0)
        {
            EnemyFire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void EnemyFire()
    {
        var laser = Instantiate(enemyLaser, gameObject.transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, enemyProjectileSpeed);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, SFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health = health - damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Destroy(gameObject);
            GameObject enemyDeathParticles = Instantiate(enemyDeathVFX, transform.position, transform.rotation);
            Destroy(enemyDeathParticles, durationOfExplosion);
            AudioSource.PlayClipAtPoint(enemyDeathSFX, Camera.main.transform.position, SFXvolume);
        }
    }
}
