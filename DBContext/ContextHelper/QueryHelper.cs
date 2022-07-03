//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Oracle.EntityFrameworkCore.Scaffolding.Internal;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Linq;
//using System.Text;

//namespace Core.AuthorizationContexts.ContextHelper
//{
//    public class QueryHelper
//    {
//        public static List<T> SqlQuery<T>(string query, SecurityContext context, Func<DbDataReader, T> map)
//        {
//            using var command = context.Database.GetDbConnection().CreateCommand();
//            command.CommandText = query;
//            command.CommandType = CommandType.Text;

//            context.Database.OpenConnection();

//            using (var result = command.ExecuteReader())
//            {
//                var entities = new List<T>();

//                while (result.Read())
//                {
//                    entities.Add(map(result));
//                }

//                return entities;
//            }
//        }

//        public static List<T> SqlQuery<T>(string query, SecurityContext context, IMapper _mapper)
//        {
//            using var command = context.Database.GetDbConnection().CreateCommand();
//            command.CommandText = query;
//            command.CommandType = CommandType.Text;

//            context.Database.OpenConnection();

//            using (var result = command.ExecuteReader())
//            {
//                var entities = new List<T>();

//                while (result.Read())
//                {
//                   // var entity = _mapper.Map<T>(result.GetValues());
//                    entities.Add(_mapper.Map<T>(result));
//                }
//                //entities = _mapper.Map<List<T>>(result);
//                //var dt = new DataTable();
//                //dt.Load(result);
//                //List<DataRow> dr = dt.AsEnumerable().ToList();
//                //entities = _mapper.Map<List<T>>(dr);
//                return entities;
//            }
//        }
//    }
//}
