using System.Security;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using server_api.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace server_api.Data.Models.Repositories
{
    public static class Repository
    {
        // Выводит кортеж с ролью и списком пользователей
        public static async Task<Tuple<IdentityRole, IList<User>>> getRoleWithUsers(IdentityRole role, UserManager<User> userManager)
        {
            return Tuple.Create(role, await userManager.GetUsersInRoleAsync(role.Name));
        }
        // Выводит кортежи для каждой роли из списка
        public static IEnumerable<Tuple<IdentityRole, IList<User>>> getRolesWithUsersAsync(IEnumerable<IdentityRole> roles, UserManager<User> userManager)
        {
            return roles.Select(async role => await getRoleWithUsers(role, userManager)).Select(result => result.Result);
        }
        // Получить данные из загловка Authorization
        public static Tuple<User, string> GetAuthDataFromHeader(HttpContext httpContext)
        {
            var reguest = httpContext.Request;
            string auth = reguest.Headers["Authorization"];
            if (auth == null) return null;
            var headerValue = AuthenticationHeaderValue.Parse(auth);
            var schema = headerValue?.Scheme;
            if (!string.Equals(schema, "basic", StringComparison.OrdinalIgnoreCase)) return null;
            var parameters = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.Parameter)).Split(':');
            var userName = parameters[0];
            var password = parameters[1];
            if (string.IsNullOrWhiteSpace(userName)) return null;
            return Tuple.Create(new User { UserName = userName }, password);
        }

        //Добавить пользователя
        public static async Task<User> CreateUser(string username, string[] roles, string email, string password, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var ExistDefaultAccount = await userManager.FindByNameAsync(username);
            var existRoles = roleManager.Roles.Select(role => role.Name);
            var notExistRoles = roles.Where(role => !existRoles.Contains(role));
            foreach (var role in notExistRoles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new User { UserName = username, Email = email };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
            return user;
        }
        //Создать пользователя из настройки
        public static async Task CreateDefaultAccount(IServiceProvider service, IConfiguration configuration)
        {
            var userManager = service.GetRequiredService<UserManager<User>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            var username = configuration["DefaultAccount:Name"];
            var role = configuration["DefaultAccount:Role"];
            var email = configuration["DefaultAccount:Email"];
            var password = configuration["DefaultAccount:Password"];

            await CreateUser(username, new string[] { role }, email, password, userManager, roleManager);
        }
      
        //Добавить ошибки
        public static ModelStateDictionary AddErrors(this ModelStateDictionary model, string str, IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                model.AddModelError(str, error);
            }
            return model;
        }
        //Установить роли для пользователя
        public static async Task setUserRolesAsync(this UserManager<User> userManager, User user, IEnumerable<string> roles)
        {
            var roleInUser = (await userManager.GetRolesAsync(user)).ToHashSet<string>();
            var roleToSet = new HashSet<string>(roles);
            var roleToRemove = roleInUser.Except(roleToSet).ToHashSet<string>();
            var roleToAdd = roleToSet.Except(roleInUser).ToHashSet<string>();

            await userManager.RemoveFromRolesAsync(user, roleToRemove);
            await userManager.AddToRolesAsync(user, roleToAdd);
        }
        // Получить значение из словаря без ошибкок
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue result;
            return dic.TryGetValue(key, out result) ?
                result :
                default(TValue);
        }
        // Выводит хэш по массиву байт
        public static string getSHA1Hash(byte[] image)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(sha1.ComputeHash(image));
            }
        }
        // Сравнивает два массива байт
        public static bool ByteArrayCompare(byte[] p_BytesLeft, byte[] p_BytesRight)
        {
            if (p_BytesLeft.Length != p_BytesRight.Length)
                return false;

            var length = p_BytesLeft.Length;

            for (int i = 0; i < length; i++)
            {
                if (p_BytesLeft[i] != p_BytesRight[i])
                    return false;
            }

            return true;
        }
        // Возвращает имя файла из Uri
        public static string getFileName(Uri uri) => Path.GetFileName(uri.LocalPath);
        public static string getFileName(string path)
        {
            return isUrl(path)
                ? getFileName(new Uri(path).LocalPath)
                : Path.GetFileName(path);
        }
        public static string getPath(string path)
        {
            return isUrl(path)
                ? getFileName(new Uri(path).LocalPath)
                : Path.GetFileName(path);
        }


        public static bool isUrl(string path)
        {
            return Uri.IsWellFormedUriString(path, UriKind.Absolute);
        }
        public static IQueryable<T> Order<T>(this IQueryable<T> source, string propertyName, string descending, bool anotherLevel = false)
        {
            var param = Expression.Parameter(typeof(T), string.Empty);
            var property = Expression.PropertyOrField(param, propertyName);
            var sort = Expression.Lambda(property, param);

            var call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") +
                (descending == "Descending" ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));

            return source.Provider.CreateQuery<T>(call);

        }
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return !string.IsNullOrEmpty(request.Headers["X-Requested-With"]) &&
                    string.Equals(
                        request.Headers["X-Requested-With"],
                        "XmlHttpRequest",
                        StringComparison.OrdinalIgnoreCase);

            return false;
        }

        public static async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password, UserManager<User> userManager)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                var roles = (await userManager.GetRolesAsync(userToVerify)).ToArray();
                return await Task.FromResult(JwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, roles));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        public static Dictionary<int, string> GetDifferenceUri(Dictionary<int, Uri> uris)
        {
            //Составляем словарь мест сегментов в uri начиная с хоста:
            var dictionary = new Dictionary<int, HashSet<string>>();
            foreach (var uri in uris.Values)
            {
                var parts = new string[] { uri.Host }.Concat(uri.Segments.Take(uri.Segments.Length - 1)).ToArray();
                for (int j = 0; j < parts.Length; j++)
                {
                    if (!dictionary.ContainsKey(j)) dictionary.Add(j, new HashSet<string>());
                    dictionary[j].Add(parts[j]);
                }
            }

            //Если на этом месте только один сегмент - его можно не показывать - удаляем(если на одном и том же сегменте во всех файлах только одно значение то его не показываем)
            //Предпоследний и последний сегмент показывется всегда
            //Пример есть пути:
            // host:\\1\2\3\4\5\f.txt =>...3\..5\f.txt
            // host:\\1\2\7\4\5\f.txt =>...7\...5\f.txt
            
            var showedSegmentUri = dictionary.Where(set => set.Value.Count > 1).Select(set => set.Key).ToHashSet();

            var result = new Dictionary<int, string>();
            var stringBuilder = new StringBuilder();
            var showPreviousSegment = true; ;

            foreach (var item in uris)
            {
                var uri = item.Value;
                var segments = uri.Segments;
                var segmentsWithHost = new string[] { uri.Host }.Concat(segments).ToArray();
                stringBuilder.Clear();
                for (int i = 0; i < segmentsWithHost.Length; i++)
                {
                    if (showedSegmentUri.Contains(i) || i == segmentsWithHost.Length - 1)
                    {
                        stringBuilder.Append(segmentsWithHost[i]);
                        showPreviousSegment = true;
                    }
                    else
                    {
                        if (showPreviousSegment)
                        {
                            stringBuilder.Append("...");
                            showPreviousSegment = false;
                        };
                    }
                }
                result.Add(item.Key, stringBuilder.ToString());
            }

            return result;
        }
    }
}
