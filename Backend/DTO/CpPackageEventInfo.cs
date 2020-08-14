using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DTO
{
	/// <summary> 
	///Author: nnamthach@gmail.com 
	/// <summary>
	
    public class CpPackageEventInfo : DTOInfo
    {
		#region Local Variable
		public class Field {
            public const string
				tvcdb = "tvcdb"
				,packagecode = "packagecode"
				,eventid = "eventid"
				,eventtype = "eventtype"
				,startdate = "startdate"
				,enddate = "enddate"
				,eventstatus = "eventstatus"
				,lookup = "lookup"
				,pic = "pic"
				,notes = "notes"
				,createdby = "createdby"
				,createddate = "createddate"
				,updatedby = "updatedby"
				,lastupdate = "lastupdate"
            ;
		}
		
		public String tvcdb { get; set; }
		public String packagecode { get; set; }
		public String eventid { get; set; }
		public String eventtype { get; set; }
		public DateTime? startdate { get; set; }
		public DateTime? enddate { get; set; }
		public String eventstatus { get; set; }
		public String lookup { get; set; }
		public String pic { get; set; }
		public String notes { get; set; }
		public String createdby { get; set; }
		public DateTime? createddate { get; set; }
		public String updatedby { get; set; }
		public DateTime? lastupdate { get; set; }
		
        #endregion LocalVariable
        
        #region Constructor
		public CpPackageEventInfo() { }
		
		public CpPackageEventInfo(DataRow dr) { LoadDataRow(dr); }

		public CpPackageEventInfo(DataRowView dr) : this(dr.Row) { }
		
		public CpPackageEventInfo(CpPackageEventInfo objEntr) : base(objEntr) {	}
        #endregion Constructor       
        
    }
}
