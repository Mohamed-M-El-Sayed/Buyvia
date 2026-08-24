namespace OnlineStore.Application.Contracts.Services.Caching
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class InvalidateCacheAttribute(string tag) : Attribute
    {
        public string Tag { get; } = tag;
    }
}
