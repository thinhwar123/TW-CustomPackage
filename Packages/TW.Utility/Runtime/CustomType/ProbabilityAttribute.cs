using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.Utilities;


#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Sirenix.OdinInspector.Editor.ValueResolvers;
#endif

namespace TW.Utility.CustomType
{

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class ProbabilityAttribute : Attribute
    {
        public string ColorName;
        public string DataMember;
        
        public ProbabilityAttribute(string dataMember, string colors = "Fall")
        {
            this.DataMember = dataMember;
            this.ColorName = colors;
        }
    }
#if UNITY_EDITOR


    public sealed class ProbabilityAttributeDrawer : OdinAttributeDrawer<ProbabilityAttribute, float[]>
    {
        private string m_ErrorMessage;
        ValueResolver<object[]> m_BaseItemDataGetter;
        private State m_State;
        private readonly Vector2 m_SelectSize = new Vector2(30, 30);
        private List<float> m_SelectValues = new List<float>();
        private readonly List<Rect> m_Ranges = new List<Rect>();
        private int m_SelectId = -1;
        private Color[] m_Colors;

        private float[] SetData<T>(IReadOnlyCollection<T> dataCollection, ref List<float> select, IReadOnlyList<float> value)
        {

            if (dataCollection == null || dataCollection.Count == 0)
            {
                select.Clear();
                m_Ranges.Clear();
                return Array.Empty<float>();

            }

            if (dataCollection.Count == 1)
            {
                select.Clear();
                m_Ranges.Clear();
                for (int i = 0; i < dataCollection.Count; i++)
                {
                    m_Ranges.Add(new Rect());
                }
                return Array.Empty<float>();

            }

            if (dataCollection.Count - 1 != value.Count)
            {
                select.Clear();
                for (int i = 0; i < dataCollection.Count; i++)
                {
                    if (i != dataCollection.Count - 1)
                    {
                        select.Add(1 / (float)dataCollection.Count * (i + 1));
                    }
                }
                m_Ranges.Clear();
                for (int i = 0; i < dataCollection.Count; i++)
                {
                    m_Ranges.Add(new Rect());
                }


            }
            else if (dataCollection.Count - 1 != select.Count)
            {
                select.Clear();
                for (int i = 0; i < dataCollection.Count; i++)
                {
                    if (i != dataCollection.Count - 1)
                    {
                        select.Add(value[i]);
                    }
                }
                m_Ranges.Clear();
                for (int i = 0; i < dataCollection.Count; i++)
                {
                    m_Ranges.Add(new Rect());
                }

            }
            return select.ToArray();
        }
        protected override void Initialize()
        {
            if (this.Attribute.DataMember != null)
            {
                this.m_BaseItemDataGetter = ValueResolver.Get<object[]>(this.Property, this.Attribute.DataMember);
                if (this.m_ErrorMessage != null)
                {
                    this.m_ErrorMessage = this.m_BaseItemDataGetter.ErrorMessage;
                }
            }

            if (m_BaseItemDataGetter.GetValue() != null && m_BaseItemDataGetter.GetValue().Length != 0)
            {
                this.ValueEntry.SmartValue = SetData(m_BaseItemDataGetter.GetValue(), ref m_SelectValues, ValueEntry.SmartValue);
            }


            for (int i = 0; i < ColorPaletteManager.Instance.ColorPalettes.Count; i++)
            {
                if (ColorPaletteManager.Instance.ColorPalettes[i].Name != Attribute.ColorName) continue;
                m_Colors = ColorPaletteManager.Instance.ColorPalettes[i].Colors.ToArray();
                break;
            }
        }
        protected override void DrawPropertyLayout(GUIContent label)
        {
            DrawAll(m_BaseItemDataGetter);
        }

        private void DrawAll<T>(ValueResolver<T[]> inspectorPropertyValueGetter)
        {
            if (inspectorPropertyValueGetter.GetValue() != null && inspectorPropertyValueGetter.GetValue().Length != 0)
            {
                Rect rect = EditorGUILayout.GetControlRect(false, 35);
                DrawRange(rect, m_BaseItemDataGetter);
                DrawSelect(rect, m_BaseItemDataGetter);
            }
        }

        private void DrawRange<T>(Rect rect, ValueResolver<T[]> inspectorPropertyValueGetter)
        {
            if (inspectorPropertyValueGetter.GetValue() == null || inspectorPropertyValueGetter.GetValue().Length == 0) return;
            SirenixEditorGUI.DrawSolidRect(rect, new Color(0.7f, 0.7f, 0.7f, 1f));
            GUIStyle style = new GUIStyle
            {
                alignment = TextAnchor.UpperCenter
            };
            GUIStyle percentageStyle = new GUIStyle
            {
                alignment = TextAnchor.LowerCenter
            };

            this.ValueEntry.SmartValue = SetData(inspectorPropertyValueGetter.GetValue(), ref m_SelectValues, ValueEntry.SmartValue);

            T[] value = inspectorPropertyValueGetter.GetValue();
            GameObject[] go = value as GameObject[];


            for (int i = 0; i < m_Ranges.Count; i++)
            {
                if (m_Ranges.Count == 1)
                {
                    m_Ranges[i] = rect.SetXMin(rect.xMin).SetXMax(rect.xMax);
                    SirenixEditorGUI.DrawSolidRect(m_Ranges[i], m_Colors[i]);
                }
                else
                {
                    if (i == 0)
                    {
                        m_Ranges[i] = rect.SetXMin(rect.xMin).SetXMax(rect.width * m_SelectValues[i] + (m_SelectSize.x / 2));
                        SirenixEditorGUI.DrawSolidRect(m_Ranges[i], m_Colors[i]);
                    }
                    else if (i == m_Ranges.Count - 1)
                    {
                        m_Ranges[i] = rect.SetXMin(rect.width * m_SelectValues[i - 1] + (m_SelectSize.x / 2));
                        SirenixEditorGUI.DrawSolidRect(m_Ranges[i], m_Colors[i]);
                    }
                    else
                    {
                        m_Ranges[i] = rect.SetXMin(rect.width * m_SelectValues[i - 1] + (m_SelectSize.x / 2)).SetXMax(rect.width * m_SelectValues[i] + (m_SelectSize.x / 2));
                        SirenixEditorGUI.DrawSolidRect(m_Ranges[i], m_Colors[i]);
                    }
                    SirenixEditorGUI.DrawSolidRect(m_Ranges[i], m_Colors[i]);
                }

                GUIHelper.PushColor(Color.black);
                if (inspectorPropertyValueGetter.GetValue()[i] != null)
                {
                    if (go != null)
                    {
                        if (go[i] != null)
                        {
                            GUI.Label(m_Ranges[i].AlignCenterX(m_Ranges[i].width / 2), go[i].name, style);
                        }
                    }
                    else
                    {
                        GUI.Label(m_Ranges[i].AlignCenterX(m_Ranges[i].width / 2), inspectorPropertyValueGetter.GetValue()[i].ToString(), style);
                    }
                }
                float percentage = (m_Ranges[i].width / rect.width) * 100;

                GUI.Label(m_Ranges[i].AlignCenterX(m_Ranges[i].width / 2), Mathf.Round(percentage) + "%", percentageStyle);
                GUIHelper.PopColor();
            }
        }

        private void DrawSelect<T>(Rect rect, ValueResolver<T[]> inspectorPropertyValueGetter)
        {
            if (inspectorPropertyValueGetter.GetValue() == null || inspectorPropertyValueGetter.GetValue().Length == 0) return;
            Event currentEvent = Event.current;
            Rect[] selectPoint = new Rect[m_SelectValues.Count];
            for (int i = 0; i < selectPoint.Length; i++)
            {
                selectPoint[i] = new Rect(rect.width * m_SelectValues[i], rect.y, m_SelectSize.x, m_SelectSize.y);
                GUI.Label(selectPoint[i], EditorIcons.Eject.Raw);
            }

            for (int i = 0; i < selectPoint.Length; i++)
            {
                if (!selectPoint[i].Contains(currentEvent.mousePosition)) continue;
                if (currentEvent.type != EventType.MouseDown || currentEvent.button != 0) continue;
                ChangeState(State.Down);
                m_SelectId = i;

            }
            if (currentEvent.type == EventType.MouseUp && currentEvent.button == 0)
            {
                ChangeState(State.Up);
                m_SelectId = -1;
            }
            ///////////////////Mouse Event/////////////////////
            switch (m_State)
            {
                case State.Down:
                    if (currentEvent.type == EventType.MouseDrag && currentEvent.button == 0)
                    {
                        ChangeState(State.Drag);
                    }
                    break;
                case State.Up:
                    break;
                case State.Drag:

                    if (Event.current.type != EventType.Repaint) return;
                    float value = currentEvent.mousePosition.x;
                    value = Mathf.Clamp(value, rect.xMin, rect.xMax);
                    value = (value - rect.xMin) / (rect.xMax - rect.xMin);

                    if (m_SelectValues.Count == 1)
                    {
                        m_SelectValues[m_SelectId] = value;
                    }
                    else
                    {
                        if (m_SelectId == 0)
                        {
                            value = Mathf.Clamp(value, 0, m_SelectValues[m_SelectId + 1]);
                            m_SelectValues[m_SelectId] = value;
                        }
                        else if (m_SelectId == selectPoint.Length - 1)
                        {
                            value = Mathf.Clamp(value, m_SelectValues[m_SelectId - 1], 1);
                            m_SelectValues[m_SelectId] = value;
                        }
                        else
                        {
                            value = Mathf.Clamp(value, m_SelectValues[m_SelectId - 1], m_SelectValues[m_SelectId + 1]);
                            m_SelectValues[m_SelectId] = value;
                        }
                    }
                    break;
                case State.Out:
                    break;
                default:
                    break;
            }
        }
        public enum State { None, Down, Up, Drag, Out }

        private void ChangeState(State state)
        {
            this.m_State = state;
        }
    }

#endif

}