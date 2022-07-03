using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Utilities.CommonState
{
    [Serializable]
    public abstract class ModelBase<T> where T : ModelBase<T>
    {
        public ModelBase()
        {
            isAdd = true;
            isDelete = false;
            isOld = false;
        }
        [NotMapped]
        public bool IsRequestSuccess { get; set; } = false;
        [NotMapped]
        public string Error_Msg { get; set; }
        [NotMapped]
        public string Error_Code { get; set; }

        [BUAttributes(isNftLog = false)]
        [NotMapped]
        public T CloneObj { get; set; }

        [BUAttributes(isNftLog = false)]
        [NotMapped]
        public bool isAdd { get; set; }

        [BUAttributes(isNftLog = false)]
        [NotMapped]
        public bool isDelete { get; set; }

        [BUAttributes(isNftLog = false)]
        [NotMapped]
        public bool isOld { get; set; }

        [BUAttributes(isNftLog = false)]
        [NotMapped]
        public string CreateByLoginId { get; set; }
        [BUAttributes(isNftLog = false)]
        [NotMapped]
        public string MakeByLoginId { get; set; }
        [BUAttributes(isNftLog = false)]
        [NotMapped]
        public string AuthByLoginId { get; set; }
        public void MarkOld()
        {
            isAdd = false;
            isDelete = false;
            isOld = true;
        }

        public void setClone()
        {
            ////CloneObj = ObjectCopier.Clone(O);

            //CloneObj =  (T)MemberwiseClone();

            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                CloneObj = (T)formatter.Deserialize(ms);
            }
        }

        //public bool checkUnauthorized(System.Data.Entity.Database datbase)
        //{
        //    try
        //    {
        //        Type t = this.GetType();
        //        PropertyInfo prop = t.GetProperty("Buid");
        //        string vBuid = prop.GetValue(this).ToString();
        //        string vTableName = ((TableAttribute)((this.GetType().GetCustomAttributes(typeof(TableAttribute), true))).First()).Name;
        //        string TableSqlQuery = @"Select count(*) as ISUNAUTH from  " + vTableName + " where Buid ='" + vBuid + @"' and AUTH_STATUS_ID='U'";
        //        var count = datbase.SqlQuery<int>(TableSqlQuery).First();
        //        if (count.ToString() == "0")
        //            return false;
        //        else
        //            return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}
