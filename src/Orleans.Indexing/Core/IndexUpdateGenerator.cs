using System;
using System.Reflection;

namespace Orleans.Indexing
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [GenerateSerializer]
    internal class IndexUpdateGenerator : IIndexUpdateGenerator
    {
        [Id(0)]
        PropertyInfo prop;
        [Id(1)]
        object? nullValue;

        public IndexUpdateGenerator(PropertyInfo prop)
        {
            this.prop = prop;
            this.nullValue = IndexUtils.GetNullValue(prop);
        }

        public IMemberUpdate CreateMemberUpdate(object? gProps, object? befImg)
        {
            object? aftImg = gProps is null ? null : ExtractIndexImage(gProps);
            return new MemberUpdate(befImg, aftImg);
        }

        public IMemberUpdate CreateMemberUpdate(object aftImg)
            => new MemberUpdate(null, aftImg);

        public object? ExtractIndexImage(object gProps)
        {
            var currentValue = this.prop.GetValue(gProps);
            return currentValue is null || this.nullValue is null
                ? currentValue
                : (currentValue.Equals(this.nullValue) ? null : currentValue);
        }
    }
}
