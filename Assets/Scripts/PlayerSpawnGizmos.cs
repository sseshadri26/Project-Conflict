using System.Collections;
using UnityEngine;

public class PlayerSpawnGizmos : MonoBehaviour
{
    //public .tiff file drop in
    public string image;

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, image, true);
    }
}
