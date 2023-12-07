using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
/* [Nava, Elizeo]
 * [December 7, 2023]
 * [This is the code for all of the Inputs and Data with in the KokomiContainer. There is a Singleton Pattern that will allow for this code to be used in other scripts.]
 */
public class KokomiController : MonoBehaviour
{
    // Kokomi's General (Data) Codes. Each of these codes will be used in an individual UI script thanks to a Singleton pattern.
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

    //Kokomi's Material Codes (Reserved for her Ultimate/Elemental Burst)
    private int kokoMatNum;
    public Material[] kokoMat;
    Renderer kokoRend;

    //Kokomi's Movement Codes
    public float kokomiSpeed;
    public float jumpForce;
    private Rigidbody rb;
    private Vector3 moveVec;

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
    public GameObject healingText;
    public GameObject buffedHealingText;
    public GameObject attackHealingText;
    public bool atkHealActive;
    public IEnumerator activeUlt;
    public IEnumerator activeSkill;
    public GameObject gamePause;

    //This Awake() void will start the codes within the script
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
        atkHealActive = false;
        healingText.SetActive(false);
        buffedHealingText.SetActive(false);
        attackHealingText.SetActive(false);
        gamePause.SetActive(false);
        Time.timeScale = 1f;
    }
    //This Start() void will help the project function in-game.
    void Start()
    {
        jellyfishSkill = GameObject.FindWithTag("Skill");
        KokomiControls.KokomiActions.Skill.started += _ => KokomiSkill();
        KokomiControls.KokomiActions.Ultimate.started += _ => KokomiUltimate();
        KokomiControls.KokomiActions.Attack.started += _ => KokomiAttack();
        KokomiControls.KokomiActions.Pause.started += _ => KokomiPause();
        KokoMatPrep();
    }
    //This Update() void also helps the project function in-game, but constantly.
    private void FixedUpdate()
    {
        Debug.Log("Health:" + currentHealth + "/" + maxHealth);
        moveVec = KokomiControls.KokomiMovement.MovementInputs.ReadValue<Vector3>();
        rb.AddForce(new Vector3 (moveVec.x, 0 ,moveVec.z) * kokomiSpeed, ForceMode.Force);
        KokomiControls.KokomiActions.Jump.started += _ => KokomiJump();
        JumpingState();
        WhileSkillActive();
        WhileUltActive();
        OnSkillCooldown();
        OnUltCooldown();
        kokoRend.sharedMaterial = kokoMat[kokoMatNum];
        KokomiHPandUltData();
    }
    //This code enables Kokomi's health system, with the aesthetic change of her ultimate.
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
    //Command for Kokomi's Skill
    public void KokomiSkill()
    {
       //Debug.Log("E Skill has been activated");
        jellyfishSkill = Instantiate(jellyfishPrefab, jellyfishSpot.position, Quaternion.identity);
        skillActive = true;
    }
    //Command for Kokomi's Attack
    public void KokomiAttack()
    {
        //Debug.Log("Kokomi has attacked");
        NormalAttack();
        
    }
    //Command for Kokomi's Ultimate
    public void KokomiUltimate()
    {
       // Debug.Log("Ultimate has been activated");
        ultimateActive = true;
    }
    //Command for Kokomi's Jump
    public void KokomiJump()
    {
       //Debug.Log("Kokomi has jumped");
       rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
       isJumping = true;
    }
    //Command for the pause button
    public void KokomiPause()
    {
        Pause();
    }
    //These OnCollisionEnter codes help Kokomi's jump command ignore the attack input. Could not get the other inputs such as Skill and Ultimate to work the same way
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
        }

        if (collision.gameObject.tag == "GroundUnderWater")
        {
            isJumping = false;
        }
        if (collision.gameObject.tag == "Water")
        {
            isJumping = false;
        }
    }
    //General Function for the Pause Screen. Added a disable code to make sure that Kokomi doesn't attack immediately after unpausing.
    public void Pause()
    {
        gamePause.SetActive(true);
        Time.timeScale = 0f;
        KokomiControls.KokomiActions.Attack.Disable();
    }
    //General Function for the Resume Button. Added a Coroutine to make sure that Kokomi can use her attack a second or two after unpausing.
    public void ResumeButton()
    {
        gamePause.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(AfterPause());
    }
    //This is the Coroutine. It sort of works, The timer does not count down accurately.
    public IEnumerator AfterPause()
    {
        yield return new WaitForSeconds(2.0f);
        KokomiControls.KokomiActions.Attack.Enable();

    }
    //The code for the isJumping bool.
    public void JumpingState()
    {
        if (isJumping)
        {
            KokomiControls.KokomiActions.Attack.Disable();
            //KokomiControls.KokomiActions.Skill.Disable(); Jumping was supposed to disable these inputs while the bool for it was on.
            //KokomiControls.KokomiActions.Ultimate.Disable(); It would not work properly.
            KokomiControls.KokomiActions.Jump.Disable();
        }
        else
        {
            KokomiControls.KokomiActions.Attack.Enable();
            //KokomiControls.KokomiActions.Skill.Enable(); So, this is scrapped.
            //KokomiControls.KokomiActions.Ultimate.Enable(); 
            KokomiControls.KokomiActions.Jump.Enable();
        }
        
    }
    //Code for Skill Activity
    public void WhileSkillActive()
    {
        if (skillActive)
        {
            KokomiControls.KokomiActions.Skill.Disable();
            if (activeSkill == null)
            {
                activeSkill = SkillTime();
                StartCoroutine(activeSkill); 
            }
        }
       // else
      //  {
      //      KokomiControls.KokomiActions.Skill.Enable();
      //  }
      // ^^ also Scrapped because it caused a problem where you would activate the skill even on "Cooldown"
      // So, that was for the best. There are "else" codes just like it going down.
    }
    //Code for Skill Cooldown
    public void OnSkillCooldown()
    {
        if (skillCooldown)
        {
            KokomiControls.KokomiActions.Skill.Disable();
        }
       // else
      //  {
      //      if (activeSkill == null!)
       //     KokomiControls.KokomiActions.Skill.Enable();
       // }

    }
    //Timer for Skill Activity
    public IEnumerator SkillTime()
    {
        yield return new WaitForSeconds(skillTime);
        Debug.Log("The Jellyfish should be destroyed");
        Destroy(jellyfishSkill);
        skillCooldown = true;
        skillActive = false;
        activeSkill = null;
        StartCoroutine(SkillCooldownTime());
    }
    //Timer for Skill Cooldown
    public IEnumerator SkillCooldownTime()
    {
        OnSkillCooldown();
        Debug.Log("The Skill is now on cooldown");
        float skillcooldownTime = 20;
        yield return new WaitForSeconds(skillcooldownTime);
        skillCooldown = false;
        KokomiControls.KokomiActions.Skill.Enable();
        Debug.Log("The Skill can now be used again");
    }
    //Code for Ult Activity
    public void WhileUltActive()
    {
        if (ultimateActive)
        {
            KokomiControls.KokomiActions.Ultimate.Disable();
            if (activeUlt == null)
            {
                activeUlt = UltTime();
                StartCoroutine(activeUlt);
            }
            
        }
    ///    else
     ///   {
      ///      KokomiControls.KokomiActions.Ultimate.Enable();
      ///  }
    }
    //Code for Ult Cooldown
    public void OnUltCooldown()
    {
        if (ultimateCooldown)
        {
            KokomiControls.KokomiActions.Ultimate.Disable();
        }
     //   else
       // {
      //      KokomiControls.KokomiActions.Ultimate.Enable();
      //  }
    }
    //Timer for Ult Activity
    public IEnumerator UltTime()
    {
        yield return new WaitForSeconds(ultimateTime);
        Debug.Log("The Ultimate Should End");
        ultimateCooldown = true;
        ultimateActive = false;
        activeUlt = null;
        StartCoroutine(UltCooldownTime());
    }
    //Timer for Ult Cooldown
    public IEnumerator UltCooldownTime()
    {
        Debug.Log("The Ultimate is now on cooldown");
        OnUltCooldown();
        float ultCooldownTime = 10;
        yield return new WaitForSeconds(ultCooldownTime);
        ultimateCooldown = false;
        KokomiControls.KokomiActions.Ultimate.Enable();
        Debug.Log("The Ultimate can now be used again");
    }
    //Code for Kokomi's Attack
    public void NormalAttack()
    {
        if (Time.time-lastFish<attackTime)
        {
            return;
        }
        lastFish = Time.time;
        Instantiate(fishPrefab, transform.position, Quaternion.identity);
    }
    //Code for the Kokomi's Color to change during Ultimate
    public void KokoMatPrep()
    {
        kokoMatNum = 0;
        kokoRend = GetComponent<Renderer>();
        kokoRend.enabled = true;
        kokoRend.sharedMaterial = kokoMat[kokoMatNum];
    }

}
