using System.Web.Http;
using WebApi2Book.Web.Common.Routing;
using WebApi2Book.Web.Api.Models;
using System.Net.Http;
using WebApi2Book.Web.Common;
using WebApi2Book.Web.Api.MaintenanceProcessing;
using WebApi2Book.Common;
using WebApi2Book.Web.Api.InquiryProcessing;
using WebApi2Book.Web.Common.Validation;

namespace WebApi2Book.Web.Api.Controllers.V1
{
    [ApiVersion1RoutePrefix("tasks")]
    [UnitOfWorkActionFilter]
    [Authorize(Roles = Constants.RoleNames.JuniorWorker)]
    public class TasksController : ApiController
    {
        private readonly IAddTaskMaintenanceProcessor _addTaskMaintenanceProcessor;
        private readonly ITaskByIdInquiryProcessor _taskByIdInquiryProcessor;
        private readonly IUpdateTaskMaintenanceProcessor _updateTaskMaintenanceProcessor;
        private readonly IPagedDataRequestFactory _pagedDataRequestFactory;
        private readonly IAllTasksInquiryProcessor _allTasksInquiryProcessor;

        public TasksController(ITasksControllerDependencyBlock tasksControllerDependencyBlock)
        {
            _addTaskMaintenanceProcessor = tasksControllerDependencyBlock.AddTaskMaintenanceProcessor;
            _allTasksInquiryProcessor = tasksControllerDependencyBlock.AllTasksInquiryProcessor;
            _pagedDataRequestFactory = tasksControllerDependencyBlock.PagedDataRequestFactory;
            _taskByIdInquiryProcessor = tasksControllerDependencyBlock.TaskByIdInquiryProcessor;
            _updateTaskMaintenanceProcessor = tasksControllerDependencyBlock.UpdateTaskMaintenanceProcessor;
        }

        [Route("", Name = "AddTaskRoute")]
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = Constants.RoleNames.Manager)]
        public IHttpActionResult AddTask(HttpRequestMessage requestMessage, NewTask newTask)
        {
            var task = _addTaskMaintenanceProcessor.AddTask(newTask);
            var result = new TaskCreatedActionResult(requestMessage, task);
            return result;
        }

        [Route("{id:long}", Name = "GetTaskRoute")]
        //[Authorize(Roles = Constants.RoleNames.Manager)]
        //[Authorize(Roles = Constants.RoleNames.Guest + "," + Constants.RoleNames.SeniorWorker)]
        //[Authorize(Roles = Constants.RoleNames.Manager + "," + Constants.RoleNames.SeniorWorker)]
        //[Authorize(Users = "jdoe,jbob")]
        [Authorize(Users = "bhogg,jbob", Roles = Constants.RoleNames.Manager + "," + Constants.RoleNames.SeniorWorker)]
        public Task GetTask(long id)
        {
            var s = "";
            foreach(var c in (this.RequestContext.Principal as System.Security.Claims.ClaimsPrincipal).Claims)
            {
                s = c.Value;
            }
            var task = _taskByIdInquiryProcessor.GetTask(id);
            return task;
        }

        [Route("", Name = "GetTasksRoute")]
        public PagedDataInquiryResponse<Task> GetTasks(HttpRequestMessage requestMessage)
        {
            var request = _pagedDataRequestFactory.Create(requestMessage.RequestUri);
            var tasks = _allTasksInquiryProcessor.GetTasks(request);
            return tasks;
        }

        [Route("{id:long}", Name = "UpdateTaskRoute")]
        [HttpPut]
        [HttpPatch]
        [ValidateTaskUpdateRequest]
        [Authorize(Roles = Constants.RoleNames.SeniorWorker)]
        public Task UpdateTask(long id, [FromBody] object updatedTask)
        {
            var task = _updateTaskMaintenanceProcessor.UpdateTask(id, updatedTask);
            return task;
        }
    }
}