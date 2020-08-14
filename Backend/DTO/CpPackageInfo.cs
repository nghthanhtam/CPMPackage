using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DTO
{
    /// <summary> 
    ///Author: nnamthach@gmail.com 
    /// <summary>

    public class CpPackageInfo : DTOInfo
    {
        #region Local Variable
        public class Field
        {
            public const string
                tvcdb = "tvcdb"
                , packagecode = "packagecode"
                , approvedqty = "approvedqty"
                , packagetitle = "packagetitle"
                , status = "status"
                , dagid = "dagid"
                , parentcode = "parentcode"
                , packagedate = "packagedate"
                , packageno = "packageno"
                , costcode = "costcode"
                , pic = "pic"
                , supervisor = "supervisor"
                , packagedesc = "packagedesc"
                , requireddate = "requireddate"
                , expirydate = "expirydate"
                , asigndate = "asigndate"
                , receiveddate = "receiveddate"
                , currencycode = "currencycode"
                , currencyrate = "currencyrate"
                , amount = "amount"
                , vatcode = "vatcode"
                , vatrate = "vatrate"
                , vatamt = "vatamt"
                , totalamt = "totalamt"
                , comments = "comments"
                , project = "project"
                , pcklevel = "pcklevel"
                , leaf = "leaf"
                , investmentamt = "investmentamt"
                , contractcode = "contractcode"
                , anal_cpk0 = "anal_cpk0"
                , anal_cpk1 = "anal_cpk1"
                , anal_cpk2 = "anal_cpk2"
                , anal_cpk3 = "anal_cpk3"
                , anal_cpk4 = "anal_cpk4"
                , anal_cpk5 = "anal_cpk5"
                , anal_cpk6 = "anal_cpk6"
                , anal_cpk7 = "anal_cpk7"
                , anal_cpk8 = "anal_cpk8"
                , anal_cpk9 = "anal_cpk9"
                , extreference1 = "extreference1"
                , extreference2 = "extreference2"
                , extreference3 = "extreference3"
                , extreference4 = "extreference4"
                , extdate1 = "extdate1"
                , extdate2 = "extdate2"
                , extdate3 = "extdate3"
                , extdate4 = "extdate4"
                , extnumber1 = "extnumber1"
                , extnumber2 = "extnumber2"
                , extnumber3 = "extnumber3"
                , extnumber4 = "extnumber4"
                , extdescription1 = "extdescription1"
                , extdescription2 = "extdescription2"
                , extdescription3 = "extdescription3"
                , extdescription4 = "extdescription4"
                , approvalstatus = "approvalstatus"
                , approvedby = "approvedby"
                , approveddate = "approveddate"
                , approvednote = "approvednote"
                , packagecate = "packagecate"
                , createdby = "createdby"
                , createddate = "createddate"
                , updatedby = "updatedby"
                , lastupdate = "lastupdate"
                , approvedref = "approvedref"
                , cancelremarks = "cancelremarks"
            ;
        }

        public String tvcdb { get; set; }
        public String packagecode { get; set; }
        public String packagetitle { get; set; }
        public String approvedqty { get; set; }
        public String status { get; set; }
        public String dagid { get; set; }
        public String parentcode { get; set; }
        public DateTime? packagedate { get; set; }
        public String packageno { get; set; }
        public String costcode { get; set; }
        public String pic { get; set; }
        public String supervisor { get; set; }
        public String packagedesc { get; set; }
        public DateTime? requireddate { get; set; }
        public DateTime? expirydate { get; set; }
        public DateTime? asigndate { get; set; }
        public DateTime? receiveddate { get; set; }
        public String currencycode { get; set; }
        public Decimal? currencyrate { get; set; }
        public Decimal? amount { get; set; }
        public String vatcode { get; set; }
        public Decimal? vatrate { get; set; }
        public Decimal? vatamt { get; set; }
        public Decimal? totalamt { get; set; }
        public String comments { get; set; }
        public String project { get; set; }
        public Int16? pcklevel { get; set; }
        public String leaf { get; set; }
        public String contractcode { get; set; }
        public String anal_cpk0 { get; set; }
        public String anal_cpk1 { get; set; }
        public String anal_cpk2 { get; set; }
        public String anal_cpk3 { get; set; }
        public String anal_cpk4 { get; set; }
        public String anal_cpk5 { get; set; }
        public String anal_cpk6 { get; set; }
        public String anal_cpk7 { get; set; }
        public String anal_cpk8 { get; set; }
        public String anal_cpk9 { get; set; }
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
        public String approvalstatus { get; set; }
        public String approvedby { get; set; }
        public Decimal? investmentamt { get; set; }
        public DateTime? approveddate { get; set; }
        public String approvednote { get; set; }
        public String packagecate { get; set; }
        public String approvedref { get; set; }
        public String createdby { get; set; }
        public DateTime? createddate { get; set; }
        public String updatedby { get; set; }
        public DateTime? lastupdate { get; set; }
        public CpPackageAllocInfo[] allocs { get; set; }
        public string[] allocString { get; set; }
        public String cancelremarks { get; set; }
        #endregion LocalVariable

        #region Constructor
        public CpPackageInfo() { }

        public CpPackageInfo(DataRow dr) { LoadDataRow(dr); }

        public CpPackageInfo(DataRowView dr) : this(dr.Row) { }

        public CpPackageInfo(CpPackageInfo objEntr) : base(objEntr) { }
        #endregion Constructor       

    }
}
