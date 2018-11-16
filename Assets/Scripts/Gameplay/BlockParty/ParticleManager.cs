using UnityEngine;

public class ParticleManager : MonoBehaviour {
    public GameObject[,] Particles;
    public GameObject ParticlePrefab;
    public GameObject ParticleParent;
    public BoardRaiser BoardRaiser;
    public const int Columns = 6, Rows = 12;

	Vector3 initialPosition;

    void Awake() {
        Particles = new GameObject[Columns, Rows];
        for(int row = 0; row < Rows; row++) {
            for(int column = 0; column < Columns; column++) {
                Vector3 position = new Vector3(column, row, 0f);
                Particles[column, row] = Instantiate(ParticlePrefab, position, Quaternion.identity);
                Particles[column, row].name = "Particle System [" + column + ", " + row + "]";
                Particles[column, row].transform.SetParent(ParticleParent.transform, false);
            }
        }
    }

    void Start() {
        initialPosition = transform.position;
    }

    void Update() {
        Vector3 raiseTranslation = initialPosition + new Vector3(0, BoardRaiser.Elapsed / BoardRaiser.Duration);

		transform.position = raiseTranslation;
    }
}