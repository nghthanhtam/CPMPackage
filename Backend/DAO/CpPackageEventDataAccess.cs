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
    public class CpPackageEventDataAccess : Connection
    {
		#region Local Variable
        public CpPackageEventDataAccess(string type, string connectString, int timeout = 0) : base(type, connectString, timeout) { }
        public CpPackageEventDataAccess(Connection connection) : base(connection) { }
		#endregion Local Variable
		
		#region Method        
		public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            string sql = @"SELECT  packagecode, eventid 
            FROM  cppackageevent
            WHERE  tvcdb = @tvcdb";
			
			bool hasfilters = BeginFilterCriteria(ref sql, filters);

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageEventInfo.Field.tvcdb, tvcdb);
            if (hasfilters) AddFilterCriteria(filters);
			
            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }
		
		public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0) {
            string sql = @"SELECT * FROM (SELECT p.*, c.name AS eventname, m.name AS statusname
                        FROM cppackageevent p
                        LEFT OUTER JOIN cscodedictionary c ON p.tvcdb = c.tvcdb AND p.eventtype = c.code AND c.category = 'CPKETYPE'
                        LEFT OUTER JOIN cscodedictionary m ON p.tvcdb = m.tvcdb AND p.eventstatus = m.code AND m.category = 'CPKESTATUS'
                        WHERE p.tvcdb = @tvcdb) AS A
                        WHERE tvcdb = @tvcdb";
   

            bool hasfilters = BeginFilterCriteria(ref sql, filters);
			string sort =  GetOrderCriteria(filters);
			
			if (string.IsNullOrEmpty(sort)) sql += @" ORDER BY  tvcdb, packagecode, eventid";
			else sql += sort;
			
            if (itemPerPage != 0) {
                if (DbType == "S") sql += @" OFFSET @indexPage ROWS FETCH NEXT @itemPerPage ROWS ONLY";
                else sql += @" LIMIT @indexPage ,@itemPerPage ";
            }
            
            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageEventInfo.Field.tvcdb, tvcdb);
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
		
		
		public CpPackageEventInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0) {
			DataTable list = GetByFilterToDataTable(tvcdb, filters, ref sErr, indexPage, itemPerPage);

			if (!string.IsNullOrEmpty(sErr)) return null;
			
            CpPackageEventInfo[] res = null;            
			
			try
			{
				res = new CpPackageEventInfo[list.Rows.Count];
				for (int i = 0; i < list.Rows.Count; i++)
					res[i] = new CpPackageEventInfo(list.Rows[i]);
			}
			catch (Exception ex) { sErr = ex.Message; }
            
			if (!string.IsNullOrEmpty(sErr)) return null;
            return res;
        }
		
		public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            string sql = @"SELECT COUNT(*)
                FROM  cppackageevent
                WHERE  tvcdb = @tvcdb";
				
			bool hasfilters = BeginFilterCriteria(ref sql, filters);
			
			int ret = -1;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageEventInfo.Field.tvcdb, tvcdb);
          
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
        public int Add(CpPackageEventInfo objEntr, ref string sErr) {
            string sqlFields = @" INSERT INTO cppackageevent(tvcdb,packagecode,eventid,";
            string sqlValues = @" VALUES(@tvcdb,@packagecode,@eventid,";

            if (objEntr.eventtype != null) { sqlFields += " eventtype,"; sqlValues += " @eventtype,"; }
            if (objEntr.startdate != null) { sqlFields += " startdate,"; sqlValues += " @startdate,"; }
            if (objEntr.enddate != null) { sqlFields += " enddate,"; sqlValues += " @enddate,"; }
            if (objEntr.eventstatus != null) { sqlFields += " eventstatus,"; sqlValues += " @eventstatus,"; }
            if (objEntr.lookup != null) { sqlFields += " lookup,"; sqlValues += " @lookup,"; }
            if (objEntr.pic != null) { sqlFields += " pic,"; sqlValues += " @pic,"; }
            if (objEntr.notes != null) { sqlFields += " notes,"; sqlValues += " @notes,"; }
			if (objEntr.createdby != null) { sqlFields += " createdby,"; sqlValues += " @createdby,"; }
			if (objEntr.updatedby != null) { sqlFields += " updatedby,"; sqlValues += " @updatedby,"; }

            sqlFields = sqlFields.Substring(0, sqlFields.Length - 1) + ")";
            sqlValues = sqlValues.Substring(0, sqlValues.Length - 1) + ")";

            int ret = -1;
			string sql = sqlFields + sqlValues;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageEventInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageEventInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageEventInfo.Field.eventid, objEntr.eventid);

            if (objEntr.eventtype != null) AddParameter(CpPackageEventInfo.Field.eventtype, objEntr.eventtype);         
            if (objEntr.startdate != null) AddParameter(CpPackageEventInfo.Field.startdate, objEntr.startdate);         
            if (objEntr.enddate != null) AddParameter(CpPackageEventInfo.Field.enddate, objEntr.enddate);         
            if (objEntr.eventstatus != null) AddParameter(CpPackageEventInfo.Field.eventstatus, objEntr.eventstatus);         
            if (objEntr.lookup != null) AddParameter(CpPackageEventInfo.Field.lookup, objEntr.lookup);         
            if (objEntr.pic != null) AddParameter(CpPackageEventInfo.Field.pic, objEntr.pic);         
            if (objEntr.notes != null) AddParameter(CpPackageEventInfo.Field.notes, objEntr.notes);         
			if (objEntr.createdby != null) AddParameter(CpPackageEventInfo.Field.createdby, objEntr.createdby);
			if (objEntr.updatedby != null) AddParameter(CpPackageEventInfo.Field.updatedby, objEntr.updatedby);
          
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

        public string Update(CpPackageEventInfo objEntr) {
            string sql = @"UPDATE cppackageevent SET ";
			
            if (objEntr.eventtype != null) { sql += "eventtype = @eventtype,"; }
            if (objEntr.startdate != null) { sql += "startdate = @startdate,"; }
            if (objEntr.enddate != null) { sql += "enddate = @enddate,"; }
            if (objEntr.eventstatus != null) { sql += "eventstatus = @eventstatus,"; }
            if (objEntr.lookup != null) { sql += "lookup = @lookup,"; }
            if (objEntr.pic != null) { sql += "pic = @pic,"; }
            if (objEntr.notes != null) { sql += "notes = @notes,"; }
			if (objEntr.updatedby != null) { sql += "updatedby = @updatedby,"; }

			sql += " lastupdate = getdate()";
            sql += @" WHERE  tvcdb = @tvcdb AND packagecode = @packagecode AND eventid = @eventid";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageEventInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageEventInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageEventInfo.Field.eventid, objEntr.eventid);

            if (objEntr.eventtype != null) AddParameter(CpPackageEventInfo.Field.eventtype, objEntr.eventtype);         
            if (objEntr.startdate != null) AddParameter(CpPackageEventInfo.Field.startdate, objEntr.startdate);         
            if (objEntr.enddate != null) AddParameter(CpPackageEventInfo.Field.enddate, objEntr.enddate);         
            if (objEntr.eventstatus != null) AddParameter(CpPackageEventInfo.Field.eventstatus, objEntr.eventstatus);         
            if (objEntr.lookup != null) AddParameter(CpPackageEventInfo.Field.lookup, objEntr.lookup);         
            if (objEntr.pic != null) AddParameter(CpPackageEventInfo.Field.pic, objEntr.pic);         
            if (objEntr.notes != null) AddParameter(CpPackageEventInfo.Field.notes, objEntr.notes);         
			if (objEntr.updatedby != null) AddParameter(CpPackageEventInfo.Field.updatedby, objEntr.updatedby);          
               
            string sErr = string.Empty;

            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

        public string Delete(String tvcdb, String packagecode, String eventid) {
            string sql = @"DELETE cppackageevent
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode AND eventid = @eventid";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageEventInfo.Field.tvcdb, tvcdb);
            AddParameter(CpPackageEventInfo.Field.packagecode, packagecode);
            AddParameter(CpPackageEventInfo.Field.eventid, eventid);
              
            string sErr = string.Empty;
            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

		public string Clear(string db) {
            string sql = @"IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='cppackageevent')
			DELETE cppackageevent
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
        
        public Boolean IsExist(String tvcdb, String packagecode, String eventid, ref string sErr) {
            string sql = @"SELECT TOP (1) tvcdb, packagecode, eventid
            FROM  cppackageevent
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode AND eventid = @eventid";
			
            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageEventInfo.Field.tvcdb, tvcdb);              
            AddParameter(CpPackageEventInfo.Field.packagecode, packagecode);              
            AddParameter(CpPackageEventInfo.Field.eventid, eventid);              
            
            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = ex.Message + " sql='" + sql + "'"; }

			return list.Rows.Count == 1;
        }		
		
		#endregion Method
     
    }
}
