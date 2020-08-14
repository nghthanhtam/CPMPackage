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
    public class CpPackageDataAccess : Connection
    {
        #region Local Variable
        public CpPackageDataAccess(string type, string connectString, int timeout = 0) : base(type, connectString, timeout) { }
        public CpPackageDataAccess(Connection connection) : base(connection) { }
        #endregion Local Variable

        #region Method        
        public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            string sql = @"SELECT  packagecode 
            FROM  cppackage
            WHERE  tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, tvcdb);
            if (hasfilters) AddFilterCriteria(filters);

            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }

        public decimal? GetTotalByPackageTerm(string db, string project, string parentcode, string notId, ref string sErr)
        {
            string sql = @"SELECT SUM(totalamt)
                FROM  cppackage
                WHERE  tvcdb = @tvcdb AND parentcode = @parentcode";

            if (!string.IsNullOrEmpty(project)) sql += " AND project = @project";
            if (!string.IsNullOrEmpty(notId)) sql += " AND packagecode <> @notId";
            decimal ret = 0;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, db);
            AddParameter(CpPackageInfo.Field.parentcode, parentcode);
            if (!string.IsNullOrEmpty(project)) AddParameter(CpPackageInfo.Field.project, project);
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

        public decimal? GetAllTotalPackage(string db, string project, string notId, ref string sErr)
        {
            string sql = @"SELECT SUM(totalamt)
                FROM  cppackage
                WHERE  tvcdb = @tvcdb";

            if (!string.IsNullOrEmpty(project)) sql += " AND project = @project";
            if (!string.IsNullOrEmpty(notId)) sql += " AND packagecode <> @notId";
            decimal ret = 0;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, db);
            if (!string.IsNullOrEmpty(project)) AddParameter(CpPackageInfo.Field.project, project);
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

        public decimal? GetTotalByCostCode(string db, string project, string costcode, string notId, ref string sErr)
        {
            string sql = @"SELECT SUM(totalamt)
                FROM  cppackage
                WHERE  tvcdb = @tvcdb AND costcode = @costcode";

            if (!string.IsNullOrEmpty(project)) sql += " AND project = @project";
            if (!string.IsNullOrEmpty(notId)) sql += " AND packagecode <> @notId";
            decimal ret = 0;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, db);
            AddParameter(CpPackageInfo.Field.costcode, costcode);
            if (!string.IsNullOrEmpty(project)) AddParameter(CpPackageInfo.Field.project, project);
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

        public DataTable GetCostCode(string db, string packagecode, ref string sErr)
        {
            string sql = @"SELECT costcode, project
                FROM  cppackage
                WHERE   tvcdb = @tvcdb AND packagecode = @packagecode";

            DataTable ret = new DataTable();
            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, db);
            AddParameter(CpPackageInfo.Field.packagecode, packagecode);

            try
            {
                ret = executeSelect();
            }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return ret;
        }

        public int GetQueryCount(string db, string query, List<Criteria> filters, ref string sErr)
        {
            string sql = @"SELECT COUNT(*)
                FROM  cppackage
                WHERE   tvcdb = @tvcdb";
            string[] qs = null;
            if (!string.IsNullOrEmpty(query))
            {
                qs = query.Split(' ');
                for (var i = 0; i < qs.Length; i++)
                    sql += " AND (UPPER(packagecode) LIKE @query" + i + " OR UPPER(packagetitle) LIKE @query" + i + ")";
            }
            bool hafiltersParameter = BeginFilterCriteria(ref sql, filters);

            int ret = -1;
            openConnection();
            InitCommand(sql);
            AddParameter(CsContactInfo.Field.tvcdb, db);
            if (!string.IsNullOrEmpty(query))
            {
                for (var i = 0; i < qs.Length; i++)
                    AddParameter("query" + i, "%" + qs[i].ToUpper() + "%");
            }
            if (hafiltersParameter) AddFilterCriteria(filters);

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

        public DataTable GetQueryByFilter(string db, string query, List<Criteria> filters, ref string sErr, int indexPage, int itemPerPage)
        {
            string sql = @"SELECT  packagecode,  packagetitle, costcode
            FROM  cppackage 
            WHERE   tvcdb = @tvcdb ";
            string[] qs = null;
            if (!string.IsNullOrEmpty(query))
            {
                qs = query.Split(' ');
                for (var i = 0; i < qs.Length; i++)
                    sql += " AND (UPPER(packagecode) LIKE @query" + i + " OR UPPER(packagetitle) LIKE @query" + i + ")";
            }
            bool hafiltersParameter = BeginFilterCriteria(ref sql, filters);
            string sort = GetOrderCriteria(filters);

            if (string.IsNullOrEmpty(sort)) sql += @" ORDER BY  tvcdb, packagecode, packagetitle";
            else sql += sort;

            if (itemPerPage != 0)
            {
                if (DbType == "S") sql += @" OFFSET @indexPage ROWS FETCH NEXT @itemPerPage ROWS ONLY";
                else sql += @" LIMIT @indexPage ,@itemPerPage ";
            }

            InitConnect();
            InitCommand(sql);
            AddParameter(CsContactInfo.Field.tvcdb, db);
            if (itemPerPage != 0)
            {
                AddParameter("indexPage", indexPage);
                AddParameter("itemPerPage", itemPerPage);
            }
            if (!string.IsNullOrEmpty(query))
            {
                for (var i = 0; i < qs.Length; i++)
                    AddParameter("query" + i, "%" + qs[i].ToUpper() + "%");
            }
            if (hafiltersParameter) AddFilterCriteria(filters);

            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }

        public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0)
        {
            //string sql = @"SELECT  tvcdb, packagecode, packagetitle,approvedqty, status, dagid, parentcode, packagedate, packageno, costcode, pic, supervisor, packagedesc, requireddate, expirydate, asigndate, receiveddate, currencycode, currencyrate, amount, vatcode, vatrate, vatamt, totalamt, comments, project, pcklevel, leaf, contractcode, anal_cpk0, anal_cpk1, anal_cpk2, anal_cpk3, anal_cpk4, anal_cpk5, anal_cpk6, anal_cpk7, anal_cpk8, anal_cpk9, extreference1, extreference2, extreference3, extreference4, extdate1, extdate2, extdate3, extdate4, extnumber1, extnumber2, extnumber3, extnumber4, extdescription1, extdescription2, extdescription3, extdescription4, approvalstatus, approvedby, approveddate, approvednote, createdby, createddate, updatedby, lastupdate
            //FROM  cppackage
            //WHERE  tvcdb = @tvcdb";
            string sql = @"SELECT * FROM (SELECT p.*, c.packagetitle AS title, m.name AS employeename
                        FROM cppackage p
                        LEFT OUTER JOIN cppackageterm c ON p.tvcdb = c.tvcdb AND p.parentcode = c.packagecode
                        LEFT OUTER JOIN hremployee m ON p.tvcdb = m.tvcdb AND p.pic = m.employee 
                        WHERE p.tvcdb = @tvcdb) AS A
                        WHERE tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);
            string sort = GetOrderCriteria(filters);

            if (string.IsNullOrEmpty(sort)) sql += @" ORDER BY  tvcdb, packagecode";
            else sql += sort;

            if (itemPerPage != 0)
            {
                if (DbType == "S") sql += @" OFFSET @indexPage ROWS FETCH NEXT @itemPerPage ROWS ONLY";
                else sql += @" LIMIT @indexPage ,@itemPerPage ";
            }

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, tvcdb);
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


        public CpPackageInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0)
        {
            DataTable list = GetByFilterToDataTable(tvcdb, filters, ref sErr, indexPage, itemPerPage);

            if (!string.IsNullOrEmpty(sErr)) return null;

            CpPackageInfo[] res = null;

            try
            {
                res = new CpPackageInfo[list.Rows.Count];
                for (int i = 0; i < list.Rows.Count; i++)
                    res[i] = new CpPackageInfo(list.Rows[i]);
            }
            catch (Exception ex) { sErr = ex.Message; }

            if (!string.IsNullOrEmpty(sErr)) return null;
            return res;
        }

        public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr)
        {
            string sql = @"SELECT COUNT(*)
                FROM  cppackage
                WHERE  tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);

            int ret = -1;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, tvcdb);

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
        public int Add(CpPackageInfo objEntr, ref string sErr)
        {
            string sqlFields = @" INSERT INTO cppackage(tvcdb,packagecode,";
            string sqlValues = @" VALUES(@tvcdb,@packagecode,";

            if (objEntr.cancelremarks != null) { sqlFields += " cancelremarks,"; sqlValues += " @cancelremarks,"; }
            if (objEntr.approvedref != null) { sqlFields += " approvedref,"; sqlValues += " @approvedref,"; }
            if (objEntr.packagecate != null) { sqlFields += " packagecate,"; sqlValues += " @packagecate,"; }
            if (objEntr.investmentamt != null) { sqlFields += " investmentamt,"; sqlValues += " @investmentamt,"; }
            if (objEntr.approvedqty != null) { sqlFields += " approvedqty,"; sqlValues += " @approvedqty,"; }
            if (objEntr.packagetitle != null) { sqlFields += " packagetitle,"; sqlValues += " @packagetitle,"; }
            if (objEntr.status != null) { sqlFields += " status,"; sqlValues += " @status,"; }
            if (objEntr.dagid != null) { sqlFields += " dagid,"; sqlValues += " @dagid,"; }
            if (objEntr.parentcode != null) { sqlFields += " parentcode,"; sqlValues += " @parentcode,"; }
            //if (objEntr.isterm != null) { sqlFields += " isterm,"; sqlValues += " @isterm,"; }
            if (objEntr.packagedate != null) { sqlFields += " packagedate,"; sqlValues += " @packagedate,"; }
            if (objEntr.packageno != null) { sqlFields += " packageno,"; sqlValues += " @packageno,"; }
            if (objEntr.costcode != null) { sqlFields += " costcode,"; sqlValues += " @costcode,"; }
            if (objEntr.pic != null) { sqlFields += " pic,"; sqlValues += " @pic,"; }
            if (objEntr.supervisor != null) { sqlFields += " supervisor,"; sqlValues += " @supervisor,"; }
            if (objEntr.packagedesc != null) { sqlFields += " packagedesc,"; sqlValues += " @packagedesc,"; }
            if (objEntr.requireddate != null) { sqlFields += " requireddate,"; sqlValues += " @requireddate,"; }
            if (objEntr.expirydate != null) { sqlFields += " expirydate,"; sqlValues += " @expirydate,"; }
            if (objEntr.asigndate != null) { sqlFields += " asigndate,"; sqlValues += " @asigndate,"; }
            if (objEntr.receiveddate != null) { sqlFields += " receiveddate,"; sqlValues += " @receiveddate,"; }
            if (objEntr.currencycode != null) { sqlFields += " currencycode,"; sqlValues += " @currencycode,"; }
            if (objEntr.currencyrate != null) { sqlFields += " currencyrate,"; sqlValues += " @currencyrate,"; }
            if (objEntr.amount != null) { sqlFields += " amount,"; sqlValues += " @amount,"; }
            if (objEntr.vatcode != null) { sqlFields += " vatcode,"; sqlValues += " @vatcode,"; }
            if (objEntr.vatrate != null) { sqlFields += " vatrate,"; sqlValues += " @vatrate,"; }
            if (objEntr.vatamt != null) { sqlFields += " vatamt,"; sqlValues += " @vatamt,"; }
            if (objEntr.totalamt != null) { sqlFields += " totalamt,"; sqlValues += " @totalamt,"; }
            if (objEntr.comments != null) { sqlFields += " comments,"; sqlValues += " @comments,"; }
            if (objEntr.project != null) { sqlFields += " project,"; sqlValues += " @project,"; }
            if (objEntr.pcklevel != null) { sqlFields += " pcklevel,"; sqlValues += " @pcklevel,"; }
            if (objEntr.leaf != null) { sqlFields += " leaf,"; sqlValues += " @leaf,"; }
            if (objEntr.contractcode != null) { sqlFields += " contractcode,"; sqlValues += " @contractcode,"; }
            if (objEntr.anal_cpk0 != null) { sqlFields += " anal_cpk0,"; sqlValues += " @anal_cpk0,"; }
            if (objEntr.anal_cpk1 != null) { sqlFields += " anal_cpk1,"; sqlValues += " @anal_cpk1,"; }
            if (objEntr.anal_cpk2 != null) { sqlFields += " anal_cpk2,"; sqlValues += " @anal_cpk2,"; }
            if (objEntr.anal_cpk3 != null) { sqlFields += " anal_cpk3,"; sqlValues += " @anal_cpk3,"; }
            if (objEntr.anal_cpk4 != null) { sqlFields += " anal_cpk4,"; sqlValues += " @anal_cpk4,"; }
            if (objEntr.anal_cpk5 != null) { sqlFields += " anal_cpk5,"; sqlValues += " @anal_cpk5,"; }
            if (objEntr.anal_cpk6 != null) { sqlFields += " anal_cpk6,"; sqlValues += " @anal_cpk6,"; }
            if (objEntr.anal_cpk7 != null) { sqlFields += " anal_cpk7,"; sqlValues += " @anal_cpk7,"; }
            if (objEntr.anal_cpk8 != null) { sqlFields += " anal_cpk8,"; sqlValues += " @anal_cpk8,"; }
            if (objEntr.anal_cpk9 != null) { sqlFields += " anal_cpk9,"; sqlValues += " @anal_cpk9,"; }
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
            if (objEntr.approvalstatus != null) { sqlFields += " approvalstatus,"; sqlValues += " @approvalstatus,"; }
            if (objEntr.approvedby != null) { sqlFields += " approvedby,"; sqlValues += " @approvedby,"; }
            if (objEntr.approveddate != null) { sqlFields += " approveddate,"; sqlValues += " @approveddate,"; }
            if (objEntr.approvednote != null) { sqlFields += " approvednote,"; sqlValues += " @approvednote,"; }
            if (objEntr.createdby != null) { sqlFields += " createdby,"; sqlValues += " @createdby,"; }
            if (objEntr.updatedby != null) { sqlFields += " updatedby,"; sqlValues += " @updatedby,"; }

            sqlFields = sqlFields.Substring(0, sqlFields.Length - 1) + ")";
            sqlValues = sqlValues.Substring(0, sqlValues.Length - 1) + ")";

            int ret = -1;
            string sql = sqlFields + sqlValues;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageInfo.Field.packagecode, objEntr.packagecode);

            if (objEntr.cancelremarks != null) AddParameter(CpPackageInfo.Field.cancelremarks, objEntr.cancelremarks);
            if (objEntr.approvedref != null) AddParameter(CpPackageInfo.Field.approvedref, objEntr.approvedref);
            if (objEntr.packagecate != null) AddParameter(CpPackageInfo.Field.packagecate, objEntr.packagecate);
            if (objEntr.investmentamt != null) AddParameter(CpPackageInfo.Field.investmentamt, objEntr.investmentamt);
            if (objEntr.approvedqty != null) AddParameter(CpPackageInfo.Field.approvedqty, objEntr.approvedqty);
            if (objEntr.packagetitle != null) AddParameter(CpPackageInfo.Field.packagetitle, objEntr.packagetitle);
            if (objEntr.status != null) AddParameter(CpPackageInfo.Field.status, objEntr.status);
            if (objEntr.dagid != null) AddParameter(CpPackageInfo.Field.dagid, objEntr.dagid);
            if (objEntr.parentcode != null) AddParameter(CpPackageInfo.Field.parentcode, objEntr.parentcode);
            //if (objEntr.isterm != null) AddParameter(CpPackageInfo.Field.isterm, objEntr.isterm);         
            if (objEntr.packagedate != null) AddParameter(CpPackageInfo.Field.packagedate, objEntr.packagedate);
            if (objEntr.packageno != null) AddParameter(CpPackageInfo.Field.packageno, objEntr.packageno);
            if (objEntr.costcode != null) AddParameter(CpPackageInfo.Field.costcode, objEntr.costcode);
            if (objEntr.pic != null) AddParameter(CpPackageInfo.Field.pic, objEntr.pic);
            if (objEntr.supervisor != null) AddParameter(CpPackageInfo.Field.supervisor, objEntr.supervisor);
            if (objEntr.packagedesc != null) AddParameter(CpPackageInfo.Field.packagedesc, objEntr.packagedesc);
            if (objEntr.requireddate != null) AddParameter(CpPackageInfo.Field.requireddate, objEntr.requireddate);
            if (objEntr.expirydate != null) AddParameter(CpPackageInfo.Field.expirydate, objEntr.expirydate);
            if (objEntr.asigndate != null) AddParameter(CpPackageInfo.Field.asigndate, objEntr.asigndate);
            if (objEntr.receiveddate != null) AddParameter(CpPackageInfo.Field.receiveddate, objEntr.receiveddate);
            if (objEntr.currencycode != null) AddParameter(CpPackageInfo.Field.currencycode, objEntr.currencycode);
            if (objEntr.currencyrate != null) AddParameter(CpPackageInfo.Field.currencyrate, objEntr.currencyrate);
            if (objEntr.amount != null) AddParameter(CpPackageInfo.Field.amount, objEntr.amount);
            if (objEntr.vatcode != null) AddParameter(CpPackageInfo.Field.vatcode, objEntr.vatcode);
            if (objEntr.vatrate != null) AddParameter(CpPackageInfo.Field.vatrate, objEntr.vatrate);
            if (objEntr.vatamt != null) AddParameter(CpPackageInfo.Field.vatamt, objEntr.vatamt);
            if (objEntr.totalamt != null) AddParameter(CpPackageInfo.Field.totalamt, objEntr.totalamt);
            if (objEntr.comments != null) AddParameter(CpPackageInfo.Field.comments, objEntr.comments);
            if (objEntr.project != null) AddParameter(CpPackageInfo.Field.project, objEntr.project);
            if (objEntr.pcklevel != null) AddParameter(CpPackageInfo.Field.pcklevel, objEntr.pcklevel);
            if (objEntr.leaf != null) AddParameter(CpPackageInfo.Field.leaf, objEntr.leaf);
            if (objEntr.contractcode != null) AddParameter(CpPackageInfo.Field.contractcode, objEntr.contractcode);
            if (objEntr.anal_cpk0 != null) AddParameter(CpPackageInfo.Field.anal_cpk0, objEntr.anal_cpk0);
            if (objEntr.anal_cpk1 != null) AddParameter(CpPackageInfo.Field.anal_cpk1, objEntr.anal_cpk1);
            if (objEntr.anal_cpk2 != null) AddParameter(CpPackageInfo.Field.anal_cpk2, objEntr.anal_cpk2);
            if (objEntr.anal_cpk3 != null) AddParameter(CpPackageInfo.Field.anal_cpk3, objEntr.anal_cpk3);
            if (objEntr.anal_cpk4 != null) AddParameter(CpPackageInfo.Field.anal_cpk4, objEntr.anal_cpk4);
            if (objEntr.anal_cpk5 != null) AddParameter(CpPackageInfo.Field.anal_cpk5, objEntr.anal_cpk5);
            if (objEntr.anal_cpk6 != null) AddParameter(CpPackageInfo.Field.anal_cpk6, objEntr.anal_cpk6);
            if (objEntr.anal_cpk7 != null) AddParameter(CpPackageInfo.Field.anal_cpk7, objEntr.anal_cpk7);
            if (objEntr.anal_cpk8 != null) AddParameter(CpPackageInfo.Field.anal_cpk8, objEntr.anal_cpk8);
            if (objEntr.anal_cpk9 != null) AddParameter(CpPackageInfo.Field.anal_cpk9, objEntr.anal_cpk9);
            if (objEntr.extreference1 != null) AddParameter(CpPackageInfo.Field.extreference1, objEntr.extreference1);
            if (objEntr.extreference2 != null) AddParameter(CpPackageInfo.Field.extreference2, objEntr.extreference2);
            if (objEntr.extreference3 != null) AddParameter(CpPackageInfo.Field.extreference3, objEntr.extreference3);
            if (objEntr.extreference4 != null) AddParameter(CpPackageInfo.Field.extreference4, objEntr.extreference4);
            if (objEntr.extdate1 != null) AddParameter(CpPackageInfo.Field.extdate1, objEntr.extdate1);
            if (objEntr.extdate2 != null) AddParameter(CpPackageInfo.Field.extdate2, objEntr.extdate2);
            if (objEntr.extdate3 != null) AddParameter(CpPackageInfo.Field.extdate3, objEntr.extdate3);
            if (objEntr.extdate4 != null) AddParameter(CpPackageInfo.Field.extdate4, objEntr.extdate4);
            if (objEntr.extnumber1 != null) AddParameter(CpPackageInfo.Field.extnumber1, objEntr.extnumber1);
            if (objEntr.extnumber2 != null) AddParameter(CpPackageInfo.Field.extnumber2, objEntr.extnumber2);
            if (objEntr.extnumber3 != null) AddParameter(CpPackageInfo.Field.extnumber3, objEntr.extnumber3);
            if (objEntr.extnumber4 != null) AddParameter(CpPackageInfo.Field.extnumber4, objEntr.extnumber4);
            if (objEntr.extdescription1 != null) AddParameter(CpPackageInfo.Field.extdescription1, objEntr.extdescription1);
            if (objEntr.extdescription2 != null) AddParameter(CpPackageInfo.Field.extdescription2, objEntr.extdescription2);
            if (objEntr.extdescription3 != null) AddParameter(CpPackageInfo.Field.extdescription3, objEntr.extdescription3);
            if (objEntr.extdescription4 != null) AddParameter(CpPackageInfo.Field.extdescription4, objEntr.extdescription4);
            if (objEntr.approvalstatus != null) AddParameter(CpPackageInfo.Field.approvalstatus, objEntr.approvalstatus);
            if (objEntr.approvedby != null) AddParameter(CpPackageInfo.Field.approvedby, objEntr.approvedby);
            if (objEntr.approveddate != null) AddParameter(CpPackageInfo.Field.approveddate, objEntr.approveddate);
            if (objEntr.approvednote != null) AddParameter(CpPackageInfo.Field.approvednote, objEntr.approvednote);
            if (objEntr.createdby != null) AddParameter(CpPackageInfo.Field.createdby, objEntr.createdby);
            if (objEntr.updatedby != null) AddParameter(CpPackageInfo.Field.updatedby, objEntr.updatedby);

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

        public string Update(CpPackageInfo objEntr)
        {
            string sql = @"UPDATE cppackage SET ";

            if (objEntr.cancelremarks != null) { sql += "cancelremarks = @cancelremarks,"; }
            if (objEntr.approvedref != null) { sql += "approvedref = @approvedref,"; }
            if (objEntr.packagecate != null) { sql += "packagecate = @packagecate,"; }
            if (objEntr.investmentamt != null) { sql += "investmentamt = @investmentamt,"; }
            if (objEntr.approvedqty != null) { sql += "approvedqty = @approvedqty,"; }
            if (objEntr.packagetitle != null) { sql += "packagetitle = @packagetitle,"; }
            if (objEntr.status != null) { sql += "status = @status,"; }
            if (objEntr.dagid != null) { sql += "dagid = @dagid,"; }
            if (objEntr.parentcode != null) { sql += "parentcode = @parentcode,"; }
            //if (objEntr.isterm != null) { sql += "isterm = @isterm,"; }
            if (objEntr.packagedate != null) { sql += "packagedate = @packagedate,"; }
            if (objEntr.packageno != null) { sql += "packageno = @packageno,"; }
            if (objEntr.costcode != null) { sql += "costcode = @costcode,"; }
            if (objEntr.pic != null) { sql += "pic = @pic,"; }
            if (objEntr.supervisor != null) { sql += "supervisor = @supervisor,"; }
            if (objEntr.packagedesc != null) { sql += "packagedesc = @packagedesc,"; }
            if (objEntr.requireddate != null) { sql += "requireddate = @requireddate,"; }
            if (objEntr.expirydate != null) { sql += "expirydate = @expirydate,"; }
            if (objEntr.asigndate != null) { sql += "asigndate = @asigndate,"; }
            if (objEntr.receiveddate != null) { sql += "receiveddate = @receiveddate,"; }
            if (objEntr.currencycode != null) { sql += "currencycode = @currencycode,"; }
            if (objEntr.currencyrate != null) { sql += "currencyrate = @currencyrate,"; }
            if (objEntr.amount != null) { sql += "amount = @amount,"; }
            if (objEntr.vatcode != null) { sql += "vatcode = @vatcode,"; }
            if (objEntr.vatrate != null) { sql += "vatrate = @vatrate,"; }
            if (objEntr.vatamt != null) { sql += "vatamt = @vatamt,"; }
            if (objEntr.totalamt != null) { sql += "totalamt = @totalamt,"; }
            if (objEntr.comments != null) { sql += "comments = @comments,"; }
            if (objEntr.project != null) { sql += "project = @project,"; }
            if (objEntr.pcklevel != null) { sql += "pcklevel = @pcklevel,"; }
            if (objEntr.leaf != null) { sql += "leaf = @leaf,"; }
            if (objEntr.contractcode != null) { sql += "contractcode = @contractcode,"; }
            if (objEntr.anal_cpk0 != null) { sql += "anal_cpk0 = @anal_cpk0,"; }
            if (objEntr.anal_cpk1 != null) { sql += "anal_cpk1 = @anal_cpk1,"; }
            if (objEntr.anal_cpk2 != null) { sql += "anal_cpk2 = @anal_cpk2,"; }
            if (objEntr.anal_cpk3 != null) { sql += "anal_cpk3 = @anal_cpk3,"; }
            if (objEntr.anal_cpk4 != null) { sql += "anal_cpk4 = @anal_cpk4,"; }
            if (objEntr.anal_cpk5 != null) { sql += "anal_cpk5 = @anal_cpk5,"; }
            if (objEntr.anal_cpk6 != null) { sql += "anal_cpk6 = @anal_cpk6,"; }
            if (objEntr.anal_cpk7 != null) { sql += "anal_cpk7 = @anal_cpk7,"; }
            if (objEntr.anal_cpk8 != null) { sql += "anal_cpk8 = @anal_cpk8,"; }
            if (objEntr.anal_cpk9 != null) { sql += "anal_cpk9 = @anal_cpk9,"; }
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
            if (objEntr.approvalstatus != null) { sql += "approvalstatus = @approvalstatus,"; }
            if (objEntr.approvedby != null) { sql += "approvedby = @approvedby,"; }
            if (objEntr.approveddate != null) { sql += "approveddate = @approveddate,"; }
            if (objEntr.approvednote != null) { sql += "approvednote = @approvednote,"; }
            if (objEntr.updatedby != null) { sql += "updatedby = @updatedby,"; }

            sql += " lastupdate = getdate()";
            sql += @" WHERE  tvcdb = @tvcdb AND packagecode = @packagecode";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageInfo.Field.packagecode, objEntr.packagecode);

            if (objEntr.cancelremarks != null) AddParameter(CpPackageInfo.Field.cancelremarks, objEntr.cancelremarks);
            if (objEntr.approvedref != null) AddParameter(CpPackageInfo.Field.approvedref, objEntr.approvedref);
            if (objEntr.packagecate != null) AddParameter(CpPackageInfo.Field.packagecate, objEntr.packagecate);
            if (objEntr.investmentamt != null) AddParameter(CpPackageInfo.Field.investmentamt, objEntr.investmentamt);
            if (objEntr.approvedqty != null) AddParameter(CpPackageInfo.Field.approvedqty, objEntr.approvedqty);
            if (objEntr.packagetitle != null) AddParameter(CpPackageInfo.Field.packagetitle, objEntr.packagetitle);
            if (objEntr.status != null) AddParameter(CpPackageInfo.Field.status, objEntr.status);
            if (objEntr.dagid != null) AddParameter(CpPackageInfo.Field.dagid, objEntr.dagid);
            if (objEntr.parentcode != null) AddParameter(CpPackageInfo.Field.parentcode, objEntr.parentcode);
            //if (objEntr.isterm != null) AddParameter(CpPackageInfo.Field.isterm, objEntr.isterm);         
            if (objEntr.packagedate != null) AddParameter(CpPackageInfo.Field.packagedate, objEntr.packagedate);
            if (objEntr.packageno != null) AddParameter(CpPackageInfo.Field.packageno, objEntr.packageno);
            if (objEntr.costcode != null) AddParameter(CpPackageInfo.Field.costcode, objEntr.costcode);
            if (objEntr.pic != null) AddParameter(CpPackageInfo.Field.pic, objEntr.pic);
            if (objEntr.supervisor != null) AddParameter(CpPackageInfo.Field.supervisor, objEntr.supervisor);
            if (objEntr.packagedesc != null) AddParameter(CpPackageInfo.Field.packagedesc, objEntr.packagedesc);
            if (objEntr.requireddate != null) AddParameter(CpPackageInfo.Field.requireddate, objEntr.requireddate);
            if (objEntr.expirydate != null) AddParameter(CpPackageInfo.Field.expirydate, objEntr.expirydate);
            if (objEntr.asigndate != null) AddParameter(CpPackageInfo.Field.asigndate, objEntr.asigndate);
            if (objEntr.receiveddate != null) AddParameter(CpPackageInfo.Field.receiveddate, objEntr.receiveddate);
            if (objEntr.currencycode != null) AddParameter(CpPackageInfo.Field.currencycode, objEntr.currencycode);
            if (objEntr.currencyrate != null) AddParameter(CpPackageInfo.Field.currencyrate, objEntr.currencyrate);
            if (objEntr.amount != null) AddParameter(CpPackageInfo.Field.amount, objEntr.amount);
            if (objEntr.vatcode != null) AddParameter(CpPackageInfo.Field.vatcode, objEntr.vatcode);
            if (objEntr.vatrate != null) AddParameter(CpPackageInfo.Field.vatrate, objEntr.vatrate);
            if (objEntr.vatamt != null) AddParameter(CpPackageInfo.Field.vatamt, objEntr.vatamt);
            if (objEntr.totalamt != null) AddParameter(CpPackageInfo.Field.totalamt, objEntr.totalamt);
            if (objEntr.comments != null) AddParameter(CpPackageInfo.Field.comments, objEntr.comments);
            if (objEntr.project != null) AddParameter(CpPackageInfo.Field.project, objEntr.project);
            if (objEntr.pcklevel != null) AddParameter(CpPackageInfo.Field.pcklevel, objEntr.pcklevel);
            if (objEntr.leaf != null) AddParameter(CpPackageInfo.Field.leaf, objEntr.leaf);
            if (objEntr.contractcode != null) AddParameter(CpPackageInfo.Field.contractcode, objEntr.contractcode);
            if (objEntr.anal_cpk0 != null) AddParameter(CpPackageInfo.Field.anal_cpk0, objEntr.anal_cpk0);
            if (objEntr.anal_cpk1 != null) AddParameter(CpPackageInfo.Field.anal_cpk1, objEntr.anal_cpk1);
            if (objEntr.anal_cpk2 != null) AddParameter(CpPackageInfo.Field.anal_cpk2, objEntr.anal_cpk2);
            if (objEntr.anal_cpk3 != null) AddParameter(CpPackageInfo.Field.anal_cpk3, objEntr.anal_cpk3);
            if (objEntr.anal_cpk4 != null) AddParameter(CpPackageInfo.Field.anal_cpk4, objEntr.anal_cpk4);
            if (objEntr.anal_cpk5 != null) AddParameter(CpPackageInfo.Field.anal_cpk5, objEntr.anal_cpk5);
            if (objEntr.anal_cpk6 != null) AddParameter(CpPackageInfo.Field.anal_cpk6, objEntr.anal_cpk6);
            if (objEntr.anal_cpk7 != null) AddParameter(CpPackageInfo.Field.anal_cpk7, objEntr.anal_cpk7);
            if (objEntr.anal_cpk8 != null) AddParameter(CpPackageInfo.Field.anal_cpk8, objEntr.anal_cpk8);
            if (objEntr.anal_cpk9 != null) AddParameter(CpPackageInfo.Field.anal_cpk9, objEntr.anal_cpk9);
            if (objEntr.extreference1 != null) AddParameter(CpPackageInfo.Field.extreference1, objEntr.extreference1);
            if (objEntr.extreference2 != null) AddParameter(CpPackageInfo.Field.extreference2, objEntr.extreference2);
            if (objEntr.extreference3 != null) AddParameter(CpPackageInfo.Field.extreference3, objEntr.extreference3);
            if (objEntr.extreference4 != null) AddParameter(CpPackageInfo.Field.extreference4, objEntr.extreference4);
            if (objEntr.extdate1 != null) AddParameter(CpPackageInfo.Field.extdate1, objEntr.extdate1);
            if (objEntr.extdate2 != null) AddParameter(CpPackageInfo.Field.extdate2, objEntr.extdate2);
            if (objEntr.extdate3 != null) AddParameter(CpPackageInfo.Field.extdate3, objEntr.extdate3);
            if (objEntr.extdate4 != null) AddParameter(CpPackageInfo.Field.extdate4, objEntr.extdate4);
            if (objEntr.extnumber1 != null) AddParameter(CpPackageInfo.Field.extnumber1, objEntr.extnumber1);
            if (objEntr.extnumber2 != null) AddParameter(CpPackageInfo.Field.extnumber2, objEntr.extnumber2);
            if (objEntr.extnumber3 != null) AddParameter(CpPackageInfo.Field.extnumber3, objEntr.extnumber3);
            if (objEntr.extnumber4 != null) AddParameter(CpPackageInfo.Field.extnumber4, objEntr.extnumber4);
            if (objEntr.extdescription1 != null) AddParameter(CpPackageInfo.Field.extdescription1, objEntr.extdescription1);
            if (objEntr.extdescription2 != null) AddParameter(CpPackageInfo.Field.extdescription2, objEntr.extdescription2);
            if (objEntr.extdescription3 != null) AddParameter(CpPackageInfo.Field.extdescription3, objEntr.extdescription3);
            if (objEntr.extdescription4 != null) AddParameter(CpPackageInfo.Field.extdescription4, objEntr.extdescription4);
            if (objEntr.approvalstatus != null) AddParameter(CpPackageInfo.Field.approvalstatus, objEntr.approvalstatus);
            if (objEntr.approvedby != null) AddParameter(CpPackageInfo.Field.approvedby, objEntr.approvedby);
            if (objEntr.approveddate != null) AddParameter(CpPackageInfo.Field.approveddate, objEntr.approveddate);
            if (objEntr.approvednote != null) AddParameter(CpPackageInfo.Field.approvednote, objEntr.approvednote);
            if (objEntr.updatedby != null) AddParameter(CpPackageInfo.Field.updatedby, objEntr.updatedby);

            string sErr = string.Empty;

            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

        public DataTable GetPKTByFilter(string db, string itemId, string project, CriteriaCollection filters, ref string sErr)
        {
            string sql = @"SELECT * FROM (SELECT c.tvcdb, c.investmentamt, c.cancelremarks, c.packagecate, c.packagecode, c.packagetitle, c.parentcode, c.leaf, c.pcklevel, c.costcode,
                    c.status, c.dagid,  c.project, c.packagedate, c.packageno, c.amount, c.packagedesc, c.comments,  c.pic, c.supervisor, c.totalamt,
                    c.anal_cpk0, c.anal_cpk1, c.anal_cpk2, c.anal_cpk3, c.anal_cpk4, c.anal_cpk5, c.anal_cpk6, c.anal_cpk7, c.anal_cpk8, c.anal_cpk9,
                    c.extreference1, c.extreference2, c.extreference3, c.extreference4,
                    c.extdate1, c.extdate2, c.extdate3, c.extdate4,
                    c.extnumber1, c.extnumber2, c.extnumber3, c.extnumber4,
                    c.extdescription1, c.extdescription2, c.extdescription3, c.extdescription4,
                SUM(CASE WHEN p.original = 'Y' AND p.status = 'A' THEN p.amount ELSE 0 END ) AS originalvalue,
                SUM(CASE WHEN p.original <> 'Y' AND p.status = 'A' THEN p.amount ELSE 0 END ) AS adjustment,
                SUM(CASE WHEN p.status = 'A' THEN p.amount ELSE 0 END ) AS total,
                SUM(CASE WHEN p.status <> 'A' THEN p.amount ELSE 0 END ) AS waitingapproval
            FROM  cppackage c LEFT OUTER JOIN cppackagevalue p ON c.tvcdb = p.tvcdb AND c.packagecode = p.packagecode 
            WHERE   c.tvcdb = @tvcdb AND c.parentcode = @parentcode  AND c.project = @project
            GROUP BY c.tvcdb, c.packagecate, c.packagecode, c.packagetitle, c.parentcode, c.leaf, c.pcklevel, c.costcode, 
                    c.status, c.dagid, c.project, c.packagedate, c.packageno, c.amount, c.packagedesc, c.comments,  c.pic, c.supervisor, c.totalamt,
                    c.anal_cpk0, c.anal_cpk1, c.anal_cpk2, c.anal_cpk3, c.anal_cpk4, c.anal_cpk5, c.anal_cpk6, c.anal_cpk7, c.anal_cpk8, c.anal_cpk9,
                    c.extreference1, c.extreference2, c.extreference3, c.extreference4,
                    c.extdate1, c.extdate2, c.extdate3, c.extdate4,
                    c.extnumber1, c.extnumber2, c.extnumber3, c.extnumber4,
                    c.extdescription1, c.extdescription2, c.extdescription3, c.extdescription4) AS A
            WHERE tvcdb = @tvcdb";

            bool hafiltersParameter = BeginFilterCriteria(ref sql, filters);
            string sort = GetOrderCriteria(filters);

            if (string.IsNullOrEmpty(sort)) sql += @" ORDER BY  tvcdb, packagecode, packagetitle";
            else sql += sort;

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, db);
            AddParameter(CpPackageInfo.Field.parentcode, itemId);
            AddParameter(CpPackageInfo.Field.project, project);

            if (hafiltersParameter) AddFilterCriteria(filters);

            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }

        public string Delete(String tvcdb, String packagecode)
        {
            string sql = @"DELETE cppackage
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageInfo.Field.tvcdb, tvcdb);
            AddParameter(CpPackageInfo.Field.packagecode, packagecode);

            string sErr = string.Empty;
            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

        public string Clear(string db)
        {
            string sql = @"IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='cppackage')
			DELETE cppackage
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

        public Boolean IsExist(String tvcdb, String packagecode, ref string sErr)
        {
            string sql = @"SELECT TOP (1) tvcdb, packagecode
            FROM  cppackage
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode";

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageInfo.Field.tvcdb, tvcdb);
            AddParameter(CpPackageInfo.Field.packagecode, packagecode);

            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = ex.Message + " sql='" + sql + "'"; }

            return list.Rows.Count == 1;
        }

        #endregion Method

    }
}
