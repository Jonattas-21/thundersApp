namespace Domain.Utils
{
    public static class IntExtensions
    {
        public static bool IsBetweenAnalysis(this int value)
        {
            return value >= 1 && value <= 5;
        }
    }
}
