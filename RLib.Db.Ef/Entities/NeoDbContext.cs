/********************************************************************
    created:	2019/11/20 18:07:13
    author:		rush
    email:		
	
    purpose:	
*********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.TypeConversion;
using DataModel;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using RLib.Base;
using Enum = System.Enum;
using Type = Google.Protobuf.WellKnownTypes.Type;

namespace RLib.Db.Ef
{
    public abstract class DbContextBase: DbContext
    {
        //public NeoDbContext(DbContextOptions options)
        //    :base(options)
        //{

        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //         //				 => options.UseSqlite("Data Source=test.db");
        //         => options.UseSqlite("Data Source=NeoDb.db");


        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        return base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //            .SelectMany(x => x.ValidationErrors)
        //            .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //    }
        //}

        //public override Task<int> SaveChangesAsync()
        //{
        //    try
        //    {
        //        return base.SaveChangesAsync();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //            .SelectMany(x => x.ValidationErrors)
        //            .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //    }
        //}



        public void         Sync<T>(IEnumerable<T> entities, Expression<Func<T, bool>> samplePredicate = null) where T : class       // 让数据库同entitys完全一致
        {
            if (samplePredicate == null)
                samplePredicate = t => true; 

            // 去除不再的
            var dels = Set<T>().Where(samplePredicate).Where(p => !entities.Any(n => n.GetProperty("Id") == p.GetProperty("Id"))).ToList();
            Set<T>().RemoveRange(dels);

            // 更新现有的活添加
            AddOrUpdateRange(entities);
        }


        public void         AddOrUpdate<T>(T entity) where T : class
        {
            AddOrUpdateRange(new[] {entity});
        }

        public void         AddOrUpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                var id = entity.GetProperty("Id") as object[];

                // null resolution operator casts to object, so use ternary
                var tracked = (id != null)
                    ? Set<T>().Find(id)
                    : Set<T>().Find(entity.GetProperty("Id"));

                if (tracked != null)
                {
                    // perform shallow copy
                    Entry(tracked).CurrentValues.SetValues(entity);
                }
                else
                {
                    Entry(entity).State = EntityState.Added;
                }
            }
            //return SaveChanges();
        }




   /// <inheritdoc />
        public void RollBack()
        {
            foreach (var entry in base.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        // Note - problem with deleted entities:
                        // When an entity is deleted its relationships to other entities are severed. 
                        // This includes setting FKs to null for nullable FKs or marking the FKs as conceptually null (don’t ask!) 
                        // if the FK property is not nullable. You’ll need to reset the FK property values to 
                        // the values that they had previously in order to re-form the relationships. 
                        // This may include FK properties in other entities for relationships where the 
                        // deleted entity is the principal of the relationship–e.g. has the PK 
                        // rather than the FK. I know this is a pain–it would be great if it could be made easier in the future, but for now it is what it is.
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

      

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Ignore<Timestamp>();
        //    modelBuilder.Ignore<DaySessionDm>();
        //    modelBuilder.Ignore<TradingSessionDm>();

        //    DefaultModelCreating<ExchangeDM>(modelBuilder);
        //    DefaultModelCreating<SymbolDM>(modelBuilder);
        //    DefaultModelCreating<NeoSettings>(modelBuilder);
        //    modelBuilder.Entity<NeoSettings>().Property(m => m.Value).HasJsonConversion();

        //    DefaultModelCreating<MarketTimeDm>(modelBuilder);
        //    modelBuilder.Entity<MarketTimeDm>().Property(m => m.DaySessions).HasField("daySessions_").HasJsonConversion();

        //    DefaultModelCreating<FutureContractDM>(modelBuilder);
        //    modelBuilder.Entity<FutureContractDM>().Property(m => m.SettlementMonths).HasJsonConversion(); 


        //    //// http://stackoverflow.com/questions/7924758/entity-framework-creates-a-plural-table-name-but-the-view-expects-a-singular-ta
        //    //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        //    //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
        //    //    .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
        //    //    .Where(type => type.BaseType != null && type.BaseType.IsGenericType
        //    //                   && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

        //    //foreach (var type in typesToRegister)
        //    //{
        //    //    dynamic configurationInstance = Activator.CreateInstance(type);
        //    //    modelBuilder.Configurations.Add(configurationInstance);
        //    //}
        //}

        protected void DefaultModelCreating<T>(ModelBuilder modelBuilder) where T : class
        {
            var timestampConverter = new ValueConverter<Timestamp, DateTime>(
                    v => v.ToDateTime(),
                    v => v.ToLocalTime().ToUniversalTime().ToTimestamp());

            var doubleConverter = new ValueConverter<double, double>(
                    v => v.ToDbDouble(),
                    v => v.FromDbDouble());

            // 无法使用obj coverter
            //var objConverter = new ValueConverter<object, string>(
            //        v => v.ToJsonNoLoop(),
            //        v => v.ToJsonObj(v.GetType()));


            System.Type t = typeof(T);

            foreach (PropertyInfo info in t.GetProperties())
            {
                if (info.PropertyType.IsList())
                {
                    //todo: 做一个通用的converter
                    //modelBuilder.Entity<KVTestDM2>().Property(m => m.Kvs).HasField("kvs_")
                    //       .HasConversion(v => v.ToJson(), v => (RepeatedField<global::EfTest.KVDM>)v.ToJsonObj(v.GetType()));

                    if (info.PropertyType.Is<RepeatedField<KVDM>>())        // protobuf repeated field
                    {// 
                        //string filed = info.Name + "_";
                        //FieldInfo f = t.GetFields().FirstOrDefault(p => p.Name.Equals(filed, StringComparison.InvariantCultureIgnoreCase));

                        string filed = info.Name + "_";
                        filed = char.ToLower(filed[0]) + filed.Substring(1);

                        modelBuilder.Entity<T>().Property<RepeatedField<KVDM>>(info.Name).HasField(filed).HasJsonConversion<RepeatedField<KVDM>>();
                    }
                    else if (info.PropertyType.Is<RepeatedField<int>>())        // protobuf repeated field
                    {// 
                        string filed = info.Name + "_";
                        filed = char.ToLower(filed[0]) + filed.Substring(1);
                        //var v = t.GetFields().ToList();
                        //FieldInfo f = t.GetFields().FirstOrDefault(p => p.Name.Equals(filed, StringComparison.InvariantCultureIgnoreCase));

                        modelBuilder.Entity<T>().Property<RepeatedField<int>>(info.Name).HasField(filed).HasJsonConversion<RepeatedField<int>>();
                    }
                }
                else
                {
                    // 同时有setget
                    if (info.GetGetMethod() == null || info.GetSetMethod() == null)
                        continue;

                    if (info.PropertyType.IsEnum)
                    {
                        modelBuilder.Entity<T>().Property(info.PropertyType, info.Name).HasConversion<string>();
                    }
                    else if (info.PropertyType.IsDouble())
                    {
                        modelBuilder.Entity<T>().Property(info.PropertyType, info.Name).HasConversion(doubleConverter);
                    }
                    else if (info.PropertyType.Is<Google.Protobuf.WellKnownTypes.Timestamp>())
                    {
                        modelBuilder.Entity<T>().Property(info.PropertyType, info.Name).HasConversion(timestampConverter);
                    }
                }
            }
        }
    }


    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
        {
            ValueConverter<T, string> converter = new ValueConverter<T, string>
            (
                v => JsonEx.ToJson(v),
                v => JsonEx.ToJsonObj<T>(v) ?? new T()
            );

            ValueComparer<T> comparer = new ValueComparer<T>
            (
                (l, r) => JsonEx.ToJson(l) == JsonEx.ToJson(r),
                v => v == null ? 0 : JsonEx.ToJson(v).GetHashCode(),
                v => JsonEx.ToJsonObj<T>(JsonEx.ToJson(v))
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("jsonb");

            return propertyBuilder;
        }
    }


}
