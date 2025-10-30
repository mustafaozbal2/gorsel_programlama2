using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0,45,-45);

    private void LateUpdate(){
        transform.position = player.position + offset;
    }
}
