﻿using System;
using System.Collections.Generic;
using System.Linq;
using MemoryPack;
using ObservableCollections;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using R3;

namespace TW.Reactive.CustomComponent
{
    [MemoryPackable]
    [Serializable]
    public partial class ReactiveList<T>
    {
#if UNITY_EDITOR
        public class ReactiveStructValueDrawer : OdinValueDrawer<ReactiveList<T>> 
        {
            protected override void DrawPropertyLayout(GUIContent label)
            {
                ValueEntry.Property.Children.First().Draw(label);
            }
        }
#endif
        [field: ListDrawerSettings(CustomAddFunction = nameof(CustomAddFunction), 
            CustomRemoveElementFunction = nameof(CustomRemoveElementFunction),
            CustomRemoveIndexFunction = nameof(CustomRemoveIndexFunction))] 
        [field: OnValueChanged(nameof(OnValueChange))] 
        [field: SerializeField] 
        [MemoryPackInclude]
        private List<T> Value { get; set; }
        [MemoryPackIgnore]
        public ObservableList<T> ObservableList {get; private set;}


        public ReactiveList()
        {
            Value = new List<T>();
            ObservableList = new ObservableList<T>(Value);
            
            ObservableList.ObserveAdd().Subscribe(OnObservableListAdd);
            ObservableList.ObserveRemove().Subscribe(OnObservableListRemove);
            ObservableList.ObserveMove().Subscribe(OnObservableListMove);
            ObservableList.ObserveReplace().Subscribe(OnObservableListReplace);
            ObservableList.ObserveReset().Subscribe(OnObservableListReset);
            ObservableList.ObserveCountChanged().Subscribe(OnObservableListCountChanged);
        }
        [MemoryPackConstructor]
        public ReactiveList(List<T> value)
        {
            Value = value;
            ObservableList = new ObservableList<T>(value);
            
            ObservableList.ObserveAdd().Subscribe(OnObservableListAdd);
            ObservableList.ObserveRemove().Subscribe(OnObservableListRemove);
            ObservableList.ObserveMove().Subscribe(OnObservableListMove);
            ObservableList.ObserveReplace().Subscribe(OnObservableListReplace);
            ObservableList.ObserveReset().Subscribe(OnObservableListReset);
            ObservableList.ObserveCountChanged().Subscribe(OnObservableListCountChanged);
        }

        private void OnObservableListAdd(CollectionAddEvent<T> collectionAddEvent)
        {
            Value.Insert(collectionAddEvent.Index, collectionAddEvent.Value);
        }
        private void OnObservableListRemove(CollectionRemoveEvent<T> collectionRemoveEvent)
        {
            Value.RemoveAt(collectionRemoveEvent.Index);
        }
        private void OnObservableListMove(CollectionMoveEvent<T> collectionMoveEvent)
        {
            Value.Insert(collectionMoveEvent.NewIndex, collectionMoveEvent.Value);
        }
        private void OnObservableListReplace(CollectionReplaceEvent<T> collectionReplaceEvent)
        {
            Value[collectionReplaceEvent.Index] = collectionReplaceEvent.NewValue;
        }
        private void OnObservableListReset(Unit unit)
        {
            Value.Clear();
        }
        private void OnObservableListCountChanged(int count)
        {
            Value.Capacity = count;
        }


        public static implicit operator List<T>(ReactiveList<T> reactiveValue)
        {
            return reactiveValue.ObservableList.ToList();
        }
    }
    public partial class ReactiveList<T>
    {
        private void OnValueChange()
        {
#if UNITY_EDITOR
            ObservableList.Clear();
            ObservableList.AddRange(Value);
#endif
        }
        private void CustomAddFunction()
        {
#if UNITY_EDITOR
            ObservableList.Add(default(T));
#endif
        }

        private void CustomRemoveElementFunction(T element)
        {
#if UNITY_EDITOR
            ObservableList.Remove(element);
#endif
        }
        private void CustomRemoveIndexFunction(int index)
        {
#if UNITY_EDITOR
            ObservableList.RemoveAt(index);
#endif
        }
    }
}
