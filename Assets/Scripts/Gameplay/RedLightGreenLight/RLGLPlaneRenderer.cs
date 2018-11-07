using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLGLPlaneRenderer : MonoBehaviour {
	public RLGLGreenLightManager GreenLightManager;
	public MeshRenderer PlaneMeshRenderer;
	public Material GreenMaterial;
	public Material RedMaterial;

	void Update() {
		PlaneMeshRenderer.material = GreenLightManager.GreenLight ? GreenMaterial : RedMaterial;
	}
}
