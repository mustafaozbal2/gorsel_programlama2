using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public GameObject pickup;
    public GameObject ground;
    public int amount;

    private void Start()
    {
        float groundSizeX = ground.transform.localScale.x * 10f;
        float groundSizeZ = ground.transform.localScale.z * 10f;
        
        for(int i = 0; i<amount;i++)
        {
            float randomX = Random.Range(-groundSizeX / 2f, groundSizeX / 2f);
            float randomZ = Random.Range(-groundSizeZ / 2f, groundSizeZ / 2f);

            Vector3 location = new Vector3(randomX, 3f, randomZ);
            Instantiate(pickup, location, Quaternion.identity);
        }
    }
}
