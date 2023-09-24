using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture
{
    public class ArchitectureTests
    {
        private const string DomainNamespace = "NewsManagement.Domain";
        private const string ApplicationNamespace = "NewsManagement.Application";
        private const string PersistenceNamespace = "NewsManagement.Persistence";
        private const string ApiNamespace = "NewsManagement.Api";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            var assembly = typeof(NewsManagement.Domain.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                ApplicationNamespace,
                PersistenceNamespace,
                ApiNamespace
            };

            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue();
        }
        
        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            var assembly = typeof(NewsManagement.Application.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                PersistenceNamespace,
                ApiNamespace
            };

            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
        {
            var assembly = typeof(NewsManagement.Persistence.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                ApiNamespace
            };

            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Controllers_Should_HaveDependencyOnMediatR()
        {
            var assembly = typeof(NewsManagement.Api.AssemblyReference).Assembly;

            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue();
        }
    }
}