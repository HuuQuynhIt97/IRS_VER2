using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IRS.Data;
using System;

namespace IRS.Installer
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        }
    }
}
