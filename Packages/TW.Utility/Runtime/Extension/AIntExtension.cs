namespace TW.Utility.Extension
{
    public static class AIntExtension
    {
        public static int Repeat(this int value, int length)
        {
            while (value >= length)
            {
                value -= length;
            }

            return value;
        }
    }
}