using AutoFixture;
using Balea;
using FluentAssertions;
using FunctionalTests.Seedwork;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace FunctionalTests.Scenarios
{
    public class school_api_with_schemes : ApiServerTest
    {
        private const string InvalidSub = "0";
        private const string DefaultScheme = "scheme1";
        private const string BaleaScheme = "scheme2";
        private const string NotBaleaScheme = "scheme3";

        public school_api_with_schemes(TestServerFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_not_authenticated(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_non_balea_schema_and_not_authorized(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(InvalidSub), DefaultScheme)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_balea_schema_and_not_authorized(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(InvalidSub), BaleaScheme)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_non_balea_schema_and_belongs_to_the_teacher_role(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(InvalidSub), DefaultScheme)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task allow_to_view_grades_if_the_user_is_authenticated_with_balea_schema_and_belongs_to_the_teacher_role(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            using var scope = server.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAccessControlContext>();

            await context.GivenAnApplication();
            // await context.GivenAnSubject(Subs.Teacher);
            await context.GivenARole(Roles.Teacher, Subs.Teacher);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(Subs.Teacher), BaleaScheme)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();

            var schemes = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());

            schemes.Should().HaveCount(2);
            schemes.Should().Contain(BaleaScheme);
            schemes.Should().Contain("Balea");
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_non_balea_schema_and_not_belongs_to_the_teacher_role(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(InvalidSub), DefaultScheme)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task not_allow_to_view_grades_if_the_user_is_authenticated_with_balea_schema_and_not_belongs_to_the_teacher_role(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetGrades)
                .WithIdentity(new Fixture().Sub(InvalidSub), BaleaScheme)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task call_to_endpoint_authorized_with_default_non_balea_scheme_should_not_include_balea(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetSchemes)
                .WithIdentity(new Fixture().Sub(InvalidSub), DefaultScheme)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();

            var schemes = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());

            schemes.Should().HaveCount(1);
            schemes.Should().Contain(DefaultScheme);
            schemes.Should().NotContain("Balea");
        }

        [Theory]
        [MemberData(nameof(TestServerData.GetTestServersWithSchemaSupport), MemberType = typeof(TestServerData))]
        public async Task call_to_endpoint_authorized_with_not_balea_configured_scheme_should_not_include_balea(Type serverType)
        {
            var server = Fixture.GetTestServer(serverType);

            var response = await server
                .CreateRequest(Api.School.GetCustomPolicy)
                .WithIdentity(new Fixture().Sub(InvalidSub), NotBaleaScheme)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();

            var schemes = JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());

            schemes.Should().HaveCount(1);
            schemes.Should().Contain(NotBaleaScheme);
            schemes.Should().NotContain("Balea");
        }
    }
}
