using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DTO
{
	/// <summary> 
	///Author: nnamthach@gmail.com 
	/// <summary>
	
    public class CpPackageItemInfo : DTOInfo
    {
		#region Local Variable
		public class Field {
            public const string
				tvcdb = "tvcdb"
				,packagecode = "packagecode"
				,itemid = "itemid"
				,itemcate = "itemcate"
				,itemdesc = "itemdesc"
				,status = "status"
				,location = "location"
				,unit = "unit"
				,quantity = "quantity"
				,materialprice = "materialprice"
				,serviceprice = "serviceprice"
				,materialvalue = "materialvalue"
				,servicevalue = "servicevalue"
				,vatcode = "vatcode"
				,vatrate = "vatrate"
				,vatamt = "vatamt"
				,totalamt = "totalamt"
				,techrequire = "techrequire"
				,duration = "duration"
				,onsitedate = "onsitedate"
				,commencedate = "commencedate"
				,completiondate = "completiondate"
				,comments = "comments"
				,costcode = "costcode"
				,chaptercode = "chaptercode"
				,chaptername = "chaptername"
				,sectioncode = "sectioncode"
				,sectionname = "sectionname"
				,groupcode = "groupcode"
				,groupname = "groupname"
				,typecode = "typecode"
				,typename = "typename "
				,anal_pke0 = "anal_pke0"
				,anal_pke1 = "anal_pke1"
				,anal_pke2 = "anal_pke2"
				,anal_pke3 = "anal_pke3"
				,anal_pke4 = "anal_pke4"
				,anal_pke5 = "anal_pke5"
				,anal_pke6 = "anal_pke6"
				,anal_pke7 = "anal_pke7"
				,anal_pke8 = "anal_pke8"
				,anal_pke9 = "anal_pke9"
				,extreference1 = "extreference1"
				,extreference2 = "extreference2"
				,extreference3 = "extreference3"
				,extreference4 = "extreference4"
				,extdate1 = "extdate1"
				,extdate2 = "extdate2"
				,extdate3 = "extdate3"
				,extdate4 = "extdate4"
				,extnumber1 = "extnumber1"
				,extnumber2 = "extnumber2"
				,extnumber3 = "extnumber3"
				,extnumber4 = "extnumber4"
				,extdescription1 = "extdescription1"
				,extdescription2 = "extdescription2"
				,extdescription3 = "extdescription3"
				,extdescription4 = "extdescription4"
				,createdby = "createdby"
				,createddate = "createddate"
				,updatedby = "updatedby"
				,lastupdate = "lastupdate"
            ;
		}
		
		public String tvcdb { get; set; }
		public String packagecode { get; set; }
		public String itemid { get; set; }
		public String itemcate { get; set; }
		public String itemdesc { get; set; }
		public String status { get; set; }
		public String location { get; set; }
		public String unit { get; set; }
		public Decimal? quantity { get; set; }
		public Decimal? materialprice { get; set; }
		public Decimal? serviceprice { get; set; }
		public Decimal? materialvalue { get; set; }
		public Decimal? servicevalue { get; set; }
		public String vatcode { get; set; }
		public Decimal? vatrate { get; set; }
		public Decimal? vatamt { get; set; }
		public Decimal? totalamt { get; set; }
		public String techrequire { get; set; }
		public Decimal? duration { get; set; }
		public DateTime? onsitedate { get; set; }
		public DateTime? commencedate { get; set; }
		public DateTime? completiondate { get; set; }
		public String comments { get; set; }
		public String costcode { get; set; }
		public String chaptercode { get; set; }
		public String chaptername { get; set; }
		public String sectioncode { get; set; }
		public String sectionname { get; set; }
		public String groupcode { get; set; }
		public String groupname { get; set; }
		public String typecode { get; set; }
		public String typename { get; set; }
		public String anal_pke0 { get; set; }
		public String anal_pke1 { get; set; }
		public String anal_pke2 { get; set; }
		public String anal_pke3 { get; set; }
		public String anal_pke4 { get; set; }
		public String anal_pke5 { get; set; }
		public String anal_pke6 { get; set; }
		public String anal_pke7 { get; set; }
		public String anal_pke8 { get; set; }
		public String anal_pke9 { get; set; }
		public String extreference1 { get; set; }
		public String extreference2 { get; set; }
		public String extreference3 { get; set; }
		public String extreference4 { get; set; }
		public DateTime? extdate1 { get; set; }
		public DateTime? extdate2 { get; set; }
		public DateTime? extdate3 { get; set; }
		public DateTime? extdate4 { get; set; }
		public Decimal? extnumber1 { get; set; }
		public Decimal? extnumber2 { get; set; }
		public Decimal? extnumber3 { get; set; }
		public Decimal? extnumber4 { get; set; }
		public String extdescription1 { get; set; }
		public String extdescription2 { get; set; }
		public String extdescription3 { get; set; }
		public String extdescription4 { get; set; }
		public String createdby { get; set; }
		public DateTime? createddate { get; set; }
		public String updatedby { get; set; }
		public DateTime? lastupdate { get; set; }
		
        #endregion LocalVariable
        
        #region Constructor
		public CpPackageItemInfo() { }
		
		public CpPackageItemInfo(DataRow dr) { LoadDataRow(dr); }

		public CpPackageItemInfo(DataRowView dr) : this(dr.Row) { }
		
		public CpPackageItemInfo(CpPackageItemInfo objEntr) : base(objEntr) {	}
        #endregion Constructor       
        
    }
}
