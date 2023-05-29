using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    void OnSceneGUI()
    {
        NPC npc = (NPC)target;

        if (EditorApplication.isPlaying)
        {
            // Drawing the wander state
            WanderState wanderState = npc.CurrentState as WanderState;

            if (wanderState != null)
            {
                Handles.color = Color.blue;
                Handles.DrawWireDisc(npc.transform.position, Vector3.up, wanderState.WanderRadius);
            }

            // // Draw vision range and field of view
            // Handles.color = Color.green;

            // Vector3 npcPosition = npc.transform.position;

            // // Start direction for the field of view is to the right of the NPC's forward direction, rotated by half the field of view angle
            // Vector3 startDirection = Quaternion.Euler(0, -npc.visionAngle / 2, 0) * npc.transform.forward;

            // // Draw the field of view as a solid arc (a filled "pizza slice")
            // Handles.DrawSolidArc(npcPosition, Vector3.up, startDirection, npc.visionAngle, npc.visionDistance);
        }
    }
}

