namespace Ayls.WP8Toolkit.Converters
{
    public abstract class BooleanToStringConverter : FuncToStringConverter
    {
        protected BooleanToStringConverter(string stringOnTrue, string stringOnFalse) : base (o => (bool)o ? stringOnTrue : stringOnFalse)
        {
        }
    }
}
