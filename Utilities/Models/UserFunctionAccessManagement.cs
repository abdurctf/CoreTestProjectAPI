using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Models
{
    public class UserFunctionAccessManagement
    {
        public string FunctionId { get; set; }
        public string FunctionName { get; set; }
        public int HeadOfficeFunctionFlag { get; set; }
        public int AllowAddFlag { get; set; }
        public int AllowEditFlag { get; set; }
        public int AllowDeleteFlag { get; set; }
        public int AllowViewFlag { get; set; }
        public int AllowAuthFlag { get; set; }
        public int AllowProcessFlag { get; set; }
        public int AllowReportViewFlag { get; set; }
        public int AllowReportPrintFlag { get; set; }
        public int AllowReportGenFlag { get; set; }
        public int AllowAnyBranchOperationFlag { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string TargetPath { get; set; }
        public string FunctionGroupId { get; set; }
        public string ItemType { get; set; }
        public string FastPathNo { get; set; }
        public int IsFinancial { get; set; }
    }
    public class UserFavoriteMenu
    {
        public string UserId { get; set; }
        public string FunctionId { get; set; }
    }
}
