using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    public float gravity = -9.8f;

    public void Attract(Transform body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        body.GetComponent<Rigidbody>().AddForce(gravityUp * PhysicsManager.Gravity);
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
    }

    public Vector3 GravityUp(Transform body) => (body.position - transform.position).normalized;
}
