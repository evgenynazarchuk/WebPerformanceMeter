namespace WebPerformanceMeter.PerformancePlans
{
    using System;
    using System.Threading.Tasks;
    using WebPerformanceMeter.Interfaces;
    using WebPerformanceMeter.Support;
    using WebPerformanceMeter.Users;

    public sealed class ActiveUsersBySteps : PerformancePlan
    {
        private readonly int FromActiveUsersCount;

        private readonly int ToActiveUsersCount;

        private readonly int Step;

        private readonly TimeSpan StepPeriodDuration;

        private readonly Task[] ActiveUsers;

        private readonly int PeriodsCount;

        private readonly int UserLoopCount;

        private readonly IEntityReader? DataReader;

        private readonly bool ReuseDataInLoop;

        public ActiveUsersBySteps(
            User user,
            int fromActiveUsersCount,
            int toActiveUsersCount,
            int step,
            TimeSpan? stepPeriodDuration = null,
            TimeSpan? performancePlanDuration = null,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
            : base(user)
        {
            UsersCountValidation(fromActiveUsersCount, toActiveUsersCount);
            DurationTimeValidation(stepPeriodDuration, performancePlanDuration);
            StepValidation(step, toActiveUsersCount);

            int maximumActiveUsersCount = Math.Max(fromActiveUsersCount, toActiveUsersCount);
            int minimumActiveUsersCount = Math.Min(fromActiveUsersCount, toActiveUsersCount);

            this.FromActiveUsersCount = fromActiveUsersCount;
            this.ToActiveUsersCount = toActiveUsersCount;
            this.Step = step;
            this.PeriodsCount = ((maximumActiveUsersCount - minimumActiveUsersCount) / step) + 1;
            this.ActiveUsers = new Task[maximumActiveUsersCount];
            this.StepPeriodDuration = CalculateStepPeriodDuration(stepPeriodDuration, performancePlanDuration, this.PeriodsCount);

            this.UserLoopCount = userLoopCount;
            this.DataReader = dataReader;
            this.ReuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            int currentMaximumActiveUsersCountPerPeriod = this.FromActiveUsersCount;

            for (int i = 1; i <= this.PeriodsCount; i++)
            {
                var endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this.StepPeriodDuration.TotalSeconds;

                while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
                {
                    for (int currentUser = 0; currentUser < currentMaximumActiveUsersCountPerPeriod; currentUser++)
                    {
                        if (this.ActiveUsers[currentUser] is null || this.ActiveUsers[currentUser].IsCompleted)
                        {
                            this.ActiveUsers[currentUser] = this.User.InvokeAsync(this.UserLoopCount, this.DataReader, this.ReuseDataInLoop);
                        }
                    }
                }

                if (this.FromActiveUsersCount <= this.ToActiveUsersCount)
                    currentMaximumActiveUsersCountPerPeriod += this.Step;
                else
                    currentMaximumActiveUsersCountPerPeriod -= this.Step;
            }

            Task.WaitAll(this.ActiveUsers);

            await Task.CompletedTask;
        }

        private static void StepValidation(int step, int end)
        {
            if (step < 1)
                throw new ApplicationException("StepValueMustBeGreaterThanZero");

            if (step > end)
                throw new ApplicationException("StepValueMustBeLessOrEqualEndUserCount");
        }

        private static void DurationTimeValidation(
            TimeSpan? stepPeriodDuration = null,
            TimeSpan? performancePlanDuration = null)
        {
            if (stepPeriodDuration is not null && performancePlanDuration is not null)
            {
                throw new ApplicationException("InitManyDuration");
            }

            if (stepPeriodDuration is null && performancePlanDuration is null)
            {
                throw new ApplicationException("DurationIsNotSet");
            }
        }

        private static void UsersCountValidation(int fromActiveUsersCount, int toActiveUsersCount)
        {
            if (fromActiveUsersCount < 0 || toActiveUsersCount < 0)
                throw new ApplicationException("ErrorUsersCount");
        }

        private static TimeSpan CalculateStepPeriodDuration(
            TimeSpan? stepPeriodDuration,
            TimeSpan? performancePlanDuration,
            int periodsCount)
        {
            if (stepPeriodDuration is not null)
            {
                return stepPeriodDuration.Value;
            }
            else if (stepPeriodDuration is null && performancePlanDuration is not null)
            {
                return performancePlanDuration.Value / periodsCount;
            }
            else throw new ApplicationException("DurationIsNotSet");
        }
    }
}