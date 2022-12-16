namespace RunAsManager
{
    public static class MainClass
    {
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static void Main(string[] args)
        {
            if (args.Length == 1)
                return;
            else if (args.Length == 2)
            {
                new RunService().RunWithParameters(args);
                return;
            }

            var app = new RunAsManager.App();

            app.InitializeComponent();

            app.Run();
        }
    }
}
