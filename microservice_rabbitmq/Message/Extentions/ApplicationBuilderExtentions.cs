using Message.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message.Extentions
{
    public static class ApplicationBuilderExtentions
    {
        public static EventBusRabbitMQConsumer Listener { get; set; }
        
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusRabbitMQConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStart);
            //life.ApplicationStarted.Register(OnStoping);

            return app;
        }

        private static void OnStart()
        {
            Listener.Consume();
        }

        private static void OnStoping()
        {
            Listener.Disconnect();
        }
    }
}
