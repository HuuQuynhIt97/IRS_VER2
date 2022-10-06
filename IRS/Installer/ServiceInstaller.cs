using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IRS.Helpers;
using IRS.Services;

namespace IRS.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailingService, MailingService>();
            services.AddScoped<IAccountPermissionService, AccountPermissionService>();
            services.AddScoped<IAccountRoleService, AccountRoleService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IMethodService, MethodService>();
            services.AddScoped<ISystemLanguageService, SystemLanguageService>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<IRoomService, RoomService>();

            services.AddScoped<IXAccountService, XAccountService>();
            services.AddScoped<IXAccountGroupService, XAccountGroupService>();
            services.AddScoped<ISysMenuService, SysMenuService>();

            services.AddScoped<IReportService, ReportService> ();

            services.AddScoped<ICustomerService, CustomerService>();
           
            services.AddScoped<ICodePermissionService, CodePermissionService>();

            services.AddScoped<IReportConfigService, ReportConfigService>();

           
            services.AddScoped<IStoredProcedureService, StoredProcedureService>();
            services.AddScoped<ICodeTypeService, CodeTypeService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ISystemConfigService, SystemConfigService>();

            services.AddScoped<IHallService, HallService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IWorkOrderService, WorkOrderService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ISequenceService, SequenceService>();


            services.AddScoped<IChemicalService, ChemicalService>();
            services.AddScoped<IGluesService, GluesService>();
            services.AddScoped<IShoeService, ShoeService>();
            services.AddScoped<IGlueChemicalService, GlueChemicalService>();
            services.AddScoped<IShoeGlueService, ShoeGlueService>();

            services.AddScoped<IInkService, InkService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<IChemicalService, ChemicalService>();
            services.AddScoped<ITreatmentWayService, TreatmentWayService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IInkColorService, InkColorService>();
            services.AddScoped<IChemicalColorService, ChemicalColorService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IProcess2Service, Process2Service>();
            services.AddScoped<IInInkService, InInkService>();
            services.AddScoped<IInChemicalService, InChemicalService>();
            services.AddScoped<IWorkPlanService, WorkPlanService>();
            services.AddScoped<IWorkPlan2Service, WorkPlan2Service>();
            services.AddScoped<IColorWorkPlanService, ColorWorkPlanService>();

            services.AddScoped<IStockInInkService, StockInInkService>();
            services.AddScoped<IStockInChemicalService, StockInChemicalService>();
        }
    }
}
