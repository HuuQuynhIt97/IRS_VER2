using IRS.Models;
using IRS.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DTO
{
   
    public class ToDoListDto 
    {
        public int ID { get; set; }
        public string Action { get; set; }
        public string Remark { get; set; }
        public int? ProgressID { get; set; }
        public int ObjectiveID { get; set; }
        public int? ParentID { get; set; }
        public int Level { get; set; }
        public string AccountGroupType{ get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsReject { get; set; }
        public bool IsRelease { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }

    }

    public class ToDoListByLevelL1L2Dto
    {

        public int ID { get; set; }
        public string Objective { get; set; }
        public string L0TargetList { get; set; }
        public List<string> L0ActionList { get; set; }
        public string Result1OfMonth { get; set; }
        public string Result2OfMonth { get; set; }
        public string Result3OfMonth { get; set; }
        public object Months { get; set; }
    }
    public class SelfScoreDto
    {

        public List<string> ObjectiveList { get; set; }
        public string ResultOfMonth { get; set; }
        public int Month { get; set; }
    }
    public class ImportExcelFHO
    {

        public string KPIObjective { get; set; }
        public  string UserList { get; set; }
    }
    public class UpdaterDto
    {
        public UpdaterDto(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
    public class L0Dto
    {
        public int ID { get; set; }
        public int TodolistID { get; set; }
        public string Topic { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public int Period { get; set; }
        public int HalfYearID { get; set; }
        public int QuarterPeriodTypeID { get; set; }
        public int Quarter { get; set; }
        public int PeriodTypeID { get; set; }
        public List<int> Settings { get; set; }
        public bool IsDisplayUploadResult { get; set; }
        public bool IsDisplaySelfScore { get; set; }
        public bool IsDisplayAction { get; set; }
        public bool IsReject { get; set; }
        public bool IsRelease { get; set; }
        public bool HasFunctionalLeader { get; set; }
        public bool IsUpdatedResult { get; set; }
        public bool IsSelfScore { get; set; }
        public bool GHRScored { get; set; }
        public bool FunctionalLeaderScored { get; set; }

    }
    public class FunctionalLeaderDto
    {
        public int ID { get; set; }
        public string Objective { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public int Period { get; set; }
        public int PeriodTypeID { get; set; }
        public List<int> Settings { get; set; }
        public bool IsDisplayKPIScore { get; set; }
        public bool IsDisplayAttitude { get; set; }
        public bool FunctionalLeaderScored { get; set; }

    }
    public class L1Dto
    {
        public int ID { get; set; }
        public string Objective { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public int Period { get; set; }
        public int HalfYearPeriod { get; set; }
        public int PeriodTypeID { get; set; }
        public int HalfYearID { get; set; }
        public List<int> Settings { get; set; }
        public bool IsSelfScore { get; set; }
        public bool WasUpdatedResultOfMonth { get; set; }
        public bool IsDisplayKPIScore { get; set; }
        public bool IsDisplayAttitude { get; set; }
        public bool FunctionalLeaderScored { get; set; }
        public bool L1Scored { get; set; }
        public bool HasFunctionalLeader { get; set; }
    }
    public class L2Dto
    {
        public int ID { get; set; }
        public string Objective { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public int Period { get; set; }
        public int HalfYearPeriod { get; set; }
        public int PeriodTypeID { get; set; }
        public int HalfYearID { get; set; }
        public List<int> Settings { get; set; }
        public bool IsDisplayKPIScore { get; set; }
        public bool IsDisplayAttitude { get; set; }
        public bool HasFunctionalLeader { get; set; }

        public bool L1Scored { get; set; }
        public bool L2Scored { get; set; }
    }
    public class GHRDto
    {
        public int ID { get; set; }
        public string Objective { get; set; }
        public DateTime DueDate { get; set; }
        public string Type { get; set; }
        public int Period { get; set; }
        public int PeriodTypeID { get; set; }
        public List<int> Settings { get; set; }
        public bool IsDisplayKPIScore { get; set; }
        public bool IsShow { get; set; }
        public object CreatedTodolist { get; set; }
        public object Todolist { get; set; }
        public bool IsDisplayAttitude { get; set; }
    }
    public class Q1OrQ3Request
    {
        public int Period { get; set; }
        public int PeriodTypeID { get; set; }
        public int AccountID { get; set; }
    }

    public class Q1OrQ3Export
    {
        public string FullName { get; set; }
        public double L1Score { get; set; }
        public string L1Comment { get; set; }
        public double L2Score { get; set; }
        public string L2Comment { get; set; }
        public double GHRSmartScore { get; set; }
    }
}
