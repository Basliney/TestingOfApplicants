using System.Collections.Generic;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Models.Statistics
{
    public class AllStatistics
    {
        public List<User> users { get; private set; }
        public List<CompletedTestDto> completedTests { get; private set; }
        public List<TestHeader> headers { get; private set; }
        public AllStatistics(List<User> users, List<CompletedTestDto> completedTests, 
            List<TestHeader> headers)
        {
            this.users = users;
            this.completedTests = completedTests;
            this.headers = headers;
        }
    }
}
