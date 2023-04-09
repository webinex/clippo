namespace Webinex.Clippo.Interceptors
{
    internal class MaxSizeSettings
    {
        public MaxSizeSettings(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}