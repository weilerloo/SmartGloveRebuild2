#if ANDROID
using SmartGloveRebuild2.Platforms.Android;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Handlers
{
    public class BorderlessEntry : Entry
    {
#if ANDROID

        public BorderlessEntry()
        {
            Completed += OnCompleted;
        }

        private void OnCompleted(object sender, EventArgs e)
        {
            KeyboardHelper.HideKeyboard();
        }
#endif

    }
}
