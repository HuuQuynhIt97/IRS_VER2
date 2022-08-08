using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using IRS.Constants;
using IRS.Data;
using IRS.DTO;
using IRS.Helpers;
using IRS.Models;
using IRS.Services.Base;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IRS.Services
{
    public interface IDashboardService : IServiceBase<Dashboard, DashboardDto>
    {
        Task<object> GetDashboards();
        Task<object> GetDashboardsNav(string farmGuid);
        Task<object> GetDashboards(string dashboardGuid, DateTime yearMonth, string lang);
       
        Task<object> GetDashboards(string codeType, int top, int skip, string filter, string selected, string lang);
        Task<object> GetAreas(string codeType, int top, int skip, string filter, string selected, string lang);
        Task<object> LoadData(DataManager data);
        Task<object> LoadDashboardData(DataManager data, string farmGuid);
        Task<object> LoadAreaData(DataManager data, string dashboardGuid);
        Task<object> LoadItemData(DataManager data, string dashboardGuid, string areaGuid, List<string> type);
        Task<object> LoadData(DataManager data, string codeType);
        Task<object> GetAudit(object id);
        Task<bool> CheckExist(string areaGuid, string type);
    }
    public class DashboardService : ServiceBase<Dashboard, DashboardDto>, IDashboardService
    {
        private readonly IRepositoryBase<Dashboard> _repo;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISequenceService _sequenceService;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _currentEnvironment;

        public DashboardService(
            IRepositoryBase<Dashboard> repo,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            ISequenceService sequenceService,
            IMapper mapper,
            MapperConfiguration configMapper,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment currentEnvironment
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _sequenceService = sequenceService;
            _mapper = mapper;
            _configMapper = configMapper;
            _httpContextAccessor = httpContextAccessor;
            _currentEnvironment = currentEnvironment;
        }

        public async Task<object> GetDashboards()
        {
            var query = await _repo.FindAll(x => x.Status == 1 ).AsNoTracking().Select(x =>
                x.Text01
            ).Distinct().ToListAsync();
            return query.Select(x=> new { ID = x, Name = x });
        }

        /// <summary>
        /// Add account sau do add DashboardGroupDashboard
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(DashboardDto model)
        {
            try
            {
               
                var item = _mapper.Map<Dashboard>(model);
                item.Status =1;
                _repo.Add(item);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        /// <summary>
        /// Add account sau do add DashboardGroupDashboard
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> UpdateAsync(DashboardDto model)
        {
            try
            {
                var item = _mapper.Map<Dashboard>(model);
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public override async Task<List<DashboardDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1)
                .ProjectTo<DashboardDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }
        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            item.Status = 0;
            _repo.Update(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<object> GetDashboards(string codeType, int top, int skip, string filter, string selected, string lang)
        {
            var selectedData = await _repo.FindAll(x => x.Text01 == codeType && x.Status == 1 && x.DashBoardName == selected).Select(x => new {
                Guid = x.Guid,
                Name = x.DashBoardName

            }).ToListAsync();

            if (string.IsNullOrEmpty(filter) == true)
            {
                var query = _repo.FindAll(x => x.Type == codeType && x.Status == 1 )
                    .OrderBy(x => x.SortId)
                    .Skip(skip)
                    .Take(top)
                    .Select(x => new {
                        Guid = x.Guid,
                        Name = x.DashBoardName

                    });

                var data = await query.ToListAsync();
                return data.Union(selectedData).ToList();
            }
            else
            {
                var query = _repo.FindAll(x => x.Type.Contains(filter) && x.Status == 1).OrderBy(x => x.SortId).Skip(skip).Take(top).Select(x => new {
                    Guid = x.Guid,
                    Name = x.DashBoardName

                });

                var data = await query.ToListAsync();
                return data.Union(selectedData).ToList();
            }
        }
        public async Task<object> GetAreas(string codeType, int top, int skip, string filter, string selected, string lang)
        {
            var selectedData = await _repo.FindAll(x => x.Text01 == codeType && x.Status == 1 && x.AreaName == selected).OrderBy(x=> x.SortId).Select(x => new {
                Guid = x.Guid,
                Name = x.AreaName

            }).ToListAsync();

            if (string.IsNullOrEmpty(filter) == true)
            {
                var query = _repo.FindAll(x => x.Type == codeType && x.Status == 1)
                    .OrderBy(x => x.SortId)
                    .Skip(skip)
                    .Take(top)
                    .Select(x => new {
                        Guid = x.Guid,
                        Name = x.AreaName

                    });

                var data = await query.ToListAsync();
                return data.Union(selectedData).ToList();
            }
            else
            {
                var query = _repo.FindAll(x => x.Type.Contains(filter) && x.Status == 1).OrderBy(x => x.SortId).Skip(skip).Take(top).Select(x => new {
                    Guid = x.Guid,
                    Name = x.AreaName

                });

                var data = await query.ToListAsync();
                return data.Union(selectedData).ToList();
            }
        }
        public async Task<object> LoadData(DataManager data, string codeType)
        {
            if(string.IsNullOrEmpty(codeType))
            {
                return await LoadData(data);
            }
            if (codeType == "All")
            {
                return await LoadData(data);
            }
            IQueryable<DashboardDto> datasource = _repo.FindAll(x => x.Status == 1 && codeType == x.Type)
                .OrderByDescending(x => x.Id)
                .ProjectTo<DashboardDto>(_configMapper);
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }
        public async Task<object> LoadDashboardData(DataManager data, string farmGuid)
        {
          
            IQueryable<DashboardDto> datasource = _repo.FindAll(x => x.Status == 1 && x.Type == CodeTypeConst.Dashboard && x.FarmGuid == farmGuid)
                .OrderByDescending(x => x.Id)
                .ProjectTo<DashboardDto>(_configMapper);
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }
        public async Task<object> LoadAreaData(DataManager data, string dashboardGuid)
        {

            IQueryable<DashboardDto> datasource = _repo.FindAll(x => x.Status == 1 && dashboardGuid == x.UpperDashBoard && x.Type == CodeTypeConst.Area)
                .OrderByDescending(x => x.Id)
                .ProjectTo<DashboardDto>(_configMapper);
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }
        public async Task<object> LoadItemData(DataManager data, string dashboardGuid, string areaGuid, List<string> type)
        {

            IQueryable<DashboardDto> datasource = _repo.FindAll(x =>type.Contains(x.Type) && x.Status == 1 && dashboardGuid == x.UpperDashBoard && areaGuid == x.UpperArea )
                .OrderByDescending(x => x.Id)
                .ProjectTo<DashboardDto>(_configMapper);
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }

        public async Task<object> LoadData(DataManager data)
        {
            IQueryable<DashboardDto> datasource = _repo.FindAll(x => x.Status == 1)
                .OrderByDescending(x => x.Id)
                .ProjectTo<DashboardDto>(_configMapper);
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }
        public async Task<object> GetAudit(object id)
        {
            var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync();
            string createBy = "N/A";
            string createDate = "N/A";
            string updateBy = "N/A";
            string updateDate = "N/A";
            if (data == null)
                return new
                {
                    createBy,
                    createDate,
                    updateBy,
                    updateDate
                };
            //if (data.UpdateBy.HasValue)
            //{
            //    var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.UpdateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
            //    updateBy = updateBy != null ? updateAudit.Uid : "N/A";
            //    updateDate = data.UpdateDate.HasValue ? data.UpdateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            //}
            if (data.CreateBy.HasValue)
            {
                var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.CreateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                createBy = createAudit != null ? createAudit.Uid : "N/A";
                createDate = data.CreateDate.HasValue ? data.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            }
            return new
            {
                createBy,
                createDate,
                updateBy,
                updateDate
            };
        }

        public async Task<object> GetDashboards(string dashboardGuid, DateTime yearMonth, string lang)
        {
            var types = new List<string> {"A", "B", "C", "D"};
            var data = new List<DashboardArea>();
              var areaData = await _repo.FindAll(x => x.Status == 1 && x.Type == CodeTypeConst.Area && x.UpperDashBoard == dashboardGuid ).AsNoTracking().OrderBy(x => x.SortId).Select(x=> new {
                 x.AreaNo,
                 x.Guid,
                 x.AreaName,
                 x.SortId
             }).OrderBy(x=> x.SortId).ToListAsync();
             foreach (var item in areaData)
             {
                 var itemArea = new DashboardArea();
                 itemArea.AreaName  = item.AreaName;
                 itemArea.AreaNo  = item.AreaNo;
                 itemArea.Guid  = item.Guid;
                 itemArea.DashboardGuid  = dashboardGuid;
                itemArea.Sort = item.SortId;
                 var charts = await _repo.FindAll(x => x.CreateDate.Value.Year == yearMonth.Year && x.CreateDate.Value.Month == yearMonth.Month && x.Type == "E" && x.Status == 1 && x.UpperArea == item.Guid && x.UpperDashBoard == dashboardGuid )
                    .AsNoTracking().OrderBy(x=> x.SortId).Select(x=> new ChartItem {
                    Guid =  x.Guid,
                    UpperArea =  x.UpperArea,
                    UpperDashBoard =  x.UpperDashBoard,
                        ChartTitle=x.ChartTitle,
                    Text01 =  x.Text01,
                    Text02 =  x.Text02,
                    Value01 =  x.Value01,
                    Value02 =  x.Value02,
                    ValueColor01 =  x.ValueColor01,
                    ValueColor02 =  x.ValueColor02,
                    CreateDate =  x.CreateDate,
                
                }).ToListAsync();
                var chartItems = new List<ChartItemData>();
                foreach (var itemChart in charts)
                {
                    var it = new ChartItemData();
                    it.Title = itemChart.ChartTitle;
                    var dataSource = new List<ChartItemDataSource>();
                    dataSource.Add(new ChartItemDataSource{
                        Fill = itemChart.ValueColor01,
                        DataSource = new List<dynamic> {new {y = itemChart.Value01, x = itemChart.Text01}},
                    });
                     dataSource.Add( new ChartItemDataSource{
                        Fill = itemChart.ValueColor02,
                        DataSource = new List<dynamic> {new {y = itemChart.Value02, x = itemChart.Text02}},
                    });
                    it.Data = dataSource;
                    chartItems.Add(it);
                }
                itemArea.ChartItems = chartItems;

                 var normalItemsA = await _repo.FindAll(x => x.CreateDate.Value.Year == yearMonth.Year && x.CreateDate.Value.Month == yearMonth.Month && x.Type == "A" && x.Status == 1 && x.UpperArea == item.Guid && x.UpperDashBoard == dashboardGuid )
                    .AsNoTracking().OrderBy(x=> x.SortId).OrderBy(x=> x.SortId).Select(x=> new NormalItem
                    {
                        Guid = x.Guid,
                        Type = x.Type,
                        UpperArea = x.UpperArea,
                        UpperDashBoard = x.UpperDashBoard,
                        Text01 = x.Text01,
                        TextColor01 = x.TextColor01,
                        Value01 = x.Value01,
                        Value02 = x.Value02,
                        ValueColor01 = x.ValueColor01,
                        ValueColor02 = x.ValueColor02,
                        CreateDate = x.CreateDate,

                    }).ToListAsync();
                itemArea.NormalItemsA = normalItemsA;

                var normalItemsB = await _repo.FindAll(x => x.CreateDate.Value.Year == yearMonth.Year && x.CreateDate.Value.Month == yearMonth.Month && x.Type == "B" && x.Status == 1 && x.UpperArea == item.Guid && x.UpperDashBoard == dashboardGuid)
                   .AsNoTracking().OrderBy(x=> x.SortId).Select(x => new NormalItem
                   {
                       Guid = x.Guid,
                       Type = x.Type,
                       UpperArea = x.UpperArea,
                       UpperDashBoard = x.UpperDashBoard,
                       Text01 = x.Text01,
                       TextColor01 = x.TextColor01,
                       Value01 = x.Value01,
                       Value02 = x.Value02,
                       ValueColor01 = x.ValueColor01,
                       ValueColor02 = x.ValueColor02,
                       CreateDate = x.CreateDate,
                   }).ToListAsync();
                itemArea.NormalItemsB = normalItemsB;

                var normalItemsC = await _repo.FindAll(x => x.CreateDate.Value.Year == yearMonth.Year && x.CreateDate.Value.Month == yearMonth.Month && x.Type == "C" && x.Status == 1 && x.UpperArea == item.Guid && x.UpperDashBoard == dashboardGuid)
                   .AsNoTracking().OrderBy(x=> x.SortId).Select(x => new NormalItem
                   {
                       Guid = x.Guid,
                       Type = x.Type,
                       UpperArea = x.UpperArea,
                       UpperDashBoard = x.UpperDashBoard,
                       Text01 = x.Text01,
                       TextColor01 = x.TextColor01,
                       Value01 = x.Value01,
                       Value02 = x.Value02,
                       ValueColor01 = x.ValueColor01,
                       ValueColor02 = x.ValueColor02,
                       CreateDate = x.CreateDate,

                   }).ToListAsync();
                itemArea.NormalItemsC = normalItemsC;

                var normalItemsD = await _repo.FindAll(x => x.CreateDate.Value.Year == yearMonth.Year && x.CreateDate.Value.Month == yearMonth.Month && x.Type == "D" && x.Status == 1 && x.UpperArea == item.Guid && x.UpperDashBoard == dashboardGuid)
                  .AsNoTracking().OrderBy(x=> x.SortId).Select(x => new NormalItem
                  {
                      Guid = x.Guid,
                      Type = x.Type,
                      UpperArea = x.UpperArea,
                      UpperDashBoard = x.UpperDashBoard,
                      Text01 = x.Text01,
                      TextColor01 = x.TextColor01,
                      Value01 = x.Value01,
                      Value02 = x.Value02,
                      ValueColor01 = x.ValueColor01,
                      ValueColor02 = x.ValueColor02,
                      CreateDate = x.CreateDate,

                  }).ToListAsync();
                itemArea.NormalItemsD = normalItemsD;


                var tableItems = await _repo.FindAll(x => (x.LangID == null || x.LangID == "" || x.LangID == lang) && x.CreateDate.Value.Year == yearMonth.Year && x.CreateDate.Value.Month == yearMonth.Month && x.Type == "G" && x.Status == 1 && x.UpperArea == item.Guid && x.UpperDashBoard == dashboardGuid )
                    .AsNoTracking().OrderBy(x=> x.SortId).Select(x=> new TableItem {
                    Guid =  x.Guid,
                    UpperArea =  x.UpperArea,
                    UpperDashBoard =  x.UpperDashBoard,
                    Text01 =  x.Text01,
                    TextColor01 =  x.TextColor01,
                    Value01 =  x.Value01,
                    Value02 =  x.Value02,
                    Value04=  x.Value04,
                    Value03=  x.Value03,
                    Value05=  x.Value05,
                    Value06=  x.Value06,
                    Value07=  x.Value07,
                    Value08=  x.Value08,
                    Value09=  x.Value09,
                    ValueColor01=  x.ValueColor01,
                    ValueColor02=  x.ValueColor02,
                    ValueColor04=  x.ValueColor04,
                    ValueColor03=  x.ValueColor03,
                    ValueColor05=  x.ValueColor05,
                    ValueColor06=  x.ValueColor06,
                    ValueColor07=  x.ValueColor07,
                    ValueColor08=  x.ValueColor08,
                    ValueColor09=  x.ValueColor09,
                    CreateDate =  x.CreateDate,
                
                }).ToListAsync();
                itemArea.TableItems = tableItems;
                itemArea.HasChart = chartItems.Any();
                itemArea.HasTable = tableItems.Any();
                itemArea.HasNormalA = normalItemsA.Any();
                itemArea.HasNormalB = normalItemsB.Any();
                itemArea.HasNormalC = normalItemsC.Any();
                itemArea.HasNormalD = normalItemsD.Any();
                data.Add(itemArea);
             }    
            return data;
        }
       
        public async Task<object> GetDashboardsNav(string farmGuid)
        {
             var data = await _repo.FindAll(x => x.FarmGuid == farmGuid && x.Status == 1 && x.Type == CodeTypeConst.Dashboard).AsNoTracking().Select(x=> new {
                 x.Guid,
                 x.DashBoardName
             }).ToListAsync();
             return data;
        }

        public async Task<bool> CheckExist(string areaGuid, string type)
        {
            var  items = await _repo.FindAll(x =>x.UpperArea == areaGuid).AsNoTracking().Select(x=> x.Type).Distinct().ToListAsync();
            bool flag = false;
            foreach (var item in items)
            {
                if(type == item)
                {
                    flag = true;
                    break;
                }    
            }
            return flag;
        }
    }
}
