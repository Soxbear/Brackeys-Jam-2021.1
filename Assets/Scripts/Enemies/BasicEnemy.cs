using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BasicEnemy : MonoBehaviour
{

    AIPath PathfindingSettings;
    Seeker PathCalculator;
    AIDestinationSetter DestinationSetter;
    Animator Anim;

    public EnemyController enemyController;

    [HideInInspector]
    public float DamageStatBoots = 0f;

    [Header("Current Stats")]

    [SerializeField]
    int health = 100;
    public bool NoticedPlayer;
    public Transform CurrentTarget;
    public bool Shooting;
    public Transform[] Targets;

    float TimeSincePlayerSeen;
    int TargetNum;
    Vector2 PreviousLocation;
    float TimeSinceLook;

    [Header("Settings")]
    public int MaxHealth = 100;
    public int RegenerationRate = 1;
    public float MovementSpeed = 2;
    public float AttackRange = 7f;
    public float PlayerDetectionRange = 15f;
    public float PlayerAttackRange = 10f;
    public float MaxPlayerFollowTime = 10f;
    public float[] DefaultStats = { 100, 2, 0, 1 };
    public float[] GroupStatBoosts = { 0f, 0f, 0f, 0f };
    public float BoostRadius;
    public float ChangeLookTime = 5f;
    public LayerMask BasicEnemyMask;
    public GameObject DeathObject;
    public GameObject HealthBar;
    public GameObject Player;

    void Start()
    {
        PathfindingSettings = GetComponent<AIPath>();
        PathCalculator = GetComponent<Seeker>();
        DestinationSetter = GetComponent<AIDestinationSetter>();

        InvokeRepeating("Regeneration", 1f, 1f);

        DestinationSetter.target = Targets[0];
        TargetNum = 0;
        enemyController.enemySpawned();

        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        HealthBar.transform.localScale -= new Vector3(HealthBar.transform.localScale.x, 0, 0);
        HealthBar.transform.localScale += new Vector3(((float)health/(float)MaxHealth), 0, 0);
    }

    void FixedUpdate()
    {
        MaxHealth = (int) DefaultStats[0];
        MovementSpeed = DefaultStats[1];
        DamageStatBoots = (int)DefaultStats[2];
        RegenerationRate = (int)DefaultStats[3];
        Collider2D[] BasicEnemies = Physics2D.OverlapCircleAll(transform.position, BoostRadius, BasicEnemyMask);
        foreach (Collider2D Enemy in BasicEnemies)
        {
            MaxHealth += (int) Enemy.gameObject.GetComponent<BasicEnemy>().GroupStatBoosts[0];
            MovementSpeed += Enemy.gameObject.GetComponent<BasicEnemy>().GroupStatBoosts[1];
            DamageStatBoots += (int) Enemy.gameObject.GetComponent<BasicEnemy>().GroupStatBoosts[2];
            RegenerationRate += (int) Enemy.gameObject.GetComponent<BasicEnemy>().GroupStatBoosts[3];
        }

        PathfindingSettings.maxSpeed = MovementSpeed;

        RaycastHit2D PlayerDetection = Physics2D.Raycast(transform.position, (Player.transform.position) - transform.position, PlayerDetectionRange);
        if (PlayerDetection == true)
        {
            if (PlayerDetection.collider.name == Player.name && !NoticedPlayer)
                NoticedPlayer = true;
        }

        PathfindingSettings.canMove = true;
        Shooting = false;

        if (NoticedPlayer)
        {
            DestinationSetter.target = Player.transform;
            RaycastHit2D PlayerAttack = Physics2D.Raycast(transform.position, (Player.transform.position) - transform.position, PlayerAttackRange);
            if (PlayerDetection == true)
            {
                if (PlayerDetection.collider.gameObject.name == Player.name)
                {
                    if (PlayerAttack == true)
                    {
                        if (PlayerAttack.collider.gameObject.name == Player.name)
                        {
                            PathfindingSettings.canMove = false;
                            Shooting = true;
                        }
                    }                    

                    TimeSincePlayerSeen = 0;
                }
                else
                {
                    TimeSincePlayerSeen += Time.fixedDeltaTime;
                    if (TimeSincePlayerSeen >= MaxPlayerFollowTime)
                    {
                        NoticedPlayer = false;
                        DestinationSetter.target = Targets[Targets.Length - 1];
                        TargetNum = Targets.Length - 1;
                    }
                }
            }            
            else
            {
                TimeSincePlayerSeen += Time.fixedDeltaTime;
                if (TimeSincePlayerSeen >= MaxPlayerFollowTime)
                {
                    NoticedPlayer = false;
                    DestinationSetter.target = Targets[Targets.Length - 1];
                }
            }
        }

        if (Vector2.Distance(transform.position, DestinationSetter.target.position) < 0.25)
        {
            TargetNum++;
            if (TargetNum == Targets.Length)
                TargetNum = 0;
            DestinationSetter.target = Targets[TargetNum];
        }

        Anim.SetFloat("Horizontal", transform.position.x - PreviousLocation.x);
        Anim.SetFloat("Vertical", transform.position.y - PreviousLocation.y);
        Anim.SetFloat("Speed", new Vector2(Mathf.Abs(transform.position.x - PreviousLocation.x), Mathf.Abs(transform.position.y - PreviousLocation.y)).sqrMagnitude * (1 / Time.fixedDeltaTime));

        PreviousLocation = new Vector2(transform.position.x, transform.position.y);

        if (NoticedPlayer)
        {
            Anim.SetFloat("MouseX", Player.transform.position.x - transform.position.x);
            Anim.SetFloat("MouseY", Player.transform.position.y - transform.position.y);
        }
        else
        {
            if (TimeSinceLook >= ChangeLookTime)
            {
                Anim.SetFloat("MouseX", Random.Range(-1f, 1f));
                Anim.SetFloat("MouseY", Random.Range(-1f, 1f));
                TimeSinceLook = 0f;
            }
            TimeSinceLook += Time.fixedDeltaTime;
        }

        GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)-transform.position.y;
    }


    public void TakeDamage(int Amount)
    {
        health -= Amount;

        if (health <= 0)
        {
            Instantiate(DeathObject, transform.position, transform.rotation);
            enemyController.enemyDies();
            Destroy(gameObject);
        }
    }

    void Regeneration()
    {        
        health += RegenerationRate;
        if (health > MaxHealth)
            health = MaxHealth;
    }

}
