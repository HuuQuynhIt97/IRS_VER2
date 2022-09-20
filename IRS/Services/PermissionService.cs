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

namespace IRS.Services
{
    public interface IPermissionService
    {
        Task<List<FunctionSystem>> GetAllFunction();
        Task<IEnumerable<HierarchyNode<FunctionTreeDto>>> GetFunctionsAsTreeView();
        Task<object> GetModulesAsTreeView();
        Task<List<Module>> GetAllModule();
        Task<object> LoadData(DataManager data);
        Task<object> LoadActionData(DataManager data);
        Task<object> LoadModuleData(DataManager data);
        Task<List<Models.Action>> GetAllAction();

        Task<OperationResult> GetAllFunctionByPermision();
        Task<OperationResult> UpdateModule(ModuleDto module);
        Task<OperationResult> DeleteModule(int moduleID);
        Task<OperationResult> AddModule(ModuleDto module);

        Task<OperationResult> UpdateFunction(FunctionDto module);
        Task<OperationResult> DeleteFunction(int functionID);
        Task<OperationResult> AddFunction(FunctionDto module);

        Task<OperationResult> UpdateAction(Models.Action module);
        Task<OperationResult> DeleteAction(int actionID);
        Task<OperationResult> AddAction(Models.Action module);
        Task<OperationResult> PutPermissionByRoleId(int roleID, UpdatePermissionRequest request);
        Task<OperationResult> PostActionToFunction(int functionID, ActionAssignRequest request);
        Task<OperationResult> DeleteActionToFunction(int functionID, ActionAssignRequest request);
        Task<object> GetMenuByUserPermission(int userId, string lang);
        Task<object> GetMenuByLangID(int userId, string langID);
        Task<object> GetScreenAction(int functionID);
        Task<object> GetActionInFunctionByRoleID(int roleID);
        Task<object> GetScreenFunctionAndAction(ScreenFunctionAndActionRequest request);
    }
    public class PermissionService : IPermissionService
    {
        private readonly IRepositoryBase<Permission> _repo;
        private readonly IRepositoryBase<Role> _repoRole;
        private readonly IRepositoryBase<AccountRole> _repoAccountRole;
        private readonly IRepositoryBase<AccountGroup> _repoAccountGroup;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IRepositoryBase<Module> _repoModule;
        private readonly IRepositoryBase<ActionInFunctionSystem> _repoActionInFunctionSystem;
        private readonly IRepositoryBase<Models.Action> _repoAction;
        private readonly IRepositoryBase<FunctionSystem> _repoFunctionSystem;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private OperationResult operationResult;
        public PermissionService(
            IRepositoryBase<Permission> repo,
            IRepositoryBase<Role> repoRole,
            IRepositoryBase<AccountRole> repoAccountRole,
            IRepositoryBase<AccountGroup> repoAccountGroup,
            IRepositoryBase<Account> repoAccount,
            IRepositoryBase<Module> repoModule,
            IRepositoryBase<ActionInFunctionSystem> repoActionInFunctionSystem,
            IRepositoryBase<Models.Action> repoAction,
            IRepositoryBase<FunctionSystem> repoFunctionSystem,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
        {
            _repo = repo;
            _repoRole = repoRole;
            _repoAccountRole = repoAccountRole;
            _repoAccountGroup = repoAccountGroup;
            _repoAccount = repoAccount;
            _repoModule = repoModule;
            _repoActionInFunctionSystem = repoActionInFunctionSystem;
            _repoAction = repoAction;
            _repoFunctionSystem = repoFunctionSystem;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }


        #region Module
        public async Task<object> GetModulesAsTreeView()
        {
            var model = (await _repoModule.FindAll()
                .OrderBy(x => x.Sequence).ToListAsync())
                .Select((x, i) => new HierarchyNode<ModuleTreeDto>
                {
                    Entity = new ModuleTreeDto
                    {
                        Index = ++i,
                        ID = x.ID,
                        Code = x.Code,
                        Name = x.EN,
                        Icon = x.Icon,
                        Url = x.Url,
                        Sequence = x.Sequence,
                        LanguageID = "",
                        Level = 1,
                    }
                }).ToList();

            return model;
        }
        public async Task<OperationResult> UpdateModule(ModuleDto module)
        {

            try
            {
               // var module = _mapper.Map<Module>(moduleDto);
                var item = await _repoModule.FindByIDAsync(module.ID);
                item.TW = module.TW;
                item.VN = module.VN;
                item.EN = module.EN;
                item.CN = module.CN;
                item.Icon = module.Icon;
                item.Url = module.Url;
                item.Sequence = module.Sequence;
                _repoModule.Update(item);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = module
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;

        }

        public async Task<OperationResult> DeleteModule(int moduleID)
        {
            var module = await _repoModule.FindAll(x => x.ID == moduleID).FirstOrDefaultAsync();
            try
            {
               
                _repoModule.Remove(module);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = module
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
      

        public async Task<OperationResult> AddModule(ModuleDto moduleDto)
        {

            try
            {
                var module = _mapper.Map<Module>(moduleDto);
                module.CreatedTime = DateTime.Now;
                _repoModule.Add(module);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = module
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }


        #endregion

        #region Function
        public async Task<IEnumerable<HierarchyNode<FunctionSystem>>> GetFunctionsAsTreeViewV1()
        {
            var model = (await _repoFunctionSystem.FindAll().Include(x => x.Module).Include(x => x.Function).OrderBy(x => x.ID).OrderBy(x => x.ModuleID).ThenBy(x => x.Sequence).ToListAsync()).AsHierarchy(x => x.ID, y => y.ParentID);
            return model;
        }

        public async Task<IEnumerable<HierarchyNode<FunctionTreeDto>>> GetFunctionsAsTreeView()
        {
            var parents = (await _repoFunctionSystem.FindAll()
                .Include(x => x.Module)
                .OrderBy(x => x.ID)
                .OrderBy(x => x.ModuleID)
                .ThenBy(x => x.Sequence)
                .ToListAsync())
                .Select((x, i) => new HierarchyNode<FunctionTreeDto>
                {
                    Entity = new FunctionTreeDto
                    {
                        ID = x.ID,
                        Index = ++i,
                        Name = x.EN,
                        Code = x.Code,
                        Icon = x.Icon,
                        Url = x.Url,
                        Sequence = x.Sequence,
                        ModuleID = x.ModuleID,
                        ModuleName = x.Module.EN,
                        LanguageID = "",
                        Level = 1,
                        ParentID = null,
                    }
                })
                .ToList();

            return parents;
        }
        public async Task<OperationResult> UpdateFunction(FunctionDto functionDto)
        {

            try
            {
                //var function = _mapper.Map<FunctionSystem>(functionDto);
                var item = await _repoFunctionSystem.FindByIDAsync(functionDto.ID);
                item.TW =functionDto.TW;
                item.VN =functionDto.VN;
                item.EN = functionDto.EN;
                item.CN = functionDto.CN;
                item.Icon = functionDto.Icon;
                item.Url = functionDto.Url;
                item.Sequence = functionDto.Sequence;
                _repoFunctionSystem.Update(item);
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


        public async Task<OperationResult> DeleteFunction(int functionID)
        {

            try
            {
                var function = await _repoFunctionSystem.FindAll(x => x.ID == functionID)
              .FirstOrDefaultAsync();
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = function
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> AddFunction(FunctionDto functionDto)
        {
            try
            {
                var function = _mapper.Map<FunctionSystem>(functionDto);
                _repoFunctionSystem.Add(function);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = function
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public Task<OperationResult> GetAllFunctionByPermision()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Action
        public async Task<object> GetScreenAction(int functionID)
        {
            var query = from a in _repoAction.FindAll()
                        join f in _repoActionInFunctionSystem.FindAll(x => x.FunctionSystemID == functionID)
                                    .Include(x => x.FunctionSystem)
                            on a.ID equals f.ActionID
                        into af
                        from c in af.DefaultIfEmpty()
                        select new
                        {
                            Id = a.ID,
                            Name = a.EN,
                            FuncName = c != null ? c.FunctionSystem.EN : "N/A",
                            Status = c != null ? true : false,
                        };
            var data = await query.ToListAsync();
            return data;
        }
        public async Task<OperationResult> UpdateAction(Models.Action action)
        {
            try
            {
                var item = await _repoAction.FindByIDAsync(action.ID);
                item.TW = action.TW;
                item.VN = action.VN;
                item.EN = action.EN;
                item.CN = action.CN;
                item.Sequence = action.Sequence;
                _repoAction.Update(item);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = action
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> DeleteAction(int actionID)
        {
            var action = await _repoAction.FindAll(x => x.ID == actionID).FirstOrDefaultAsync();
            try
            {
            _repoAction.Remove(action);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = action
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> AddAction(Models.Action action)
        {
            try
            {
            _repoAction.Add(action);
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = action
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<List<Models.Action>> GetAllAction() => await _repoAction.FindAll().ToListAsync();
        #endregion

        #region ActionInFuntion

        public async Task<object> GetScreenFunctionAndAction(ScreenFunctionAndActionRequest request)
        {
            string lang = request.Lang;
            var roleID = request.RoleIDs;
            var permission = _repo.FindAll();
            var query = _repoActionInFunctionSystem.FindAll()
             .Include(x => x.Action)
             .Include(x => x.FunctionSystem)
             .ThenInclude(x => x.Module)
             .Select(x => new
             {
                 Id = x.FunctionSystem.ID,
                 FunctionCode = x.FunctionSystem.Code,
                 Name = lang == Languages.EN ? x.FunctionSystem.EN : lang == Languages.VI ? x.FunctionSystem.VN : lang == Languages.TW ? x.FunctionSystem.TW : lang == Languages.CN ? x.FunctionSystem.CN : x.FunctionSystem.EN,
                 ActionName = lang == Languages.EN ? x.Action.EN : lang == Languages.VI ? x.Action.VN : lang == Languages.TW ? x.Action.TW : lang == Languages.CN ? x.Action.CN : x.Action.EN,
                 ActionID = x.Action.ID,
                 SequenceFunction = x.FunctionSystem.Sequence,
                 Module = x.FunctionSystem.Module,
                 ModuleCode = x.FunctionSystem.Module.Code,
                 ModuleNameID = x.FunctionSystem.Module.ID,
                 Code = x.Action.Code,
             });
            // Dieu kien nay de khong load nhung chuc nang he thong
            var model = from t1 in query
                        from t2 in permission.Where(x => roleID.Contains(x.RoleID) && t1.Id == x.FunctionSystemID && x.ActionID == t1.ActionID)
                            .DefaultIfEmpty()
                        select new
                        {
                            t1.Id,
                            t1.Name,
                            t1.ActionName,
                            t1.ActionID,
                            t1.Code,
                            t1.Module,
                            t1.SequenceFunction,
                            Permission = t2
                        };
            var data = (await model.ToListAsync())
                        .GroupBy(x => x.Module)
                        .Select(x => new
                        {
                            ModuleName = lang == Languages.EN ? x.Key.EN : lang == Languages.VI ? x.Key.VN : lang == Languages.TW ? x.Key.TW : lang == Languages.CN ? x.Key.CN : x.Key.EN,
                            Sequence = x.Key.Sequence,
                            Fields = new
                            {
                                DataSource = x.GroupBy(s => new { s.Id, s.Name, s.SequenceFunction })
                                .Select(g => new
                                {
                                    Id = g.Key.Id,
                                    Name = g.Key.Name,
                                    SequenceFunction = g.Key.SequenceFunction,
                                    Childrens = g
                                    .Select(a => new
                                    {
                                        ParentID = g.Key.Id,
                                        ID = $"{a.ActionID}_{g.Key.Id}_{roleID.FirstOrDefault()}",
                                        Name = a.ActionName,
                                        a.ActionID,
                                        FunctionID = g.Key.Id,
                                        a.ActionName,
                                        Status = a.Permission != null,
                                            // Status = permission.Any(p => roleID.Contains(p.RoleID) && a.ActionID == p.ActionID && p.FunctionSystemID == g.Key.Id) 
                                            // IsChecked = permission.Any(p => roleID.Contains(p.RoleID) && a.ActionID == p.ActionID && p.FunctionSystemID == g.Key.Id)

                                        }).ToList()
                                }).OrderBy(x => x.SequenceFunction).ToList(),
                                Id = "id",
                                Text = "name",
                                Child = "childrens"
                            }
                        });
            return data.OrderBy(x => x.Sequence).ToList();
        }


        public async Task<object> GetActionInFunctionByRoleID(int roleID)
        {
            var account = await _repoAccount.FindAll(x => x.Id == roleID).FirstOrDefaultAsync();
            var query = _repo.FindAll(x => x.RoleID == account.AccountGroupId)
                .Include(x => x.Functions)
                .Include(x => x.Action)
                .Select(x => new
                {
                    FunctionCode = x.Functions.Code,
                    x.Functions.Url,
                    x.Action.Code,
                    x.ActionID
                });
            var data = (await query.ToListAsync()).GroupBy(x => new { x.FunctionCode, x.Url })
                    .Select(x => new
                    {
                        x.Key.FunctionCode,
                        x.Key.Url,
                        Childrens = x
                    });
            return data;
        }
        public async Task<OperationResult> PostActionToFunction(int functionID, ActionAssignRequest request)
        {
            foreach (var actionId in request.ActionIds)
            {
                if (await _repoActionInFunctionSystem.FindAll(x => x.FunctionSystemID == functionID && x.ActionID == actionId).AnyAsync() != false)
                    return new OperationResult { Success = false, Message = "This action has been existed in function" };
                var entity = new ActionInFunctionSystem
                {
                    ActionID = actionId,
                    FunctionSystemID = functionID
                };

                _repoActionInFunctionSystem.Add(entity);
            }

            try
            {
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = request
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;

            // tao role moi
        }
        public async Task<OperationResult> DeleteActionToFunction(int functionID, ActionAssignRequest request)
        {
            try
            {
                foreach (var actionId in request.ActionIds)
                {
                    var entity = await _repoActionInFunctionSystem.FindAll(x => x.FunctionSystemID == functionID && x.ActionID == actionId).FirstOrDefaultAsync();
                    if (entity == null)
                        return new OperationResult { Success = false, Message = "This action is not existed in function" };

                    _repoActionInFunctionSystem.Remove(entity);
                }
                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = request
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;

            // tao role moi
        }

        #endregion

        #region Permission

        public async Task<object> GetMenuByLangID(int userId, string lang)
        {
            var roles = await _repoAccountRole.FindAll(x => x.AccountId == userId).Select(x => x.AccountId).ToArrayAsync();

            var query = from p in _repo.FindAll()
                        join f in _repoFunctionSystem.FindAll()
                                .Include(x => x.Module)
                        on p.FunctionSystemID equals f.ID
                        join r in _repoRole.FindAll() on p.RoleID equals r.ID
                        join a in _repoAction.FindAll()
                            on p.ActionID equals a.ID
                        where roles.Contains(r.ID) && a.Code == "VIEW"
                        select new
                        {
                            Id = f.ID,
                            Name = lang == Languages.EN ? f.EN : lang == Languages.VI ? f.VN : lang == Languages.TW ? f.TW : lang == Languages.CN ? f.CN : f.EN,
                            Code = f.Code,
                            Url = f.Url,
                            Icon = f.Icon,
                            ParentId = f.ParentID,
                            SortOrder = f.Sequence,
                            Module = f.Module,
                            ModuleId = f.ModuleID
                        };
            var data = await query.Distinct()
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.SortOrder)
                .ToListAsync();
            return data.GroupBy(x => x.Module).Select(x => new
            {
                Module = lang == Languages.EN ? x.Key.EN : lang == Languages.VI ? x.Key.VN : lang == Languages.TW ? x.Key.TW : lang == Languages.CN ? x.Key.CN : x.Key.EN,
                Icon = x.Key.Icon,
                Url = x.Key.Url,
                Sequence = x.Key.Sequence,
                Children = x,
                HasChildren = x.Any()
            }).OrderBy(x => x.Sequence).ToList();
        }
        public async Task<OperationResult> PutPermissionByRoleId(int roleID, UpdatePermissionRequest request)
        {

            try
            {
                //create new permission list from user changed
                var newPermissions = new List<Permission>();
                foreach (var p in request.Permissions)
                {
                    newPermissions.Add(new Permission(roleID, p.ActionID, p.FunctionID));
                }
                var existingPermissions = await _repo.FindAll(x => x.RoleID == roleID).ToListAsync();

                _repo.RemoveMultiple(existingPermissions);
            

                _repo.AddRange(newPermissions.DistinctBy(x => new { x.RoleID, x.ActionID, x.FunctionSystemID }).ToList());

                var result = await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = request
                };
            }
            catch (System.Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;

            // tao role moi
        }


        public async Task<object> GetMenuByUserPermission1(int userId, string lang)
        {
            var account = await _repoAccount.FindAll(x => x.Id == userId).FirstOrDefaultAsync();
            var query = from f in _repoFunctionSystem.FindAll().Include(x => x.Module)
                        join p in _repo.FindAll()
                            on f.ID equals p.FunctionSystemID
                        join a in _repoAction.FindAll()
                            on p.ActionID equals a.ID
                        where account.AccountGroupId == p.RoleID && a.Code == "Read"
                        select new
                        {
                            Id = f.ID,
                            Name = lang == Languages.EN ? f.EN : lang == Languages.VI ? f.VN : lang == Languages.TW ? f.TW : lang == Languages.CN ? f.CN : f.EN,
                            Code = f.Code,
                            Url = f.Url,
                            Icon = f.Icon,
                            ParentId = f.ParentID,
                            Sequence = f.Sequence,
                            Module = f.Module,
                            ModuleId = f.ModuleID
                        };
            var data = await query.Distinct()
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.Sequence)
                .ToListAsync();
            return data.GroupBy(x => x.Module).Select(x => new
            {
                Module = lang == Languages.EN ? x.Key.EN : lang == Languages.VI ? x.Key.VN : lang == Languages.TW ? x.Key.TW : lang == Languages.CN ? x.Key.CN : x.Key.EN,
                Icon = x.Key.Icon,
                Url = x.Key.Url,
                Sequence = x.Key.Sequence,
                Children = x,
                HasChildren = x.Any()
            }).OrderBy(x => x.Sequence).ToList();
        }

        public async Task<object> GetMenuByUserPermission(int accountGroupID, string lang)
        {
           // var account = await _repoAccount.FindAll(x => x.Id == userId).AsNoTracking().FirstOrDefaultAsync();
            var query = from f in _repoFunctionSystem.FindAll().AsNoTracking()
                        join m in _repoModule.FindAll().AsNoTracking() on f.ModuleID equals m.ID into g
                        from mf in g.DefaultIfEmpty()
                        join p in _repo.FindAll(p=>accountGroupID == p.RoleID).AsNoTracking()
                            on f.ID equals p.FunctionSystemID
                        join a in _repoAction.FindAll(a => a.Code == "Read").AsNoTracking()
                            on p.ActionID equals a.ID
                        where mf != null
                        select new
                        {
                            Id = f.ID,
                            Name = lang == Languages.EN ? f.EN : lang == Languages.VI ? f.VN : lang == Languages.TW ? f.TW : lang == Languages.CN ? f.CN : f.EN,
                            Code = f.Code,
                            Url = f.Url,
                            Icon = f.Icon,
                            ParentId = f.ParentID,
                            Sequence = f.Sequence,
                            ModuleId = mf.ID,
                            ModuleSequence = mf.Sequence,
                            ModuleIcon = mf.Icon,
                            ModuleUrl = mf.Url,
                            ModuleName = lang == Languages.EN ? mf.EN : lang == Languages.VI ? mf.VN : lang == Languages.TW ? mf.TW : lang == Languages.CN ? mf.CN : mf.EN,

                        };
            var data = await query.Distinct().ToListAsync();
            var data1 = data.GroupBy(x => new {
                x.ModuleName,
                x.ModuleSequence,
                x.ModuleIcon,
                x.ModuleUrl
            }).Select(x => new MenuDto
            {
                Module = x.Key.ModuleName,
                Icon = x.Key.ModuleIcon,
                Url = x.Key.ModuleUrl,
                Sequence = x.Key.ModuleSequence,
                Children = x,
                HasChildren = x.Any()
            }).ToList();
           
            var result = data1.OrderBy(x => x.Sequence).ToList();
            return result;
        }
        public async Task<object> LoadData(DataManager data)
        {
            IQueryable<FunctionDto> datasource = _repoFunctionSystem.FindAll().ProjectTo<FunctionDto>(_configMapper);
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

        public async Task<object> LoadActionData(DataManager data)
        {
            IQueryable<Models.Action> datasource = _repoAction.FindAll().OrderBy(x=>x.Sequence);
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
        public async Task<object> LoadModuleData(DataManager data)
        {
            IQueryable<ModuleDto> datasource = _repoModule.FindAll().ProjectTo<ModuleDto>(_configMapper).OrderBy(x => x.Sequence); ;
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

        public async Task<List<FunctionSystem>> GetAllFunction()
        {
            var functions = await _repoFunctionSystem.FindAll().Include(x => x.Module).ToListAsync();
            return functions;
        }

        public async Task<List<Module>> GetAllModule()
        => await _repoModule.FindAll().OrderBy(x => x.Sequence).ToListAsync();
        #endregion
    }
}
