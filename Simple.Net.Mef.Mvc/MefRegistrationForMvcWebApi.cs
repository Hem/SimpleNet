using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Web.Http;
using System.Web.Mvc;

namespace Simple.Net.Mef.Mvc
{
    public static class MefRegistrationForMvcWebApi
    {

        public static CompositionContainer GetMefContainer(string binDirPath, CompositionBatch batch = null, RegistrationBuilder builder = null)
        {
            if (builder == null)
                builder = new RegistrationBuilder();

            builder.ForTypesDerivedFrom<Controller>()
                            .SetCreationPolicy(CreationPolicy.NonShared).Export();

            builder.ForTypesDerivedFrom<ApiController>()
                .SetCreationPolicy(CreationPolicy.NonShared).Export();

            
            var catalogs = new DirectoryCatalog(binDirPath, builder);

            var container = new CompositionContainer(catalogs, CompositionOptions.DisableSilentRejection |
                                                               CompositionOptions.IsThreadSafe);

            if (batch == null)
                batch = new CompositionBatch();

            // make container availalbe for di
            batch.AddExportedValue(container);

            container.Compose(batch);


            return container;
        }

    }
}
