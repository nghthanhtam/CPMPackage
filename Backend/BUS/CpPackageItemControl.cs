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
    public class CpPackageItemControl : BUSControl
    {
		#region Local Variable
		const string shortCmd = "";
        private CpPackageItemDataAccess _objDAO;
		#endregion Local Variable

	    public CpPackageItemControl(string type, string connectString, int timeout = 0)
            : base(type, connectString, timeout) { _objDAO = new CpPackageItemDataAccess(Connnection); }
			
        public CpPackageItemControl(BUSControl common)
            : base(common) { _objDAO = new CpPackageItemDataAccess(common.Connnection); }
	
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
		
		public CpPackageItemInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string logMsg, int indexPage = 0, int itemPerPage = 0) {
            CpPackageItemInfo[] res = _objDAO.GetByFilter(tvcdb, filters, ref logMsg, indexPage, itemPerPage);
			//get details
			
			return res;
        }
		
        public int Add(CpPackageItemInfo obj, ref string sErr) {
            _objDAO.Add(obj, ref sErr);
			if (string.IsNullOrEmpty(sErr)) {
				//add details
			}
			return 1;
        }
		
        public string Update(CpPackageItemInfo obj) {
            string sErr = _objDAO.Update(obj);
			// delete details
			// add details
			return sErr;
        }
		
        public string Delete(String tvcdb, String packagecode, String itemid) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Delete(tvcdb, packagecode, itemid);
			return sErr;
        }  
		
		public string Clear(string db) {
            string sErr = string.Empty;
			// delete details
			
			if (string.IsNullOrEmpty(sErr)) sErr = _objDAO.Clear(db);
			return sErr;
        }
		
        public bool IsExist(String tvcdb, String packagecode, String itemid, ref string sErr) {
            return _objDAO.IsExist(tvcdb, packagecode, itemid, ref sErr);
        } 

        public string InsertUpdate(CpPackageItemInfo obj)
        {
            string sErr = string.Empty;
            if (IsExist(obj.tvcdb, obj.packagecode, obj.itemid, ref sErr)) { sErr = Update(obj); }
            else {
				obj.createdby = USER_ID;
				if (string.IsNullOrEmpty(sErr)) Add(obj, ref sErr); 
			}
            return sErr;
        }
       
        public override string TransferIn(DTOInfo inf, BUS.BUSControl.TransferMode mode) {
            string sErr = string.Empty;
            if (inf is CpPackageItemInfo) {
				((CpPackageItemInfo)inf).updatedby = USER_ID;
                if (mode == BUS.BUSControl.TransferMode.AddNew) {
					((CpPackageItemInfo)inf).createdby = USER_ID;
					Add((DTO.CpPackageItemInfo)inf, ref sErr);
				} else if (mode == BUS.BUSControl.TransferMode.Update) sErr = Update((CpPackageItemInfo)inf);
                else sErr = InsertUpdate((CpPackageItemInfo)inf);
            }
            return sErr;
        }

		public override bool validate(JObject o, BUS.BUSControl.ValidType type, ref string sErr) {
			BUS.StLanguageControl.TraslateMethod tMethod = BUS.StLanguageControl.translate;
            bool inputErr = false;
			if (type == ValidType.Del) {
                if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
                if (o["itemid"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "itemid")) + Environment.NewLine; inputErr = true; }
            } else {
				if (o["packagecode"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "packagecode")) + Environment.NewLine; inputErr = true; }
				if (o["itemid"] == null) { sErr += string.Format(tMethod(this, "Invalid {0}. Please check again"), tMethod(this, "itemid")) + Environment.NewLine; inputErr = true; }
//				if (o["itemcate"] != null) { if (o["itemcate"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "itemcate"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["itemdesc"] != null) { if (o["itemdesc"].ToString().Length > 255) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "itemdesc"), 255) + Environment.NewLine; inputErr = true;}}
//				if (o["status"] != null) { if (o["status"].ToString().Length > 1) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "status"), 1) + Environment.NewLine; inputErr = true;}}
//				if (o["location"] != null) { if (o["location"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "location"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["unit"] != null) { if (o["unit"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "unit"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["quantity"] != null) { if (o["quantity"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "quantity"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["materialprice"] != null) { if (o["materialprice"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "materialprice"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["serviceprice"] != null) { if (o["serviceprice"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "serviceprice"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["materialvalue"] != null) { if (o["materialvalue"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "materialvalue"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["servicevalue"] != null) { if (o["servicevalue"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "servicevalue"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["vatcode"] != null) { if (o["vatcode"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatcode"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["vatrate"] != null) { if (o["vatrate"].ToString().Length > 9) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatrate"), 9) + Environment.NewLine; inputErr = true;}}
//				if (o["vatamt"] != null) { if (o["vatamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "vatamt"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["totalamt"] != null) { if (o["totalamt"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "totalamt"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["techrequire"] != null) { if (o["techrequire"].ToString().Length > 500) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "techrequire"), 500) + Environment.NewLine; inputErr = true;}}
//				if (o["duration"] != null) { if (o["duration"].ToString().Length > 18) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "duration"), 18) + Environment.NewLine; inputErr = true;}}
//				if (o["onsitedate"] != null) { if (o["onsitedate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "onsitedate"), 0) + Environment.NewLine; inputErr = true;}}
//				if (o["commencedate"] != null) { if (o["commencedate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "commencedate"), 0) + Environment.NewLine; inputErr = true;}}
//				if (o["completiondate"] != null) { if (o["completiondate"].ToString().Length > 0) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "completiondate"), 0) + Environment.NewLine; inputErr = true;}}
//				if (o["comments"] != null) { if (o["comments"].ToString().Length > 500) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "comments"), 500) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke0"] != null) { if (o["anal_pke0"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke0"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke1"] != null) { if (o["anal_pke1"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke1"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke2"] != null) { if (o["anal_pke2"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke2"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke3"] != null) { if (o["anal_pke3"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke3"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke4"] != null) { if (o["anal_pke4"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke4"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke5"] != null) { if (o["anal_pke5"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke5"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke6"] != null) { if (o["anal_pke6"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke6"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke7"] != null) { if (o["anal_pke7"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke7"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke8"] != null) { if (o["anal_pke8"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke8"), 25) + Environment.NewLine; inputErr = true;}}
//				if (o["anal_pke9"] != null) { if (o["anal_pke9"].ToString().Length > 25) { sErr += String.Format(tMethod(this, "Length of {0} is longer than {1} character. Please check"), tMethod(this, "anal_pke9"), 25) + Environment.NewLine; inputErr = true;}}
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
            return new DTO.CpPackageItemInfo().isField(field);
        }
		public override object CreateDataSource(string tableName)
        {
            return new DTO.CpPackageItemInfo().ToDataTable(tableName);
        }
		
		public override bool import(string db, JArray p, string mode, ref string errMsg, ref string logMsg) {
            bool anyError = false,
				isTransaction = false;
            BUS.CpPackageItemControl ctr = this;

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
            string keyField = "packagecodeitemid",
				tableName = "cppackageitem";
            foreach (JObject o in p) {
                if (anyError) break;
                DTO.CpPackageItemInfo inf = null;
				if (isSeq && (o[keyField] == null || o[keyField].ToString() == "")) {
                    o[keyField] = ctrSq.genSeqCode(db, this.LD, ref seqcode, shortCmd, tableName, keyField, o, ref format);
                }
                anyError = ctr.validate(o, BUS.BUSControl.ValidType.Imp, ref errMsg);

                if (anyError) break;
                try {
                    inf = o.ToObject<DTO.CpPackageItemInfo>();
                    inf.tvcdb = db;
                    inf.updatedby = USER_ID;
                } catch (Exception ex) {
                    anyError = true;
                    logMsg = ex.Message;
                    errMsg = BUS.StLanguageControl.translate(this, "There was processing errors. Transaction ROLLBACKED");
                }

                if (anyError) break;

                if (IsExist(inf.tvcdb, inf.packagecode, inf.itemid, ref logMsg)) {
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
