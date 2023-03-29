using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace VoterDetailsAPI.Model
{
    public abstract class BaseResponse
    {
        public int StatusCode { get; set; } = StatusCodes.Status200OK;
        public IEnumerable<string> ErrorList { get; set; }
    }

    public class ListResponse<T> : BaseResponse where T : class
    {
        public ListResponse(IEnumerable<T> list)
        {
            List = list;
        }
        public IEnumerable<T> List { get; }
    }
}
