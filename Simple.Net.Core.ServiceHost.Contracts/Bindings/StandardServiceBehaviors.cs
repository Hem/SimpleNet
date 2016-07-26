using System.ServiceModel.Description;

namespace SimpleNet.ServiceHost.Contracts.Bindings
{
    public class StandardServiceBehaviors
    {
        public static IServiceBehavior HttpServiceMetadataBehavior
        {
            get
            {
                return new ServiceMetadataBehavior { HttpGetEnabled = true, MetadataExporter = { PolicyVersion = PolicyVersion.Policy15} };
            }
        }

        public static IServiceBehavior HttpsServiceMetadataBehavior
        {
            get
            {
                return new ServiceMetadataBehavior { HttpsGetEnabled = true, MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 } };
            }
        }

        public static IServiceBehavior TcpServiceMetadataBehavior
        {
            get
            {
                return new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true,
                    MetadataExporter = { PolicyVersion = PolicyVersion.Policy15}
                };
            }
        }
        public static IServiceBehavior ServiceDebugBehavior
        {
            get
            {
                var smb = new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true };
                return smb;
            }
        }
    }
}