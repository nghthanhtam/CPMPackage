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
    public class CpPackageVdrDataAccess : Connection
    {
		#region Local Variable
        public CpPackageVdrDataAccess(string type, string connectString, int timeout = 0) : base(type, connectString, timeout) { }
        public CpPackageVdrDataAccess(Connection connection) : base(connection) { }
		#endregion Local Variable
		
		#region Method        
		public DataTable GetShortData(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            string sql = @"SELECT  packagecode, vendorcode 
            FROM  cppackagevdr
            WHERE  tvcdb = @tvcdb";
			
			bool hasfilters = BeginFilterCriteria(ref sql, filters);

            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageVdrInfo.Field.tvcdb, tvcdb);
            if (hasfilters) AddFilterCriteria(filters);
			
            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }

            return list;
        }
		
		public DataTable GetByFilterToDataTable(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0) {            
            string sql = 
                @"SELECT * FROM (SELECT p.*, c.name AS vendorname
                FROM  cppackagevdr p
                    LEFT OUTER JOIN csnameaddress c ON p.tvcdb = c.tvcdb AND p.vendorcode = c.nadcode
                    WHERE  p.tvcdb = @tvcdb) AS A
                WHERE  tvcdb = @tvcdb";

            bool hasfilters = BeginFilterCriteria(ref sql, filters);
			string sort =  GetOrderCriteria(filters);
			
			if (string.IsNullOrEmpty(sort)) sql += @" ORDER BY  tvcdb, packagecode, vendorcode";
			else sql += sort;
			
            if (itemPerPage != 0) {
                if (DbType == "S") sql += @" OFFSET @indexPage ROWS FETCH NEXT @itemPerPage ROWS ONLY";
                else sql += @" LIMIT @indexPage ,@itemPerPage ";
            }
            
            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageVdrInfo.Field.tvcdb, tvcdb);
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
		
		
		public CpPackageVdrInfo[] GetByFilter(String tvcdb, List<DTO.Criteria> filters, ref string sErr, int indexPage = 0, int itemPerPage = 0) {
			DataTable list = GetByFilterToDataTable(tvcdb, filters, ref sErr, indexPage, itemPerPage);

			if (!string.IsNullOrEmpty(sErr)) return null;
			
            CpPackageVdrInfo[] res = null;            
			
			try
			{
				res = new CpPackageVdrInfo[list.Rows.Count];
				for (int i = 0; i < list.Rows.Count; i++)
					res[i] = new CpPackageVdrInfo(list.Rows[i]);
			}
			catch (Exception ex) { sErr = ex.Message; }
            
			if (!string.IsNullOrEmpty(sErr)) return null;
            return res;
        }
		
		public int GetCountRecord(String tvcdb, List<DTO.Criteria> filters, ref string sErr) {
            string sql = @"SELECT COUNT(*)
                FROM  cppackagevdr
                WHERE  tvcdb = @tvcdb";
				
			bool hasfilters = BeginFilterCriteria(ref sql, filters);
			
			int ret = -1;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageVdrInfo.Field.tvcdb, tvcdb);
          
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
        public int Add(CpPackageVdrInfo objEntr, ref string sErr) {
            string sqlFields = @" INSERT INTO cppackagevdr(tvcdb,packagecode,vendorcode,";
            string sqlValues = @" VALUES(@tvcdb,@packagecode,@vendorcode,";

            if (objEntr.status != null) { sqlFields += " status,"; sqlValues += " @status,"; }
            if (objEntr.invitationdate != null) { sqlFields += " invitationdate,"; sqlValues += " @invitationdate,"; }
            if (objEntr.responsedate != null) { sqlFields += " responsedate,"; sqlValues += " @responsedate,"; }
            if (objEntr.mark != null) { sqlFields += " mark,"; sqlValues += " @mark,"; }
            if (objEntr.overal != null) { sqlFields += " overal,"; sqlValues += " @overal,"; }
            if (objEntr.contractno != null) { sqlFields += " contractno,"; sqlValues += " @contractno,"; }
            if (objEntr.techevaluate != null) { sqlFields += " techevaluate,"; sqlValues += " @techevaluate,"; }
            if (objEntr.bidprice != null) { sqlFields += " bidprice,"; sqlValues += " @bidprice,"; }
            if (objEntr.impactamt != null) { sqlFields += " impactamt,"; sqlValues += " @impactamt,"; }
            if (objEntr.externalamt != null) { sqlFields += " externalamt,"; sqlValues += " @externalamt,"; }
            if (objEntr.discountamt != null) { sqlFields += " discountamt,"; sqlValues += " @discountamt,"; }
            if (objEntr.vatcode != null) { sqlFields += " vatcode,"; sqlValues += " @vatcode,"; }
            if (objEntr.vatrate != null) { sqlFields += " vatrate,"; sqlValues += " @vatrate,"; }
            if (objEntr.vatamt != null) { sqlFields += " vatamt,"; sqlValues += " @vatamt,"; }
            if (objEntr.totalawardprice != null) { sqlFields += " totalawardprice,"; sqlValues += " @totalawardprice,"; }
            if (objEntr.ranking != null) { sqlFields += " ranking,"; sqlValues += " @ranking,"; }
            if (objEntr.pricedifference != null) { sqlFields += " pricedifference,"; sqlValues += " @pricedifference,"; }
            if (objEntr.pevaluation != null) { sqlFields += " pevaluation,"; sqlValues += " @pevaluation,"; }
            if (objEntr.commercialevaluation != null) { sqlFields += " commercialevaluation,"; sqlValues += " @commercialevaluation,"; }
            if (objEntr.techevaludation != null) { sqlFields += " techevaludation,"; sqlValues += " @techevaludation,"; }
            if (objEntr.comments != null) { sqlFields += " comments,"; sqlValues += " @comments,"; }
			if (objEntr.createdby != null) { sqlFields += " createdby,"; sqlValues += " @createdby,"; }
			if (objEntr.updatedby != null) { sqlFields += " updatedby,"; sqlValues += " @updatedby,"; }

            sqlFields = sqlFields.Substring(0, sqlFields.Length - 1) + ")";
            sqlValues = sqlValues.Substring(0, sqlValues.Length - 1) + ")";

            int ret = -1;
			string sql = sqlFields + sqlValues;
            openConnection();
            InitCommand(sql);
            AddParameter(CpPackageVdrInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageVdrInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageVdrInfo.Field.vendorcode, objEntr.vendorcode);

            if (objEntr.status != null) AddParameter(CpPackageVdrInfo.Field.status, objEntr.status);         
            if (objEntr.invitationdate != null) AddParameter(CpPackageVdrInfo.Field.invitationdate, objEntr.invitationdate);         
            if (objEntr.responsedate != null) AddParameter(CpPackageVdrInfo.Field.responsedate, objEntr.responsedate);         
            if (objEntr.mark != null) AddParameter(CpPackageVdrInfo.Field.mark, objEntr.mark);         
            if (objEntr.overal != null) AddParameter(CpPackageVdrInfo.Field.overal, objEntr.overal);         
            if (objEntr.contractno != null) AddParameter(CpPackageVdrInfo.Field.contractno, objEntr.contractno);         
            if (objEntr.techevaluate != null) AddParameter(CpPackageVdrInfo.Field.techevaluate, objEntr.techevaluate);         
            if (objEntr.bidprice != null) AddParameter(CpPackageVdrInfo.Field.bidprice, objEntr.bidprice);         
            if (objEntr.impactamt != null) AddParameter(CpPackageVdrInfo.Field.impactamt, objEntr.impactamt);         
            if (objEntr.externalamt != null) AddParameter(CpPackageVdrInfo.Field.externalamt, objEntr.externalamt);         
            if (objEntr.discountamt != null) AddParameter(CpPackageVdrInfo.Field.discountamt, objEntr.discountamt);         
            if (objEntr.vatcode != null) AddParameter(CpPackageVdrInfo.Field.vatcode, objEntr.vatcode);         
            if (objEntr.vatrate != null) AddParameter(CpPackageVdrInfo.Field.vatrate, objEntr.vatrate);         
            if (objEntr.vatamt != null) AddParameter(CpPackageVdrInfo.Field.vatamt, objEntr.vatamt);         
            if (objEntr.totalawardprice != null) AddParameter(CpPackageVdrInfo.Field.totalawardprice, objEntr.totalawardprice);         
            if (objEntr.ranking != null) AddParameter(CpPackageVdrInfo.Field.ranking, objEntr.ranking);         
            if (objEntr.pricedifference != null) AddParameter(CpPackageVdrInfo.Field.pricedifference, objEntr.pricedifference);         
            if (objEntr.pevaluation != null) AddParameter(CpPackageVdrInfo.Field.pevaluation, objEntr.pevaluation);         
            if (objEntr.commercialevaluation != null) AddParameter(CpPackageVdrInfo.Field.commercialevaluation, objEntr.commercialevaluation);         
            if (objEntr.techevaludation != null) AddParameter(CpPackageVdrInfo.Field.techevaludation, objEntr.techevaludation);         
            if (objEntr.comments != null) AddParameter(CpPackageVdrInfo.Field.comments, objEntr.comments);         
			if (objEntr.createdby != null) AddParameter(CpPackageVdrInfo.Field.createdby, objEntr.createdby);
			if (objEntr.updatedby != null) AddParameter(CpPackageVdrInfo.Field.updatedby, objEntr.updatedby);
          
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

        public string Update(CpPackageVdrInfo objEntr) {
            string sql = @"UPDATE cppackagevdr SET ";
			
            if (objEntr.status != null) { sql += "status = @status,"; }
            if (objEntr.invitationdate != null) { sql += "invitationdate = @invitationdate,"; }
            if (objEntr.responsedate != null) { sql += "responsedate = @responsedate,"; }
            if (objEntr.mark != null) { sql += "mark = @mark,"; }
            if (objEntr.overal != null) { sql += "overal = @overal,"; }
            if (objEntr.contractno != null) { sql += "contractno = @contractno,"; }
            if (objEntr.techevaluate != null) { sql += "techevaluate = @techevaluate,"; }
            if (objEntr.bidprice != null) { sql += "bidprice = @bidprice,"; }
            if (objEntr.impactamt != null) { sql += "impactamt = @impactamt,"; }
            if (objEntr.externalamt != null) { sql += "externalamt = @externalamt,"; }
            if (objEntr.discountamt != null) { sql += "discountamt = @discountamt,"; }
            if (objEntr.vatcode != null) { sql += "vatcode = @vatcode,"; }
            if (objEntr.vatrate != null) { sql += "vatrate = @vatrate,"; }
            if (objEntr.vatamt != null) { sql += "vatamt = @vatamt,"; }
            if (objEntr.totalawardprice != null) { sql += "totalawardprice = @totalawardprice,"; }
            if (objEntr.ranking != null) { sql += "ranking = @ranking,"; }
            if (objEntr.pricedifference != null) { sql += "pricedifference = @pricedifference,"; }
            if (objEntr.pevaluation != null) { sql += "pevaluation = @pevaluation,"; }
            if (objEntr.commercialevaluation != null) { sql += "commercialevaluation = @commercialevaluation,"; }
            if (objEntr.techevaludation != null) { sql += "techevaludation = @techevaludation,"; }
            if (objEntr.comments != null) { sql += "comments = @comments,"; }
			if (objEntr.updatedby != null) { sql += "updatedby = @updatedby,"; }

			sql += " lastupdate = getdate()";
            sql += @" WHERE  tvcdb = @tvcdb AND packagecode = @packagecode AND vendorcode = @vendorcode";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageVdrInfo.Field.tvcdb, objEntr.tvcdb);
            AddParameter(CpPackageVdrInfo.Field.packagecode, objEntr.packagecode);
            AddParameter(CpPackageVdrInfo.Field.vendorcode, objEntr.vendorcode);

            if (objEntr.status != null) AddParameter(CpPackageVdrInfo.Field.status, objEntr.status);         
            if (objEntr.invitationdate != null) AddParameter(CpPackageVdrInfo.Field.invitationdate, objEntr.invitationdate);         
            if (objEntr.responsedate != null) AddParameter(CpPackageVdrInfo.Field.responsedate, objEntr.responsedate);         
            if (objEntr.mark != null) AddParameter(CpPackageVdrInfo.Field.mark, objEntr.mark);         
            if (objEntr.overal != null) AddParameter(CpPackageVdrInfo.Field.overal, objEntr.overal);         
            if (objEntr.contractno != null) AddParameter(CpPackageVdrInfo.Field.contractno, objEntr.contractno);         
            if (objEntr.techevaluate != null) AddParameter(CpPackageVdrInfo.Field.techevaluate, objEntr.techevaluate);         
            if (objEntr.bidprice != null) AddParameter(CpPackageVdrInfo.Field.bidprice, objEntr.bidprice);         
            if (objEntr.impactamt != null) AddParameter(CpPackageVdrInfo.Field.impactamt, objEntr.impactamt);         
            if (objEntr.externalamt != null) AddParameter(CpPackageVdrInfo.Field.externalamt, objEntr.externalamt);         
            if (objEntr.discountamt != null) AddParameter(CpPackageVdrInfo.Field.discountamt, objEntr.discountamt);         
            if (objEntr.vatcode != null) AddParameter(CpPackageVdrInfo.Field.vatcode, objEntr.vatcode);         
            if (objEntr.vatrate != null) AddParameter(CpPackageVdrInfo.Field.vatrate, objEntr.vatrate);         
            if (objEntr.vatamt != null) AddParameter(CpPackageVdrInfo.Field.vatamt, objEntr.vatamt);         
            if (objEntr.totalawardprice != null) AddParameter(CpPackageVdrInfo.Field.totalawardprice, objEntr.totalawardprice);         
            if (objEntr.ranking != null) AddParameter(CpPackageVdrInfo.Field.ranking, objEntr.ranking);         
            if (objEntr.pricedifference != null) AddParameter(CpPackageVdrInfo.Field.pricedifference, objEntr.pricedifference);         
            if (objEntr.pevaluation != null) AddParameter(CpPackageVdrInfo.Field.pevaluation, objEntr.pevaluation);         
            if (objEntr.commercialevaluation != null) AddParameter(CpPackageVdrInfo.Field.commercialevaluation, objEntr.commercialevaluation);         
            if (objEntr.techevaludation != null) AddParameter(CpPackageVdrInfo.Field.techevaludation, objEntr.techevaludation);         
            if (objEntr.comments != null) AddParameter(CpPackageVdrInfo.Field.comments, objEntr.comments);         
			if (objEntr.updatedby != null) AddParameter(CpPackageVdrInfo.Field.updatedby, objEntr.updatedby);          
               
            string sErr = string.Empty;

            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

        public string Delete(String tvcdb, String packagecode, String vendorcode) {
            string sql = @"DELETE cppackagevdr
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode AND vendorcode = @vendorcode";

            openConnection();
            InitCommand(sql);

            AddParameter(CpPackageVdrInfo.Field.tvcdb, tvcdb);
            AddParameter(CpPackageVdrInfo.Field.packagecode, packagecode);
            AddParameter(CpPackageVdrInfo.Field.vendorcode, vendorcode);
              
            string sErr = string.Empty;
            try { executeNonQuery(); }
            catch (Exception ex) { sErr = string.Format("{0} sql='{1}'", ex.Message, sql); }
            finally { closeConnection(); }

            return sErr;
        }

		public string Clear(string db) {
            string sql = @"IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='cppackagevdr')
			DELETE cppackagevdr
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
        
        public Boolean IsExist(String tvcdb, String packagecode, String vendorcode, ref string sErr) {
            string sql = @"SELECT TOP (1) tvcdb, packagecode, vendorcode
            FROM  cppackagevdr
            WHERE 	 tvcdb = @tvcdb AND packagecode = @packagecode AND vendorcode = @vendorcode";
			
            InitConnect();
            InitCommand(sql);
            AddParameter(CpPackageVdrInfo.Field.tvcdb, tvcdb);              
            AddParameter(CpPackageVdrInfo.Field.packagecode, packagecode);              
            AddParameter(CpPackageVdrInfo.Field.vendorcode, vendorcode);              
            
            DataTable list = new DataTable();
            try { list = executeSelect(); }
            catch (Exception ex) { sErr = ex.Message + " sql='" + sql + "'"; }

			return list.Rows.Count == 1;
        }		
		
		#endregion Method
     
    }
}
