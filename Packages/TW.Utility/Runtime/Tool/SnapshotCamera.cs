using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TW.Utility.Tool
{
    // Object rendering code based on Dave Carlile's "Create a GameObject Image Using Render Textures" post
    // Link: http://crappycoding.com/2014/12/create-gameobject-image-using-render-textures/

    /// <summary>
    /// Takes snapshot images of prefabs and GameObject instances, and provides methods to save them as PNG files.
    /// </summary>
    public class SnapshotCamera : MonoBehaviour
    {
        // This disables the "never assigned" warning.
        // These fields will be assigned by the factory.
        /// <summary>
        /// The Camera used internally by the SnapshotCamera.
        /// </summary>
        private Camera m_Cam;
        /// <summary>
        /// The layer on which the SnapshotCamera takes snapshots.
        /// </summary>
        private int m_Layer;

        /// <summary>
        /// The default position offset applied to objects when none is specified.
        /// </summary>
        public Vector3 m_DefaultPositionOffset = new Vector3(0, 0, 1);
        /// <summary>
        /// The default rotation applied to objects when none is specified.
        /// </summary>
        public Vector3 m_DefaultRotation = new Vector3(345.8529f, 313.8297f, 14.28433f);
        /// <summary>
        /// The default scale applied to objects when none is specified.
        /// </summary>
        public Vector3 m_DefaultScale = new Vector3(1, 1, 1);

        // This private constructor serves to ensure only the factory can produce new instances.
        private SnapshotCamera() { }

        /// <summary>
        /// Factory method which sets up and configures a new SnapshotCamera, then returns it.
        /// </summary>
        /// <param name="layer">The name of the layer on which to take snapshots.</param>
        /// <param name="name">The name that will be given to the new GameObject the SnapshotCamera will be attached to.</param>
        /// <returns>A new SnapshotCamera, ready for use.</returns>
        public static SnapshotCamera MakeSnapshotCamera(string layer, string name = "Snapshot Camera")
        {
            return MakeSnapshotCamera(LayerMask.NameToLayer(layer), name);
        }

        /// <summary>
        /// Factory method which sets up and configures a new SnapshotCamera, then returns it.
        /// </summary>
        /// <param name="layer">The layer number of the layer on which to take snapshots.</param>
        /// <param name="name">The name that will be given to the new GameObject the SnapshotCamera will be attached to.</param>
        /// <returns>A new SnapshotCamera, ready for use.</returns>
        public static SnapshotCamera MakeSnapshotCamera(int layer = 5, string name = "Snapshot Camera")
        {
            if (layer is < 0 or > 31) throw new ArgumentOutOfRangeException(nameof(layer), "layer argument must specify a valid layer between 0 and 31");

            // Create a new GameObject to hold the camera
            GameObject snapshotCameraGo = new GameObject(name);
            // Add a Camera component to the GameObject
            Camera cam = snapshotCameraGo.AddComponent<Camera>();

            // Configure the Camera
            cam.cullingMask = 1 << layer;
            cam.orthographic = true;
            cam.orthographicSize = 1;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.clear;
            cam.nearClipPlane = 0.1f;
            cam.enabled = false;

            // Add a SnapshotCamera component to the GameObject
            SnapshotCamera snapshotCamera = snapshotCameraGo.AddComponent<SnapshotCamera>();

            // Set the SnapshotCamera's cam and layer fields
            snapshotCamera.m_Cam = cam;
            snapshotCamera.m_Layer = layer;

            // Return the SnapshotCamera
            return snapshotCamera;
        }

        #region PNG saving
        /// <summary>
        /// Sanitizes a filename string by replacing illegal characters with underscores.
        /// </summary>
        /// <param name="dirty">The un sanitized filename string.</param>
        /// <returns>A sanitized filename string with illegal characters replaced with underscores.</returns>
        private static string SanitizeFilename(string dirty)
        {
            string invalidFileNameChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidFileNameChars);

            return Regex.Replace(dirty, invalidRegStr, "_");
        }

        /// <summary>
        /// Saves a byte array of PNG data as a PNG file.
        /// </summary>
        /// <param name="bytes">The PNG data to write to a file.</param>
        /// <param name="filename">The name of the file. This will be the current timestamp if not specified.</param>
        /// <param name="directory">The directory in which to save the file. This will be the game/Snapshots directory if not specified.</param>
        /// <returns>A FileInfo pointing to the created PNG file</returns>
        public static FileInfo SavePNG(byte[] bytes, string filename = "", string directory = "")
        {
            directory = directory != "" ? Directory.CreateDirectory(directory).FullName : Directory.CreateDirectory(Path.Combine(Application.dataPath, "../Snapshots")).FullName;
            filename = filename != "" ? SanitizeFilename(filename) + ".png" : System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + ".png";
            string filepath = Path.Combine(directory, filename);

            File.WriteAllBytes(filepath, bytes);

            return new FileInfo(filepath);
        }

        /// <summary>
        /// Saves a Texture2D as a PNG file.
        /// </summary>
        /// <param name="tex">The Texture2D to write to a file.</param>
        /// <param name="filename">The name of the file. This will be the current timestamp if not specified.</param>
        /// <param name="directory">The directory in which to save the file. This will be the game/Snapshots directory if not specified.</param>
        /// <returns>A FileInfo pointing to the created PNG file</returns>
        public static FileInfo SavePNG(Texture2D tex, string filename = "", string directory = "")
        {
            return SavePNG(tex.EncodeToPNG(), filename, directory);
        }
        #endregion

        #region Object preparation
        /// <summary>
        /// This stores the a state (layers, position, rotation, and scale) of a GameObject, and provides a method to restore it.
        /// </summary>
        private readonly struct GameObjectStateSnapshot
        {
            private readonly GameObject m_GameObject;
            private readonly Vector3 m_Position;
            private readonly Quaternion m_Rotation;
            private readonly Vector3 m_Scale;
            private readonly Dictionary<GameObject, int> m_Layers;

            /// <summary>
            /// Store the current state (layers, position, rotation, and scale) of a GameObject
            /// </summary>
            /// <param name="gameObject">The GameObject whose state to store.</param>
            public GameObjectStateSnapshot(GameObject gameObject)
            {
                this.m_GameObject = gameObject;
                this.m_Position = gameObject.transform.position;
                this.m_Rotation = gameObject.transform.rotation;
                this.m_Scale = gameObject.transform.localScale;

                this.m_Layers = new Dictionary<GameObject, int>();
                foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
                {
                    this.m_Layers.Add(t.gameObject, t.gameObject.layer);
                }
            }

            /// <summary>
            /// Restore the gameObject to the state stored in this GameObjectStateSnapshot.
            /// </summary>
            public void Restore()
            {
                this.m_GameObject.transform.position = this.m_Position;
                this.m_GameObject.transform.rotation = this.m_Rotation;
                this.m_GameObject.transform.localScale = this.m_Scale;

                foreach (KeyValuePair<GameObject, int> entry in this.m_Layers)
                {
                    entry.Key.layer = entry.Value;
                }
            }
        }

        /// <summary>
        /// Set the layers of the GameObject and all its children to the SnapshotCamera's snapshot layer so the SnapshotCamera can see it.
        /// </summary>
        /// <param name="go">The GameObject apply the layer modifications to.</param>
        private void SetLayersRecursively(GameObject go)
        {
            foreach (Transform tf in go.GetComponentsInChildren<Transform>(true))
                tf.gameObject.layer = m_Layer;
        }

        /// <summary>
        /// Prepares an instantiated GameObject for taking a snapshot by setting its layers and applying the specified position offset, rotation, and scale to it.
        /// </summary>
        /// <param name="prefab">The instantiated GameObject to prepare.</param>
        /// <param name="positionOffset">The position offset relative to the SnapshotCamera to apply to the gameObject.</param>
        /// <param name="rotation">The rotation to apply to the gameObject.</param>
        /// <param name="scale">The scale to apply to the gameObject.</param>
        /// <returns>A GameObjectStateSnapshot containing the state of the gameObject prior to modifying its layers, position, rotation, and scale.</returns>
        private GameObjectStateSnapshot PrepareObject(GameObject go, Vector3 positionOffset, Quaternion rotation, Vector3 scale)
        {
            GameObjectStateSnapshot gameObjectStateSnapshot = new GameObjectStateSnapshot(go);

            go.transform.position = transform.position + positionOffset;
            go.transform.rotation = rotation;
            go.transform.localScale = scale;
            SetLayersRecursively(go);

            return gameObjectStateSnapshot;
        }

        /// <summary>
        /// Prepares a prefab for taking a snapshot by creating an instance, setting its layers and applying the specified position offset, rotation, and scale to it.
        /// </summary>
        /// <param name="prefab">The prefab to prepare.</param>
        /// <param name="positionOffset">The position offset relative to the SnapshotCamera to apply to the prefab.</param>
        /// <param name="rotation">The rotation to apply to the prefab.</param>
        /// <param name="scale">The scale to apply to the prefab.</param>
        /// <returns>A prefab instance ready for taking a snapshot.</returns>
        private GameObject PreparePrefab(GameObject prefab, Vector3 positionOffset, Quaternion rotation, Vector3 scale)
        {
            GameObject go = GameObject.Instantiate(prefab, transform.position + positionOffset, rotation) as GameObject;
            go.transform.localScale = scale;
            SetLayersRecursively(go);

            return go;
        }
        #endregion

        #region TakeObjectSnapshot
        /// <summary>
        /// Takes a snapshot of an instantiated GameObject and returns it as a Texture2D.
        /// 
        /// Uses a completely transparent background and
        /// applies the default position offset, rotation and scale to the gameObject while taking the snapshot, and restores them afterwards.
        /// </summary>
        /// <param name="go">The instantiated GameObject to snapshot.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakeObjectSnapshot(GameObject go, int width = 128, int height = 128)
        {
            return TakeObjectSnapshot(go, Color.clear, m_DefaultPositionOffset, Quaternion.Euler(m_DefaultRotation), m_DefaultScale, width, height);
        }

        /// <summary>
        /// Takes a snapshot of an instantiated GameObject and returns it as a Texture2D.
        /// 
        /// Applies the default position offset, rotation and scale to the gameObject while taking the snapshot, and restores them afterwards.
        /// </summary>
        /// <param name="go">The instantiated GameObject to snapshot.</param>
        /// <param name="backgroundColor">The background color of the snapshot. Can be transparent.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakeObjectSnapshot(GameObject go, Color backgroundColor, int width = 128, int height = 128)
        {
            return TakeObjectSnapshot(go, backgroundColor, m_DefaultPositionOffset, Quaternion.Euler(m_DefaultRotation), m_DefaultScale, width, height);
        }

        /// <summary>
        /// Takes a snapshot of an instantiated GameObject and returns it as a Texture2D.
        /// 
        /// Uses a completely transparent background.
        /// </summary>
        /// <param name="go">The instantiated GameObject to snapshot.</param>
        /// <param name="positionOffset">The position offset relative to the SnapshotCamera that will be applied to the gameObject while taking the snapshot. Its position will be restored after taking the snapshot.</param>
        /// <param name="rotation">The rotation that will be applied to the gameObject while taking the snapshot. Its rotation will be restored after taking the snapshot.</param>
        /// <param name="scale">The scale that will be applied to the gameObject while taking the snapshot. Its scale will be restored after taking the snapshot.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakeObjectSnapshot(GameObject go, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
        {
            return TakeObjectSnapshot(go, Color.clear, positionOffset, rotation, scale, width, height);
        }

        /// <summary>
        /// Takes a snapshot of an instantiated GameObject and returns it as a Texture2D.
        /// </summary>
        /// <param name="go">The instantiated GameObject to snapshot.</param>
        /// <param name="backgroundColor">The background color of the snapshot. Can be transparent.</param>
        /// <param name="positionOffset">The position offset relative to the SnapshotCamera that will be applied to the gameObject while taking the snapshot. Its position will be restored after taking the snapshot.</param>
        /// <param name="rotation">The rotation that will be applied to the gameObject while taking the snapshot. Its rotation will be restored after taking the snapshot.</param>
        /// <param name="scale">The scale that will be applied to the gameObject while taking the snapshot. Its scale will be restored after taking the snapshot.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakeObjectSnapshot(GameObject go, Color backgroundColor, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
        {
            if (go == null)
                throw new ArgumentNullException(nameof(go));
            else if (go.scene.name == null)
                throw new ArgumentException("gameObject parameter must be an instantiated GameObject! If you want to use a prefab directly, use TakePrefabSnapshot instead.", nameof(go));

            // Prepare the gameObject and save its current state so we can restore it later
            GameObjectStateSnapshot previousState = PrepareObject(go, positionOffset, rotation, scale);

            // Take a snapshot
            Texture2D snapshot = TakeSnapshot(backgroundColor, width, height);

            // Restore the gameObject to its previous state
            previousState.Restore();

            // Return the snapshot
            return snapshot;
        }
        #endregion

        #region TakePrefabSnapshot
        /// <summary>
        /// Takes a snapshot of a prefab and returns it as a Texture2D.
        /// 
        /// Uses a completely transparent background and
        /// applies the default position offset, rotation and scale to the prefab while taking the snapshot.
        /// </summary>
        /// <param name="prefab">The prefab to snapshot.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakePrefabSnapshot(GameObject prefab, int width = 128, int height = 128)
        {
            return TakePrefabSnapshot(prefab, Color.clear, m_DefaultPositionOffset, Quaternion.Euler(m_DefaultRotation), m_DefaultScale, width, height);
        }

        /// <summary>
        /// Takes a snapshot of a prefab and returns it as a Texture2D.
        /// 
        /// Applies the default position offset, rotation and scale to the prefab while taking the snapshot.
        /// </summary>
        /// <param name="prefab">The prefab to snapshot.</param>
        /// <param name="backgroundColor">The background color of the snapshot. Can be transparent.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakePrefabSnapshot(GameObject prefab, Color backgroundColor, int width = 128, int height = 128)
        {
            return TakePrefabSnapshot(prefab, backgroundColor, m_DefaultPositionOffset, Quaternion.Euler(m_DefaultRotation), m_DefaultScale, width, height);
        }

        /// <summary>
        /// Takes a snapshot of a prefab and returns it as a Texture2D.
        /// 
        /// Uses a completely transparent background.
        /// </summary>
        /// <param name="prefab">The prefab to snapshot.</param>
        /// <param name="positionOffset">The position offset relative to the SnapshotCamera that will be applied to the prefab while taking the snapshot.</param>
        /// <param name="rotation">The rotation that will be applied to the prefab while taking the snapshot.</param>
        /// <param name="scale">The scale that will be applied to the prefab while taking the snapshot.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakePrefabSnapshot(GameObject prefab, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
        {
            return TakePrefabSnapshot(prefab, Color.clear, positionOffset, rotation, scale, width, height);
        }

        /// <summary>
        /// Takes a snapshot of a prefab and returns it as a Texture2D.
        /// </summary>
        /// <param name="prefab">The prefab to snapshot.</param>
        /// <param name="backgroundColor">The background color of the snapshot. Can be transparent.</param>
        /// <param name="positionOffset">The position offset relative to the SnapshotCamera that will be applied to the prefab while taking the snapshot.</param>
        /// <param name="rotation">The rotation that will be applied to the prefab while taking the snapshot.</param>
        /// <param name="scale">The scale that will be applied to the prefab while taking the snapshot.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        public Texture2D TakePrefabSnapshot(GameObject prefab, Color backgroundColor, Vector3 positionOffset, Quaternion rotation, Vector3 scale, int width = 128, int height = 128)
        {
            if (prefab == null)
                throw new ArgumentNullException(nameof(prefab));
            else if (prefab.scene.name != null)
                throw new ArgumentException("prefab parameter must be a prefab! If you want to use an instance, use TakeObjectSnapshot instead.", nameof(prefab));

            // Prepare an instance of the prefab
            GameObject instance = PreparePrefab(prefab, positionOffset, rotation, scale);

            // Take a snapshot
            Texture2D snapshot = TakeSnapshot(backgroundColor, width, height);

            // Destroy the instance we created
            DestroyImmediate(instance);

            // Return the snapshot
            return snapshot;
        }
        #endregion

        /// <summary>
        /// Takes a snapshot of whatever is in front of the camera and within the camera's culling mask and returns it as a Texture2D.
        /// </summary>
        /// <param name="backgroundColor">The background color to apply to the camera before taking the snapshot.</param>
        /// <param name="width">The width of the snapshot image.</param>
        /// <param name="height">The height of the snapshot image.</param>
        /// <returns>A Texture2D containing the captured snapshot.</returns>
        private Texture2D TakeSnapshot(Color backgroundColor, int width, int height)
        {
            // Set the background color of the camera
            m_Cam.backgroundColor = backgroundColor;

            // Get a temporary render texture and render the camera
            m_Cam.targetTexture = RenderTexture.GetTemporary(width, height, 24);
            m_Cam.Render();

            // Activate the temporary render texture
            RenderTexture previouslyActiveRenderTexture = RenderTexture.active;
            RenderTexture targetTexture = m_Cam.targetTexture;
            RenderTexture.active = targetTexture;

            // Extract the image into a new texture without mipmaps
            Texture2D texture = new Texture2D(targetTexture.width, targetTexture.height, TextureFormat.ARGB32, false);
            texture.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            texture.Apply(false);

            // Reactivate the previously active render texture
            RenderTexture.active = previouslyActiveRenderTexture;

            // Clean up after ourselves
            m_Cam.targetTexture = null;
            RenderTexture.ReleaseTemporary(m_Cam.targetTexture);

            // Return the texture
            return texture;
        }
    }

}