using System;
using System.Data;
using System.Linq;
using Sample.Net.Core.Data.UnitTest.Dto;
using SimpleNet.Data.Connection;
using SimpleNet.Data.Mapper;
using SimpleNet.Data.Repository;
using SimpleNet.Data.Repository.Contracts;

namespace SampleNet.Data.UnitTest
{
    /// <summary>
    /// Always create your base sql repository to allow for future enhancements that may be needed.
    /// </summary>
    public class BaseSqlRepository : AbstractSimpleSqlRepository
    {
        /// <summary>
        /// reason this is abstract is we can use dipendecy inject using attribute injection instead of constructor injection
        /// </summary>
        public override sealed ISimpleDataAccess Database { get; set; }

        public BaseSqlRepository()
        {
            Database = new SimpleDataAccess("<Your_App_Connection_String_Name>");

            // if for somereason you need to roll your own Sql connection provider
            // Database = new SimpleDataAccess(new SimpleSqlConnectionProvider("<Your_App_Connection_String_Name>"));
        }
    }

    public interface IStateRepository
    {
        State GetById(int id);

        State GetAll(int id);

        State Create(State state);

        State Update(State state);
    }

    public class StateRepository : BaseSqlRepository, IStateRepository
    {
        // Please review documentation from Enterprise Libary Db Accessors to 
        // Find out more about Row Mapping
        private static readonly IRowMapper<State> StateMapper = MapBuilder<State>.BuildAllProperties();

        // sample 1 
        private static readonly IRowMapper<State> StateRowMapper = MapBuilder<State>
                                                                        .MapAllProperties()
                                                                        .Build();
        
        // sample 2 for mapping possibliti
        private static readonly IRowMapper<State> StateRowMapper2 = MapBuilder<State>
                                                                        .MapNoProperties()
                                                                        .MapByName(x => x.Id)
                                                                        .Map(x => x.Code).ToColumn("Code")
                                                                        .Map(x => x.Name).WithFunc(x => x["Name"].ToString())
                                                                        .Build();




        public State GetById(int id)
        {
            const string SQL = @"SELECT Id, Code, Name FROM STATE s where s.Id = @Id";

            return Read<State>(StateMapper, SQL, CommandType.Text, new[]
            {
                GetDbParameter("@Id", id)
            }).FirstOrDefault();
        }
        
        public State GetAll(int id)
        {
            const string SQL = @"SELECT Id, Code, Name FROM STATE s  ORDER BY s.Name ";

            return Read<State>(StateMapper, SQL, CommandType.Text, null).FirstOrDefault();
        }

        public DataTable ReadById(int id)
        {
            const string SQL = @"SELECT Id, Code, Name FROM STATE s where s.Id = @Id";

            return Read(SQL, CommandType.Text, new[]
            {
                GetDbParameter("@Id", id)
            });
        }


        public State Create(State state)
        {
            const string SQL = @"
                                INSERT INTO STATE (Code, Name) 
                                VALUES (@Code, @Name); 
                                SELECT SCOPE_IDENTITY()        
                                ";

            var id = ExecuteScalar(SQL, CommandType.Text, new[]
            {
                GetDbParameter("@Code", state.Code),
                GetDbParameter("@Name", state.Name)
            });

            state.Id = Convert.ToInt32(id);

            return state;
        }


        public State Update(State state)
        {
            const string SQL = @" UPDATE STATE
                                  SET Code = @Code,
                                    Name = @Name
                                  WHERE Id = @Id        
                                ";

            var id = ExecuteNonQuery(SQL, CommandType.Text, new[]
            {
                GetDbParameter("@Code", state.Code),
                GetDbParameter("@Name", state.Name),
                GetDbParameter("@Id", state.Id)
            });
            

            return state;
        }

    }



}
