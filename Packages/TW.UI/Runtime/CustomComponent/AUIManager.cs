using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TW.Utility.DesignPattern;
using UnityEngine;

namespace TW.UI.CustomComponent
{
    public class AUIManager : Singleton<AUIManager>
    {
        [field: SerializeField] public Transform AUIPanelsParentTf { get; private set; }
        private Dictionary<Type, AUIPanel> AUIPanels { get; set; } = new Dictionary<Type, AUIPanel>();

        private Dictionary<Type, AUIPanel> AUIPanelsPrefab { get; set; } =
            new Dictionary<Type, AUIPanel>();

        private AUIPanel[] AUIPanelsPrefabArray { get; set; }

        private T GetPrefab<T>() where T : AUIPanel
        {
            if (AUIPanelsPrefab.ContainsKey(typeof(T))) return AUIPanelsPrefab[typeof(T)] as T;
            AUIPanelsPrefabArray ??= Resources.LoadAll<AUIPanel>("UI/");
            AUIPanelsPrefab.Add(typeof(T), AUIPanelsPrefabArray.First(ui => ui is T));
            return AUIPanelsPrefab[typeof(T)] as T;
        }

        public bool IsOpenedUI<T>() where T : AUIPanel
        {
            return AUIPanels.ContainsKey(typeof(T)) && AUIPanels[typeof(T)] != null &&
                   AUIPanels[typeof(T)].gameObject.activeInHierarchy;
        }

        public T GetUI<T>() where T : AUIPanel
        {
            if (AUIPanels.ContainsKey(typeof(T)) && AUIPanels[typeof(T)] != null) return AUIPanels[typeof(T)] as T;
            AUIPanel ui = Instantiate(GetPrefab<T>(), AUIPanelsParentTf);
            AUIPanels[typeof(T)] = ui;

            return (T)AUIPanels[typeof(T)];
        }
        
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

        public T OpenUI<T>() where T : AUIPanel
        {
            AUIPanel ui = GetUI<T>();

            ui.OpenPanel<T>();

            return (T)ui;
        }

        public void CloseUI<T>() where T : AUIPanel
        {
            if (IsOpenedUI<T>())
            {
                GetUI<T>().ClosePanel<T>();
            }
        }
    }

}