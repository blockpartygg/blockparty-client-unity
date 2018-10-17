using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlaneRenderer : MonoBehaviour {
	RLGLGreenLightManager greenLightManager;
	MeshRenderer planeMeshRenderer;
	[SerializeField]
	Material greenMaterial;
	[SerializeField]
	Material redMaterial;

	void Awake() {
		greenLightManager = GameObject.Find("Red Light Green Light").GetComponent<RLGLGreenLightManager>();
		planeMeshRenderer = GetComponent<MeshRenderer>();
	}

	void Update() {
		planeMeshRenderer.material = greenLightManager.GreenLight ? greenMaterial : redMaterial;
	}
}
