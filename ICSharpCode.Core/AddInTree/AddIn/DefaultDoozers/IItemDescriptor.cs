/*
 * Created by SharpDevelop.
 * User: Admin
 * Date: 11/9/2017
 * Time: 1:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.Core
{
	/// <summary>
	/// Description of IItemDescriptor.
	/// </summary>
	public interface IItemDescriptor
	{
		object Parameter { get; }
		Codon Codon { get; }
		IList SubItems { get; }
		IReadOnlyCollection<ICondition> Conditions { get; }
	}
}
