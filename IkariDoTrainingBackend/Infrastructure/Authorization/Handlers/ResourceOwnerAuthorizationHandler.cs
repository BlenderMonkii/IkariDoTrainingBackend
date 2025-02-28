using IkariDoTrainingBackend.Data;
using IkariDoTrainingBackend.Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IkariDoTrainingBackend.Infrastructure.Authorization.Handlers
{
    public class ResourceOwnerAuthorizationHandler : AuthorizationHandler<ResourceOwnerRequirement>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResourceOwnerAuthorizationHandler(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ResourceOwnerRequirement requirement)
        {
            // 1) UserId aus den Claims lesen
            var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out var userId))
            {
                return;
            }

            // 2) Route-Daten auslesen (z.B. "sessionId", "id", "planId", etc.)
            var routeData = _httpContextAccessor.HttpContext?.GetRouteData();
            if (routeData == null)
                return;

            // Beispiel: Wir gehen davon aus, dass dein Routen-Parameter 
            // bei SessionController "id" heißt, bei PlanController "planId", etc.
            // => Du könntest hier verschiedene Parameter-Namen prüfen:
            if (routeData.Values.TryGetValue("id", out var idValue) && int.TryParse(idValue?.ToString(), out int sessionId))
            {
                // Prüfen, ob der Session-Besitzer = userId
                var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == sessionId);

                // Session existiert nicht => kein Erfolg
                if (session == null) return;

                // Eigentum prüfen
                if (session.OwnerId == userId)
                {
                    // Erfüllt => Markiere Requirement als bestanden
                    context.Succeed(requirement);
                    return;
                }
            }

            // Du könntest hier erweitern:
            // - Plans (planId)
            // - Exercises (exerciseId)
            // => Ähnliche Logik wie bei Session

            // Falls wir bis hierhin kommen, ist das Requirement nicht erfüllt
        }
    }
}
