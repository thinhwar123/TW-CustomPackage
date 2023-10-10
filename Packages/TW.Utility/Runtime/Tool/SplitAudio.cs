using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using System;
using System.Reflection;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif


namespace TW.Utility.Tool
{
#if UNITY_EDITOR
    public class SplitAudio : Editor
    {
        private static AudioClip AudioClip { get; set; }
        
        [MenuItem("Assets/Split Audio", true)]
        private static bool CanSplitAudio()
        {
            return Selection.activeObject is AudioClip;
        }

        [MenuItem("Assets/Split Audio")]
        private static void DoSplitAudio()
        {
            AudioClip = Selection.activeObject as AudioClip;
            SplitAudioEditorWindow.OpenEditorWindow().InitAudioClip(AudioClip);
        }
    }

    public class SplitAudioEditorWindow : EditorWindow
    {
        private AudioClip AudioClip { get; set; }
        private AudioClip ResultAudioClip { get; set; }
        private int AudioClipSamples { get; set; }
        
        private int startSplit;
        private int endSplit;
        
        [MenuItem("Tools/Editor Windows/SplitAudio")]
        public static SplitAudioEditorWindow OpenEditorWindow()
        {
            SplitAudioEditorWindow win = ScriptableObject.CreateInstance<SplitAudioEditorWindow>();
            win.position = new Rect(10, 10, 400, 400);
            win.Show(true);

            return win;
        }
        
        public void InitAudioClip(AudioClip audioClip)
        {
            AudioClip = audioClip;
            AudioClipSamples = AudioClip.samples;
        }
        public void OnGUI()
        {
            DrawSelectAudioClip();
            DrawSplitRangeAudio();
            DrawButtonPlayDemo();
            DrawButtonSplitAudio();
        }

        private void DrawButtonSplitAudio()
        {
            
        }

        private void DrawButtonPlayDemo()
        {   
            if (GUILayout.Button("Play Demo"))
            {
                if (AudioClip == null) return;
                ResultAudioClip = CreateAudioClip(AudioClip, startSplit, endSplit);
                SaveAudioClip(ResultAudioClip);
                StopAllClips();
                PlayClip(ResultAudioClip);
            }
        }

        private void DrawSplitRangeAudio()
        {
            
        } 

        private void DrawSelectAudioClip()
        {
            AudioClip = EditorGUILayout.ObjectField("Current Audio Select", AudioClip, typeof(AudioClip), false) as AudioClip;
            ResultAudioClip = EditorGUILayout.ObjectField("Result Audio Select", ResultAudioClip, typeof(AudioClip), false) as AudioClip;
            EditorGUILayout.LabelField("Audio Clip Sample", AudioClipSamples.ToString(CultureInfo.InvariantCulture));
            EditorGUILayout.LabelField("Audio Clip Start", startSplit.ToString(CultureInfo.InvariantCulture));
            EditorGUILayout.LabelField("Audio Clip End", endSplit.ToString(CultureInfo.InvariantCulture));
            float startSplitRef = startSplit;
            float endSplitRef = endSplit;
            EditorGUILayout.MinMaxSlider("Split Range",ref startSplitRef, ref endSplitRef, 0, AudioClipSamples);
            startSplit = (int)startSplitRef;
            endSplit = (int)endSplitRef;
        }
        private AudioClip CreateAudioClip(AudioClip source, float start, float end)
        {
            AudioClip clip = AudioClip.Create(source.name, (int)(end - start), source.channels, source.frequency, false);
            float[] data = new float[(int)(end - start) * source.channels];
            source.GetData(data, (int)(start * source.channels));
            clip.SetData(data, 0);
            return clip;
        }
        private void SaveAudioClip(AudioClip source)
        {
            string defaultFileName = source.name + "_split_.mp3";
            string path = AssetDatabase.GetAssetPath(AudioClip);

            AssetDatabase.CreateAsset(source, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
     
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayPreviewClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
 
            Debug.Log(method);
            method.Invoke(
                null,
                new object[] { clip, startSample, loop }
            );
        }
 
        public static void StopAllClips()
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
 
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "StopAllPreviewClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { },
                null
            );
 
            Debug.Log(method);
            method.Invoke(
                null,
                new object[] { }
            );
        }
    }
#endif
}