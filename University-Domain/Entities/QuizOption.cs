using System;
using System.Collections.Generic;
using System.Text;
using University_Domain.Common;

namespace University_Domain.Entities
{
    public class QuizOption : BaseEntity
    {
        public int QuizQuestionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public int OrderNumber { get; set; }

        public QuizQuestion QuizQuestion { get; set; } = null!;

        public ICollection<QuizAnswer> SelectedAnswers { get; set; }
            = new List<QuizAnswer>();
    }
}
