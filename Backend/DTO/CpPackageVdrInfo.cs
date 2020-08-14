using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DTO
{
	/// <summary> 
	///Author: nnamthach@gmail.com 
	/// <summary>
	
    public class CpPackageVdrInfo : DTOInfo
    {
		#region Local Variable
		public class Field {
            public const string
				tvcdb = "tvcdb"
				,packagecode = "packagecode"
				,vendorcode = "vendorcode"
				,status = "status"
				,invitationdate = "invitationdate"
				,responsedate = "responsedate"
				,mark = "mark"
				,overal = "overal"
				,contractno = "contractno"
				,techevaluate = "techevaluate"
				,bidprice = "bidprice"
				,impactamt = "impactamt"
				,externalamt = "externalamt"
				,discountamt = "discountamt"
				,vatcode = "vatcode"
				,vatrate = "vatrate"
				,vatamt = "vatamt"
				,totalawardprice = "totalawardprice"
				,ranking = "ranking"
				,pricedifference = "pricedifference"
                ,pevaluation = "pevaluation"
				,commercialevaluation = "commercialevaluation"
				,techevaludation = "techevaludation"
				,comments = "comments"
				,createdby = "createdby"
				,createddate = "createddate"
				,updatedby = "updatedby"
				,lastupdate = "lastupdate"
            ;
		}
		
		public String tvcdb { get; set; }
		public String packagecode { get; set; }
		public String vendorcode { get; set; }
		public String status { get; set; }
		public DateTime? invitationdate { get; set; }
		public DateTime? responsedate { get; set; }
		public Int16? mark { get; set; }
		public String overal { get; set; }
		public String contractno { get; set; }
		public String techevaluate { get; set; }
		public Decimal? bidprice { get; set; }
		public Decimal? impactamt { get; set; }
		public Decimal? externalamt { get; set; }
		public Decimal? discountamt { get; set; }
		public String vatcode { get; set; }
		public Decimal? vatrate { get; set; }
		public Decimal? vatamt { get; set; }
		public Decimal? totalawardprice { get; set; }
		public String ranking { get; set; }
		public Decimal? pricedifference { get; set; }
		public String pevaluation { get; set; }
		public String commercialevaluation { get; set; }
		public String techevaludation { get; set; }
		public String comments { get; set; }
		public String createdby { get; set; }
		public DateTime? createddate { get; set; }
		public String updatedby { get; set; }
		public DateTime? lastupdate { get; set; }
		
        #endregion LocalVariable
        
        #region Constructor
		public CpPackageVdrInfo() { }
		
		public CpPackageVdrInfo(DataRow dr) { LoadDataRow(dr); }

		public CpPackageVdrInfo(DataRowView dr) : this(dr.Row) { }
		
		public CpPackageVdrInfo(CpPackageVdrInfo objEntr) : base(objEntr) {	}
        #endregion Constructor       
        
    }
}
