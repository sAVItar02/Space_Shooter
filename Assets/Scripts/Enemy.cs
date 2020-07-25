using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 500;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.1f;
    [SerializeField] float maxTimeBetweenShots = 0.5f;
    [SerializeField] GameObject enemyLaser;
    [SerializeField] GameObject enemyDeathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] float enemyProjectileSpeed = -10f;
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
        }
    }
}
