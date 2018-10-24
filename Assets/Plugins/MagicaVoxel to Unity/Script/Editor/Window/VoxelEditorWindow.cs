﻿namespace VoxeltoUnity {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using Moenen.Saving;
	using Moenen;
	using UnityEngine.SceneManagement;
	using UnityEditor.SceneManagement;


	// Main Part
	public partial class VoxelEditorWindow : MoenenEditorWindow {



		#region --- SUB ---



		private class Int3 {

			public int this[int axis] {
				get {
					return axis == 0 ? x : axis == 1 ? y : z;
				}
				set {
					if (axis == 0) {
						x = value;
					} else if (axis == 1) {
						y = value;
					} else {
						z = value;
					}
				}
			}

			public int x;
			public int y;
			public int z;
			public Int3 (int _x, int _y, int _z) {
				x = _x;
				y = _y;
				z = _z;
			}
			public static implicit operator Int3 (Vector3 v) {
				return new Int3(
					Mathf.RoundToInt(v.x),
					Mathf.RoundToInt(v.y),
					Mathf.RoundToInt(v.z)
				);
			}
			public static implicit operator bool (Int3 i) {
				return i != null;
			}
			public static Int3 Min (Int3 a, Int3 b) {
				return new Int3(
					Mathf.Min(a.x, b.x),
					Mathf.Min(a.y, b.y),
					Mathf.Min(a.z, b.z)
				);
			}
			public static Int3 Max (Int3 a, Int3 b) {
				return new Int3(
					Mathf.Max(a.x, b.x),
					Mathf.Max(a.y, b.y),
					Mathf.Max(a.z, b.z)
				);
			}
			public override string ToString () {
				return string.Format("({0}, {1}, {2})", x, y, z);
			}
		}



		#endregion



		#region --- VAR ---


		// Global
		private const int RENDER_TEXTURE_HEIGHT = 512;
		private static VoxelEditorWindow Main = null;


		// Short
		private bool RigAvailable {
			get {
				return !string.IsNullOrEmpty(VoxelFilePath) && Util.GetExtension(VoxelFilePath) == ".vox";
			}
		}


		// UI Data
		private VoxelData Data = null;
		private Texture2D FileIcon = null;
		private Int3 ModelSize = new Int3(0, 0, 0);
		private string VoxelFilePath = "";
		private int CurrentModelIndex = 0;
		private bool DataDirty = false;


		// Data
		private Vector2 MasterScrollPosition;
		private Rect ViewRect;
		private Dictionary<int, bool> NodeOpen = new Dictionary<int, bool>();
		private bool ColorfulTitle = true;
		private bool DataPanelOpen = false;
		private bool DataPalettePanelOpen = false;
		private bool DataNodePanelOpen = false;
		private bool DataMaterialPanelOpen = false;
		private bool DataRigPanelOpen = false;


		// Save
		private EditorSavingBool ShowStateInfo = new EditorSavingBool("VEditor.ShowStateInfo", true);


		#endregion




		#region --- MSG ---



		[MenuItem("Tools/Voxel Editor")]
		public static void OpenWindow () {
			var window = GetWindow<VoxelEditorWindow>("Voxel Editor", true);
			window.minSize = new Vector2(900, 800);
			window.maxSize = new Vector2(1200, 1000);
			window.position = new Rect(window.position) {
				width = window.minSize.x,
				height = window.minSize.y,
			};
		}



		[InitializeOnLoadMethod]
		private static void EditorInit () {
			var oldRoot = GameObject.Find(ROOT_NAME);
			if (oldRoot) {
				DestroyImmediate(oldRoot, false);
			}
			EditorSceneManager.sceneSaved += (scene) => {
				if (Main) {
					Main.SaveData();
					Main.Repaint();
				}
			};
		}



		private void OnEnable () {
			Main = this;
			wantsMouseMove = true;
			wantsMouseEnterLeaveWindow = true;
			DataPanelOpen = false;
			QUAD_SHADER = Shader.Find("Sprites/Default");
			GROUND_SHADER = Shader.Find("Unlit/Color");
			COLOR_SHADER_ID = Shader.PropertyToID("_Color");
			LAYER_ID = LayerMask.NameToLayer("Water");
			LAYER_ID_ALPHA = LayerMask.NameToLayer("TransparentFX");
			LAYER_MASK = LayerMask.GetMask("Water");
			LAYER_MASK_ALPHA = LayerMask.GetMask("TransparentFX");
			EditorLoad();
		}



		private void OnFocus () {
			EditorLoad();
		}



		private void OnDestroy () {
			RemoveRoot();
			Main = null;
		}



		private void OnGUI () {

			if (Data) {

				HeaderGUI();

				MasterScrollPosition = GUILayout.BeginScrollView(MasterScrollPosition, GUI.skin.scrollView);

				ViewGUI();
				StateInfoGUI();
				EditToolGUI();

				CubeGUI();
				HighlightGUI();
				SceneBoneNameGUI();
				PanelGUI();

				RigEditingGUI();
				BoneMouseGUI();
				SpriteEditingGUI();

				WeightPointGUI();

				DataGUI();

				KeyboardGUI();

				GUILayout.EndScrollView();
			} else {
				PickGUI();
			}

			DragInFileGUI();

			if (GUI.changed) {
				EditorSave();
			}

			if (Event.current.type == EventType.MouseDown) {
				GUI.FocusControl(null);
				Repaint();
			}

		}




		#endregion




		#region --- GUI ---



		private void HeaderGUI () {

			const string MAIN_TITLE = "Voxel Editor";
			const string MAIN_TITLE_RICH = "<color=#ff3333>V</color><color=#ffcc00>o</color><color=#ffff33>x</color><color=#33ff33>e</color><color=#33ccff>l</color><color=#eeeeee> Editor</color>";

			LayoutH(() => {

				// Icon
				if (FileIcon) {
					GUI.DrawTexture(GUIRect(24, 24), FileIcon);
					Space(4);
				}

				// Label
				GUI.Label(GUIRect(56 + 80, 24), Util.GetNameWithExtension(VoxelFilePath), new GUIStyle(GUI.skin.label) {
					alignment = TextAnchor.MiddleLeft,
				});

				// Title
				GUIStyle style = new GUIStyle() {
					alignment = TextAnchor.LowerCenter,
					fontSize = 12,
					fontStyle = FontStyle.Bold
				};
				style.normal.textColor = Color.white;
				style.richText = true;

				Rect rect = GUIRect(0, 18);

				GUIStyle shadowStyle = new GUIStyle(style) {
					richText = false
				};

				EditorGUI.DropShadowLabel(rect, MAIN_TITLE, shadowStyle);
				GUI.Label(rect, ColorfulTitle ? MAIN_TITLE_RICH : MAIN_TITLE, style);


				GUIRect(80, 24);

				// Save Data Button
				if (ColorfulButton(GUIRect(56, 24), "Save", DataDirty ? new Color(0.6f, 1f, 0.7f, 1) : Color.white, new GUIStyle(EditorStyles.miniButtonLeft) { fontSize = 11 })) {
					SaveData();
					Repaint();
				}

				// Close Button
				if (ColorfulButton(GUIRect(24, 24), "×", new Color(1, 0.6f, 0.6f, 1), new GUIStyle(EditorStyles.miniButtonMid) { fontSize = 14 })) {
					EditorApplication.delayCall += () => { CloseTarget(); };
				}

			}, false, new GUIStyle() {
				padding = new RectOffset(0, 0, 0, 0),
				margin = new RectOffset(28, 0, 12, 0),
			});
			Space(4);
		}



		private void PickGUI () {
			LayoutV(() => {

				GUI.Label(GUIRect(0, 48), "Pick a vox/qb file to edit it.\nDrag vox/qb file from project view to this window or use the buttons below.", new GUIStyle(GUI.skin.label) {
					alignment = TextAnchor.MiddleCenter,
					fontSize = 12,
				});

				Space(12);

				LayoutH(() => {

					GUIRect(0, 36);

					if (GUI.Button(GUIRect(240, 36), "Pick VOX File")) {
						EditorApplication.delayCall += () => {
							PickTarget(true);
						};
					}

					Space(12);

					if (GUI.Button(GUIRect(240, 36), "Pick QB File")) {
						EditorApplication.delayCall += () => {
							PickTarget(false);
						};
					}

					GUIRect(0, 36);

				});
			}, false, new GUIStyle() {
				padding = new RectOffset(120, 120, 180, 120),
				margin = new RectOffset(28, 20, 8, 4),
			});


		}



		private void DragInFileGUI () {
			if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform) {
				if (DragAndDrop.paths.Length > 0) {
					var path = DragAndDrop.paths[0];
					if (!string.IsNullOrEmpty(path)) {
						var ex = Util.GetExtension(path);
						if (ex == ".vox" || ex == ".qb") {
							if (Event.current.type == EventType.DragPerform) {
								bool set = true;
								if (Data) {
									set = SetTargetSafeDialog();
								}
								if (set) {
									SetTargetAt(path);
								}
								Repaint();
								DragAndDrop.AcceptDrag();
							} else {
								DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
							}
						}
					}
				}
			}
		}



		private void ViewGUI () {

			var targetTexture = Camera.targetTexture;
			int textureWidth = targetTexture.width;
			int currentViewWidth = (int)EditorGUIUtility.currentViewWidth - 2;
			if (Data && Camera && CameraRoot && targetTexture) {

				// Width Check
				if (targetTexture.width != currentViewWidth) {
					Camera.targetTexture = new RenderTexture(currentViewWidth, RENDER_TEXTURE_HEIGHT, 24) { antiAliasing = 2, };
					AlphaCamera.targetTexture = new RenderTexture(currentViewWidth, RENDER_TEXTURE_HEIGHT, 24) { antiAliasing = 2, };
					textureWidth = currentViewWidth;
					RefreshCubeTransform();
					Camera.Render();
					Repaint();
				}

				Space(4);

				// Toolbar
				ToolbarGUI();

				// View
				ViewRect = GUIRect(textureWidth, RENDER_TEXTURE_HEIGHT);
				GUI.DrawTexture(ViewRect, Camera.targetTexture);
				var oldC = GUI.color;
				GUI.color = new Color(1, 1, 1, 0.309f);
				GUI.DrawTexture(ViewRect, AlphaCamera.targetTexture);
				GUI.color = oldC;

				// Camera
				if (Event.current.type == EventType.MouseDrag) {
					// Mosue Right Drag
					if (Event.current.button == 1) {
						if (Event.current.alt) {
							// Drag Zoom
							SetCameraSize(Mathf.Clamp(
								Camera.orthographicSize * (1f + Event.current.delta.y * 0.01f),
								CameraSizeMin, CameraSizeMax
							));
							HighlightTF.gameObject.SetActive(false);
							RefreshCubeTransform();
							Repaint();
						} else {
							// Rotate
							Vector2 del = Event.current.delta * 0.4f;
							float angle = Camera.transform.rotation.eulerAngles.x + del.y;
							angle = angle > 89 && angle < 180 ? 89 : angle;
							angle = angle > 180 && angle < 271 ? 271 : angle;
							CameraRoot.rotation = Quaternion.Euler(angle, CameraRoot.rotation.eulerAngles.y + del.x, 0f);
							HighlightTF.gameObject.SetActive(false);
							RefreshCubeTransform();
							Repaint();
						}
					} else if (Event.current.button == 2 || (Event.current.button == 0 && Event.current.alt)) {
						// Move
						Vector3 del = Event.current.delta * Camera.orthographicSize * 0.005f;
						del = CameraRoot.rotation * new Vector3(-del.x, del.y, 0f);
						CameraRoot.localPosition += del;
						HighlightTF.gameObject.SetActive(false);
						Repaint();
					}
				} else if (Event.current.isScrollWheel) {
					// Wheel Zoom
					if (ViewRect.Contains(Event.current.mousePosition)) {
						SetCameraSize(Mathf.Clamp(
							Camera.orthographicSize * (1f + Event.current.delta.y * 0.04f),
							CameraSizeMin, CameraSizeMax
						));
						HighlightTF.gameObject.SetActive(false);
						Event.current.Use();
						RefreshCubeTransform();
						Repaint();
					}
				} else if (Event.current.type == EventType.MouseDown) {
					if (Event.current.button == 1 && Event.current.shift) {
						// Focus
						ViewCastHit((hit) => {
							if (hit.transform.name != "Cube") {
								CameraRoot.position = hit.point;
								Repaint();
							}
						});
					}
				}

			} else {
				EditorGUI.HelpBox(GUIRect(0, RENDER_TEXTURE_HEIGHT), "No camera.\nPlease open the voxel editor again.", MessageType.Warning);
			}
		}



		private void EditToolGUI () {
			if (IsRigging || IsSpriting) { return; }

			var rect = new Rect(ViewRect.x + 12, ViewRect.y + ViewRect.height - 18 - 6, 50, 18);

			// Flip
			GUI.Label(rect, "Flip");
			rect.x += rect.width;
			rect.width = 24;
			if (GUI.Button(rect, "X", EditorStyles.miniButtonLeft)) {
				Flip(0);
			}
			rect.x += rect.width;
			if (GUI.Button(rect, "Y", EditorStyles.miniButtonMid)) {
				Flip(1);
			}
			rect.x += rect.width;
			if (GUI.Button(rect, "Z", EditorStyles.miniButtonRight)) {
				Flip(2);
			}
			rect.x = ViewRect.x + 12;
			rect.y -= rect.height + 6;

			// Rot
			rect.width = 50;
			GUI.Label(rect, "Rotate");
			rect.x += rect.width;
			rect.width = 24;
			if (GUI.Button(rect, "X", EditorStyles.miniButtonLeft)) {
				Rotate(0);
			}
			rect.x += rect.width;
			if (GUI.Button(rect, "Y", EditorStyles.miniButtonMid)) {
				Rotate(1);
			}
			rect.x += rect.width;
			if (GUI.Button(rect, "Z", EditorStyles.miniButtonRight)) {
				Rotate(2);
			}

		}



		private void ToolbarGUI () {
			// Toolbar
			LayoutH(() => {

				// Data Button
				var tempRect = new Rect();
				if (GUI.Button(tempRect = GUIRect(70, 18), "Debug Data", EditorStyles.toolbarButton)) {
					DataPanelOpen = !DataPanelOpen;
				}
				if (DataPanelOpen) {
					ColorBlock(tempRect);
				}


				// State Info Button
				if (GUI.Button(tempRect = GUIRect(45, 18), "State", EditorStyles.toolbarButton)) {
					ShowStateInfo.Value = !ShowStateInfo;
				}
				if (ShowStateInfo) {
					ColorBlock(tempRect);
				}


				// Bone Name Button
				if (IsRigging) {
					if (GUI.Button(tempRect = GUIRect(75, 18), "Bone Name", EditorStyles.toolbarButton)) {
						ShowBoneName.Value = !ShowBoneName;
					}
					if (ShowBoneName) {
						ColorBlock(tempRect);
					}
				}

				Space(16);

				// Tag
				if (Data.Voxels.Count > 1) {
					LayoutH(() => {
						Rect barRect = GUIRect(0, 18);
						int len = Data.Voxels.Count;
						float tagWidth = Mathf.Min(40f, barRect.width / len);
						for (int i = 0; i < len; i++) {
							if (ColorfulButton(
								new Rect(barRect.x + i * tagWidth, barRect.y, tagWidth, 18),
								i.ToString(),
								i == CurrentModelIndex ? new Color(0.6f, 0.8f, 0.7f, 0.3f) : Color.white,
								EditorStyles.toolbarButton
							)) {
								SwitchModel(i);
							}
						}
					});
					Space(60);
				} else {
					GUIRect(0, 18);
				}

				// Help
				if (GUI.Button(GUIRect(18, 18), "?", EditorStyles.toolbarButton)) {
					EditorUtil.Dialog(
						"Help",
						"[Right drag] Rotate camera.\n" +
						"[Middle drag] or [Alt + Left drag] Move camera\n" +
						"[Mouse wheel] or [Alt + Right drag] Zoom camera\n" +
						"[Shift + right click] Focus camera\n" +
						"[Shift + S] Show/Hide state info\n" +
						"[P] Start/stop paint selecting bone\n" +
						"[ESC] Stop painting weight point\n" +
						"[V] [B] [N] Set brush type\n" +
						"[T] [R] Set attach or erase\n" +
						"[(Hold)Shift] Switch between attach and erase\n" +
						"[C] Reset Camera\n" +
						"[G] Show background box or not",
						"OK"
					);
				}

				Space(12);

			}, false, EditorStyles.toolbar);

		}



		private void PanelGUI () {

			const int WIDTH = 158;
			const int HEIGHT = 30;
			const int GAP = 3;

			var buttonStyle = new GUIStyle(EditorStyles.miniButtonLeft) {
				fontSize = 11,
			};
			var dotStyle = new GUIStyle(GUI.skin.label) {
				richText = true,
				alignment = TextAnchor.MiddleLeft,
			};

			// Snapshot
			Rect rect = new Rect(ViewRect.x + ViewRect.width - WIDTH, ViewRect.y + ViewRect.height - HEIGHT - GAP, WIDTH, HEIGHT);
			if (IsSpriting) {

				// Spriting
				rect.width = WIDTH * 0.618f;
				rect.x = ViewRect.x + ViewRect.width - rect.width;
				if (GUI.Button(rect, "Back", buttonStyle)) {
					SwitchEditMode(null);
					Repaint();
				}
				rect.width = WIDTH;
				rect.x = ViewRect.x + ViewRect.width - rect.width;
				rect.y -= HEIGHT + GAP * 2;
				if (GUI.Button(rect, "   Create Screenshot", buttonStyle)) {
					CreateScreenShot();
					Repaint();
				}
				GUI.Label(rect, "  <color=#ffcc00>●</color>", dotStyle);
				rect.y -= HEIGHT + GAP;
				if (GUI.Button(rect, "   Create 8bit Sprite", buttonStyle)) {
					CreateSprite(true);
					Repaint();
				}
				GUI.Label(rect, "  <color=#ffcc00>●</color>", dotStyle);
				rect.y -= HEIGHT + GAP;

				if (GUI.Button(rect, "   Create 2D Sprite", buttonStyle)) {
					CreateSprite(false);
					Repaint();
				}
				GUI.Label(rect, "  <color=#ffcc00>●</color>", dotStyle);
				rect.y -= HEIGHT + GAP;

			} else if (IsRigging) {

				// Rig
				rect.width = WIDTH * 0.618f;
				rect.x = ViewRect.x + ViewRect.width - rect.width;
				if (GUI.Button(rect, "Back", buttonStyle)) {
					SwitchEditMode(null);
					Repaint();
				}
				rect.width = WIDTH;
				rect.x = ViewRect.x + ViewRect.width - rect.width;
				rect.y -= HEIGHT + GAP * 2;
				if (GUI.Button(rect, "   Create Rig Prefab", buttonStyle)) {
					CreateRigPrefab(false);
					Repaint();
				}
				GUI.Label(rect, "  <color=#33ccff>●</color>", dotStyle);
				rect.y -= HEIGHT + GAP;
				if (GUI.Button(rect, "   Create Avatar Prefab", buttonStyle)) {
					CreateRigPrefab(true);
					Repaint();
				}
				GUI.Label(rect, "  <color=#33ccff>●</color>", dotStyle);
				rect.y -= HEIGHT + GAP;
				if (GUI.Button(rect, "   Export Rig Json", buttonStyle)) {
					ExportRigData();
					Repaint();
				}
				GUI.Label(rect, "  <color=#cccccc>●</color>", dotStyle);
				rect.y -= HEIGHT + GAP;
				if (GUI.Button(rect, "   Import Rig Json", buttonStyle)) {
					ImportRigData();
					Repaint();
				}
				GUI.Label(rect, "  <color=#cccccc>●</color>", dotStyle);
				rect.y -= HEIGHT + GAP;

			} else {

				rect.width = WIDTH * 1.2f;
				rect.height = HEIGHT * 1.1f;
				rect.x = ViewRect.x + ViewRect.width - rect.width;
				// Sprite
				if (GUI.Button(rect, "Sprite", buttonStyle)) {
					SwitchEditMode(false);
					Repaint();
				}
				GUI.Label(rect, "      <color=#ffcc00>●</color>", dotStyle);
				rect.y -= rect.height + GAP;

				// Rig
				if (RigAvailable) {
					if (GUI.Button(rect, "Rigging", buttonStyle)) {
						SwitchEditMode(true);
						Repaint();
					}
					GUI.Label(rect, "      <color=#33ccff>●</color>", dotStyle);
					rect.y -= rect.height + GAP;
				}

			}


			// Rot Slider
			if (CameraRoot) {

				const int OFFSET_X = -12;

				rect.width = WIDTH - 48;
				rect.height = 18;
				rect.x = ViewRect.x + ViewRect.width - rect.width + OFFSET_X;
				rect.y = ViewRect.y + 60;

				Vector3 oldAngel = CameraRoot.rotation.eulerAngles;
				Vector3 newAngel = oldAngel;

				// Slider
				newAngel.x = Mathf.Repeat(GUI.HorizontalSlider(rect, Mathf.Repeat(CameraRoot.rotation.eulerAngles.x + 90f, 360f), 0f, 180f) - 90f, 360f);
				rect.y += 20;
				newAngel.y = GUI.HorizontalSlider(rect, CameraRoot.rotation.eulerAngles.y, 0f, 360f);

				if (newAngel != oldAngel) {
					if (newAngel.x != oldAngel.x) {
						newAngel.x = Mathf.Round(newAngel.x / 18f) * 18f;
					}
					if (newAngel.y != oldAngel.y) {
						newAngel.y = Mathf.Round(newAngel.y / 36f) * 36f;
					}
					CameraRoot.rotation = Quaternion.Euler(newAngel);
					RefreshCubeTransform();
					Repaint();
				}

				// Label
				rect.x -= 30;
				rect.y -= 20;
				GUI.Label(rect, newAngel.x.ToString("00"));
				rect.y += 20;
				GUI.Label(rect, newAngel.y.ToString("00"));

				// Reset Camera Button
				rect.x = ViewRect.x + ViewRect.width - 28 + OFFSET_X;
				rect.y = ViewRect.y + 6;
				rect.width = 22;
				rect.height = 22;
				if (GUI.Button(rect, "C", EditorStyles.miniButton)) {
					ResetCamera();
					Repaint();
				}

				// Box Button
				rect.y += 24;
				if (GUI.Button(rect, "G", EditorStyles.miniButton)) {
					SetBoxBackgroundActive(!ShowBackgroundBox);
					Repaint();
				}

			}


		}



		private void DataGUI () {

			if (!Data || !DataPanelOpen) { return; }

			LayoutV(() => {

				// Palette
				AltLayoutF(() => {
					bool oldE = GUI.enabled;
					GUI.enabled = false;
					const int COLUMN_COUNT = 32;
					int rowCount = Mathf.CeilToInt(Data.Palette.Count / (float)COLUMN_COUNT);
					for (int i = 0; i < rowCount; i++) {
						LayoutH(() => {
							for (int j = 0; j < COLUMN_COUNT; j++) {
								int index = i * COLUMN_COUNT + j;
								if (index >= Data.Palette.Count) {
									break;
								}
#if UNITY_2018
								EditorGUI.ColorField(GUIRect(18, 18), GUIContent.none, Data.Palette[index], false, false, false);
#else
								EditorGUI.ColorField(GUIRect(18, 18), GUIContent.none, Data.Palette[index], false, false, false, null);
#endif
							}
						});
						Space(2);
					}
					GUI.enabled = oldE;
				}, string.Format("Palette [{0}]", Data.Palette.Count), ref DataPalettePanelOpen, false, new GUIStyle() {
					padding = new RectOffset(18, 0, 0, 0),
				});

				// Node
				AltLayoutF(() => {
					NodGUI(0);
				}, string.Format("Node [{0}]", Data.Transforms.Count), ref DataNodePanelOpen, false, new GUIStyle() {
					padding = new RectOffset(18, 0, 0, 0),
				});

				// Material
				AltLayoutF(() => {
					// Containt
					for (int i = 0; i < Data.Materials.Count; i++) {
						if (i % 20 == 0) {
							// Labels
							LayoutH(() => {
								GUIRect(36, 18);
								GUI.Label(GUIRect(62, 18), "Type");
								Space(12);
								GUI.Label(GUIRect(48, 18), "Weight");
								GUI.Label(GUIRect(48, 18), "Rough");
								GUI.Label(GUIRect(48, 18), "Spec");
								GUI.Label(GUIRect(48, 18), "Ior");
								GUI.Label(GUIRect(48, 18), "Att");
								GUI.Label(GUIRect(48, 18), "Flux");
								GUI.Label(GUIRect(48, 18), "Glow");
								GUI.Label(GUIRect(48, 18), "Plastic");
							});
						}
						MaterialGUI(Data.Materials[i]);
					}
				}, string.Format("Material [{0}]", Data.Materials.Count), ref DataMaterialPanelOpen, false, new GUIStyle() {
					padding = new RectOffset(18, 0, 0, 0),
				});

				// Rig
				AltLayoutF(() => {
					foreach (var r in Data.Rigs) {
						RigDataGUI(r.Key, r.Value);
					}
				}, string.Format("Rigging [{0}]", Data.Rigs.Count), ref DataRigPanelOpen, false, new GUIStyle() {
					padding = new RectOffset(18, 0, 0, 0),
				});

			}, false, new GUIStyle(GUI.skin.box) {
				padding = new RectOffset(9, 6, 4, 4),
				margin = new RectOffset(14, 20, 20, 4),
			});


		}



		private void StateInfoGUI () {
			if (!ShowStateInfo) { return; }

			var rect = ViewRect;
			rect.x += 12;
			rect.y += 12;
			rect.width = 220;
			rect.height = 18;

			if (!IsRigging) {
				// Normal && Spriting
				GUI.Label(rect, "Name\t" + Util.GetNameWithExtension(VoxelFilePath));
				rect.y += rect.height + 2;

				if (Data.Voxels.Count > 1) {
					GUI.Label(rect, "Model\t" + CurrentModelIndex + "/" + (Data.Voxels.Count - 1));
					rect.y += rect.height + 2;
				}

				GUI.Label(rect, "Size\t" + string.Format("{0}×{1}×{2}", ModelSize.x, ModelSize.y, ModelSize.z));
				rect.y += rect.height + 2;




			} else {

				// Rigging

				int boneIndex = Mathf.Max(SelectingBoneIndex, PaintingBoneIndex);
				var bone = TheBones != null && boneIndex != -1 ? TheBones[boneIndex] : null;

				// Bone Name
				GUI.Label(rect, "Name\t" + (bone ? bone.Name : "---"));
				rect.y += rect.height + 2;

				// Bone Index
				GUI.Label(rect, "Index\t" + (bone ? boneIndex.ToString() : "---"));
				rect.y += rect.height + 2;

				// Bone Position
				GUI.Label(rect, "X\t" + (bone ? bone.PositionX.ToString() : "---"));
				rect.y += rect.height + 2;
				GUI.Label(rect, "Y\t" + (bone ? bone.PositionY.ToString() : "---"));
				rect.y += rect.height + 2;
				GUI.Label(rect, "Z\t" + (bone ? bone.PositionZ.ToString() : "---"));
				rect.y += rect.height + 2;

				if (PaintingBoneIndex != -1) {

					// Cursor
					GUI.Label(rect, "Cursor\t" + (HoveringVoxelPosition != null ? HoveringVoxelPosition.ToString() : "---"));
					rect.y += rect.height + 2;

					// Bones
					string bonesLabelA = "---";
					string bonesLabelB = "---";
					var weight = TheWeights != null && HoveringVoxelPosition && HoveredVoxelPosition && Util.InRange(HoveredVoxelPosition.x, HoveredVoxelPosition.y, HoveredVoxelPosition.z, ModelSize.x, ModelSize.y, ModelSize.z) ? TheWeights[HoveredVoxelPosition.x, HoveredVoxelPosition.y, HoveredVoxelPosition.z] : null;
					if (weight != null && TheBones != null) {
						bonesLabelA = (weight.BoneIndexA != -1 ? TheBones[weight.BoneIndexA].Name : "---");
						bonesLabelB = (weight.BoneIndexB != -1 ? TheBones[weight.BoneIndexB].Name : "---");
					}
					GUI.Label(rect, "Bones A\t" + bonesLabelA);
					rect.y += rect.height + 2;
					GUI.Label(rect, "Bones B\t" + bonesLabelB);
					rect.y += rect.height + 2;


				}

			}

		}



		private void KeyboardGUI () {
			if (Event.current.isKey && Event.current.type == EventType.KeyDown && !EditorUtil.IsTypingInGUI()) {
				var key = Event.current.keyCode;
				switch (key) {
					case KeyCode.Escape:
						if (PaintingBoneIndex != -1) {
							int index = PaintingBoneIndex;
							StopPaintBone();
							SelectBone(index);
							SetWeightPointQuadDirty();
							Repaint();
						} else if (SelectingBoneIndex != -1) {
							DeselectBone();
							SetWeightPointQuadDirty();
							Repaint();
						}
						break;
					case KeyCode.N:
						if (PaintingBoneIndex != -1) {
							SetBrushType(BrushType.Rect);
							Repaint();
						}
						Repaint();
						break;
					case KeyCode.S:
						if (Event.current.shift) {
							ShowStateInfo.Value = !ShowStateInfo;
							Repaint();
						}
						break;
					case KeyCode.Delete:
						if (SelectingBoneIndex != -1) {
							DeleteBoneDialog(SelectingBoneIndex);
						}
						break;
					case KeyCode.RightArrow:
						int index0 = Mathf.Max(SelectingBoneIndex, PaintingBoneIndex);
						if (index0 != -1) {
							var bones = TheBones;
							if (bones != null) {
								SetBoneOpen(bones[index0], true);
								Repaint();
							}
						}
						break;
					case KeyCode.LeftArrow:
						int index1 = Mathf.Max(SelectingBoneIndex, PaintingBoneIndex);
						if (index1 != -1) {
							var bones = TheBones;
							if (bones != null) {
								SetBoneOpen(bones[index1], false);
								Repaint();
							}
						}
						break;
					case KeyCode.V:
						if (PaintingBoneIndex != -1) {
							SetBrushType(BrushType.Voxel);
							Repaint();
						}
						break;
					case KeyCode.B:
						if (PaintingBoneIndex != -1) {
							SetBrushType(BrushType.Box);
							Repaint();
						}
						break;
					case KeyCode.P:
						if (PaintingBoneIndex == -1 && SelectingBoneIndex != -1) {
							StartPaintBone(SelectingBoneIndex);
							Repaint();
						} else if (PaintingBoneIndex != -1) {
							int oldPaintingBone = PaintingBoneIndex;
							StopPaintBone();
							SelectBone(oldPaintingBone);
							Repaint();
						}
						break;
					case KeyCode.C:
						ResetCamera();
						Repaint();
						break;
					case KeyCode.G:
						SetBoxBackgroundActive(!ShowBackgroundBox);
						Repaint();
						break;
					case KeyCode.T:
						AttachBrush = true;
						Repaint();
						break;
					case KeyCode.R:
						AttachBrush = false;
						Repaint();
						break;
				}
			}
		}




		#region --- Data GUI ---



		private void NodGUI (int id) {
			if (Data) {
				if (Data.Transforms.ContainsKey(id)) {
					TransformNodGUI(id, Data.Transforms[id]);
				} else if (Data.Groups.ContainsKey(id)) {
					GroupNodGUI(id, Data.Groups[id]);
				} else if (Data.Shapes.ContainsKey(id)) {
					ShapeNodGUI(id, Data.Shapes[id]);
				}
			}
		}



		private void TransformNodGUI (int id, VoxelData.TransformData tfData) {

			const int ITEM_HEIGHT = 18;

			LayoutV(() => {

				Space(2);
				LayoutH(() => {

					// Name
					LayoutH(() => {
						GUI.Label(GUIRect(60, ITEM_HEIGHT), "Name");
						EditorGUI.LabelField(GUIRect(42, ITEM_HEIGHT), tfData.Name);
					}, true);
					Space(4);

					// LayerID
					LayoutH(() => {
						GUI.Label(GUIRect(60, ITEM_HEIGHT), "Layer");
						EditorGUI.LabelField(GUIRect(42, ITEM_HEIGHT), tfData.LayerID.ToString());
					}, true);
					Space(4);

					// Reserved
					LayoutH(() => {
						GUI.Label(GUIRect(60, ITEM_HEIGHT), "Reserved");
						EditorGUI.LabelField(GUIRect(42, ITEM_HEIGHT), tfData.Reserved.ToString());
					}, true);
					Space(4);

					// Hidden
					LayoutH(() => {
						GUI.Label(GUIRect(60, ITEM_HEIGHT), "Hidden");
						EditorGUI.LabelField(GUIRect(42, ITEM_HEIGHT), tfData.Hidden.ToString());
					}, true);

				});

				Space(2);

				// Frames
				for (int i = 0; i < tfData.Frames.Length; i++) {

					LayoutH(() => {

						var frame = tfData.Frames[i];

						// Pos
						LayoutH(() => {
							GUI.Label(GUIRect(60, ITEM_HEIGHT), "Position");
							EditorGUI.LabelField(GUIRect(100, ITEM_HEIGHT), frame.Position.ToString());
						}, true);
						Space(2);

						// Rot
						LayoutH(() => {
							GUI.Label(GUIRect(60, ITEM_HEIGHT), "Rotation");
							EditorGUI.LabelField(GUIRect(100, ITEM_HEIGHT), frame.Rotation.ToString());
						}, true);
						Space(2);

						// Scale
						LayoutH(() => {
							GUI.Label(GUIRect(40, ITEM_HEIGHT), "Scale");
							EditorGUI.LabelField(GUIRect(100, ITEM_HEIGHT), frame.Scale.ToString());
						}, true);

					});
				}

				Space(8);

				// Child Node
				NodGUI(tfData.ChildID);

			}, true);

		}



		private void GroupNodGUI (int id, VoxelData.GroupData gpData) {

			bool open = false;
			if (!NodeOpen.ContainsKey(id)) {
				NodeOpen.Add(id, false);
			}
			open = NodeOpen[id];
			AltLayoutF(() => {
				for (int i = 0; i < gpData.ChildNodeId.Length; i++) {
					NodGUI(gpData.ChildNodeId[i]);
					Space(6);
				}
			}, "Child", ref open, false, new GUIStyle() {
				margin = new RectOffset(0, 0, 0, 0),
				padding = new RectOffset(24, 0, 0, 0),
			});

			NodeOpen[id] = open;
		}



		private void ShapeNodGUI (int id, VoxelData.ShapeData spData) {
			LayoutH(() => {
				string labelText = "Model ID [ ";
				for (int i = 0; i < spData.ModelData.Length; i++) {
					labelText += spData.ModelData[i].Key.ToString() + (i < spData.ModelData.Length - 1 ? ", " : "");
				}
				labelText += " ]";
				GUI.Label(GUIRect(0, 18), labelText);
			});
		}



		private void MaterialGUI (VoxelData.MaterialData matData) {

			const int LABEL_WIDTH = 36;
			const int SPACE_WIDTH = 12;

			LayoutH(() => {

				GUI.Label(GUIRect(LABEL_WIDTH, 18), matData.Index.ToString());

				// Type
				EditorGUI.LabelField(GUIRect(62, 18), matData.Type.ToString());
				Space(SPACE_WIDTH);

				// Weight
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Weight.ToString());
				Space(SPACE_WIDTH);

				// Rough
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Rough.ToString());
				Space(SPACE_WIDTH);

				// Spec
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Spec.ToString());
				Space(SPACE_WIDTH);

				// Ior
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Ior.ToString());
				Space(SPACE_WIDTH);

				// Att
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Att.ToString());
				Space(SPACE_WIDTH);

				// Flux
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Flux.ToString());
				Space(SPACE_WIDTH);

				// Glow
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Glow.ToString());
				Space(SPACE_WIDTH);

				// Plastic
				EditorGUI.LabelField(GUIRect(LABEL_WIDTH, 18), matData.Plastic.ToString());
				Space(SPACE_WIDTH);

			});
			Space(2);

		}



		private void RigDataGUI (int id, VoxelData.RigData rigData) {
			LayoutH(() => {
				GUI.Label(GUIRect(36, 18), id.ToString());
				Space();
				GUI.Label(GUIRect(80, 18), "Bones x " + rigData.Bones.Count);
				Space();
				GUI.Label(GUIRect(80, 18), "Weights x " + rigData.Weights.Count);
			});
			Space(2);
		}




		#endregion




		#endregion




		#region --- LGC ---




		private void EditorLoad () {
			ColorfulTitle = EditorPrefs.GetBool("V2U.ColorfulTitle", true);
			ShowBackgroundBox.Load();
			ShowBoneName.Load();
			BrushTypeIndex.Load();
			ShowStateInfo.Load();
		}



		private void EditorSave () {
			ShowBackgroundBox.TrySave();
			ShowBoneName.TrySave();
			BrushTypeIndex.TrySave();
			ShowStateInfo.TrySave();
		}



		// Target
		private void PickTarget (bool pickVox) {
			if (Data) { return; }
			SetTargetAt(EditorUtil.RelativePath(EditorUtility.OpenFilePanel("Pick Voxel Target", "Assets", pickVox ? "vox" : "qb")));
			Repaint();
		}



		private bool SetTargetSafeDialog () {
			return EditorUtil.Dialog("Warning", "Open another voxel model?", "Open", "Cancel");
		}



		private void SetTargetAt (string path) {
			Data = null;
			FileIcon = null;
			TheWeights = null;
			VoxelFilePath = "";
			CurrentModelIndex = 0;
			DataPanelOpen = false;
			NodeOpen.Clear();
			BoneOpen.Clear();
			RemoveRoot();
			if (!string.IsNullOrEmpty(path)) {
				try {
					string ex = Util.GetExtension(path);
					EditorUtil.ProgressBar("", "Importing...", 0f);
					Data = VoxelFile.GetVoxelData(Util.FileToByte(path), ex == ".vox");
					EditorUtil.ProgressBar("", "Importing...", 0.333f);
					VoxelFilePath = path;
					var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
					if (obj) {
						FileIcon = AssetPreview.GetMiniThumbnail(obj);
					}
					// Node Open
					if (Data) {
						foreach (var gp in Data.Groups) {
							if (!NodeOpen.ContainsKey(gp.Key)) {
								NodeOpen.Add(gp.Key, false);
							}
						}
					}
					// Faces
					SwitchModel(CurrentModelIndex);
					DataDirty = false;
					Repaint();
				} catch (System.Exception ex) {
					EditorUtil.ClearProgressBar();
					Debug.LogWarning("[Voxel Editor] Fail to open voxel file.\n" + ex.Message);
					EditorUtil.Dialog("", "Fail to open vox file.", "OK");
				}
			}
		}



		private void CloseTarget (bool forceClose = false) {
			if (forceClose || EditorUtil.Dialog("", "Close the voxel editor?", "Close", "Don't Close")) {
				Data = null;
				VoxelFilePath = "";
				NodeOpen.Clear();
				RemoveRoot();
				DataDirty = false;
				BoneOpen.Clear();
				Repaint();
			}
		}



		private void ResetCamera () {
			CameraRoot.localPosition = Vector3.zero;
			CameraRoot.localRotation = Quaternion.Euler(33.5f, 33.5f, 0f);
			SetCameraSize((CameraSizeMin + CameraSizeMax) * 0.5f);
			RefreshCubeTransform();
		}



		private void SetDataDirty () {
			DataDirty = true;
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}



		private void SaveData () {
			try {
				if (!Data || string.IsNullOrEmpty(VoxelFilePath)) { return; }
				MakeWeightIntoData();
				FreshAllBonesParentIndexByParent();
				var bytes = VoxelFile.GetVoxelByte(Data, Util.GetExtension(VoxelFilePath) == ".vox");
				if (bytes == null) { return; }
				Util.ByteToFile(bytes, VoxelFilePath);
				DataDirty = false;
			} catch (System.Exception ex) {
				Debug.LogError(ex.Message);
			}
		}



		private void Rerender () {
			if (Camera) {
				Camera.Render();
			}
			if (AlphaCamera) {
				AlphaCamera.Render();
			}
			Repaint();
		}



		private void SwitchModel (int index) {

			// Save Weight Data
			MakeWeightIntoData();

			// Switch Model
			index = Mathf.Clamp(index, 0, Data.Voxels.Count - 1);

			RemoveRoot();
			CurrentModelIndex = index;

			bool success = SpawnRoot();

			if (!success) {
				CloseTarget(true);
				return;
			}

			ModelSize = Data.GetModelSize(CurrentModelIndex);


			// Fix Weight Data
			GetWeightsFromData();

			BoneOpen.Clear();
			EditorApplication.delayCall += Repaint;
		}



		// Mode
		private void SwitchEditMode (bool? rigOrSprite) {

			ClearRigCacheIndexs();

			if (rigOrSprite.HasValue) {
				if (rigOrSprite.Value) {
					// Rig
					if (RigAvailable) {
						if (RigEditingRoot) { RigEditingRoot.gameObject.SetActive(true); }
						if (SpriteEditingRoot) { SpriteEditingRoot.gameObject.SetActive(false); }
						if (!Data.Rigs.ContainsKey(CurrentModelIndex)) {
							Data.Rigs.Add(CurrentModelIndex, new VoxelData.RigData() {
								Bones = new List<VoxelData.RigData.Bone>(),
								Weights = new List<VoxelData.RigData.Weight>(),
							});
							SetDataDirty();
						}
						SetWeightPointQuadDirty();
						SpawnBoneTransforms();
						SetAllVoxelCollidersEnable(false);

						StopPaintBone(true);
						DeselectBone(true);
					}
				} else {
					// Sprite
					if (RigEditingRoot) { RigEditingRoot.gameObject.SetActive(false); }
					if (SpriteEditingRoot) { SpriteEditingRoot.gameObject.SetActive(true); }
					SetAllVoxelCollidersEnable(false);


				}
			} else {
				// None
				if (RigEditingRoot) { RigEditingRoot.gameObject.SetActive(false); }
				if (SpriteEditingRoot) { SpriteEditingRoot.gameObject.SetActive(false); }
				SetAllVoxelCollidersEnable(true);
				StopPaintBone();
				DeselectBone();
				SetWeightPointQuadDirty();

			}
		}



		// Misc
		private void Flip (int axis) {

			var voxels = Data.Voxels[CurrentModelIndex];

			int sizeX = (int)(axis == 0 ? ModelSize.x * 0.5f : ModelSize.x);
			int sizeY = (int)(axis == 1 ? ModelSize.y * 0.5f : ModelSize.y);
			int sizeZ = (int)(axis == 2 ? ModelSize.z * 0.5f : ModelSize.z);

			for (int x = 0; x < sizeX; x++) {
				for (int y = 0; y < sizeY; y++) {
					for (int z = 0; z < sizeZ; z++) {
						int i = axis == 0 ? ModelSize.x - x - 1 : x;
						int j = axis == 1 ? ModelSize.y - y - 1 : y;
						int k = axis == 2 ? ModelSize.z - z - 1 : z;
						int temp = voxels[x, y, z];
						voxels[x, y, z] = voxels[i, j, k];
						voxels[i, j, k] = temp;
					}
				}
			}

			if (Data.Rigs.ContainsKey(CurrentModelIndex)) {
				var rig = Data.Rigs[CurrentModelIndex];
				if (rig != null) {
					// Bones
					var bones = TheBones;
					if (bones != null) {
						for (int i = 0; i < bones.Count; i++) {
							var bone = bones[i];
							if (bone) {
								if (bone.Parent) {
									bone.PositionX = axis == 0 ? -bone.PositionX : bone.PositionX;
									bone.PositionY = axis == 1 ? -bone.PositionY : bone.PositionY;
									bone.PositionZ = axis == 2 ? -bone.PositionZ : bone.PositionZ;
								} else {
									bone.PositionX = axis == 0 ? ModelSize.x - bone.PositionX - 1 : bone.PositionX;
									bone.PositionY = axis == 1 ? ModelSize.y - bone.PositionY - 1 : bone.PositionY;
									bone.PositionZ = axis == 2 ? ModelSize.z - bone.PositionZ - 1 : bone.PositionZ;
								}
								bones[i] = bone;
							}
						}
					}
					// Weights
					var weights = rig.Weights;
					if (weights != null) {
						for (int i = 0; i < weights.Count; i++) {
							var weight = weights[i];
							if (weight != null) {
								weight.X = axis == 0 ? ModelSize.x - weight.X - 1 : weight.X;
								weight.Y = axis == 1 ? ModelSize.y - weight.Y - 1 : weight.Y;
								weight.Z = axis == 2 ? ModelSize.z - weight.Z - 1 : weight.Z;
								weights[i] = weight;
							}
						}
					}

				}
			}

			SetDataDirty();
			GetWeightsFromData();
			SwitchModel(CurrentModelIndex);

			Repaint();
		}



		private void Rotate (int axis) {

			var voxels = Data.Voxels[CurrentModelIndex];
			var newVoxels = new int[
				axis == 0 ? ModelSize.x : axis == 1 ? ModelSize.z : ModelSize.y,
				axis == 0 ? ModelSize.z : axis == 1 ? ModelSize.y : ModelSize.x,
				axis == 0 ? ModelSize.y : axis == 1 ? ModelSize.x : ModelSize.z
			];

			for (int x = 0; x < ModelSize.x; x++) {
				for (int y = 0; y < ModelSize.y; y++) {
					for (int z = 0; z < ModelSize.z; z++) {
						int i = x, j = y, k = z;
						if (axis == 0) {
							j = ModelSize.z - z - 1;
							k = y;
						} else if (axis == 1) {
							i = ModelSize.z - z - 1;
							k = x;
						} else {
							i = ModelSize.y - y - 1;
							j = x;
						}
						newVoxels[i, j, k] = voxels[x, y, z];
					}
				}
			}

			Data.Voxels[CurrentModelIndex] = newVoxels;


			if (Data.Rigs.ContainsKey(CurrentModelIndex)) {
				var rig = Data.Rigs[CurrentModelIndex];
				if (rig != null) {
					// Bones
					var bones = TheBones;
					if (bones != null) {
						for (int i = 0; i < bones.Count; i++) {
							var bone = bones[i];
							if (bone) {
								int x = bone.PositionX;
								int y = bone.PositionY;
								int z = bone.PositionZ;
								if (bone.Parent) {
									if (axis == 0) {
										bone.PositionY = -z;
										bone.PositionZ = y;
									} else if (axis == 1) {
										bone.PositionX = -z;
										bone.PositionZ = x;
									} else {
										bone.PositionX = -y;
										bone.PositionY = x;
									}
								} else {
									if (axis == 0) {
										bone.PositionY = ModelSize.z - z - 1;
										bone.PositionZ = y;
									} else if (axis == 1) {
										bone.PositionX = ModelSize.z - z - 1;
										bone.PositionZ = x;
									} else {
										bone.PositionX = ModelSize.y - y - 1;
										bone.PositionY = x;
									}
								}
								bones[i] = bone;
							}
						}
					}
					// Weights
					var weights = rig.Weights;
					if (weights != null) {
						for (int i = 0; i < weights.Count; i++) {
							var weight = weights[i];
							if (weight != null) {
								int x = weight.X;
								int y = weight.Y;
								int z = weight.Z;
								if (axis == 0) {
									weight.Y = ModelSize.z - z - 1;
									weight.Z = y;
								} else if (axis == 1) {
									weight.X = ModelSize.z - z - 1;
									weight.Z = x;
								} else {
									weight.X = ModelSize.y - y - 1;
									weight.Y = x;
								}
								weights[i] = weight;
							}
						}
					}

				}
			}

			ModelSize.x = newVoxels.GetLength(0);
			ModelSize.y = newVoxels.GetLength(1);
			ModelSize.z = newVoxels.GetLength(2);

			SetDataDirty();
			GetWeightsFromData();
			SwitchModel(CurrentModelIndex);
			Repaint();
		}




		#endregion




	}





}