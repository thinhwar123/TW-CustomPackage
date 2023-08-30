using UnityEngine;

namespace TW.Utility.CustomType
{
    /// <summary>
    /// The BitMask class represents a bitmask, which is a way of compactly storing a set of boolean values as individual bits of an integer.
    /// </summary>
    [System.Serializable]
    public class BitMask
    {
        /// <summary>
        /// The m_Mask field is the integer value used to store the bits of the bitmask.
        /// </summary>
        [SerializeField] private int m_Mask;
        /// <summary>
        /// The default constructor initializes the bitmask to 0.
        /// </summary>
        public BitMask()
        {
            m_Mask = 0;
        }
        /// <summary>
        /// The parameterized constructor initializes the bitmask to the provided integer value.
        /// </summary>
        /// <param name="mask">The integer value to initialize the bitmask to.</param>
        public BitMask(int mask)
        {
            m_Mask = mask;
        }
        /// <summary>
        /// The CheckIsTrue method checks whether the bit at the specified index is set to true.
        /// </summary>
        /// <param name="index">The index of the bit to check.</param>
        /// <returns>A boolean indicating whether the bit at the specified index is set.</returns>
        public bool CheckIsTrue(int index)
        {
            return (m_Mask & (1 << index)) != 0;
        }
        /// <summary>
        /// The SetValue method sets the bit at the specified index to the provided boolean value.
        /// </summary>
        /// <param name="index">The index of the bit to set.</param>
        /// <param name="value">The boolean value to set the bit to.</param>
        public void SetValue(int index, bool value)
        {
            if (value)
            {
                m_Mask |= (1 << index);
            }
            else
            {
                m_Mask &= ~(1 << index);
            }
        }
        /// <summary>
        /// The FlipValue method toggles the value of the bit at the specified index.
        /// </summary>
        /// <param name="index">The index of the bit to toggle.</param>
        public void FlipValue(int index)
        {
            m_Mask ^= (1 << index);
        }
        /// <summary>
        /// The SetTrueBelow method sets all the bits below the specified index to true.
        /// </summary>
        /// <param name="index">The index to set all the bits below to true.</param>
        public void SetTrueBelow(int index)
        {
            m_Mask = ((1 << index) - 1);
        }
        /// <summary>
        /// The GetLowestTrue method returns the index of the lowest true bit in the bitmask.
        /// </summary>
        /// <returns>The index of the lowest true bit.</returns>
        public int GetLowestTrue()
        {
            return m_Mask & (-m_Mask);
        }
    }

}