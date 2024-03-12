using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace TW.UGUI.AssetLoaders
{
    public readonly struct AssetLoadHandleId : IEquatable<AssetLoadHandleId>
    {
        private uint Value { get; }

        public AssetLoadHandleId(uint value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(AssetLoadHandleId other)
        {
            return Value == other.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            if (obj is AssetLoadHandleId other)
            {
                return Value == other.Value;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return Value.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator AssetLoadHandleId(uint value)
        {
            return new AssetLoadHandleId(value);
        }
    }

    public class AssetLoadHandle
    {
        private Func<float> PercentCompleteFunc { get; set; }
        public Object TypelessResult { get; private set; }
        public UniTask TypelessTask { get; private set; }

        public AssetLoadHandle(AssetLoadHandleId id)
        {
            Id = id;
        }

        public AssetLoadHandleId Id { get; }

        public bool IsDone => Status != AssetLoadStatus.None;

        public AssetLoadStatus Status { get; private set; }

        public float PercentComplete => PercentCompleteFunc.Invoke();

        public Exception OperationException { get; private set; }

        public void SetStatus(AssetLoadStatus status)
        {
            Status = status;
        }

        public void SetPercentCompleteFunc(Func<float> percentComplete)
        {
            PercentCompleteFunc = percentComplete;
        }

        public void SetOperationException(Exception ex)
        {
            OperationException = ex;
        }

        public void SetTypelessResult(Object result)
        {
            TypelessResult = result;
        }

        public void SetTypelessTask(UniTask task)
        {
            TypelessTask = task;
        }
    }

    public class AssetLoadHandle<T> : AssetLoadHandle
        where T : Object
    {
        public AssetLoadHandle(AssetLoadHandleId id) : base(id)
        {
        }

        public T Result { get; private set; }

        public UniTask<T> Task { get; private set; }

        public void SetResult(T result)
        {
            Result = result;
            SetTypelessResult(result);
        }

        public void SetTask(UniTask<T> task)
        {
            Task = task;
            SetTypelessTask(task.AsUniTask());
        }
    }
}