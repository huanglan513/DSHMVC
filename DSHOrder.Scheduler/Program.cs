using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Topshelf;

namespace DSHOrder.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Host host = HostFactory.New(x =>
            {
                x.Service<IQuartzServer>(s =>
                {
                    s.SetServiceName("quartz.server");
                    s.ConstructUsing(builder =>
                    {
                        QuartzServer server = new QuartzServer();
                        server.Initialize();
                        return server;
                    });
                    s.WhenStarted(server => server.Start());
                    s.WhenPaused(server => server.Pause());
                    s.WhenContinued(server => server.Resume());
                    s.WhenStopped(server => server.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription(QuartzConfig.ServiceDescription);
                x.SetDisplayName(QuartzConfig.ServiceDisplayName);
                x.SetServiceName(QuartzConfig.ServiceName);
            });

            host.Run();
        }
    }
}
