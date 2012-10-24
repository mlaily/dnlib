﻿using System.Collections.Generic;
using dot10.DotNet.MD;

namespace dot10.DotNet.Writer {
	/// <summary>
	/// Creates rows in a table. Rows can optionally be shared to create a compact table.
	/// </summary>
	/// <typeparam name="T">The raw row type</typeparam>
	public class MDTable<T> {
		readonly Table table;
		readonly Dictionary<T, uint> cachedDict;
		readonly List<T> cached;
		bool isSorted;

		/// <summary>
		/// Gets the table type
		/// </summary>
		public Table Table {
			get { return table; }
		}

		/// <summary>
		/// <c>true</c> if the table is empty
		/// </summary>
		public bool IsEmpty {
			get { return cached.Count == 0; }
		}

		/// <summary>
		/// Gets the number of rows in this table
		/// </summary>
		public int Rows {
			get { return cached.Count; }
		}

		/// <summary>
		/// Gets/sets a value indicating whether it's sorted
		/// </summary>
		public bool IsSorted {
			get { return isSorted; }
			set { isSorted = value; }
		}

		/// <summary>
		/// Gets the value with rid <paramref name="rid"/>
		/// </summary>
		/// <param name="rid">The row ID</param>
		public T this[uint rid] {
			get { return cached[(int)rid - 1]; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="table">The table type</param>
		/// <param name="equalityComparer">Equality comparer</param>
		public MDTable(Table table, IEqualityComparer<T> equalityComparer) {
			this.table = table;
			this.cachedDict = new Dictionary<T, uint>(equalityComparer);
			this.cached = new List<T>();
		}

		/// <summary>
		/// Adds a row. If the row already exists, returns a rid to the existing one, else
		/// it's created and a new rid is returned.
		/// </summary>
		/// <param name="row">The row. It's now owned by us and must NOT be modified by the caller.</param>
		/// <returns>The RID (row ID) of the row</returns>
		public uint Add(T row) {
			uint rid;
			if (cachedDict.TryGetValue(row, out rid))
				return rid;
			return Create(row);
		}

		/// <summary>
		/// Creates a new row even if this row already exists.
		/// </summary>
		/// <param name="row">The row. It's now owned by us and must NOT be modified by the caller.</param>
		/// <returns>The RID (row ID) of the row</returns>
		public uint Create(T row) {
			uint rid = (uint)cached.Count + 1;
			if (!cachedDict.ContainsKey(row))
				cachedDict[row] = rid;
			cached.Add(row);
			return rid;
		}
	}
}
