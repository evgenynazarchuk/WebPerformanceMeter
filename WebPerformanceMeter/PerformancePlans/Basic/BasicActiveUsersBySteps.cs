using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;

namespace WebPerformanceMeter.PerformancePlans
{
    public abstract class BasicActiveUsersBySteps : UsersPerformancePlan
    {
        protected readonly int fromActiveUsersCount;

        protected readonly int toActiveUsersCount;

        protected readonly int usersStep;

        protected readonly TimeSpan stepPeriodDuration;

        protected readonly Task[] activeUsers;

        protected readonly int periodsCount;

        protected readonly int userLoopCount;

        public BasicActiveUsersBySteps(
            IBaseUser user,
            int fromActiveUsersCount,
            int toActiveUsersCount,
            int usersStep,
            TimeSpan? stepPeriodDuration = null,
            TimeSpan? performancePlanDuration = null,
            int userLoopCount = 1)
            : base(user)
        {
            this.UsersCountValidation(fromActiveUsersCount, toActiveUsersCount);
            this.UsersStepValidation(usersStep, toActiveUsersCount);
            this.DurationTimeValidation(stepPeriodDuration, performancePlanDuration);

            int maximumActiveUsersCount = Math.Max(fromActiveUsersCount, toActiveUsersCount);
            int minimumActiveUsersCount = Math.Min(fromActiveUsersCount, toActiveUsersCount);

            this.fromActiveUsersCount = fromActiveUsersCount;
            this.toActiveUsersCount = toActiveUsersCount;
            this.usersStep = usersStep;
            this.periodsCount = ((maximumActiveUsersCount - minimumActiveUsersCount) / usersStep) + 1;
            this.activeUsers = new Task[maximumActiveUsersCount];
            this.stepPeriodDuration = this.CalculateStepPeriodDuration(stepPeriodDuration, performancePlanDuration, this.periodsCount);

            this.userLoopCount = userLoopCount;
        }

        public override async Task StartAsync()
        {
            int currentMaximumActiveUsersCountPerPeriod = this.fromActiveUsersCount;

            for (int i = 1; i <= this.periodsCount; i++)
            {
                var endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this.stepPeriodDuration.TotalSeconds;

                while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
                {
                    for (int currentUser = 0; currentUser < currentMaximumActiveUsersCountPerPeriod; currentUser++)
                    {
                        if (this.activeUsers[currentUser] is null || this.activeUsers[currentUser].IsCompleted)
                        {
                            this.activeUsers[currentUser] = this.InvokeUserAsync();
                        }
                    }
                }

                if (this.fromActiveUsersCount <= this.toActiveUsersCount)
                    currentMaximumActiveUsersCountPerPeriod += this.usersStep;
                else
                    currentMaximumActiveUsersCountPerPeriod -= this.usersStep;
            }

            await Task.WhenAll(this.activeUsers);
        }

        protected abstract Task InvokeUserAsync();

        private void UsersStepValidation(int step, int end)
        {
            if (step < 1)
                throw new ApplicationException("StepValueMustBeGreaterThanZero");

            if (step > end)
                throw new ApplicationException("StepValueMustBeLessOrEqualEndUserCount");
        }

        private void DurationTimeValidation(
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

        private void UsersCountValidation(int fromActiveUsersCount, int toActiveUsersCount)
        {
            if (fromActiveUsersCount < 0 || toActiveUsersCount < 0)
                throw new ApplicationException("ErrorUsersCount");
        }

        private TimeSpan CalculateStepPeriodDuration(
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