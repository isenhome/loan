using System;

namespace Loan.Core.Attributes
{
    /// <summary>
    /// 维度所在的基础数据表名
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class DataTableNameAttribute : Attribute
    {
        private String _dataTableName = String.Empty;

        public String DataTableName
        {
            get { return _dataTableName; }
            set { _dataTableName = value; }
        }

        public DataTableNameAttribute() { }

        public DataTableNameAttribute(String dataTableName)
        {
            _dataTableName = dataTableName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class MobileDataTableNameAttribute : Attribute
    {
        private String _mobileDataTableName = String.Empty;

        public String MobileDataTableName
        {
            get { return _mobileDataTableName; }
            set { _mobileDataTableName = value; }
        }

        public MobileDataTableNameAttribute() { }

        public MobileDataTableNameAttribute(String mobileDataTableName)
        {
            _mobileDataTableName = mobileDataTableName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class InteractiveTableNameAttribute : Attribute
    {
        private String _interactiveTableName = String.Empty;

        public String InteractiveTableName
        {
            get { return _interactiveTableName; }
            set { _interactiveTableName = value; }
        }

        public InteractiveTableNameAttribute()
        { }

        public InteractiveTableNameAttribute(String interactiveTableName)
        {
            _interactiveTableName = interactiveTableName;
        }
    }

    /// <summary>
    /// 维度所在的UV数据表名参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class UVTableNameParameterAttribute : Attribute
    {
        private String _uvTableNameParameter = String.Empty;

        public String UVTableNameParameter
        {
            get { return _uvTableNameParameter; }
            set { _uvTableNameParameter = value; }
        }

        public UVTableNameParameterAttribute()
        { }

        public UVTableNameParameterAttribute(String uvTableNameParameter)
        {
            _uvTableNameParameter = uvTableNameParameter;
        }
    }

    /// <summary>
    /// Used for select,order by,group by
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class FieldNameAttribute : Attribute
    {
        private String _fieldName = String.Empty;

        public String FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public FieldNameAttribute()
        { }

        public FieldNameAttribute(String fieldName)
        {
            _fieldName = fieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class SecondFieldNameAttribute : Attribute
    {
        private String _secondFieldName = String.Empty;

        public String SecondFieldName
        {
            get { return _secondFieldName; }
            set { _secondFieldName = value; }
        }

        public SecondFieldNameAttribute()
        { }

        public SecondFieldNameAttribute(String secondFieldName)
        {
            _secondFieldName = secondFieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class ThirdFieldNameAttribute : Attribute
    {
        private String _thirdFieldName = String.Empty;

        public String ThirdFieldName
        {
            get { return _thirdFieldName; }
            set { _thirdFieldName = value; }
        }

        public ThirdFieldNameAttribute()
        { }

        public ThirdFieldNameAttribute(String thirdFieldName)
        {
            _thirdFieldName = thirdFieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class FourthFieldNameAttribute : Attribute
    {
        private String _fourthFieldName = String.Empty;

        public String FourthFieldName
        {
            get { return _fourthFieldName; }
            set { _fourthFieldName = value; }
        }

        public FourthFieldNameAttribute()
        { }

        public FourthFieldNameAttribute(String fourthFieldName)
        {
            _fourthFieldName = fourthFieldName;
        }
    }

    /// <summary>
    /// Used for left join table name
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class LeftJoinTableNameAttribute : Attribute
    {
        private String _leftJoinTableName = String.Empty;

        public String LeftJoinTableName
        {
            get { return _leftJoinTableName; }
            set { _leftJoinTableName = value; }
        }

        public LeftJoinTableNameAttribute()
        { }

        public LeftJoinTableNameAttribute(String leftJoinTableName)
        {
            _leftJoinTableName = leftJoinTableName;
        }
    }

    /// <summary>
    /// Used for left join table name
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class LeftJoinTableShortNameAttribute : Attribute
    {
        private String _leftJoinTableShortName = String.Empty;

        public String LeftJoinTableShortName
        {
            get { return _leftJoinTableShortName; }
            set { _leftJoinTableShortName = value; }
        }

        public LeftJoinTableShortNameAttribute()
        { }

        public LeftJoinTableShortNameAttribute(String leftJoinTableShortName)
        {
            _leftJoinTableShortName = leftJoinTableShortName;
        }
    }

    /// <summary>
    /// Used for left join table primary key
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class LeftJoinPrimaryKeyAttribute : Attribute
    {
        private String _leftJoinPrimaryKey = String.Empty;

        public String LeftJoinPrimaryKey
        {
            get { return _leftJoinPrimaryKey; }
            set { _leftJoinPrimaryKey = value; }
        }

        public LeftJoinPrimaryKeyAttribute()
        { }

        public LeftJoinPrimaryKeyAttribute(String leftJoinPrimaryKey)
        {
            _leftJoinPrimaryKey = leftJoinPrimaryKey;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class LeftJoinFieldNameAttribute : Attribute
    {
        private String _leftJoinFieldName = String.Empty;

        public String LeftJoinFieldName
        {
            get { return _leftJoinFieldName; }
            set { _leftJoinFieldName = value; }
        }

        public LeftJoinFieldNameAttribute()
        { }

        public LeftJoinFieldNameAttribute(String leftJoinFieldName)
        {
            _leftJoinFieldName = leftJoinFieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class InteractiveLeftJoinFieldNameAttribute : Attribute
    {
        private String _interactiveLeftJoinFieldName = String.Empty;

        public String InteractiveLeftJoinFieldName
        {
            get { return _interactiveLeftJoinFieldName; }
            set { _interactiveLeftJoinFieldName = value; }
        }

        public InteractiveLeftJoinFieldNameAttribute()
        { }

        public InteractiveLeftJoinFieldNameAttribute(String interactiveLeftJoinFieldName)
        {
            _interactiveLeftJoinFieldName = interactiveLeftJoinFieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class UVTablePrimaryKeyAttribute : Attribute
    {
        private String _uvTablePrimaryKey = String.Empty;

        public String UVTablePrimaryKey
        {
            get { return _uvTablePrimaryKey; }
            set { _uvTablePrimaryKey = value; }
        }

        public UVTablePrimaryKeyAttribute()
        { }

        public UVTablePrimaryKeyAttribute(String uvTablePrimaryKey)
        {
            _uvTablePrimaryKey = uvTablePrimaryKey;
        }
    }


    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class MapValueAttribute : Attribute
    {
        private Int32 _mapValue = 0;
        public Int32 MapValue
        {
            get { return _mapValue; }
            set { _mapValue = value; }
        }

        public MapValueAttribute()
        {

        }

        public MapValueAttribute(Int32 valueMap)
        {
            _mapValue = valueMap;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class MapTypeAttribute : Attribute
    {
        private string _mapType = "";
        public string MapType
        {
            get { return _mapType; }
            set { _mapType = value; }
        }

        public MapTypeAttribute()
        {

        }

        public MapTypeAttribute(string mapType)
        {
            _mapType = mapType;
        }
    }
}
