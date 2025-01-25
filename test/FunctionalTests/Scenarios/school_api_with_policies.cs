using AutoFixture;
using Orca;
using FluentAssertions;
using FunctionalTests.Seedwork;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace FunctionalTests.Scenarios
{
    public class school_api_with_policies : ApiServerTest
    {
        private const string InvalidSub = "0";

        public school_api_with_policies(TestServerFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_policie_is_not_satisfied(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetAbacPolicy)
                .WithIdentity(new Fixture().Sub(InvalidSub))
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task allow_to_view_grades_if_the_policie_is_satisfied(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAccessControlContext>();

            await context.GivenAnApplication();
            await context.GivenAnSubject(Subs.Teacher);
            await context.GivenARole(Roles.Teacher, Subs.Teacher);

            await context.GivenAPolicy("abac-policy", AbacPolicies.Substitute);

            var response = await server
                .CreateRequest(Api.School.GetAbacPolicy)
                .WithIdentity(new Fixture().Sub(Subs.Teacher))
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
