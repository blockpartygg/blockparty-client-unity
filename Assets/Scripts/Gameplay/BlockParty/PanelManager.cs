using UnityEngine;

public class PanelManager : MonoBehaviour {
	public Panel[,] Panels;
    public Panel PanelPrefab;
    public GameObject PanelParent;
    public BoardRaiser BoardRaiser;
    public const int Columns = 6, Rows = 12;

    Vector3 initialPosition;

    void Awake() {
        Panels = new Panel[Columns, Rows];
        for(int row = 0; row < Rows; row++) {
            for(int column = 0; column < Columns; column++) {
                Vector3 position = new Vector3(column, row, 0f);
                Panels[column, row] = Instantiate(PanelPrefab, position, Quaternion.identity);
                Panels[column, row].name = "Panel [" + column + ", " + row + "]";
                Panels[column, row].transform.SetParent(PanelParent.transform, false);
                Panels[column, row].Column = column;
                Panels[column, row].Row = row;
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