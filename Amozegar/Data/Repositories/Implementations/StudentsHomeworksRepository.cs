using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class StudentsHomeworksRepository : GenericRepository<StudentHomework>, IStudentsHomeworksRepository
    {
        public StudentsHomeworksRepository(AmozegarContext context) : base(context)
        {
        }

        // Utilities

        private async Task<Homework> getHomeworkByIdAsync(int homeworkId)
        {
            var homework = await this._context.Homeworks
                .SingleAsync(h => h.HomeworkId == homeworkId);

            return homework;
        }


        private async Task<StudentHomeworkState> getStudentHomeworkStateByStateAsync(string state)
        {
            var studentHomeworkstate = await this._context.StudentsHomeworskStates
                .SingleAsync(shs => shs.State == state);
            return studentHomeworkstate;
        }


        // Main Methods
        public async Task AddStudentToHomeworkByHomeworkIdAsync(int homeworkId, int classStudentId)
        {
            var homework = await this.getHomeworkByIdAsync(homeworkId);

            var defaultState = await this.getStudentHomeworkStateByStateAsync("Pending");

            var studentHomework = new StudentHomework()
            {
                Homework = homework,
                HomeworkId = homework.HomeworkId,
                StudentHomeworkState = defaultState,
                StudentHomeworkStateId = defaultState.StudentHomeworkStateId,
                ClassStudentId = classStudentId
            };

            await this._context.StudentsHomeworks
                .AddAsync(studentHomework);

        }
    }
}
