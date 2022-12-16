using RunAsManager.DbModel;
using System.Linq;

namespace RunAsManager
{
    internal class RunService
    {
        public void Run(ProgramDetail programDetail)
        {
            var db = new DbContext();

            var programDetailFound = db.ProgramsDetails
                .Where(p => p.Path == programDetail.Path && p.Arguments == programDetail.Arguments)
                .FirstOrDefault();

            var sc = new PasswordService().GetPassword(db.AdministratorUserName);

            if (sc == null)
                return;

            new RunAs().Start(db.AdministratorUserName, sc, programDetailFound.Path, programDetailFound.Arguments);
        }

        public void RunWithParameters(string[] args)
        {
            if (args.Length != 2)
                return;

            var db = new DbContext();

            var programDetail = db.ProgramsDetails
                            .Where(p => p.Path == args[0] && p.Arguments == args[1])
                            .FirstOrDefault();

            if (programDetail == null)
                return;

            var sc = new PasswordService().GetPassword(db.AdministratorUserName);

            if (sc == null)
                return;

            new RunAs().Start(db.AdministratorUserName, sc, programDetail.Path, programDetail.Arguments);
        }
    }
}