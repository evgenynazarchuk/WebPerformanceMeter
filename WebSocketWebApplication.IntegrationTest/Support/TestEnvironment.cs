namespace WebSocketWebApplication.IntegrationTest.Support
{
    internal class TestEnvironment
    {
        public readonly TestApplication App;

        public TestEnvironment()
        {
            this.App = new TestApplication();
        }
    }
}
