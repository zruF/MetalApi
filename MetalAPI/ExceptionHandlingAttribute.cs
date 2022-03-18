using Shared.Exceptions;
using System.Net;
using System.Web.Http.Filters;

namespace MetalAPI
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            HttpStatusCode status;
            if(context.Exception.GetType() == typeof(NotFoundException))
            {
                status = HttpStatusCode.NotFound;
            }
            else if(context.Exception.GetType() == typeof(BadArgumentException))
            {
                status = HttpStatusCode.BadRequest;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
            }

            context.Response = context.Request.CreateResponse(status);
        }
    }
}
