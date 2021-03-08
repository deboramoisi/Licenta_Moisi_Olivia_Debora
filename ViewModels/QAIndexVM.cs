using Licenta.Models.QandA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licenta.ViewModels
{
    public class QAIndexVM
    {
        public IEnumerable<Question> questions { get; set; }
        public IEnumerable<QuestionCategory> questionCategories { get; set; }

        public List<int> selectedCategories { get; set; }
    }
}
