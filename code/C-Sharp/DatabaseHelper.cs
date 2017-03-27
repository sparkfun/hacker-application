using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Wcss
{
    public class _DatabaseCommandHelper
    {
        public static SubSonic.QueryCommand CreateQueryCommand()
        {
            return _DatabaseCommandHelper.CreateQueryCommand(string.Empty);
        }
        public static SubSonic.QueryCommand CreateQueryCommand(StringBuilder sb)
        {
            return _DatabaseCommandHelper.CreateQueryCommand(sb.ToString());
        }
        public static SubSonic.QueryCommand CreateQueryCommand(string sql)
        {
            return new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
        }


        public SubSonic.QueryCommand Cmd { get; set; }

        //Constructors
        public _DatabaseCommandHelper()
        {
            Cmd = _DatabaseCommandHelper.CreateQueryCommand();
        }
        public _DatabaseCommandHelper(string sql)
        {
            Cmd = _DatabaseCommandHelper.CreateQueryCommand(sql);
        }
        public _DatabaseCommandHelper(StringBuilder sql)
        {
            Cmd = _DatabaseCommandHelper.CreateQueryCommand(sql);
        }


        public void AddCmdParameter(string paramName, object paramValue, System.Data.DbType dbtype)
        {
            Cmd.Parameters.Add(string.Format("@{0}", paramName.TrimStart('@')), paramValue, dbtype);
        }

        /// <summary>
        /// Be sure to encapsulte in a using statement - don't leave this object open
        /// </summary>
        /// <returns></returns>
        public System.Data.IDataReader GetReader()
        {
            return SubSonic.DataService.GetReader(this.Cmd);
        }

        public void PopulateCollectionByReader<T>(T item)
        {
            //item.LoadAndCloseReader(SubSonic.DataService.GetReader(this.Cmd));
            object[] Parameters = new object[1];
            Parameters[0] = SubSonic.DataService.GetReader(this.Cmd);

            item.GetType().InvokeMember("LoadAndCloseReader",
                System.Reflection.BindingFlags.InvokeMethod|System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public,
                null, item, Parameters);
        }
        public List<ListItem> LoadListItemList()
        {
            List<ListItem> list = new List<ListItem>();

            using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(this.Cmd))
            {
                //bool init = false;
                while (dr.Read())
                {
                    list.Add(new ListItem(dr.GetValue(dr.GetOrdinal("Name")).ToString(), dr.GetValue(dr.GetOrdinal("Id")).ToString()));
                }

                dr.Close();
            }

            return list;
        }
        public int PerformQuery(string objectName)
        {
            try
            {
                return (int)SubSonic.DataService.ExecuteScalar(this.Cmd);
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                _Error.LogException(sex);
                throw new Exception(string.Format("{0} Sql Error.\r\n{1}\r\n{2}", objectName, sex.Message, sex.StackTrace));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("{0} Error.\r\n{1}\r\n{2}", objectName, ex.Message, ex.StackTrace));
            }
        }
    }
}