using System;
using System.Collections.Generic;
using System.Text;
using DTO;
using System.Data;
using System.Data.SqlClient;

namespace DAO
{
	/// <summary> 
	///Author: nnamthach@gmail.com 
	/// <summary>
    public class CpPackageItemDataAccess : Connection
    {
		#region Local Variable
        public CpPackageItemDataAccess(string type, string connectString, int timeout = 0) : base(type, connectString, timeout) { }
        public CpPackageItemDataAccess(Connection connection) : base(connection) { }
		#endregion Local Variable
		
		#region Method        
		public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            string sql = @"SELECT  packagecode, itemid 
            FROM  cppackageitem
            WHERE  tvcdb = @tvcdb";
			
			bool hasfilters = BeginFilterCriteria(ref sql, filters);

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageItemInfo.Field.tvcdb, tvcdb);
            if (hasfilters) AddFilterCriteria(filters);
			
            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }
		
		public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0) {                  
            string sql = @"SELECT* FROM(SELECT c.*, l.locdesc
            FROM  cppackageitem c
                INNER JOIN cppackage m ON c.tvcdb = m.tvcdb AND c.packagecode = m.packagecode
                LEFT OUTER JOIN cplocation l ON c.tvcdb = l.tvcdb AND c.location = l.location AND m.project = l.project
            WHERE c.tvcdb = @tvcdb) A
            WHERE  tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);
			string sort =  GetOrderCriteria(filters);
			
			if (string.IsNullOrEmpty(sort)) sql += @" ORDER BY  tvcdb, packagecode, itemid";
			else sql += sort;
			
            if (itemPerPage != 0) {
                if (DbType == "S") sql += @" OFFSET @indexPage ROWS FETCH NEXT @itemPerPage ROWS ONLY";
                else sql += @" LIMIT @indexPage ,@itemPerPage ";
            }
            
            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageItemInfo.Field.tvcdb, tvcdb);
            if (itemPerPage != 0) {
                AddParameter("indexPage", indexPage);
                AddParameter("itemPerPage", itemPerPage);
            }

			if (hasfilters) AddFilterCriteria(filters);
			
            DataTable list = new DataTable();
            try { list = executeSelect(); }
			catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }
		
		
		public CpPackageItemInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0) {
			DataTable list = GetByFilterToDataTable(tvcdb, filters, ref sErr, indexPage, itemPerPage);

			if (!string.IsNullOrEmpty(sErr)) return null;
			
            CpPackageItemInfo[] res = null;            
			
			try
			{
				res = new CpPackageItemInfo[list.Rows.Count];
				for (int i = 0; i < list.Rows.Count; i++)
					res[i] = new CpPackageItemInfo(list.Rows[i]);
			}
			catch (Exception ex) { sErr = ex.Message; }
            
			if (!string.IsNullOrEmpty(sErr)) return null;
            return res;
        }
		
		public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            string sql = @"SELECT COUNT(*)
                FROM  cppackageitem
                WHERE  tvcdb = @tvcdb";
				
			bool hasfilters = BeginFilterCriteria(ref sql, filters);
			
			int ret = -1;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageItemInfo.Field.tvcdb, tvcdb);
          
			if (hasfilters) AddFilterCriteria(filters);
		  
            try
            {
                object tmp = executeScalar();

                if(tmp != null && tmp != DBNull.Value) ret = Convert.ToInt32(tmp);
				else ret = 0;
            }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }
			
            return ret;			
        }
		
		/// <summary>
        /// Return 1: Table is exist Identity Field
        /// Return 0: Table is not exist Identity Field
        /// Return -1: Erro
        /// </summary>
        /// <param name="tableName"></param>
        public int Add(CpPackageItemInfo objEntr, ref string sErr) {
            string sqlFields = @" INSERT INTO cppackageitem(tvcdb,packagecode,itemid,";
            string sqlValues = @" VALUES(@tvcdb,@packagecode,@itemid,";

            if (objEntr.costcode != null) { sqlFields += " costcode,"; sqlValues += " @costcode,"; }
            if (objEntr.chaptercode != null) { sqlFields += " chaptercode,"; sqlValues += " @chaptercode,"; }
            if (objEntr.chaptername != null) { sqlFields += " chaptername,"; sqlValues += " @chaptername,"; }
            if (objEntr.sectioncode != null) { sqlFields += " sectioncode,"; sqlValues += " @sectioncode,"; }
            if (objEntr.sectionname != null) { sqlFields += " sectionname,"; sqlValues += " @sectionname,"; }
            if (objEntr.groupcode != null) { sqlFields += " groupcode,"; sqlValues += " @groupcode,"; }
            if (objEntr.groupname != null) { sqlFields += " groupname,"; sqlValues += " @groupname,"; }
            if (objEntr.typecode != null) { sqlFields += " typecode,"; sqlValues += " @typecode,"; }
            if (objEntr.typename != null) { sqlFields += " typename,"; sqlValues += " @typename,"; }
            if (objEntr.itemcate != null) { sqlFields += " itemcate,"; sqlValues += " @itemcate,"; }
            if (objEntr.itemdesc != null) { sqlFields += " itemdesc,"; sqlValues += " @itemdesc,"; }
            if (objEntr.status != null) { sqlFields += " status,"; sqlValues += " @status,"; }
            if (objEntr.location != null) { sqlFields += " location,"; sqlValues += " @location,"; }
            if (objEntr.unit != null) { sqlFields += " unit,"; sqlValues += " @unit,"; }
            if (objEntr.quantity != null) { sqlFields += " quantity,"; sqlValues += " @quantity,"; }
            if (objEntr.materialprice != null) { sqlFields += " materialprice,"; sqlValues += " @materialprice,"; }
            if (objEntr.serviceprice != null) { sqlFields += " serviceprice,"; sqlValues += " @serviceprice,"; }
            if (objEntr.materialvalue != null) { sqlFields += " materialvalue,"; sqlValues += " @materialvalue,"; }
            if (objEntr.servicevalue != null) { sqlFields += " servicevalue,"; sqlValues += " @servicevalue,"; }
            if (objEntr.vatcode != null) { sqlFields += " vatcode,"; sqlValues += " @vatcode,"; }
            if (objEntr.vatrate != null) { sqlFields += " vatrate,"; sqlValues += " @vatrate,"; }
            if (objEntr.vatamt != null) { sqlFields += " vatamt,"; sqlValues += " @vatamt,"; }
            if (objEntr.totalamt != null) { sqlFields += " totalamt,"; sqlValues += " @totalamt,"; }
            if (objEntr.techrequire != null) { sqlFields += " techrequire,"; sqlValues += " @techrequire,"; }
            if (objEntr.duration != null) { sqlFields += " duration,"; sqlValues += " @duration,"; }
            if (objEntr.onsitedate != null) { sqlFields += " onsitedate,"; sqlValues += " @onsitedate,"; }
            if (objEntr.commencedate != null) { sqlFields += " commencedate,"; sqlValues += " @commencedate,"; }
            if (objEntr.completiondate != null) { sqlFields += " completiondate,"; sqlValues += " @completiondate,"; }
            if (objEntr.comments != null) { sqlFields += " comments,"; sqlValues += " @comments,"; }
            if (objEntr.anal_pke0 != null) { sqlFields += " anal_pke0,"; sqlValues += " @anal_pke0,"; }
            if (objEntr.anal_pke1 != null) { sqlFields += " anal_pke1,"; sqlValues += " @anal_pke1,"; }
            if (objEntr.anal_pke2 != null) { sqlFields += " anal_pke2,"; sqlValues += " @anal_pke2,"; }
            if (objEntr.anal_pke3 != null) { sqlFields += " anal_pke3,"; sqlValues += " @anal_pke3,"; }
            if (objEntr.anal_pke4 != null) { sqlFields += " anal_pke4,"; sqlValues += " @anal_pke4,"; }
            if (objEntr.anal_pke5 != null) { sqlFields += " anal_pke5,"; sqlValues += " @anal_pke5,"; }
            if (objEntr.anal_pke6 != null) { sqlFields += " anal_pke6,"; sqlValues += " @anal_pke6,"; }
            if (objEntr.anal_pke7 != null) { sqlFields += " anal_pke7,"; sqlValues += " @anal_pke7,"; }
            if (objEntr.anal_pke8 != null) { sqlFields += " anal_pke8,"; sqlValues += " @anal_pke8,"; }
            if (objEntr.anal_pke9 != null) { sqlFields += " anal_pke9,"; sqlValues += " @anal_pke9,"; }
            if (objEntr.extreference1 != null) { sqlFields += " extreference1,"; sqlValues += " @extreference1,"; }
            if (objEntr.extreference2 != null) { sqlFields += " extreference2,"; sqlValues += " @extreference2,"; }
            if (objEntr.extreference3 != null) { sqlFields += " extreference3,"; sqlValues += " @extreference3,"; }
            if (objEntr.extreference4 != null) { sqlFields += " extreference4,"; sqlValues += " @extreference4,"; }
            if (objEntr.extdate1 != null) { sqlFields += " extdate1,"; sqlValues += " @extdate1,"; }
            if (objEntr.extdate2 != null) { sqlFields += " extdate2,"; sqlValues += " @extdate2,"; }
            if (objEntr.extdate3 != null) { sqlFields += " extdate3,"; sqlValues += " @extdate3,"; }
            if (objEntr.extdate4 != null) { sqlFields += " extdate4,"; sqlValues += " @extdate4,"; }
            if (objEntr.extnumber1 != null) { sqlFields += " extnumber1,"; sqlValues += " @extnumber1,"; }
            if (objEntr.extnumber2 != null) { sqlFields += " extnumber2,"; sqlValues += " @extnumber2,"; }
            if (objEntr.extnumber3 != null) { sqlFields += " extnumber3,"; sqlValues += " @extnumber3,"; }
            if (objEntr.extnumber4 != null) { sqlFields += " extnumber4,"; sqlValues += " @extnumber4,"; }
            if (objEntr.extdescription1 != null) { sqlFields += " extdescription1,"; sqlValues += " @extdescription1,"; }
            if (objEntr.extdescription2 != null) { sqlFields += " extdescription2,"; sqlValues += " @extdescription2,"; }
            if (objEntr.extdescription3 != null) { sqlFields += " extdescription3,"; sqlValues += " @extdescription3,"; }
            if (objEntr.extdescription4 != null) { sqlFields += " extdescription4,"; sqlValues += " @extdescription4,"; }
			if (objEntr.createdby != null) { sqlFields += " createdby,"; sqlValues += " @createdby,"; }
			if (objEntr.updatedby != null) { sqlFields += " updatedby,"; sqlValues += " @updatedby,"; }

            sqlFields = sqlFields.Substring(0, sqlFields.Length - 1) + ")";
            sqlValues = sqlValues.Substring(0, sqlValues.Length - 1) + ")";

            int ret = -1;
			string sql = sqlFields + sqlValues;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageItemInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageItemInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageItemInfo.Field.itemid, objEntr.itemid);

            if (objEntr.costcode != null) AddParameter(CpPackageItemInfo.Field.costcode, objEntr.costcode);
            if (objEntr.chaptercode != null) AddParameter(CpPackageItemInfo.Field.chaptercode, objEntr.chaptercode);
            if (objEntr.chaptername != null) AddParameter(CpPackageItemInfo.Field.chaptername, objEntr.chaptername);
            if (objEntr.sectioncode != null) AddParameter(CpPackageItemInfo.Field.sectioncode, objEntr.sectioncode);
            if (objEntr.sectionname != null) AddParameter(CpPackageItemInfo.Field.sectionname, objEntr.sectionname);
            if (objEntr.groupcode != null) AddParameter(CpPackageItemInfo.Field.groupcode, objEntr.groupcode);
            if (objEntr.groupname != null) AddParameter(CpPackageItemInfo.Field.groupname, objEntr.groupname);
            if (objEntr.typecode != null) AddParameter(CpPackageItemInfo.Field.typecode, objEntr.typecode);
            if (objEntr.typename != null) AddParameter(CpPackageItemInfo.Field.typename, objEntr.typename);
            if (objEntr.itemcate != null) AddParameter(CpPackageItemInfo.Field.itemcate, objEntr.itemcate);         
            if (objEntr.itemdesc != null) AddParameter(CpPackageItemInfo.Field.itemdesc, objEntr.itemdesc);         
            if (objEntr.status != null) AddParameter(CpPackageItemInfo.Field.status, objEntr.status);         
            if (objEntr.location != null) AddParameter(CpPackageItemInfo.Field.location, objEntr.location);         
            if (objEntr.unit != null) AddParameter(CpPackageItemInfo.Field.unit, objEntr.unit);         
            if (objEntr.quantity != null) AddParameter(CpPackageItemInfo.Field.quantity, objEntr.quantity);         
            if (objEntr.materialprice != null) AddParameter(CpPackageItemInfo.Field.materialprice, objEntr.materialprice);         
            if (objEntr.serviceprice != null) AddParameter(CpPackageItemInfo.Field.serviceprice, objEntr.serviceprice);         
            if (objEntr.materialvalue != null) AddParameter(CpPackageItemInfo.Field.materialvalue, objEntr.materialvalue);         
            if (objEntr.servicevalue != null) AddParameter(CpPackageItemInfo.Field.servicevalue, objEntr.servicevalue);         
            if (objEntr.vatcode != null) AddParameter(CpPackageItemInfo.Field.vatcode, objEntr.vatcode);         
            if (objEntr.vatrate != null) AddParameter(CpPackageItemInfo.Field.vatrate, objEntr.vatrate);         
            if (objEntr.vatamt != null) AddParameter(CpPackageItemInfo.Field.vatamt, objEntr.vatamt);         
            if (objEntr.totalamt != null) AddParameter(CpPackageItemInfo.Field.totalamt, objEntr.totalamt);         
            if (objEntr.techrequire != null) AddParameter(CpPackageItemInfo.Field.techrequire, objEntr.techrequire);         
            if (objEntr.duration != null) AddParameter(CpPackageItemInfo.Field.duration, objEntr.duration);         
            if (objEntr.onsitedate != null) AddParameter(CpPackageItemInfo.Field.onsitedate, objEntr.onsitedate);         
            if (objEntr.commencedate != null) AddParameter(CpPackageItemInfo.Field.commencedate, objEntr.commencedate);         
            if (objEntr.completiondate != null) AddParameter(CpPackageItemInfo.Field.completiondate, objEntr.completiondate);         
            if (objEntr.comments != null) AddParameter(CpPackageItemInfo.Field.comments, objEntr.comments);         
            if (objEntr.anal_pke0 != null) AddParameter(CpPackageItemInfo.Field.anal_pke0, objEntr.anal_pke0);         
            if (objEntr.anal_pke1 != null) AddParameter(CpPackageItemInfo.Field.anal_pke1, objEntr.anal_pke1);         
            if (objEntr.anal_pke2 != null) AddParameter(CpPackageItemInfo.Field.anal_pke2, objEntr.anal_pke2);         
            if (objEntr.anal_pke3 != null) AddParameter(CpPackageItemInfo.Field.anal_pke3, objEntr.anal_pke3);         
            if (objEntr.anal_pke4 != null) AddParameter(CpPackageItemInfo.Field.anal_pke4, objEntr.anal_pke4);         
            if (objEntr.anal_pke5 != null) AddParameter(CpPackageItemInfo.Field.anal_pke5, objEntr.anal_pke5);         
            if (objEntr.anal_pke6 != null) AddParameter(CpPackageItemInfo.Field.anal_pke6, objEntr.anal_pke6);         
            if (objEntr.anal_pke7 != null) AddParameter(CpPackageItemInfo.Field.anal_pke7, objEntr.anal_pke7);         
            if (objEntr.anal_pke8 != null) AddParameter(CpPackageItemInfo.Field.anal_pke8, objEntr.anal_pke8);         
            if (objEntr.anal_pke9 != null) AddParameter(CpPackageItemInfo.Field.anal_pke9, objEntr.anal_pke9);         
            if (objEntr.extreference1 != null) AddParameter(CpPackageItemInfo.Field.extreference1, objEntr.extreference1);         
            if (objEntr.extreference2 != null) AddParameter(CpPackageItemInfo.Field.extreference2, objEntr.extreference2);         
            if (objEntr.extreference3 != null) AddParameter(CpPackageItemInfo.Field.extreference3, objEntr.extreference3);         
            if (objEntr.extreference4 != null) AddParameter(CpPackageItemInfo.Field.extreference4, objEntr.extreference4);         
            if (objEntr.extdate1 != null) AddParameter(CpPackageItemInfo.Field.extdate1, objEntr.extdate1);         
            if (objEntr.extdate2 != null) AddParameter(CpPackageItemInfo.Field.extdate2, objEntr.extdate2);         
            if (objEntr.extdate3 != null) AddParameter(CpPackageItemInfo.Field.extdate3, objEntr.extdate3);         
            if (objEntr.extdate4 != null) AddParameter(CpPackageItemInfo.Field.extdate4, objEntr.extdate4);         
            if (objEntr.extnumber1 != null) AddParameter(CpPackageItemInfo.Field.extnumber1, objEntr.extnumber1);         
            if (objEntr.extnumber2 != null) AddParameter(CpPackageItemInfo.Field.extnumber2, objEntr.extnumber2);         
            if (objEntr.extnumber3 != null) AddParameter(CpPackageItemInfo.Field.extnumber3, objEntr.extnumber3);         
            if (objEntr.extnumber4 != null) AddParameter(CpPackageItemInfo.Field.extnumber4, objEntr.extnumber4);         
            if (objEntr.extdescription1 != null) AddParameter(CpPackageItemInfo.Field.extdescription1, objEntr.extdescription1);         
            if (objEntr.extdescription2 != null) AddParameter(CpPackageItemInfo.Field.extdescription2, objEntr.extdescription2);         
            if (objEntr.extdescription3 != null) AddParameter(CpPackageItemInfo.Field.extdescription3, objEntr.extdescription3);         
            if (objEntr.extdescription4 != null) AddParameter(CpPackageItemInfo.Field.extdescription4, objEntr.extdescription4);         
			if (objEntr.createdby != null) AddParameter(CpPackageItemInfo.Field.createdby, objEntr.createdby);
			if (objEntr.updatedby != null) AddParameter(CpPackageItemInfo.Field.updatedby, objEntr.updatedby);
          
            try
            {
                object tmp = executeScalar();
                if(tmp != null && tmp != DBNull.Value) ret = Convert.ToInt32(tmp);
				else ret = 0;
            }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return ret;
        }

        public string Update(CpPackageItemInfo objEntr) {
            string sql = @"UPDATE cppackageitem SET ";

            if (objEntr.chaptercode != null) { sql += "chaptercode = @chaptercode,"; }
            if (objEntr.chaptername != null) { sql += "chaptername = @chaptername,"; }
            if (objEntr.sectioncode != null) { sql += "sectioncode = @sectioncode,"; }
            if (objEntr.sectionname != null) { sql += "sectionname = @sectionname,"; }
            if (objEntr.groupcode != null) { sql += "groupcode = @groupcode,"; }
            if (objEntr.groupname != null) { sql += "groupname = @groupname,"; }
            if (objEntr.typecode != null) { sql += "typecode = @typecode,"; }
            if (objEntr.typename != null) { sql += "typename = @typename,"; }
            if (objEntr.itemcate != null) { sql += "itemcate = @itemcate,"; }
            if (objEntr.itemdesc != null) { sql += "itemdesc = @itemdesc,"; }
            if (objEntr.status != null) { sql += "status = @status,"; }
            if (objEntr.location != null) { sql += "location = @location,"; }
            if (objEntr.unit != null) { sql += "unit = @unit,"; }
            if (objEntr.quantity != null) { sql += "quantity = @quantity,"; }
            if (objEntr.materialprice != null) { sql += "materialprice = @materialprice,"; }
            if (objEntr.serviceprice != null) { sql += "serviceprice = @serviceprice,"; }
            if (objEntr.materialvalue != null) { sql += "materialvalue = @materialvalue,"; }
            if (objEntr.servicevalue != null) { sql += "servicevalue = @servicevalue,"; }
            if (objEntr.vatcode != null) { sql += "vatcode = @vatcode,"; }
            if (objEntr.vatrate != null) { sql += "vatrate = @vatrate,"; }
            if (objEntr.vatamt != null) { sql += "vatamt = @vatamt,"; }
            if (objEntr.totalamt != null) { sql += "totalamt = @totalamt,"; }
            if (objEntr.techrequire != null) { sql += "techrequire = @techrequire,"; }
            if (objEntr.duration != null) { sql += "duration = @duration,"; }
            if (objEntr.onsitedate != null) { sql += "onsitedate = @onsitedate,"; }
            if (objEntr.commencedate != null) { sql += "commencedate = @commencedate,"; }
            if (objEntr.completiondate != null) { sql += "completiondate = @completiondate,"; }
            if (objEntr.comments != null) { sql += "comments = @comments,"; }
            if (objEntr.anal_pke0 != null) { sql += "anal_pke0 = @anal_pke0,"; }
            if (objEntr.anal_pke1 != null) { sql += "anal_pke1 = @anal_pke1,"; }
            if (objEntr.anal_pke2 != null) { sql += "anal_pke2 = @anal_pke2,"; }
            if (objEntr.anal_pke3 != null) { sql += "anal_pke3 = @anal_pke3,"; }
            if (objEntr.anal_pke4 != null) { sql += "anal_pke4 = @anal_pke4,"; }
            if (objEntr.anal_pke5 != null) { sql += "anal_pke5 = @anal_pke5,"; }
            if (objEntr.anal_pke6 != null) { sql += "anal_pke6 = @anal_pke6,"; }
            if (objEntr.anal_pke7 != null) { sql += "anal_pke7 = @anal_pke7,"; }
            if (objEntr.anal_pke8 != null) { sql += "anal_pke8 = @anal_pke8,"; }
            if (objEntr.anal_pke9 != null) { sql += "anal_pke9 = @anal_pke9,"; }
            if (objEntr.extreference1 != null) { sql += "extreference1 = @extreference1,"; }
            if (objEntr.extreference2 != null) { sql += "extreference2 = @extreference2,"; }
            if (objEntr.extreference3 != null) { sql += "extreference3 = @extreference3,"; }
            if (objEntr.extreference4 != null) { sql += "extreference4 = @extreference4,"; }
            if (objEntr.extdate1 != null) { sql += "extdate1 = @extdate1,"; }
            if (objEntr.extdate2 != null) { sql += "extdate2 = @extdate2,"; }
            if (objEntr.extdate3 != null) { sql += "extdate3 = @extdate3,"; }
            if (objEntr.extdate4 != null) { sql += "extdate4 = @extdate4,"; }
            if (objEntr.extnumber1 != null) { sql += "extnumber1 = @extnumber1,"; }
            if (objEntr.extnumber2 != null) { sql += "extnumber2 = @extnumber2,"; }
            if (objEntr.extnumber3 != null) { sql += "extnumber3 = @extnumber3,"; }
            if (objEntr.extnumber4 != null) { sql += "extnumber4 = @extnumber4,"; }
            if (objEntr.extdescription1 != null) { sql += "extdescription1 = @extdescription1,"; }
            if (objEntr.extdescription2 != null) { sql += "extdescription2 = @extdescription2,"; }
            if (objEntr.extdescription3 != null) { sql += "extdescription3 = @extdescription3,"; }
            if (objEntr.extdescription4 != null) { sql += "extdescription4 = @extdescription4,"; }
			if (objEntr.updatedby != null) { sql += "updatedby = @updatedby,"; }

			sql += " lastupdate = getdate()";
            sql += @" WHERE  tvcdb = @tvcdb AND packagecode = @packagecode AND itemid = @itemid";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageItemInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageItemInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageItemInfo.Field.itemid, objEntr.itemid);

            if (objEntr.chaptercode != null) AddParameter(CpPackageItemInfo.Field.chaptercode, objEntr.chaptercode);
            if (objEntr.chaptername != null) AddParameter(CpPackageItemInfo.Field.chaptername, objEntr.chaptername);
            if (objEntr.sectioncode != null) AddParameter(CpPackageItemInfo.Field.sectioncode, objEntr.sectioncode);
            if (objEntr.sectionname != null) AddParameter(CpPackageItemInfo.Field.sectionname, objEntr.sectionname);
            if (objEntr.groupcode != null) AddParameter(CpPackageItemInfo.Field.groupcode, objEntr.groupcode);
            if (objEntr.groupname != null) AddParameter(CpPackageItemInfo.Field.groupname, objEntr.groupname);
            if (objEntr.typename != null) AddParameter(CpPackageItemInfo.Field.typename, objEntr.typename);
            if (objEntr.typecode != null) AddParameter(CpPackageItemInfo.Field.typecode, objEntr.typecode);
            if (objEntr.itemcate != null) AddParameter(CpPackageItemInfo.Field.itemcate, objEntr.itemcate);         
            if (objEntr.itemdesc != null) AddParameter(CpPackageItemInfo.Field.itemdesc, objEntr.itemdesc);         
            if (objEntr.status != null) AddParameter(CpPackageItemInfo.Field.status, objEntr.status);         
            if (objEntr.location != null) AddParameter(CpPackageItemInfo.Field.location, objEntr.location);         
            if (objEntr.unit != null) AddParameter(CpPackageItemInfo.Field.unit, objEntr.unit);         
            if (objEntr.quantity != null) AddParameter(CpPackageItemInfo.Field.quantity, objEntr.quantity);         
            if (objEntr.materialprice != null) AddParameter(CpPackageItemInfo.Field.materialprice, objEntr.materialprice);         
            if (objEntr.serviceprice != null) AddParameter(CpPackageItemInfo.Field.serviceprice, objEntr.serviceprice);         
            if (objEntr.materialvalue != null) AddParameter(CpPackageItemInfo.Field.materialvalue, objEntr.materialvalue);         
            if (objEntr.servicevalue != null) AddParameter(CpPackageItemInfo.Field.servicevalue, objEntr.servicevalue);         
            if (objEntr.vatcode != null) AddParameter(CpPackageItemInfo.Field.vatcode, objEntr.vatcode);         
            if (objEntr.vatrate != null) AddParameter(CpPackageItemInfo.Field.vatrate, objEntr.vatrate);         
            if (objEntr.vatamt != null) AddParameter(CpPackageItemInfo.Field.vatamt, objEntr.vatamt);         
            if (objEntr.totalamt != null) AddParameter(CpPackageItemInfo.Field.totalamt, objEntr.totalamt);         
            if (objEntr.techrequire != null) AddParameter(CpPackageItemInfo.Field.techrequire, objEntr.techrequire);         
            if (objEntr.duration != null) AddParameter(CpPackageItemInfo.Field.duration, objEntr.duration);         
            if (objEntr.onsitedate != null) AddParameter(CpPackageItemInfo.Field.onsitedate, objEntr.onsitedate);         
            if (objEntr.commencedate != null) AddParameter(CpPackageItemInfo.Field.commencedate, objEntr.commencedate);         
            if (objEntr.completiondate != null) AddParameter(CpPackageItemInfo.Field.completiondate, objEntr.completiondate);         
            if (objEntr.comments != null) AddParameter(CpPackageItemInfo.Field.comments, objEntr.comments);         
            if (objEntr.anal_pke0 != null) AddParameter(CpPackageItemInfo.Field.anal_pke0, objEntr.anal_pke0);         
            if (objEntr.anal_pke1 != null) AddParameter(CpPackageItemInfo.Field.anal_pke1, objEntr.anal_pke1);         
            if (objEntr.anal_pke2 != null) AddParameter(CpPackageItemInfo.Field.anal_pke2, objEntr.anal_pke2);         
            if (objEntr.anal_pke3 != null) AddParameter(CpPackageItemInfo.Field.anal_pke3, objEntr.anal_pke3);         
            if (objEntr.anal_pke4 != null) AddParameter(CpPackageItemInfo.Field.anal_pke4, objEntr.anal_pke4);         
            if (objEntr.anal_pke5 != null) AddParameter(CpPackageItemInfo.Field.anal_pke5, objEntr.anal_pke5);         
            if (objEntr.anal_pke6 != null) AddParameter(CpPackageItemInfo.Field.anal_pke6, objEntr.anal_pke6);         
            if (objEntr.anal_pke7 != null) AddParameter(CpPackageItemInfo.Field.anal_pke7, objEntr.anal_pke7);         
            if (objEntr.anal_pke8 != null) AddParameter(CpPackageItemInfo.Field.anal_pke8, objEntr.anal_pke8);         
            if (objEntr.anal_pke9 != null) AddParameter(CpPackageItemInfo.Field.anal_pke9, objEntr.anal_pke9);         
            if (objEntr.extreference1 != null) AddParameter(CpPackageItemInfo.Field.extreference1, objEntr.extreference1);         
            if (objEntr.extreference2 != null) AddParameter(CpPackageItemInfo.Field.extreference2, objEntr.extreference2);         
            if (objEntr.extreference3 != null) AddParameter(CpPackageItemInfo.Field.extreference3, objEntr.extreference3);         
            if (objEntr.extreference4 != null) AddParameter(CpPackageItemInfo.Field.extreference4, objEntr.extreference4);         
            if (objEntr.extdate1 != null) AddParameter(CpPackageItemInfo.Field.extdate1, objEntr.extdate1);         
            if (objEntr.extdate2 != null) AddParameter(CpPackageItemInfo.Field.extdate2, objEntr.extdate2);         
            if (objEntr.extdate3 != null) AddParameter(CpPackageItemInfo.Field.extdate3, objEntr.extdate3);         
            if (objEntr.extdate4 != null) AddParameter(CpPackageItemInfo.Field.extdate4, objEntr.extdate4);         
            if (objEntr.extnumber1 != null) AddParameter(CpPackageItemInfo.Field.extnumber1, objEntr.extnumber1);         
            if (objEntr.extnumber2 != null) AddParameter(CpPackageItemInfo.Field.extnumber2, objEntr.extnumber2);         
            if (objEntr.extnumber3 != null) AddParameter(CpPackageItemInfo.Field.extnumber3, objEntr.extnumber3);         
            if (objEntr.extnumber4 != null) AddParameter(CpPackageItemInfo.Field.extnumber4, objEntr.extnumber4);         
            if (objEntr.extdescription1 != null) AddParameter(CpPackageItemInfo.Field.extdescription1, objEntr.extdescription1);         
            if (objEntr.extdescription2 != null) AddParameter(CpPackageItemInfo.Field.extdescription2, objEntr.extdescription2);         
            if (objEntr.extdescription3 != null) AddParameter(CpPackageItemInfo.Field.extdescription3, objEntr.extdescription3);         
            if (objEntr.extdescription4 != null) AddParameter(CpPackageItemInfo.Field.extdescription4, objEntr.extdescription4);         
			if (objEntr.updatedby != null) AddParameter(CpPackageItemInfo.Field.updatedby, objEntr.updatedby);          
               
            string sErr = string.Empty;

            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

        public string Delete(String tvcdb, String packagecode, String itemid) {
            string sql = @"DELETE cppackageitem
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode AND itemid = @itemid";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageItemInfo.Field.tvcdb, tvcdb);
            AddParameter(CpPackageItemInfo.Field.packagecode, packagecode);
            AddParameter(CpPackageItemInfo.Field.itemid, itemid);
              
            string sErr = string.Empty;
            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

		public string Clear(string db) {
            string sql = @"IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='cppackageitem')
			DELETE cppackageitem
            WHERE 	tvcdb = @tvcdb";

            openConnection();
            InitCommand(sql);

            AddParameter("tvcdb", db);
              
            string sErr = string.Empty;
            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }		
        
        public Boolean IsExist(String tvcdb, String packagecode, String itemid, ref string sErr) {
            string sql = @"SELECT TOP (1) tvcdb, packagecode, itemid
            FROM  cppackageitem
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode AND itemid = @itemid";
			
            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageItemInfo.Field.tvcdb, tvcdb);              
            AddParameter(CpPackageItemInfo.Field.packagecode, packagecode);              
            AddParameter(CpPackageItemInfo.Field.itemid, itemid);              
            
            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = ex.Message + " sql='" + sql + "'"; }

			return list.Rows.Count == 1;
        }		
		
		#endregion Method
     
    }
}
