using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;
using University_Domain.Enums;

namespace University_Domain.Entities
{
    public class QuizQuestion : BaseEntity
    {
        public int QuizId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public QuizQuestionType QuestionType { get; set; }

        public decimal Points { get; set; }

        public int OrderNumber { get; set; }

        public Quiz Quiz { get; set; } = null!;

        public ICollection<QuizOption> Options { get; set; }
            = new List<QuizOption>();

        public ICollection<QuizAnswer> Answers { get; set; }
            = new List<QuizAnswer>();
    }
}
