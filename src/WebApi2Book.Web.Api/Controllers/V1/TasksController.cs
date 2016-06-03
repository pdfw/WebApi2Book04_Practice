using System.Web.Http;
using WebApi2Book.Web.Common.Routing;
using WebApi2Book.Web.Api.Models;
using System.Net.Http;

namespace WebApi2Book.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("tasks")]
    public class TasksController : ApiController
    {
        [Route("", Name = "AddTaskRoute")]
        [HttpPost]
        public Task AddTask(HttpRequestMessage requestMessage, Task newTask)
        {
            return new Task
            {
                Subject = "In v1, newTask.Subject = " + newTask.Subject
            };
        }
    }
}