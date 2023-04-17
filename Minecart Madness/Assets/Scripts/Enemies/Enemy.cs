using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] NetworkVariable<int>  currentHealth = new NetworkVariable<int>();
    [SerializeField] HealthBar healthBar;
    [SerializeField] Cart cart;
    [SerializeField] float movementSpeed;
    [SerializeField] int scoreGiven;
    //[SerializeField] LayerMask terrainMask;
    //[SerializeField] LayerMask cartMask;

    float distanceToCart;
    Vector3 movementDirection;
    [SerializeField] float attackRange;
    [SerializeField] float timeBetweenAttacks;
    float timeSinceLastAttack;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Taking damage!");
            TakeDamageServerRPC(5);
        }

        distanceToCart = Vector3.Distance(transform.position, cart.transform.position);
        movementDirection = (cart.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(movementDirection);

        if (distanceToCart < attackRange)
        {
            if(timeSinceLastAttack >= timeBetweenAttacks)
            {
                GetComponent<Animator>().Play("Attack");
                cart.SlowCartByPercentage(50, 1);
                Debug.Log("Attacking!");
                timeSinceLastAttack = 0;
            }
        }
        else
        {
            Move();
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        currentHealth.Value = maxHealth;
        cart = FindObjectOfType<Cart>();
    }

    private void Start()
    {
        cart = FindObjectOfType<Cart>();
    }

    [ServerRpc]
    public void TakeDamageServerRPC(int damage)
    {
        Debug.Log("Taking damage on the server!");

        if (currentHealth.Value == maxHealth)
        {
            healthBar.gameObject.SetActive(true);
        }

        currentHealth.Value -= damage;
        GetComponent<Animator>().Play("Take Damage");

        healthBar.UpdateHealthBar((float)currentHealth.Value / (float)maxHealth);
        Debug.Log("Enemy Health: " + currentHealth.Value);
        if(currentHealth.Value <= 0 && IsHost)
        {
            DieServerRPC();
        }
    }

    [ServerRpc]
    public void DieServerRPC()
    {
        if (!IsServer)
            return;

        GetComponentInParent<NetworkObject>().Despawn();
        FindObjectOfType<GameManager>().ChangeScore(scoreGiven);
        //Play death animation for all clients
    }

    public void Stun(float seconds)
    {

    }

    public void Move()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, movementDirection, out hit, 1))
        {
            if(hit.collider.gameObject.tag == "Terrain")
            {
                movementDirection = new Vector3(movementDirection.x/2, 1, movementDirection.z/2);
            }
        }

        transform.position += movementDirection * movementSpeed * Time.deltaTime;
    }
}
