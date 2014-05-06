using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;

namespace SQLBuilder
{
    public struct MyColum
    {
        public string columName;
        public string dbType;
        public int dLen;
        public bool isKey;
        public Type dataType;
        public object defVal;
    }

    public struct MyTable
    {
        public string tbName;
        public MyColum[] colums;
    }

    public partial class Form1 : Form
    {
        private List<MyTable> tbList;
        private Dictionary<string, string> confDic;

        public Form1()
        {
            InitializeComponent();
            tbList = new List<MyTable>();
            confDic = new Dictionary<string, string>();
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SQLStrTB.Text))
            {
                return;
            }
            DbHelperSQL.connectionString = SQLStrTB.Text;
            //无奈，与直接在数据库读取的数据不一致
            string SQL = @"
                SELECT
	                case 
	                when 
		                exists(
			                SELECT 1 
			                FROM sysobjects 
			                where xtype= 'PK' and name in ( 
				                SELECT name 
				                FROM sysindexes 
				                WHERE indid in( 
					                SELECT indid 
					                FROM sysindexkeys 
					                WHERE id = c.id AND colid=c.colid ))) 
	                then 
		                1
	                else 
		                0 
	                end AS isKey, 
	                i.TABLE_NAME AS tn,
	                i.COLUMN_NAME AS cn,
	                i.DATA_TYPE AS dataType,
	                i.CHARACTER_MAXIMUM_LENGTH AS dataLen,
	                i.COLUMN_DEFAULT AS dataDefault,
	                CASE COLUMNPROPERTY(object_id(i.TABLE_NAME),i.COLUMN_NAME, 'IsIdentity')
	                WHEN 1 THEN
		                1
	                ELSE
		                0
	                END AS isIdentify
                FROM INFORMATION_SCHEMA.COLUMNS i
                INNER JOIN syscolumns c 
                ON object_id(i.TABLE_NAME) = c.id AND i.COLUMN_NAME=c.name
                ORDER BY c.id";
            SqlDataReader sdr = DbHelperSQL.ExecuteReader(SQL);
            int top = 0;
            int left = 0;
            string tbName = "";
            int tbIndex = 0;
            List<MyColum> colums = null;
            MyTable mt = new MyTable();
            //创建checkAll控件
            CheckBox AllCB = (CheckBox)BuildTBCK("Check All", -1, top, left);
            AllCB.CheckedChanged += new EventHandler(AllCB_CheckedChanged);
            TBList.Controls.Add(AllCB);
            left += 120;

            while (sdr.Read())
            {
                string tmp = sdr["tn"].ToString();
                if (tbName != tmp)
                {
                    //添加tbList
                    if (colums != null)
                    {
                        mt.colums = colums.ToArray();
                        tbList.Add(mt);
                    }
                    //初始化myTable
                    tbName = tmp;
                    mt = new MyTable();
                    colums = new List<MyColum>();
                    mt.tbName = tbName;
                    //添加CheckBox
                    TBList.Controls.Add(BuildTBCK(tbName, tbIndex, top, left));
                    left += 120;
                    if (left >= 360)
                    {
                        top += 20;
                        left = 0;
                    }
                    ++tbIndex;
                }
                Type tmpType = null;
                try
                {
                    tmpType = JudgeType(sdr["dataType"].ToString());
                }
                catch
                {
                    continue;
                }
                MyColum mc = new MyColum();
                mc.columName = sdr["cn"].ToString();
                mc.dbType = sdr["dataType"].ToString();
                if (mc.dbType != "image")
                {
                    mc.dLen = sdr["dataLen"] == DBNull.Value ? 0 : int.Parse(sdr["dataLen"].ToString());
                }
                else
                {
                    mc.dLen = 0;
                }
                mc.dataType = tmpType;
                try
                {
                    if (sdr["dataDefault"] != DBNull.Value)
                    {
                        //格式为(?)
                        string tmpSQLDV = sdr["dataDefault"].ToString();
                        tmpSQLDV = tmpSQLDV.Substring(1, tmpSQLDV.Length - 2);
                        if (mc.dataType == typeof(String))
                        {
                            //格式为N'?'
                            tmpSQLDV = tmpSQLDV.Substring(tmpSQLDV.IndexOf('\'') + 1, tmpSQLDV.LastIndexOf('\'') - tmpSQLDV.IndexOf('\'') - 1);
                            //tmpSQLDV = tmpSQLDV.Substring(2, tmpSQLDV.Length - 3);
                            mc.defVal = tmpSQLDV;
                        }
                        else if (mc.dataType == typeof(DateTime))
                        {
                            mc.defVal = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            //数字格式为(?)
                            tmpSQLDV = tmpSQLDV.Substring(1, tmpSQLDV.Length - 2);
                            MethodInfo mi = tmpType.GetMethod("Parse", new Type[] { typeof(String) });
                            mc.defVal = mi.Invoke(null, new object[] { tmpSQLDV });
                        }
                    }
                    else
                    {
                        mc.defVal = System.Activator.CreateInstance(tmpType);
                    }

                }
                catch (MissingMemberException ex)
                {
                    if (mc.dataType == typeof(String))
                    {
                        mc.defVal = "";
                    }
                    else
                    {
                        throw new Exception(tmpType.FullName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(tmpType.FullName);
                }
                mc.isKey = sdr["isKey"].ToString() == "1";
                colums.Add(mc);
            }
            if (colums != null)
            {
                mt.colums = colums.ToArray();
                tbList.Add(mt);
            }
            sdr.Close();
        }

        void AllCB_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox AllCB = (CheckBox)sender;
            foreach (Control control in TBList.Controls)
            {
                (control as CheckBox).Checked = AllCB.Checked;
            }
        }

        private Type JudgeType(string sqlTypeStr)
        {
            string tmpStr = sqlTypeStr.ToLower();
            if(tmpStr == SqlDbType.TinyInt.ToString().ToLower()
                || tmpStr == SqlDbType.Bit.ToString().ToLower())
            {
                return typeof(Byte);
            }
            else if(tmpStr == SqlDbType.SmallInt.ToString().ToLower())
            {
                return typeof(Int16);
            }
            else if(tmpStr == SqlDbType.Int.ToString().ToLower())
            {
                return typeof(Int32);
            }
            else if(tmpStr == SqlDbType.BigInt.ToString().ToLower())
            {
                return typeof(Int64);
            }
            else if(tmpStr == SqlDbType.Binary.ToString().ToLower() 
                || tmpStr == SqlDbType.Image.ToString().ToLower() 
                || tmpStr == SqlDbType.VarBinary.ToString().ToLower())
            {
                return typeof(Int16);
            }
            else if(tmpStr == SqlDbType.Char.ToString().ToLower() 
                || tmpStr == SqlDbType.NChar.ToString().ToLower()
                || tmpStr == SqlDbType.NText.ToString().ToLower()
                || tmpStr == SqlDbType.NVarChar.ToString().ToLower()
                || tmpStr == SqlDbType.Text.ToString().ToLower()
                || tmpStr == SqlDbType.VarChar.ToString().ToLower())
            {
                return typeof(String);
            }
            else if (tmpStr == SqlDbType.Decimal.ToString().ToLower()
                || tmpStr == SqlDbType.Float.ToString().ToLower()
                || tmpStr == SqlDbType.Money.ToString().ToLower()
                || tmpStr == SqlDbType.Real.ToString().ToLower())
            {
                return typeof(Double);
            }
            else if (tmpStr == SqlDbType.SmallDateTime.ToString().ToLower()
                || tmpStr == SqlDbType.DateTime.ToString().ToLower())
            {
                return typeof(DateTime);
            }
            else
            {
                throw new Exception("no Type");
            }
        }

        private Control BuildTBCK(string TBName,int tbIndex,int top,int left)
        {
            CheckBox cb = new CheckBox();
            cb.Text = TBName;
            cb.Checked = true;
            cb.Top = top;
            cb.Left = left;
            cb.Tag = tbIndex;
            return cb;
        }

        /// <summary>
        /// 构造DeleteRow的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildDeleteRowStore(MyTable mt, MyColum keyColum)
        {
            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_DeleteRow]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_DeleteRow]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result += 
                "CREATE PROCEDURE sp_" + mt.tbName + "_DeleteRow\r\n" +
                GetDBParamsStr(mt, true, false) +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tDELETE " + mt.tbName + " WHERE [" + keyColum.columName + "] = @" + keyColum.columName + "\r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        /// <summary>
        /// 构造Insert的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildInsertStore(MyTable mt, MyColum keyColum)
        {
            string str = "";
            string str1 = "";
            foreach (MyColum mc in mt.colums)
            {
                if (mc.isKey)
                {
                    continue;
                }
                str += "[" + mc.columName + "],";
                str1 += "@" + mc.columName + ",";
            }
            str = str.Trim(',');
            str1 = str1.Trim(',');

            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_Insert]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_Insert]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_Insert\r\n" +
                GetDBParamsStr(mt, false, true) +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tINSERT INTO " + mt.tbName + "\r\n" +
                "\t\t(" + str + ")\r\n" +
                "\tVALUES\r\n" +
                "\t\t(" + str1 + ")\r\n" +
                "\tDECLARE @ReferenceID int\r\n" +
                "\tSELECT @ReferenceID = @@IDENTITY\r\n" +
                "\tReturn @ReferenceID \r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        /// <summary>
        /// 构造SelectAll的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildSelectAllStore(MyTable mt, MyColum keyColum)
        {
            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_SelectAll]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_SelectAll]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_SelectAll\r\n" +
                GetDBParamsStr(mt, false, false) +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tSELECT * FROM " + mt.tbName + "\r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        /// <summary>
        /// 构造SelectRow的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildSelectRowStore(MyTable mt, MyColum keyColum)
        {
            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_SelectRow]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_SelectRow]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_SelectRow\r\n" +
                GetDBParamsStr(mt, true, false) +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tSELECT * FROM " + mt.tbName + "\r\n" +
                "\tWHERE [" + keyColum.columName + "] = " + "@" + keyColum.columName + "\r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        /// <summary>
        /// 构造Update的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildUpdateStore(MyTable mt, MyColum keyColum)
        {
            string str = "";
            string paramsStr = GetDBParamsStr(mt, true, false) + ",";
            string tmpSQLStr = "";
            string tmpStr1 = "@" + keyColum.columName + " " + keyColum.dbType + ",";
            string tmpStr2 = "@" + keyColum.columName + ",";
            foreach (MyColum mc in mt.colums)
            {
                if (mc.isKey)
                {
                    continue;
                }
                paramsStr += "\t@" + mc.columName + " " + mc.dbType + " = NULL,\r\n";
                tmpSQLStr +=
                    "\tIF @" + mc.columName + " IS NULL\r\n" +
                    "\tBEGIN\r\n" +
                    "\t\tSET @SQLStr = @SQLStr + " + "'[" + mc.columName + "] = [" + mc.columName + "],'\r\n" +
                    "\tEND\r\n" +
                    "\tELSE\r\n" +
                    "\tBEGIN\r\n" +
                    "\t\tSET @SQLStr = @SQLStr + " + "'[" + mc.columName + "] = @" + mc.columName + ",'\r\n" +
                    "\tEND\r\n";
                str += "[" + mc.columName + "] = @" + mc.columName + ",";
                tmpStr1 += "@" + mc.columName + " " + mc.dbType + ",";
                tmpStr2 += "@" + mc.columName + ",";
            }
            str = str.Trim(',');
            //去除最后的","
            paramsStr = paramsStr.Remove(paramsStr.Length - 3, 1);
            tmpStr1 = tmpStr1.Trim(',');
            tmpStr2 = tmpStr2.Trim(',');

            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_Update]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_Update]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_Update\r\n" +
                paramsStr +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tDECLARE @SQLStr NVARCHAR(4000)\r\n" +
                "\tSET @SQLStr = N'UPDATE " + mt.tbName + " SET '\r\n" +
                tmpSQLStr +
                "\tSET @SQLStr = Left(@SQLStr,Len(@SQLStr) - 1) + ' WHERE [" + keyColum.columName + "] = @" + keyColum.columName + "'\r\n" +
                "EXEC sp_executesql @SQLStr, N'" + tmpStr1 + "', " + tmpStr2 + "\r\n" + 
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        /// <summary>
        /// 构造DeleteByWhere的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildDeleteByWhereStore(MyTable mt, MyColum keyColum)
        {
            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_DeleteByWhere]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_DeleteByWhere]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_DeleteByWhere\r\n" +
                "\t@where VARCHAR(3500)\r\n" +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tSET NOCOUNT ON\r\n" +
                "\tDECLARE @SQL NVARCHAR(4000)\r\n" +
                "\tSET @SQL = N'DELETE " + mt.tbName + " WHERE ' + @where\r\n" +
                "\tEXEC sp_executesql @SQL\r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        /// <summary>
        /// 构造SelectByWhere的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildSelectByWhereStore(MyTable mt, MyColum keyColum)
        {
            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_SelectByWhere]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_SelectByWhere]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_SelectByWhere\r\n" +
                "\t@Where VARCHAR(3500)\r\n" +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tSET NOCOUNT ON\r\n" +
                "\tDECLARE @SQL NVARCHAR(4000)\r\n" +
                "\tSET @SQL = 'SELECT * FROM " + mt.tbName + " WHERE ' + @Where\r\n" +
                "\tEXEC sp_executesql @SQL\r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        /// <summary>
        /// 构造SelectRangeByWhere的存储过程
        /// </summary>
        /// <returns></returns>
        public string BuildSelectRangeByWhereStore(MyTable mt, MyColum keyColum)
        {
            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_SelectRangeByWhere]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_SelectRangeByWhere]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }
            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_SelectRangeByWhere\r\n" +
                "\t@RowIndex INT,\r\n" +
                "\t@Count INT,\r\n" +
                "\t@Where VARCHAR(3500),\r\n" +
                "\t@DescStr VARCHAR(50) = '" + keyColum.columName + "',\r\n" +
                "\t@DescTag BIT,\r\n" +
                "\t@RowCount INT OUTPUT\r\n" +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tSET NOCOUNT ON\r\n" +
                "\tDECLARE @FOrder NVARCHAR(4),@BOrder NVARCHAR(4)\r\n" +
                "\tIF @DescTag = 1\r\n" +
                "\tBEGIN\r\n" +
                "\t\tSET @FOrder = N'ASC'\r\n" +
                "\t\tSET @BOrder = N'DESC'\r\n" +
                "\tEND\r\n" +
                "\tELSE\r\n" +
                "\tBEGIN\r\n" +
                "\t\tSET @FOrder = N'DESC'\r\n" +
                "\t\tSET @BOrder = N'ASC'\r\n" +
                "\tEND\r\n" +
                "\tDECLARE @WhereSQL NVARCHAR(4000)\r\n" +
                "\tSET @WhereSQL = N''\r\n" +
                "\tIF Len(@Where) > 0\r\n" +
                "\tBEGIN\r\n" +
                "\t\tSET @WhereSQL = N'WHERE ' + @Where\r\n" +
                "\tEND\r\n" +
                "\tDECLARE @SQL NVARCHAR(2000)\r\n" +
                "\tSET @SQL = N'\r\n" +
                "\tSELECT @RowCount = Count(1) FROM " + mt.tbName + " ' + @WhereSQL + N'\r\n" +
                "\tIF @RowIndex < @RowCount\r\n" +
                "\tBEGIN\r\n" +
                "\t\tIF (@RowCount - @RowIndex) < @Count\r\n" +
                "\t\tBEGIN\r\n" +
                "\t\t\tSET @Count = @RowCount - @RowIndex\r\n" +
                "\t\tEND\r\n" +
                "\t\tDECLARE @FC INT\r\n" +
                "\t\tSET @FC = @RowCount - @RowIndex\r\n" +
                "\t\tSELECT TOP(@Count) * \r\n" +
                "\t\tFROM (SELECT TOP(@FC) * FROM " + mt.tbName + " ' + @WhereSQL + '\r\n" +
                "\t\tORDER BY ' + @DescStr + ' ' + @FOrder\r\n" +
                "\tIF @DescStr <> N'" + keyColum.columName + "'\r\n" +
                "\tBEGIN\r\n" +
                "\t\tSET @SQL = @SQL + '," + keyColum.columName + " ' + @FOrder\r\n" +
                "\tEND\r\n" +
                "\tSET @SQL = @SQL + ') AS tb ORDER BY ' + @DescStr + ' ' + @BOrder\r\n" +
                "\tIF @DescStr <> N'" + keyColum.columName + "'\r\n" +
                "\tBEGIN\r\n" +
                "\t\tSET @SQL = @SQL + '," + keyColum.columName + " ' + @BOrder\r\n" +
                "\tEND\r\n" +
                "\tSET @SQL = @SQL  + '\r\n" +
                "\tEND'\r\n" +
                "\tEXEC sp_executesql @SQL, N'@RowCount INT OUTPUT, @RowIndex INT, @Count INT, @DescTag Bit',@RowCount OUTPUT,@RowIndex,@Count,@DescTag\r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        public string BuildInsertUpdateStore(MyTable mt, MyColum keyColum)
        {
            string result = "";
            if (ReplaceCB.Checked)
            {
                result +=
                    "IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_" + mt.tbName + "_InsertUpdate]') AND type in (N'P', N'PC'))\r\n" +
                    "BEGIN\r\n" +
                    "\tDROP PROCEDURE [dbo].[sp_" + mt.tbName + "_InsertUpdate]\r\n" +
                    "END\r\n" +
                    "GO\r\n";
            }

            result +=
                "CREATE PROCEDURE sp_" + mt.tbName + "_InsertUpdate\r\n" +
                GetDBParamsStr(mt, true, true) +
                "AS\r\n" +
                "BEGIN\r\n" +
                "\tSET NOCOUNT ON\r\n" +
                "\tDECLARE @Count INT\r\n" +
                "\tSELECT @Count = Count(" + keyColum.columName + ") FROM " + mt.tbName + " WHERE " + keyColum.columName + " = @" + keyColum.columName + "\r\n" +
                "\tIF @Count > 0\r\n" +
                "\tBEGIN\r\n" +
                "\t\tUPDATE " + mt.tbName + " SET \r\n";
            foreach (MyColum mc in mt.colums)
            {
                if (mc.isKey)
                {
                    continue;
                }
                result += "\t\t\t[" + mc.columName + "] = @" + mc.columName + ",\r\n";
            }
            result = result.Remove(result.Length - 3, 1);
            result +=
                "\t\tWHERE " + keyColum.columName + " = @" + keyColum.columName + "\r\n" +
                "\tEND\r\n" +
                "\tELSE\r\n" +
                "\tBEGIN\r\n" +
                "\t\tINSERT " + mt.tbName + " VALUES(\r\n";
            foreach (MyColum mc in mt.colums)
            {
                if (mc.isKey)
                {
                    continue;
                }
                result += "\t\t\t@" + mc.columName + ",\r\n";
            }
            result = result.Remove(result.Length - 3, 1);
            result += ")\r\n" +
                "\tEND\r\n" +
                "END\r\n" +
                "GO\r\n\r\n";
            return result;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            //生成字符串
            foreach (Control con in TBList.Controls)
            {
                StringBuilder tmpStr = new StringBuilder(1024);
                CheckBox cb = con as CheckBox;
                if ((int)cb.Tag == -1)
                {
                    continue;
                }
                if (cb.Checked)
                {
                    int tbIndex = (int)cb.Tag;
                    try
                    {
                        MyTable mt = tbList[tbIndex];
                        //获得key的colum
                        MyColum keyColum = mt.colums.First(r => r.isKey);
                        if (DeleteRowCB.Checked)
                        {
                            //创建 DeleteRow
                            tmpStr.Append(BuildDeleteRowStore(mt, keyColum));
                        }
                        if (InsertCB.Checked)
                        {
                            //创建 Insert
                            tmpStr.Append(BuildInsertStore(mt, keyColum));
                        }
                        if (SelectAllCB.Checked)
                        {
                            //创建 SelectAll
                            tmpStr.Append(BuildSelectAllStore(mt, keyColum));
                        }
                        if (SelectRowCB.Checked)
                        {
                            //创建 SelectRow
                            tmpStr.Append(BuildSelectRowStore(mt, keyColum));
                        }
                        if (UpdateCB.Checked)
                        {
                            //创建 Update
                            tmpStr.Append(BuildUpdateStore(mt, keyColum));
                        }
                        if (DeleteByWhereCB.Checked)
                        {
                            //创建 DeleteByWhere
                            tmpStr.Append(BuildDeleteByWhereStore(mt, keyColum));
                        }
                        if (SelectByWhereCB.Checked)
                        {
                            //创建 SelectByWhere
                            tmpStr.Append(BuildSelectByWhereStore(mt, keyColum));
                        }
                        if (SelectRangeByWhereCB.Checked)
                        {
                            //创建 SelectRangeByWhere
                            tmpStr.Append(BuildSelectRangeByWhereStore(mt, keyColum));
                        }
                        if (InsertUpdateCB.Checked)
                        {
                            //创建 InsertUpdate
                            tmpStr.Append(BuildInsertUpdateStore(mt, keyColum));
                        }
                        SQLTB.Text += tmpStr.ToString();
                    }
                    catch(Exception ex)
                    {
                        //tag与cbIndex相差1
                        CheckBox tmpCB = (CheckBox)TBList.Controls[tbIndex + 1];
                        tmpCB.ForeColor = Color.Red;
                        continue;
                    }
                }
            }
        }

        public string GetDBParamsStr(MyTable mt, bool haveKey, bool haveMember)
        {
            StringBuilder sb = new StringBuilder(100);
            foreach (MyColum mc in mt.colums)
            {
                bool tag = false;
                if (mc.isKey)
                {
                    if (haveKey)
                    {
                        tag = true;
                    }
                }
                else
                {
                    if (haveMember)
                    {
                        tag = true;
                    }
                }
                if (tag)
                {
                    string defValue = mc.defVal.ToString();
                    if (mc.dataType == typeof(String))
                    {
                        defValue = "'" + defValue + "'";
                    }
                    else if (mc.dbType == "image")
                    {
                        defValue = "00" + defValue;
                        defValue = "0x" + defValue.Substring(defValue.Length - 2, 2);
                    }
                    sb.Append("\t@" + mc.columName + " " + mc.dbType + (mc.dLen == 0 ? "" : "(" + (mc.dLen == -1 ? "MAX" : mc.dLen.ToString()) + ")") + " = " + defValue + ",\r\n");
                }
            }
            if (sb.Length == 0)
            {
                return "";
            }
            //去除最后的","
            sb.Remove(sb.Length - 3, 1);
            return sb.ToString();
        }

        private void AppBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void SAllBtn_Click(object sender, EventArgs e)
        {
            SQLTB.SelectionStart = 0;
            SQLTB.SelectionLength = SQLTB.Text.Length;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            //把tb里的内容保存成SQL后缀的文件
            if (sfDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(sfDialog.FileName);
                FileStream fs = fi.Open(FileMode.Create);
                byte[] buffers = Encoding.Unicode.GetBytes(SQLTB.Text);
                fs.Write(new byte[] { 0xFF, 0xFE }, 0, 2);
                fs.Write(buffers, 0, buffers.Length);
                fs.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //读取Ini文件
            //获得最后一次的ConnectionString值
            string iniFP = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "config.ini";
            if (!File.Exists(iniFP))
            {
                File.Create(iniFP);
            }
            else
            {
                FileStream fs = File.OpenRead(iniFP);
                byte[] buffers = new byte[fs.Length];
                fs.Read(buffers, 0, (int)fs.Length);
                string str = Encoding.ASCII.GetString(buffers);
                string[] strArr = str.Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tmpStr in strArr)
                {
                    int index = tmpStr.IndexOf('=');
                    if (index < 0)
                    {
                        //格式错误
                        continue;
                    }
                    string keyStr = tmpStr.Substring(0, index);
                    string valStr = tmpStr.Substring(index + 1);
                    confDic.Add(keyStr.Trim(), valStr.Trim());
                }
                fs.Close();
            }
            if (confDic.Keys.Contains("ConnStr"))
            {
                SQLStrTB.Text = confDic["ConnStr"];
            }
            else
            {
                confDic.Add("ConnStr", "");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //保存Ini文件
            confDic["ConnStr"] = SQLStrTB.Text;
            string iniFP = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "config.ini";
            if (!File.Exists(iniFP))
            {
                File.Create(iniFP);
            }
            FileStream fs = File.OpenWrite(iniFP);

            foreach (KeyValuePair<string, string> kv in confDic)
            {
                string tmpStr = kv.Key + " = " + kv.Value + "\r\n";
                byte[] buffers = Encoding.ASCII.GetBytes(tmpStr);
                fs.Write(buffers, 0, buffers.Length);
            }
            fs.Close();
        }
    }
}
