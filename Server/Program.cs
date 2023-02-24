using Server.Hubs;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddSimpleConsole(options => options.TimestampFormat = "HH:mm:ss.ffff ");
            });

            builder.Services.AddSignalR();

            var app = builder.Build();

            app.MapHub<StreamingHub>("/hubs/streaming");
            //app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
