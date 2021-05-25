using UnityEngine;

namespace Player.Raycasting
{
    public class RayCasting : MonoBehaviour
    {
        public static float distanceTarget;

        public float toTarget;

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
            {
                toTarget = hit.distance;
                distanceTarget = toTarget;
            }
        }
    }
}