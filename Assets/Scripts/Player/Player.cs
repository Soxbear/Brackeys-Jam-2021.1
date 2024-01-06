using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 CurrentInput;
    public Texture2D cursor;
    public Camera cam;
    public bool turretEnabled = false;
    Vector2 imageOffset = new Vector2(25, 25);
    Vector2 mousePos;
    public Animator Anim;

    [Header("Weapons")]
    Gun gun;
    public GameObject turret;
    public float turretStock = 1f;
    TurretStockText turretStockText;
    public WeaponText weaponText;
    public int currentGun = 1;
    public int prevGun;
    public string weapon;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public int RegenRate = 1;

    public HealthBar healthBar;

    [Header("Movement")]
    public float Speed = 2.5f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        Cursor.SetCursor(cursor, imageOffset, CursorMode.ForceSoftware);
        if (turretEnabled)
        {
            turretStockText = FindObjectOfType<TurretStockText>();
            turretStockText.updateStock();
        }
        Anim.SetInteger("HeldItem", currentGun);
        DisplayGun();
        weaponText.updateGun();
        gun = FindObjectOfType<Gun>();
        Anim = GetComponent<Animator>();

        InvokeRepeating("Regen", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        CurrentInput = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
            CurrentInput.y++;
        if (Input.GetKey(KeyCode.S))
            CurrentInput.y--;
        if (Input.GetKey(KeyCode.D))
            CurrentInput.x++;
        if (Input.GetKey(KeyCode.A))
            CurrentInput.x--;

        if (turretEnabled)
        {
            if (Input.GetKeyDown(KeyCode.E) && turretStock > 0)
            {
                Instantiate(turret, new Vector3(rb.position.x, rb.position.y, 0), Quaternion.identity);
                turretStock -= 1;
                turretStockText.updateStock();
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
            CurrentInput = new Vector2(0, 0);

        Anim.SetFloat("Horizontal", CurrentInput.x);
        Anim.SetFloat("Vertical", CurrentInput.y);
        Anim.SetFloat("Speed", CurrentInput.sqrMagnitude);

        Vector2 lookDir = mousePos - rb.position;
        Anim.SetFloat("MouseX", lookDir.x);
        Anim.SetFloat("MouseY", lookDir.y);

        prevGun = currentGun;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentGun >= 2)
            {
                currentGun = 0;
            }
            else
            {
                currentGun++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentGun <= 0)
            {
                currentGun = 2;
            }
            else
            {
                currentGun--;
            }
        }
        if (prevGun != currentGun)
        {
            Anim.SetInteger("HeldItem", currentGun);
            DisplayGun();
            weaponText.updateGun();
        }
    }

    void FixedUpdate()
    {

        rb.MovePosition(rb.position + (CurrentInput * Speed * Time.fixedDeltaTime));
        GetComponent<SpriteRenderer>().sortingOrder = (int) -transform.position.y;

        //float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle; 
        if (currentHealth <= 0)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        healthBar.SetHealth(currentHealth);
    }

    public void DisplayGun()
    {
        if (currentGun == 0)
        {
            weapon = "Hand";
            Speed = 12f;
        }
        else if (currentGun == 1)
        {
            weapon = "Pistol";
            Speed = 8f;
        }
        else if (currentGun == 2)
        {
            weapon = "SMG";
            Speed = 8;
        }
    }

    void Regen()
    {
        currentHealth += RegenRate;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }
}
