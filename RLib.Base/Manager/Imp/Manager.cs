///********************************************************************
//    created:	2017/5/21 20:26:20
//    author:		rush
//    email:		
	
//    purpose:	
//*********************************************************************/
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Diagnostics.CodeAnalysis;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;


//namespace RLib.Base
//{

//    public class Manager<TID, T>:Dictionary<TID, T>, IManager<TID, T> where T:IProduct<TID>
//    {
//#region IGottaInit
//#region properties
//        public EInitState   InitStat { get { return _InitStat; } set { Set("InitStat", ref _InitStat, value); } }                         // init 状态
//#endregion

//#region virtuals
//	    public virtual void Init()
//	    {
//		    InitStat = EInitState.Initing;

//			// 生成Dynamic Factory
//            // 找程序下的所有IFeeder
//            // todo: 以后决定从哪些assembly 加载
//            List<Type> lt = new List<Type>();
//            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            foreach (Assembly a in assemblies)
//            {
//                lt.AddRange(a.GetTypes());
//            }

//            foreach (Type type in lt)
//            {
//                if (type.IsClass &&
//                    !type.IsAbstract &&
//                    typeof(T).IsAssignableFrom(type)
//                    )
//                {
//                    m_pFactories.Add(type.FullName, new DynamicFactory(this, type));
//                }
//            }

////            Factorires.Add(new ExcelFeederFactory(this));

//            ReadConfig();

//			OnIniting();

//		    InitStat = EInitState.Inited;
//	    }

//	    public virtual void UnInit()
//	    {
//		    InitStat = EInitState.Uniniting;

//			OnUnIniting();

//            WriteConfig();

//		    InitStat = EInitState.Uninited;
//	    }

//	    public virtual void	OnIniting(){}
//	    public virtual void	OnUnIniting(){}
//#endregion

//#region Members
//	    protected EInitState _InitStat;
//#endregion

//#endregion


//#region Observable oberservableObject

//        /// <summary>
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
//            return;
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

//        // 带old mod:2018/9/17
//        public virtual void RaisePropertyChanging(string propertyName, object newval, object oldval) 
//        {
//            VerifyPropertyName(propertyName);

//            var handler = PropertyChanging;
//            if (handler != null)
//            {
//                handler(this, new PropertyChangingEventArgsEx(propertyName, newval, oldval));
//            }
//        }


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


//#endregion



//#region Configured instance
//        public string       ConfigureFile { get { return m_strConfigureFile; } }
//        public IObservableDictionary<string, ConfiguredInstance> Configured { get { return m_pConfigured; } }     // 已经配置好的instance

//        public void         ReadConfig()
//        {
//            if (!File.Exists(ConfigureFile))
//                return;

//            XmlDocument doc = new XmlDocument();
//            doc.Load(ConfigureFile);

//            XmlNodeList connections = doc.SelectNodes("/Root/Configured/Ins");
//            if (connections != null)
//            {
//                foreach (XmlNode con in connections)
//                {
//                    string name = con.Attributes["name"].Value;
//                    string fac = con.Attributes["Factory"].Value;

//                    // 如果没有该fac，continue
//                    IFactory v = null;
//                    Factories.TryGetValue(fac, out v);
//                    if(v == null)
//                        continue;

//                    ConfiguredInstance fc = new ConfiguredInstance(name, fac, v.ProtoParams.Clone);

//                    foreach (XmlNode node  in con.ChildNodes)
//                    {
//                        IParam p = Param.FromXmlNode(node);

//                        fc.Params.SetValue(p.Name, p.Value);
//                    }

//                    m_pConfigured.Add(fc.ID, fc);
//                }
//            }
//        }
//        public void         WriteConfig()
//        {
//            XmlDocument doc = new XmlDocument();
//            XmlNode root = doc.CreateElement("Root");
//            doc.AppendChild(root);

//            XmlNode configured = doc.CreateElement("Configured");
//            root.AppendChild(configured);


//            // 填充
//            foreach (KeyValuePair<string, ConfiguredInstance> kv in Configured)
//            {
//                ConfiguredInstance i = kv.Value;

//                XmlNode node = doc.CreateElement("Ins"); // 子
//                configured.AppendChild(node);

//                XmlAttribute name = doc.CreateAttribute("name");
//                name.Value = i.ID;
//                node.Attributes.Append(name);

//                XmlAttribute fac = doc.CreateAttribute("Factory");
//                fac.Value = i.Facotry;
//                node.Attributes.Append(fac);

//                /// params
//                foreach (IParam p in i.Params.ToList)
//                {
//                    XmlNode param =p.ToXmlNode(doc);
//                    node.AppendChild(param);
//                }
//            }

//            doc.Save(ConfigureFile);
//        }

//        protected RObservableDictionary<string, ConfiguredInstance> m_pConfigured = new RObservableDictionary<string, ConfiguredInstance>();
//#endregion


//#region IManager<T>
//        public IDictionary<string, IFactory> Factories { get { return m_pFactories; } }                // facotry 


//        public T Instantiate(string factory, params object[] args) // 是否对其进行init
//        {
//            return Instantiate(factory, false, args);
//        }


//        public T            Instantiate(string factory, bool init = true, params object[] args)
//        {
//            // todo: 这里需要进一步明确定义
//            T ret = default(T);
//            try
//            {

//                IFactory f = null;
//                Factories.TryGetValue(factory, out f);

//                if (f == null)
//                    return ret;

//                if (f is IDynamicFactory)
//                {
//                    ret = (T)f.Create(args);
//                    ret._SetFactory(f);
//                    ret._SetManager(this);
//                    foreach (var v in args)  // 设定params
//                    {
//                        if (v is IParams)
//                            (ret as IDynamicProduct<TID>)._SetParams(v as IParams);
//                    }
//                    if (init)
//                        ret.Init();
//                }
//                else if (f is IAttrFactory)
//                {
//                    ret = (T)f.Create(args);
//                    ret._SetFactory(f);
//                    ret._SetManager(this);
//                    if (init)
//                        ret.Init();
//                }
//                else  // 静态factory
//                {
//                    IFactory<TID, T> fac = f as IFactory<TID, T>;

//                    List<Object> ps = null;
//                    if (args != null)
//                        ps = new List<object>(args);
//                    else
//                        ps = new List<object>();

//                    ps.Insert(0, this);
//                    ps.Insert(0, fac);

//                    ret = (T)fac.Create(ps.ToArray());
//                    ret._SetFactory(f);
//                    ret._SetManager(this);

//                    if (ret != null && init)
//                        ret.Init();
//                }

//            }
//            catch (Exception e)
//            {
//                RLibBase.Logger.Error(e);
//            }

//            return ret;
//        }

//        public T            InstantiateAndAdd(string factory, bool init, params object[] args) // 是否对其进行init
//        {

//            T ret = Instantiate(factory, false, args);
//	        if (ret != null)
//	        {
//                Add(ret.ID, ret);
		        
//				if(init)
//					ret.Init();
//	        }

//            return ret;
            
//        }

//        public T            InstantiateAndAdd(string factory, params object[] args)
//        {
//            T ret = Instantiate(factory, false,args);
//	        if (ret != null)
//	        {
//                Add(ret.ID, ret);
//				ret.Init();
//	        }

//            return ret;
//        }

//        public void         UnInInstantiate(T val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);
//            val.UnInit();
//        }


//        public void         UnInInstantiateAndRemove(T val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);
//            val.UnInit();

//            Remove(val.ID);
//        }

//        public void         UnInInstantiateAndRemove(TID val)
//        {
//            T obj = default(T);
//            TryGetValue(val, out obj);
//            if(obj != null)
//                UnInInstantiateAndRemove(obj);
//        }

//        public void         UnInInstantiateAndRemoveAll()
//        {
//            var vals = Values;

//            foreach (var v in Values)
//            {
//                UnInInstantiateAndRemove(v);
//            }
//        }
//#endregion

//#region C&D
//        public              Manager(string configureFile)
//        {
//            m_strConfigureFile = configureFile;
//        }
//        public              Manager()
//        {}
//#endregion

//#region Members
//        protected Dictionary<string, IFactory> m_pFactories  = new Dictionary<string, IFactory>();
//        protected string    m_strConfigureFile;
//#endregion
//    }


//    public class ObservableManager<TID, T>:RObservableDictionary<TID, T>, IObservableManager<TID, T> where T:IProduct<TID>
//    {
//#region IGottaInit
//#region properties
//        public EInitState   InitStat { get { return _InitStat; } set { Set("InitStat", ref _InitStat, value); } }                         // init 状态
//#endregion

//#region virtuals
//	    public virtual void Init()
//	    {
//		    InitStat = EInitState.Initing;

//			// 生成Dynamic Factory
//            // 找程序下的所有IFeeder
//            // todo: 以后决定从哪些assembly 加载
//            List<Type> lt = new List<Type>();
//            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            foreach (Assembly a in assemblies)
//            {
//                lt.AddRange(a.GetTypes());
//            }

//            foreach (Type type in lt)
//            {
//                if (type.IsClass &&
//                    !type.IsAbstract &&
//                    typeof(T).IsAssignableFrom(type)
//                    )
//                {
//                    m_pFactories.Add(type.FullName, new DynamicFactory(this, type));
//                }
//            }

////            Factorires.Add(new ExcelFeederFactory(this));

//            ReadConfig();

//			OnIniting();

//		    InitStat = EInitState.Inited;
//	    }

//	    public virtual void UnInit()
//	    {
//		    InitStat = EInitState.Uniniting;

//			OnUnIniting();

//            WriteConfig();

//		    InitStat = EInitState.Uninited;
//	    }

//	    public virtual void	OnIniting(){}
//	    public virtual void	OnUnIniting(){}
//#endregion

//#region Members
//	    protected EInitState _InitStat;
//#endregion

//#endregion


//#region Observable oberservableObject

//        /// <summary>
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
        

//#endregion



//#region Configured instance
//        public string       ConfigureFile { get { return m_strConfigureFile; } }
//        public IObservableDictionary<string, ConfiguredInstance> Configured { get { return m_pConfigured; } }     // 已经配置好的instance

//        public void         ReadConfig()
//        {
//            if (!File.Exists(ConfigureFile))
//                return;

//            XmlDocument doc = new XmlDocument();
//            doc.Load(ConfigureFile);

//            XmlNodeList connections = doc.SelectNodes("/Root/Configured/Ins");
//            if (connections != null)
//            {
//                foreach (XmlNode con in connections)
//                {
//                    string name = con.Attributes["name"].Value;
//                    string fac = con.Attributes["Factory"].Value;

//                    // 如果没有该fac，continue
//                    IFactory v = null;
//                    Factories.TryGetValue(fac, out v);
//                    if(v == null)
//                        continue;

//                    ConfiguredInstance fc = new ConfiguredInstance(name, fac, v.ProtoParams.Clone);

//                    foreach (XmlNode node  in con.ChildNodes)
//                    {
//                        IParam p = Param.FromXmlNode(node);

//                        fc.Params.SetValue(p.Name, p.Value);
//                    }

//                    m_pConfigured.Add(fc.ID, fc);
//                }
//            }
//        }
//        public void         WriteConfig()
//        {
//            if (string.IsNullOrWhiteSpace(ConfigureFile))
//                return;

//            XmlDocument doc = new XmlDocument();
//            XmlNode root = doc.CreateElement("Root");
//            doc.AppendChild(root);

//            XmlNode configured = doc.CreateElement("Configured");
//            root.AppendChild(configured);


//            // 填充
//            foreach (KeyValuePair<string, ConfiguredInstance> kv in Configured)
//            {
//                ConfiguredInstance i = kv.Value;

//                XmlNode node = doc.CreateElement("Ins"); // 子
//                configured.AppendChild(node);

//                XmlAttribute name = doc.CreateAttribute("name");
//                name.Value = i.ID;
//                node.Attributes.Append(name);

//                XmlAttribute fac = doc.CreateAttribute("Factory");
//                fac.Value = i.Facotry;
//                node.Attributes.Append(fac);

//                /// params
//                foreach (IParam p in i.Params.ToList)
//                {
//                    XmlNode param =p.ToXmlNode(doc);
//                    node.AppendChild(param);
//                }
//            }

//            doc.Save(ConfigureFile);
//        }

//        protected RObservableDictionary<string, ConfiguredInstance> m_pConfigured = new RObservableDictionary<string, ConfiguredInstance>();
//#endregion


//#region IManager<T>
//        public IObservableDictionary<string, IFactory> Factories { get { return m_pFactories; } }                // facotry 

//        public T Instantiate(string factory,  params object[] args)
//        {
//            return Instantiate(factory, false, args);
//        }
//        public T            Instantiate(string factory, bool init = true, params object[] args)
//        {
//            // todo: 这里需要进一步明确定义

//            T ret = default(T);
//            try
//            {
//                IFactory f = null;
//                Factories.TryGetValue(factory, out f);

//                if (f == null)
//                    return ret;

//                if (f is IDynamicFactory)
//                {
//                    ret = (T)f.Create(args);
//                    ret._SetFactory(f);
//                    ret._SetManager(this);
//                    foreach (var v in args)  // 设定params
//                    {
//                        if (v is IParams)
//                            (ret as IDynamicProduct<TID>)._SetParams(v as IParams);
//                    }

                  

//                    if (init)
//                        ret.Init();
//                }
//                else if (f is IAttrFactory)
//                {
//                    ret = (T)f.Create(args);
//                    ret._SetFactory(f);
//                    ret._SetManager(this);
//                    if (init)
//                        ret.Init();
//                }
//                else  // 静态factory
//                {
//                    IFactory<TID, T> fac = f as IFactory<TID, T>;

//                    List<Object> ps = null;
//                    if (args != null)
//                        ps = new List<object>(args);
//                    else
//                        ps = new List<object>();

//                    ps.Insert(0, this);
//                    ps.Insert(0, fac);

//                    ret = (T)fac.Create(ps.ToArray());
//                    ret._SetFactory(f);
//                    ret._SetManager(this);

//                    if (ret != null && init)
//                        ret.Init();
//                }
//            }
//            catch (Exception e)
//            {
//                RLibBase.Logger.Error(e);
//            }

//            return ret;
//        }

//        public T            InstantiateAndAdd(string factory, bool init, params object[] args) // 是否对其进行init
//        {

//            T ret = Instantiate(factory, false, args);
//	        if (ret != null)
//	        {
//                Add(ret.ID, ret);
		        
//				if(init)
//					ret.Init();
//	        }

//            return ret;
            
//        }

//        public T            InstantiateAndAdd(string factory, params object[] args)
//        {
//            T ret = Instantiate(factory, false,args);
//	        if (ret != null)
//	        {
//                Add(ret.ID, ret);
//				ret.Init();
//	        }

//            return ret;
//        }

//        public void         UnInInstantiate(T val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);
//            val.UnInit();
//        }


//        public void         UnInInstantiateAndRemove(T val)
//        {
//            System.Diagnostics.Debug.Assert(val != null);
//            val.UnInit();

//            Remove(val.ID);
//        }

//        public void         UnInInstantiateAndRemove(TID val)
//        {
//            T obj = default(T);
//            TryGetValue(val, out obj);
//            if(obj != null)
//                UnInInstantiateAndRemove(obj);
//        }

//        public void         UnInInstantiateAndRemoveAll()
//        {
//            var vals = Values;

//            foreach (var v in Values)
//            {
//                UnInInstantiateAndRemove(v);
//            }
//        }
//#endregion

//#region C&D
//        public              ObservableManager(string configureFile, bool haveObservableCollection = false)
//            :base(haveObservableCollection)
//        {
//            m_strConfigureFile = configureFile;
//        }
//        public              ObservableManager(bool haveObservableCollection = false)
//            :base(haveObservableCollection)
//        {}

//        public              ObservableManager()
//            :base(false)
//        {}
//#endregion

//#region Members
//        protected RObservableDictionary<string, IFactory> m_pFactories  = new RObservableDictionary<string, IFactory>();
//        protected string    m_strConfigureFile;
//#endregion
//    }




////    public class Manager<T, TID>:Group<T, TID>, IManager<T, TID> where T:IProduct<TID>
////    {
////#region IGottaInit
////#region properties
////        public EInitState   InitState { get { return m_eInitState; } }                         // init 状态
////#endregion

////#region Manipulators
////        public virtual void _SetInitState(EInitState state)
////        {
////            System.Diagnostics.Debug.Assert(state > InitState); // 确保没有回头路

////            EInitState before = InitState;
////            m_eInitState = state;

////            switch (state)
////            {
////                case EInitState.Initing:
////                    OnIniting();
////                    break;
////                case EInitState.Inited:
////                    OnInited();
////                    break;
////                case EInitState.Uniniting:
////                    OnUnIniting();
////                    break;
////                case EInitState.Uninited:
////                    OnUnInited();
////                    break;
////            }

////            __FireInitStateChanged(before, state);
////        }

////#endregion

////#region virtuals
////        public virtual void Init()
////        {
////            _SetInitState(EInitState.Initing);

////            // 生成Dynamic Factory
////            // 找程序下的所有IFeeder
////            // todo: 以后决定从哪些assembly 加载
////            List<Type> lt = new List<Type>();
////            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
////            foreach (Assembly a in assemblies)
////            {
////                lt.AddRange(a.GetTypes());
////            }

////            foreach (Type type in lt)
////            {
////                if (type.IsClass &&
////                    !type.IsAbstract &&
////                    typeof(T).IsAssignableFrom(type)
////                    )
////                {
////                    Factories.Add(new DynamicFactory(this, type));
////                }
////            }

//////            Factorires.Add(new ExcelFeederFactory(this));

////            ReadConfig();
            

////            _SetInitState(EInitState.Inited);
////        }
////        public virtual void UnInit()
////        {
////            _SetInitState(EInitState.Uniniting);

////            WriteConfig();

////            _SetInitState(EInitState.Uninited);
////        }

////        public virtual void OnIniting(){}                                   // 初始化时调用
////        public virtual void OnInited() {}                                   // 初始化完成时调用
////        public virtual void OnUnIniting() {}                                // 反始化开始时调用
////        public virtual void OnUnInited() {}                                 // 反始化完成时调用
////#endregion

////#region events
////        public event EventHandler<ChangedEventArgs<EInitState>> OnInitStateChangedEvent;

////        protected void      __FireInitStateChanged(EInitState before, EInitState after)
////        {
////            if (OnInitStateChangedEvent != null)
////            {
////                var eve = OnInitStateChangedEvent;
////                eve(this, new ChangedEventArgs<EInitState>(before, after));
////            }
////        }
////#endregion

////#region Members
////        protected EInitState m_eInitState = EInitState.NotInited;
////#endregion
////#endregion

////#region IManager<T>
////        public IGroup<IFactory, string> Factories { get { return m_pFactories; } }                             // facotry 

////#region Configured instance
////        public string       ConfigureFile { get { return m_strConfigureFile; } }
////        public IGroup<ConfiguredInstance, string>  Configured { get { return m_pConfigured; } }     // 已经配置好的instance

////        public void         ReadConfig()
////        {
////            if (!File.Exists(ConfigureFile))
////                return;

////            XmlDocument doc = new XmlDocument();
////            doc.Load(ConfigureFile);

////            XmlNodeList connections = doc.SelectNodes("/Root/Configured/Ins");
////            if (connections != null)
////            {
////                foreach (XmlNode con in connections)
////                {
////                    string name = con.Attributes["name"].Value;
////                    string fac = con.Attributes["Factory"].Value;

////                    // 如果没有该fac，continue
////                    var v = Factories.Get(fac);
////                    if(v == null)
////                        continue;

////                    ConfiguredInstance fc = new ConfiguredInstance(name, fac, v.ProtoParams);

////                    foreach (XmlNode node  in con.ChildNodes)
////                    {
////                        IParam p = Param.FromXmlNode(node);

////                        fc.Params.SetValue(p.Name, p.Value);
////                    }

////                    m_pConfigured.Add(fc);
////                }
////            }
////        }
////        public void         WriteConfig()
////        {
////            XmlDocument doc = new XmlDocument();
////            XmlNode root = doc.CreateElement("Root");
////            doc.AppendChild(root);

////            XmlNode configured = doc.CreateElement("Configured");
////            root.AppendChild(configured);

////            // 填充
////            foreach (ConfiguredInstance i in m_pConfigured.Items)
////            {
////                XmlNode node = doc.CreateElement("Ins"); // 子
////                configured.AppendChild(node);

////                XmlAttribute name = doc.CreateAttribute("name");
////                name.Value = i.ID;
////                node.Attributes.Append(name);

////                XmlAttribute fac = doc.CreateAttribute("Factory");
////                fac.Value = i.Facotry;
////                node.Attributes.Append(fac);

////                /// params
////                foreach (IParam p in i.Params.ToList)
////                {
////                    XmlNode param =p.ToXmlNode(doc);
////                    node.AppendChild(param);
////                }
////            }

////            doc.Save(ConfigureFile);
////        }

////        protected Group<ConfiguredInstance, string> m_pConfigured = new Group<ConfiguredInstance, string>();
////#endregion

////        public T            Instantiate(string factory, params object[] args)
////        {
////            T ret = default(T);

////            IFactory f = Factories.Get(factory);
////            if (f is IDynamicFactory)
////            {
////                ret = (T)f.Create(args);
////                ret._SetFactory(f);
////                ret._SetManager(this);
////                foreach (var v in args)  // 设定params
////                {
////                    if(v is IParams)
////                        ret._SetParams(v as IParams);
////                }
////                ret.Init();
////            }
////            else  // 静态factory
////            {
////                IFactory<T, TID> fac = f as IFactory<T, TID>;
////                if (fac == null)
////                    return default(T);

////                List<Object> ps = null;
////                if (args != null)
////                    ps = new List<object>(args);
////                else
////                    ps = new List<object>();

////                ps.Insert(0, this);
////                ps.Insert(0, fac);

////                ret = (T)fac.Create(ps.ToArray());
////                if (ret != null)
////                    ret.Init();
////            }

////            return ret;
////        }

////        public T            InstantiateAndAdd(string factory, params object[] args)
////        {
////            T ret = Instantiate(factory, args);
////            Add(ret);

////            return ret;
////        }

////        public void         UnInInstantiate(T val)
////        {
////            System.Diagnostics.Debug.Assert(val != null);
////            val.UnInit();
////        }


////        public void         UnInInstantiateAndRemove(T val)
////        {
////            System.Diagnostics.Debug.Assert(val != null);
////            val.UnInit();

////            Remove(val);
////        }

////        public void         UnInInstantiateAndRemove(TID val)
////        {
////            UnInInstantiateAndRemove(Get(val));
////        }

////        public void         UnInInstantiateAndRemoveAll()
////        {
////            var keys = m_dGroupItems.Keys;
////            foreach (var k in keys)
////            {
////                UnInInstantiateAndRemove(Get(k));
////            }
////        }
////#endregion

////#region C&D
////        public              Manager(string configureFile)
////        {
////            m_strConfigureFile = configureFile;
////        }
////        public              Manager()
////        {}
////#endregion

////#region Members
////        protected Group<IFactory, string> m_pFactories = new Group<IFactory, string>();
////        protected string    m_strConfigureFile;
////#endregion
////    }
//}
