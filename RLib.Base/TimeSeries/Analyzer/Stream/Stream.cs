﻿///********************************************************************
//    created:	2018/1/9 14:48:26
//    author:	rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using DataModel;
//

//namespace RLib.Base
//{
//    public class Stream<T> :  RObservableCollection<T>, IStream<T>, IReadonlyStream, IStream, IReadonlyStream<T> /*where T:IComparable*/
//    {
//#region Overrides
//        public override string ToString() { return "Stream:"+ID; }
//#endregion

//#region IGroupItem<ulong>
//        public string         ID { get { return m_strID; } }
//       /// <summary>
//        /// Occurs after a property value changes.
//        /// </summary>
//        public event PropertyChangedEventHandler PropertyChanged;

//        /// <summary>
//        /// Provides access to the PropertyChanged event handler to derived classes.
//        /// </summary>
//        protected PropertyChangedEventHandler PropertyChangedHandler
//        {
//            get
//            {
//                return PropertyChanged;
//            }
//        }

//#if !PORTABLE && !SL4
//        /// <summary>
//        /// Occurs before a property value changes.
//        /// </summary>
//        public event PropertyChangingEventHandler PropertyChanging;

//        /// <summary>
//        /// Provides access to the PropertyChanging event handler to derived classes.
//        /// </summary>
//        protected PropertyChangingEventHandler PropertyChangingHandler
//        {
//            get
//            {
//                return PropertyChanging;
//            }
//        }
//#endif

//        /// <summary>
//        /// Verifies that a property name exists in this ViewModel. This method
//        /// can be called before the property is used, for instance before
//        /// calling RaisePropertyChanged. It avoids errors when a property name
//        /// is changed but some places are missed.
//        /// </summary>
//        /// <remarks>This method is only active in DEBUG mode.</remarks>
//        /// <param name="propertyName">The name of the property that will be
//        /// checked.</param>
//        [Conditional("DEBUG")]
//        [DebuggerStepThrough]
//        public void VerifyPropertyName(string propertyName)
//        {
//            var myType = GetType();

//#if NETFX_CORE
//            var info = myType.GetTypeInfo();

//            if (!string.IsNullOrEmpty(propertyName)
//                && info.GetDeclaredProperty(propertyName) == null)
//            {
//                // Check base types
//                var found = false;

//                while (info.BaseType != typeof(Object))
//                {
//                    info = info.BaseType.GetTypeInfo();

//                    if (info.GetDeclaredProperty(propertyName) != null)
//                    {
//                        found = true;
//                        break;
//                    }
//                }

//                if (!found)
//                {
//                    throw new ArgumentException("Property not found", propertyName);
//                }
//            }
//#else
//            if (!string.IsNullOrEmpty(propertyName)
//                && myType.GetProperty(propertyName) == null)
//            {
//#if !SILVERLIGHT
//                var descriptor = this as ICustomTypeDescriptor;

//                if (descriptor != null)
//                {
//                    if (descriptor.GetProperties()
//                        .Cast<PropertyDescriptor>()
//                        .Any(property => property.Name == propertyName))
//                    {
//                        return;
//                    }
//                }
//#endif

//                throw new ArgumentException("Property not found", propertyName);
//            }
//#endif
//        }

//#if !PORTABLE && !SL4
//#if CMNATTR
//        /// <summary>
//        /// Raises the PropertyChanging event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">(optional) The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanging(
//            [CallerMemberName] string propertyName = null)
//#else
//        /// <summary>
//        /// Raises the PropertyChanging event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanging(
//            string propertyName)
//#endif
//        {
//            VerifyPropertyName(propertyName);

//            var handler = PropertyChanging;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangingEventArgs(propertyName));
//            }
//        }
//#endif

//#if CMNATTR
//        /// <summary>
//        /// Raises the PropertyChanged event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">(optional) The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanged(
//            [CallerMemberName] string propertyName = null)
//#else
//        /// <summary>
//        /// Raises the PropertyChanged event if needed.
//        /// </summary>
//        /// <remarks>If the propertyName parameter
//        /// does not correspond to an existing property on the current class, an
//        /// exception is thrown in DEBUG configuration only.</remarks>
//        /// <param name="propertyName">The name of the property that
//        /// changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        public virtual void RaisePropertyChanged(
//            string propertyName) 
//#endif
//        {
//            VerifyPropertyName(propertyName);

//            var handler = PropertyChanged;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }

//#if !PORTABLE && !SL4
//        /// <summary>
//        /// Raises the PropertyChanging event if needed.
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changes.</typeparam>
//        /// <param name="propertyExpression">An expression identifying the property
//        /// that changes.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        [SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:GenericMethodsShouldProvideTypeParameter",
//            Justification = "This syntax is more convenient than other alternatives.")]
//        public virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
//        {
//            var handler = PropertyChanging;
//            if (handler != null)
//            {
//                var propertyName = GetPropertyName(propertyExpression);
//                handler(this, new PropertyChangingEventArgs(propertyName));
//            }
//        }
//#endif

//        /// <summary>
//        /// Raises the PropertyChanged event if needed.
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="propertyExpression">An expression identifying the property
//        /// that changed.</param>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1030:UseEventsWhereAppropriate",
//            Justification = "This cannot be an event")]
//        [SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:GenericMethodsShouldProvideTypeParameter",
//            Justification = "This syntax is more convenient than other alternatives.")]
//        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
//        {
//            var handler = PropertyChanged;

//            if (handler != null)
//            {
//                var propertyName = GetPropertyName(propertyExpression);

//                if (!string.IsNullOrEmpty(propertyName))
//                {
//                    // ReSharper disable once ExplicitCallerInfoArgument
//                    RaisePropertyChanged(propertyName);
//                }
//            }
//        }

//        /// <summary>
//        /// Extracts the name of a property from an expression.
//        /// </summary>
//        /// <typeparam name="T">The type of the property.</typeparam>
//        /// <param name="propertyExpression">An expression returning the property's name.</param>
//        /// <returns>The name of the property returned by the expression.</returns>
//        /// <exception cref="ArgumentNullException">If the expression is null.</exception>
//        /// <exception cref="ArgumentException">If the expression does not represent a property.</exception>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1011:ConsiderPassingBaseTypesAsParameters",
//            Justification = "This syntax is more convenient than the alternatives."), 
//         SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:DoNotNestGenericTypesInMemberSignatures",
//            Justification = "This syntax is more convenient than the alternatives.")]
//        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
//        {
//            if (propertyExpression == null)
//            {
//                throw new ArgumentNullException("propertyExpression");
//            }

//            var body = propertyExpression.Body as MemberExpression;

//            if (body == null)
//            {
//                throw new ArgumentException("Invalid argument", "propertyExpression");
//            }

//            var property = body.Member as PropertyInfo;

//            if (property == null)
//            {
//                throw new ArgumentException("Argument is not a property", "propertyExpression");
//            }

//            return property.Name;
//        }

//        /// <summary>
//        /// Assigns a new value to the property. Then, raises the
//        /// PropertyChanged event if needed. 
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="propertyExpression">An expression identifying the property
//        /// that changed.</param>
//        /// <param name="field">The field storing the property's value.</param>
//        /// <param name="newValue">The property's value after the change
//        /// occurred.</param>
//        /// <returns>True if the PropertyChanged event has been raised,
//        /// false otherwise. The event is not raised if the old
//        /// value is equal to the new value.</returns>
//        [SuppressMessage(
//            "Microsoft.Design",
//            "CA1006:DoNotNestGenericTypesInMemberSignatures",
//            Justification = "This syntax is more convenient than the alternatives."), 
//         SuppressMessage(
//            "Microsoft.Design", 
//            "CA1045:DoNotPassTypesByReference",
//            MessageId = "1#",
//            Justification = "This syntax is more convenient than the alternatives.")]
//        protected bool Set<T>(
//            Expression<Func<T>> propertyExpression,
//            ref T field,
//            T newValue)
//        {
//            if (EqualityComparer<T>.Default.Equals(field, newValue))
//            {
//                return false;
//            }

//#if !PORTABLE && !SL4
//            RaisePropertyChanging(propertyExpression);
//#endif
//            field = newValue;
//            RaisePropertyChanged(propertyExpression);
//            return true;
//        }

//        /// <summary>
//        /// Assigns a new value to the property. Then, raises the
//        /// PropertyChanged event if needed. 
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="propertyName">The name of the property that
//        /// changed.</param>
//        /// <param name="field">The field storing the property's value.</param>
//        /// <param name="newValue">The property's value after the change
//        /// occurred.</param>
//        /// <returns>True if the PropertyChanged event has been raised,
//        /// false otherwise. The event is not raised if the old
//        /// value is equal to the new value.</returns>
//        [SuppressMessage(
//            "Microsoft.Design", 
//            "CA1045:DoNotPassTypesByReference",
//            MessageId = "1#",
//            Justification = "This syntax is more convenient than the alternatives.")]
//        protected bool Set<T>(
//            string propertyName,
//            ref T field,
//            T newValue)
//        {
//            if (EqualityComparer<T>.Default.Equals(field, newValue))
//            {
//                return false;
//            }

//#if !PORTABLE && !SL4
//            RaisePropertyChanging(propertyName);
//#endif
//            T old = field;      // mod:2018/6/21

//            field = newValue;

//            // ReSharper disable ExplicitCallerInfoArgument
//            RaisePropertyChanged(propertyName, newValue, old);
//            // ReSharper restore ExplicitCallerInfoArgument
            
//            return true;
//        }

//        // 带old mod:2018/6/21
//        public virtual void RaisePropertyChanged(string propertyName, object newval, object oldval) 
//        {
//            VerifyPropertyName(propertyName);

//            var handler = PropertyChanged;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangedEventArgsEx(propertyName, newval, oldval));
//            }
//        }

//#if CMNATTR
//        /// <summary>
//        /// Assigns a new value to the property. Then, raises the
//        /// PropertyChanged event if needed. 
//        /// </summary>
//        /// <typeparam name="T">The type of the property that
//        /// changed.</typeparam>
//        /// <param name="field">The field storing the property's value.</param>
//        /// <param name="newValue">The property's value after the change
//        /// occurred.</param>
//        /// <param name="propertyName">(optional) The name of the property that
//        /// changed.</param>
//        /// <returns>True if the PropertyChanged event has been raised,
//        /// false otherwise. The event is not raised if the old
//        /// value is equal to the new value.</returns>
//        protected bool Set<T>(
//            ref T field,
//            T newValue,
//            [CallerMemberName] string propertyName = null)
//        {
//            return Set(propertyName, ref field, newValue);
//        }
//#endif

//        #endregion

//#region IReadonlyTimeSeries
//        public int          TimeFrame { get { return Analyzer.TimeFrame; }}                               // 对应的TimeFrame，用int通用一些

//        public virtual EStreamType  Type { get { throw  new NotImplementedException();} }

//        public DateTime     From { get { return Analyzer.Time(First); } }
//        public DateTime     To { get { return Analyzer.Time(Count-1);} }


//        public virtual bool IsSuppurtVolume { get { return false; } }                        // 是否支持Volume 数据


//        public DateTime Time(int index)
//        {
//            return Analyzer.Time(index);
//        }

//        public int          Index(DateTime time)                            // 找不到返回-1
//        {
//            return Analyzer.Index(time);
//        }

//        public bool         AllowDisorder { get { return true; } }  // 允许乱序
//        public bool         DuplicateReplace { get { return true; } } // 重复置换，还是添加

//        IList<object> IReadonlyTimeSeries.GetRange2(DateTime from, DateTime to)
//        {
//            return GetRange2(from, to) as IList<object>;
//        }

//        public IList<T> GetRange2(DateTime from, DateTime to)
//        {
//            List<T> ret = new List<T>();

//            bool bin = false;
//            int n = 0;
//            foreach (T bd in this)
//            {
//                if (Time(n) == from)
//                    bin = true;

//                if(bin)
//                    ret.Add(bd);

//                if(Time(n) == to)
//                    break;

//                n++;
//            }

//            return ret;
//        }


//        public IReadonlyTimeSeries GetRange(DateTime from, DateTime to)
//        {
//            throw new NotImplementedException();
            
//        }

//        public bool         ContainRange(DateTime from, DateTime to) // 是否包含在timerange中
//        {
//            if (From <= from && To >= to)
//                return true;
//            else
//                return false;
//        }
//#endregion



//#region IStream
//        public ITsAnalyzer   Analyzer { get { return m_pAna; } }

//        public new virtual T this[int index]
//        {
//            get
//            {
//                if (index >= Count)
//                {
//                    int n = index - Count;
//                    for (int i = 0; i <= n; i++)
//                    {
//                        base.Add(default(T));
//                    }
//                }

//                return base[index];
//            }
//            set
//            {
//       //         System.Diagnostics.Debug.Assert(!ReadOnly);

//                if(index < Count)
//                    base[index] = value;
//                else if (index == Count)
//                    base.Add(value);
//                else
//                {
//                    int n = index - Count;
//                    for (int i = 0; i < n; i++)
//                    {
//                        base.Add(default(T));
//                    }

//                    base.Add(value);
//                }
//            }
//        }

//        object              IReadonlyObservableCollection.this[int index] { get { return this[index]; } }
//        object              IStream.this[int index] { get { return this[index]; }set { this[index] = (T)value; } }

////        public IInstance    Instance { get { return m_pInstance; } }

//        public int          First { get { return m_nFirst; } }              // 第一个有意义的下标
//        public int          Extent { get { return m_nExtent; } }

//        public virtual EStreamShapeType ShapeType { get { return m_eShapeType;}set{throw new NotFiniteNumberException();} }

//        public string       Label
//        {
//            get { return m_strLabel; }
//            set
//            {
//                if (RMath.Equal(Label, value))
//                    return;

//                m_strLabel = value;
//                RaisePropertyChanged("Label");

//                _Invalidate();
//            }

//        } // 自己特别设置的Label


//        public bool         IsVisible
//        {
//            get { return m_bIsVisible; }
//            set
//            {
//                if (RMath.Equal(IsVisible, value))
//                    return;

//                m_bIsVisible = value;
//                RaisePropertyChanged("IsVisible");

//                _Invalidate();
//            }
//        }
////        public string       Label { get { return m_strLabel; } set { m_strLabel = value; } }


//#region Colors
//        public RColor        Color
//        {
//            get
//            {
//                return m_cColor;
//            }
//            set
//            {
//                if (RMath.Equal(Color, value))
//                    return;

//                m_cColor = value;
//                RaisePropertyChanged("Color");

//                _Invalidate();
//            }
//        }

//        public RColor       GetColor(int index)
//        {
//            if (m_lColors.Count == 0)
//                return Color;
//            else
//            {
//                if (index >= Count)
//                {
//                    int n = index - Count;
//                    for (int i = 0; i <= n; i++)
//                    {
//                        m_lColors.Add(Color);
//                    }
//                }

//                return m_lColors[index];
//            }
//        }

//        public void         SetColor(int index, RColor color)
//        {
//            if (index < Count)
//                m_lColors[index] = color;
//            else if (index == Count)
//                m_lColors.Add(color);
//            else
//            {
//                int n = index - Count;
//                for (int i = 0; i < n; i++)
//                {
//                    m_lColors.Add(Color);
//                }

//                m_lColors.Add(color);
//            }

//        }

//        protected List<RColor> m_lColors = new List<RColor>();
//#endregion


//        public ELineStyle   LineStyle
//        {
//            get { return m_eLineStyle; }
//            set
//            {
//                if (RMath.Equal(LineStyle, value))
//                    return;

//                m_eLineStyle = value;
//                RaisePropertyChanged("LineStyle");
//                _Invalidate();
//            }
//        }

//        public int          LineWidth
//        {
//            get
//            {
//                return m_nLineWidth;
//            }
//            set
//            {
//                if (RMath.Equal(LineWidth, value))
//                    return;

//                m_nLineWidth = value;
//                RaisePropertyChanged("LineWidth");
//                _Invalidate();


//            }
//        }


//#region Levels
//        public IObservableCollection<SLevelInfo> Levels { get { return m_pLevels; }}

//        public void         AddLevel(float level, RColor color, ELineStyle style = ELineStyle.LINE_SOLID, int width = 1)
//        {
//            SLevelInfo li = new SLevelInfo(level, style, width, color);
//            m_pLevels.Add(li);
//        }

//        RObservableCollection<SLevelInfo> m_pLevels = new RObservableCollection<SLevelInfo>();
//#endregion

//        public virtual T    Min(int from, int to)                           // 获取最大
//        {
//            //return RMath.Min(this, from, to);
//            throw new NotImplementedException();
//        }

//        public virtual T    Max(int from, int to)                           // 获取最小
//        {
//            //return RMath.Max(this, from, to);
//            throw new NotImplementedException();
//        }

//        public virtual T    Avg(int from, int to)                           // 平均值
//        {
//            throw new NotImplementedException();
//        }

//        public virtual T    Sum(int from, int to)                           // 总共
//        {
//            throw new NotImplementedException();
//        }

//        object              IReadonlyStream.Min(int from, int to) // 获取最大
//        {
//            throw new NotImplementedException();
//        }

//        object              IReadonlyStream.Max(int from, int to) // 获取最小
//        {
//            throw new NotImplementedException();
//        }

//        object              IReadonlyStream.Avg(int from, int to) // 平均值
//        {
//            throw new NotImplementedException();
//        }
//        object              IReadonlyStream.Sum(int from, int to)                          // 总共
//        {
//            throw new NotImplementedException();
//        }

//        public virtual void _Invalidate() {}
//        #endregion

//        //#region IStream<T>
//        //        public new virtual T this[int index]
//        //        {
//        //            get { return base[index]; }
//        //            set
//        //            {
//        //                System.Diagnostics.Debug.Assert(!ReadOnly);

//        //                if(index < Count)
//        //                    base[index] = value;
//        //                else if (index == Count)
//        //                    base.Add(value);
//        //                else
//        //                {
//        //                    int n = index - Count;
//        //                    for (int i = 0; i < n; i++)
//        //                    {
//        //                        base.Add(default(T));
//        //                    }

//        //                    base.Add(value);
//        //                }
//        //            }
//        //        }
//        //        public virtual T    Min(int from, int to)
//        //        {
//        //            Debug.Assert( from <= to &&
//        //                from>=0 && from <Count &&
//        //                to >= 0 && to < Count);

//        //            T min = this[from];
//        //            for (int i = from; i <= to; ++i)
//        //            {
//        //                if(this[i].CompareTo(min) < 0)
//        //                    min = this[i];
//        //            }

//        //            return min;
//        //        }
//        //        public virtual T    Max(int from, int to)
//        //        {
//        //            Debug.Assert( from <= to &&
//        //                from>=0 && from <Count &&
//        //                to >= 0 && to < Count);

//        //            T max = this[from];
//        //            for (int i = from; i <= to; ++i)
//        //            {
//        //                if(this[i].CompareTo(max) > 0)
//        //                    max = this[i];
//        //            }

//        //            return max;
//        //        }
//        //        public virtual T    Avg(int from, int to)
//        //        {
//        //            Debug.Assert( from <= to &&
//        //                from>=0 && from <Count &&
//        //                to >= 0 && to < Count);

//        //            throw new NotImplementedException();

//        //        }

//        //#region Internal
//        //        public virtual void _Set(int index, T val)
//        //        {
//        //            Debug.Assert(index >= 0);

//        //            if (index >= 0 &&
//        //                index < Count)
//        //                base[index] = val;
//        //            else
//        //            {
//        //                int n = index - Count;
//        //                for (int i = 0; i != n; ++i)
//        //                    base.Add(default(T));  // 插入代表不是数字的值

//        //                base.Add(val);
//        //            }
//        //        }
//        //#endregion
//        //#endregion

//        #region C&D
//        public Stream(ITsAnalyzer ana, string name,
//            int first = 0,
//            EStreamShapeType shape = EStreamShapeType.Line, int extent = 0, bool visible = true, RColor c = new RColor(), ELineStyle style = ELineStyle.LINE_SOLID, int width = 1)
//        {
//            m_pAna = ana;
//            m_nFirst = first;

//            m_strID = name;

//            m_bIsVisible = visible;
//            m_cColor = c;
//            m_eLineStyle = style;
//            m_nLineWidth = width;
//            m_eShapeType = shape;

//            m_nExtent = extent;
//            if (m_nExtent > 0)
//            {
                
//            }
//        }
//#endregion

//#region Members
//        protected string    m_strID;

//        protected int       m_nFirst;
//        protected int       m_nExtent;

//        protected ITsAnalyzer m_pAna;
////        protected bool      m_bReadOnly;

//        protected bool      m_bIsVisible;
//        protected string    m_strLabel;

//        protected RColor     m_cColor;
//        protected ELineStyle m_eLineStyle;
//        protected int       m_nLineWidth;
//        protected EStreamShapeType m_eShapeType;

//        #endregion
//    }

//    public class FloatStream : Stream<float>
//    {
//        public override EStreamType  Type { get { return EStreamType.FloatStream;} }

//        public override float Min(int from, int to) // 获取最大
//        {
//            return RMath.Min(this, from, to);
//        }

//        public override float Max(int from, int to) // 获取最小
//        {
//            return RMath.Max(this, from, to);
//        }
//        public override float Avg(int from, int to)                           // 平均值
//        {
//            return RMath.Avg(this, from, to);
//        }

//        public override float Sum(int from, int to)                           // 总共
//        {
//            return RMath.Sum(this, from, to);
//        }

//#region C&D
//        public              FloatStream(ITsAnalyzer ana, string name,
//            int first = 0,
//            EStreamShapeType shape = EStreamShapeType.Line, int extent = 0, bool visible = true, RColor c = new RColor(), ELineStyle style = ELineStyle.LINE_SOLID, int width = 1)
//            :base(ana, name,
//                 first, shape, extent, visible, c, style, width
//            )
//        {
//        }
//#endregion
//    }

//    public class DoubleStream : Stream<double>
//    {
//        public override EStreamType  Type { get { return EStreamType.DoubleStream;} }

//        public override double Avg(int from, int to)                           // 平均值
//        {
//            return RMath.Avg(this, from, to);
//        }

//        public override double Sum(int from, int to)                           // 总共
//        {
//            return RMath.Sum(this, from, to);
//        }

//#region C&D
//        public              DoubleStream(ITsAnalyzer ana, string name,
//            int first = 0,
//            EStreamShapeType shape = EStreamShapeType.Line, int extent = 0, bool visible = true, RColor c = new RColor(), ELineStyle style = ELineStyle.LINE_SOLID, int width = 1)
//            :base(ana, name,
//                 first,
//            shape, extent, visible, c, style, width
//            )
//        {
//        }
//#endregion
//    }
//}
