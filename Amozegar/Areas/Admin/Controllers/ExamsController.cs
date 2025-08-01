using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amozegar.Areas.Admin.Controllers
{
    [Route("Panel/Admin/Exams")]
    public class ExamsController : BaseController
    {
        private IUnitOfWork _context;

        public ExamsController(IUnitOfWork context)
        {
            this._context = context;
        }

        // Utilities

        private IActionResult RedirectToExams() => RedirectToAction("Index", "Exams", new { pageNumber = 1 });
        private IActionResult RedirectToExams(string error)
        {
            TempData["Error"] = error;
            return RedirectToAction("Index", "Exams", new { pageNumber = 1 });
        }

        private IActionResult RedirectToShowExam(int examId, int questionPageNumber, int studentResultPageNumber) => RedirectToAction("ShowExam", "Exams", new { area = "Admin", examId = examId, questionPageNumber = questionPageNumber, studentResultPageNumber = studentResultPageNumber});

        private async Task<IActionResult> doPostActions(int examId, string to, params string[] shouldBe)
        {
            var exam = await this._context.ExamRepository
                .GetExamByIdByThisStatesAsync(examId, shouldBe);

            if (exam == null)
            {
                return this.RedirectToExams("چنین امتحانی وجود ندارد");
            }

            await this._context.ExamRepository.ChangeExamStateAsync(exam, to);
            await this._context.SaveChangesAsync();

            return this.RedirectToExams();
        }


        // Main Methods


        [Route("{pageNumber}")]
        public async Task<IActionResult> Index(int pageNumber)
        {
            ViewBag.Route = "Exams";


            var exams = await this._context.ExamRepository
                .GetExamsByPagNumberAsync(pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, exams.Count()))
            {
                return this.RedirectToExams();
            }

            var examsCount = await this._context.ExamRepository
                .GetNotDeletedExamsCountAsync();

            this.checkNextOrPrevForViewBags(examsCount, pageNumber);


            return View(exams);
        }


        [Route("ShowExam/{examId}")]
        public async Task<IActionResult> ShowExam(int examId, int questionPageNumber, int studentResultPageNumber)
        {
            ViewBag.Route = "Exams";

            var exam = await this._context.ExamRepository
                .GetExamByIdForAdminByPageNumbersAsync(examId, studentResultPageNumber, questionPageNumber);

            if (exam == null)
            {
                return this.RedirectToExams("چنین امتحانی وجود ندارد");
            }

            this.setPaginationViewDatas(questionPageNumber, "Questions");
            this.setPaginationViewDatas(studentResultPageNumber, "Students");

            if (this.validateUserPageNumber(questionPageNumber, exam.Questions.Count()))
            {
                return this.RedirectToShowExam(examId, 1, studentResultPageNumber);
            }

            if (this.validateUserPageNumber(studentResultPageNumber, exam.Students.Count()))
            {
                return this.RedirectToShowExam(examId, questionPageNumber, 1);
            }


            this.checkNextOrPrevForViewDatas(exam.QuestionCount, questionPageNumber, "Questions");
            this.checkNextOrPrevForViewDatas(exam.JoinerCount, studentResultPageNumber, "Students");

            return View(exam);
        }

        [Route("ShowQuestion/{questionId}/{pageNumber}")]
        public async Task<IActionResult> ShowQuestion(int questionId, int pageNumber)
        {
            ViewBag.Route = "Exams";

            var question = await this._context.QuestionsRepository
                .GetQuestionByIdByPageNumberForAdminAsync(questionId, pageNumber);

            if (question == null)
            {
                return this.RedirectToExams("چنین سوالی وجود ندارد");
            }

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, question.Options.Count()))
            {
                return RedirectToAction("ShowQuestion", "Exams", new { area = "Admin", questionId = questionId, pageNumber = 1 });
            }

            this.checkNextOrPrevForViewBags(question.OptionsCount, pageNumber);

            return View(question);
        }

        [HttpPost("DeleteExam/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExam(int examId)
        {
            return await this.doPostActions(examId, "Deleted", "Draft", "Scheduled", "Closed");
        }

        [HttpPost("CloseExam/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseExam(int examId)
        {
            return await this.doPostActions(examId, "Closed", "Ongoing");
        }

        [HttpPost("ScheduledExam/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduledExam(int examId)
        {
            return await this.doPostActions(examId, "Scheduled", "Draft");
        }

        [HttpPost("DraftExam/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DraftExam(int examId)
        {
            return await this.doPostActions(examId, "Draft", "Scheduled");
        }



    }
}
