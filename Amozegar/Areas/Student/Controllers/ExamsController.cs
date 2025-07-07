using Amozegar.Areas.Student.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amozegar.Areas.Student.Controllers
{
    [Route("Panel/Student/{classId}/Exams")]
    public class ExamsController : BaseController
    {
        private IUnitOfWork _context;

        public ExamsController(IUnitOfWork context)
        {
            _context = context;
        }

        // Utilities

        private IActionResult returnToOnGoingExams()
        {
            return RedirectToAction("Index", "Exams", new { area = "Shared", roleName = "Student", classId = this.classId, pageNumber = 1, type = "Ongoing" });
        }

        private IActionResult returnToDisplayExam(int examId, string type)
        {
            return RedirectToAction("DisplayExam", "Exams", new { area = "Student", classId = this.classId, examId = examId, type = type });
        }

        private IActionResult returnToDoExam(int examId, int questionIndex = 1)
        {
            return RedirectToAction("DoExam", "Exams", new { area = "Student", classId = classId, examId = examId, questionIndex = questionIndex, pageNumber = 1 });
        }




        private async Task<IEnumerable<ExamState>> getValidExamStatesForOngoingDisplayAsync()
        {
            var states = await this._context.ExamStatesRepository
                .GetStatesByStates("Ongoing", "Scheduled", "Closed");
            return states;

        }

        private bool IsNotValidQuestionIndex(int questionIndex, int questionCount, int lastCompletedQuestion)
        {
            return (
                questionIndex < 1 ||
                questionIndex - 1 > lastCompletedQuestion ||
                questionIndex > questionCount
               );
        }

        private void setDoExamViewBags(int examId, int questionIndex, int questionCount)
        {
            ViewBag.ExamId = examId;
            ViewBag.IsLast = questionIndex == questionCount;
        }

        // Main Methods

        [Route("DisplayExam/{examId}/{type}")]
        public async Task<IActionResult> DisplayExam(string classId, int examId, string type)
        {
            switch (type)
            {
                case "Scheduled":
                case "Closed":
                case "Ongoing":
                    {
                        ViewBag.Route = "Exams";
                        break;
                    }

                case "Completed":
                    {
                        ViewBag.Route = "CompletedExams";
                        break;
                    }

                default:
                    {
                        return NotFound();
                    }
            }

            var exam = await this._context.ExamRepository
                .GetExamByClassIdByExamIdForDetailsAsync(classId, examId);

            var classStudentId = await this._context.ClassStudentsRepository
                .GetClassStudentIdByUserNameByClassIdentity(classId, User.Identity.Name);

            var isFinishExam = await this._context.ClassStudentsToExamRepository
                .GetIsFinishedByExamIdByStudentIdAsync(examId, classStudentId);

            ViewBag.IsFinish = isFinishExam != null;

            if (exam == null || exam.State.State == "Draft")
            {
                return this.returnToOnGoingExams();
            }
            var examEndDate = await this._context.ExamRepository
                .GetExamEndDateByClassIdentityByExamIdAsync(classId, examId);

            if (examEndDate < DateTime.Now || isFinishExam != null)
            {

                var examResult = new ExamResultViewModel()
                {
                    AwnserCount = 0,
                    JoinAt = "شرکت نکرده",
                    TrueAwnserCount = 0,
                    Score = 0
                };
                if(isFinishExam != null)
                {
                    var trueAwnserCount = await this._context.ClassStudentToExamToQuestionRepository
                        .TrueAwnserCountsByClassStudentToExamIdCountAsync(isFinishExam.ClassStudentsToExamId);

                    examResult.AwnserCount = isFinishExam.LastCompletedQuestion;
                    examResult.JoinAt = isFinishExam.JoinAt.ToShamsi();
                    examResult.TrueAwnserCount = trueAwnserCount;
                    examResult.Score = (int)Math.Round(((double)trueAwnserCount / exam.QuestionCount) * 100);

                }
                exam.ExamResult = examResult;
            }

            return View(exam);
        }


        [HttpPost("GoToExam/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudentToExam(string classId, int examId)
        {

            var exitExam = await this._context.ExamRepository
                .IsExistByExamIdByClassIdentityForOngoingAsync(classId, examId);

            if (!exitExam)
            {
                return NotFound();
            }

            var classStudentId = await this._context.ClassStudentsRepository
                .GetClassStudentIdByUserNameByClassIdentity(classId, User.Identity.Name);

            var classStudentToExam = await this._context.ClassStudentsToExamRepository
                .GetByClassStudentIdByExamIdAsync(classStudentId, examId);

            if (classStudentToExam != null && !classStudentToExam.IsFinish)
            {
                return this.returnToDoExam(examId);
            }

            if (classStudentToExam != null && classStudentToExam.IsFinish)
            {
                return returnToDisplayExam(examId, "Ongoing");
            }

            var newClassStudentToExam = new ClassStudentsToExam()
            {
                ExamId = examId,
                ClassStudentId = classStudentId
            };

            await this._context.ClassStudentsToExamRepository.AddAsync(newClassStudentToExam);

            await this._context.SaveChangesAsync();

            return this.returnToDoExam(examId);
        }


        [Route("DoExam/{examId}/{questionIndex}/{pageNumber}")]
        public async Task<IActionResult> DoExam(string classId, int examId, int questionIndex, int pageNumber)
        {


            var exitExam = await this._context.ExamRepository
                .IsExistByExamIdByClassIdentityForOngoingAsync(classId, examId);

            if (!exitExam)
            {
                return NotFound();
            }

            var classStudentId = await this._context.ClassStudentsRepository
                .GetClassStudentIdByUserNameByClassIdentity(classId, User.Identity.Name);


            var classStudentToExam = await this._context.ClassStudentsToExamRepository
                .GetByClassStudentIdByExamIdAsync(classStudentId, examId);

            if (classStudentToExam == null || classStudentToExam.IsFinish)
            {
                return returnToDisplayExam(examId, "Ongoing");
            }

            var questionCount = await this._context.QuestionsRepository
                .GetQuestionsCountByExamIdForShowAsync(examId);



            if (this.IsNotValidQuestionIndex(questionIndex, questionCount, classStudentToExam.LastCompletedQuestion))
            {
                return this.returnToDoExam(examId);
            }


            this.setDoExamViewBags(examId, questionIndex, questionCount);


            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIndexAsync(examId, questionIndex);

            var classStudentToExamQuestion = await this._context.ClassStudentToExamToQuestionRepository
                .GetByClassStudentToExamIdByQuestionIdAsync(classStudentToExam.ClassStudentsToExamId, question.QuestionId);

            List<QuestionOptionForShowViewModel> options;

            if (classStudentToExamQuestion == null)
            {
                options = await this._context.QuestionOptionsRepository
                    .GetOptionsByQuestionIdByPageNumberByAwnserIdAsync(question.QuestionId, pageNumber);
            }
            else
            {
                options = await this._context.QuestionOptionsRepository
                    .GetOptionsByQuestionIdByPageNumberByAwnserIdAsync(question.QuestionId, pageNumber, classStudentToExamQuestion.SelectedOptionId);
            }



            var optionsCount = await this._context.QuestionOptionsRepository
                .GetCountOptionsByQuestionIdAsync(question.QuestionId);

            var questionForDoViewModel = new QuestionForDoViewModel()
            {
                QuestionAsk = question.QuestionAsk,
                Options = options,
                QuestionsCount = questionCount,
                CurrentQuestionIndex = questionIndex,
                OptionsCount = optionsCount,
                QuestionId = question.QuestionId
            };



            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, options.Count()))
            {
                return this.returnToDoExam(examId, questionIndex);
            }


            this.checkNextOrPrevForViewBags(optionsCount, pageNumber);


            return View(questionForDoViewModel);
        }

        [HttpPost("DoExam/{examId}/{questionIndex}/{pageNumber}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoExam(string classId, int examId, int questionIndex, int pageNumber, QuestionForDoViewModel doExam)
        {

            var exitExam = await this._context.ExamRepository
                .IsExistByExamIdByClassIdentityForOngoingAsync(classId, examId);

            if (!exitExam)
            {
                return NotFound();
            }

            var classStudentId = await this._context.ClassStudentsRepository
                .GetClassStudentIdByUserNameByClassIdentity(classId, User.Identity.Name);


            var classStudentToExam = await this._context.ClassStudentsToExamRepository
                .GetByClassStudentIdByExamIdAsync(classStudentId, examId);

            if (classStudentToExam == null || classStudentToExam.IsFinish)
            {
                return returnToDisplayExam(examId, "Ongoing");
            }

            var questionCount = await this._context.QuestionsRepository
                .GetQuestionsCountByExamIdForShowAsync(examId);

            if (this.IsNotValidQuestionIndex(questionIndex, questionCount, classStudentToExam.LastCompletedQuestion))
            {
                return this.returnToDoExam(examId);
            }

            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIndexAsync(examId, questionIndex);

            var option = await this._context.QuestionOptionsRepository
                .GetByQuestionIdByOptionIdAsync(question.QuestionId, doExam.AwnserOptionId);

            if (option == null)
            {
                ModelState.AddModelError("AwnserOptionId", "گزینه انتخابی باید از میان گزینه های نمایش داده شده باشد");
            }

            var classStudentToExamQuestion = await this._context.ClassStudentToExamToQuestionRepository
                .GetByClassStudentToExamIdByQuestionIdAsync(classStudentToExam.ClassStudentsToExamId, question.QuestionId);


            if (!ModelState.IsValid)
            {
                this.setDoExamViewBags(examId, questionIndex, questionCount);


                List<QuestionOptionForShowViewModel> options;

                if (classStudentToExamQuestion == null)
                {
                    options = await this._context.QuestionOptionsRepository
                        .GetOptionsByQuestionIdByPageNumberByAwnserIdAsync(question.QuestionId, pageNumber);
                }
                else
                {
                    options = await this._context.QuestionOptionsRepository
                        .GetOptionsByQuestionIdByPageNumberByAwnserIdAsync(question.QuestionId, pageNumber, classStudentToExamQuestion.SelectedOptionId);
                }


                var optionsCount = await this._context.QuestionOptionsRepository
                    .GetCountOptionsByQuestionIdAsync(question.QuestionId);

                doExam.QuestionAsk = question.QuestionAsk;
                doExam.Options = options;
                doExam.OptionsCount = optionsCount;
                doExam.QuestionsCount = questionCount;
                doExam.QuestionId = question.QuestionId;
                doExam.CurrentQuestionIndex = questionIndex;

                this.setPaginationViewBags(pageNumber);

                if (this.validateUserPageNumber(pageNumber, options.Count()))
                {
                    return this.returnToDoExam(examId, questionIndex);
                }


                this.checkNextOrPrevForViewBags(optionsCount, pageNumber);


                return View(doExam);
            }




            if (classStudentToExamQuestion == null)
            {
                ClassStudentsToExamToQuestion studentToExamQuestion = new()
                {
                    ClassStudentToExamId = classStudentToExam.ClassStudentsToExamId,
                    ClassStudentsToExam = classStudentToExam,
                    QuestionId = question.QuestionId,
                    SelectedOptionId = option.QuestionOptionId,
                    SelectedOption = option,
                    Question = question,
                };

                if (question.Answer == option?.Option)
                {
                    studentToExamQuestion.IsTrueAwnser = true;
                }
                else
                {
                    studentToExamQuestion.IsTrueAwnser = false;
                }

                classStudentToExam.LastCompletedQuestion += 1;

                this._context.ClassStudentsToExamRepository
                    .Update(classStudentToExam);
                await this._context.ClassStudentToExamToQuestionRepository
                    .AddAsync(studentToExamQuestion);
            }
            else
            {
                if (question.Answer == option?.Option)
                {
                    classStudentToExamQuestion.IsTrueAwnser = true;
                }
                else
                {
                    classStudentToExamQuestion.IsTrueAwnser = false;
                }

                this._context.ClassStudentToExamToQuestionRepository
                    .Update(classStudentToExamQuestion);
            }



            await this._context.SaveChangesAsync();

            if (questionCount == classStudentToExam.LastCompletedQuestion)
            {

                classStudentToExam.IsFinish = true;

                this._context.ClassStudentsToExamRepository
                    .Update(classStudentToExam);

                await this._context.SaveChangesAsync();
                return this.returnToOnGoingExams();
            }


            return this.returnToDoExam(examId, questionIndex + 1);
        }

    }
}
