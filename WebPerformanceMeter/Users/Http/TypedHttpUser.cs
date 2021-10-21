using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using WebPerformanceMeter.Interfaces;

namespace WebPerformanceMeter
{
    public abstract class TypedHttpUser<TEntity> : BaseHttpUser, ITypedHttpUser<TEntity>
        where TEntity : class
    {
        public TypedHttpUser(HttpClient client, string userName = "")
            : base(client, userName) { }

        public TypedHttpUser(
            string address,
            IDictionary<string, string>? defaultHeaders = null,
            IEnumerable<Cookie>? defaultCookies = null,
            string userName = "")
            : base(address, defaultHeaders, defaultCookies, userName) { }

        public virtual async Task InvokeAsync(
            int userloopCount = 1,
            IDataReader? dataReader = null,
            bool reuseDataInLoop = true
            )
        {
            TEntity? entity = null;

            if (dataReader is not null)
            {
                entity = (TEntity?)dataReader.GetEntity();
            }

            for (int i = 0; i < userloopCount; i++)
            {
                if (entity is null)
                {
                    return;
                }

                await PerformanceAsync(entity);

                if (dataReader is not null && !reuseDataInLoop)
                {
                    entity = (TEntity?)dataReader.GetEntity();
                }
            }
        }

        public abstract Task PerformanceAsync(TEntity data);
    }
}
