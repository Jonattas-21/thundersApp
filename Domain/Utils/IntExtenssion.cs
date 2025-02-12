namespace Domain.Utils
{
    public static class IntExtensions
    {
        public static bool IsBetweenAnalysis(this int value) // todo enum
        {
            return value >= 1 && value <= 5;
        }
    }
}
