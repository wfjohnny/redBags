using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model
{
    [Serializable]
    public class BaseModel
    {
        #region<<分页>>

        private int _currentPage;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                _rowsFrom = (_currentPage - 1) * _pageSize;
                _rowsEnd = _currentPage * _pageSize;
            }
        }

        private int _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
                _rowsFrom = (_currentPage - 1) * _pageSize;
                _rowsEnd = _currentPage * _pageSize;
            }
        }
        public int PageCount { get; set; }
        private int _rowsFrom;
        public int RowsFrom
        {
            get
            {
                return _rowsFrom;
            }
            set
            {
                _rowsFrom = value;
            }

        }
        private int _rowsEnd;
        public int RowsEnd
        {
            get
            {
                return _rowsEnd;
            }
            set
            {
                _rowsEnd = value;
            }
        }

        #endregion

        public string DataStatus { get; set; }

        #region<<时间范围查询字段>>

        private string _dateField;
        /// <summary>
        /// 搜索字段
        /// </summary>
        public string DateField
        {
            get { return _dateField; }
            set { _dateField = value; }
        }

        private DateTime? _startSearchDate;
        /// <summary>
        /// 搜索开始时间
        /// </summary>
        public DateTime? StartSearchDate
        {
            get { return _startSearchDate; }
            set { _startSearchDate = value; }
        }

        private DateTime? _endSearchDate;
        /// <summary>
        /// 搜索结束时间
        /// </summary>
        public DateTime? EndSearchDate
        {
            get { return _endSearchDate; }
            set { _endSearchDate = value; }
        }

        #endregion

        #region<<排序方式指定>>

        private string _sortField;
        /// <summary>
        /// 排序字段，仅在条件查询及分页查询有效
        /// </summary>
        public string SortField
        {
            get { return _sortField; }
            set { _sortField = value; }
        }

        private string _sortType;
        /// <summary>
        /// 排序方式，仅在条件查询及分页查询有效
        /// ASC: 升序 DESC: 降序
        /// </summary>
        public string SortType
        {
            get { return _sortType; }
            set { _sortType = value; }
        }

        #endregion

        #region<<枚举查询条件>>

        private string _enumField;
        /// <summary>
        /// 枚举字段名称
        /// </summary>
        public string EnumField
        {
            get { return _enumField; }
            set { _enumField = value; }
        }

        private int _enumValue;
        /// <summary>
        /// 指定的查询条件值
        /// </summary>
        public int EnumValue
        {
            get { return _enumValue; }
            set { _enumValue = value; }
        }

        #endregion

        #region<<时间交集范围查询>>

        private string _intersectionDateFromField;
        /// <summary>
        /// 时间范围起始字段
        /// </summary>
        public string IntersectionDateFromField
        {
            get { return _intersectionDateFromField; }
            set { _intersectionDateFromField = value; }
        }

        private string _intersectionDateEndField;
        /// <summary>
        /// 时间范围结束字段
        /// </summary>
        public string IntersectionDateEndField
        {
            get { return _intersectionDateEndField; }
            set { _intersectionDateEndField = value; }
        }

        private DateTime? _intersectionDateFromValue;
        /// <summary>
        /// 查询起始时间
        /// </summary>
        public DateTime? IntersectionDateFromValue
        {
            get { return _intersectionDateFromValue; }
            set { _intersectionDateFromValue = value; }
        }

        private DateTime? _intersectionDateToValue;
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime? IntersectionDateToValue
        {
            get { return _intersectionDateToValue; }
            set { _intersectionDateToValue = value; }
        }
        #endregion

        public long? MainKey { get; set; }

        #region<<IN 查询>>


        /// <summary>
        ///INT in字段名称
        /// </summary>
        public string IntArrayField { get; set; }
        public IList<int> IntArray { get; set; }



        /// <summary>
        ///Long in字段名称
        /// </summary>
        public string LongArrayField { get; set; }
        public IList<long> LongArray { get; set; }

        /// <summary>
        ///String in字段名称
        /// </summary>
        public string StringArrayField { get; set; }
        public IList<string> StringArray { get; set; }
        #endregion

        #region<<枚举字段>>

        public IList<string> EnumFields { get; set; }

        #endregion

        #region<<模糊查询>>

        public IList<string> LikeFields { get; set; }

        #endregion

        #region<<BETWEEN>>

        public IList<IntBetweenMap> IntBetweens { get; set; }

        #endregion

        #region<<NULL处理，查询和修改>>

        public IList<string> NullFields { get; set; }

        #endregion

        #region<<MaxMin>>


        public string MaxMinField { get; set; }

        public int IsMax { get; set; }

        #endregion
    }

    public class IntBetweenMap
    {
        public string Field { get; set; }
        public int? IntValueFrom { get; set; }
        public int? IntValueTo { get; set; }
    }
}
