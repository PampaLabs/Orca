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
    public class school_api : ApiServerTest
    {
        private const string InvalidSub = "0";

        public school_api(TestServerFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authenticated(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authorized(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(InvalidSub))
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task allow_to_view_grades_if_the_user_have_permission(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var stores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await stores.GivenAnApplication();
            await stores.GivenAnSubject(Subs.Teacher);
            await stores.GivenARole(Roles.Teacher, Subs.Teacher);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(Subs.Teacher))
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_does_not_have_permissions(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var stores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await stores.GivenAnApplication();
            await stores.GivenAnSubject(Subs.SubstituteTwo);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(Subs.SubstituteTwo))
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task allow_to_edit_grades_if_the_user_does_have_permission(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var stores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await stores.GivenAnApplication();
            await stores.GivenAnSubject(Subs.Teacher);
            await stores.GivenARole(Roles.Teacher, Subs.Teacher);

            var response = await server
                .CreateRequest(Api.School.EditGrades)
                .WithIdentity(new Fixture().Sub(Subs.Teacher))
                .PutAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task allow_to_edit_grades_if_the_client_does_have_permission(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await context.GivenAnApplication();
            await context.GivenAnSubject(Subs.Teacher);
            await context.GivenARole(Roles.Teacher, Subs.Teacher);

            var response = await server
                .CreateRequest(Api.School.EditGrades)
                .WithIdentity(new Fixture().Client(Subs.Teacher))
                .PutAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task allow_to_edit_grades_if_the_upn_user_does_have_permission(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var stores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await stores.GivenAnApplication();
            await stores.GivenAnSubject(Subs.Teacher);
            await stores.GivenARole(Roles.Teacher, Subs.Teacher);

            var response = await server
                .CreateRequest(Api.School.EditGrades)
                .WithIdentity(new Fixture().UpnSub(Subs.Teacher))
                .PutAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task allow_to_edit_grades_if_someone_has_delegated_his_permissions(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var stores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await stores.GivenAnApplication();
            await stores.GivenAnSubject(Subs.Teacher);
            await stores.GivenAnSubject(Subs.SubstituteOne);
            await stores.GivenARole(Roles.Teacher, Subs.Teacher);

            await stores.GivenAnUserWithADelegation(Subs.Teacher, Subs.SubstituteOne);

            var response = await server
                .CreateRequest(Api.School.EditGrades)
                .WithIdentity(new   Fixture().Sub(Subs.SubstituteOne))
                .PutAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithoutSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_edit_grades_if_someone_has_delegated_his_permissions_but_no_delegations_has_been_selected(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var stores = scope.ServiceProvider.GetRequiredService<IOrcaStoreAccessor>();

            await stores.GivenAnApplication();
            await stores.GivenAnSubject(Subs.Teacher);
            await stores.GivenAnSubject(Subs.SubstituteTwo);
            await stores.GivenARole(Roles.Teacher, Subs.Teacher);
            await stores.GivenAnUserWithADelegation(Subs.Teacher, Subs.SubstituteTwo, false);

            var response = await server
                .CreateRequest(Api.School.EditGrades)
                .WithIdentity(new Fixture().Sub(Subs.SubstituteTwo))
                .PutAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
