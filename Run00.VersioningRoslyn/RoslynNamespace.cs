﻿using Roslyn.Compilers.Common;
using Run00.Versioning;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Run00.VersioningRoslyn
{
	public class RoslynNamespace : IContractItem
	{
		public RoslynNamespace(INamespaceSymbol namespaceSymbol)
		{
			Contract.Requires(namespaceSymbol == null);

			_namespace = namespaceSymbol;
		}

		bool IContractItem.IsPrivate { get { return false; } }

		bool IContractItem.IsCodeBlock { get { return false; } }

		IEnumerable<IContractItem> IContractItem.Children
		{
			get
			{
				return _namespace.GetTypeMembers().AsEnumerable().Select(n => (IContractItem)new RoslynType(n)).Union(
							 _namespace.GetNamespaceMembers().Select(n => (IContractItem)new RoslynNamespace(n)));
			}
		}

		string IContractItem.Name
		{
			get { return _namespace.Name; }
		}

		bool IContractItem.IsMatchedWith(IContractItem item)
		{
			return ((IContractItem)this).Name.Equals(item.Name);
		}

		string IContractItem.ToFullString()
		{
			return _namespace.ToDisplayString();
		}

		bool IContractItem.IsEquivalentTo(IContractItem node)
		{
			return false;
		}

		private readonly INamespaceSymbol _namespace;
	}
}
