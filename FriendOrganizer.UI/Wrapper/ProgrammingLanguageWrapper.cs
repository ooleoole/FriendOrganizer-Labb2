using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Wrapper
{
    public class ProgrammingLanguageWrapper : ModelWrapper<ProgrammingLanguage>
    {
        public int Id => Model.Id;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ProgrammingLanguageWrapper(ProgrammingLanguage model) : base(model)
        {
        }
    }
}
