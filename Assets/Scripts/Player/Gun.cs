using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public LayerMask rayMask;
    public Camera cam;
    public Rigidbody2D player;
    public Player pScript;
    public GameObject gunpoint;
    public bool canShoot = true;
    Vector2 mousePos;
    public LineRenderer line;

    public AudioSource PistolSound;
    public AudioSource SmgSound;
    public AudioClip SmgClip;

    [Header("Gun")]
    public float fireRate = 15f;
    public float shootStun = 3f;
    private float nextTimeToFire = 0f;
    public float bulletDuration = 2f;
    public int damage = 25;

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire && canShoot)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        if (pScript.prevGun != pScript.currentGun)
        {
            // Hand??
            if (pScript.currentGun == 0)
            {
                canShoot = false;
            }
            // Pistol
            if (pScript.currentGun == 1)
            {
                fireRate = 2f;
                damage = 20;
                canShoot = true;
            }
            // SMG
            if (pScript.currentGun == 2)
            {
                fireRate = 15f;
                damage = 5;
                canShoot = true;
            }
        }
    }
    public void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.position, mousePos - player.position, 1000, rayMask); ;
        if (hit.collider.tag == "BasicEnemy")
        {
            hit.collider.gameObject.GetComponent<BasicEnemy>().TakeDamage(damage);
        }
        line.enabled = true;
        line.SetPosition(0, gunpoint.transform.position);
        line.SetPosition(1, hit.point);
        Invoke("DisableLineRenderer", Time.fixedDeltaTime * bulletDuration);
        if (pScript.currentGun == 1)
            PistolSound.Play();
        else
            SmgSound.PlayOneShot(SmgClip);
    }
    private void DisableLineRenderer()
    {
        line.enabled = false;
    }
}
