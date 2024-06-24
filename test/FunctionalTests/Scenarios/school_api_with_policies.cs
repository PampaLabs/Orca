using AutoFixture;
using FluentAssertions;
using FunctionalTests.Seedwork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace FunctionalTests.Scenarios
{
    public class school_api_with_policies : ApiServerTest
    {
        private const string InvalidSub = "0";
        private readonly IEnumerable<TestServer> servers;

        public school_api_with_policies(TestServerFixture fixture) : base(fixture)
        {
            this.servers = fixture.Servers
                .Where(x => !x.SupportSchemes)
                .Select(x => x.TestServer);
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_policie_is_not_satisfied()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetAbacPolicy)
                    .WithIdentity(new Fixture().Sub(InvalidSub))
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        public async Task allow_to_view_grades_if_the_policie_is_satisfied()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.Teacher);
            await Fixture.GivenARole(Roles.Teacher, Subs.Teacher);

            await Fixture.GivenAPolicy("abac-policy", AbacPolicies.Substitute);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetAbacPolicy)
                    .WithIdentity(new Fixture().Sub(Subs.Teacher))
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }
    }
}
