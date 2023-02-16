using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveOvertime.Controls
{
    public class UnFocusTriggerAction : TriggerAction<Entry>
    {
        protected override void Invoke(Entry entry)
        {
            entry.Unfocus();
        }
    }
}
