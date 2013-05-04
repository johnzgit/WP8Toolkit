using System;

namespace Ayls.WP8Toolkit.Multibinding
{
    /// <summary>
    /// Code sourced from http://www.thejoyofcode.com/MultiBinding_for_Silverlight_3.aspx
    /// </summary>
    public class Disposer : IDisposable
    {
        private readonly Action _dispose;

        public Disposer(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose();
        }
    }
}
