using System;
using UnityEngine;

namespace TW.UI.CustomStyleSheet
{
    public static class CColor
    {
        public static Color blue = new Color(0.42f, 0.58f, 0.91f);
        public static Color lightYellow = new Color(0.72f, 0.60f, 0.32f);
        public static Color lightGreen = new Color(0.25f, 0.78f, 0.55f);
        public static Color darkGreen = new Color(0.21f, 0.80f, 0.47f);
        public static Color lightPurple = new Color(0.74f, 0.57f, 0.95f);
        public static Color lightPink = new Color(0.91f, 0.58f, 0.77f);
    }

    public class GUIColorScope : IDisposable
    {
        private readonly Color m_OldColor;
        public GUIColorScope(Color color)
        {
            m_OldColor = GUI.color;
            GUI.color = color;
        }
        public void Dispose()
        {
            GUI.color = m_OldColor;
        }
    }
}