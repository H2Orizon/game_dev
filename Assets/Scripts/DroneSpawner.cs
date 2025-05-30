using UnityEngine;

public class DroneSpawner : MonoBehaviour {
    public GameObject dronePrefab;
    public Transform[] planetCenters;
    public Transform dronesParent;

    public float orbitSpeed = 50f;
    public float offset = 0.2f;

    private GameObject[][] spawnedDrones;

    public GameObject[][] GetSpawnedDrones() => spawnedDrones;

    public void SpawnDrones(int[] droneCounts) {
        if (spawnedDrones != null) {
            foreach (var group in spawnedDrones) {
                if (group != null) {
                    foreach (var drone in group) {
                        if (drone != null) {
                            var disappear = drone.GetComponent<DroneDisappearAnimation>();
                            if (disappear != null) {
                                Vector3 planetPos = drone.transform.parent != null
                                    ? drone.transform.parent.position
                                    : drone.transform.position;

                                drone.transform.SetParent(dronesParent);
                                disappear.StartDisappearing(planetPos);
                            } else {
                                Destroy(drone);
                            }
                        }
                    }
                }
            }
        }

        spawnedDrones = new GameObject[planetCenters.Length][];
        int total = 0;
        foreach (int count in droneCounts) total += count;

        for (int i = 0; i < planetCenters.Length; i++) {
            Transform planet = planetCenters[i];

            float radius = 1.0f;
            var sprite = planet.GetComponent<SpriteRenderer>();
            if (sprite != null)
                radius = sprite.bounds.extents.x + offset;

            float percent = (float)droneCounts[i] / total;
            int dronesToSpawn = Mathf.RoundToInt(percent * 10);
            dronesToSpawn = Mathf.Clamp(dronesToSpawn, 0, 10);

            spawnedDrones[i] = new GameObject[dronesToSpawn];

            for (int j = 0; j < dronesToSpawn; j++) {
                float angle = j * Mathf.PI * 2 / dronesToSpawn;
                Vector3 offsetVec = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                Vector3 spawnPos = planet.position + offsetVec;

                GameObject drone = Instantiate(dronePrefab, spawnPos, Quaternion.identity);
                drone.transform.SetParent(dronesParent);
                spawnedDrones[i][j] = drone;
            }
        }
    }

    void Update() {
        if (spawnedDrones == null) return;

        for (int i = 0; i < planetCenters.Length; i++) {
            Transform planet = planetCenters[i];
            if (spawnedDrones[i] == null) continue;

            foreach (var drone in spawnedDrones[i]) {
                if (drone == null) continue;

                drone.transform.RotateAround(planet.position, Vector3.forward, orbitSpeed * Time.deltaTime);
            }
        }
    }
}
