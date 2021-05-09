using System;
using System.Security.Claims;
using System.Threading.Tasks;
using back.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace back.Authorization
{
    public class MustBeQuestionAuthorHandler:AuthorizationHandler<MustBeQuestionAuthorRequirement> {
        private readonly IDataRepository _dataRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MustBeQuestionAuthorHandler(IDataRepository dataRepository, IHttpContextAccessor httpContextAccessor)
        {
            _dataRepository = dataRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeQuestionAuthorRequirement requirement)
        {
            // Check That user is authenticated
            if (!context.User.Identity.IsAuthenticated) {
                context.Fail();
            }
            // Get question Id from Route/HttpQuestion
            var questionId=_httpContextAccessor.HttpContext.Request.RouteValues["questionId"];
            int Id=Convert.ToInt32(questionId);

            //Get User name
            var userId=context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //Get Question
            var question=_dataRepository.GetQuestion(Id);

            // Always it is better to pass the Authorization chain to next Auth and Not
            // Fail it. But some times for security of info you should fail user
            if (question==null) {
                return Task.FromResult(0);
            }
            if (question.UserId==userId) {
                context.Succeed(requirement);
            }
            return Task.FromResult(0);
        }
    }
}