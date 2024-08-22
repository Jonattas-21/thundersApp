using System.ComponentModel;

namespace Domain.Attributes
{
    public class DefaultDateNowAttribute : DefaultValueAttribute
    {
        public DefaultDateNowAttribute()
            : base(DateTime.Now)
        {
        }
    }
}
