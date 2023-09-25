using System;
using System.Collections.Generic;
using System.Linq;
using TW.Utility.DesignPattern;
using UnityEngine;

namespace TW.UI.CustomComponent
{
    /// <summary>
    /// A manager class for managing UI panels in the game.
    /// </summary>
    public class AUIManager : Singleton<AUIManager>
    {
        /// <summary>
        /// The parent transform under which UI panels will be instantiated.
        /// </summary>
        [field: SerializeField] public Transform AUIPanelsParentTf { get; private set; }

        private Dictionary<Type, AUIPanel> AUIPanels { get; set; } = new Dictionary<Type, AUIPanel>();

        private Dictionary<Type, AUIPanel> AUIPanelsPrefab { get; set; } =
            new Dictionary<Type, AUIPanel>();

        private AUIPanel[] AUIPanelsPrefabArray { get; set; }

        /// <summary>
        /// Get the prefab for a specified UI panel type.
        /// </summary>
        /// <typeparam name="T">The type of the UI panel.</typeparam>
        /// <returns>The prefab of the specified UI panel.</returns>
        private T GetPrefab<T>() where T : AUIPanel
        {
            if (AUIPanelsPrefab.ContainsKey(typeof(T))) return AUIPanelsPrefab[typeof(T)] as T;
            AUIPanelsPrefabArray ??= Resources.LoadAll<AUIPanel>("UI/");
            AUIPanelsPrefab.Add(typeof(T), AUIPanelsPrefabArray.First(ui => ui is T));
            return AUIPanelsPrefab[typeof(T)] as T;
        }

        /// <summary>
        /// Check if a UI panel of the specified type is currently open.
        /// </summary>
        /// <typeparam name="T">The type of the UI panel to check.</typeparam>
        /// <returns>True if the UI panel is open; otherwise, false.</returns>
        public bool IsOpenedUI<T>() where T : AUIPanel
        {
            return AUIPanels.ContainsKey(typeof(T)) && AUIPanels[typeof(T)] != null &&
                   AUIPanels[typeof(T)].gameObject.activeInHierarchy;
        }

        /// <summary>
        /// Get an instance of a specified UI panel.
        /// </summary>
        /// <typeparam name="T">The type of the UI panel to get.</typeparam>
        /// <returns>An instance of the specified UI panel.</returns>
        public T GetUI<T>() where T : AUIPanel
        {
            if (AUIPanels.ContainsKey(typeof(T)) && AUIPanels[typeof(T)] != null) return AUIPanels[typeof(T)] as T;
            AUIPanel ui = Instantiate(GetPrefab<T>(), AUIPanelsParentTf);
            AUIPanels[typeof(T)] = ui;

            return (T)AUIPanels[typeof(T)];
        }

        /// <summary>
        /// Try to get an instance of a specified UI panel.
        /// </summary>
        /// <param name="ui">An output parameter that receives the UI panel instance if found.</param>
        /// <typeparam name="T">The type of the UI panel to get.</typeparam>
        /// <returns>True if the UI panel was found; otherwise, false.</returns>
        public bool TryGetUI<T>(out T ui) where T : AUIPanel
        {
            if (AUIPanels.ContainsKey(typeof(T)) && AUIPanels[typeof(T)] != null)
            {
                ui = AUIPanels[typeof(T)] as T;
                return true;
            }

            ui = null;
            return false;
        }

        /// <summary>
        /// Open a UI panel of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the UI panel to open.</typeparam>
        /// <returns>An instance of the opened UI panel.</returns>
        public T OpenUI<T>() where T : AUIPanel
        {
            AUIPanel ui = GetUI<T>();

            ui.OpenPanel<T>();

            return (T)ui;
        }

        /// <summary>
        /// Close a UI panel of the specified type if it is currently open.
        /// </summary>
        /// <typeparam name="T">The type of the UI panel to close.</typeparam>
        public void CloseUI<T>() where T : AUIPanel
        {
            if (IsOpenedUI<T>())
            {
                GetUI<T>().ClosePanel<T>();
            }
        }
    }
}
