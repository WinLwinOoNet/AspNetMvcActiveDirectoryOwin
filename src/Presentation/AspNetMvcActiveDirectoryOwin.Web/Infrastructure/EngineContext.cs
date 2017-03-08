using System.Runtime.CompilerServices;

namespace AspNetMvcActiveDirectoryOwin.Web.Infrastructure
{
    public class EngineContext
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize()
        {
            if (Singleton<IEngine>.Instance == null)
            {
                Singleton<IEngine>.Instance = new AppEngine();
                Singleton<IEngine>.Instance.Initialize();
            }
            return Singleton<IEngine>.Instance;
        }

        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize();
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}