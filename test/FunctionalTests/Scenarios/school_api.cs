using AutoFixture;
using FluentAssertions;
using FunctionalTests.Seedwork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace FunctionalTests.Scenarios
{
    public class school_api : ApiServerTest
    {
        private const string InvalidSub = "0";
        private readonly IEnumerable<TestServer> servers;

        public school_api(TestServerFixture fixture) : base(fixture)
        {
            this.servers = fixture.Servers
                .Where(x => !x.SupportSchemes)
                .Select(x => x.TestServer);
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authenticated()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authorized()
        {
            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Sub(InvalidSub))
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        public async Task allow_to_view_grades_if_the_user_have_permission()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.Teacher);
            await Fixture.GivenARole(Roles.Teacher, Subs.Teacher);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Sub(Subs.Teacher))
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        public async Task not_allow_to_view_grades_if_the_user_does_not_have_permissions()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.SubstituteTwo);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.GetGrades)
                    .WithIdentity(new Fixture().Sub(Subs.SubstituteTwo))
                    .GetAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }

        [Fact]
        public async Task allow_to_edit_grades_if_the_user_does_have_permission()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.Teacher);
            await Fixture.GivenARole(Roles.Teacher, Subs.Teacher);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Sub(Subs.Teacher))
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        public async Task allow_to_edit_grades_if_the_client_does_have_permission()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.Teacher);
            await Fixture.GivenARole(Roles.Teacher, Subs.Teacher);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Client(Subs.Teacher))
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        public async Task allow_to_edit_grades_if_the_upn_user_does_have_permission()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.Teacher);
            await Fixture.GivenARole(Roles.Teacher, Subs.Teacher);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().UpnSub(Subs.Teacher))
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }


        [Fact]
        public async Task allow_to_edit_grades_if_someone_has_delegated_his_permissions()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.Teacher);
            // await Fixture.GivenAnSubject(Subs.SubstituteOne);
            await Fixture.GivenARole(Roles.Teacher, Subs.Teacher);
            await Fixture.GivenAnUserWithADelegation(Subs.Teacher, Subs.SubstituteOne);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Sub(Subs.SubstituteOne))
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status200OK);
            }
        }

        [Fact]
        public async Task not_allow_to_edit_grades_if_someone_has_delegated_his_permissions_but_no_delegations_has_been_selected()
        {
            await Fixture.GivenAnApplication();
            // await Fixture.GivenAnSubject(Subs.Teacher);
            // await Fixture.GivenAnSubject(Subs.SubstituteTwo);
            await Fixture.GivenARole(Roles.Teacher, Subs.Teacher);
            await Fixture.GivenAnUserWithADelegation(Subs.Teacher, Subs.SubstituteTwo, false);

            foreach (var server in servers)
            {
                var response = await server
                    .CreateRequest(Api.School.EditGrades)
                    .WithIdentity(new Fixture().Sub(Subs.SubstituteTwo))
                    .PutAsync();

                response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            }
        }
    }
}
