namespace Simple.Net.Core.ServiceHost.Contracts
{
    public interface ISelfContainedHost
    {
        ServiceStatus ServiceStatus { get; }

        void Start();

        void Stop();

        void Pause();

    }
}
