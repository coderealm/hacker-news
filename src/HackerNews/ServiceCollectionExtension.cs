using HackerNews.Handlers;
using HackerNews.Models;
using HackerNews.Http;
using MediatR;
using Serilog;
using Serilog.Formatting.Compact;
using HackerNews.Service;
using HackerNews.Serialize;

namespace HackerNews
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStoryRequestModel, StoryRequestModel>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<IHackerNewsService, HackerNewsService>();
            services.AddScoped<ISerializer, Serializer>();
            services.AddScoped(typeof(IRequest<List<StoryResponseModel>>), typeof(StoryRequestModel));
            services.AddScoped(typeof(IRequestHandler<StoryRequestModel, List<StoryResponseModel>>), typeof(BestStoryHandler));
            services.AddHttpClient<HttpService>();
            services.AddMemoryCache();         

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.File(new CompactJsonFormatter(), "Log/log.txt")
            .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            return services;
        }
    }
}
