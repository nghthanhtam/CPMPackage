using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DTO
{
	/// <summary> 
	///Author: nnamthach@gmail.com 
	/// <summary>
	
    public class CpPackageMtrInfo : DTOInfo
    {
		#region Local Variable
		public class Field {
            public const string
				tvcdb = "tvcdb"
				,packagecode = "packagecode"
				,materialcode = "materialcode"
				,materialname = "materialname"
				,status = "status"
				,category = "category"
				,materialtype = "materialtype"
				,partnum = "partnum"
				,techrequire = "techrequire"
				,specification = "specification"
				,origincountry = "origincountry"
				,manufacturer = "manufacturer"
				,vendor = "vendor"
				,unit = "unit"
				,netprice = "netprice"
				,grossprice = "grossprice"
				,comments = "comments"
				, materialgroup = "materialgroup"
				, techtype = "techtype"
				, createdby = "createdby"
				,createddate = "createddate"
				,updatedby = "updatedby"
				,lastupdate = "lastupdate"
            ;
		}
		
		public String tvcdb { get; set; }
		public String packagecode { get; set; }
		public String materialcode { get; set; }
		public String materialname { get; set; }
		public String status { get; set; }
		public String category { get; set; }
		public String materialtype { get; set; }
		public String partnum { get; set; }
		public String techrequire { get; set; }
		public String specification { get; set; }
		public String origincountry { get; set; }
		public String manufacturer { get; set; }
		public String vendor { get; set; }
		public String unit { get; set; }
		public Decimal? netprice { get; set; }
		public Decimal? grossprice { get; set; }
		public String comments { get; set; }
		public String materialgroup { get; set; }
		public String techtype { get; set; }
		public String createdby { get; set; }
		public DateTime? createddate { get; set; }
		public String updatedby { get; set; }
		public DateTime? lastupdate { get; set; }
		
        #endregion LocalVariable
        
        #region Constructor
		public CpPackageMtrInfo() { }
		
		public CpPackageMtrInfo(DataRow dr) { LoadDataRow(dr); }

		public CpPackageMtrInfo(DataRowView dr) : this(dr.Row) { }
		
		public CpPackageMtrInfo(CpPackageMtrInfo objEntr) : base(objEntr) {	}
        #endregion Constructor       
        
    }
}
