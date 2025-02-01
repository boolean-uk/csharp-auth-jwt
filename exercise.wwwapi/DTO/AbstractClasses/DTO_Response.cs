using System.Diagnostics;
using api_cinema_challenge.Models;
using api_cinema_challenge.Models.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Models;
using exercise.wwwapi.Payload;
using Microsoft.EntityFrameworkCore.Metadata;

namespace api_cinema_challenge.DTO.Interfaces
{
    public interface IDTO_Defines_Include<Model_Type>
        where Model_Type : class, ICustomModel
    {
        public static abstract Func<IQueryable<Model_Type>, IQueryable<Model_Type>> _includeData();
    }
    /// <summary>
    /// The abstract DTO_Response class contains many generic functions to turn a model into a DTO, and package into a payload...
    /// </summary>
    public abstract class DTO_Response<DTO_Type, Model_Type> 
        where DTO_Type : DTO_Response<DTO_Type, Model_Type>, new()
        where Model_Type : class, ICustomModel
    {
        public DTO_Response() { }
        protected abstract void _Initialize(Model_Type model);

        public void Initialize(Model_Type model)
        {
            try
            {
                _Initialize(model);
            }
            catch(ArgumentNullException ex)
            {
                throw new NotImplementedException($"{this.GetType().Name} does not implement the `{nameof(IDTO_Defines_Include<Model_Type>)}` interface; Implement that interface to avoid null references by returning the include query");
            }
            catch (NullReferenceException ex)
            {
                throw new NotImplementedException($"{this.GetType().Name} does not implement the `{nameof(IDTO_Defines_Include<Model_Type>)}` interface; Implement that interface to avoid null references by returning the include query");
            }
        }

        public static async Task<IEnumerable<DTO_Type>> Gets(IRepository<Model_Type> repo)
        {
            IEnumerable<Model_Type>  list = await repo.GetEntries();

            return list.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();
        }
        public static async Task<IEnumerable<DTO_Type>> Gets(IRepository<Model_Type> repo, Func<IQueryable<Model_Type>, IQueryable<Model_Type>>? WhereQuery)
        {
            IEnumerable<Model_Type> list = await repo.GetEntries(WhereQuery);

            return list.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();
        }

        public static async Task<IEnumerable<DTO_Type>> Gets<T>(IRepository<T> repo, Func<IQueryable<T>, IQueryable<T>>? WhereQuery = null, Func<IEnumerable<T>, IEnumerable<Model_Type>>? selectorfunc = null)
            where T : class, ICustomModel
        {
            IEnumerable<Model_Type> list;
            if (WhereQuery == null) list = selectorfunc!.Invoke(await repo.GetEntries()).ToList();
            else
            {
                list = selectorfunc.Invoke(await repo.GetEntries(WhereQuery)).ToList();
                
            }

            return list.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();
        }
        
        public static IEnumerable<DTO_Type> Gets<T>(IEnumerable<T> models, Func<IEnumerable<T>, IEnumerable<Model_Type>>? selectorfunc)
            where T : class, ICustomModel
        {
            IEnumerable<Model_Type> list = selectorfunc.Invoke(models).ToList();
    
            return list.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();
        }

        public static IEnumerable<DTO_Type> Gets(IEnumerable<Model_Type> models)
        {
            return models.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();
        }

        public static Payload<DTO_Type,Model_Type> toPayload(Model_Type model, string status = "success")
        {
            var a = new DTO_Type();
            a.Initialize(model);

            var p = new Payload<DTO_Type,Model_Type>();
            p.Data = a;
            p.Status = status;
            return p;
        }
        public static Payload<IEnumerable<DTO_Type>, Model_Type> toPayload(IEnumerable<Model_Type> models, string status = "success")
        {
            var list = models.Select(x => { var a = new DTO_Type(); a.Initialize(x); return a; }).ToList();

            var p = new Payload<IEnumerable<DTO_Type>,Model_Type>();
            p.Data = list;
            p.Status = status;
            return p;
        }
        public static Payload<IEnumerable<DTO_Type>, Model_Type> toPayload(IEnumerable<DTO_Type> modelsDtoList, string status = "success")
        {
            var p = new Payload<IEnumerable<DTO_Type>,Model_Type>();
            p.Data = modelsDtoList;
            p.Status = status;
            return p;
        }

        public static async Task<Payload<IEnumerable<DTO_Type>, Model_Type>> toPayload(IRepository<Model_Type> repo, string status = "success")
            
        {
            try
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                var include_interf = typeof(DTO_Type).GetInterface(typeof(IDTO_Defines_Include<Model_Type>).Name);
                if (include_interf != null) 
                {
                    var methodInfo = typeof(DTO_Type).GetMethod(
                            nameof(IDTO_Defines_Include<Model_Type>._includeData), 
                            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                    try
                    {
                        var queryArgs = (Func<IQueryable<Model_Type>, IQueryable<Model_Type>>)methodInfo.Invoke(null,null);
                        p.Data = await Gets(repo, queryArgs);
                    }
                    catch(ArgumentNullException ex)
                    {
                        throw new NotImplementedException($"{typeof(DTO_Type).Name} does not Include all members of {ex.TargetSite.DeclaringType.Name} in their `{nameof(IDTO_Defines_Include<Model_Type>)}` function; Ensure to include all required includes for {ex.TargetSite.DeclaringType.Name} dto class");
                    }
                    catch(NullReferenceException ex)
                    {
                        throw new NotImplementedException($"{typeof(DTO_Type).Name} does not Include all members of {ex.TargetSite.DeclaringType.Name} in their `{nameof(IDTO_Defines_Include<Model_Type>)}` function; Ensure to include all required includes for {ex.TargetSite.DeclaringType.Name} dto class");
                    }
                }
                else
                    p.Data = await Gets(repo);
                p.Status = status;
                return p;
            }
            catch (HttpRequestException ex)
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                p.Data = [];
                p.Status = "Failure";
                return p;
            }

        }

        public static async Task<Payload<IEnumerable<DTO_Type>, Model_Type>> toPayload<T>(IRepository<T> repo, Func<IQueryable<T>, IQueryable<T>> WhereQuery
            , Func<IEnumerable<T>, IEnumerable<Model_Type>>? selectorfunc
            , string status = "success"
            )
            where T : class, ICustomModel 
        {

            try
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                p.Data = await Gets<T>(repo, WhereQuery, selectorfunc);
                p.Status = status;
                return p;
            }
            catch (HttpRequestException ex)
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                p.Data = [];
                p.Status = "Failure";
                return p;
            }
        }

        public static async Task<Payload<IEnumerable<DTO_Type>, Model_Type>> toPayload(IRepository<Model_Type> repo, Func<IQueryable<Model_Type>, IQueryable<Model_Type>>? WhereQuery, string status = "success")
        {
            try
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                p.Data = await Gets(repo, WhereQuery);
                p.Status = status;
                return p;
            }
            catch (HttpRequestException ex)
            {
                var p = new Payload<IEnumerable<DTO_Type>, Model_Type>();
                p.Data = [];
                p.Status = "Failure";
                return p;
            }

        }

        
    }
}
