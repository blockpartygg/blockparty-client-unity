namespace VoxeltoUnity {
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;

	
	public struct EditorUtil {



		#region --- Path ---




		public static string RelativePath (string path) {
			path = FixPath(path);
			if (path.StartsWith("Assets")) {
				return path;
			}
			var fixedDataPath = FixPath(Application.dataPath);
			if (path.StartsWith(fixedDataPath)) {
				return "Assets" + path.Substring(fixedDataPath.Length);
			} else {
				return "";
			}
		}





		private static string FixPath (string _path) {
			char dsChar = Path.DirectorySeparatorChar;
			char adsChar = Path.AltDirectorySeparatorChar;
			_path = _path.Replace(adsChar, dsChar);
			_path = _path.Replace(new string(dsChar, 2), dsChar.ToString());
			while (_path.Length > 0 && _path[0] == dsChar) {
				_path = _path.Remove(0, 1);
			}
			return _path;
		}




		#endregion



		#region --- MSG ---


		public static bool Dialog (string title, string msg, string ok, string cancel = "") {
			EditorApplication.Beep();
			PauseWatch();
			if (string.IsNullOrEmpty(cancel)) {
				bool sure = EditorUtility.DisplayDialog(title, msg, ok);
				RestartWatch();
				return sure;
			} else {
				bool sure = EditorUtility.DisplayDialog(title, msg, ok, cancel);
				RestartWatch();
				return sure;
			}
		}


		public static int DialogComplex (string title, string msg, string ok, string cancel, string alt) {
			EditorApplication.Beep();
			PauseWatch();
			int index = EditorUtility.DisplayDialogComplex(title, msg, ok, cancel, alt);
			RestartWatch();
			return index;
		}


		public static void ProgressBar (string title, string msg, float value) {
			value = Mathf.Clamp01(value);
			EditorUtility.DisplayProgressBar(title, msg, value);
		}


		public static void ClearProgressBar () {
			EditorUtility.ClearProgressBar();
		}


		#endregion



		#region --- Watch ---


		private static System.Diagnostics.Stopwatch TheWatch;


		public static void StartWatch () {
			TheWatch = new System.Diagnostics.Stopwatch();
			TheWatch.Start();
		}


		public static void PauseWatch () {
			if (TheWatch != null) {
				TheWatch.Stop();
			}
		}


		public static void RestartWatch () {
			if (TheWatch != null) {
				TheWatch.Start();
			}
		}


		public static double StopWatchAndGetTime () {
			if (TheWatch != null) {
				TheWatch.Stop();
				return TheWatch.Elapsed.TotalSeconds;
			}
			return 0f;
		}


		#endregion

		

		public static bool IsTypingInGUI () {
			return GUIUtility.keyboardControl != 0;
		}

		

		public static Mesh CreateConeMesh (float radius, float height, int subdivisions = 12) {
			Mesh mesh = new Mesh();

			Vector3[] vertices = new Vector3[subdivisions + 2];
			Vector2[] uv = new Vector2[vertices.Length];
			int[] triangles = new int[(subdivisions * 2) * 3];

			vertices[0] = Vector3.zero;
			uv[0] = new Vector2(0.5f, 0f);
			for (int i = 0, n = subdivisions - 1; i < subdivisions; i++) {
				float ratio = (float)i / n;
				float r = ratio * (Mathf.PI * 2f);
				float x = Mathf.Cos(r) * radius;
				float z = Mathf.Sin(r) * radius;
				vertices[i + 1] = new Vector3(x, 0f, z);
				uv[i + 1] = new Vector2(ratio, 0f);
			}
			vertices[subdivisions + 1] = new Vector3(0f, height, 0f);
			uv[subdivisions + 1] = new Vector2(0.5f, 1f);

			// construct bottom

			for (int i = 0, n = subdivisions - 1; i < n; i++) {
				int offset = i * 3;
				triangles[offset] = 0;
				triangles[offset + 1] = i + 1;
				triangles[offset + 2] = i + 2;
			}

			// construct sides

			int bottomOffset = subdivisions * 3;
			for (int i = 0, n = subdivisions - 1; i < n; i++) {
				int offset = i * 3 + bottomOffset;
				triangles[offset] = i + 1;
				triangles[offset + 1] = subdivisions + 1;
				triangles[offset + 2] = i + 2;
			}

			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.RecalculateBounds();
			mesh.RecalculateNormals();

			return mesh;
		}


	}

	
}
