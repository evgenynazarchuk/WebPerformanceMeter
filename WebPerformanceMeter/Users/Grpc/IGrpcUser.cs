using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPerformanceMeter.Users.Grpc
{
    public interface IGrpcUser : IUser
    {
        void UseGrpcClient(Type grpcClient);
    }
}
