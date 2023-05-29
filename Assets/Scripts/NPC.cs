using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    // Add properties that all NPCs should have.
    // Health, faction, etc.
    public static int id;
    public int health = 100;
    public string faction;
    public State CurrentState { get { return stateMachine.CurrentState; } }
    public NPC Target { get; set; }
    public float rotationSpeed = 5f;
    public float visionDistance = 10f;
    public float visionAngle = 45f;
    public float attackRange = 10f;
    public int attackDamage = 10;
    public int numberOfRays = 10;  // The number of additional rays to draw in the vision cone
    public Gun gun; // The gun this NPC will use
    public float idleTime = 5f; // Time in Idle state
    public float wanderTime = 5f; // Time in Wander state
    public float wanderRadius = 10f; // Radius of the wander area



    private StateMachine stateMachine;
    private List<NPC> allNPCs = new List<NPC>();

    protected virtual void Start()
    {
        id++;
        stateMachine = new StateMachine();
        // stateMachine.Initialize(new WanderState(this, 20f));
        stateMachine.Initialize(new IdleState(this));
        allNPCs = new List<NPC>(FindObjectsOfType<NPC>());
    }

    protected virtual void Update()
    {
        // Call the update function of the current state.
        stateMachine.Update();
    }

    private bool IsInSight(Ray visionRay, NPC potentialEnemy)
    {
        RaycastHit hit;
        if (Physics.Raycast(visionRay, out hit, visionDistance))
        {
            // If the raycast hit the potential enemy, return true
            return hit.transform == potentialEnemy.transform;
        }
        // Otherwise, the enemy is not in sight
        return false;
    }

    public void ChangeState(State state)
    {
        stateMachine.ChangeState(state);
    }

    // You can add more functions here depending on what actions an NPC can perform
    // e.g., public void Attack(NPC target), public void Flee(), etc.

    private bool IsEnemyInSight(NPC npc, float visionDistance, float visionAngle)
    {
        // Calculate the direction to the NPC.
        Vector3 directionToNpc = npc.transform.position - this.transform.position;

        // Check if the NPC is within the vision distance.
        if (directionToNpc.magnitude > visionDistance)
        {
            return false;
        }

        // Check if the NPC is within the vision angle.
        float angleToNpc = Vector3.Angle(this.transform.forward, directionToNpc);
        if (angleToNpc > visionAngle / 2) // Divide by 2 because angle extends in both directions from the forward direction
        {
            return false;
        }

        return true;
    }

    public bool IsEnemy(NPC npc)
    {
        // Determine whether the given NPC is an enemy. This could be based on faction, context, etc.
        return npc.faction != this.faction;
    }
    public void ReceiveDamage(int damage)
    {
        // Subtract damage from health
        health -= damage;

        // Check if health has dropped to zero or below
        if (health <= 0)
        {
            // Health is zero or below, so this NPC is now dead.
            // Switch to 'Dead' state.
            ChangeState(new DeadState(this));
        }
    }

    public void FireGun()
    {
        if (gun != null && gun.CanShoot())
        {
            gun.Shoot();
        }
    }

    public List<NPC> GetAllVisibleNPCs()
    {
        List<NPC> visibleNPCs = new List<NPC>();

        // Get all NPC instances in the scene.
        NPC[] allNPCs = FindObjectsOfType<NPC>();

        foreach (NPC npc in allNPCs)
        {
            // Check if the NPC is within the vision range and angle.
            if (IsEnemyInSight(npc, visionDistance, visionAngle))
            {
                visibleNPCs.Add(npc);
            }
        }

        return visibleNPCs;
    }

    //To String displays the NPC's ID, health, and faction
    public override string ToString()
    {
        return "NPC " + id + " Health: " + health + " Faction: " + faction;
    }

    
    void OnDrawGizmos()
    {
        // Draw vision range and field of view
        Gizmos.color = Color.green;

        Vector3 npcPosition = this.transform.position;

        // Start direction for the field of view is to the right of the NPC's forward direction, rotated by half the field of view angle
        Vector3 startDirection = Quaternion.Euler(0, -visionAngle / 2, 0) * this.transform.forward;

        // Then draw two lines that delimit the vision area
        Vector3 leftBoundary = Quaternion.Euler(0, visionAngle, 0) * startDirection;
        Vector3 rightBoundary = startDirection;

        Gizmos.DrawRay(npcPosition, leftBoundary * visionDistance);
        Gizmos.DrawRay(npcPosition, rightBoundary * visionDistance);

        // Draw additional rays in the vision cone
        for (int i = 0; i < numberOfRays; i++)
        {
            float interpolationFactor = (float)(i + 1) / (numberOfRays + 1);
            Vector3 interpolatedDirection = Vector3.Slerp(rightBoundary, leftBoundary, interpolationFactor);
            Gizmos.DrawRay(npcPosition, interpolatedDirection * visionDistance);
        }
    } 
}
