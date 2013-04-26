namespace Ayls.WP8Toolkit.Converters
{
    public class BooleanToVisibilityConverter : FuncToVisibilityConverter
    {
        public BooleanToVisibilityConverter() : base(o => (bool)o)
        {
        }
    }
}
