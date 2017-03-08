using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AspNetMvcActiveDirectoryOwin.Core;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public abstract class PersistenceTest
    {
        protected AppDbContext context;

        [SetUp]
        public void SetUp()
        {
            //TODO fix compilation warning (below)
            #pragma warning disable 0618
            Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
            context = new AppDbContext(GetTestDbName());
            context.Database.Delete();
            context.Database.Create();
        }

        protected string GetTestDbName()
        {
            string testDbName = "Data Source=" + 
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                @"\\AspNetMvcActiveDirectoryOwin.Data.Tests.Db.sdf;Persist Security Info=False";
            return testDbName;
        }
        
        protected T SaveAndLoadEntity<T>(T entity, bool disposeContext = true) where T : BaseEntity
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();

            object id = entity.EntityId;

            if (disposeContext)
            {
                context.Dispose();
                context = new AppDbContext(GetTestDbName());
            }

            try
            {
                return context.Set<T>().Find(id);
            }
            catch
            {
                // If a record cannot be find by id, it must be composite primary key. 
                // So we just return the very first record. 
                return context.Set<T>().FirstOrDefault();
            }
        }
    }
}