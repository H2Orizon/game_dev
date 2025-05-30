using UnityEngine;

public class OrbitingDrone : MonoBehaviour
{
    public Transform planetCenter;
    public float orbitSpeed = 15f;

    void Update(){
        if (planetCenter != null){
            transform.RotateAround(planetCenter.position, Vector3.forward, orbitSpeed * Time.deltaTime);
        }
    }
}
