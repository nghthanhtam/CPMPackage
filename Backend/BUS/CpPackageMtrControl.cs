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
    public class CpPackageMtrControl : BUSControl
    {
		#region Local Variable
		const string shortCmd = "";
        private CpPackageMtrDataAccess _objDAO;
		#endregion Local Variable

	    public CpPackageMtrControl(string type, string connectString, int timeout = 0)
            : base(type, connectString, timeout) { _objDAO = new CpPackageMtrDataAccess(Connnection); }
			
        public CpPackageMtrControl(BUSControl common)
            : base(common) { _objDAO = new CpPackageMtrDataAccess(common.Connnection); }
	
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
		
		public CpPackageMtrInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0) {
            CpPackageMtrInfo[] res = _objDAO.GetByFilter(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
			//get details
			
			return res;
        }
		
        public int Add(CpPackageMtrInfo obj, ref string sErr) {
            _objDAO.Add(obj, ref sErr);
			if (string.IsNullOrEmpty(sErr)) {
				//add details
			}
			return 1;
        }
		
        public string Update(CpPackageMtrInfo obj) {
            string sErr = _objDAO.Update(obj);
			// delete details
			// add details
			return sErr;
        }
		
        public string Delete(String tvcdb, String packagecode, String materialcode) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Delete(tvcdb, packagecode, materialcode);
			return sErr;
        }  
		
		public string Clear(string db) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Clear(db);
			return sErr;
        }
		
        public bool IsExist(String tvcdb, String packagecode, String materialcode, ref string sErr) {
            return _objDAO.IsExist(tvcdb, packagecode, materialcode, ref sErr);
        } 

        public string InsertUpdate(CpPackageMtrInfo obj)
        {
            string sErr = string.Empty;
            if (IsExist(obj.tvcdb, obj.packagecode, obj.materialcode, ref sErr)) { sErr = Update(obj); }
            else {
				obj.createdby = USER_ID;
				if (string.IsNullOrEmpty(sErr)) Add(obj, ref sErr); 
			}
            return sErr;
        }
       
        public override string TransferIn(DTOInfo inf, BUS.BUSControl.TransferMode mode) {
            string sErr = string.Empty;
            if (inf is CpPackageMtrInfo) {
				((CpPackageMtrInfo)inf).updatedby = USER_ID;
                if (mode == BUS.BUSControl.TransferMode.AddNew) {
					((CpPackageMtrInfo)inf).createdby = USER_ID;
					Add((DTO.CpPackageMtrInfo)inf, ref sErr);
				} else if (mode == BUS.BUSControl.TransferMode.Update) sErr = Update((CpPackageMtrInfo)inf);
                else sErr = InsertUpdate((CpPackageMtrInfo)inf);
            }
            return sErr;
        }

		public override bool validate(JObject o, BUS.BUSControl.ValidType type, ref string sErr) {
			BUS.StLanguageControl.TraslateMethod tMethod = BUS.StLanguageControl.translate;
            bool inputErr = false;
			if (type == ValidType.Del) {
                if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
                if (o["materialcode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "materialcode")) + Environment.NewLine; inputErr = true; }
            } else {
				if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
				if (o["materialcode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "materialcode")) + Environment.NewLine; inputErr = true; }
//				if (o["materialname"] != null) { if (o["materialname"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "materialname"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["status"] != null) { if (o["status"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "status"), 1) + Environment.NewLine; inputErr = true;}}
//				if (o["category"] != null) { if (o["category"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "category"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["materialtype"] != null) { if (o["materialtype"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "materialtype"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["partnum"] != null) { if (o["partnum"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "partnum"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["techrequire"] != null) { if (o["techrequire"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "techrequire"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["specification"] != null) { if (o["specification"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "specification"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["origincountry"] != null) { if (o["origincountry"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "origincountry"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["manufacturer"] != null) { if (o["manufacturer"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "manufacturer"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["vendor"] != null) { if (o["vendor"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vendor"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["unit"] != null) { if (o["unit"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "unit"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["netprice"] != null) { if (o["netprice"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "netprice"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["grossprice"] != null) { if (o["grossprice"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "grossprice"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["comments"] != null) { if (o["comments"].ToString().Length > 500) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "comments"), 500) + Environment.NewLine; inputErr = true;}}

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
            return new DTO.CpPackageMtrInfo().isField(field);
        }
		public override object CreateDataSource(string tableName)
        {
            return new DTO.CpPackageMtrInfo().ToDataTable(tableName);
        }
		
		public override bool import(string db, JArray p, string mode, ref string errMsg, ref string logMsg) {
            bool anyError = false,
				isTransaction = false;
            BUS.CpPackageMtrControl ctr = this;

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
            string keyField = "packagecodematerialcode",
				tableName = "cppackagemtr";
            foreach (JObject o in p) {
                if (anyError) break;
                DTO.CpPackageMtrInfo inf = null;
				if (isSeq && (o[keyField] == null || o[keyField].ToString() == "")) {
                    o[keyField] = ctrSq.genSeqCode(db, this.LD, ref seqcode, shortCmd, tableName, keyField, o, ref format);
                }
                anyError = ctr.validate(o, BUS.BUSControl.ValidType.Imp, ref errMsg);

                if (anyError) break;
                try {
                    inf = o.ToObject<DTO.CpPackageMtrInfo>();
                    inf.tvcdb = db;
                    inf.updatedby = USER_ID;
                } catch (Exception ex) {
                    anyError = true;
                    logMsg = ex.Message;
                    errMsg = BUS.StLanguageControl.translate(this, "There was processing errors. Transaction ROLLBACKED");
                }

                if (anyError) break;

                if (IsExist(inf.tvcdb, inf.packagecode, inf.materialcode, ref logMsg)) {
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
