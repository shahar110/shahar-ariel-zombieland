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
    private float attackDist = 4.0f;
    private int health = 2;
    private int scoreVal = 4;
    private bool isAlive = true;
    private bool isAttacking = false;
    private bool isDamaging = false;
    private static int damage = 2;
    private float speed;

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
        nav = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animController = this.GetComponent<Animator>();
        meshCollider = this.GetComponent<MeshCollider>();
    }

    void Update()
    {
        nav.SetDestination(playerTransform.position);

        // attack anim
        if (Vector3.Distance(playerTransform.position, this.transform.position) < attackDist)
        {
            isAttacking = true;
            animController.SetBool("shouldAttack", true);
        }
        else
        {
            isAttacking = false;
            animController.SetBool("shouldAttack", false);
        }

        // Is damaging
        if(isDamaging && isAlive)
        {
            Invoke("incDamage", 5);
        }
        playSounds();
    }

    private void playSounds()
    {
        if (isAttacking) // Attack
        {
            m_audioSource.clip = SoundClips.attackSound;
            if (!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
                m_audioSource.loop = true;
            }
        }
        else // Walk
        {
            m_audioSource.clip = SoundClips.walkSound;
            m_audioSource.Play();
            m_audioSource.loop = true;
            Debug.Log("<< sound walk");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        isDamaging = true;
        Debug.Log("<< OnCollisionEnter zombie, hit with: " + other.gameObject.tag + other.gameObject.name);
        if (other.gameObject.tag == "Bullet")
        {
            health--;
            Debug.Log("<< dec zombie helth");
        }
        else if (other.gameObject.tag == "Player" && isAlive) // Attacking player
        {
            Debug.Log("<< zombie attack player, attack points: ");
            GameManager.decHealth(damage);
        }
        if (health == 0 && isAlive == true)
        {
            Debug.Log("<<  zombie dead");
            onDeath();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
            isDamaging = false;
    }

    void onDeath()
    {
        isAlive = false;
        m_audioSource.clip = SoundClips.damageSound;
        if (!m_audioSource.isPlaying)
        {
            m_audioSource.Play();
        }
        nav.isStopped = true;
        animController.SetBool("isHit", true);
        GameManager.addPoints(scoreVal);
        EnemySpawner.decEnemyCount();
        Destroy(this.gameObject, 10);
    }

    public static void incDamage(int delta)
    {
        damage += delta;
        Debug.Log("<<< inc damage");
    }
}
