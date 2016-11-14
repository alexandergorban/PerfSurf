using Microsoft.AspNet.SignalR;
using PerfSurf.Counters;
using System.Threading.Tasks;

namespace PerfSurf.Hubs
{
    public class PerfHub : Hub
    {
        public PerfHub()
        {
            StartCounterCollection();
        }

        private void StartCounterCollection()
        {
            var task = Task.Factory.StartNew(async () =>
            {
                var perfService = new PerfCounterService();
                while (true)
                {
                    var result = perfService.GetResults();
                    Clients.All.newCounters(result);
                    await Task.Delay(2000);
                }
            });
        }

        public void Send(string message)
        {
            Clients.All.newMessage(
                Context.User.Identity.Name + " says " + message
                );
        }
    }
}