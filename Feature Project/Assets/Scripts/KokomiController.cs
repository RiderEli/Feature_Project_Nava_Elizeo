using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class KokomiController : MonoBehaviour
{
    // Kokomi's General (Data) Codes. Each of these codes will be used in an individual UI script.
    public int maxHealth = 40000;
    public int damage = 700;
    public int healing = 5000;
    public int buffedDamage = 3000;
    public int buffedHealing = 7000;
    public int attackHealing = 800;
    public int skillDamage = 1000;
    public int buffedSkillDamage = 2700;
    public float skillTime; //12 Seconds
    public float ultimateTime; // 16 Seconds
    public float attackTime;
    public int currentHealth;
    private int kokoMatNum;
    public Material[] kokoMat;
    Renderer kokoRend;

    //Kokomi's Movement Codes
    public float kokomiSpeed;
    public float jumpForce;
    private Rigidbody rb;
    private Vector3 moveVec;
    private Vector2 mousePos;

    //Kokomi's Cooldown Codes
    public bool skillCooldown; // 20 Seconds
    public bool ultimateCooldown; // 10 Seconds

    //Kokomi's Ability Codes
    public GameObject fishPrefab;
    public GameObject jellyfishPrefab;
    public bool skillActive;
    public bool ultimateActive;
    public Transform jellyfishSpot;
    private GameObject jellyfishSkill;

    //Kokomi's Input System
    KokomiControls KokomiControls;

    //This is a Singleton
    static public KokomiController instance;

    //Other Codes for the Feature
    public bool isJumping;
    private float lastFish;

    private void Awake()
    {
        currentHealth = maxHealth;
        instance = this;
        KokomiControls = new KokomiControls();
        KokomiControls.Enable();
        rb = GetComponent<Rigidbody>();
        isJumping = false;
        skillActive = false;
        ultimateActive = false;
        skillCooldown = false;
        ultimateCooldown = false;
    }
    void Start()
    {
        currentHealth -= 20000;
        jellyfishSkill = GameObject.FindWithTag("Skill");
        KokomiControls.KokomiActions.Skill.started += _ => KokomiSkill();
        KokomiControls.KokomiActions.Ultimate.started += _ => KokomiUltimate();
        KokomiControls.KokomiActions.Attack.started += _ => KokomiAttack();
        KokoMatPrep();
    }
    private void FixedUpdate()
    {
        Debug.Log("Health:" + currentHealth + "/" + maxHealth);
        moveVec = KokomiControls.KokomiMovement.MovementInputs.ReadValue<Vector3>();
        rb.AddForce(new Vector3 (moveVec.x, 0 ,moveVec.z) * kokomiSpeed, ForceMode.Force);
        KokomiControls.KokomiActions.Jump.started += _ => KokomiJump();
        JumpingState();
        WhileSkillActive();
        WhileUltActive();
        kokoRend.sharedMaterial = kokoMat[kokoMatNum];
        KokomiHPandUltData();
    }

    public void KokomiHPandUltData()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (ultimateActive == true)
        {
            kokoMatNum = 1;
        }
        else
        {
            kokoMatNum = 0;
        }
    }


    public void KokomiSkill()
    {
        Debug.Log("E Skill has been activated");
        jellyfishSkill = Instantiate(jellyfishPrefab, jellyfishSpot.position, Quaternion.identity);
        skillActive = true;
    }
    public void KokomiAttack()
    {
        Debug.Log("Kokomi has attacked");
        NormalAttack();
        
    }
    public void KokomiUltimate()
    {
        Debug.Log("Ultimate has been activated");
        ultimateActive = true;
    }
    
    public void KokomiJump()
    {
       Debug.Log("Kokomi has jumped");
       rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
       isJumping = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }
    }

    public void JumpingState()
    {
        if (isJumping)
        {
            KokomiControls.KokomiActions.Attack.Disable();
        }
        else
        {
            KokomiControls.KokomiActions.Attack.Enable();
        }
        
    }

    public void WhileSkillActive()
    {
        if (skillActive)
        {
            KokomiControls.KokomiActions.Skill.Disable();
            StartCoroutine(SkillTime());
        }
        else
        {
            KokomiControls.KokomiActions.Skill.Enable();
        }
    }

    public void OnSkillCooldown()
    {
        if (skillCooldown)
        {
            KokomiControls.KokomiActions.Skill.Disable();
        }
        else
        {
            KokomiControls.KokomiActions.Skill.Enable();
        }
    }

    public IEnumerator SkillTime()
    {
        yield return new WaitForSeconds(skillTime);
        Debug.Log("The Jellyfish should be destroyed");
        Destroy(jellyfishSkill);
        skillCooldown = true;
        skillActive = false;
        StartCoroutine(SkillCooldownTime());
    }

    public IEnumerator SkillCooldownTime()
    {
        Debug.Log("The Skill is now on cooldown");
        yield return new WaitForSeconds(20);
        skillCooldown = false;
        Debug.Log("The Skill can now be used again");
        yield return null;
    }

    public void WhileUltActive()
    {
        if (ultimateActive)
        {
            KokomiControls.KokomiActions.Ultimate.Disable();
            StartCoroutine(UltTime());
        }
        else
        {
            KokomiControls.KokomiActions.Ultimate.Enable();
        }
    }
    public void OnUltCooldown()
    {
        if (ultimateCooldown)
        {
            KokomiControls.KokomiActions.Ultimate.Disable();
        }
        else
        {
            KokomiControls.KokomiActions.Ultimate.Enable();
        }
    }
    public IEnumerator UltTime()
    {
        yield return new WaitForSeconds(ultimateTime);
        Debug.Log("The Ultimate Should End");
        ultimateCooldown = true;
        ultimateActive = false;
        StartCoroutine(UltCooldownTime());
    }

    public IEnumerator UltCooldownTime()
    {
        Debug.Log("The Ultimate is now on cooldown");
        yield return new WaitForSeconds(10);
        ultimateCooldown = false;
        Debug.Log("The Ultimate can now be used again");
        yield return null;
    }

    public void NormalAttack()
    {
        if (Time.time-lastFish<attackTime)
        {
            return;
        }
        lastFish = Time.time;
        Instantiate(fishPrefab, transform.position, Quaternion.identity);
    }

    public void KokoMatPrep()
    {
        kokoMatNum = 0;
        kokoRend = GetComponent<Renderer>();
        kokoRend.enabled = true;
        kokoRend.sharedMaterial = kokoMat[kokoMatNum];
    }

}
