using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DBContext.ContextHelper
{
    public class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly DynamicParameters dynamicParameters = new DynamicParameters();
        private readonly List<OracleParameter> oracleParameters = new List<OracleParameter>();

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction, object value = null, int? size = null)
        {
            OracleParameter oracleParameter;
            if (direction != ParameterDirection.ReturnValue)
            {
                if (size.HasValue)
                {
                    oracleParameter = new OracleParameter(name, oracleDbType, size.Value, value, direction);
                }
                else
                {
                    oracleParameter = new OracleParameter(name, oracleDbType, value, direction);
                }
            }
            else
            {
                if (size.HasValue)
                {
                    oracleParameter = (new OracleParameter(name, oracleDbType, size.Value));
                    oracleParameter.Direction = ParameterDirection.ReturnValue;
                }
                else
                {
                    oracleParameter = (new OracleParameter(name, oracleDbType));
                    oracleParameter.Direction = ParameterDirection.ReturnValue;
                    //oracleParameter = new OracleParameter(name, oracleDbType, value: value);
                    // oracleParameter.Direction = ParameterDirection.ReturnValue;
                }
            }

            oracleParameters.Add(oracleParameter);
        }

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType, direction);
            oracleParameters.Add(oracleParameter);
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            ((SqlMapper.IDynamicParameters)dynamicParameters).AddParameters(command, identity);

            var oracleCommand = command as OracleCommand;

            if (oracleCommand != null)
            {
                oracleCommand.Parameters.AddRange(oracleParameters.ToArray());
            }
        }

        public T Get<T>(string parameterName)
        {
            var parameter = oracleParameters.SingleOrDefault(t => t.ParameterName == parameterName);
            if (parameter != null)
            {
                // object convertible = (object)(parameter.Value as IConvertible);
                //return (T)Convert.ChangeType(parameter.Value, typeof(T));

                return (T)Convert.ChangeType(parameter.Value.ToString(), typeof(T));
            }
                
            return default(T);
        }

        public T Get<T>(int index)
        {
            var parameter = oracleParameters[index];
            if (parameter != null)
                return (T)Convert.ChangeType(parameter.Value, typeof(T));
            return default(T);
        }
        public dynamic Get(dynamic parameterName)
        {
            var parameter = oracleParameters.SingleOrDefault(t => t.ParameterName == parameterName);
            if (parameter != null)
                return parameter.Value;
            return null;
        }
    }
}
