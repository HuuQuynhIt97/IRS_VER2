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
    public interface IBookingService : IServiceBase<BookingDetail, BookingDetailDto>
    {
        Task<object> LoadData(DataManager data);
        Task<object> GetAudit(object id);
        Task<object> GetBookingCheck();
        Task<object> GetByHall(string hallID);
        Task<object> GetPreviewScheduler(string siteID);
        Task<object> Search(BookingFilterDto filter);
        Task<object> SearchDetailRoom(BookingFilterDto filter);
        Task<object> SearchBookingCheck(BookingCheckFilterDto filter);
        Task<object> GetSearchBookingCalendar(BookingCheckFilterDto filter);
        Task<object> GetSearchByList(BookingCheckFilterDto filter);

        Task<object> GetPreviewScheduler(DataManager data, string siteGuid);

    }
    public class BookingService : ServiceBase<BookingDetail, BookingDetailDto>, IBookingService
    {
        private readonly IRepositoryBase<BookingDetail> _repo;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IRepositoryBase<Hall> _repoHall;
        private readonly IRepositoryBase<HallTime> _repoHallTime;
        private readonly IRepositoryBase<Room> _repoRoom;
        private readonly IRepositoryBase<Site> _repoSite;
        private readonly IRepositoryBase<WorkOrder> _repoWorkOrder;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public BookingService(
            IRepositoryBase<BookingDetail> repo,
            IRepositoryBase<XAccount> repoXAccount,
            IRepositoryBase<Hall> repoHall,
            IRepositoryBase<HallTime> repoHallTime,
            IRepositoryBase<Room> repoRoom,
            IRepositoryBase<Site> repoSite,
            IRepositoryBase<WorkOrder> repoWorkOrder,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoXAccount = repoXAccount;
            _repoHall = repoHall;
            _repoHallTime = repoHallTime;
            _repoSite = repoSite;
            _repoWorkOrder = repoWorkOrder;
            _repoRoom = repoRoom;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public override async Task<OperationResult> AddAsync(BookingDetailDto model)
        {
            try
            {
                var list_time_booking = _repo.FindAll(x => 
                x.HallGuid == model.HallGuid
                && x.RoomGuid == model.RoomGuid
                && x.SiteGuid == model.SiteGuid
                && x.HallType == model.HallType
                && x.RoomType == model.RoomType
                ).ToList();
                var check_exist_time = false;

                foreach (var itemOfListTime in list_time_booking)
                {
                    DateTime startDate = new DateTime(itemOfListTime.StartDate.Value.Year, itemOfListTime.StartDate.Value.Month, itemOfListTime.StartDate.Value.Day,
                        itemOfListTime.StartDate.Value.Hour, itemOfListTime.StartDate.Value.Minute, 0);
                    DateTime endDate = new DateTime(itemOfListTime.EndDate.Value.Year, itemOfListTime.EndDate.Value.Month, itemOfListTime.EndDate.Value.Day,
                        itemOfListTime.EndDate.Value.Hour, itemOfListTime.EndDate.Value.Minute, 0);
                    bool time_start_exist = model.StartDate < endDate && model.EndDate > startDate;
                    bool time_end_exist = model.EndDate <= endDate && model.EndDate > startDate;
                    if (time_start_exist || time_end_exist)
                    {
                        check_exist_time = true;
                        break;
                    }
                }

                if (check_exist_time == false)
                {
                    var item = _mapper.Map<BookingDetail>(model);
                    item.Status = 1;
                    item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
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
                else
                {
                    operationResult = new OperationResult
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = MessageReponse.Exist,
                        Success = true
                    };
                }
                
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public override async Task<List<BookingDetailDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<BookingDetailDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }
        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            //item.CancelFlag = "Y";
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

        public async Task<object> LoadData(DataManager data)
        {
            var datasource = _repo.FindAll(x => x.Status == 1)
            .OrderByDescending(x=> x.Id)
            .Select(x => new {
                x.Id,
                x.Guid,
                x.HallGuid,
            });
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

        public async Task<object> GetBookingCheck()
        {
            var booking = await _repo.FindAll(x => x.Status == 1).ToListAsync();
            var hall = await _repoHall.FindAll().ToListAsync();
            var site = await _repoSite.FindAll().ToListAsync();
            var room = await _repoRoom.FindAll().ToListAsync();
            var workOrder = await _repoWorkOrder.FindAll().ToListAsync();
            var funeral_director = await _repoXAccount.FindAll().ToListAsync();

            var result = from b in booking
                         join s in site on b.SiteGuid equals s.Guid
                         join h in hall on b.HallGuid equals h.Guid
                         join r in room on b.RoomGuid equals r.Guid
                         join w in workOrder on b.WorkOrderGuid equals w.Guid
                         join f in funeral_director on b.FuneralDirector equals f.Guid
                         select new
                         {
                             b.Id,
                             s.SiteName,
                             h.HallName,
                             r.RoomName,
                             b.BookingTimeS,
                             b.BookingTimeE,
                             w.WorkOrderName,
                             f.AccountName,
                             OrderDate = b.OrderDate.Value.ToString("yyyy-MM-dd"),
                             b.OrderName,
                             b.OrderNo
                         };

            return result;
        }

        public async Task<object> GetAudit(object id)
        {
            var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x => new { x.UpdateBy, x.CreateBy, x.UpdateDate, x.CreateDate }).FirstOrDefaultAsync();
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
            if (data.UpdateBy.HasValue)
            {
                var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.UpdateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                updateBy = updateBy != null ? updateAudit.Uid : "N/A";
                updateDate = data.UpdateDate.HasValue ? data.UpdateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            }
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

        public async Task<object> GetByHall(string hallID)
        {
            var query = _repo.FindAll(x => x.Status == 1 && x.HallGuid == hallID).Select(x => new {
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;
        }

        public async Task<object> GetPreviewScheduler(string siteID)
        {
            var booking = await _repo.FindAll(x => x.SiteGuid == siteID && x.Status == 1).ToListAsync();
            var data = from x in booking
                       let hall = _repoHall.FindAll(o => o.Guid == x.HallGuid).FirstOrDefault() != null ?
                       _repoHall.FindAll(o => o.Guid == x.HallGuid).FirstOrDefault().HallName : ""
                       let room = _repoRoom.FindAll(o => o.Guid == x.RoomGuid).FirstOrDefault() != null ?
                       _repoRoom.FindAll(o => o.Guid == x.RoomGuid).FirstOrDefault().RoomName : ""
                       select new
                       {
                           Id = x.Id,
                           Subject = x.OrderName,
                           StartTime = x.StartDate,
                           CategoryColor = "#1aaa55",
                           OrderNo = x.OrderNo,
                           HallName = hall,
                           RoomName = room,
                           OrderName = x.OrderName,
                           EndTime = x.EndDate
                       };
            return data;
        }

        public async Task<object> GetPreviewScheduler(DataManager data, string siteGuid)
        {
            var datasource = _repo.FindAll(x => x.SiteGuid == siteGuid).Select(x => new
            {
                Id = x.Id,
                Subject = x.OrderName,
                StartTime = x.StartDate,
                CategoryColor = "#1aaa55",
                EndTime = x.EndDate
            });
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
            throw new NotImplementedException();
        }

        public async Task<object> Search(BookingFilterDto filter)
        {
            var booking = new List<BookingDetail>();
            //nếu chọn hall , ko chọn room and funeral
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter))
            {
                booking = await _repo.FindAll(x => 
                x.SiteGuid == filter.SiteGuid 
                && x.HallGuid == filter.HallGuidFiter
                && x.Status == 1).ToListAsync();
            }
            //nếu chọn hall , ko chọn room và chọn funeral
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter) && !string.IsNullOrEmpty(filter.FuneralDirectorFilter))
            {
                booking = await _repo.FindAll(x =>
                x.SiteGuid == filter.SiteGuid
                && x.HallGuid == filter.HallGuidFiter
                && x.FuneralDirector == filter.FuneralDirectorFilter
                && x.Status == 1).ToListAsync();
            }
            //nếu chọn hall - room
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && !string.IsNullOrEmpty(filter.RoomGuidFiter))
            {
                booking = await _repo.FindAll(x =>
                x.SiteGuid == filter.SiteGuid
                && x.HallGuid == filter.HallGuidFiter
                && x.RoomGuid == filter.RoomGuidFiter
                && x.Status == 1).ToListAsync();
            }

            // nếu chọn hall-room-funeral
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && !string.IsNullOrEmpty(filter.RoomGuidFiter) && !string.IsNullOrEmpty(filter.FuneralDirectorFilter))
            {
                booking = await _repo.FindAll(x =>
                x.SiteGuid == filter.SiteGuid
                && x.HallGuid == filter.HallGuidFiter
                && x.RoomGuid == filter.RoomGuidFiter
                && x.FuneralDirector == filter.FuneralDirectorFilter
                && x.Status == 1).ToListAsync();
            }
            //nếu chỉ chọn funeral
            if (string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter) && !string.IsNullOrEmpty(filter.FuneralDirectorFilter))
            {
                booking = await _repo.FindAll(x =>
                x.SiteGuid == filter.SiteGuid
                && x.FuneralDirector == filter.FuneralDirectorFilter
                && x.Status == 1).ToListAsync();
            }

            var data = from x in booking
                       let hall = _repoHall.FindAll(o => o.Guid == x.HallGuid).FirstOrDefault() != null ?
                       _repoHall.FindAll(o => o.Guid == x.HallGuid).FirstOrDefault().HallName : ""
                       let room = _repoRoom.FindAll(o => o.Guid == x.RoomGuid).FirstOrDefault() != null ?
                       _repoRoom.FindAll(o => o.Guid == x.RoomGuid).FirstOrDefault().RoomName : ""
                       select new
                       {
                           Id = x.Id,
                           Subject = x.OrderName,
                           StartTime = x.StartDate,
                           CategoryColor = "#1aaa55",
                           OrderNo = x.OrderNo,
                           HallName = hall,
                           RoomName = room,
                           OrderName = x.OrderName,
                           EndTime = x.EndDate
                       };
            return data;
        }

        public async Task<object> SearchBookingCheck(BookingCheckFilterDto filter)
        {
            var booking = new List<BookingDetail>();

            //nếu chọn hall , ko chọn room and funeral and
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter))
            {
                if (filter.bookingDate != null)
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.HallGuid == filter.HallGuidFiter
                    && x.OrderDate == filter.bookingDate
                    && x.Status == 1).ToListAsync();
                }else
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.HallGuid == filter.HallGuidFiter
                    && x.Status == 1).ToListAsync();
                }
            }
            //nếu chọn hall , ko chọn room và chọn funeral
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter) && !string.IsNullOrEmpty(filter.FuneralDirectorFilter))
            {
                if (filter.bookingDate != null)
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.HallGuid == filter.HallGuidFiter
                    && x.OrderDate == filter.bookingDate
                    && x.FuneralDirector == filter.FuneralDirectorFilter
                    && x.Status == 1).ToListAsync();
                }
                else
                {
                    booking = await _repo.FindAll(x =>
                     x.SiteGuid == filter.SiteGuid
                     && x.HallGuid == filter.HallGuidFiter
                     && x.FuneralDirector == filter.FuneralDirectorFilter
                     && x.Status == 1).ToListAsync();
                }
                
            }
            //nếu chọn hall - room
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && !string.IsNullOrEmpty(filter.RoomGuidFiter))
            {
                if (filter.bookingDate != null)
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.OrderDate == filter.bookingDate
                    && x.HallGuid == filter.HallGuidFiter
                    && x.RoomGuid == filter.RoomGuidFiter
                    && x.Status == 1).ToListAsync();
                }
                else
                {
                    booking = await _repo.FindAll(x =>
                     x.SiteGuid == filter.SiteGuid
                     && x.HallGuid == filter.HallGuidFiter
                     && x.RoomGuid == filter.RoomGuidFiter
                     && x.Status == 1).ToListAsync();
                }
                
            }

            // nếu chọn hall-room-funeral
            if (!string.IsNullOrEmpty(filter.HallGuidFiter) && !string.IsNullOrEmpty(filter.RoomGuidFiter) && !string.IsNullOrEmpty(filter.FuneralDirectorFilter))
            {
                if (filter.bookingDate != null)
                {
                    booking = await _repo.FindAll(x =>
                   x.SiteGuid == filter.SiteGuid
                   && x.HallGuid == filter.HallGuidFiter
                   && x.OrderDate == filter.bookingDate
                   && x.RoomGuid == filter.RoomGuidFiter
                   && x.FuneralDirector == filter.FuneralDirectorFilter
                   && x.Status == 1).ToListAsync();
                }
                else
                {
                    booking = await _repo.FindAll(x =>
                   x.SiteGuid == filter.SiteGuid
                   && x.HallGuid == filter.HallGuidFiter
                   && x.RoomGuid == filter.RoomGuidFiter
                   && x.FuneralDirector == filter.FuneralDirectorFilter
                   && x.Status == 1).ToListAsync();
                }
               
            }
            //nếu chỉ chọn funeral
            if (string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter) && !string.IsNullOrEmpty(filter.FuneralDirectorFilter))
            {
                if (filter.bookingDate != null)
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.OrderDate == filter.bookingDate
                    && x.FuneralDirector == filter.FuneralDirectorFilter
                    && x.Status == 1).ToListAsync();
                }
                else
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.FuneralDirector == filter.FuneralDirectorFilter
                    && x.Status == 1).ToListAsync();
                }
                
            }

            //nếu chỉ chọn ngay
            if (string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter) && string.IsNullOrEmpty(filter.FuneralDirectorFilter))
            {
                if (filter.bookingDate != null)
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.OrderDate == filter.bookingDate
                    && x.Status == 1).ToListAsync();
                }
                else
                {
                    booking = await _repo.FindAll(x =>
                    x.SiteGuid == filter.SiteGuid
                    && x.Status == 1).ToListAsync();
                }

            }
            var hall = await _repoHall.FindAll().ToListAsync();
            var site = await _repoSite.FindAll().ToListAsync();
            var room = await _repoRoom.FindAll().ToListAsync();
            var workOrder = await _repoWorkOrder.FindAll().ToListAsync();
            var funeral_director = await _repoXAccount.FindAll().ToListAsync();

            var result = from b in booking
                         join s in site on b.SiteGuid equals s.Guid
                         join h in hall on b.HallGuid equals h.Guid
                         join r in room on b.RoomGuid equals r.Guid
                         join w in workOrder on b.WorkOrderGuid equals w.Guid
                         join f in funeral_director on b.FuneralDirector equals f.Guid
                         select new
                         {
                             b.Id,
                             b.BookingTimeS,
                             b.BookingTimeE,
                             s.SiteName,
                             h.HallName,
                             r.RoomName,
                             w.WorkOrderName,
                             f.AccountName,
                             OrderDate = b.OrderDate.Value.ToString("yyyy-MM-dd"),
                             b.OrderName,
                             b.OrderNo
                         };

            return result;
        }

        public async Task<object> SearchDetailRoom(BookingFilterDto filter)
        {
            var result = new object();
            if (string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.RoomGuidFiter) || string.IsNullOrEmpty(filter.RoomGuidFiter))
            {
                return null;
            }
            var x = _repoRoom.FindAll(x => x.HallGuid == filter.HallGuidFiter && x.Guid == filter.RoomGuidFiter).FirstOrDefault();
            var y = _repoHall.FindAll(x => x.Guid == filter.HallGuidFiter).FirstOrDefault();

            result = new
            {
                x.Id,
                x.Type,
                y.HallName,
                y.HallNo,
                x.RoomNo,
                x.RoomName,
                x.RoomSize,
                x.RoomDescript,
                x.Price,
                x.PriceMember,
                x.PriceSpecial,
                x.RoomLocation,
                x.RoomPhoto,
                x.RoomPhoto1,
                x.RoomPhoto2,
                x.RoomPhoto3,
                x.RoomPhoto4,
                x.RoomPhoto5,
            };
            
            return result;
        }

        public async Task<object> GetSearchBookingCalendar(BookingCheckFilterDto filter)
        {
            var list_room = new List<Room>();
            var list_hall_time = new List<HallTime>();
            if (string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.SiteGuidFilter) || string.IsNullOrEmpty(filter.HallGuidFiter))
            {
                list_room = await _repoRoom.FindAll().ToListAsync();
                list_hall_time = await _repoHallTime.FindAll().ToListAsync();
            }else
            {
                list_room = await _repoRoom.FindAll(x => x.SiteGuid == filter.SiteGuidFilter && x.HallGuid == filter.HallGuidFiter).ToListAsync();
                list_hall_time = await _repoHallTime.FindAll(x => x.SiteGuid == filter.SiteGuidFilter && x.HallGuid == filter.HallGuidFiter).ToListAsync();
            }
            
            var room = new List<RoomSearchCalendarDto>();

            foreach (var item_hall_time in list_hall_time)
            {
                foreach (var item_room in list_room)
                {
                    var list_time_booking = new List<BookingDetail>();
                    if (string.IsNullOrEmpty(filter.HallGuidFiter) && string.IsNullOrEmpty(filter.SiteGuidFilter) || string.IsNullOrEmpty(filter.HallGuidFiter))
                    {
                        list_time_booking = _repo.FindAll(x =>
                        x.RoomGuid == item_room.Guid
                        && x.Status == 1
                        ).ToList();
                    }
                    else
                    {
                        list_time_booking = _repo.FindAll(x =>
                        x.HallGuid == filter.HallGuidFiter
                        && x.RoomGuid == item_room.Guid
                        && x.SiteGuid == filter.SiteGuidFilter
                        && x.Status == 1
                        ).ToList();
                    }
                    var isBooking = false;
                    decimal bookingId = 0;
                    foreach (var itemOfListTime in list_time_booking)
                    {
                        DateTime startDate = new DateTime(itemOfListTime.StartDate.Value.Year, itemOfListTime.StartDate.Value.Month, itemOfListTime.StartDate.Value.Day,
                            itemOfListTime.StartDate.Value.Hour, itemOfListTime.StartDate.Value.Minute, 0);
                        DateTime endDate = new DateTime(itemOfListTime.EndDate.Value.Year, itemOfListTime.EndDate.Value.Month, itemOfListTime.EndDate.Value.Day,
                            itemOfListTime.EndDate.Value.Hour, itemOfListTime.EndDate.Value.Minute, 0);

                        DateTime time_hall_start = new DateTime(filter.bookingDate.Value.Year, filter.bookingDate.Value.Month, filter.bookingDate.Value.Day,
                            TimeSpan.Parse(item_hall_time.Start).Hours, TimeSpan.Parse(item_hall_time.Start).Minutes, 0);

                        DateTime time_hall_end = new DateTime(filter.bookingDate.Value.Year, filter.bookingDate.Value.Month, filter.bookingDate.Value.Day,
                            TimeSpan.Parse(item_hall_time.End).Hours, TimeSpan.Parse(item_hall_time.End).Minutes, 0);

                        bool time_start_exist = time_hall_start < endDate && time_hall_start > startDate;
                        bool time_end_exist = time_hall_end <= endDate && time_hall_end > startDate;
                        if (time_start_exist || time_end_exist)
                        {
                            isBooking = true;
                            bookingId = itemOfListTime.Id;
                            break;
                        }
                    }
                    var item_add = new RoomSearchCalendarDto {
                        RoomName = item_room.RoomName,
                        RoomGuid = item_room.Guid,
                        HallGuid = item_room.HallGuid,
                        RoomID = item_room.Id,
                        BookingID = bookingId,
                        RoomNo = item_room.RoomNo,
                        IsBooking = isBooking,
                        RoomDescript = item_room.RoomDescript,
                        HallTime = item_hall_time.Start
                    };


                    room.Add(item_add);
                }
            }


            return room;
        }

        public async Task<object> GetSearchByList(BookingCheckFilterDto filter)
        {

            var list_room = new List<Room>();
            var list_hall_time = new List<HallTime>();
            if (string.IsNullOrEmpty(filter.HallGuidFiter))
            {
                list_room = await _repoRoom.FindAll().ToListAsync();
                list_hall_time = await _repoHallTime.FindAll().ToListAsync();
            }
            else
            {
                list_room = await _repoRoom.FindAll(x => x.SiteGuid == filter.SiteGuidFilter && x.HallGuid == filter.HallGuidFiter).ToListAsync();
                list_hall_time = await _repoHallTime.FindAll(x => x.SiteGuid == filter.SiteGuidFilter && x.HallGuid == filter.HallGuidFiter).ToListAsync();
            }
            
            var room = new List<RoomSearchByList>();

            foreach (var item_hall_time in list_hall_time)
            {
                foreach (var item_room in list_room)
                {
                    var list_time_booking = new List<BookingDetail>();
                    if (string.IsNullOrEmpty(filter.HallGuidFiter))
                    {
                        list_time_booking = _repo.FindAll(x =>
                        x.RoomGuid == item_room.Guid
                        && x.Status == 1
                        ).ToList();
                    }
                    else
                    {
                        list_time_booking = _repo.FindAll(x =>
                        x.HallGuid == filter.HallGuidFiter
                        && x.RoomGuid == item_room.Guid
                        && x.SiteGuid == filter.SiteGuidFilter
                        && x.Status == 1
                        ).ToList();
                    }
                    var isBooking = false;
                    decimal bookingId = 0;
                    foreach (var itemOfListTime in list_time_booking)
                    {
                        DateTime startDate = new DateTime(itemOfListTime.StartDate.Value.Year, itemOfListTime.StartDate.Value.Month, itemOfListTime.StartDate.Value.Day,
                            itemOfListTime.StartDate.Value.Hour, itemOfListTime.StartDate.Value.Minute, 0);
                        DateTime endDate = new DateTime(itemOfListTime.EndDate.Value.Year, itemOfListTime.EndDate.Value.Month, itemOfListTime.EndDate.Value.Day,
                            itemOfListTime.EndDate.Value.Hour, itemOfListTime.EndDate.Value.Minute, 0);

                        DateTime time_hall_start = new DateTime(filter.bookingDate.Value.Year, filter.bookingDate.Value.Month, filter.bookingDate.Value.Day,
                            TimeSpan.Parse(item_hall_time.Start).Hours, TimeSpan.Parse(item_hall_time.Start).Minutes, 0);

                        DateTime time_hall_end = new DateTime(filter.bookingDate.Value.Year, filter.bookingDate.Value.Month, filter.bookingDate.Value.Day,
                            TimeSpan.Parse(item_hall_time.End).Hours, TimeSpan.Parse(item_hall_time.End).Minutes, 0);

                        bool time_start_exist = time_hall_start < endDate && time_hall_start > startDate;
                        bool time_end_exist = time_hall_end <= endDate && time_hall_end > startDate;
                        if (time_start_exist || time_end_exist)
                        {
                            isBooking = true;
                            bookingId = itemOfListTime.Id;
                            break;
                        }
                    }
                    var hall = _repoHall.FindAll(x => x.Guid == filter.HallGuidFiter).FirstOrDefault();
                    var item_add = new RoomSearchByList
                    {
                        RoomName = item_room.RoomName,
                        HallName = hall != null ?  hall.HallName : "N/A",
                        HallStart = item_hall_time.Start,
                        HallEnd = item_hall_time.End,
                        Time = filter.bookingDate.Value.ToString("yyyy-MM-dd"),
                        RoomGuid = item_room.Guid,
                        HallGuid = item_room.HallGuid,
                        RoomID = item_room.Id,
                        BookingID = bookingId,
                        RoomNo = item_room.RoomNo,
                        IsBooking = isBooking,
                        RoomDescript = item_room.RoomDescript,
                        HallTime = item_hall_time.Start
                    };


                    room.Add(item_add);
                }
            }


            return room;
        }
    }
}
