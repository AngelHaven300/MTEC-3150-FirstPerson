using UnityEngine;

public class overheadcam : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
    }
}
