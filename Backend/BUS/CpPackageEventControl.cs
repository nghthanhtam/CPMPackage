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
    public class CpPackageEventControl : BUSControl
    {
		#region Local Variable
		const string shortCmd = "";
        private CpPackageEventDataAccess _objDAO;
		#endregion Local Variable

	    public CpPackageEventControl(string type, string connectString, int timeout = 0)
            : base(type, connectString, timeout) { _objDAO = new CpPackageEventDataAccess(Connnection); }
			
        public CpPackageEventControl(BUSControl common)
            : base(common) { _objDAO = new CpPackageEventDataAccess(common.Connnection); }
	
		#region Method
		public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            return _objDAO.GetShortData(tvcdb, filters, ref sErr);
        }
       		
        public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0) {
            return _objDAO.GetByFilterToDataTable(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
        }
		
		public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            return _objDAO.GetCountRecord(tvcdb, filters, ref sErr);
        }
		
		public CpPackageEventInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0) {
            CpPackageEventInfo[] res = _objDAO.GetByFilter(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
			//get details
			
			return res;
        }
		
        public int Add(CpPackageEventInfo obj, ref string sErr) {
            _objDAO.Add(obj, ref sErr);
			if (string.IsNullOrEmpty(sErr)) {
				//add details
			}
			return 1;
        }
		
        public string Update(CpPackageEventInfo obj) {
            string sErr = _objDAO.Update(obj);
			// delete details
			// add details
			return sErr;
        }
		
        public string Delete(String tvcdb, String packagecode, String eventid) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Delete(tvcdb, packagecode, eventid);
			return sErr;
        }  
		
		public string Clear(string db) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Clear(db);
			return sErr;
        }
		
        public bool IsExist(String tvcdb, String packagecode, String eventid, ref string sErr) {
            return _objDAO.IsExist(tvcdb, packagecode, eventid, ref sErr);
        } 

        public string InsertUpdate(CpPackageEventInfo obj)
        {
            string sErr = string.Empty;
            if (IsExist(obj.tvcdb, obj.packagecode, obj.eventid, ref sErr)) { sErr = Update(obj); }
            else {
				obj.createdby = USER_ID;
				if (string.IsNullOrEmpty(sErr)) Add(obj, ref sErr); 
			}
            return sErr;
        }
       
        public override string TransferIn(DTOInfo inf, BUS.BUSControl.TransferMode mode) {
            string sErr = string.Empty;
            if (inf is CpPackageEventInfo) {
				((CpPackageEventInfo)inf).updatedby = USER_ID;
                if (mode == BUS.BUSControl.TransferMode.AddNew) {
					((CpPackageEventInfo)inf).createdby = USER_ID;
					Add((DTO.CpPackageEventInfo)inf, ref sErr);
				} else if (mode == BUS.BUSControl.TransferMode.Update) sErr = Update((CpPackageEventInfo)inf);
                else sErr = InsertUpdate((CpPackageEventInfo)inf);
            }
            return sErr;
        }

		public override bool validate(JObject o, BUS.BUSControl.ValidType type, ref string sErr) {
			BUS.StLanguageControl.TraslateMethod tMethod = BUS.StLanguageControl.translate;
            bool inputErr = false;
			if (type == ValidType.Del) {
                if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
                if (o["eventid"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "eventid")) + Environment.NewLine; inputErr = true; }
            } else {
				if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
				if (o["eventid"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "eventid")) + Environment.NewLine; inputErr = true; }
//				if (o["eventtype"] != null) { if (o["eventtype"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "eventtype"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["startdate"] != null) { if (o["startdate"].ToString().Length > 3) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "startdate"), 3) + Environment.NewLine; inputErr = true;}}
//				if (o["enddate"] != null) { if (o["enddate"].ToString().Length > 3) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "enddate"), 3) + Environment.NewLine; inputErr = true;}}
//				if (o["eventstatus"] != null) { if (o["eventstatus"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "eventstatus"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["lookup"] != null) { if (o["lookup"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "lookup"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["pic"] != null) { if (o["pic"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "pic"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["notes"] != null) { if (o["notes"].ToString().Length > 500) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "notes"), 500) + Environment.NewLine; inputErr = true;}}

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
		public override bool isField(string field) {
            return new DTO.CpPackageEventInfo().isField(field);
        }
		public override object CreateDataSource(string tableName)
        {
            return new DTO.CpPackageEventInfo().ToDataTable(tableName);
        }
		
		public override bool import(string db, JArray p, string mode, ref string errMsg, ref string logMsg) {
            bool anyError = false,
				isTransaction = false;
            BUS.CpPackageEventControl ctr = this;

            if (p.Count > 1) isTransaction = true;

            if (isTransaction) {
                ctr.BeginTransaction(ref logMsg);
                if (logMsg != "") {
                    errMsg = BUS.StLanguageControl.translate(this, "There was processing errors. Transaction ROLLBACKED");
                    anyError = true;
                }
            }
			String seqcode = null, format = string.Empty;
            BUS.CsSequenceControl ctrSq = new BUS.CsSequenceControl(this);
            bool isSeq = ctrSq.CheckSequence(db, shortCmd, ref logMsg);
            string keyField = "packagecodeeventid",
				tableName = "cppackageevent";
            foreach (JObject o in p) {
                if (anyError) break;
                DTO.CpPackageEventInfo inf = null;
				if (isSeq && (o[keyField] == null || o[keyField].ToString() == "")) {
                    o[keyField] = ctrSq.genSeqCode(db, this.LD, ref seqcode, shortCmd, tableName, keyField, o, ref format);
                }
                anyError = ctr.validate(o, BUS.BUSControl.ValidType.Imp, ref errMsg);

                if (anyError) break;
                try {
                    inf = o.ToObject<DTO.CpPackageEventInfo>();
                    inf.tvcdb = db;
                    inf.updatedby = USER_ID;
                } catch (Exception ex) {
                    anyError = true;
                    logMsg = ex.Message;
                    errMsg = BUS.StLanguageControl.translate(this, "There was processing errors. Transaction ROLLBACKED");
                }

                if (anyError) break;

                if (IsExist(inf.tvcdb, inf.packagecode, inf.eventid, ref logMsg)) {
                    logMsg = ctr.Update(inf);
                } else {
                    inf.createdby = USER_ID;
                    if (string.IsNullOrEmpty(logMsg)) ctr.Add(inf, ref logMsg);
                    if (!string.IsNullOrEmpty(logMsg)) anyError = true;
                }
            }

            if (isTransaction) {
                if (anyError) RollbackTransaction(ref logMsg);
                else CommitTransaction(ref logMsg);
            }

            return anyError;
        }
		#endregion Method

    }
}
