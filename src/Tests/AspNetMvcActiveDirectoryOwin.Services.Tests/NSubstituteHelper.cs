using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NSubstitute;

namespace AspNetMvcActiveDirectoryOwin.Services.Tests
{
    public static class NSubstituteHelper
    {
        public static IDbSet<T> CreateMockDbSet<T>(IQueryable<T> data = null) where T : class 
        {
            var mockSet = Substitute.For<MockableDbSetWithExtensions<T>, IQueryable<T>, IDbAsyncEnumerable<T>>();
            mockSet.AsNoTracking().Returns(mockSet);
            if (data != null)
            {
                ((IDbAsyncEnumerable<T>) mockSet).GetAsyncEnumerator().Returns(new TestDbAsyncEnumerator<T>(data.GetEnumerator()));
                ((IQueryable<T>) mockSet).Provider.Returns(new TestDbAsyncQueryProvider<T>(data.Provider));
                ((IQueryable<T>) mockSet).Expression.Returns(data.Expression);
                ((IQueryable<T>) mockSet).ElementType.Returns(data.ElementType);
                ((IQueryable<T>)mockSet).GetEnumerator().Returns(new TestDbEnumerator<T>(data.GetEnumerator()));
            }
            return mockSet;
        }
    }
}