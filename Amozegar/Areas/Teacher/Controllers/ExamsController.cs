using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Areas.Teacher.Models.Interface;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mono.TextTemplating;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Route("Panel/Teacher/{classId}/Exams")]
    public class ExamsController : BaseController
    {
        private IUnitOfWork _context;

        public ExamsController(IUnitOfWork context)
        {
            this._context = context;

        }

        // Utilities

        private IActionResult returnToExams()
        {
            return RedirectToAction("Index", "Exams", new { area = "Shared", roleName = "Teacher", classId = this.classId, pageNumber = 1 });
        }

        private IActionResult returnToExamsWithError(string error)
        {
            TempData["Error"] = error;
            return returnToExams();
        }

        private IActionResult returnToEditExam(int examId)
        {
            return RedirectToAction("EditExam", "Exams", new { area = "Teacher", classId = this.classId, examId = examId, pageNumber = 1 });
        }

        private IActionResult returnToEditQuestion(int examId, int questionId)
        {
            return RedirectToAction("EditQuestion", "Exams", new { area = "Teacher", classId = this.classId, examId = examId, questionId = questionId, pageNumber = 1 });
        }

        private IActionResult returnToEditQuestionWithError(int examId, int questionId, string errorType, string error)
        {
            TempData[errorType] = error;
            return this.returnToEditQuestion(examId, questionId);
        }

        private async Task<List<QuestionForEditViewModel>> getQuestionsForViewByExamIdAsync(int examId, int pageNumber)
        {
            List<QuestionForEditViewModel> questions = new();

            var examQuestions = await this._context.QuestionsRepository
                .GetQuestionsByExamIdForShowByPageNumberAsync(examId, pageNumber);

            int counter = 1;

            foreach (var item in examQuestions)
            {
                questions.Add(new QuestionForEditViewModel()
                {
                    Count = item.OptionsCount,
                    QuestionId = item.QuestionId,
                    Question = item.QuestionAsk,
                    Counter = counter,
                });
                counter++;
            }

            return questions;
        }

        private async Task<ExamForEditViewModel> setEditViewModel(Exam exam, IEnumerable<ExamState> states, int pageNumber)
        {
            List<SelectListItem> statesSelectList = new();

            if (exam.ExamState.State != "Closed")
            {
                states = states.Where(s => s.State != "Closed").ToList();
            }

            foreach (var state in states)
            {
                var selectItem = new SelectListItem()
                {
                    Text = state.PersianState,
                    Value = state.State
                };

                if (exam.ExamState == state)
                {
                    selectItem.Selected = true;
                }

                statesSelectList.Add(selectItem);

            }

            var questions = await this.getQuestionsForViewByExamIdAsync(exam.ExamId, pageNumber);

            var examForEditViewModel = new ExamForEditViewModel()
            {
                ExamTitle = exam.ExamTitle,
                ExamDescription = exam.ExamDescription,
                EndDate = exam.EndDate.ToShamsiDate(),
                EndTime = exam.EndDate.ToShamsiTime(),
                StartDate = exam.StartDate.ToShamsiDate(),
                StartTime = exam.StartDate.ToShamsiTime(),
                States = statesSelectList,
                Questions = questions
            };

            ViewBag.ExamId = exam.ExamId;

            return examForEditViewModel;
        }

        private async Task<QuestionEditViewModel> setEditViewModelForQuestion(Question question, int examId, int pageNumber)
        {
            var questionEditViewModel = new QuestionEditViewModel()
            {
                QuestionAsk = question.QuestionAsk,
                QuestionId = question.QuestionId
            };

            ViewBag.ExamId = examId;

            var questionOptions = await this._context.QuestionOptionsRepository
                .GetOptionsByQuestionIdByPageNumberAsync(question.QuestionId, pageNumber);

            List<QuestionOptionsForEditViewModel> options = new();

            foreach (var option in questionOptions)
            {
                options.Add(new()
                {
                    IsCorrect = option.Option == question.Answer,
                    QuestionOption = option.Option,
                    QuestionOptionId = option.QuestionOptionId
                });
            }

            questionEditViewModel.Options = options;

            return questionEditViewModel;
        }

        private List<DateTime>? validateDates(IExamDateInput dates)
        {
            var hasError = false;

            var now = DateTime.Now;
            Func<string, bool> IsInvalidTime = (input) =>
            {
                try
                {
                    var time = input.Split(":").Select(int.Parse).ToList();
                    return (time[0] >= 24 || time[1] >= 60);
                }
                catch
                {
                    return true;
                }
            };
            Func<string, bool> IsInvalidDate = (input) =>
            {

                try
                {
                    var date = input.Split("/").Select(int.Parse).ToList();
                    return (
                        date[0].ToString().Length < 4 ||
                        date[0].ToString().Length > 4 ||
                        date[1] > 12 ||
                        date[1] < 1 ||
                        date[2] > DateConvertor.GetDaysOfPersianDate(date[0], date[1]) ||
                        date[2] < 1
                    );
                }
                catch
                {
                    return true;
                }
            };

            if (IsInvalidTime(dates.StartTime))
            {
                ModelState.AddModelError("StartTime", "ساعت شروع درست نمیباشد");
                hasError = true;
            }

            if (IsInvalidTime(dates.EndTime))
            {
                ModelState.AddModelError("EndTime", "ساعت پایان درست نمیباشد");
                hasError = true;
            }

            if (IsInvalidDate(dates.StartDate))
            {
                ModelState.AddModelError("StartDate", "تاریخ شروع درست نمیباشد");
                hasError = true;
            }

            if (IsInvalidDate(dates.EndDate))
            {
                ModelState.AddModelError("EndDate", "تاریخ پایان درست نمیباشد");
                hasError = true;
            }

            if (hasError == true)
            {
                return null;
            }

            var startDate = DateConvertor.ToDateTime(dates.StartDate, dates.StartTime);
            var endDate = DateConvertor.ToDateTime(dates.EndDate, dates.EndTime);


            if (startDate.Date < now.Date)
            {
                ModelState.AddModelError("StartDate", "تاریخ شروع نمیتواند برای گذشته باشد");
                hasError = true;
            }
            else if (startDate.Date == now.Date && startDate.TimeOfDay <= now.TimeOfDay)
            {
                ModelState.AddModelError("StartTime", "ساعت شروع نمیتواند برای گذشته باشد");
                hasError = true;
            }

            if (endDate.Date < now.Date)
            {
                ModelState.AddModelError("EndDate", "تاریخ پایان نمیتواند برای گذشته باشد");
                hasError = true;
            }
            else if (endDate.Date == now.Date && endDate.TimeOfDay <= now.TimeOfDay)
            {
                ModelState.AddModelError("EndTime", "ساعت پایان نمیتواند برای گذشته باشد");
                hasError = true;
            }

            if (startDate >= endDate)
            {
                ModelState.AddModelError("EndTime", "زمان و تاریخ پایان امتحان نمیتواند برابر یا کوچک تر از زمان و تاریخ شروع امتحان باشد");
                hasError = true;
            }


            if (hasError == true)
            {
                return null;
            }

            return new List<DateTime>()
            {
                startDate,
                endDate
            };

        }

        private async Task<int> getQuestionCountByExamId(int examId)
        {
            var examQuestions = await this._context.QuestionsRepository
                .CountAsync(q => q.ExamId == examId);
            return examQuestions;
        }

        private async Task<bool> validateForChnageState(int examId, string? state, IEnumerable<ExamState> validStates)
        {
            var examQuestions = await this.getQuestionCountByExamId(examId);

            if (string.IsNullOrEmpty(state))
            {
                ModelState.AddModelError("States", "لطفا یک وضعیت را انتخاب کنید");
                return true;
            }

            if (!validStates.Any(vs => vs.State == state))
            {
                ModelState.AddModelError("States", "لطفا یک وضعیت را انتخاب کنید");
                return true;
            }

            if (examQuestions <= 0 && state == "Scheduled")
            {
                ModelState.AddModelError("States", "برای تغییر وضعیت به زمان بندی شده امتحان حدقل نیازمند یک سوال میباشد");
                return true;
            }



            return false;
        }

        private void setTempDataForModelStateError(string errorType)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            string errorHtml = "<ul>";
            foreach (var error in errors)
            {
                errorHtml += $"<li>{error}</li>";
            }
            errorHtml += "</ul>";

            TempData[errorType] = errorHtml;
        }

        private void setTempDatasForModelStateInExams(AddQuestionViewModel addQuestion)
        {
            if (addQuestion.Options != null && addQuestion.Options.Any())
            {
                TempData["Options"] = JsonSerializer.Serialize(addQuestion.Options);
            }
            if (!string.IsNullOrEmpty(addQuestion.QuestionAsk))
            {
                TempData["Question"] = addQuestion.QuestionAsk;
            }

            this.setTempDataForModelStateError("Error");
        }

        private void setTempDatasForModelStateInQuestions(List<string>? options)
        {
            if (options != null && options.Any())
            {
                TempData["Options"] = JsonSerializer.Serialize(options);
            }

            this.setTempDataForModelStateError("Error");
        }

        private async Task saveQuestionAsync(AddQuestionViewModel addQuestion, Exam exam)
        {
            var question = new Question()
            {
                Answer = addQuestion.CorrectAnswer,
                QuestionAsk = addQuestion.QuestionAsk,
                ExamId = exam.ExamId,
                Exam = exam,
            };

            await this._context.QuestionsRepository
                .AddAsync(question);

            await this._context.SaveChangesAsync();

            var questionOptions = new List<QuestionOption>();

            foreach (var option in addQuestion.Options)
            {
                var questionOption = new QuestionOption()
                {
                    Option = option,
                    Question = question,
                    QuestionId = question.QuestionId,
                };
                await this._context.QuestionOptionsRepository
                    .AddAsync(questionOption);
            }
            await this._context.SaveChangesAsync();
        }

        private async Task<Exam?> getExamByExamId(int examId)
        {
            var exam = await this._context.ExamRepository
                .GetByClassIdentityByExamIdAsync(this.classId, examId);
            return exam;
        }

        private async Task<IEnumerable<ExamState>> getValidExamStates()
        {
            var states = await this._context.ExamStatesRepository
                .GetStatesByStates("Draft", "Scheduled", "Closed");
            return states;
        }

        // Main Methods

        [Route("Add")]
        public IActionResult AddExams(string classId)
        {
            return View();
        }

        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExams(string classId, AddOrEditExamViewModel add)
        {

            if (!ModelState.IsValid)
            {
                return View(add);
            }
            var dates = this.validateDates(add);
            if (dates == null)
            {
                return View(add);
            }

            var examState = await this._context.ExamStatesRepository.GetByStateAsync("Draft");
            var cls = await this._context.ClassesRepository.GetByClassIdentityAsync(this.classId);

            var exam = new Exam()
            {
                ClassId = cls.ClassId,
                ClassRoam = cls,
                EndDate = dates[1],
                StartDate = dates[0],
                ExamDescription = add.ExamDescription,
                ExamState = examState,
                ExamStateId = examState.ExamStateId,
                ExamTitle = add.ExamTitle,
            };

            await this._context.ExamRepository.AddAsync(exam);
            await this._context.SaveChangesAsync();

            return returnToExams();
        }

        [HttpPost("Delete/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExam(string classId, int examId)
        {
            var exam = await this.getExamByExamId(examId);
            if (exam == null)
            {
                return this.returnToExams();
            }

            if (
                    exam.ExamState.State != "Scheduled" &&
                    exam.ExamState.State != "Draft" &&
                    exam.ExamState.State != "Closed"
                )
            {
                return this.returnToExamsWithError($"امکان حذف امتحانات در حال برگزاری یا تکمیل شده نیست");
            }




            await this._context.ExamRepository
                .ChangeStateByExamAsync(exam, "Deleted");

            await this._context.SaveChangesAsync();

            return this.returnToExams();
        }

        [HttpPost("CloseExam/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseExam(string classId, int examId)
        {
            var exam = await this.getExamByExamId(examId);
            if (exam == null)
            {
                return this.returnToExams();
            }

            if (exam.ExamState.State != "Ongoing")
            {
                return this.returnToExamsWithError($"بستن امتحان فقط در حالت در حال برگزاری ممکن است");
            }

            var classStudentsToExam = await this._context.ClassStudentsToExamRepository
                .GetByExamIdAsync(examId);

            var classStudentToExamToQuestions = classStudentsToExam.SelectMany(cste => cste.ClassStudentsToExamToQuestions).ToList();

            this._context.ClassStudentToExamToQuestionRepository.Delete(classStudentToExamToQuestions);

            this._context.ClassStudentsToExamRepository.Delete(classStudentsToExam);

            await this._context.ExamRepository
                .ChangeStateByExamAsync(exam, "Closed");

            await this._context.SaveChangesAsync();

            return this.returnToExams();
        }

        [Route("Edit/{examId}/{pageNumber}")]
        public async Task<IActionResult> EditExam(string classId, int examId, int pageNumber)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }


            var examForEditViewModel = await this.setEditViewModel(exam, states, pageNumber);

            this.setPaginationViewBags(pageNumber);


            if (this.validateUserPageNumber(pageNumber, examForEditViewModel.Questions.Count()))
            {
                return this.returnToEditExam(examId);
            }

            var questionsCount = await this._context.QuestionsRepository
                .GetQuestionsCountByExamIdForShowAsync(examId);

            this.checkNextOrPrevForViewBags(questionsCount, pageNumber);


            return View(examForEditViewModel);
        }
        [HttpPost("Edit/{examId}/{pageNumber}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExam(string classId, int examId, int pageNumber, AddOrEditExamViewModel edit)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }

            var examForEditViewModel = await this.setEditViewModel(exam, states, pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, examForEditViewModel.Questions.Count()))
            {
                return this.returnToEditExam(examId);
            }

            var questionsCount = await this._context.QuestionsRepository
                .GetQuestionsCountByExamIdForShowAsync(examId);

            this.checkNextOrPrevForViewBags(questionsCount, pageNumber);

            examForEditViewModel.ExamTitle = edit.ExamTitle;
            examForEditViewModel.ExamDescription = edit.ExamDescription;
            examForEditViewModel.StartDate = edit.StartDate;
            examForEditViewModel.StartTime = edit.StartTime;
            examForEditViewModel.EndDate = edit.EndDate;
            examForEditViewModel.EndTime = edit.EndTime;

            if (!ModelState.IsValid)
            {
                return View(examForEditViewModel);
            }

            var dates = this.validateDates(edit);
            if (dates == null)
            {
                return View(examForEditViewModel);
            }

            if (await this.validateForChnageState(examId, edit.State, states))
            {
                return View(examForEditViewModel);
            }

            var state = states.Single(vs => vs.State == edit.State);

            exam.StartDate = dates[0];
            exam.EndDate = dates[1];
            exam.ExamTitle = examForEditViewModel.ExamTitle;
            exam.ExamDescription = examForEditViewModel.ExamDescription;
            exam.ExamState = state;
            exam.ExamStateId = state.ExamStateId;

            await this._context.SaveChangesAsync();


            return returnToExams();
        }

        // Questions

        [HttpPost("AddQuestion/{examId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestion(string classId, int examId, AddQuestionViewModel addQuestion)
        {
            int pageNumber = 1;
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null || !states.Contains(exam.ExamState) || addQuestion.ExamId != examId)
            {
                return this.returnToExams();
            }
            var examForEditViewModel = await this.setEditViewModel(exam, states, pageNumber);


            this.setPaginationViewBags(pageNumber);


            if (this.validateUserPageNumber(pageNumber, examForEditViewModel.Questions.Count()))
            {
                return this.returnToEditExam(examId);
            }

            var questionsCount = await this._context.QuestionsRepository
                .GetQuestionsCountByExamIdForShowAsync(examId);

            this.checkNextOrPrevForViewBags(questionsCount, pageNumber);

            if (addQuestion.Options.Count() > 0 && addQuestion.Options.Any(o => string.IsNullOrEmpty(o) || o.Length > 500))
            {
                ModelState.AddModelError("Options", "گزینه ها نمیتوانند خالی یا بزرگتر از 500 کاراکتر باشند");
            }

            if (!ModelState.IsValid)
            {

                this.setTempDatasForModelStateInExams(addQuestion);

                return returnToEditExam(examId);
            }

            await this.saveQuestionAsync(addQuestion, exam);

            return this.returnToEditExam(examId);
        }

        [HttpPost("DeleteQuestion/{examId}/{questionId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(string classId, int examId, int questionId)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }

            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIdWithOptionsAsync(examId, questionId);

            if (question == null)
            {
                return this.returnToEditExam(examId);
            }

            var questionCount = await this.getQuestionCountByExamId(examId);

            if (questionCount <= 2)
            {
                TempData["QuestionError"] = "حذف سوال زمانی که کمتر از 3 سوال وجود دارد امکان پذیر نیست";
                return this.returnToEditExam(examId);
            }

            var questionOptions = await this._context.QuestionOptionsRepository
                .GetOptionsByQuestionIdAsync(questionId);
            foreach (var option in questionOptions)
            {
                this._context.QuestionOptionsRepository
                    .Delete(option);
            }

            this._context.QuestionsRepository
                .Delete(question);

            await this._context.SaveChangesAsync();

            return returnToEditExam(examId);
        }

        [Route("EditQuestion/{examId}/{questionId}/{pageNumber}")]
        public async Task<IActionResult> EditQuestion(string classId, int examId, int questionId, int pageNumber)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }


            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIdWithOptionsAsync(examId, questionId);

            if (question == null)
            {
                return this.returnToEditExam(examId);
            }

            var questionEditViewModel = await this.setEditViewModelForQuestion(question, examId, pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, questionEditViewModel.Options.Count()))
            {
                return this.returnToEditQuestion(examId, questionId);
            }


            var optionsCount = await this._context.QuestionOptionsRepository
                .GetCountOptionsByQuestionIdAsync(questionId);

            this.checkNextOrPrevForViewBags(optionsCount, pageNumber);

            return View(questionEditViewModel);
        }

        [HttpPost("EditQuestion/{examId}/{questionId}/{pageNumber}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(string classId, int examId, int questionId, int pageNumber, EditQuestionAskViewModel edit)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }

            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIdWithOptionsAsync(examId, questionId);

            if (question == null)
            {
                return this.returnToEditExam(examId);
            }

            var questionEditViewModel = await this.setEditViewModelForQuestion(question, examId, pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, questionEditViewModel.Options.Count()))
            {
                return this.returnToEditQuestion(examId, questionId);
            }


            var optionsCount = await this._context.QuestionOptionsRepository
                .GetCountOptionsByQuestionIdAsync(questionId);

            this.checkNextOrPrevForViewBags(optionsCount, pageNumber);

            if (!ModelState.IsValid)
            {
                return View(questionEditViewModel);
            }

            question.QuestionAsk = edit.QuestionAsk;
            this._context.QuestionsRepository.Update(question);
            await this._context.SaveChangesAsync();

            return returnToEditQuestion(examId, questionId);
        }

        [HttpPost("EditQuestion/AddOptions/{examId}/{questionId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOption(string classId, int examId, int questionId, AddOptionViewModel add)
        {

            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }

            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIdWithOptionsAsync(examId, questionId);

            if (question == null)
            {
                return this.returnToEditExam(examId);
            }

            if (add.Options?.Count() > 0)
            {
                var isExist = await this._context.QuestionOptionsRepository
                    .ExistByQuestionIdByOptionsAsync(questionId, add.Options);
                if (isExist)
                {
                    ModelState.AddModelError("Options", "امکان افزودن گزینه‌ی تکراری وجود ندارد");
                }
            }

            if (!ModelState.IsValid)
            {
                this.setTempDatasForModelStateInQuestions(add.Options);
                return this.returnToEditQuestion(examId, questionId);
            }


            foreach (var option in add.Options)
            {
                await this._context.QuestionOptionsRepository
                    .AddAsync(new QuestionOption()
                    {
                        Option = option,
                        Question = question,
                        QuestionId = question.QuestionId,
                    });
            }

            await this._context.SaveChangesAsync();

            return this.returnToEditQuestion(examId, questionId);
        }

        [HttpPost("EditQuestion/DeleteOption/{examId}/{questionId}/{optionId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOption(string classId, int examId, int questionId, int optionId)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }

            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIdWithOptionsAsync(examId, questionId);
            if (question == null)
            {
                return this.returnToEditExam(examId);
            }

            var option = await this._context.QuestionOptionsRepository
                .GetByQuestionIdByOptionIdAsync(questionId, optionId);

            if (option == null)
            {
                return this.returnToEditQuestion(examId, questionId);
            }

            var optionsCount = await this._context.QuestionOptionsRepository
                .GetCountOptionsByQuestionIdAsync(questionId);

            if (optionsCount < 3)
            {
                return this.returnToEditQuestionWithError(examId, questionId, "SimpleError", "حذف گزینه ها زمانی که کمتر از 3 گزینه وجود دارد امکان پذیر نیست");
            }

            if (option.Option == question.Answer)
            {
                return this.returnToEditQuestionWithError(examId, questionId, "SimpleError", "امکان حذف جواب وجود ندارد");
            }


            this._context.QuestionOptionsRepository
                .Delete(option);

            await this._context.SaveChangesAsync();

            return this.returnToEditQuestion(examId, questionId);
        }

        [HttpPost("EditQuestion/SetAsAwnser/{examId}/{questionId}/{optionId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAsAwnser(string classId, int examId, int questionId, int optionId)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }

            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIdWithOptionsAsync(examId, questionId);
            if (question == null)
            {
                return this.returnToEditExam(examId);
            }

            var option = await this._context.QuestionOptionsRepository
                .GetByQuestionIdByOptionIdAsync(questionId, optionId);

            if (option == null)
            {
                return this.returnToEditQuestion(examId, questionId);
            }

            question.Answer = option.Option;

            this._context.QuestionsRepository
                .Update(question);

            await this._context.SaveChangesAsync();


            return this.returnToEditQuestion(examId, questionId);
        }


        [HttpPost("EditQuestion/EditOption/{examId}/{questionId}/{optionId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOption(string classId, int examId, int questionId, int optionId, EditOptionViewModel edit)
        {
            var exam = await this.getExamByExamId(examId);
            var states = await this.getValidExamStates();

            if (exam == null)
            {
                return this.returnToExams();
            }

            if (!states.Contains(exam.ExamState))
            {
                return this.returnToExamsWithError("امکان ویرایش امتحانات تکمیل شده یا در‌حال‌برگزاری نیست");
            }

            var question = await this._context.QuestionsRepository
                .GetQuestionByExamIdByQuestionIdWithOptionsAsync(examId, questionId);
            if (question == null)
            {
                return this.returnToEditExam(examId);
            }

            var option = await this._context.QuestionOptionsRepository
                .GetByQuestionIdByOptionIdAsync(questionId, optionId);

            if (option == null)
            {
                return this.returnToEditQuestion(examId, questionId);
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .First();
                return this.returnToEditQuestionWithError(examId, questionId, "EditError", errors);
            }

            if (question.Answer == option.Option)
            {
                question.Answer = edit.Option;
            }

            option.Option = edit.Option;

            this._context.QuestionsRepository
                .Update(question);

            this._context.QuestionOptionsRepository
                .Update(option);

            await this._context.SaveChangesAsync();

            return this.returnToEditQuestion(examId, questionId);
        }

        [Route("ExamResult/{examId}/{pageNumber}")]
        public async Task<IActionResult> ExamResult(string classId, int examId, int pageNumber)
        {
            ViewBag.Route = "Exams";

            var examResult = await this._context.ExamRepository
                .GetExamResultsByIdByClassIdentityAsync(pageNumber, classId, examId);

            if (examResult == null)
            {
                return this.returnToExams();
            }

            var studentResultCount = await this._context.ClassStudentsToExamRepository
                .CountByExamIdForShowAsync(examId);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, examResult.StudentsResults.Count()))
            {
                return RedirectToAction("ExamResult", "Exams", new { area = "Teacher", classId = classId, examId = examId, pageNumber = 1 });
            }

            this.checkNextOrPrevForViewBags(studentResultCount, pageNumber);

            return View(examResult);
        }


    }
}
