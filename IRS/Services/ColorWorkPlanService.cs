using AutoMapper;
using AutoMapper.QueryableExtensions;
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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;

namespace IRS.Services
{
    public interface IColorWorkPlanService
    {
        Task<object> LoadDataColorWorkPlan(DataManager data);
        Task<OperationResult> DeleteColorWorkPlan(int Id);
        Task<object> LoadDataColorWorkPlan2();
        Task<object> LoadShoes();
        Task<OperationResult> AddColorWorkPlan(ColorWorkPlanDto model);
        Task<OperationResult> UpdateColorWorkPlan(ColorWorkPlanDto model);
        Task<object> StoreProcedureCreateColorTodo(DateTime currentDate);

        Task<object> LoadColorToDo();
        Task<OperationResult> UpdateIsFinishedColorToDo(ColorTodoDto model);
        Task<OperationResult> UpdateColorToDoAmount(ColorTodoDto model);
    }
    public class ColorWorkPlanService : IColorWorkPlanService
    {
        
        private readonly IRepositoryBase<ColorWorkPlan> _repoColorWP;
        private readonly IRepositoryBase<ColorWorkPlanNew> _repoColorWPNew;
        private readonly IRepositoryBase<Shoe> _repoShoe;
        private readonly IRepositoryBase<Process> _repoTreatment;
        private readonly IRepositoryBase<Process2> _repoProcess;
        private readonly IConfiguration _configuration;
       

        private readonly IRepositoryBase<ColorTodo> _repoColorTodo;
        private readonly IRepositoryBase<Models.Schedule> _repoSchedule;
        private readonly IRepositoryBase<Color> _repoColor;
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public ColorWorkPlanService(
           
            IRepositoryBase<ColorWorkPlan> repoColorWP,
            IRepositoryBase<ColorWorkPlanNew> repoColorWPNew,
            IRepositoryBase<Shoe> repoShoe,
            
            IRepositoryBase<Process> repoTreatment,
            IRepositoryBase<Process2> repoProcess,

            IConfiguration configuration,


            IRepositoryBase<ColorTodo> repoColorTodo,
            IRepositoryBase<Models.Schedule> repoSchedule,
            IRepositoryBase<Color> repoColor,
            
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
           
            _repoColorWP = repoColorWP;
            _repoColorWPNew = repoColorWPNew;
            _repoShoe = repoShoe;
            _repoTreatment = repoTreatment;
            _repoProcess = repoProcess;
            _configuration = configuration;
            _repoColorTodo = repoColorTodo;
            _repoSchedule = repoSchedule;
            _repoColor = repoColor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<object> LoadDataColorWorkPlan(DataManager data)
        {
            // IQueryable<FunctionDto> datasource = _repoFunctionSystem.FindAll().ProjectTo<FunctionDto>(_configMapper);

            var listShoe = (from a in _repoShoe.FindAll().ToList()
                            let ModelName = a.ModelName.ToSafetyString()
                            let ModelNo = a.ModelNo.ToSafetyString() != string.Empty ? ModelName != string.Empty ? " - " + a.ModelNo.ToSafetyString() : a.ModelNo.ToSafetyString() : ""
                            let Article1 = a.Article1.ToSafetyString() != string.Empty ? " - " + a.Article1.ToSafetyString() : ""
                            let Treatment = _repoTreatment.FindAll().Where(x => x.Guid == a.TreatmentGuid).FirstOrDefault() != null
                                            ? " - (" + _repoTreatment.FindAll().Where(x => x.Guid == a.TreatmentGuid).FirstOrDefault().Name + ")" : ""

                            let Process = _repoProcess.FindAll().Where(x => x.Guid == a.ProcessGuid).FirstOrDefault() != null
                                            ? " - " + _repoProcess.FindAll().Where(x => x.Guid == a.ProcessGuid).FirstOrDefault().Name : ""

                            let Version = a.Version.ToSafetyString() != string.Empty ? " - " + a.Version.ToString() : ""
                            select new
                            {
                                Id = a.Id,
                                Guid = a.Guid,
                                Name = ModelName + ModelNo + Article1 + Treatment + Process + Version
                            }).ToList();
           
            var result = (from a in _repoColorWP.FindAll().OrderByDescending(x=> x.Id).ToList()
                          let Shoe = listShoe.Where(x => x.Guid == a.ShoeGuid).FirstOrDefault() != null
                                     ? listShoe.Where(x => x.Guid == a.ShoeGuid).FirstOrDefault().Name : ""     
                          select new ColorWorkPlanDto {
                                Id = a.Id,
                                Guid = a.Guid,
                                ShoeGuid = a.ShoeGuid,
                                ShoeName = Shoe,
                                CreateDate = a.CreateDate,
                                CreateBy = a.CreateBy,
                                ExecuteDate = a.ExecuteDate,
                                Active = a.Active
                           });

            var queryable = result.AsQueryable();
            var datasource = queryable;
            
            var count = datasource.Count();
            if (data.Where != null) // for filtering
               datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
               datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
               datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = datasource.Count();
            if (data.Skip >= 0)//for paging
               datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
               datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
               Result = datasource,
               Count = count
            };
        }

        public async Task<object> LoadDataColorWorkPlan2()
        {
            // IQueryable<FunctionDto> datasource = _repoFunctionSystem.FindAll().ProjectTo<FunctionDto>(_configMapper);

            var listShoe = (from a in _repoShoe.FindAll().ToList()
                            let ModelName = a.ModelName.ToSafetyString()
                            let ModelNo = a.ModelNo.ToSafetyString() != string.Empty ? ModelName != string.Empty ? " - " + a.ModelNo.ToSafetyString() : a.ModelNo.ToSafetyString() : ""
                            let Article1 = a.Article1.ToSafetyString() != string.Empty ? " - " + a.Article1.ToSafetyString() : ""
                            let Treatment = _repoTreatment.FindAll().Where(x => x.Guid == a.TreatmentGuid).FirstOrDefault() != null
                                            ? " - (" + _repoTreatment.FindAll().Where(x => x.Guid == a.TreatmentGuid).FirstOrDefault().Name + ")" : ""

                            let Process = _repoProcess.FindAll().Where(x => x.Guid == a.ProcessGuid).FirstOrDefault() != null
                                            ? " - " + _repoProcess.FindAll().Where(x => x.Guid == a.ProcessGuid).FirstOrDefault().Name : ""

                            let Version = a.Version.ToSafetyString() != string.Empty ? " - " + a.Version.ToString() : ""
                            select new
                            {
                                Id = a.Id,
                                Guid = a.Guid,
                                Name = ModelName + ModelNo + Article1 + Treatment + Process + Version
                            }).ToList();

            var result = (from a in _repoColorWP.FindAll().OrderByDescending(x => x.Id).ToList()
                          let Shoe = listShoe.Where(x => x.Guid == a.ShoeGuid).FirstOrDefault() != null
                                     ? listShoe.Where(x => x.Guid == a.ShoeGuid).FirstOrDefault().Name : ""
                          select new ColorWorkPlanDto
                          {
                              Id = a.Id,
                              Guid = a.Guid,
                              ShoeGuid = a.ShoeGuid,
                              ShoeName = Shoe,
                              CreateDate = a.CreateDate,
                              CreateBy = a.CreateBy,
                              ExecuteDate = a.ExecuteDate,
                              Active = a.Active
                          }).ToList();


            return result;
        }

        public async Task<object> LoadShoes() 
        {
            var listShoe =  (from a in await _repoShoe.FindAll().ToListAsync()
                            let ModelName = a.ModelName.ToSafetyString()
                            let ModelNo = a.ModelNo.ToSafetyString() != string.Empty ? ModelName != string.Empty ? " - " + a.ModelNo.ToSafetyString() : a.ModelNo.ToSafetyString() : ""
                            let Article1 = a.Article1.ToSafetyString() != string.Empty ? " - " + a.Article1.ToSafetyString() : ""
                            let Treatment = _repoTreatment.FindAll().Where(x => x.Guid == a.TreatmentGuid).FirstOrDefault() != null
                                            ? " - (" + _repoTreatment.FindAll().Where(x => x.Guid == a.TreatmentGuid).FirstOrDefault().Name + ")" : ""

                            let Process = _repoProcess.FindAll().Where(x => x.Guid == a.ProcessGuid).FirstOrDefault() != null
                                            ? " - " + _repoProcess.FindAll().Where(x => x.Guid == a.ProcessGuid).FirstOrDefault().Name : ""

                            let Version = a.Version.ToSafetyString() != string.Empty ? " - " + a.Version.ToString() : ""
                            select new
                            {
                                Id = a.Id,
                                Guid = a.Guid,
                                Name = ModelName + ModelNo + Article1 + Treatment + Process + Version
                            }).OrderByDescending(x => x.Id).ToList();
            return listShoe;
        }

        public async Task<OperationResult> AddColorWorkPlan(ColorWorkPlanDto model)
        {
            try
            {
                if (model.ShoeGuid == string.Empty || model.ShoeGuid == null)
                {
                    operationResult = new OperationResult
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = MessageReponse.AddSuccess,
                        Success = false,
                        Data = model
                    };
                    return operationResult;
                }
                var item = _mapper.Map<ColorWorkPlan>(model);
                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                item.CreateDate = DateTime.Now;
                item.Active = 0;
                _repoColorWP.Add(item);

               var data = await _unitOfWork.SaveChangeAsync();

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

        public async Task<OperationResult> UpdateColorWorkPlan(ColorWorkPlanDto model)
        {
            try
            {
                var item = _mapper.Map<ColorWorkPlan>(model);
            
                _repoColorWP.Update(item);
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

        public async Task<OperationResult> DeleteColorWorkPlan(int Id)
        {
            var item = await _repoColorWP.FindAll(x => x.Id == Id).FirstOrDefaultAsync();
            try
            {
               
                _repoColorWP.Remove(item);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<object> StoreProcedureCreateColorTodo(DateTime currentDate)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    await conn.OpenAsync();
                }
                    string storeName = "CreateColorTodo";
                    string executeDate = currentDate.ToString("yyyy-MM-dd");
                    var values = new { exedate = executeDate, building = 1 };
                try
                {
                    var data = await conn.QueryAsync(storeName, values, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    return new object[] {};
                }
            }
        }

        public async Task<object> LoadColorToDo()
        {
            var query = await (from a in _repoColorTodo.FindAll()
                            join b in _repoSchedule.FindAll() on a.SchedulesGuid equals b.Guid
                            join c in _repoColor.FindAll() on b.ColorGuid equals c.Guid
                            
                            select new ColorTodoDto()
                            {
                                Id = a.Id,
                                Guid = a.Guid.ToString(),
                                ColorName = c.Name,
                                ExecuteDate = a.ExecuteDate,
                                ExecuteAmount = a.ExecuteAmount,
                                IsFinished = a.IsFinished

                            }).OrderByDescending(x => x.ExecuteDate).ToListAsync();
            return query;
        }

        public async Task<OperationResult> UpdateIsFinishedColorToDo(ColorTodoDto model)
        {
            var item = await _repoColorTodo.FindByIDAsync(model.Id);
            if (item == null)
            {
                return new OperationResult { 
                    StatusCode = HttpStatusCode.NotFound, 
                    Message = "Update Error", Success = false 
                    };
            }
            item.IsFinished = 1;
            item.FinishedBy = model.FinishedBy;
            item.FinishedTime = model.FinishedTime;
            
           try
            {
                _repoColorTodo.Update(item);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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

        public async Task<OperationResult> UpdateColorToDoAmount(ColorTodoDto model)
        {
            var item = await _repoColorTodo.FindByIDAsync(model.Id);
            if (item == null)
            {
                return new OperationResult { 
                    StatusCode = HttpStatusCode.NotFound, 
                    Message = "Update Error", Success = false 
                    };
            }
            item.ExecuteAmount = model.ExecuteAmount;
            
           try
            {
                _repoColorTodo.Update(item);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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

    }
}
