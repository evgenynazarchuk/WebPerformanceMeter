using System;
using System.Threading.Tasks;
using WebPerformanceMeter.Interfaces;
using WebPerformanceMeter.Support;
using WebPerformanceMeter.Users;

namespace WebPerformanceMeter.PerformancePlans
{
    public sealed class ActiveUsersBySteps : PerformancePlan
    {
        private readonly int _fromActiveUsersCount;

        private readonly int _toActiveUsersCount;

        private readonly int _usersStep;

        private readonly TimeSpan _stepPeriodDuration;

        private readonly Task[] _activeUsers;

        private readonly int _periodsCount;

        private readonly int _UserLoopCount;

        private readonly IEntityReader? _dataReader;

        private readonly bool _reuseDataInLoop;

        public ActiveUsersBySteps(
            User user,
            int fromActiveUsersCount,
            int toActiveUsersCount,
            int usersStep,
            TimeSpan? stepPeriodDuration = null,
            TimeSpan? performancePlanDuration = null,
            int userLoopCount = 1,
            IEntityReader? dataReader = null,
            bool reuseDataInLoop = true)
            : base(user)
        {
            UsersCountValidation(fromActiveUsersCount, toActiveUsersCount);
            UsersStepValidation(usersStep, toActiveUsersCount);
            DurationTimeValidation(stepPeriodDuration, performancePlanDuration);
            
            int maximumActiveUsersCount = Math.Max(fromActiveUsersCount, toActiveUsersCount);
            int minimumActiveUsersCount = Math.Min(fromActiveUsersCount, toActiveUsersCount);

            this._fromActiveUsersCount = fromActiveUsersCount;
            this._toActiveUsersCount = toActiveUsersCount;
            this._usersStep = usersStep;
            this._periodsCount = ((maximumActiveUsersCount - minimumActiveUsersCount) / usersStep) + 1;
            this._activeUsers = new Task[maximumActiveUsersCount];
            this._stepPeriodDuration = CalculateStepPeriodDuration(stepPeriodDuration, performancePlanDuration, this._periodsCount);

            this._UserLoopCount = userLoopCount;
            this._dataReader = dataReader;
            this._reuseDataInLoop = reuseDataInLoop;
        }

        public override async Task StartAsync()
        {
            int currentMaximumActiveUsersCountPerPeriod = this._fromActiveUsersCount;

            for (int i = 1; i <= this._periodsCount; i++)
            {
                var endTime = ScenarioTimer.Time.Elapsed.TotalSeconds + this._stepPeriodDuration.TotalSeconds;

                while (ScenarioTimer.Time.Elapsed.TotalSeconds < endTime)
                {
                    for (int currentUser = 0; currentUser < currentMaximumActiveUsersCountPerPeriod; currentUser++)
                    {
                        if (this._activeUsers[currentUser] is null || this._activeUsers[currentUser].IsCompleted)
                        {
                            this._activeUsers[currentUser] = this.User.InvokeAsync(this._UserLoopCount, this._dataReader, this._reuseDataInLoop);
                        }
                    }
                }

                if (this._fromActiveUsersCount <= this._toActiveUsersCount)
                    currentMaximumActiveUsersCountPerPeriod += this._usersStep;
                else
                    currentMaximumActiveUsersCountPerPeriod -= this._usersStep;
            }

            Task.WaitAll(this._activeUsers);

            await Task.CompletedTask;
        }

        private static void UsersStepValidation(int step, int end)
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