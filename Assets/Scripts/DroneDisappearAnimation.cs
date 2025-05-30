using UnityEngine;

public class DroneDisappearAnimation : MonoBehaviour {
    private Vector3 targetPosition;
    private float shrinkSpeed = 1.5f;
    private float moveSpeed = 5f;
    private bool startDisappearing = false;

    public void StartDisappearing(Vector3 position) {
        targetPosition = position;
        startDisappearing = true;
    }

    void Update() {
        if (!startDisappearing) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f || transform.localScale.magnitude < 0.05f) {
            Destroy(gameObject);
        }
    }
}
