using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace AspNetMvcActiveDirectoryOwin.Services.Tests
{
    public abstract class MockableDbSetWithExtensions<T> : DbSet<T> where T : class
    {
        public abstract void AddOrUpdate(params T[] entities);
        public abstract void AddOrUpdate(Expression<Func<T, object>> identifierExpression, params T[] entities);
    }
}