using UnityEngine;

public class StarBackgroundFollow : MonoBehaviour
{
    public shipScript playerShip;
    public float parallaxFactor = 1f;

    private Vector3 lastPlayerPosition;

    void Start()
    {
        if (playerShip == null)
            playerShip = FindFirstObjectByType<shipScript>();
        lastPlayerPosition = playerShip.transform.position;
    }

    void Update()
    {
        if (playerShip == null)
            return;

        Vector3 playerDelta = playerShip.transform.position - lastPlayerPosition;

        transform.position -= playerDelta * parallaxFactor;
        lastPlayerPosition = playerShip.transform.position;
    }
}