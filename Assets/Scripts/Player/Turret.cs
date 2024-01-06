using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float retargetRate = 1f;
    public int damage = 25;
    public float AimStray = 1f;
    public float fireRate = 0.5f;
    public float bulletDuration = 3f;
    public float range = 50f;
    public bool inRange;
    public bool shooting = false;
    private float dist;
    public bool lineOfSight = false;
    public LineRenderer line;
    GameObject closeEnemy;
    GameObject[] enemyList;
    // Start is called before the first frame update
    void Start()
    {
        enemyList = GameObject.FindGameObjectsWithTag("BasicEnemy");
    }
    void Awake()
    {
        InvokeRepeating("Retarget", 0f, retargetRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (closeEnemy != null)
        {
            dist = Vector3.Distance(transform.position, closeEnemy.transform.position);
            if (dist < range)
            {
                inRange = true;
            }
            else if (dist > range)
            {
                inRange = false;
            }
            if (inRange == false && shooting == true)
            {
                CancelInvoke("Shoot");
                shooting = false;
            }
            if (inRange && shooting == false && lineOfSight)
            {
                InvokeRepeating("Shoot", 0f, fireRate);
                shooting = true;
            }
        }
    }
    void FixedUpdate()
    {
        if (closeEnemy != null)
        {
            RaycastHit2D seeEnemy = Physics2D.Raycast(transform.position, (closeEnemy.transform.position) - transform.position, 1000);
            if (seeEnemy == true)
            {
                if (seeEnemy.collider.tag == "BasicEnemy")
                    lineOfSight = true;
            }
        }
    }

    void Retarget()
    {
        enemyList = GameObject.FindGameObjectsWithTag("BasicEnemy");
        closeEnemy = GetClosestEnemy(enemyList);
    }

    GameObject GetClosestEnemy(GameObject[] enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
    void Shoot()
    {
        Vector3 Stray = new Vector3(Random.Range(-AimStray, AimStray), Random.Range(-AimStray, AimStray), 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (closeEnemy.transform.position + Stray) - transform.position, 1000);
        if (hit.collider.gameObject.tag == "BasicEnemy")
        {
            hit.collider.gameObject.GetComponent<BasicEnemy>().TakeDamage(damage);
        }
        line.enabled = true;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hit.point);
        Invoke("DisableLineRenderer", Time.fixedDeltaTime * bulletDuration);
    }

    void DisableLineRenderer()
    {
        line.enabled = false;
    }
}
