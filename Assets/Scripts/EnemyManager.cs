using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    private NavMeshAgent nav;
    private Transform enemyTransofrm;
    private Transform playerTransform;
    private Animator animController;
    private MeshCollider meshCollider;

    public float distFromPlayer;
    public float attackAnimDist = 4.0f;
    public float damageDist = 2.0f;
    private int health = 2;
    private int scoreVal = 200;
    public bool isAlive = true;
    public bool isAttacking = false;
    private static int damage = 10;
    private float speed;
    private float attackDamageTimer = 0f;
    private float attackDamageInterval = 5.0f;

    [System.Serializable]
    public class soundClips
    {
        public AudioClip attackSound;
        public AudioClip walkSound;
        public AudioClip damageSound;
    }
    public soundClips SoundClips;
    //Main audio source
    public AudioSource m_audioSource;

    void Awake()
    {
        enemyTransofrm = this.transform;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();

        animController = this.GetComponent<Animator>();
        nav.SetDestination(playerTransform.position);
        
        attackAnimDist = 4.0f;
        damageDist = 2.0f;
        health = 2;
        scoreVal = 200;
        isAlive = true;
        isAttacking = false;
        damage = 10;
        attackDamageTimer = 0f;
        attackDamageInterval = 5.0f;
    }

    void Update()
    {
        distFromPlayer = Vector3.Distance(playerTransform.position, this.transform.position);
        attackDamageTimer += Time.deltaTime;

        if (isAlive)
        {
            if (distFromPlayer <= attackAnimDist) // In attack range
            {
                if (!isAttacking) // Start attacking
                    startAttacking(distFromPlayer);
                else if (distFromPlayer <= damageDist)// Already attacking
                    attacking();
            }
            else // Out of range
            {
                if (!isAttacking)
                    walking();
                else
                {
                    walking();
                    isAttacking = false;
                }
            }
        }
    }

    private void attacking()
    {
        // Debug.Log("<< zombie damage interval");
        if (attackDamageTimer >= attackDamageInterval)
        {
            GameManager.decHealth(damage);
            attackDamageTimer = 0f;

        }
    }

    private void startAttacking(float distnace)
    {
        // Debug.Log("<< zombie start attacking player");
        isAttacking = true;
        animController.SetBool("shouldAttack", true);
        if (distnace <= damageDist)
        {
            nav.isStopped = true;
            // Debug.Log("<< nav.isStopped = true");
        }
            
        m_audioSource.clip = SoundClips.attackSound;
        if (!m_audioSource.isPlaying)
        {
            // Debug.Log(this.name + "<<: start loop sound attack");
            m_audioSource.Play();
            m_audioSource.loop = true;
        }
    }

    private void walking()
    {
        isAttacking = false;
        animController.SetBool("shouldAttack", false);

        if (!m_audioSource.isPlaying)
        {
            m_audioSource.clip = SoundClips.walkSound;
            m_audioSource.Play();
            m_audioSource.loop = true;
            // Debug.Log(this.name + "<< start walk sound loop");
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("<< OnCollisionEnter zombie, hit with: " + other.gameObject.tag + other.gameObject.name);
        if (other.gameObject.tag == "Bullet")
        {
            health--;
            // Debug.Log("<< dec zombie helth");
        }
        if (health == 0 && isAlive == true)
        {
            // Debug.Log("<<  zombie dead");
            nav.isStopped = true;
            onDeath();
        }
    }

    void onDeath()
    {
        // Debug.Log("<< enemyManager onDeath()");
        isAlive = false;
        m_audioSource.loop = false;
        m_audioSource.clip = SoundClips.damageSound;
        m_audioSource.Play();
        // Debug.Log(this.name + "<<: sound damage");
        animController.SetBool("isHit", true);
        GameManager.addPoints(scoreVal);
        EnemySpawner.decEnemyCount();
        Destroy(this.gameObject, 10);
    }
}