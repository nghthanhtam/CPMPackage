using System;
using System.Collections.Generic;
using System.Text;
using DTO;
using DAO;
using System.Data;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace BUS
{
    /// <summary> 
    ///Author: nnamthach@gmail.com 
    /// <summary>
    public class CpPackageControl : BUSControl
    {
        #region Local Variable
        // const string shortCmd = "";
        private CpPackageDataAccess _objDAO;
        #endregion Local Variable

        public CpPackageControl(string type, string connectString, int timeout = 0)
            : base(type, connectString, timeout) { _objDAO = new CpPackageDataAccess(Connnection); }

        public CpPackageControl(BUSControl common)
            : base(common) { _objDAO = new CpPackageDataAccess(common.Connnection); }

        #region Method
        public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            return _objDAO.GetShortData(tvcdb, filters, ref sErr);
        }

        public decimal? GetTotalByPackageTerm(string db, string project, string parentcode, string notId, ref string sErr)
        {
            return _objDAO.GetTotalByPackageTerm(db, project, parentcode, notId, ref sErr);
        }
        public decimal? GetAllTotalPackage(string db, string project, string notId, ref string sErr)
        {
            return _objDAO.GetAllTotalPackage(db, project, notId, ref sErr);
        }

        public decimal? GetTotalByCostCode(string db, string project, string costcode, string notId, ref string sErr)
        {
            return _objDAO.GetTotalByCostCode(db, project, costcode, notId, ref sErr);
        }

        public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0)
        {
            DataTable res = _objDAO.GetByFilterToDataTable(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
            res.Columns.Add("allocString", typeof(String[]));
            BUS.CpPackageAllocControl ctr = new CpPackageAllocControl(this);

            foreach (DataRow i in res.Rows)
            {
                var allocs = ctr.GetByCPK(tvcdb, i["packagecode"].ToString(), ref logMsg);
                String[] arr = new string[allocs.Length];
                int x = 0;
                foreach (var a in allocs)
                {
                    arr[x++] = a.block;
                }
                i["allocString"] = arr;
            }

            return res;
        }

        public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            return _objDAO.GetCountRecord(tvcdb, filters, ref sErr);
        }

        public CpPackageInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0)
        {
            CpPackageInfo[] res = _objDAO.GetByFilter(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
            //get details

            BUS.CpPackageAllocControl ctr = new CpPackageAllocControl(this);

            foreach (var i in res)
            {
                DTO.CriteriaCollection fs = new CriteriaCollection();
                fs.Add("packagecode", i.packagecode);
                i.allocs = ctr.GetByCPK(tvcdb, i.packagecode, ref logMsg);
            }
            return res;
        }

        public int Add(CpPackageInfo obj, ref string sErr)
        {
            _objDAO.Add(obj, ref sErr);
            if (string.IsNullOrEmpty(sErr))
            {
                if (obj.allocs != null)
                {
                    BUS.CpPackageAllocControl ctr = new CpPackageAllocControl(this);
                    foreach (var d in obj.allocs)
                    {
                        if (!string.IsNullOrEmpty(sErr)) break;
                        DTO.CpPackageAllocInfo a = new CpPackageAllocInfo
                        {
                            tvcdb = obj.tvcdb,
                            packagecode = obj.packagecode,
                            block = d.block,
                            createdby = this.USER_ID,
                            updatedby = this.USER_ID
                        };
                        ctr.Add(a, ref sErr);
                    }
                }
            }

            //BUS.CpWorkItemControl widCtr = new CpWorkItemControl(this);
            //if (string.IsNullOrEmpty(sErr))
            //{
            //    if (obj.amount != null && !string.IsNullOrEmpty(obj.costcode))
            //    {
            //        sErr = widCtr.updateCPK(DB, obj.project, obj.costcode);
            //    }

            //}
            return 1;
        }

        public string Update(CpPackageInfo obj)
        {
            string sErr = _objDAO.Update(obj);

            if (obj.allocs != null)
            {
                BUS.CpPackageAllocControl ctrA = new CpPackageAllocControl(this);
                sErr = ctrA.Delete(obj.tvcdb, obj.packagecode);
                foreach (var d in obj.allocs)
                {
                    if (!string.IsNullOrEmpty(sErr)) break;
                    DTO.CpPackageAllocInfo a = new CpPackageAllocInfo
                    {
                        tvcdb = obj.tvcdb,
                        packagecode = obj.packagecode,
                        block = d.block,
                        createdby = this.USER_ID,
                        updatedby = this.USER_ID
                    };
                    ctrA.Add(a, ref sErr);
                }
            }
            // add details
            //BUS.CpWorkItemControl widCtr = new CpWorkItemControl(this);
            //if (string.IsNullOrEmpty(sErr))
            //{
            //    if (obj.amount != null || !string.IsNullOrEmpty(obj.costcode))
            //    {
            //        if (string.IsNullOrEmpty(obj.costcode))
            //        {
            //            var dt = _objDAO.GetCostCode(DB, obj.packagecode, ref sErr);
            //            if (dt.Rows.Count == 1)
            //            {
            //                obj.costcode = dt.Rows[0][0] + "";
            //            }
            //        }
            //        if (!string.IsNullOrEmpty(obj.costcode)) sErr = widCtr.updateCPK(DB, obj.project, obj.costcode);
            //    }

            //}
            return sErr;
        }


        public string Delete(String tvcdb, String packagecode)
        {
            string sErr = string.Empty,
                costcode = string.Empty,
                project = string.Empty;
            // delete details
            var dt = _objDAO.GetCostCode(tvcdb, packagecode, ref sErr);
            //if (dt.Rows.Count == 1)
            //{
            //    costcode = dt.Rows[0][0] + "";
            //    project = dt.Rows[0][1] + "";
            //}
            if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Delete(tvcdb, packagecode);
            //BUS.CpWorkItemControl widCtr = new CpWorkItemControl(this);
            //if (string.IsNullOrEmpty(sErr))
            //{
            //    if (!string.IsNullOrEmpty(costcode)) sErr = widCtr.updateCPK(tvcdb, project, costcode);
            //}
            return sErr;
        }

        public string Clear(string db)
        {
            string sErr = string.Empty;
            // delete details

            if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Clear(db);
            return sErr;
        }

        public bool IsExist(String tvcdb, String packagecode, ref string sErr)
        {
            return _objDAO.IsExist(tvcdb, packagecode, ref sErr);
        }

        public string InsertUpdate(CpPackageInfo obj)
        {
            string sErr = string.Empty;
            if (IsExist(obj.tvcdb, obj.packagecode, ref sErr)) { sErr = Update(obj); }
            else
            {
                obj.createdby = USER_ID;
                if (string.IsNullOrEmpty(sErr)) Add(obj, ref sErr);
            }
            return sErr;
        }

        public override string TransferIn(DTOInfo inf, BUS.BUSControl.TransferMode mode)
        {
            string sErr = string.Empty;
            if (inf is CpPackageInfo)
            {
                ((CpPackageInfo)inf).updatedby = USER_ID;
                if (mode == BUS.BUSControl.TransferMode.AddNew)
                {
                    ((CpPackageInfo)inf).createdby = USER_ID;
                    Add((DTO.CpPackageInfo)inf, ref sErr);
                }
                else if (mode == BUS.BUSControl.TransferMode.Update) sErr = Update((CpPackageInfo)inf);
                else sErr = InsertUpdate((CpPackageInfo)inf);
            }
            return sErr;
        }

        public override bool validate(JObject o, BUS.BUSControl.ValidType type, ref string sErr)
        {
            BUS.StLanguageControl.TraslateMethod tMethod = BUS.StLanguageControl.translate;
            bool inputErr = false;
            if (type == ValidType.Del)
            {
                if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
            }
            else
            {
                if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
                //				if (o["packagetitle"] != null) { if (o["packagetitle"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "packagetitle"), 255) + Environment.NewLine; inputErr = true;}}
                //				if (o["status"] != null) { if (o["status"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "status"), 1) + Environment.NewLine; inputErr = true;}}
                //				if (o["dagid"] != null) { if (o["dagid"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "dagid"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["parentcode"] != null) { if (o["parentcode"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "parentcode"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["isterm"] != null) { if (o["isterm"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "isterm"), 1) + Environment.NewLine; inputErr = true;}}
                //				if (o["packagedate"] != null) { if (o["packagedate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "packagedate"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["packageno"] != null) { if (o["packageno"].ToString().Length > 50) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "packageno"), 50) + Environment.NewLine; inputErr = true;}}
                //				if (o["costcode"] != null) { if (o["costcode"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "costcode"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["pic"] != null) { if (o["pic"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "pic"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["supervisor"] != null) { if (o["supervisor"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "supervisor"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["packagedesc"] != null) { if (o["packagedesc"].ToString().Length > 1000) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "packagedesc"), 1000) + Environment.NewLine; inputErr = true;}}
                //				if (o["requireddate"] != null) { if (o["requireddate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "requireddate"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["expirydate"] != null) { if (o["expirydate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "expirydate"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["asigndate"] != null) { if (o["asigndate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "asigndate"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["receiveddate"] != null) { if (o["receiveddate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "receiveddate"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["currencycode"] != null) { if (o["currencycode"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "currencycode"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["currencyrate"] != null) { if (o["currencyrate"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "currencyrate"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["amount"] != null) { if (o["amount"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "amount"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["vatcode"] != null) { if (o["vatcode"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatcode"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["vatrate"] != null) { if (o["vatrate"].ToString().Length > 9) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatrate"), 9) + Environment.NewLine; inputErr = true;}}
                //				if (o["vatamt"] != null) { if (o["vatamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatamt"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["totalamt"] != null) { if (o["totalamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "totalamt"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["comments"] != null) { if (o["comments"].ToString().Length > 500) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "comments"), 500) + Environment.NewLine; inputErr = true;}}
                //				if (o["project"] != null) { if (o["project"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "project"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["pcklevel"] != null) { if (o["pcklevel"].ToString().Length > 5) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "pcklevel"), 5) + Environment.NewLine; inputErr = true;}}
                //				if (o["leaf"] != null) { if (o["leaf"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "leaf"), 1) + Environment.NewLine; inputErr = true;}}
                //				if (o["contractcode"] != null) { if (o["contractcode"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "contractcode"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk0"] != null) { if (o["anal_cpk0"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk0"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk1"] != null) { if (o["anal_cpk1"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk1"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk2"] != null) { if (o["anal_cpk2"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk2"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk3"] != null) { if (o["anal_cpk3"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk3"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk4"] != null) { if (o["anal_cpk4"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk4"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk5"] != null) { if (o["anal_cpk5"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk5"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk6"] != null) { if (o["anal_cpk6"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk6"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk7"] != null) { if (o["anal_cpk7"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk7"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk8"] != null) { if (o["anal_cpk8"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk8"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["anal_cpk9"] != null) { if (o["anal_cpk9"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_cpk9"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["extreference1"] != null) { if (o["extreference1"].ToString().Length > 30) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extreference1"), 30) + Environment.NewLine; inputErr = true;}}
                //				if (o["extreference2"] != null) { if (o["extreference2"].ToString().Length > 30) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extreference2"), 30) + Environment.NewLine; inputErr = true;}}
                //				if (o["extreference3"] != null) { if (o["extreference3"].ToString().Length > 30) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extreference3"), 30) + Environment.NewLine; inputErr = true;}}
                //				if (o["extreference4"] != null) { if (o["extreference4"].ToString().Length > 30) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extreference4"), 30) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdate1"] != null) { if (o["extdate1"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdate1"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdate2"] != null) { if (o["extdate2"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdate2"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdate3"] != null) { if (o["extdate3"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdate3"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdate4"] != null) { if (o["extdate4"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdate4"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["extnumber1"] != null) { if (o["extnumber1"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extnumber1"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["extnumber2"] != null) { if (o["extnumber2"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extnumber2"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["extnumber3"] != null) { if (o["extnumber3"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extnumber3"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["extnumber4"] != null) { if (o["extnumber4"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extnumber4"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdescription1"] != null) { if (o["extdescription1"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdescription1"), 255) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdescription2"] != null) { if (o["extdescription2"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdescription2"), 255) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdescription3"] != null) { if (o["extdescription3"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdescription3"), 255) + Environment.NewLine; inputErr = true;}}
                //				if (o["extdescription4"] != null) { if (o["extdescription4"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "extdescription4"), 255) + Environment.NewLine; inputErr = true;}}
                //				if (o["approvalstatus"] != null) { if (o["approvalstatus"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "approvalstatus"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["approvedby"] != null) { if (o["approvedby"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "approvedby"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["approveddate"] != null) { if (o["approveddate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "approveddate"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["approvednote"] != null) { if (o["approvednote"].ToString().Length > 500) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "approvednote"), 500) + Environment.NewLine; inputErr = true;}}

                //				if (o["status"] != null) {					
                //                   if (!Array.Exists(new string[] { "C", "W", "S" }, el => el == o["status"].ToString()))
                //                   {
                //                        sErr += String.Format(tMethod(this, "Invalid {0} value not in ({1}). Please check"), tMethod(this, "Status"), "S, C, W") + Environment.NewLine;
                //                        inputErr = true;
                //                   }
                //				}
            }
            return inputErr;
        }

        public int GetQueryCount(string db, string query, List<Criteria> filters, ref string sErr)
        {
            return _objDAO.GetQueryCount(db, query, filters, ref sErr);
        }

        public DataTable GetQueryByFilter(string db, string query, List<Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0)
        {
            return _objDAO.GetQueryByFilter(db, query, filters, ref sErr, indexPage, itemPerPage);
        }

        public override bool isField(string field)
        {
            return new DTO.CpPackageInfo().isField(field);
        }
        public override object CreateDataSource(string tableName)
        {
            return new DTO.CpPackageInfo().ToDataTable(tableName);
        }

        public override bool import(string db, JArray p, string mode, ref string errMsg, ref string logMsg)
        {
            bool anyError = false,
                isTransaction = false;
            BUS.CpPackageControl ctr = this;

            if (p.Count > 1) isTransaction = true;

            if (isTransaction)
            {
                ctr.BeginTransaction(ref logMsg);
                if (logMsg != "")
                {
                    errMsg = BUS.StLanguageControl.translate(this, "There was processing errors. Transaction ROLLBACKED");
                    anyError = true;
                }
            }
            //string shortCmd = _oShortCmd;
            string shortCmd = "CPK";
            String seqcode = null, format = string.Empty;
            BUS.CsSequenceControl ctrSq = new BUS.CsSequenceControl(this);
            bool isSeq = ctrSq.CheckSequence(db, shortCmd, ref logMsg);
            string keyField = "packagecode",
                tableName = "cppackage";
            foreach (JObject o in p)
            {
                if (anyError) break;
                DTO.CpPackageInfo inf = null;
                if (isSeq && (o[keyField] == null || o[keyField].ToString() == ""))
                {
                    o[keyField] = ctrSq.genSeqCode(db, this.LD, ref seqcode, shortCmd, tableName, keyField, o, ref format);
                    if (shortCmd == "PKT") o["isterm"] = "Y";
                    else o["isterm"] = "N";
                }
                anyError = ctr.validate(o, BUS.BUSControl.ValidType.Imp, ref errMsg);

                if (anyError) break;
                try
                {
                    inf = o.ToObject<DTO.CpPackageInfo>();
                    inf.tvcdb = db;
                    inf.updatedby = USER_ID;
                }
                catch (Exception ex)
                {
                    anyError = true;
                    logMsg = ex.Message;
                    errMsg = BUS.StLanguageControl.translate(this, "There was processing errors. Transaction ROLLBACKED");
                }

                if (anyError) break;

                if (IsExist(inf.tvcdb, inf.packagecode, ref logMsg))
                {
                    logMsg = ctr.Update(inf);
                }
                else
                {
                    inf.createdby = USER_ID;
                    if (string.IsNullOrEmpty(logMsg)) ctr.Add(inf, ref logMsg);
                    if (!string.IsNullOrEmpty(logMsg)) anyError = true;
                }
            }

            if (isTransaction)
            {
                if (anyError) RollbackTransaction(ref logMsg);
                else CommitTransaction(ref logMsg);
            }

            return anyError;
        }

        public DataTable GetPKTByFilter(string db, string itemId, string project, CriteriaCollection filters, ref string sErr)
        {
            return _objDAO.GetPKTByFilter(db, itemId, project, filters, ref sErr);
        }
        #endregion Method

    }
}
