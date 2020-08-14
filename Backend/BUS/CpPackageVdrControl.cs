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
    public class CpPackageVdrControl : BUSControl
    {
		#region Local Variable
		const string shortCmd = "";
        private CpPackageVdrDataAccess _objDAO;
		#endregion Local Variable

	    public CpPackageVdrControl(string type, string connectString, int timeout = 0)
            : base(type, connectString, timeout) { _objDAO = new CpPackageVdrDataAccess(Connnection); }
			
        public CpPackageVdrControl(BUSControl common)
            : base(common) { _objDAO = new CpPackageVdrDataAccess(common.Connnection); }
	
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
		
		public CpPackageVdrInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0) {
            CpPackageVdrInfo[] res = _objDAO.GetByFilter(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
			//get details
			
			return res;
        }
		
        public int Add(CpPackageVdrInfo obj, ref string sErr) {
            _objDAO.Add(obj, ref sErr);
			if (string.IsNullOrEmpty(sErr)) {
				//add details
			}
			return 1;
        }
		
        public string Update(CpPackageVdrInfo obj) {
            string sErr = _objDAO.Update(obj);
			// delete details
			// add details
			return sErr;
        }
		
        public string Delete(String tvcdb, String packagecode, String vendorcode) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Delete(tvcdb, packagecode, vendorcode);
			return sErr;
        }  
		
		public string Clear(string db) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Clear(db);
			return sErr;
        }
		
        public bool IsExist(String tvcdb, String packagecode, String vendorcode, ref string sErr) {
            return _objDAO.IsExist(tvcdb, packagecode, vendorcode, ref sErr);
        } 

        public string InsertUpdate(CpPackageVdrInfo obj)
        {
            string sErr = string.Empty;
            if (IsExist(obj.tvcdb, obj.packagecode, obj.vendorcode, ref sErr)) { sErr = Update(obj); }
            else {
				obj.createdby = USER_ID;
				if (string.IsNullOrEmpty(sErr)) Add(obj, ref sErr); 
			}
            return sErr;
        }
       
        public override string TransferIn(DTOInfo inf, BUS.BUSControl.TransferMode mode) {
            string sErr = string.Empty;
            if (inf is CpPackageVdrInfo) {
				((CpPackageVdrInfo)inf).updatedby = USER_ID;
                if (mode == BUS.BUSControl.TransferMode.AddNew) {
					((CpPackageVdrInfo)inf).createdby = USER_ID;
					Add((DTO.CpPackageVdrInfo)inf, ref sErr);
				} else if (mode == BUS.BUSControl.TransferMode.Update) sErr = Update((CpPackageVdrInfo)inf);
                else sErr = InsertUpdate((CpPackageVdrInfo)inf);
            }
            return sErr;
        }

		public override bool validate(JObject o, BUS.BUSControl.ValidType type, ref string sErr) {
			BUS.StLanguageControl.TraslateMethod tMethod = BUS.StLanguageControl.translate;
            bool inputErr = false;
			if (type == ValidType.Del) {
                if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
                if (o["vendorcode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "vendorcode")) + Environment.NewLine; inputErr = true; }
            } else {
				if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
				if (o["vendorcode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "vendorcode")) + Environment.NewLine; inputErr = true; }
//				if (o["status"] != null) { if (o["status"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "status"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["invitationdate"] != null) { if (o["invitationdate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "invitationdate"), 0) + Environment.NewLine; inputErr = true;}}
//				if (o["responsedate"] != null) { if (o["responsedate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "responsedate"), 0) + Environment.NewLine; inputErr = true;}}
//				if (o["mark"] != null) { if (o["mark"].ToString().Length > 5) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "mark"), 5) + Environment.NewLine; inputErr = true;}}
//				if (o["overal"] != null) { if (o["overal"].ToString().Length > 1000) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "overal"), 1000) + Environment.NewLine; inputErr = true;}}
//				if (o["contractno"] != null) { if (o["contractno"].ToString().Length > 50) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "contractno"), 50) + Environment.NewLine; inputErr = true;}}
//				if (o["techevaluate"] != null) { if (o["techevaluate"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "techevaluate"), 1) + Environment.NewLine; inputErr = true;}}
//				if (o["bidprice"] != null) { if (o["bidprice"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "bidprice"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["impactamt"] != null) { if (o["impactamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "impactamt"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["externalamt"] != null) { if (o["externalamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "externalamt"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["discountamt"] != null) { if (o["discountamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "discountamt"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["vatcode"] != null) { if (o["vatcode"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatcode"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["vatrate"] != null) { if (o["vatrate"].ToString().Length > 9) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatrate"), 9) + Environment.NewLine; inputErr = true;}}
//				if (o["vatamt"] != null) { if (o["vatamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatamt"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["totalawardprice"] != null) { if (o["totalawardprice"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "totalawardprice"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["ranking"] != null) { if (o["ranking"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "ranking"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["pricedifferenceo"] != null) { if (o["pricedifferenceo"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "pricedifferenceo"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["pevaluation"] != null) { if (o["pevaluation"].ToString().Length > 1000) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "pevaluation"), 1000) + Environment.NewLine; inputErr = true;}}
//				if (o["commercialevaluation"] != null) { if (o["commercialevaluation"].ToString().Length > 1000) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "commercialevaluation"), 1000) + Environment.NewLine; inputErr = true;}}
//				if (o["techevaludation"] != null) { if (o["techevaludation"].ToString().Length > 1000) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "techevaludation"), 1000) + Environment.NewLine; inputErr = true;}}
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
            return new DTO.CpPackageVdrInfo().isField(field);
        }
		public override object CreateDataSource(string tableName)
        {
            return new DTO.CpPackageVdrInfo().ToDataTable(tableName);
        }
		
		public override bool import(string db, JArray p, string mode, ref string errMsg, ref string logMsg) {
            bool anyError = false,
				isTransaction = false;
            BUS.CpPackageVdrControl ctr = this;

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
            string keyField = "packagecodevendorcode",
				tableName = "cppackagevdr";
            foreach (JObject o in p) {
                if (anyError) break;
                DTO.CpPackageVdrInfo inf = null;
				if (isSeq && (o[keyField] == null || o[keyField].ToString() == "")) {
                    o[keyField] = ctrSq.genSeqCode(db, this.LD, ref seqcode, shortCmd, tableName, keyField, o, ref format);
                }
                anyError = ctr.validate(o, BUS.BUSControl.ValidType.Imp, ref errMsg);

                if (anyError) break;
                try {
                    inf = o.ToObject<DTO.CpPackageVdrInfo>();
                    inf.tvcdb = db;
                    inf.updatedby = USER_ID;
                } catch (Exception ex) {
                    anyError = true;
                    logMsg = ex.Message;
                    errMsg = BUS.StLanguageControl.translate(this, "There was processing errors. Transaction ROLLBACKED");
                }

                if (anyError) break;

                if (IsExist(inf.tvcdb, inf.packagecode, inf.vendorcode, ref logMsg)) {
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
