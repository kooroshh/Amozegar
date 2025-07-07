
using Amozegar.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace Amozegar.Background
{
    public class ExamsBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ExamsBackgroundService(IServiceScopeFactory scope)
        {
            this._scopeFactory = scope;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // مثال: دریافت رکوردهای پردازش نشده
                    var exams = await context.ExamRepository
                        .GetForBackgroundAsync(stoppingToken);

                    var states = await context.ExamStatesRepository
                        .GetStatesByStates("Completed", "Ongoing");

                    var completedState = states.Single(s => s.State == "Completed");
                    var ongoingState = states.Single(s => s.State == "Ongoing");
                    foreach (var exam in exams)
                    {

                        if (exam.ExamState.State == "Ongoing" && exam.EndDate <= now || exam.ExamState.State == "Scheduled" && exam.EndDate <= now)
                        {
                            exam.ExamState = completedState;
                        }
                        else if(exam.ExamState.State == "Scheduled" && exam.StartDate <= now)
                        {
                            exam.ExamState = ongoingState;
                        }

                        context.ExamRepository.Update(exam);
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }

                var delay = 60_000 - (now.Second * 1000 + now.Millisecond);
                await Task.Delay(delay, stoppingToken);
            }
        }

    }
}
