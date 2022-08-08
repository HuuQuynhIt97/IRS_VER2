using System;
using System.Collections.Generic;

namespace IRS.DTO
{
    public partial class DashboardDto
    {
        public decimal Id { get; set; }
        public string FarmGuid { get; set; }
        public decimal? DashBoardNo { get; set; }
        public string DashBoardName { get; set; }
        public string UpperDashBoard { get; set; }
        public decimal? AreaNo { get; set; }
        public string AreaName { get; set; }
        public string UpperArea { get; set; }
        public decimal? ItemNo { get; set; }
        public string ItemName { get; set; }
        public string BackGroundColor { get; set; }
        public string Type { get; set; }
        public decimal? SortId { get; set; }
        public string Text01 { get; set; }
        public string Text02 { get; set; }
        public string Text03 { get; set; }
        public string Text04 { get; set; }
        public string Text05 { get; set; }
        public string Text06 { get; set; }
        public string Text07 { get; set; }
        public string Text08 { get; set; }
        public string Text09 { get; set; }
        public string Text10 { get; set; }
        public string TextColor01 { get; set; }
        public string TextColor02 { get; set; }
        public string TextColor03 { get; set; }
        public string TextColor04 { get; set; }
        public string TextColor05 { get; set; }
        public string TextColor06 { get; set; }
        public string TextColor07 { get; set; }
        public string TextColor08 { get; set; }
        public string TextColor09 { get; set; }
        public string TextColor10 { get; set; }
        public string Value01 { get; set; }
        public string Value02 { get; set; }
        public string Value03 { get; set; }
        public string Value04 { get; set; }
        public string Value05 { get; set; }
        public string Value06 { get; set; }
        public string Value07 { get; set; }
        public string Value08 { get; set; }
        public string Value09 { get; set; }
        public string Value10 { get; set; }
        public string ValueColor01 { get; set; }
        public string ValueColor02 { get; set; }
        public string ValueColor03 { get; set; }
        public string ValueColor04 { get; set; }
        public string ValueColor05 { get; set; }
        public string ValueColor06 { get; set; }
        public string ValueColor07 { get; set; }
        public string ValueColor08 { get; set; }
        public string ValueColor09 { get; set; }
        public string ValueColor10 { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CreateBy { get; set; }
        public decimal? Status { get; set; }
        public string Guid { get; set; }
        public string ChartTitle { get; set; }
        public string LangID { get; set; }

    }
    public partial class DashboardData {
        public string DashboardName { get; set; }
        public string Guid { get; set; }
    }

    public partial class DashboardArea {

        public decimal? Sort { get; set; }
        public string Guid { get; set; }
        public string AreaName { get; set; }
        public decimal? AreaNo { get; set; }
        public string DashboardGuid { get; set; }
        public bool HasChart { get; set; }
        public bool HasTable { get; set; }
        public bool HasNormalA { get; set; }
        public bool HasNormalB { get; set; }
        public bool HasNormalC { get; set; }
        public bool HasNormalD { get; set; }
        public List<TableItem> TableItems { get; set; }
        public List<ChartItemData> ChartItems { get; set; }
        public List<NormalItem> NormalItemsA { get; set; }
        public List<NormalItem> NormalItemsB { get; set; }
        public List<NormalItem> NormalItemsC { get; set; }
        public List<NormalItem> NormalItemsD { get; set; }
    }

     public partial class TableItem {
        public string Guid { get; set; }
        public string UpperArea { get; set; }
        public string UpperDashBoard { get; set; }
        public string Text01 { get; set; }
        public string TextColor01 { get; set; }
        public string Value01 { get; set; }
      
        public string Value02 { get; set; }
        public string Value03 { get; set; }
        public string Value04 { get; set; }
        public string Value05 { get; set; }
        public string Value06 { get; set; }
        public string Value07 { get; set; }
        public string Value08 { get; set; }
        public string Value09 { get; set; }
        public string Value10 { get; set; }
        public string ValueColor01 { get; set; }
        public string ValueColor02 { get; set; }
        public string ValueColor03 { get; set; }
        public string ValueColor04 { get; set; }
        public string ValueColor05 { get; set; }
        public string ValueColor06 { get; set; }
        public string ValueColor07 { get; set; }
        public string ValueColor08 { get; set; }
        public string ValueColor09 { get; set; }
        public string ValueColor10 { get; set; }
        public DateTime? CreateDate { get; set; }

    }
    public partial class ChartItemData
    {
        public string Title { get; set; }
        public List<ChartItemDataSource> Data { get; set; }
    }
    public partial class ChartItemDataSource
    {
        public string Fill { get; set; }
        public object DataSource { get; set; }
    }
    public partial class ChartItem {
        public string Guid { get; set; }
        public string UpperArea { get; set; }
        public string UpperDashBoard { get; set; }
        public string Text01 { get; set; }
        public string Text02 { get; set; }
        public string Value01 { get; set; }
        public string ChartTitle { get; set; }

        public string Value02 { get; set; }
     
        public string ValueColor01 { get; set; }
        public string ValueColor02 { get; set; }
        public DateTime? CreateDate { get; set; }

    }
    public partial class NormalItem {
        public string Guid { get; set; }
        public string Type { get; set; }
        public string UpperArea { get; set; }
        public string UpperDashBoard { get; set; }
        public string Text01 { get; set; }
        public string TextColor01 { get; set; }
        public string Value01 { get; set; }

        public string Value02 { get; set; }

        public string ValueColor01 { get; set; }
        public string ValueColor02 { get; set; }
        public DateTime? CreateDate { get; set; }

    }
  
}