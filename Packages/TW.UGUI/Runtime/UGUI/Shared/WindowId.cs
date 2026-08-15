using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TW.UGUI.Shared
{
    /// <summary>
    /// Cross Unity-version identifier used to uniquely track a <see cref="UnityEngine.Object"/> instance
    /// (e.g. a <see cref="RectTransform"/>, <see cref="Object"/> based Sheet/Control) across a container's lifetime.
    /// </summary>
    /// <remarks>
    /// Unity 6.5 removes the 32-bit <c>InstanceID</c> APIs (<see cref="Object.GetInstanceID"/>) in favor of the
    /// 64-bit <see cref="EntityId"/> struct. This type wraps whichever one is available so the rest of the
    /// codebase doesn't need to scatter <c>#if UNITY_6000_5_OR_NEWER</c> checks.
    /// </remarks>
    [Serializable]
    public readonly struct WindowId : IEquatable<WindowId>
    {
#if UNITY_6000_5_OR_NEWER
        private readonly EntityId _value;

        private WindowId(EntityId value)
        {
            _value = value;
        }

        public static WindowId Of(Object obj)
            => new(obj.GetEntityId());
#else
        private readonly int _value;

        private WindowId(int value)
        {
            _value = value;
        }

        public static WindowId Of(Object obj)
            => new(obj.GetInstanceID());
#endif

        public bool Equals(WindowId other)
            => _value.Equals(other._value);

        public override bool Equals(object obj)
            => obj is WindowId other && Equals(other);

        public override int GetHashCode()
            => _value.GetHashCode();

        public override string ToString()
            => _value.ToString();

        public static bool operator ==(WindowId lhs, WindowId rhs)
            => lhs.Equals(rhs);

        public static bool operator !=(WindowId lhs, WindowId rhs)
            => !lhs.Equals(rhs);
    }
}
