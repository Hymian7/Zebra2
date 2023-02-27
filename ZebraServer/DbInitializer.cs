using Zebra.Library;

namespace ZebraServer
{
    public static class DbInitializer
    {
        public static void Initialize(ZebraContext context)
        {
            context.Database.EnsureCreated();

            // Inizialize here
            // https://docs.microsoft.com/de-de/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0
        }
    }
}