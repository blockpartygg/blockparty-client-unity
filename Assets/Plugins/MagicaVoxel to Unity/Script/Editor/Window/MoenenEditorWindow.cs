namespace Moenen {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using Moenen.Saving;

	public class MoenenEditorWindow : EditorWindow {



		protected static Rect GUIRect (float width, float height) {
			return GUILayoutUtility.GetRect(
				width, height,
				GUILayout.ExpandWidth(width == 0), GUILayout.ExpandHeight(height == 0)
			);
		}



		protected static void Link (int width, int height, string label, string link) {
			var buttonRect = GUIRect(width, height);
			if (GUI.Button(buttonRect, label, new GUIStyle(GUI.skin.label) {
				wordWrap = true,
				normal = new GUIStyleState() {
					textColor = new Color(86f / 256f, 156f / 256f, 214f / 256f),
					background = null,
					scaledBackgrounds = null,
				}
			})) {
				Application.OpenURL(link);
			}
			EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);
		}



		protected static void LayoutV (System.Action action, bool box = false, GUIStyle style = null) {
			if (box) {
				style = new GUIStyle(GUI.skin.box) {
					padding = new RectOffset(6, 6, 2, 2)
				};
			}
			if (style != null) {
				GUILayout.BeginVertical(style);
			} else {
				GUILayout.BeginVertical();
			}
			action();
			GUILayout.EndVertical();
		}



		protected static void LayoutH (System.Action action, bool box = false, GUIStyle style = null) {
			if (box) {
				style = new GUIStyle(GUI.skin.box) {
					padding = new RectOffset(6, 6, 2, 2)
				};
			}
			if (style != null) {
				GUILayout.BeginHorizontal(style);
			} else {
				GUILayout.BeginHorizontal();
			}
			action();
			GUILayout.EndHorizontal();
		}



		protected static void LayoutF (System.Action action, string label, EditorSavingBool open, bool box = false, GUIStyle style = null) {
			LayoutV(() => {
				open.Value = GUILayout.Toggle(
					open,
					label,
					GUI.skin.GetStyle("foldout"),
					GUILayout.ExpandWidth(true),
					GUILayout.Height(18)
				);
				if (open) {
					action();
				}
			}, box, style);
			Space(4);
		}



		protected static void AltLayoutF (System.Action action, string label, EditorSavingBool open, bool box = false, GUIStyle style = null) {
			LayoutV(() => {
				if (box) {
					style = GUI.skin.box;
				}
				Rect rect = GUIRect(0, 18);
				rect.x -= style == null ? 18 : style.padding.left;
				open.Value = GUI.Toggle(
					rect,
					open,
					label,
					GUI.skin.GetStyle("foldout")
				);
				if (open) {
					action();
				}
			}, box, style);
			Space(4);
		}



		protected static void LayoutF (System.Action action, string label, ref bool open, bool box = false, GUIStyle style = null) {
			bool _open = open;
			LayoutV(() => {
				_open = GUILayout.Toggle(
					_open,
					label,
					GUI.skin.GetStyle("foldout"),
					GUILayout.ExpandWidth(true),
					GUILayout.Height(18)
				);
				if (_open) {
					action();
				}
			}, box, style);
			Space(4);
			open = _open;
		}



		protected static void AltLayoutF (System.Action action, string label, ref bool open, bool box = false, GUIStyle style = null) {
			bool _open = open;
			LayoutV(() => {
				if (box) {
					style = GUI.skin.box;
				}
				Rect rect = GUIRect(0, 18);
				rect.x -= style == null ? 18 : style.padding.left;
				_open = GUI.Toggle(
					rect,
					_open,
					label,
					GUI.skin.GetStyle("foldout")
				);
				if (_open) {
					action();
				}
			}, box, style);
			Space(4);
			open = _open;
		}



		protected static void Space (float space = 4f) {
			GUILayout.Space(space);
		}



		protected static string GetDisplayString (string str, int maxLength) {
			return str.Length > maxLength ? str.Substring(0, maxLength - 3) + "..." : str;
		}



		protected static bool ColorfulButton (Rect rect, string label, Color color, GUIStyle style = null) {
			Color oldColor = GUI.color;
			GUI.color = color;
			bool pressed = style == null ? GUI.Button(rect, label) : GUI.Button(rect, label, style);
			GUI.color = oldColor;
			return pressed;
		}



		protected static void ColorBlock (Rect rect) {
			ColorBlock(rect, new Color(1, 1, 1, 0.1f));
		}



		protected static void ColorBlock (Rect rect, Color color) {
			var oldC = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(rect, Texture2D.whiteTexture);
			GUI.color = oldC;
		}


	}
}
