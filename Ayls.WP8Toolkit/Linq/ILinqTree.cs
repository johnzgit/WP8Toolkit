using System.Collections.Generic;

namespace Ayls.WP8Toolkit.Linq
{
    /// <summary>
    /// Defines an interface that must be implemented to generate the LinqToTree methods
    /// Code sourced from http://www.scottlogic.co.uk/blog/colin/2010/03/linq-to-visual-tree/ 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILinqTree<T>
    {
        IEnumerable<T> Children();

        T Parent { get; }
    }
}