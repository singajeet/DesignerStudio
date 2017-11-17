/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/16/2017
 * Time: 11:38 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;

namespace ICSharpCode.SharpDevelop.Services.Utils
{
	/// <summary>
	/// Description of ListExtension.
	/// </summary>
	public static class ListExtension
	{
		public static void BubbleSort(this IList o) {
        for (int i = o.Count - 1; i >= 0; i--) {
            for (int j = 1; j <= i; j++) {
                object o1 = o[j - 1];
                object o2 = o[j];
                if (((IComparable)o1).CompareTo(o2) > 0) {
                    o.Remove(o1);
                    o.Insert(j, o1);
                	}
            	}
        	}
    	}
	}
}
