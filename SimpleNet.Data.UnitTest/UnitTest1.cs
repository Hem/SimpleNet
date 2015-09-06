using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.Net.Core.Data.UnitTest.Dto;
using SimpleNet.Data.Mapper;
using SimpleNet.Data.Repository;

namespace Sample.Net.Core.Data.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        static UnitTest1()
        {
            var path = Path.GetFullPath(@"..\..\App_Data");

            AppDomain.CurrentDomain.SetData("DataDirectory", path );
        }

        [TestMethod]
        public void TestReadDataTable()
        {
            var dal = new SimpleDataAccess("Simple");

            var dt = dal.ReadSql(@"SELECT * FROM State", null);

            Assert.IsNotNull(dt);
            Assert.IsTrue(dt.Rows.Count > 0);
        }

        [TestMethod]
        public void TestReadDataTableAsync()
        {
            var dal = new SimpleDataAccess("Simple");

            var task = dal.ReadSqlAsync(@"SELECT * FROM STATE", null);

            task.Wait();

            Assert.IsTrue( task.Result.Rows.Count > 0); 
        }



        [TestMethod]
        public void TestReadDataAccessorWithParam()
        {
            var dal = new SimpleDataAccess("Simple");

            var mapper = MapBuilder<State>.BuildAllProperties();

            var records = dal.Read<State>(mapper, @"SELECT * FROM STATE WHERE Code = @Code", CommandType.Text, new []
            {
                dal.GetDbParameter("@Code", "NC")
            }).ToArray();

            Assert.IsTrue(records.Any());

            Assert.IsTrue(records.All(x => x.Code.Equals("NC")));
        }






        [TestMethod]
        public void TestReadDataAccessor()
        {
            var dal = new SimpleDataAccess("Simple");

            var mapper = MapBuilder<State>.BuildAllProperties();

            var records = dal.Read<State>(mapper, @"SELECT * FROM STATE", CommandType.Text, null).ToArray();
            
            Assert.IsTrue(records.Any());
            
            Assert.IsTrue(records.Any(x=>x.Code.Equals("NC")));
        }




        [TestMethod]
        public void TestReadDataAccessorAsync()
        {
            var dal = new SimpleDataAccess("Simple");

            var mapper = MapBuilder<State>.BuildAllProperties();

            var records = dal.ReadAsync(mapper, @"SELECT * FROM STATE", CommandType.Text, null).Result.ToList();


            Assert.IsTrue(records.Any());

            Assert.IsTrue(records.Any(x => x.Code.Equals("NC")));
        }




    }
}
