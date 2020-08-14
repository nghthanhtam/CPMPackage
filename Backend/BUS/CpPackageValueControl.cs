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
    public class CpPackageValueControl : BUSControl
    {
        #region Local Variable
        const string shortCmd = "";
        private CpPackageValueDataAccess _objDAO;
        #endregion Local Variable

        public CpPackageValueControl(string type, string connectString, int timeout = 0)
            : base(type, connectString, timeout) { _objDAO = new CpPackageValueDataAccess(Connnection); }

        public CpPackageValueControl(BUSControl common)
            : base(common) { _objDAO = new CpPackageValueDataAccess(common.Connnection); }

        #region Method
        public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            return _objDAO.GetShortData(tvcdb, filters, ref sErr);
        }

        public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0)
        {
            return _objDAO.GetByFilterToDataTable(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
        }

        public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            return _objDAO.GetCountRecord(tvcdb, filters, ref sErr);
        }

        public CpPackageValueInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0)
        {
            CpPackageValueInfo[] res = _objDAO.GetByFilter(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
            //get details

            return res;
        }

        public int Add(CpPackageValueInfo obj, ref string sErr)
        {
            _objDAO.Add(obj, ref sErr);
            if (string.IsNullOrEmpty(sErr))
            {
                //add details
            }
            return 1;
        }

        public string Update(CpPackageValueInfo obj)
        {
            string sErr = _objDAO.Update(obj);
            // delete details
            // add details
            return sErr;
        }

        public string Delete(String tvcdb, String packagecode, String valueid = null)
        {
            string sErr = string.Empty;
            // delete details

            if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Delete(tvcdb, packagecode, valueid);
            return sErr;
        }

        public string Clear(string db)
        {
            string sErr = string.Empty;
            // delete details

            if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Clear(db);
            return sErr;
        }

        public bool IsExist(String tvcdb, String packagecode, String valueid, ref string sErr)
        {
            return _objDAO.IsExist(tvcdb, packagecode, valueid, ref sErr);
        }

        public string InsertUpdate(CpPackageValueInfo obj)
        {
            string sErr = string.Empty;
            if (IsExist(obj.tvcdb, obj.packagecode, obj.valueid, ref sErr)) { sErr = Update(obj); }
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
            if (inf is CpPackageValueInfo)
            {
                ((CpPackageValueInfo)inf).updatedby = USER_ID;
                if (mode == BUS.BUSControl.TransferMode.AddNew)
                {
                    ((CpPackageValueInfo)inf).createdby = USER_ID;
                    Add((DTO.CpPackageValueInfo)inf, ref sErr);
                }
                else if (mode == BUS.BUSControl.TransferMode.Update) sErr = Update((CpPackageValueInfo)inf);
                else sErr = InsertUpdate((CpPackageValueInfo)inf);
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
                if (o["valueid"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "valueid")) + Environment.NewLine; inputErr = true; }
            }
            else
            {
                if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
                if (o["valueid"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "valueid")) + Environment.NewLine; inputErr = true; }
                //				if (o["transdate"] != null) { if (o["transdate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "transdate"), 0) + Environment.NewLine; inputErr = true;}}
                //				if (o["employee"] != null) { if (o["employee"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "employee"), 25) + Environment.NewLine; inputErr = true;}}
                //				if (o["valuedesc"] != null) { if (o["valuedesc"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "valuedesc"), 255) + Environment.NewLine; inputErr = true;}}
                //				if (o["amount"] != null) { if (o["amount"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "amount"), 18) + Environment.NewLine; inputErr = true;}}
                //				if (o["original"] != null) { if (o["original"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "original"), 1) + Environment.NewLine; inputErr = true;}}
                //				if (o["status"] != null) { if (o["status"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "status"), 1) + Environment.NewLine; inputErr = true;}}
                //				if (o["comments"] != null) { if (o["comments"].ToString().Length > 500) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "comments"), 500) + Environment.NewLine; inputErr = true;}}
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

        public decimal GetTotal(string db, string project, string costcode, string notId, string status, ref string sErr)
        {
            return _objDAO.GetTotal(db, project, costcode, notId, status, ref sErr);
        }
        public decimal GetTotalByPackageCode(string db, string project, string packagecode, string status, ref string sErr)
        {
            return _objDAO.GetTotalByPackageCode(db, project,  packagecode, status, ref sErr);
        }

        public override bool isField(string field)
        {
            return new DTO.CpPackageValueInfo().isField(field);
        }
        public override object CreateDataSource(string tableName)
        {
            return new DTO.CpPackageValueInfo().ToDataTable(tableName);
        }

        public override bool import(string db, JArray p, string mode, ref string errMsg, ref string logMsg)
        {
            bool anyError = false,
                isTransaction = false;
            BUS.CpPackageValueControl ctr = this;

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
            String seqcode = null, format = string.Empty;
            BUS.CsSequenceControl ctrSq = new BUS.CsSequenceControl(this);
            bool isSeq = ctrSq.CheckSequence(db, shortCmd, ref logMsg);
            string keyField = "packagecodevalueid",
                tableName = "cppackagevalue";
            foreach (JObject o in p)
            {
                if (anyError) break;
                DTO.CpPackageValueInfo inf = null;
                if (isSeq && (o[keyField] == null || o[keyField].ToString() == ""))
                {
                    o[keyField] = ctrSq.genSeqCode(db, this.LD, ref seqcode, shortCmd, tableName, keyField, o, ref format);
                }
                anyError = ctr.validate(o, BUS.BUSControl.ValidType.Imp, ref errMsg);

                if (anyError) break;
                try
                {
                    inf = o.ToObject<DTO.CpPackageValueInfo>();
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

                if (IsExist(inf.tvcdb, inf.packagecode, inf.valueid, ref logMsg))
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
        #endregion Method

    }
}
