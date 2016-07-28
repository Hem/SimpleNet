using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNet.Jobs.Dtos
{
    // a job contains multiple tasks
    public class SimpleJob
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // List of parameteres used for runtime...
        // This is also used as an way to pass values from one task to another!
        public IList<SimpleParameter> Parameters { get; set; }

        // List of Tasks to perform for said job!
        public IList<SimpleTask> Tasks { get; set; }
    }


    public class SimpleParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public object TransformedValue { get; set; }
        public Type DataType { get; set; }

    }


    public enum DataType
    {
        String,
        Int,
        Decimal,
        Date,
        DateTime,
        DataTable,
        Object,
        ArrayOfObjects
    }

    public class SimpleTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TaskType TaskType { get; set; }

        public IList<SimpleParameter> Parameters { get; set; }



        // This is what the Task Runner returns!
        public object TaskResponse { get; set; }

        // This is the variable name for the task response!
        public string ResponseVarName { get; set; }


    }

    public enum TaskType
    {
        Custom,

        BatchFile,

        SqlCmd,

        SqlScript,

        SqlProc,

        DataToExcel,

        SendEmail,

    }

}
