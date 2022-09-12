using AutoMapper;
using IRS.DTO;
using IRS.DTO.auth;
using IRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.Helpers.AutoMapper
{
    public class EFToDtoMappingProfile : Profile
    {
        public EFToDtoMappingProfile()
        {
            var list = new List<int> { };
            CreateMap<Account, AccountDto>();
            CreateMap<AccountType, AccountTypeDto>();
            CreateMap<Plan, PlanDto>();
            CreateMap<Mailing, MailingDto>();
            CreateMap<ToDoList, ToDoListDto>();
            CreateMap<XAccount, UserForDetailDto>()
                .ForMember(d => d.Username, o => o.MapFrom(x => x.Uid))
                .ForMember(d => d.ID, o => o.MapFrom(x => x.AccountId));
            CreateMap<Oc, OCDto>();
            CreateMap<AccountRole, AccountRoleDto>();
            CreateMap<AccountPermission, AccountPermissionDto>();
            CreateMap<AccountGroup, AccountGroupDto>();
            CreateMap<Employee, EmployeeDto>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<Method, MethodDto>();
            CreateMap<SystemLanguage, SystemLanguageDto>();
            CreateMap<FunctionSystem, FunctionDto>();
            CreateMap<Module, ModuleDto>();

            CreateMap<Role, RoleDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<XAccount, XAccountDto>();
            CreateMap<XAccountGroup, XAccountGroupDto>();
            CreateMap<SysMenu, SysMenuDto>();

            CreateMap<CustomerDto, Customer>();
 
            CreateMap<CodePermissionDto, CodePermission>();

            CreateMap<ReportConfigDto, ReportConfig>();

            CreateMap<StoredProcedureDto, StoredProcedure>();
            CreateMap<ChartSettingDto, SysMenu>();
            CreateMap<CodeTypeDto, CodeType>();
            CreateMap<DashboardDto, Dashboard>();
            CreateMap<SystemConfigDto, SystemConfig>();

            CreateMap<SiteDto, Site>();
            CreateMap<HallDto, Hall>();
            CreateMap<WorkOrderDto, WorkOrder>();
            CreateMap<BookingDetailDto, BookingDetail>();

            CreateMap<Glue, GlueDto>();
            CreateMap<Chemical, ChemicalDto>();
            CreateMap<GlueChemical, GlueChemicalDto>();
            CreateMap<Shoe, ShoeDto>();
            CreateMap<ShoeGlue, ShoeGlueDto>();

            CreateMap<Ink, InkDto>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<Process, ProcessDto>();

            CreateMap<Chemical2, Chemical2Dto>();
            CreateMap<TreatmentWay, TreatmentWayDto>();
            CreateMap<Color, ColorDto>();
            CreateMap<InkColor, InkColorDto>();
            CreateMap<ChemicalColor, ChemicalColorDto>();
            CreateMap<Part2, Part2Dto>();
            CreateMap<Schedule, ScheduleDto>();
            CreateMap<Process2, Process2Dto>();

            CreateMap<InInk, InInkDto>();
            CreateMap<InChemical, InChemicalDto>();
            CreateMap<WorkPlan, WorkPlanDto>();
            CreateMap<WorkPlanNew, WorkPlanNewDto>();
            CreateMap<WorkPlan2, WorkPlan2Dto>();


            CreateMap<StockInChemical, StockInChemicalDto>();
            CreateMap<StockInInk, StockInInkDto>();

        }

    }
}
