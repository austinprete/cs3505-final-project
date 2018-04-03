// Written by Austin Prete
// Sept. 13, 2017
//
// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadsheetUtilities
{
    // A simple type alias for a tuple of 2 strings
    using DependencyPair = Tuple<string, string>;

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        // The set that dependency pairs will be stored in
        private HashSet<DependencyPair> dependencies;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            // Initialize dependencies on instantiation
            dependencies = new HashSet<DependencyPair>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return dependencies.Count; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get {
                return GetDependees(s).Count();
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            foreach (DependencyPair pair in dependencies)
            {
                // Return true if we encounter a pair with s as a dependee
                if (pair.Item1.Equals(s))
                    return true;
            }

            // Otherwise we return false
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            foreach (DependencyPair pair in dependencies)
            {
                // Return true if we encounter a pair with s as a dependent
                if (pair.Item2.Equals(s))
                    return true;
            }

            // Otherwise we return false
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            var dependents = dependencies
                // Filters down to pairs containing dependents of s
                .Where(pair => pair.Item1.Equals(s))
                // Maps across list of pairs to only include the dependent
                .Select(pair => pair.Item2);

            return dependents;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            var dependees = dependencies
                // Filters down to pairs containing dependees of s
                .Where(pair => pair.Item2.Equals(s))
                // Maps across list of pairs to only include the dependee
                .Select(pair => pair.Item1);

            return dependees;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            dependencies.Add(new DependencyPair(s, t));
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            dependencies.Remove(new DependencyPair(s, t));
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // Removes any pairs where s is the dependee
            dependencies.RemoveWhere(pair => pair.Item1.Equals(s));

            foreach (string t in newDependents)
            {
                dependencies.Add(new DependencyPair(s, t));
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            // Removes any pairs where s is the dependent
            dependencies.RemoveWhere(pair => pair.Item2.Equals(s));

            foreach (string t in newDependees)
            {
                dependencies.Add(new DependencyPair(t, s));
            }
        }

    }

}