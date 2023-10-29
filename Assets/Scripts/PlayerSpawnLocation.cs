using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerSpawnLocation : MonoBehaviour
{
    [Header("Type : Camera or Action")]
    public string SpawnType = "Camera";
    public Color DisplayColor;
    public float DisplaySize = 0.5f;


    private void OnDrawGizmos()
    {

        Gizmos.color = DisplayColor;
        float gizsize = DisplaySize;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(gizsize, gizsize, gizsize));
        Gizmos.DrawSphere(new Vector3(0, 0, gizsize / 2), gizsize / 5);
        Gizmos.DrawSphere(new Vector3(0, 1.4f, gizsize / 2), gizsize / 10);
        //Handles.Label(transform.position, GizText);
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }
}
