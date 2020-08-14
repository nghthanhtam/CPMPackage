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
    public class CpPackageValueDataAccess : Connection
    {
        #region Local Variable
        public CpPackageValueDataAccess(string type, string connectString, int timeout = 0) : base(type, connectString, timeout) { }
        public CpPackageValueDataAccess(Connection connection) : base(connection) { }
        #endregion Local Variable

        #region Method        
        public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            string sql = @"SELECT  packagecode, valueid 
            FROM  cppackagevalue
            WHERE  tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageValueInfo.Field.tvcdb, tvcdb);
            if (hasfilters) AddFilterCriteria(filters);

            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }

        public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0)
        {
            string sql = @"SELECT * FROM (SELECT  p.*, e.name as empname, CASE WHEN original = 'Y' THEN 1 ELSE 0 END AS ckoriginal
                FROM  cppackagevalue p LEFT OUTER JOIN hremployee e ON p.tvcdb = e.tvcdb AND p.employee = e.employee
                WHERE  p.tvcdb = @tvcdb) AS A
            WHERE  tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);
            string sort = GetOrderCriteria(filters);

            if (string.IsNullOrEmpty(sort)) sql += @" ORDER BY  tvcdb, packagecode, valueid";
            else sql += sort;

            if (itemPerPage != 0)
            {
                if (DbType == "S") sql += @" OFFSET @indexPage ROWS FETCH NEXT @itemPerPage ROWS ONLY";
                else sql += @" LIMIT @indexPage ,@itemPerPage ";
            }

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageValueInfo.Field.tvcdb, tvcdb);
            if (itemPerPage != 0)
            {
                AddParameter("indexPage", indexPage);
                AddParameter("itemPerPage", itemPerPage);
            }

            if (hasfilters) AddFilterCriteria(filters);

            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }


        public CpPackageValueInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0)
        {
            DataTable list = GetByFilterToDataTable(tvcdb, filters, ref sErr, indexPage, itemPerPage);

            if (!string.IsNullOrEmpty(sErr)) return null;

            CpPackageValueInfo[] res = null;

            try
            {
                res = new CpPackageValueInfo[list.Rows.Count];
                for (int i = 0; i < list.Rows.Count; i++)
                    res[i] = new CpPackageValueInfo(list.Rows[i]);
            }
            catch (Exception ex) { sErr = ex.Message; }

            if (!string.IsNullOrEmpty(sErr)) return null;
            return res;
        }

        public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            string sql = @"SELECT COUNT(*)
                FROM  cppackagevalue
                WHERE  tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);

            int ret = -1;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageValueInfo.Field.tvcdb, tvcdb);

            if (hasfilters) AddFilterCriteria(filters);

            try
            {
                object tmp = executeScalar();

                if (tmp != null && tmp != DBNull.Value) ret = Convert.ToInt32(tmp);
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
        public int Add(CpPackageValueInfo objEntr, ref string sErr)
        {
            string sqlFields = @" INSERT INTO cppackagevalue(tvcdb,packagecode,valueid,";
            string sqlValues = @" VALUES(@tvcdb,@packagecode,@valueid,";

            if (objEntr.transdate != null) { sqlFields += " transdate,"; sqlValues += " @transdate,"; }
            if (objEntr.employee != null) { sqlFields += " employee,"; sqlValues += " @employee,"; }
            if (objEntr.valuedesc != null) { sqlFields += " valuedesc,"; sqlValues += " @valuedesc,"; }
            if (objEntr.amount != null) { sqlFields += " amount,"; sqlValues += " @amount,"; }
            if (objEntr.original != null) { sqlFields += " original,"; sqlValues += " @original,"; }
            if (objEntr.status != null) { sqlFields += " status,"; sqlValues += " @status,"; }
            if (objEntr.comments != null) { sqlFields += " comments,"; sqlValues += " @comments,"; }
            if (objEntr.approvalstatus != null) { sqlFields += " approvalstatus,"; sqlValues += " @approvalstatus,"; }
            if (objEntr.approvedby != null) { sqlFields += " approvedby,"; sqlValues += " @approvedby,"; }
            if (objEntr.approveddate != null) { sqlFields += " approveddate,"; sqlValues += " @approveddate,"; }
            if (objEntr.approvednote != null) { sqlFields += " approvednote,"; sqlValues += " @approvednote,"; }
            if (objEntr.adjustno != null) { sqlFields += " adjustno,"; sqlValues += " @adjustno,"; }
            if (objEntr.approvedref != null) { sqlFields += " approvedref,"; sqlValues += " @approvedref,"; }
            if (objEntr.createdby != null) { sqlFields += " createdby,"; sqlValues += " @createdby,"; }
            if (objEntr.updatedby != null) { sqlFields += " updatedby,"; sqlValues += " @updatedby,"; }

            sqlFields = sqlFields.Substring(0, sqlFields.Length - 1) + ")";
            sqlValues = sqlValues.Substring(0, sqlValues.Length - 1) + ")";

            int ret = -1;
            string sql = sqlFields + sqlValues;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageValueInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageValueInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageValueInfo.Field.valueid, objEntr.valueid);

            if (objEntr.transdate != null) AddParameter(CpPackageValueInfo.Field.transdate, objEntr.transdate);
            if (objEntr.employee != null) AddParameter(CpPackageValueInfo.Field.employee, objEntr.employee);
            if (objEntr.valuedesc != null) AddParameter(CpPackageValueInfo.Field.valuedesc, objEntr.valuedesc);
            if (objEntr.amount != null) AddParameter(CpPackageValueInfo.Field.amount, objEntr.amount);
            if (objEntr.original != null) AddParameter(CpPackageValueInfo.Field.original, objEntr.original);
            if (objEntr.status != null) AddParameter(CpPackageValueInfo.Field.status, objEntr.status);
            if (objEntr.comments != null) AddParameter(CpPackageValueInfo.Field.comments, objEntr.comments);
            if (objEntr.approvalstatus != null) AddParameter(CpPackageValueInfo.Field.approvalstatus, objEntr.approvalstatus);
            if (objEntr.approvedby != null) AddParameter(CpPackageValueInfo.Field.approvedby, objEntr.approvedby);
            if (objEntr.approveddate != null) AddParameter(CpPackageValueInfo.Field.approveddate, objEntr.approveddate);
            if (objEntr.approvednote != null) AddParameter(CpPackageValueInfo.Field.approvednote, objEntr.approvednote);
            if (objEntr.adjustno != null) AddParameter(CpPackageValueInfo.Field.adjustno, objEntr.adjustno);
            if (objEntr.approvedref != null) AddParameter(CpPackageValueInfo.Field.approvedref, objEntr.approvedref);
            if (objEntr.createdby != null) AddParameter(CpPackageValueInfo.Field.createdby, objEntr.createdby);
            if (objEntr.updatedby != null) AddParameter(CpPackageValueInfo.Field.updatedby, objEntr.updatedby);

            try
            {
                object tmp = executeScalar();
                if (tmp != null && tmp != DBNull.Value) ret = Convert.ToInt32(tmp);
                else ret = 0;
            }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return ret;
        }

        public string Update(CpPackageValueInfo objEntr)
        {
            string sql = @"UPDATE cppackagevalue SET ";

            if (objEntr.transdate != null) { sql += "transdate = @transdate,"; }
            if (objEntr.employee != null) { sql += "employee = @employee,"; }
            if (objEntr.valuedesc != null) { sql += "valuedesc = @valuedesc,"; }
            if (objEntr.amount != null) { sql += "amount = @amount,"; }
            if (objEntr.original != null) { sql += "original = @original,"; }
            if (objEntr.status != null) { sql += "status = @status,"; }
            if (objEntr.comments != null) { sql += "comments = @comments,"; }
            if (objEntr.approvalstatus != null) { sql += "approvalstatus = @approvalstatus,"; }
            if (objEntr.approvedby != null) { sql += "approvedby = @approvedby,"; }
            if (objEntr.approveddate != null) { sql += "approveddate = @approveddate,"; }
            if (objEntr.approvednote != null) { sql += "approvednote = @approvednote,"; }
            if (objEntr.adjustno != null) { sql += "adjustno = @adjustno,"; }
            if (objEntr.approvedref != null) { sql += "approvedref = @approvedref,"; }
            if (objEntr.updatedby != null) { sql += "updatedby = @updatedby,"; }

            sql += " lastupdate = getdate()";
            sql += @" WHERE  tvcdb = @tvcdb AND packagecode = @packagecode AND valueid = @valueid";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageValueInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageValueInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageValueInfo.Field.valueid, objEntr.valueid);

            if (objEntr.transdate != null) AddParameter(CpPackageValueInfo.Field.transdate, objEntr.transdate);
            if (objEntr.employee != null) AddParameter(CpPackageValueInfo.Field.employee, objEntr.employee);
            if (objEntr.valuedesc != null) AddParameter(CpPackageValueInfo.Field.valuedesc, objEntr.valuedesc);
            if (objEntr.amount != null) AddParameter(CpPackageValueInfo.Field.amount, objEntr.amount);
            if (objEntr.original != null) AddParameter(CpPackageValueInfo.Field.original, objEntr.original);
            if (objEntr.status != null) AddParameter(CpPackageValueInfo.Field.status, objEntr.status);
            if (objEntr.comments != null) AddParameter(CpPackageValueInfo.Field.comments, objEntr.comments);
            if (objEntr.approvalstatus != null) AddParameter(CpPackageValueInfo.Field.approvalstatus, objEntr.approvalstatus);
            if (objEntr.approvedby != null) AddParameter(CpPackageValueInfo.Field.approvedby, objEntr.approvedby);
            if (objEntr.approveddate != null) AddParameter(CpPackageValueInfo.Field.approveddate, objEntr.approveddate);
            if (objEntr.approvednote != null) AddParameter(CpPackageValueInfo.Field.approvednote, objEntr.approvednote);
            if (objEntr.adjustno != null) AddParameter(CpPackageValueInfo.Field.adjustno, objEntr.adjustno);
            if (objEntr.approvedref != null) AddParameter(CpPackageValueInfo.Field.approvedref, objEntr.approvedref);
            if (objEntr.updatedby != null) AddParameter(CpPackageValueInfo.Field.updatedby, objEntr.updatedby);

            string sErr = string.Empty;

            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }
        
        public decimal GetTotalByPackageCode(string db, string project, string packagecode, string status, ref string sErr)
        {
            string sql = @"SELECT SUM(v.amount) 
                FROM cppackagevalue v 
                INNER JOIN cppackageterm p ON v.tvcdb = p.tvcdb AND v.packagecode = p.packagecode
                    WHERE v.tvcdb = @tvcdb AND p.packagecode = @packagecode AND v.status = @status";
            if (!string.IsNullOrEmpty(project)) sql += " AND p.project = @project";
            decimal ret = 0;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageValueInfo.Field.tvcdb, db);
            AddParameter(CpPackageValueInfo.Field.status, status);
            if (!string.IsNullOrEmpty(project)) AddParameter(CpPackageTermInfo.Field.project, project);
            if (!string.IsNullOrEmpty(project)) AddParameter(CpPackageTermInfo.Field.packagecode, packagecode);
            try
            {
                object tmp = executeScalar();

                if (tmp != null && tmp != DBNull.Value) ret = Convert.ToDecimal(tmp);
            }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return ret;
        }

        public decimal GetTotal(string db, string project, string costcode, string notId, string status, ref string sErr)
        {
            string sql = @"SELECT SUM(v.amount) 
                FROM cppackagevalue v 
                INNER JOIN cppackageterm p ON v.tvcdb = p.tvcdb AND v.packagecode = p.packagecode
                    WHERE v.tvcdb = @tvcdb AND p.costcode = @costcode AND v.status = @status";
            if (!string.IsNullOrEmpty(project)) sql += " AND p.project = @project";
            if (!string.IsNullOrEmpty(notId)) sql += " AND v.packagecode <> @notId";
            decimal ret = 0;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageValueInfo.Field.tvcdb, db);
            AddParameter(CpPackageTermInfo.Field.costcode, costcode);
            AddParameter(CpPackageValueInfo.Field.status, status);
            if (!string.IsNullOrEmpty(project)) AddParameter(CpPackageTermInfo.Field.project, project);
            if (!string.IsNullOrEmpty(notId)) AddParameter("notId", notId);
            try
            {
                object tmp = executeScalar();

                if (tmp != null && tmp != DBNull.Value) ret = Convert.ToDecimal(tmp);
            }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return ret;
        }

        public string Delete(String tvcdb, String packagecode, String valueid = null)
        {
            string sql = @"DELETE cppackagevalue
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode";
            if (!string.IsNullOrEmpty(valueid)) sql += " AND valueid = @valueid";
            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageValueInfo.Field.tvcdb, tvcdb);
            AddParameter(CpPackageValueInfo.Field.packagecode, packagecode);
            if (!string.IsNullOrEmpty(valueid)) AddParameter(CpPackageValueInfo.Field.valueid, valueid);

            string sErr = string.Empty;
            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

        public string Clear(string db)
        {
            string sql = @"IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='cppackagevalue')
			DELETE cppackagevalue
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

        public Boolean IsExist(String tvcdb, String packagecode, String valueid, ref string sErr)
        {
            string sql = @"SELECT TOP (1) tvcdb, packagecode, valueid
            FROM  cppackagevalue
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode AND valueid = @valueid";

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageValueInfo.Field.tvcdb, tvcdb);
            AddParameter(CpPackageValueInfo.Field.packagecode, packagecode);
            AddParameter(CpPackageValueInfo.Field.valueid, valueid);

            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = ex.Message + " sql='" + sql + "'"; }

            return list.Rows.Count == 1;
        }

        #endregion Method

    }
}
