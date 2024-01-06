using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PistolBasicEnemy : MonoBehaviour
{

    public GameObject Player;
    public GameObject particle;
    public GameObject gunpoint;
    public LineRenderer line;
    public AudioClip GunSound;

    [Range(0, 5f)]
    public float AimStray;
    public int Damage;
    public float FireRate = 1;

    bool IsShooting = false;
    public float bulletDuration = 2f;

    void Start()
    {

    }

    
    void FixedUpdate()
    {
        if (gameObject.GetComponent<BasicEnemy>().Shooting && !IsShooting)
        {
            IsShooting = true;
            InvokeRepeating("Shoot", 0f, FireRate);
        }
        else if (!gameObject.GetComponent<BasicEnemy>().Shooting && IsShooting)
        {
            IsShooting = false;
            CancelInvoke("Shoot");
        }
    }

    void Shoot()
    {
        Vector3 Stray = new Vector3(Random.Range(-AimStray, AimStray), Random.Range(-AimStray, AimStray), 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (Player.transform.position + Stray) - transform.position, 1000);
        GetComponent<AudioSource>().PlayOneShot(GunSound);
        if (hit.collider.gameObject.name == "Player")
        {
            hit.collider.gameObject.GetComponent<Player>().TakeDamage(Damage + (int) GetComponent<BasicEnemy>().DamageStatBoots);
        }
        Instantiate(particle, hit.point, hit.transform.rotation);
        line.enabled = true;
        line.SetPosition(0, gunpoint.transform.position);
        line.SetPosition(1, hit.point);
        Invoke("DisableLineRenderer", Time.fixedDeltaTime*bulletDuration);
    }
    private void DisableLineRenderer()
    {
    line.enabled = false;
    }
}
