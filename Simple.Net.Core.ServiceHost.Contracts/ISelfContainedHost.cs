namespace SimpleNet.ServiceHost.Contracts
{
    public interface ISelfContainedHost
    {
        ServiceStatus ServiceStatus { get; }

        void Start();

        void Stop();

        void Pause();

    }
}
