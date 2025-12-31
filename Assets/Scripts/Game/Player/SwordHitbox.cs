using Unity.VisualScripting;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float distance = 0.2f;
    Vector2 lastDir = Vector2.up;
    public float yOffset = -0.2f;

    public void UpdatePosition(Vector2 latestVector)
    {
        if(latestVector != Vector2.zero)
        {
        lastDir = latestVector.normalized;
        float finalDistance = distance + yOffset;
        transform.localPosition = (Vector3)(lastDir * finalDistance);
        }
    }
}
